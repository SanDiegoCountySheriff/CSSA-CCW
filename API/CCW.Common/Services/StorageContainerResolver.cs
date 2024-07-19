using Azure.Storage.Blobs;
using CCW.Common.Services.Contracts;

namespace CCW.Common.Services;

public class StorageContainerResolver : IStorageContainerResolver
{
    private readonly Dictionary<string, BlobContainerClient> _blobContainers;

    public StorageContainerResolver(Dictionary<string, BlobContainerClient> blobContainers)
    {
        _blobContainers = blobContainers;
    }

    public BlobContainerClient GetBlobContainer(string tenantId, string containerName)
    {
        return _blobContainers[$"{tenantId}-{containerName}"];
    }
}
