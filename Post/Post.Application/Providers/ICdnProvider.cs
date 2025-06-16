using Microsoft.AspNetCore.Http;

namespace Application;

public interface ICdnProvider
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<List<string>> UploadFilesAsync(List<IFormFile> files, int maxDegreeOfParallelism = 5);
    Task<bool> DeleteFileAsync(string url);
    Task<bool> DeleteFilesAsync(List<string> urls);
}
