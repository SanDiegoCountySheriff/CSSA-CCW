using Azure.Storage.Blobs;

namespace CCW.Common.Services.Contracts;

public interface IStorageContainerResolver
{
    BlobContainerClient GetBlobContainer(string tenantId, string containerName);
}
