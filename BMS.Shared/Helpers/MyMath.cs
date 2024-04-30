using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Shared.Helpers
{
    public static class MyMath
    {
        public static double RadianToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}
