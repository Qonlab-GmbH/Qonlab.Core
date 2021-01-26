using System;
using System.Collections.Generic;
using System.Text;

namespace Qonlab.Core {
    public static class DateTimeExtensions {
        public static DateTime Trim( this DateTime date, long ticks ) {
            return new DateTime( date.Ticks - ( date.Ticks % ticks ), date.Kind );
        }

        public static DateTime TrimToSeconds( this DateTime date ) {
            return date.Trim( TimeSpan.TicksPerSecond );
        }

        public static DateTime TrimToMilliseconds( this DateTime date ) {
            return date.Trim( TimeSpan.TicksPerMillisecond );
        }
    }
}
