using System;

namespace LBMath
{
    /// <summary>
    /// 使用整数表达分数，避免不同设备间的误差,值域范围是32位
    /// </summary>
    public struct IntegerFloatXY
    {
        const int MaxY = 10000;
        /// <summary>
        /// 分母，被除数
        /// </summary>
        public long x;
        /// <summary>
        /// 分子，除数
        /// </summary>
        public long y;

        public readonly static IntegerFloatXY One = new IntegerFloatXY(1, 1);
        public readonly static IntegerFloatXY Zero = new IntegerFloatXY(0, 1);

        public IntegerFloatXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public float ToFloat()
        {
            return (float)x / y;
        }

        public int MulIntegerValue(int mulValue)
        {
            return (int)(x * mulValue / y);
        }

        private void Simplifya()
        {
            Int64 maxCommonDivisor = MathUtils.MaxCommonDivisor(x, y);
            if (0 == maxCommonDivisor)
                return;
            x /= maxCommonDivisor;
            y /= maxCommonDivisor;
            if( y > MaxY || y < -MaxY)
            {
                x = x * MaxY / y;
                y = MaxY;
            }
        }

        private void MakeDenominator(Int64 denominator)
        {
            x = x * denominator / y;
            y = denominator;
        }

        /// <summary>
        /// 把float转换为整形的浮点数
        /// 传入浮点数值域应该在int.MinValue / 10000 到 int.MaxValue / 10000之间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IntegerFloatXY FloatToIntegerFloatXY(float value)
        {
            IntegerFloatXY ret = new IntegerFloatXY((int)(value * MaxY), MaxY);
            ret.Simplifya();
            return ret;
        }
        
        public static IntegerFloatXY IntToIntegerFloatXY(int value)
        {
            IntegerFloatXY ret = One;
            ret.x *= value;
            return ret;
        }

        public IntegerFloatXY NegativeValue()
        {
            IntegerFloatXY ret = this;
            ret.x *= -1;
            return ret;
        }

        public static IntegerFloatXY operator /(IntegerFloatXY lhs, IntegerFloatXY rhs)
        {
            IntegerFloatXY ret = lhs;
            ret.x *= rhs.y;
            ret.y *= rhs.x;
            ret.Simplifya();
            return ret;
        }

        public static IntegerFloatXY operator /(IntegerFloatXY lhs, Int32 div)
        {
            lhs.y *= 1000;
            lhs.Simplifya();
            return lhs;
        }

        public static IntegerFloatXY operator *(IntegerFloatXY lhs, IntegerFloatXY rhs)
        {
            IntegerFloatXY ret = lhs;
            ret.x *= rhs.x;
            ret.y *= rhs.y;
            ret.Simplifya();
            return ret;
        }

        public static IntegerFloatXY operator *(IntegerFloatXY lhs, Int32 mul)
        {
            lhs.x *= mul;
            return lhs;
        }

        public static IntegerFloatXY operator -(IntegerFloatXY lhs, IntegerFloatXY rhs)
        {
            Int64 minMultiple = MathUtils.MinCommonMultiple(lhs.y, rhs.y);
            lhs.MakeDenominator(minMultiple);
            rhs.MakeDenominator(minMultiple);

            lhs.x -= rhs.x;
            lhs.Simplifya();
            return lhs;
        }

        public static bool operator > (IntegerFloatXY lhs, IntegerFloatXY rhs)
        {
            Int64 minMultiple = MathUtils.MinCommonMultiple(lhs.y, rhs.y);
            lhs.MakeDenominator(minMultiple);
            rhs.MakeDenominator(minMultiple);
            return lhs.x > rhs.x;
        }

        public static bool operator < (IntegerFloatXY lhs, IntegerFloatXY rhs)
        {
            Int64 minMultiple = MathUtils.MinCommonMultiple(lhs.y, rhs.y);
            lhs.MakeDenominator(minMultiple);
            rhs.MakeDenominator(minMultiple);
            return lhs.x < rhs.x;
        }

        public static IntegerFloatXY operator - (IntegerFloatXY lhs)
        {
            lhs.x *= -1;
            return lhs;
        }
        
    }
}
