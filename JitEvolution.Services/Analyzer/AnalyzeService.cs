using JitEvolution.Config;
using JitEvolution.Core.Services.Analyzer;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IO.Compression;

namespace JitEvolution.Services.Analyzer
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly Configuration _config;

        public AnalyzeService(IOptions<Configuration> config)
        {
            _config = config.Value;
        }

        public async Task AnalyzeAsync(string projectId, string projectZipFilePath)
        {
            using var extractedPath = new UseTemporaryFile(_config.GraphifyEvolution.SourcesDirectoryPath);
            ZipFile.ExtractToDirectory(projectZipFilePath, extractedPath.TemporaryPath, true);

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

            public UseTemporaryFile() : this(Path.GetTempPath())
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
