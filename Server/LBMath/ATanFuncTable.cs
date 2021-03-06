﻿
namespace LBMath
{
    public class ATanFuncTable
    {
        protected static byte[] valueTable = new byte[] {
            0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      0,      1,      1,      1,      1,      1,      1,      1,      1,      1,      1,      1,      1,
            1,      1,      1,      1,      1,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      2,      3,      3,      3,      3,      3,      3,      3,
            3,      3,      3,      3,      3,      3,      3,      3,      3,      3,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      4,      5,      5,
            5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      5,      6,      6,      6,      6,      6,      6,      6,      6,      6,      6,      6,      6,      6,      6,
            6,      6,      6,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      7,      8,      8,      8,      8,      8,      8,      8,      8,      8,
            8,      8,      8,      8,      8,      8,      8,      8,      8,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      10,     10,     10,
            10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     10,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,     11,
            11,     11,     11,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     13,     13,     13,     13,     13,     13,     13,     13,     13,
            13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     15,     15,
            15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     15,     16,     16,     16,     16,     16,     16,     16,     16,     16,     16,     16,     16,     16,
            16,     16,     16,     16,     16,     16,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     17,     18,     18,     18,     18,     18,
            18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     18,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,     19,
            19,     19,     19,     19,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     20,     21,     21,     21,     21,     21,     21,
            21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     21,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,     22,
            22,     22,     22,     22,     22,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     23,     24,     24,     24,     24,
            24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     24,     25,     25,     25,     25,     25,     25,     25,     25,     25,     25,     25,     25,     25,
            25,     25,     25,     25,     25,     25,     25,     25,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,     26,
            27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     27,     28,     28,     28,     28,     28,     28,     28,     28,
            28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     28,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,     29,
            29,     29,     29,     29,     29,     29,     29,     29,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,     30,
            30,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     31,     32,     32,     32,     32,     32,
            32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     32,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,
            33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     33,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,
            34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     34,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,     35,
            35,     35,     35,     35,     35,     35,     35,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,     36,
            36,     36,     36,     36,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,     37,
            37,     37,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,     38,
            39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,     39,
            40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,     40,
            41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,     41,
            41,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,     42,
            42,     42,     42,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,     43,
            43,     43,     43,     43,     43,     43,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     44,
            44,     44,     44,     44,     44,     44,     44,     44,     44,     44,     45};

        /// <summary>
        /// 获取dx和dy的点相对于x轴的正方向的逆时针的在第一象限的角度
        /// dx和dy必须为正
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        protected static int GetAngleInFirstQuadrant(int dx, int dy)
        {
            if (dx == dy)
                return 45;
            
            if(dx > dy)
            {
                return valueTable[dy * valueTable.Length / dx];
            }
            
            return 90 - valueTable[dx * valueTable.Length / dy];
        }

        public static int GetAngle(int dx, int dy)
        {
            if(dx == 0)
            {
                return dy > 0 ? 90 : 270;
            }
            if(dx < 0)
            {
                if(dy < 0)
                {
                    return GetAngleInFirstQuadrant(-dx, -dy) + 180;
                }
                return 180 - GetAngleInFirstQuadrant(-dx, dy);
            }
            if(dy < 0)
            {
                return 360 - GetAngleInFirstQuadrant(dx, -dy);
            }
            return GetAngleInFirstQuadrant(dx, dy);
        }
    }
}
