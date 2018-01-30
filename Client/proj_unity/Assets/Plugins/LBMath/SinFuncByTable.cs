using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBMath
{
    public class SinFuncByTable
    {
        protected static IntegerFloat[] valueTable = new IntegerFloat[] {
            new IntegerFloat(0, 10000),     new IntegerFloat(87, 5000),     new IntegerFloat(87, 2500),     new IntegerFloat(523, 10000),       new IntegerFloat(697, 10000),
            new IntegerFloat(871, 10000),       new IntegerFloat(209, 2000),        new IntegerFloat(609, 5000),        new IntegerFloat(1391, 10000),      new IntegerFloat(391, 2500),
            new IntegerFloat(217, 1250),        new IntegerFloat(477, 2500),        new IntegerFloat(2079, 10000),      new IntegerFloat(2249, 10000),      new IntegerFloat(2419, 10000),
            new IntegerFloat(647, 2500),        new IntegerFloat(689, 2500),        new IntegerFloat(2923, 10000),      new IntegerFloat(309, 1000),        new IntegerFloat(651, 2000),
            new IntegerFloat(171, 500),     new IntegerFloat(3583, 10000),      new IntegerFloat(1873, 5000),       new IntegerFloat(3907, 10000),      new IntegerFloat(4067, 10000),
            new IntegerFloat(2113, 5000),       new IntegerFloat(4383, 10000),      new IntegerFloat(4539, 10000),      new IntegerFloat(2347, 5000),       new IntegerFloat(303, 625),
            new IntegerFloat(1, 2),     new IntegerFloat(103, 200),     new IntegerFloat(5299, 10000),      new IntegerFloat(2723, 5000),       new IntegerFloat(5591, 10000),
            new IntegerFloat(1147, 2000),       new IntegerFloat(5877, 10000),      new IntegerFloat(3009, 5000),       new IntegerFloat(1539, 2500),       new IntegerFloat(6293, 10000),
            new IntegerFloat(6427, 10000),      new IntegerFloat(82, 125),      new IntegerFloat(6691, 10000),      new IntegerFloat(6819, 10000),      new IntegerFloat(3473, 5000),
            new IntegerFloat(7071, 10000),      new IntegerFloat(7193, 10000),      new IntegerFloat(7313, 10000),      new IntegerFloat(7431, 10000),      new IntegerFloat(7547, 10000),
            new IntegerFloat(383, 500),     new IntegerFloat(7771, 10000),      new IntegerFloat(197, 250),     new IntegerFloat(3993, 5000),       new IntegerFloat(809, 1000),
            new IntegerFloat(8191, 10000),      new IntegerFloat(829, 1000),        new IntegerFloat(4193, 5000),       new IntegerFloat(106, 125),     new IntegerFloat(8571, 10000),
            new IntegerFloat(433, 500),     new IntegerFloat(4373, 5000),       new IntegerFloat(8829, 10000),      new IntegerFloat(891, 1000),        new IntegerFloat(8987, 10000),
            new IntegerFloat(9063, 10000),      new IntegerFloat(1827, 2000),       new IntegerFloat(1841, 2000),       new IntegerFloat(9271, 10000),      new IntegerFloat(1867, 2000),
            new IntegerFloat(2349, 2500),       new IntegerFloat(1891, 2000),       new IntegerFloat(951, 1000),        new IntegerFloat(9563, 10000),      new IntegerFloat(2403, 2500),
            new IntegerFloat(9659, 10000),      new IntegerFloat(4851, 5000),       new IntegerFloat(9743, 10000),      new IntegerFloat(9781, 10000),      new IntegerFloat(1227, 1250),
            new IntegerFloat(1231, 1250),       new IntegerFloat(2469, 2500),       new IntegerFloat(4951, 5000),       new IntegerFloat(397, 400),     new IntegerFloat(1989, 2000),
            new IntegerFloat(9961, 10000),      new IntegerFloat(399, 400),     new IntegerFloat(4993, 5000),       new IntegerFloat(9993, 10000),      new IntegerFloat(4999, 5000),
            new IntegerFloat(1, 1),     new IntegerFloat(4999, 5000),       new IntegerFloat(9993, 10000),      new IntegerFloat(4993, 5000),       new IntegerFloat(399, 400),
            new IntegerFloat(9961, 10000),      new IntegerFloat(1989, 2000),       new IntegerFloat(397, 400),     new IntegerFloat(4951, 5000),       new IntegerFloat(2469, 2500),
            new IntegerFloat(1231, 1250),       new IntegerFloat(1227, 1250),       new IntegerFloat(9781, 10000),      new IntegerFloat(9743, 10000),      new IntegerFloat(4851, 5000),
            new IntegerFloat(9659, 10000),      new IntegerFloat(2403, 2500),       new IntegerFloat(9563, 10000),      new IntegerFloat(951, 1000),        new IntegerFloat(1891, 2000),
            new IntegerFloat(2349, 2500),       new IntegerFloat(1867, 2000),       new IntegerFloat(9271, 10000),      new IntegerFloat(1841, 2000),       new IntegerFloat(1827, 2000),
            new IntegerFloat(9063, 10000),      new IntegerFloat(8987, 10000),      new IntegerFloat(891, 1000),        new IntegerFloat(8829, 10000),      new IntegerFloat(4373, 5000),
            new IntegerFloat(433, 500),     new IntegerFloat(8571, 10000),      new IntegerFloat(106, 125),     new IntegerFloat(4193, 5000),       new IntegerFloat(829, 1000),
            new IntegerFloat(8191, 10000),      new IntegerFloat(809, 1000),        new IntegerFloat(3993, 5000),       new IntegerFloat(197, 250),     new IntegerFloat(7771, 10000),
            new IntegerFloat(383, 500),     new IntegerFloat(7547, 10000),      new IntegerFloat(7431, 10000),      new IntegerFloat(7313, 10000),      new IntegerFloat(7193, 10000),
            new IntegerFloat(7071, 10000),      new IntegerFloat(3473, 5000),       new IntegerFloat(6819, 10000),      new IntegerFloat(6691, 10000),      new IntegerFloat(82, 125),
            new IntegerFloat(6427, 10000),      new IntegerFloat(6293, 10000),      new IntegerFloat(1539, 2500),       new IntegerFloat(3009, 5000),       new IntegerFloat(5877, 10000),
            new IntegerFloat(1147, 2000),       new IntegerFloat(5591, 10000),      new IntegerFloat(2723, 5000),       new IntegerFloat(5299, 10000),      new IntegerFloat(103, 200),
            new IntegerFloat(1, 2),     new IntegerFloat(303, 625),     new IntegerFloat(2347, 5000),       new IntegerFloat(4539, 10000),      new IntegerFloat(4383, 10000),
            new IntegerFloat(2113, 5000),       new IntegerFloat(4067, 10000),      new IntegerFloat(3907, 10000),      new IntegerFloat(1873, 5000),       new IntegerFloat(3583, 10000),
            new IntegerFloat(171, 500),     new IntegerFloat(651, 2000),        new IntegerFloat(309, 1000),        new IntegerFloat(2923, 10000),      new IntegerFloat(689, 2500),
            new IntegerFloat(647, 2500),        new IntegerFloat(2419, 10000),      new IntegerFloat(2249, 10000),      new IntegerFloat(2079, 10000),      new IntegerFloat(477, 2500),
            new IntegerFloat(217, 1250),        new IntegerFloat(391, 2500),        new IntegerFloat(1391, 10000),      new IntegerFloat(609, 5000),        new IntegerFloat(209, 2000),
            new IntegerFloat(871, 10000),       new IntegerFloat(697, 10000),       new IntegerFloat(523, 10000),       new IntegerFloat(87, 2500),     new IntegerFloat(87, 5000),
            new IntegerFloat(0, 10000),     new IntegerFloat(-87, 5000),        new IntegerFloat(-87, 2500),        new IntegerFloat(523, -10000),      new IntegerFloat(697, -10000),
            new IntegerFloat(871, -10000),      new IntegerFloat(209, -2000),       new IntegerFloat(609, -5000),       new IntegerFloat(1391, -10000),     new IntegerFloat(391, -2500),
            new IntegerFloat(217, -1250),       new IntegerFloat(-477, 2500),       new IntegerFloat(-2079, 10000),     new IntegerFloat(2249, -10000),     new IntegerFloat(2419, -10000),
            new IntegerFloat(647, -2500),       new IntegerFloat(-689, 2500),       new IntegerFloat(-2923, 10000),     new IntegerFloat(-309, 1000),       new IntegerFloat(651, -2000),
            new IntegerFloat(171, -500),        new IntegerFloat(-3583, 10000),     new IntegerFloat(1873, -5000),      new IntegerFloat(-3907, 10000),     new IntegerFloat(4067, -10000),
            new IntegerFloat(2113, -5000),      new IntegerFloat(-4383, 10000),     new IntegerFloat(-4539, 10000),     new IntegerFloat(2347, -5000),      new IntegerFloat(-303, 625),
            new IntegerFloat(4999, -10000),     new IntegerFloat(-103, 200),        new IntegerFloat(5299, -10000),     new IntegerFloat(-2723, 5000),      new IntegerFloat(5591, -10000),
            new IntegerFloat(1147, -2000),      new IntegerFloat(-5877, 10000),     new IntegerFloat(3009, -5000),      new IntegerFloat(1539, -2500),      new IntegerFloat(6293, -10000),
            new IntegerFloat(6427, -10000),     new IntegerFloat(-82, 125),     new IntegerFloat(6691, -10000),     new IntegerFloat(6819, -10000),     new IntegerFloat(3473, -5000),
            new IntegerFloat(7071, -10000),     new IntegerFloat(7193, -10000),     new IntegerFloat(7313, -10000),     new IntegerFloat(7431, -10000),     new IntegerFloat(-7547, 10000),
            new IntegerFloat(383, -500),        new IntegerFloat(7771, -10000),     new IntegerFloat(197, -250),        new IntegerFloat(3993, -5000),      new IntegerFloat(809, -1000),
            new IntegerFloat(-8191, 10000),     new IntegerFloat(829, -1000),       new IntegerFloat(4193, -5000),      new IntegerFloat(106, -125),        new IntegerFloat(8571, -10000),
            new IntegerFloat(433, -500),        new IntegerFloat(-4373, 5000),      new IntegerFloat(-8829, 10000),     new IntegerFloat(891, -1000),       new IntegerFloat(-8987, 10000),
            new IntegerFloat(9063, -10000),     new IntegerFloat(1827, -2000),      new IntegerFloat(-1841, 2000),      new IntegerFloat(9271, -10000),     new IntegerFloat(-1867, 2000),
            new IntegerFloat(2349, -2500),      new IntegerFloat(-1891, 2000),      new IntegerFloat(-951, 1000),       new IntegerFloat(-9563, 10000),     new IntegerFloat(2403, -2500),
            new IntegerFloat(-9659, 10000),     new IntegerFloat(-4851, 5000),      new IntegerFloat(9743, -10000),     new IntegerFloat(-9781, 10000),     new IntegerFloat(1227, -1250),
            new IntegerFloat(-1231, 1250),      new IntegerFloat(2469, -2500),      new IntegerFloat(-4951, 5000),      new IntegerFloat(397, -400),        new IntegerFloat(1989, -2000),
            new IntegerFloat(-9961, 10000),     new IntegerFloat(-399, 400),        new IntegerFloat(-4993, 5000),      new IntegerFloat(9993, -10000),     new IntegerFloat(-4999, 5000),
            new IntegerFloat(1, -1),        new IntegerFloat(-4999, 5000),      new IntegerFloat(9993, -10000),     new IntegerFloat(-4993, 5000),      new IntegerFloat(-399, 400),
            new IntegerFloat(-9961, 10000),     new IntegerFloat(1989, -2000),      new IntegerFloat(397, -400),        new IntegerFloat(-4951, 5000),      new IntegerFloat(2469, -2500),
            new IntegerFloat(-1231, 1250),      new IntegerFloat(1227, -1250),      new IntegerFloat(-9781, 10000),     new IntegerFloat(9743, -10000),     new IntegerFloat(-4851, 5000),
            new IntegerFloat(-9659, 10000),     new IntegerFloat(2403, -2500),      new IntegerFloat(-9563, 10000),     new IntegerFloat(-951, 1000),       new IntegerFloat(-1891, 2000),
            new IntegerFloat(2349, -2500),      new IntegerFloat(-1867, 2000),      new IntegerFloat(9271, -10000),     new IntegerFloat(-1841, 2000),      new IntegerFloat(1827, -2000),
            new IntegerFloat(9063, -10000),     new IntegerFloat(-8987, 10000),     new IntegerFloat(891, -1000),       new IntegerFloat(-8829, 10000),     new IntegerFloat(-4373, 5000),
            new IntegerFloat(433, -500),        new IntegerFloat(8571, -10000),     new IntegerFloat(106, -125),        new IntegerFloat(4193, -5000),      new IntegerFloat(829, -1000),
            new IntegerFloat(-8191, 10000),     new IntegerFloat(809, -1000),       new IntegerFloat(3993, -5000),      new IntegerFloat(197, -250),        new IntegerFloat(7771, -10000),
            new IntegerFloat(383, -500),        new IntegerFloat(-7547, 10000),     new IntegerFloat(7431, -10000),     new IntegerFloat(7313, -10000),     new IntegerFloat(7193, -10000),
            new IntegerFloat(7071, -10000),     new IntegerFloat(3473, -5000),      new IntegerFloat(6819, -10000),     new IntegerFloat(6691, -10000),     new IntegerFloat(-82, 125),
            new IntegerFloat(6427, -10000),     new IntegerFloat(6293, -10000),     new IntegerFloat(1539, -2500),      new IntegerFloat(3009, -5000),      new IntegerFloat(-5877, 10000),
            new IntegerFloat(1147, -2000),      new IntegerFloat(5591, -10000),     new IntegerFloat(-2723, 5000),      new IntegerFloat(5299, -10000),     new IntegerFloat(-103, 200),
            new IntegerFloat(1, -2),        new IntegerFloat(-303, 625),        new IntegerFloat(2347, -5000),      new IntegerFloat(-4539, 10000),     new IntegerFloat(-4383, 10000),
            new IntegerFloat(2113, -5000),      new IntegerFloat(4067, -10000),     new IntegerFloat(-3907, 10000),     new IntegerFloat(1873, -5000),      new IntegerFloat(-3583, 10000),
            new IntegerFloat(171, -500),        new IntegerFloat(651, -2000),       new IntegerFloat(-309, 1000),       new IntegerFloat(-2923, 10000),     new IntegerFloat(-689, 2500),
            new IntegerFloat(647, -2500),       new IntegerFloat(2419, -10000),     new IntegerFloat(2249, -10000),     new IntegerFloat(-2079, 10000),     new IntegerFloat(-477, 2500),
            new IntegerFloat(217, -1250),       new IntegerFloat(391, -2500),       new IntegerFloat(1391, -10000),     new IntegerFloat(609, -5000),       new IntegerFloat(209, -2000),
            new IntegerFloat(871, -10000),      new IntegerFloat(697, -10000),      new IntegerFloat(523, -10000),      new IntegerFloat(-87, 2500),        new IntegerFloat(-87, 5000)};
        public static IntegerFloat SinAngle(short angle)
        {
            angle %= 360;
            return valueTable[angle];
        }
    }
}
