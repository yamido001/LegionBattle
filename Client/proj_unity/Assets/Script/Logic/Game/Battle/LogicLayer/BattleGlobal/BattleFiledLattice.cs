using System.Collections.Generic;
using GameBattle.LogicLayer.Scene;
using System;
using LBMath;

namespace GameBattle.LogicLayer
{
    public class BattleFiledLattile : Singleton<BattleFiledLattile>
    {
        List<UnitBase>[,] mLattileUnitList = null;
        short mLattileSize;
        List<UnitBase> mConditionFindRet = new List<UnitBase>(10);
        int mRowCount = 0;
        int mColumnCount = 0;

        /// <summary>
        /// 初始化，传入网格的大小
        /// </summary>
        /// <param name="size">Size.</param>
        public void InitLattile(short size)
        {
            mLattileSize = size;
            mRowCount = BattleSceneManager.Instance.SceneSize.x / size;
            if (BattleSceneManager.Instance.SceneSize.x % size != 0)
                ++mRowCount;

            mColumnCount = BattleSceneManager.Instance.SceneSize.y / size;
            if (BattleSceneManager.Instance.SceneSize.y % size != 0)
                ++mColumnCount;

            mLattileUnitList = new List<UnitBase>[mRowCount, mColumnCount];
        }


        public void UpadteUnitPos(UnitBase unit)
        {
            RemoveUnit(unit);
            List<UnitBase> newLattleUnitList = GetLattleUnitList(unit.Position.x, unit.Position.y, true);
            unit.moveComp.lattlcePos = unit.Position;
            if (null != newLattleUnitList)
                newLattleUnitList.Add(unit);
        }

        public void RemoveUnit(UnitBase unit)
        {
            List<UnitBase> preLattleUnitList = GetLattleUnitList(unit.moveComp.lattlcePos.x, unit.moveComp.lattlcePos.y);
            if (null != preLattleUnitList)
            {
                preLattleUnitList.Remove(unit);
            }
        }
        

        int PosToLattlceIndex(int pos)
        {
            return pos / mLattileSize;
        }

        int PosToSafeXLattlceIndex(int pos)
        {
            int index = PosToLattlceIndex(pos);
            index = Math.Max(0, index);
            index = Math.Min(0, mRowCount - 1);
            return index;
        }

        int PosToSafeYLattlceIndex(int pos)
        {
            int index = PosToLattlceIndex(pos);
            index = Math.Max(0, index);
            index = Math.Min(0, mColumnCount - 1);
            return index;
        }

        bool IsValidIndex(int xIndex, int yIndex)
        {
            return xIndex >= 0 && xIndex < mRowCount && yIndex >= 0 && yIndex < mColumnCount;
        }

        List<UnitBase> GetLattleUnitList(int x, int y, bool create = false)
        {
            x = PosToLattlceIndex(x);
            y = PosToLattlceIndex(y);
            if (!IsValidIndex(x, y))
                return null;
            if(null == mLattileUnitList[x, y] && create)
            {
                mLattileUnitList[x, y] = new List<UnitBase>();
            }
            return mLattileUnitList[x, y];
        }

        #region 目标搜索

