using System;

namespace Feedgre.Services.Parsing
{
    public static class Utilities
    {
        public static DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }
    }
}
