using System.Collections.Generic;
using GameBattle.LogicLayer.Scene;
using System;
using LegionBattle.ServerClientCommon;

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
            mRowCount = BattleSceneManager.Instance.SceneWidth / size;
            if (BattleSceneManager.Instance.SceneWidth % size != 0)
                ++mRowCount;

            mColumnCount = BattleSceneManager.Instance.SceneHeight / size;
            if (BattleSceneManager.Instance.SceneHeight % size != 0)
                ++mColumnCount;

            mLattileUnitList = new List<UnitBase>[mRowCount, mColumnCount];
        }


        public void UpadteUnitPos(UnitBase unit)
        {
            List<UnitBase> preLattleUnitList = GetLattleUnitList(unit.moveComp.lattlcePos.x, unit.moveComp.lattlcePos.y);
            if(null != preLattleUnitList)
            {
                preLattleUnitList.Remove(unit);
            }
            List<UnitBase> newLattleUnitList = GetLattleUnitList(unit.Position.x, unit.Position.y);
            newLattleUnitList.Add(unit);
        }

        /// <summary>
        /// 获取园中的所有单位，注意,如果不传入fill，返回的List是静态的，并不安全
        /// </summary>
        /// <returns>The unit list in circle.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="circleRadius">Circle radius.</param>
        /// <param name="filler">Filler.</param>
        public List<UnitBase> GetUnitListInCircle(IntVector2 centerPos, short circleRadius, List<UnitBase> filler = null)
        {
            List<UnitBase> ret = null;
            if(null == filler)
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
                        if ((lattleUnitList[k].Position - centerPos).SqrMagnitude > circleRadius)
                            continue;
                        ret.Add(lattleUnitList[k]);
                    }
                }
            }
            return ret;
        }

        public List<UnitBase> GetUnitListInLine(IntVector2 startPos, short angle, short lineWidth, short lineLength, List<UnitBase> filler = null)
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
            //TODO intVector2需要支持+
            IntVector2 rectangleRightBottomPos = startPos;
            rectangleRightBottomPos.x += lineWidth;
            IntVector2 rectangleLeftTopPos = rectangleLeftBottomPos;
            IntVector2 rectangleRightTopPos = rectangleRightBottomPos;
            rectangleLeftTopPos.y += lineWidth;
            rectangleRightTopPos.y += lineWidth;

            //对四个边进行旋转
            rectangleLeftBottomPos = RotatePos(rectangleLeftBottomPos, angle);
            rectangleLeftTopPos = RotatePos(rectangleLeftTopPos, angle);
            rectangleRightBottomPos = RotatePos(rectangleRightBottomPos, angle);
            rectangleRightTopPos = RotatePos(rectangleRightTopPos, angle);

            int xMin = BattleUtils.MinValue(rectangleLeftTopPos.x, rectangleLeftBottomPos.x, rectangleRightTopPos.x, rectangleRightBottomPos.x);
            int xMax = BattleUtils.MaxValue(rectangleLeftTopPos.x, rectangleLeftBottomPos.x, rectangleRightTopPos.x, rectangleRightBottomPos.x);
            int yMin = BattleUtils.MinValue(rectangleLeftTopPos.y, rectangleLeftBottomPos.y, rectangleRightTopPos.y, rectangleRightBottomPos.y);
            int yMax = BattleUtils.MaxValue(rectangleLeftTopPos.y, rectangleLeftBottomPos.y, rectangleRightTopPos.y, rectangleRightBottomPos.y);

            int searchIndexMinX = PosToLattlceIndex(xMin);
            int searchIndexMaxX = PosToLattlceIndex(xMax);
            int searchIndexMinY = PosToLattlceIndex(yMin);
            int searchIndexMaxY = PosToLattlceIndex(yMax);

            IntVector2 endPos = startPos;
            endPos.x += CosFuncByTable.CosAngle(angle).MulIntegerValue(lineLength);
            endPos.y += SinFuncByTable.SinAngle(angle).MulIntegerValue(lineLength);
            for (int i = searchIndexMinX; i <= searchIndexMaxX; i ++)
            {
                for (int j = searchIndexMinY; j <= searchIndexMaxY; ++j)
                {
                    List<UnitBase> lattleUnitList = mLattileUnitList[i, j];
                    if (null == lattleUnitList)
                        continue;
                    
                }
            }

            return ret;
        }

        IntVector2 RotatePos(IntVector2 pos, short angle)
        {
            IntVector2 ret = pos;
            IntegerFloat cosFloat = CosFuncByTable.CosAngle(angle);
            IntegerFloat sinFloat = SinFuncByTable.SinAngle(angle);
            ret.x = cosFloat.MulIntegerValue(pos.x) - sinFloat.MulIntegerValue(pos.y);
            ret.y = sinFloat.MulIntegerValue(pos.x) + cosFloat.MulIntegerValue(pos.y);
            return ret;
        }

        int PosToLattlceIndex(int pos)
        {
            return pos / mLattileSize;
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
    }

}