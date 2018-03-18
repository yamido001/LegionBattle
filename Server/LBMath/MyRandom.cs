using System;

namespace LBMath
{
    public class MyRandom
    {
        const Int32 M = 0x7fff; 
        public UInt32 Seed
        {
            get;
            set;
        }

        public Int32 Random()
        {
            Seed = (Seed * 69069 + 1);
            return (Int32)Seed & M;
        }

        public Int32 Random(Int32 r1, Int32 r2)
        {
            if(r1 > r2)
            {
                Int32 temp = r1;
                r1 = r2;
                r2 = temp;
            }
            else if(r1 == r2)
            {
                return r1;
            }
            Int32 random = Random();
            return r1 + (r2 - r1) * random / M;
        }
    }
}
