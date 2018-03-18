using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegionBattle.ServerCommon
{
    class StringUtils
    {
        public static bool CopyStringToCharArray(string str, Char[] charArray)
        {
            //结尾以'\0'
            if (str.Length > charArray.Length - 1)
                return false;
            for (int i = 0; i < charArray.Length; ++i)
            {
                charArray[i] = i < str.Length ? str[i] : '\0';
            }
            return true;
        }

        public static bool StringEqualsCharArray(string str, Char[] charArray)
        {
            if (str.Length > charArray.Length - 1)
                return false;
            for(int i = 0; i < charArray.Length; ++i)
            {
                Char curCh = charArray[i];
                if(curCh == '\0')
                {
                    if(str.Length > i)
                    {
                        //str[i]是有效数据，但是charArray已经没有数据了，不相等
                        return false;
                    }
                    break;
                }
                if(str.Length <= i)
                {
                    //str中没有数据了，不相等
                    return false;
                }
                if (str[i] != curCh)
                    return false;
            }
            return true;
        }
    }
}
