using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Qonlab.Core {
    public static class ColorExtensions {
        public static string ToRgbString( this Color? color ) {
            return color.HasValue ? $"{color.Value.R},{color.Value.G},{color.Value.B}" : null;
        }

        public static Color GetForegroundColor( this Color? backgroundColor, int threshold = 80 ) {
            return backgroundColor.HasValue && backgroundColor.Value.GetColorBrightness() < threshold
                ? Color.White
                : Color.Black;
        }

        public static int GetColorBrightness( this Color color ) {
            return ( int ) Math.Sqrt(
                color.R * color.R * .299 +
                color.G * color.G * .587 +
                color.B * color.B * .114 );
        }
    }
}
