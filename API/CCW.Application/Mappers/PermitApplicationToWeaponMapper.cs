using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers
{
    public class PermitApplicationToWeaponMapper : IMapper<PermitApplication, Weapon[]>
    {
        public Weapon[] Map(PermitApplication source)
        {
            if (source.Application.Weapons != null)
            {
                int count = source.Application.Weapons.Length;
                var newItem = new Weapon[count];
                for (int i = 0; i < count; i++)
                {
                    newItem[i] = MapWeapon(source.Application.Weapons[i], new Weapon());
                }

                return newItem;
            }

            return new Weapon[0];
        }

        private static Weapon MapWeapon(Weapon uiWeapon, Weapon dbWeapon)
        {
            dbWeapon.Make = uiWeapon.Make;
            dbWeapon.Model = uiWeapon.Model;
            dbWeapon.Caliber = uiWeapon.Caliber;
            dbWeapon.SerialNumber = uiWeapon.SerialNumber;

            return dbWeapon;
        }
    }
}
