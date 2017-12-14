using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	public struct BattlePosition{

		public static BattlePosition Zero = new BattlePosition(0, 0);

		public int x;
		public int y;

		public BattlePosition(int x, int y)
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
		/// </summary>
		/// <value>The manitude.</value>
		public int magnitude{
			get{
				return (int)Mathf.Sqrt (SqrMagnitude);
			}
		}

		public static BattlePosition Lerp(BattlePosition a, BattlePosition b, int lerpX, int lerpY)
		{
			a.x = a.x + (b.x - a.x) * lerpX / lerpY;
			a.y = a.y + (b.y - a.y) * lerpX / lerpY;
			return a;
		}

		public static bool operator == (BattlePosition lhs, BattlePosition rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		public static bool operator != (BattlePosition lhs, BattlePosition rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is BattlePosition))
				return 	false;
			BattlePosition other = (BattlePosition)obj;
			return this == other;
		}

		public static BattlePosition operator / (BattlePosition lhs, BattlePosition rhs)
		{
			BattlePosition ret = lhs;
			ret.x = lhs.x / rhs.x;
			ret.y = lhs.y / rhs.y;
			return ret;
		}

		public static BattlePosition operator * (BattlePosition lhs, BattlePosition rhs)
		{
			BattlePosition ret = lhs;
			ret.x *= rhs.x;
			ret.y *= rhs.y;
			return ret;
		}

		public static BattlePosition operator - (BattlePosition lhs, BattlePosition rhs)
		{
			BattlePosition ret = lhs;
			ret.x -= rhs.x;
			ret.y -= rhs.y;
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