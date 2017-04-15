using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using BANG.Common.Timer;

namespace BANG.CYBOSPLUS.DataCollector.StocksRequest
{
    public class RequestStockRunner
    {
        string dbConnString = null;
        string LogDir = null;
        DateTime sDateTime = new DateTime();
        DateTime eDateTime = new DateTime();
        List<string> StockCodes = new List<string>();
        public bool selfKill = false;

        public RequestStockRunner(StockRequestData argRequestData)
        {
            try
            {
                dbConnString = ((StockRequestData)argRequestData).dbConnString;
                LogDir = ((StockRequestData)argRequestData).LogDir;
                sDateTime = ((StockRequestData)argRequestData).sDate;
                eDateTime = ((StockRequestData)argRequestData).eDate;
                StockCodes = ((StockRequestData)argRequestData).StockCodes;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("RequestStockRunner", e.ToString());
            }
        }

        public void RequestStockData()
        {
            MyThreadTimer tStockMst = null;
            MyThreadTimer tCpSvr8081 = null;
            MyThreadTimer tCpSvr7225 = null;

            try
            {
                //System.Timers.Timer tStockMst = new System.Timers.Timer();
                //tStockMst.AutoReset = true;
                //tStockMst.Elapsed += new ElapsedEventHandler(tStockMst_Elapsed);
                //tStockMst.Interval = 1000 * 60;
                //tStockMst.Start();

                if (StockCodes.Count > 0)
                {
                    tStockMst = new MyThreadTimer(1000 * 60);
                    tStockMst.OnTick += new MyThreadTimer.tick(tStockMst_OnTick);
                    tStockMst.Start();
                }
                //System.Timers.Timer tCpSvr8081 = new System.Timers.Timer();
                //tCpSvr8081.AutoReset = true;
                //tCpSvr8081.Elapsed += new ElapsedEventHandler(tCpSvr8081_Elapsed);
                //tCpSvr8081.Interval = 1000 * 60;
                //tCpSvr8081.Start();

                if (StockCodes.Count == 0)
                {
                    tCpSvr8081 = new MyThreadTimer(1000 * 60);
                    tCpSvr8081.OnTick += new MyThreadTimer.tick(tCpSvr8081_OnTick);
                    tCpSvr8081.Start();
                }

                //System.Timers.Timer tCpSvr7225 = new System.Timers.Timer();
                //tCpSvr7225.AutoReset = true;
                //tCpSvr7225.Elapsed += new ElapsedEventHandler(tCpSvr7225_Elapsed);
                //tCpSvr7225.Interval = 1000 * 60;
                //tCpSvr7225.Start();

                if (StockCodes.Count == 0)
                {
                    tCpSvr7225 = new MyThreadTimer(1000 * 60);
                    tCpSvr7225.OnTick += new MyThreadTimer.tick(tCpSvr7225_OnTick);
                    tCpSvr7225.Start();
                }

                while (!selfKill)
                {
                    Thread.Sleep(1000 * 60);
                }

                if (tStockMst != null)
                    tStockMst.Stop();

                if (tCpSvr8081 != null)
                    tCpSvr8081.Stop();

                if (tCpSvr7225 != null)
                    tCpSvr7225.Stop();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RequestStockRunner-RequestStockData", ex.ToString());
            }
        }

        void tCpSvr7225_OnTick(object state)
        {
            //    throw new NotImplementedException();
            //}

            //void tCpSvr7225_Elapsed(object sender, ElapsedEventArgs e)
            //{
            try
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tCpSvr7225_OnTick");
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    CpSvr7225.RequestnSave(dbConnString, LogDir);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RequestStockRunner-tCpSvr7225_OnTick", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tCpSvr7225_OnTick" + Environment.NewLine + ex.ToString());
            }
        }

        void tCpSvr8081_OnTick(object state)
        {

            //}

            //void tCpSvr8081_Elapsed(object sender, ElapsedEventArgs e)
            //{
            try
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tCpSvr8081_OnTick");
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    CpSvr8081.RequestnSave(dbConnString, LogDir);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RequestStockRunner-tCpSvr8081_OnTick", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tCpSvr8081_OnTick" + Environment.NewLine + ex.ToString());
            }
        }

        void tStockMst_OnTick(object state)
        {
            //    //throw new NotImplementedException();
            //}

            //void tStockMst_Elapsed(object sender, ElapsedEventArgs e)
            //{
            try
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tStockMst_OnTick");
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    StockMst.RequestnSave(dbConnString, LogDir, StockCodes);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RequestStockRunner-tStockMst_OnTick", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : Data Request tStockMst_OnTick" + Environment.NewLine + ex.ToString());
            }
        }
    }

    public class StockRequestData
    {
        public string dbConnString { get; set; }
        public string LogDir { get; set; }
        public DateTime sDate { get; set; }
        public DateTime eDate { get; set; }
        public List<string> StockCodes { get; set; }
    }
}
