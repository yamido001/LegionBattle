using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegionBattle.ServerCommon
{
    public class TimeManager : SingleInstance<TimeManager>
    {
        DateTime mStandard = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public long CurrentTimeMs
        {
            get
            { 
                TimeSpan cha = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
                long t = (long)cha.TotalMilliseconds;
                return t;
            }
        }
    }
}
