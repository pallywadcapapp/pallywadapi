using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Application
{
    public static class AppCurrFormatter
    {
        public static string GetFormattedCurrency(double value, int decimalPlaces, string culture)
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            return value.ToString($"C{decimalPlaces}", cultureInfo);
        }
    }
}
