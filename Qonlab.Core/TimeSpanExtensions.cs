using System;
using System.Collections.Generic;
using System.Text;

namespace Qonlab.Core {
    public static class TimeSpanExtensions {
        public static string ToReadableTime( this TimeSpan timeSpan ) {
            return ( timeSpan.Days > 0 ? timeSpan.Days + "d " : "" ) +
                   ( timeSpan.Hours > 0 ? timeSpan.Hours + "h " : "" ) +
                   ( timeSpan.Minutes > 0 ? timeSpan.Minutes + "m " : "" ) +
                   ( timeSpan.Seconds > 0 ? timeSpan.Seconds + "s " : "" );
        }
    }
}
