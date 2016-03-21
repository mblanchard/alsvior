using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.Utility
{
    public static class FixedPointCoordConversion
    {
        /*TODO: As of 3/21, double <=> fixed-point int representations w/ base10 preserves human-readable coords. Good for dev, bad for performance
        * Improvements:
        * - Bitshifting over base10 for perf if needed
        * - Pull in a fixed-point math library if needed.
        */
        private const int FP_PRECISION = 6; //places
        public static int ToInt (double coord )
        {
            return (int) (coord * Math.Pow(10, FP_PRECISION));
        }
        public static double ToDouble (int fpCoord)
        {
            return ((double)fpCoord)/Math.Pow(10, FP_PRECISION);
        }

        public static float ToFloat(int fpCoord)
        {
            return (float)(fpCoord / Math.Pow(10, FP_PRECISION));
        }
    }
}
