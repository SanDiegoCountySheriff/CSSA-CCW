using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class UserRequestPermitApplicationToAliasMapper : IMapper<UserPermitApplicationRequestModel, Alias[]>
    {
        public Alias[] Map(UserPermitApplicationRequestModel source)
        {
            if (source.Application.Aliases != null)
            {
                int count = source.Application.Aliases.Length;
                var newItem = new Alias[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapAlias(source.Application.Aliases[i], new Alias());
                }

                return newItem;
            }

            return new Alias[0];
    }

        private static Alias MapAlias(Alias uiAlias, Alias dbAlias)
        {
            dbAlias.PrevLastName = uiAlias.PrevLastName;
            dbAlias.PrevFirstName = uiAlias.PrevFirstName;
            dbAlias.PrevMiddleName = uiAlias.PrevMiddleName;
            dbAlias.CityWhereChanged = uiAlias.CityWhereChanged;
            dbAlias.StateWhereChanged = uiAlias.StateWhereChanged;
            dbAlias.CourtFileNumber = uiAlias.CourtFileNumber;

            return dbAlias;
        }
    }
