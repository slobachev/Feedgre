using System;

namespace Feedgre.Services.Parsing
{
    /// <summary>
    /// Parsing utilities
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Method to parse dates
        /// </summary>
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
