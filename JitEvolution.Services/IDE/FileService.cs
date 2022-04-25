using JitEvolution.Config;
using JitEvolution.Core.Repositories.IDE;
using Microsoft.AspNetCore.Http;

namespace JitEvolution.Services.IDE
{
    public class FileService : IFileService
    {
        public async Task<string> SaveAsync(IFormFile formFile)
        {
            var filePath = GetTemporaryFilePath();

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return filePath;
        }

        private static string GetTemporaryFilePath() =>
            $"{Path.GetTempPath()}/{Path.GetRandomFileName()}";

    }
}
