using Microsoft.AspNetCore.Http;

namespace Application;

public interface ICdnProvider
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string key);
    Task<List<string>> UploadFilesAsync(List<IFormFile> files, int maxDegreeOfParallelism = 5);
}
