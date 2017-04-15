using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BANG.Common.Timer
{
    public class MyThreadTimer
    {
        public delegate void tick(object state);
        public event tick OnTick;

        long tickCount = 0;
        DateTime sDateTime;

        Thread tMain = null;

        public MyThreadTimer(long aTick)
        {
            this.tickCount = aTick;
        }

        public void Start()
        {
            tMain = new Thread(ThreadLoop);
            tMain.Start();
        }

        public void Stop()
        {
            tMain.Abort();
        }

        void ThreadLoop(object state)
        {
            sDateTime = DateTime.Now;
            while (true)
            {
                if (sDateTime.AddMilliseconds(tickCount) <= DateTime.Now)
                {
                    sDateTime = DateTime.Now;
                    if (OnTick != null)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(OnTick));
                    }
                }
                Thread.Sleep(1);
            }
        }

        //void EventCall(object state)
        //{
        //    OnTick();
        //}
    }
}
