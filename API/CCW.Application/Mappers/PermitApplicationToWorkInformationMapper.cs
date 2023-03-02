using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToWorkInformationMapper : IMapper<PermitApplication, WorkInformation>
{
    public WorkInformation Map(PermitApplication source)
    {
        return source.Application.WorkInformation == null ? new WorkInformation() :
            new WorkInformation
            {
                EmployerAddressLine1 = source.Application.WorkInformation.EmployerAddressLine1,
                EmployerAddressLine2 = source.Application.WorkInformation.EmployerAddressLine2,
                EmployerCity = source.Application.WorkInformation.EmployerCity,
                EmployerCountry = source.Application.WorkInformation.EmployerCountry,
                EmployerPhone = source.Application.WorkInformation.EmployerPhone,
                EmployerName = source.Application.WorkInformation.EmployerName,
                EmployerState = source.Application.WorkInformation.EmployerState,
                EmployerZip = source.Application.WorkInformation.EmployerZip,
                Occupation = source.Application.WorkInformation.Occupation,
            };
    }
}
