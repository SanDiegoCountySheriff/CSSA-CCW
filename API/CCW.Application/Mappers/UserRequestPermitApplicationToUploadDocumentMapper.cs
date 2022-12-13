using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToUploadDocumentMapper : IMapper<UserPermitApplicationRequestModel, UploadedDocument[]>
{
    public UploadedDocument[] Map(UserPermitApplicationRequestModel source)
    {
        if (source.Application.UploadedDocuments != null)
        {
            int count = source.Application.UploadedDocuments.Length;
            var newItem = new UploadedDocument[count];
            for (int i = 0; i < count; i++)
            {
                newItem[i] = MapAlias(source.Application.UploadedDocuments[i], new UploadedDocument());
            }

            return newItem;
        }

        return Array.Empty<UploadedDocument>();
    }

    private static UploadedDocument MapAlias(UploadedDocument uiUploadedDocument, UploadedDocument dbUploadedDocument)
    {
        dbUploadedDocument.DocumentType = uiUploadedDocument.DocumentType;
        dbUploadedDocument.Name = uiUploadedDocument.Name;
        dbUploadedDocument.UploadedBy = uiUploadedDocument.UploadedBy;
        dbUploadedDocument.UploadedDateTimeUtc = uiUploadedDocument.UploadedDateTimeUtc;

        return dbUploadedDocument;
    }
}