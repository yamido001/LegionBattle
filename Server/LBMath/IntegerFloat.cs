using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBMath
{
    public struct IntegerFloat
    {
        const int Precision = 10000;

        /// <summary>
        /// 等于 真实值 * Precision后再取整
        /// </summary>
        long x;

        public readonly static IntegerFloat One = new IntegerFloat(1, 1);
        public readonly static IntegerFloat Zero = new IntegerFloat(0, 1);

        public IntegerFloat(int x, int y)
        {
            if (0 == y)
                throw new Exception("IntegerFloat 构造参数y不能为0");
            this.x = (long)x * Precision / y;
        }

        public IntegerFloat(int x)
        {
            this.x = (long)x * Precision;
        }

        public IntegerFloat(float x)
        {
            this.x = (long)(x * Precision);
        }

        public float ToFloat()
        {
            return (float)(double)x / Precision;
        }

        public Int32 ToInt()
        {
            return (Int32)(x / Precision);
        }

        public Int64 ToInt64WithPrecision()
        {
            return x;
        }

        public IntegerFloat NegativeValue()
        {
            IntegerFloat ret = this;
            ret.x *= -1;
            return ret;
        }

        public static IntegerFloat operator /(IntegerFloat lhs, IntegerFloat rhs)
        {
            if (rhs.x == 0)
                throw new Exception("IntegerFloat的除数不能为0");
            lhs.x = lhs.x * Precision / rhs.x;
            return lhs;
        }

        public static IntegerFloat operator /(IntegerFloat lhs, Int32 div)
        {
            lhs.x /= div;
            return lhs;
        }

        public static IntegerFloat operator *(IntegerFloat lhs, IntegerFloat rhs)
        {
            lhs.x = lhs.x * rhs.x / Precision;
            return lhs;
        }

        public static IntegerFloat operator *(IntegerFloat lhs, Int32 mul)
        {
            lhs.x *= mul;
            return lhs;
        }

        public static IntegerFloat operator -(IntegerFloat lhs, IntegerFloat rhs)
        {
            lhs.x -= rhs.x;
            return lhs;
        }

        public static bool operator >(IntegerFloat lhs, IntegerFloat rhs)
        {
            return lhs.x > rhs.x;
        }

        public static bool operator <(IntegerFloat lhs, IntegerFloat rhs)
        {
            return lhs.x < rhs.x;
        }

        public static IntegerFloat operator -(IntegerFloat lhs)
        {
            lhs.x *= -1;
            return lhs;
        }
    }
}
