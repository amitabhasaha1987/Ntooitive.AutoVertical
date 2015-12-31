using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int? Age(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var now = float.Parse(DateTime.UtcNow.ToString("yyyy.MMdd"));
            var dob = float.Parse(date.Value.ToString("yyyy.MMdd"));

            return (int)(now - dob);
        }

        public static string AgeInYearsMonthsDays(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var years = new DateTime(DateTime.UtcNow.Subtract(date.Value).Ticks).Year - 1;
            var pastYearDate = date.Value.AddYears(years);

            var months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (pastYearDate.AddMonths(i) == now)
                {
                    months = i;
                    break;
                }
                else if (pastYearDate.AddMonths(i) >= now)
                {
                    months = i - 1;
                    break;
                }
            }

            var days = now.Subtract(pastYearDate.AddMonths(months)).Days;

            return String.Format("{0}Y{1}M{2}D", years, months, days);
        }

        public static int? YearsBeforeDying(this DateTime? date, double lifeExpectancy)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var ageString = date.AgeInYearsMonthsDays().Split(new char[] { 'Y', 'M', 'D' });

            var age = double.Parse(ageString[0]) + (double.Parse(ageString[1]) / 12.0) + (double.Parse(ageString[2]) / 365.0);
            var diff = lifeExpectancy - age;

            return (int)diff;
        }

        public static string DaysBeforeDying(this DateTime? date, double lifeExpectancy)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var ageString = date.AgeInYearsMonthsDays().Split(new char[] { 'Y', 'M', 'D' });

            var age = double.Parse(ageString[0]) + (double.Parse(ageString[1]) / 12.0) + (double.Parse(ageString[2]) / 365.0);
            var diff = lifeExpectancy - age;

            var years = (int)diff;
            var months = (int)((diff - years) * 12);
            var days = (int)((diff - years - (months / 12.0)) * 365.0);

            return String.Format("{0}Y{1}M{2}D", years, months, days);
        }

        public static long ToUnixTime(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return 0;
            }

            return date.Value.ToUnixTime();
        }

        public static long ToUnixTime(this DateTime date)
        {
            if (date <= UnixEpoch)
            {
                return 0;
            }

            return (long)(date - UnixEpoch).TotalSeconds;
        }

        public static long ToUnixTimeMilliseconds(this DateTime? date)
        {
            return date.ToUnixTime() * 1000;
        }

        public static long ToUnixTimeMilliseconds(this DateTime date)
        {
            return date.ToUnixTime() * 1000;
        }

        public static long ToUTCUnixTimeMilliseconds(this DateTime date)
        {
            var dateTime = DateTime.SpecifyKind(date, DateTimeKind.Utc);            
            return dateTime.ToUnixTime() * 1000;
        }

        public static DateTime ToDateTimeFromMilliseconds(this double unixTime)
        {
            return UnixEpoch.AddMilliseconds(unixTime);
        }

        public static string ToHoursFromMilliseconds(this double milliseconds)
        {
            if (milliseconds > 0)
            {
                double hours = (milliseconds / (1000D * 60D * 60D));
                if (hours <= 9.99)
                {
                    return string.Format("{0:0.00}", Math.Truncate(hours * 100) / 100);
                }
                else if (hours <= 99.9)
                {
                    return string.Format("{0:0.0}", Math.Truncate(hours * 10) / 10);
                }
                else
                {
                    return string.Format("{0:0}", hours);
                }
            }
            else
            {
                return "0.00";
            }
        }

        public static DateTime ToDateTimeFromMilliseconds(this long unixTime)
        {
            return UnixEpoch.AddMilliseconds(unixTime);
        }

        public static DateTime ToUTCDateTimeFromMilliseconds(this long unixTime)
        {
            var dateTime = DateTime.SpecifyKind(UnixEpoch.AddMilliseconds(unixTime), DateTimeKind.Utc);
            return dateTime;
        }

        public static DateTime ToDateTimeFromseconds(this long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
    }
}
