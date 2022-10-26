using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestPermitApplicationToWorkInformationMapper : IMapper<PermitApplicationRequestModel, WorkInformation>
    {
        public WorkInformation Map(PermitApplicationRequestModel source)
        {
            return new WorkInformation
            {
                EmployerAddressLine1 = source.Application.WorkInformation.EmployerAddressLine1,
                EmployerAddressLine2 = source.Application.WorkInformation.EmployerAddressLine2,
                EmployerCity = source.Application.WorkInformation.EmployerCity,
                EmployerCountry = source.Application.WorkInformation.EmployerCountry,
                EmployerPhone = source.Application.WorkInformation.EmployerPhone,
                EmployerName = source.Application.WorkInformation.EmployerName,
                EmployerState = source.Application.WorkInformation.EmployerState,
                EmployerZip = source.Application.WorkInformation.EmployerZip,
            };
        }
    }
}
