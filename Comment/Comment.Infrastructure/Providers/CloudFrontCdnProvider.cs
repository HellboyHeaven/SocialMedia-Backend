using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class CloudFrontCdnProvider(IConfiguration configuration, IAmazonS3 s3Client) : ICdnProvider
{
    private readonly string _bucketName = configuration["AWS:BucketName"];
    private readonly string _cloudFront = configuration["AWS:CloudFront"];

    // реализация для Amazon CloudFront
    public async Task<bool> DeleteFileAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;
        var uri = new Uri(url);
        string key = uri.AbsolutePath.TrimStart('/');
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await s3Client.DeleteObjectAsync(deleteRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    public async Task<bool> DeleteFilesAsync(IEnumerable<string> urls)
    {
        if (urls == null || !urls.Any())
            return false;

        var objectsToDelete = urls.Select(url =>
        {
            var uri = new Uri(url);
            string key = uri.AbsolutePath.TrimStart('/');
            return new KeyVersion { Key = key };
        }).ToList();

        var deleteRequest = new DeleteObjectsRequest
        {
            BucketName = _bucketName,
            Objects = objectsToDelete
        };

        var response = await s3Client.DeleteObjectsAsync(deleteRequest);

        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }


    public async Task<string> UploadFileAsync(IFormFile file)
    {
        return await UploadFileAsync(file, file.ContentType.Split('/')[0], file.ContentType.Split('/')[1]);
    }

    public async Task<IEnumerable<string>> UploadFilesAsync(IEnumerable<IFormFile> files, int maxDegreeOfParallelism = 5)
    {
        var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
        var uploadTasks = files.Select(async file =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await UploadFileAsync(file);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToList();
        var result = await Task.WhenAll(uploadTasks);
        return result.ToList();
    }

    private async Task<string> UploadFileAsync(IFormFile file, string path, string extension)
    {
        var fileTransferUtility = new TransferUtility(s3Client);
        string key = $"{path}/{Guid.NewGuid().ToString()}.{extension}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = file.OpenReadStream(),
            Key = key,
            BucketName = _bucketName,
        };
        await fileTransferUtility.UploadAsync(uploadRequest);
        return $"https://{_cloudFront}/{key}";
    }
}
