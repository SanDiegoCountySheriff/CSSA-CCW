using Azure.Storage.Blobs;

namespace CCW.Document.Services.Contracts;

public interface IStorageContainerResolver
{
    BlobContainerClient GetBlobContainer(string tenantId, string containerName);
}
