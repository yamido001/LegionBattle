using System;
using System.Collections.Generic;

namespace LegionBattle.ServerCommon
{
    class CachedObjectManager<T> where T : CachedObject, new()
    {
        List<T> mCachedList = null;
        public CachedObjectManager(int preCreateCount)
        {
            mCachedList = new List<T>(preCreateCount);
            for(int i = 0; i < mCachedList.Count; ++i)
            {
                mCachedList[i] = new T();
            }
        }

        public T GetObject()
        {
            if(0 == mCachedList.Count)
            {
                return new T();
            }
            int lastIndex = mCachedList.Count - 1;
            T ret = mCachedList[lastIndex];
            ret.OnWillUse();
            mCachedList.RemoveAt(lastIndex);
            return ret;
        }

        public void ReturnObject(T cacheObject)
        {
            cacheObject.OnWillRecycle();
            mCachedList.Add(cacheObject);
        }
    }
}
