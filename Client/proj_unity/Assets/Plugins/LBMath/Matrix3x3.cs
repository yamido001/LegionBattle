
namespace LBMath
{
    /// <summary>
    /// 三维变换矩阵，内部保证运算不会因为float类型出现误差
    /// </summary>
    public struct Matrix3x3
    {
        /// {m00, m01, m02}
        /// {m10, m11, m12}
        /// {m20, m21, m22}

        public IntegerFloat m00;
        public IntegerFloat m01;
        public IntegerFloat m02;
        public IntegerFloat m10;
        public IntegerFloat m11;
        public IntegerFloat m12;
        public IntegerFloat m20;
        public IntegerFloat m21;
        public IntegerFloat m22;

        public static Matrix3x3 Identity
        {
            get
            {
                return new Matrix3x3() {
                    m00 = IntegerFloat.One,     m01 = IntegerFloat.Zero,    m02 = IntegerFloat.Zero,
                    m10 = IntegerFloat.Zero,    m11 = IntegerFloat.One,     m21 = IntegerFloat.Zero,
                    m20 = IntegerFloat.Zero,    m22 = IntegerFloat.Zero,    m12 = IntegerFloat.Zero
                };
            }
        }

        /// <summary>
        /// 对二维坐标进行变换
        /// </summary>
        /// <param name="m3x3"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static IntVector2 operator * (Matrix3x3 m3x3, IntVector2 v1)
        {
            IntVector2 ret = v1;
            ret.x = (m3x3.m00 * v1.x).ToInt() + (m3x3.m10 * v1.y).ToInt() + m3x3.m20.ToInt();
            ret.y = (m3x3.m01 * v1.x).ToInt() + (m3x3.m11 * v1.y).ToInt() + m3x3.m21.ToInt();
            return ret;
        }

        public static Matrix3x3 Create(short angle, IntVector2 offset)
        {
            IntegerFloat cosFloat = CosFuncByTable.CosAngle(angle);
            IntegerFloat sinFloat = SinFuncByTable.SinAngle(angle);

            Matrix3x3 ma3x3 = Matrix3x3.Identity;
            ma3x3.m00 = cosFloat;
            ma3x3.m10 = sinFloat.NegativeValue();
            ma3x3.m01 = sinFloat;
            ma3x3.m11 = cosFloat;
            ma3x3.m20 = new IntegerFloat(offset.x);
            ma3x3.m21 = new IntegerFloat(offset.y);
            return ma3x3;
        }
    }
}
