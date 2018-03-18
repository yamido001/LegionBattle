
namespace LegionBattle
{
    public class SingleInstance<T> where T : new()
    {
        private static T mInstance;
        public static T Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = new T();
                }
                return mInstance;
            }
        }
    }
}
