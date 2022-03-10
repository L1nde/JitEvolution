using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Config;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Services.IDE;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IO.Compression;

namespace JitEvolution.Services.IDE
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly CurrentUser _currentUser;
        private readonly Configuration _config;

        public ProjectService(IProjectRepository projectRepository, CurrentUser currentUser, IOptions<Configuration> config)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
            _config = config.Value;
        }

        public async Task CreateOrUpdateAsync(string projectId, IFormFile projectZipFile)
        {
            using var extractedPath = new UseTemporaryFile(_config.GraphifyEvolution.SourcesDirectoryPath);
            using var zipPath = new UseTemporaryFile();
            using (var fileStream = new FileStream(zipPath.TemporaryPath, FileMode.Create))
            {
                await projectZipFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            ZipFile.ExtractToDirectory(zipPath.TemporaryPath, extractedPath.TemporaryPath, true);

            // This is probably dangerous. Need to sanitize arguments
            var processinfo = new ProcessStartInfo("docker", $"run --rm -v {_config.GraphifyEvolution.DockerVolumeName}:/source {_config.GraphifyEvolution.DockerImageName} analyse /source/{extractedPath.FileName} --language java --app-key \"{projectId}\" --neo4j-host {_config.GraphifyEvolution.ApiUrl} --neo4j-port {_config.GraphifyEvolution.Port} --neo4j-username neo4j --neo4j-password 1234")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process();
            process.StartInfo = processinfo;
            process.OutputDataReceived += OutputReceived;
            process.ErrorDataReceived += OutputReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception("Failed to start analyzer");
            }

            if (process.ExitCode == 0 && !_projectRepository.Queryable.Any(x => x.ProjectId == projectId))
            {
                await _projectRepository.AddAsync(new Core.Models.IDE.Project
                {
                    UserId = _currentUser.Id,
                    ProjectId = projectId
                });

                await _projectRepository.SaveChangesAsync();
            }

        }

        private void OutputReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
            Console.WriteLine(e.Data);
        }

        private class UseTemporaryFile : IDisposable
        {
            public readonly string TemporaryPath;
            public readonly string FileName;

            public UseTemporaryFile() :this(Path.GetTempPath())
            {
            }

            public UseTemporaryFile(string folderPath)
            {
                FileName = Path.GetRandomFileName();
                TemporaryPath = $"{folderPath}/{FileName}";
            }

            public void Dispose()
            {
                if (File.Exists(TemporaryPath))
                {
                    File.Delete(TemporaryPath);
                }
                else if (Directory.Exists(TemporaryPath))
                {
                    Directory.Delete(TemporaryPath, true);
                }
            }
        }
    }
}
