using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class EntityToPermitApplicationResponseMapper : IMapper<PermitApplication, PermitApplicationResponseModel>
{
    private readonly IMapper<PermitApplication, Entities.Application> _applicationMapper;
    private readonly IMapper<PermitApplication, History[]> _historyMapper;
    private readonly IMapper<PermitApplication, PaymentHistory[]> _paymentHistoryMapper;

    public EntityToPermitApplicationResponseMapper(
        IMapper<PermitApplication, Entities.Application> applicationMapper,
        IMapper<PermitApplication, History[]> historyMapper,
        IMapper<PermitApplication, PaymentHistory[]> paymentHistoryMapper)
    {
        _applicationMapper = applicationMapper;
        _historyMapper = historyMapper;
        _paymentHistoryMapper = paymentHistoryMapper;
    }
    public PermitApplicationResponseModel Map(PermitApplication source)
    {
        return new PermitApplicationResponseModel
        {
            Application = _applicationMapper.Map(source),
            Id = source.Id,
            History = _historyMapper.Map(source),
            UserId = source.UserId,
            PaymentHistory = _paymentHistoryMapper.Map(source),
        };
    }
}