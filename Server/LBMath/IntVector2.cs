using System.Collections;
using System.Collections.Generic;
using System;

namespace LBMath
{
	public struct IntVector2{

        public const short SerializeLength = 8;

		public readonly static IntVector2 Zero = new IntVector2(0, 0);

		public int x;
		public int y;

		public IntVector2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public long SqrMagnitude
		{
			get {
				return (long)x * x + y * y;
			}
		}


        /// <summary>
        /// 返回向量的长度
        /// 不保证没有浮点数误差
        /// </summary>
        /// <value>The manitude.</value>
        public int magnitude
        {
            get
            {
                return (int)Math.Sqrt(SqrMagnitude);
            }
        }

        public IntegerFloat safeMagnitude
        {
            get
            {
                int sqrMagnitude = x * x + y * y;
                if (sqrMagnitude == 0)
                    return IntegerFloat.Zero;

                IntegerFloat tx = new IntegerFloat(sqrMagnitude, 1);
                IntegerFloat ty = new IntegerFloat(sqrMagnitude, 2);
                IntegerFloat maxError = tx / 1000;
                IntegerFloat delta;
                do
                {
                    delta = ty * ty - tx;
                    ty -= delta / (ty * 2);
                } while (delta > maxError || delta < -maxError);
                return ty;
            }
        }


        //double sqrt(double x)
        //{
        //    float y;
        //    float delta;
        //    float maxError;

        //    if (x <= 0)
        //    {
        //        return 0;
        //    }

        //    // initial guess
        //    y = x / 2;

        //    // refine
        //    maxError = x * 0.001;

        //    do
        //    {
        //        delta = (y * y) - x;
        //        y -= delta / (2 * y);
        //    } while (delta > maxError || delta < -maxError);

        //    return y;
        //}

        public static IntVector2 MoveAngle(IntVector2 fromPos, short angle, int distant)
        {
            IntegerFloat sinValue = SinFuncByTable.SinAngle(angle);
            IntegerFloat cosValue = CosFuncByTable.CosAngle(angle);

            return new IntVector2(fromPos.x + (cosValue * distant).ToInt(), fromPos.y + (sinValue * distant).ToInt());
        }

        public static IntVector2 Rotate(IntVector2 pointPos, short angle)
        {
            IntVector2 ret = pointPos;
            IntegerFloat cosFloat = CosFuncByTable.CosAngle(angle);
            IntegerFloat sinFloat = SinFuncByTable.SinAngle(angle);
            ret.x = (cosFloat * pointPos.x).ToInt() - (sinFloat * pointPos.y).ToInt();
            ret.y = (sinFloat * pointPos.x).ToInt() + (cosFloat * pointPos.y).ToInt();
            return ret;
        }

		public static IntVector2 Lerp(IntVector2 a, IntVector2 b, int lerpX, int lerpY)
		{
			a.x = a.x + (b.x - a.x) * lerpX / lerpY;
			a.y = a.y + (b.y - a.y) * lerpX / lerpY;
			return a;
		}

		public static bool operator == (IntVector2 lhs, IntVector2 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		public static bool operator != (IntVector2 lhs, IntVector2 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}

        public static IntVector2 Clamp(IntVector2 value, IntVector2 min, IntVector2 max)
        {
            value.x = Math.Min(value.x, max.x);
            value.x = Math.Max(value.x, min.x);
            value.y = Math.Min(value.y, max.y);
            value.y = Math.Max(value.y, min.y);
            return value;
        }


        public override bool Equals (object obj)
		{
			if (!(obj is IntVector2))
				return 	false;
			IntVector2 other = (IntVector2)obj;
			return this == other;
		}

		public static IntVector2 operator / (IntVector2 lhs, IntVector2 rhs)
		{
			IntVector2 ret = lhs;
			ret.x = lhs.x / rhs.x;
			ret.y = lhs.y / rhs.y;
			return ret;
		}

		public static IntVector2 operator * (IntVector2 lhs, IntVector2 rhs)
		{
			IntVector2 ret = lhs;
			ret.x *= rhs.x;
			ret.y *= rhs.y;
			return ret;
		}

		public static IntVector2 operator - (IntVector2 lhs, IntVector2 rhs)
		{
			IntVector2 ret = lhs;
			ret.x -= rhs.x;
			ret.y -= rhs.y;
			return ret;
		}

        public static IntVector2 operator +(IntVector2 lhs, IntVector2 rhs)
        {
            IntVector2 ret = lhs;
            ret.x += rhs.x;
            ret.y += rhs.y;
            return ret;
        }

        public static IntVector2 MinVector(IntVector2 v1, IntVector2 v2, IntVector2 v3, IntVector2 v4)
        {
            IntVector2 ret = v1;
            ret.x = Math.Min(ret.x, v2.x);
            ret.x = Math.Min(ret.x, v3.x);
            ret.x = Math.Min(ret.x, v4.x);

            ret.y = Math.Min(ret.y, v2.y);
            ret.y = Math.Min(ret.y, v3.y);
            ret.y = Math.Min(ret.y, v4.y);
            return ret;
        }

        public static IntVector2 MaxVector(IntVector2 v1, IntVector2 v2, IntVector2 v3, IntVector2 v4)
        {
            IntVector2 ret = v1;
            ret.x = Math.Max(ret.x, v2.x);
            ret.x = Math.Max(ret.x, v3.x);
            ret.x = Math.Max(ret.x, v4.x);

            ret.y = Math.Max(ret.y, v2.y);
            ret.y = Math.Max(ret.y, v3.y);
            ret.y = Math.Max(ret.y, v4.y);
            return ret;
        }

        public override int GetHashCode ()
		{
			return (x << 16) | y;
		}

		public override string ToString ()
		{
			return string.Format ("[{0:D}, {1:D}]", x, y);
		}
	}
}