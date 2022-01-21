using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Repositories.IDE;
using JitEvolution.Core.Services.IDE;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO.Compression;

namespace JitEvolution.Services.IDE
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly CurrentUser _currentUser;

        public ProjectService(IProjectRepository projectRepository, CurrentUser currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task CreateAsync(string projectId, IFormFile projectZipFile)
        {
            using var extractedPath = new UseTemporaryPath();
            using var zipPath = new UseTemporaryPath();
            using (var fileStream = new FileStream(zipPath.TemporaryPath, FileMode.Create))
            {
                await projectZipFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            ZipFile.ExtractToDirectory(zipPath.TemporaryPath, extractedPath.TemporaryPath, true);

            // This is probably dangerous. Need to sanitize arguments
            var processinfo = new ProcessStartInfo("docker", $"run --rm -it -v {extractedPath.TemporaryPath}:/source t analyse /source --language java --app-key \"{projectId}\"")
            {
                UseShellExecute = false,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true
            };

            using var process = Process.Start(processinfo);
            if (process != null)
            {
                await process.WaitForExitAsync();
                var t = process.ExitCode;
            }
            else
            {
                throw new Exception("Failed to start analyzer");
            }

            if (process.ExitCode == 0)
            {
                await _projectRepository.AddAsync(new Core.Models.IDE.Project
                {
                    UserId = _currentUser.Id,
                    ProjectId = projectId
                });

                await _projectRepository.SaveChangesAsync();
            }

        }

        private class UseTemporaryPath : IDisposable
        {
            public readonly string TemporaryPath;

            public UseTemporaryPath()
            {
                TemporaryPath = $"{Path.GetTempPath()}/{Path.GetRandomFileName()}";
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
