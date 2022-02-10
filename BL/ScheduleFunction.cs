using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    public class ScheduleFunction
    {
        static CancellationTokenSource m_ctSource;
        public static void RunPrepareDaily(DateTime date)
        {
            m_ctSource = new CancellationTokenSource();
            var dateNow = DateTime.Now;
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddMinutes(1);
                ts = date - dateNow;
            }
            Task.Delay(ts).ContinueWith((x) =>
            {
                EventBL.Reminder();
                RunPrepareDaily(date);
            }, m_ctSource.Token);
        }
        
    }
}