        /// <summary>
        /// 获取园中的所有单位，注意,如果不传入fill，返回的List是静态的，并不安全
        /// </summary>
        /// <returns>The unit list in circle.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="circleRadius">Circle radius.</param>
        /// <param name="filler">Filler.</param>
        public List<UnitBase> GetUnitListInCircle(IntVector2 centerPos, int circleRadius, CampType effectCamp, List<UnitBase> filler = null)
        {
            List<UnitBase> ret = null;
            if (null == filler)
            {
                ret = mConditionFindRet;
                ret.Clear();
            }
            else
            {
                ret = filler;
            }
            int circleCenterIndexX = PosToLattlceIndex(centerPos.x);
            int circleCenterIndexY = PosToLattlceIndex(centerPos.y);
            if (!IsValidIndex(circleCenterIndexX, circleCenterIndexY))
                return ret;
            int searchLattceCount = PosToLattlceIndex(circleRadius);
            int searchIndexMinX = Math.Max(0, circleCenterIndexX - searchLattceCount);
            int searchIndexMaxX = Math.Min(mRowCount - 1, circleCenterIndexX + searchLattceCount);
            int searchIndexMinY = Math.Max(0, circleCenterIndexY - searchLattceCount);
            int searchIndexMaxY = Math.Min(mColumnCount - 1, circleCenterIndexY + searchLattceCount);
            circleRadius *= circleRadius;

            for (int i = searchIndexMinX; i < searchIndexMaxX; ++i)
            {
                for (int j = searchIndexMinY; j < searchIndexMaxY; ++j)
                {
                    List<UnitBase> lattleUnitList = mLattileUnitList[i, j];
                    if (null == lattleUnitList)
                        continue;
                    for (int k = 0; k < lattleUnitList.Count; ++k)
                    {
                        if (lattleUnitList[k].Data.camp != effectCamp)
                            continue;
                        if ((lattleUnitList[k].Position - centerPos).SqrMagnitude > circleRadius)
                            continue;
                        ret.Add(lattleUnitList[k]);
                    }
                }
            }
            return ret;
        }

        public List<UnitBase> GetUnitListInLine(IntVector2 startPos, short angle, short lineWidth, short lineLength, CampType effectCamp, List<UnitBase> filler = null)
        {
            List<UnitBase> ret = null;
            if (null == filler)
            {
                ret = mConditionFindRet;
                ret.Clear();
            }
            else
            {
                ret = filler;
            }

            //求出未旋转时的矩形的四个边的坐标
            IntVector2 rectangleLeftBottomPos = startPos - new IntVector2(lineWidth, 0);
            IntVector2 rectangleRightBottomPos = startPos + new IntVector2(lineWidth, 0);
            IntVector2 rectangleLeftTopPos = rectangleLeftBottomPos + new IntVector2(0, lineLength);
            IntVector2 rectangleRightTopPos = rectangleRightBottomPos + new IntVector2(0, lineLength);

            //对四个边进行旋转
            rectangleLeftBottomPos = IntVector2.Rotate(rectangleLeftBottomPos, angle);
            rectangleLeftTopPos = IntVector2.Rotate(rectangleLeftTopPos, angle);
            rectangleRightBottomPos = IntVector2.Rotate(rectangleRightBottomPos, angle);
            rectangleRightTopPos = IntVector2.Rotate(rectangleRightTopPos, angle);

            IntVector2 minPos = IntVector2.MinVector(rectangleLeftBottomPos, rectangleLeftTopPos, rectangleRightBottomPos, rectangleRightTopPos);
            IntVector2 maxPos = IntVector2.MaxVector(rectangleLeftBottomPos, rectangleLeftTopPos, rectangleRightBottomPos, rectangleRightTopPos);

            int searchIndexMinX = PosToSafeXLattlceIndex(minPos.x);
            int searchIndexMaxX = PosToSafeXLattlceIndex(maxPos.x);
            int searchIndexMinY = PosToSafeYLattlceIndex(minPos.y);
            int searchIndexMaxY = PosToSafeYLattlceIndex(maxPos.y);

            Matrix3x3 transMa3x3 = Matrix3x3.Create(angle, startPos);
            for (int i = searchIndexMinX; i <= searchIndexMaxX; i++)
            {
                for (int j = searchIndexMinY; j <= searchIndexMaxY; ++j)
                {
                    List<UnitBase> lattleUnitList = mLattileUnitList[i, j];
                    if (null == lattleUnitList)
                        continue;
                    for (int k = 0; k < lattleUnitList.Count; ++k)
                    {
                        UnitBase unitBase = lattleUnitList[k];
                        if (unitBase.Data.camp != effectCamp)
                            continue;
                        IntVector2 posInSquare = transMa3x3 * unitBase.Position;
                        if (MathUtils.IsPosInSquare(lineWidth, lineLength, posInSquare))
                        {
                            ret.Add(unitBase);
                        }
                    }
                }
            }
            return ret;
        }
        #endregion
    }

}