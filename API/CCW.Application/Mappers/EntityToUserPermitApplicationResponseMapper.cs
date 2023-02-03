using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToUserPermitApplicationResponseMapper : IMapper<PermitApplication, UserPermitApplicationResponseModel>
{
    private readonly IMapper<PermitApplication, UserApplication> _applicationMapper;
    private readonly IMapper<PermitApplication, PaymentHistory[]> _paymentHistoryMapper;

    public EntityToUserPermitApplicationResponseMapper(
        IMapper<PermitApplication, UserApplication> applicationMapper,
        IMapper<PermitApplication, PaymentHistory[]> paymentHistoryMapper)
    {
        _applicationMapper = applicationMapper;
        _paymentHistoryMapper = paymentHistoryMapper;
    }

    public UserPermitApplicationResponseModel Map(PermitApplication source)
    {
        return new UserPermitApplicationResponseModel
        {
            Application = _applicationMapper.Map(source),
            Id = source.Id,
            UserId = source.UserId,
            PaymentHistory = _paymentHistoryMapper.Map(source),
        };
    }
}
