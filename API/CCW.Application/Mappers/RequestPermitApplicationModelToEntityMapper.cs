using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationModelToEntityMapper : IMapper<bool, PermitApplicationRequestModel, PermitApplication>
{
    private static Random random = new Random();
    private readonly IMapper<PermitApplicationRequestModel, Entities.Application> _applicationMapper;
    private readonly IMapper<PermitApplicationRequestModel, History[]> _historyMapper;
    private readonly IMapper<PermitApplicationRequestModel, PaymentHistory[]> _paymentHistoryMapper;

    public RequestPermitApplicationModelToEntityMapper(
        IMapper<PermitApplicationRequestModel, Entities.Application> applicationMapper,
        IMapper<PermitApplicationRequestModel, History[]> historyMapper,
        IMapper<PermitApplicationRequestModel, PaymentHistory[]> paymentHistoryMapper)
    {
        _applicationMapper = applicationMapper;
        _historyMapper = historyMapper;
        _paymentHistoryMapper = paymentHistoryMapper;
    }

    public PermitApplication Map(bool isNewApplication, PermitApplicationRequestModel source)
    {
        if (isNewApplication)
        {
            source.Application.OrderId = GetPrefixLetter() + GetGeneratedTime() + RandomString();
        }

        return new PermitApplication
        {
            Application = _applicationMapper.Map(source),
            Id = source.Id,
            History = _historyMapper.Map(source),
            UserId = source.UserId,
            PaymentHistory = _paymentHistoryMapper.Map(source),
        };
    }

    private string GetGeneratedTime()
    {
        var result = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                     + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm");

        return result;
    }

    private static char GetPrefixLetter()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int num = random.Next(0, chars.Length);
        return chars[num];
    }

    private string RandomString()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[3];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new String(stringChars);
    }
}