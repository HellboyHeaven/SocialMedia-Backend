using Microsoft.AspNetCore.Http;

namespace Application;

public interface ICdnProvider
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<IEnumerable<string>> UploadFilesAsync(IEnumerable<IFormFile> files, int maxDegreeOfParallelism = 5);
    Task<bool> DeleteFileAsync(string url);
    Task<bool> DeleteFilesAsync(IEnumerable<string> urls);
}
