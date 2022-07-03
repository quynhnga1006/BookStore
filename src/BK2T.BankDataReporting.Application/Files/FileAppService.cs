using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Guids;

namespace BK2T.BankDataReporting.Files
{
    public class FileAppService : ApplicationService
    {
        private readonly IBlobContainer _blobContainer;
        private readonly IGuidGenerator _guidGenerator;

        public FileAppService(IBlobContainer blobContainer, IGuidGenerator guidGenerator)
        {
            _blobContainer = blobContainer;
            _guidGenerator = guidGenerator;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            var fileName = $"{_guidGenerator.Create()}-{file.FileName}"; 
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                await _blobContainer.SaveAsync(fileName, memoryStream);
            }
            return fileName;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAsync(string fileName)
        {
            var bytes = await _blobContainer.GetAllBytesOrNullAsync(fileName);
            if (bytes == null)
            {
                return null;
            }
            var contentType = "application/octect-stream";
            return new FileContentResult(bytes, contentType)
            {
                FileDownloadName = fileName
            };
        }

        public async Task<byte[]> GetFileAsync(string fileName)
        {
            return await _blobContainer.GetAllBytesOrNullAsync(fileName);
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            return _blobContainer.DeleteAsync(fileName);
        }
    }
}