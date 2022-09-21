using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Dominio.Compartilhado
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0].Name;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }
}
