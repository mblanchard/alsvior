using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoTimeZone;
using NodaTime;
using System.Threading.Tasks;

namespace Alsvior.Utility
{
    public static class TimeConversion
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const int SECONDS_IN_DAY = 86400;

        public static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        public static long StripMinutes(long timestamp)
        {
            return timestamp - (timestamp % 3600);
        }

        public static long StripHours(long timestamp)
        {
            return timestamp - (timestamp % (SECONDS_IN_DAY) ); //seconds in day
        }

        public static long AddDays(long timestamp, int days)
        {
            return timestamp + (SECONDS_IN_DAY * days);
        }

        public static long AddHours(long timestamp, int hours) {
            return timestamp + (3600 * hours);
        }

        //Or "when is 12:00 AM at this location, relative to UTC?"
        public static Tuple<long,long> GetDailyTimestampRange(long timestamp, int longitude)
        {
            var day = StripHours(timestamp);
            var isWesternHemisphere = longitude <= 0;
            var tzOffsetMin = isWesternHemisphere ? -2 : -14;
            var tzOffsetMax = isWesternHemisphere ? 14 : 2;
            return new Tuple<long, long>(AddHours(day, tzOffsetMin), AddHours(day, tzOffsetMax));
        }
    

        public static int GetTimezoneOffsetFromLocationAndDateInSeconds(long timestamp, int latitude, int longitude)
        {
            var timezone = TimeZoneLookup.GetTimeZone(FixedPointCoordConversion.ToDouble(latitude), FixedPointCoordConversion.ToDouble(longitude));
            Instant instant = new Instant(timestamp * 1000);
            ZonedDateTime zdt = instant.InZone(DateTimeZoneProviders.Tzdb[timezone.Result]);
            return zdt.Offset.Milliseconds / 1000;
        }

        public static DateTime ConvertToDateTime(long seconds)
        {
            return Epoch.AddSeconds(seconds);
        }
    }
}
