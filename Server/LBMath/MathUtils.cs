using System;

namespace LBMath
{
    public class MathUtils
    {
        public static Int64 Max(Int64 x, Int64 y)
        {
            return x > y ? x : y;
        }

        public static Int64 Min(Int64 x, Int64 y)
        {
            return x < y ? x : y;
        }

        public static UInt64 Max(UInt64 x, UInt64 y)
        {
            return x > y ? x : y;
        }

        public static UInt64 Min(UInt64 x, UInt64 y)
        {
            return x < y ? x : y;
        }

        /// <summary>
        /// 求最小公约数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Int64 MaxCommonDivisor(Int64 x, Int64 y)
        {
            if (x == 0 || y == 0)
                return 0;
            Int64 maxValue = Max(x, y);
            Int64 minValue = Min(x, y);
            Int64 remainder;
            while (true)
            {
                remainder = maxValue % minValue;
                if (0 == remainder)
                    return minValue;
                maxValue = minValue;
                minValue = remainder;
            }
        }

        /// <summary>
        /// 求最大公倍数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Int64 MinCommonMultiple(Int64 x, Int64 y)
        {
            if (x == 0 || y == 0)
                return 0;
            //最小公倍数是两个数的所有最小公约数的乘积，x * y = 最大公约数 * 最小公倍数
            return (long)x * y / MaxCommonDivisor(x, y);
        }

        /// <summary>
        /// 判断点是否在矩形内部
        /// 矩形顶点是(-xSize/2, 0) (xSize/2, 0) (-xSize/2, ySize) (xSize/2, ySize)
        /// </summary>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <returns></returns>
        public static bool IsPosInSquare(int xSize, int ySize, IntVector2 pointPos)
        {
            if (pointPos.x * 2 < -xSize || pointPos.x * 2 > xSize)
                return false;
            if (pointPos.y < 0 || pointPos.y > ySize)
                return false;
            return true;
        }
    }
}
