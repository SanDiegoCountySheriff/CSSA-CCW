using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class RequestUserPermitApplicationModelToEntityMapper : IMapper<bool, string, UserPermitApplicationRequestModel, PermitApplication>
    {
        private static Random random = new Random();
        private readonly IMapper<string, UserPermitApplicationRequestModel, Entities.Application> _applicationMapper;

        public RequestUserPermitApplicationModelToEntityMapper(
            IMapper<string, UserPermitApplicationRequestModel, Entities.Application> applicationMapper)
        {
            _applicationMapper = applicationMapper;
        }

        public PermitApplication Map(bool isNewApplication, string comments, UserPermitApplicationRequestModel source)
        {
            History[] history = Array.Empty<History>();
            PaymentHistory[] paymentHistory = Array.Empty<PaymentHistory>();

            if (isNewApplication)
            {
                source.Application.OrderId = GetPrefixLetter() + GetGeneratedTime() + RandomString();

                history = new[]{
                    new History
                    {
                        ChangeMadeBy = source.Application.UserEmail,
                        Change = "created application",
                        ChangeDateTimeUtc = DateTime.UtcNow,
                    }
                };
            }

            return new PermitApplication
            {
                Id = source.Id,
                UserId = source.UserId,
                History = history, //recorded for public only on create
                PaymentHistory = paymentHistory,
                Application = _applicationMapper.Map(comments, source),
            };
        }


        private string GetGeneratedTime()
        {
            var result = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                         + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

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
}
