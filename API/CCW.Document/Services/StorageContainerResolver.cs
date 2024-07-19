using Azure.Storage.Blobs;
using CCW.Document.Services.Contracts;

namespace CCW.Document.Services;

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
