using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BANG.CYBOSPLUS.DAL;
using System.Diagnostics;

namespace BANG.CYBOSPLUS.DataCollector.StocksSubscribe
{
    public class SubscribeStocksRunner
    {
        string dbConnString = null;
        string LogDir = null;

        List<String> StockCodes = new List<string>();
        DateTime sDateTime = new DateTime();
        DateTime eDateTime = new DateTime();



        // 1. 개체 선언
        CPSYSDIBLib.CpMarketWatchS cpMarketWatchS;
        DSCBO1Lib.CpSvr8091S cp8091s;
        DSCBO1Lib.CpSvr8111KS cp8111ks;
        DSCBO1Lib.CpSvr8111S cp8111s;
        //CPSYSDIBLib.CpSvrNew7221S cpNew7221s;
        //DSCBO1Lib.ExpectIndexS expectIndexS;
        //CPSYSDIBLib.K200Expect k200Expect;
        //DSCBO1Lib.StockAdkS stockAdKs;
        //DSCBO1Lib.StockAdS stockAds;
        DSCBO1Lib.StockCur stockCur;
        //DSCBO1Lib.StockIndexis stockIndexIS;
        //DSCBO1Lib.StockJpbid stockJpBid;
        //DSCBO1Lib.StockMember stockMember;

        //Dictionary<string, FileLog> flms = new Dictionary<string, FileLog>();

        public SubscribeStocksRunner(string dbConnString, string LogDir
            , DateTime sDateTime
            , DateTime eDateTime
            ,List<String> aStockCode
        )
        {
            try
            {
                this.dbConnString = dbConnString;
                this.LogDir = LogDir;
                this.sDateTime = sDateTime;
                this.eDateTime = eDateTime;

                this.StockCodes = aStockCode;

                // 2. 개체 생성
                cpMarketWatchS = new CPSYSDIBLib.CpMarketWatchS();
                cp8091s = new DSCBO1Lib.CpSvr8091S();
                cp8111ks = new DSCBO1Lib.CpSvr8111KS();
                cp8111s = new DSCBO1Lib.CpSvr8111S();
                //cpNew7221s = new CPSYSDIBLib.CpSvrNew7221S();
                //expectIndexS = new DSCBO1Lib.ExpectIndexS();
                //k200Expect = new CPSYSDIBLib.K200Expect();
                //stockAdKs = new DSCBO1Lib.StockAdkS();
                //stockAds = new DSCBO1Lib.StockAdS();
                stockCur = new DSCBO1Lib.StockCur();
                //stockIndexIS = new DSCBO1Lib.StockIndexis();
                //stockJpBid = new DSCBO1Lib.StockJpbid();
                //stockMember = new DSCBO1Lib.StockMember();

                //flms.Add("CpMarketWatchS", new FileLog(string.Format("[{0}]", "CpMarketWatchS"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("CpSvr8091S", new FileLog(string.Format("[{0}]", "CpSvr8091S"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("CpSvr8111KS", new FileLog(string.Format("[{0}]", "CpSvr8111KS"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("CpSvr8111S", new FileLog(string.Format("[{0}]", "CpSvr8111S"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("CpSvrNew7221S", new FileLog(string.Format("[{0}]", "CpSvrNew7221S"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockAdkS", new FileLog(string.Format("[{0}]", "StockAdkS"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockAdS", new FileLog(string.Format("[{0}]", "StockAdS"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockCur", new FileLog(string.Format("[{0}]", "StockCur"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockIndexis", new FileLog(string.Format("[{0}]", "StockIndexis"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockJpbid", new FileLog(string.Format("[{0}]", "StockJpbid"), LogDir, LogFileInteval.HOURLY));
                //flms.Add("StockMember", new FileLog(string.Format("[{0}]", "StockMember"), LogDir, LogFileInteval.HOURLY));
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
            }
        }

        public void Subscribe()
        {
            try
            {
                // 3. 이벤트 핸들러 정하고
                cpMarketWatchS.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(cpMarketWatchS_Received);
                cp8091s.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8091s_Received);
                cp8111ks.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8111ks_Received);
                cp8111s.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8111s_Received);
                //cpNew7221s.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(cpNew7221s_Received);
                //expectIndexS.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(expectIndexS_Received);
                //k200Expect.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(k200Expect_Received);
                //stockAdKs.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockAdKs_Received);
                //stockAds.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockAds_Received);
                stockCur.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockCur_Received);
                //stockIndexIS.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockIndexIS_Received);
                //stockJpBid.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockJpBid_Received);
                //stockMember.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockMember_Received);



                // 4. SetInputValue 정하고 구독 요청

                // CpMarketWatchS
                if (StockCodes.Count == 0)
                {
                    cpMarketWatchS.SetInputValue(0, "*");
                    cpMarketWatchS.Subscribe();

                    cp8091s.SetInputValue(0, "*");
                    cp8091s.SetInputValue(1, "*");
                    cp8091s.Subscribe();

                    cp8111ks.Subscribe();

                    cp8111s.Subscribe();

                    //cpNew7221s.SetInputValue(0, "*");
                    //cpNew7221s.Subscribe();

                    //stockAdKs.Subscribe();

                    //stockAds.Subscribe();

                    //List<string> indusCodes = new SqlDal(dbConnString).GetIndustryCode();
                    //foreach (string indCode in indusCodes)
                    //{
                    //    stockIndexIS.SetInputValue(0, indCode);
                    //    stockIndexIS.Subscribe();
                    //}
                }

                // StockCur
                //List<string> StockCodes = new SqlDal(dbConnString).GetStockCode();
                if (StockCodes.Count > 0)
                {
                    foreach (string sCode in StockCodes)
                    {
                        stockCur.SetInputValue(0, sCode);
                        stockCur.Subscribe();
                    }

                    //foreach (string sCode in StockCodes)
                    //{
                    //    stockJpBid.SetInputValue(0, sCode);
                    //    stockJpBid.Subscribe();
                    //}

                    //foreach (string sCode in StockCodes)
                    //{
                    //    stockMember.SetInputValue(0, sCode);
                    //    stockMember.Subscribe();
                    //}
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
            }
        }

        public void UnSubscribe()
        {
            try
            {
                // 3. 이벤트 핸들러 정하고
                //cpMarketWatchS.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(cpMarketWatchS_Received);
                //cp8091s.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8091s_Received);
                //cp8111ks.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8111ks_Received);
                //cp8111s.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(cp8111s_Received);
                //cpNew7221s.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(cpNew7221s_Received);
                ////expectIndexS.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(expectIndexS_Received);
                ////k200Expect.Received += new CPSYSDIBLib._ISysDibEvents_ReceivedEventHandler(k200Expect_Received);
                //stockAdKs.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockAdKs_Received);
                //stockAds.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockAds_Received);
                //stockCur.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockCur_Received);
                //stockIndexIS.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockIndexIS_Received);
                //stockJpBid.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockJpBid_Received);
                //stockMember.Received += new DSCBO1Lib._IDibEvents_ReceivedEventHandler(stockMember_Received);



                // 4. SetInputValue 정하고 구독 요청

                // CpMarketWatchS
                if (StockCodes.Count == 0)
                {
                    cpMarketWatchS.SetInputValue(0, "*");
                    cpMarketWatchS.Unsubscribe();

                    cp8091s.SetInputValue(0, "*");
                    cp8091s.SetInputValue(1, "*");
                    cp8091s.Unsubscribe();

                    cp8111ks.Unsubscribe();

                    cp8111s.Unsubscribe();

                    //cpNew7221s.SetInputValue(0, "*");
                    //cpNew7221s.Unsubscribe();

                    //stockAdKs.Unsubscribe();

                    //stockAds.Unsubscribe();

                    //List<string> indusCodes = new SqlDal(dbConnString).GetIndustryCode();
                    //foreach (string indCode in indusCodes)
                    //{
                    //    stockIndexIS.SetInputValue(0, indCode);
                    //    stockIndexIS.Unsubscribe();
                    //}
                }

                if (StockCodes.Count > 0)
                {
                    foreach (string sCode in StockCodes)
                    {
                        stockCur.SetInputValue(0, sCode);
                        stockCur.Unsubscribe();
                    }

                    //foreach (string sCode in StockCodes)
                    //{
                    //    stockJpBid.SetInputValue(0, sCode);
                    //    stockJpBid.Unsubscribe();
                    //}

                    //foreach (string sCode in StockCodes)
                    //{
                    //    stockMember.SetInputValue(0, sCode);
                    //    stockMember.Unsubscribe();
                    //}

                }

                //CPUTILLib.CpCybos ccbos = new CPUTILLib.CpCybos();
                //Debug.Print("남은 요청갯수 Subscribe : {0}", ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_SUBSCRIBE).ToString());
                //SqlDal sd = new SqlDal(dbConnString);
                //sd.UpdateRemainSubscribe(new object[] { ccbos.GetLimitRemainCount(CPUTILLib.LIMIT_TYPE.LT_SUBSCRIBE).ToString() });
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
            }
        }

        // 7. 데이터 저장 메소드 추가
        //void stockMember_Received()
        //{
        //    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : stockMember_Received");
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("StockMember", LogType.INFORMATION, LogDir, "StockMember 신호 수신", flms["StockMember"]);

        //            SqlDal sd = new SqlDal(dbConnString);
        //            object StockCode = stockMember.GetHeaderValue(0);
        //            object count = stockMember.GetHeaderValue(1);
        //            object time = stockMember.GetHeaderValue(2);

        //            List<object> Params = new List<object>();
        //            object[] objArry;

        //            for (int j = 0; j < int.Parse(count.ToString()); j++)
        //            {
        //                Params.Add(StockCode);
        //                Params.Add(j);
        //                Params.Add(time);

        //                for (int i = 0; i < 4; i++)
        //                {
        //                    object o = stockMember.GetDataValue(i, j);
        //                    if (o == null) o = "0";
        //                    Params.Add(o);
        //                }

        //                objArry = Params.ToArray();
        //                sd.SaveStockMember(objArry);

        //                Params.Clear();
        //            }
        //            //WriteMsg("StockMember", LogType.INFORMATION, LogDir, string.Format("{0} 저장", StockCode), flms["StockMember"]);

        //            sd.Close();
        //            sd = null;

        //            Params = null;
        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("StockMember", LogType.ERROR, LogDir, ex.ToString(), "StockMember");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //    }
        //}

        //void stockJpBid_Received()
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("StockJpbid", LogType.INFORMATION, LogDir, "StockJpbid 신호 수신", flms["StockJpbid"]);

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            object[] objArry;

        //            for (int i = 0; i < 69; i++)
        //            {
        //                object o = stockJpBid.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);
        //            }

        //            objArry = Params.ToArray();
        //            sd.SaveStockJpbid(objArry);
        //            //WriteMsg("StockJpbid", LogType.INFORMATION, LogDir, "저장", flms["StockJpbid"]);

        //            sd.Close();
        //            sd = null;

        //            Params.Clear();
        //            Params = null;

        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("StockJpbid", LogType.ERROR, LogDir, ex.ToString(), "StockJpbid");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //    }
        //}

        //void stockIndexIS_Received()
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("StockIndexis", LogType.INFORMATION, LogDir, "StockIndexis 신호 수신", flms["StockIndexis"]);

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            object[] objArry;

        //            for (int i = 1; i < 8; i++)
        //            {
        //                object o = stockIndexIS.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);
        //            }

        //            objArry = Params.ToArray();
        //            sd.SaveStockIndexis(objArry);
        //            //WriteMsg("StockIndexis", LogType.INFORMATION, LogDir, "저장", flms["StockIndexis"]);

        //            sd.Close();
        //            sd = null;

        //            Params.Clear();
        //            Params = null;

        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("StockIndexis", LogType.ERROR, LogDir, ex.ToString(), "StockIndexis");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
        //    }
        //}

        void stockCur_Received()
        {
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : stockCur_Received");
            //throw new NotImplementedException();
            try
            {
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    //WriteMsg("StockCur", LogType.INFORMATION, LogDir, "StockCur 신호 수신", flms["StockCur"]);

                    SqlDal sd = new SqlDal(dbConnString);
                    List<object> Params = new List<object>();
                    object[] objArry;
                    for (int i = 0; i < 29; i++)
                    {
                        if (i == 11 || i == 12) continue;

                        object o = stockCur.GetHeaderValue(i);
                        if (o == null) o = "0";
                        Params.Add(o);
                    }
                    objArry = Params.ToArray();
                    sd.SaveStockCur(objArry);
                    //WriteMsg("StockCur", LogType.INFORMATION, LogDir, string.Format("{0} 저장", stockCur.GetHeaderValue(1)), flms["StockCur"]);

                    sd.Close();
                    sd = null;

                    Params.Clear();
                    Params = null;

                    objArry = null;

                }
            }
            catch (Exception ex)
            {
                //WriteMsg("StockCur", LogType.ERROR, LogDir, ex.ToString(), "StockCur");
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
            }
        }

        //void stockAds_Received()
        //{
        //    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : stockAds_Received");
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("StockAds", LogType.INFORMATION, LogDir, "StockAds 신호 수신", flms["StockAds"]);

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            object[] objArry;
        //            for (int i = 0; i < 7; i++)
        //            {
        //                object o = stockAds.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);


        //            }
        //            objArry = Params.ToArray();
        //            sd.SavekStockAds(objArry);
        //            //WriteMsg("StockAds", LogType.INFORMATION, LogDir, "저장", flms["StockAds"]);

        //            sd.Close();
        //            sd = null;

        //            Params.Clear();
        //            Params = null;

        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("StockAds", LogType.ERROR, LogDir, ex.ToString(), "StockAds");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
        //    }
        //}

        //void stockAdKs_Received()
        //{
        //    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : stockAdKs_Received");
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("StockAdkS", LogType.INFORMATION, LogDir, "StockAdkS 신호 수신", flms["StockAdkS"]);

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            object[] objArry;
        //            for (int i = 0; i < 7; i++)
        //            {
        //                object o = stockAdKs.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);
        //            }
        //            objArry = Params.ToArray();
        //            sd.SavekStockAdkS(objArry);
        //            //WriteMsg("StockAdkS", LogType.INFORMATION, LogDir, "저장", flms["StockAdkS"]);

        //            sd.Close();
        //            sd = null;

        //            Params.Clear();
        //            Params = null;

        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("StockAdkS", LogType.ERROR, LogDir, ex.ToString(), "StockAdkS");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
        //    }
        //}

        //void k200Expect_Received()
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            WriteMsg("K200Expect", LogType.INFORMATION, LogDir, "K200Expect 신호 수신");

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            for (int i = 0; i < 12; i++)
        //            {
        //                object o = k200Expect.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);

        //                WriteMsg("K200Expect", LogType.INFORMATION, LogDir
        //                    , string.Format("{0}\t{1}", i.ToString(), o.ToString()));
        //            }
        //            sd.Savek200Expect(Params.ToArray());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteMsg("K200Expect", LogType.ERROR, LogDir, ex.ToString());
        //    }
        //}

        //void expectIndexS_Received()
        //{
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            WriteMsg("ExpectIndexS", LogType.INFORMATION, LogDir, "ExpectIndexS 신호 수신");

        //            SqlDal sd = new SqlDal(dbConnString);
        //            List<object> Params = new List<object>();
        //            for (int i = 1; i < 7; i++)
        //            {
        //                object o = expectIndexS.GetHeaderValue(i);
        //                if (o == null) o = "0";
        //                Params.Add(o);

        //                WriteMsg("ExpectIndexS", LogType.INFORMATION, LogDir
        //                    , string.Format("{0}\t{1}", i.ToString(), o.ToString()));
        //            }
        //            sd.SaveExpectIndexS(Params.ToArray());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteMsg("ExpectIndexS", LogType.ERROR, LogDir, ex.ToString());
        //    }
        //}

        //void cpNew7221s_Received()
        //{
        //    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cpNew7221s_Received");
        //    //throw new NotImplementedException();
        //    try
        //    {
        //        if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
        //        {
        //            //WriteMsg("CpSvrNew7221S", LogType.INFORMATION, LogDir, "CpSvrNew7221S 신호 수신", flms["CpSvrNew7221S"]);
        //            SqlDal sd = new SqlDal(dbConnString);
        //            object MarketType = cpNew7221s.GetHeaderValue(0);
        //            object time = cpNew7221s.GetHeaderValue(1);
        //            object count = cpNew7221s.GetHeaderValue(2);

        //            List<object> Params = new List<object>();
        //            object[] objArry;
        //            for (int j = 0; j < int.Parse(count.ToString()); j++)
        //            {
        //                Params.Add(MarketType);
        //                Params.Add(time);
        //                Params.Add(j);

        //                for (int i = 0; i < 4; i++)
        //                {
        //                    object o = cpNew7221s.GetDataValue(i, j);
        //                    if (o == null) o = "0";
        //                    Params.Add(o);
        //                }
        //                objArry = Params.ToArray();
        //                sd.SaveCpSvrNew7221S(objArry);

        //                Params.Clear();
        //            }
        //            //WriteMsg("CpSvrNew7221S", LogType.INFORMATION, LogDir, "저장", flms["CpSvrNew7221S"]);

        //            sd.Close();
        //            sd = null;

        //            Params = null;
        //            objArry = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteMsg("CpSvrNew7221S", LogType.ERROR, LogDir, ex.ToString(),"CpSvrNew7221S");
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
        //    }
        //}

        void cp8111s_Received()
        {
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp8111s_Received");
            //throw new NotImplementedException();
            try
            {
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    //WriteMsg("CpSvr8111S", LogType.INFORMATION, LogDir, "CpSvr8111S 신호 수신", flms["CpSvr8111S"]);
                    SqlDal sd = new SqlDal(dbConnString);
                    List<object> Params = new List<object>();
                    object[] objArry;
                    for (int i = 0; i < 56; i++)
                    {
                        object o = cp8111s.GetHeaderValue(i);
                        if (o == null) o = "0";
                        Params.Add(o);
                    }
                    objArry = Params.ToArray();
                    sd.SaveCpSvr8111S(objArry);

                    //WriteMsg("CpSvr8111S", LogType.INFORMATION, LogDir, "저장", flms["CpSvr8111S"]);

                    sd.Close();
                    sd = null;

                    Params.Clear();
                    Params = null;

                    objArry = null;
                }
            }
            catch (Exception ex)
            {
                //WriteMsg("CpSvr8111S", LogType.ERROR, LogDir, ex.ToString(), "CpSvr8111S");
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
            }
        }

        void cp8111ks_Received()
        {
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp8111ks_Received");
            //throw new NotImplementedException();
            try
            {
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    //WriteMsg("CpSvr8111KS", LogType.INFORMATION, LogDir, "CpSvr8111KS 신호 수신", flms["CpSvr8111KS"]);

                    SqlDal sd = new SqlDal(dbConnString);
                    List<object> Params = new List<object>();
                    object[] objArry;
                    for (int i = 0; i < 56; i++)
                    {
                        object o = cp8111ks.GetHeaderValue(i);
                        if (o == null) o = "0";
                        Params.Add(o);
                    }
                    objArry = Params.ToArray();
                    sd.SaveCpSvr8111KS(objArry);
                    //WriteMsg("CpSvr8111KS", LogType.INFORMATION, LogDir, "저장", flms["CpSvr8111KS"]);

                    sd.Close();
                    sd = null;

                    Params.Clear();
                    Params = null;

                    objArry = null;
                }
            }
            catch (Exception ex)
            {
                //WriteMsg("CpSvr8111KS", LogType.ERROR, LogDir, ex.ToString(), "CpSvr8111KS");
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
            }
        }

        void cp8091s_Received()
        {
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cp8091s_Received");
            //throw new NotImplementedException();
            try
            {
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    //WriteMsg("CpSvr8091S", LogType.INFORMATION, LogDir, "CpSvr8091S 신호 수신", flms["CpSvr8091S"]);

                    SqlDal sd = new SqlDal(dbConnString);
                    List<object> Params = new List<object>();
                    object[] objArry;
                    for (int i = 0; i < 9; i++)
                    {
                        object o = cp8091s.GetHeaderValue(i);
                        if (o == null) o = "0";
                        Params.Add(o);
                    }
                    objArry = Params.ToArray();
                    sd.SaveCpSvr8091S(objArry);
                    //WriteMsg("CpSvr8091S", LogType.INFORMATION, LogDir, "저장", flms["CpSvr8091S"]);

                    sd.Close();
                    sd = null;

                    Params.Clear();
                    Params = null;

                    objArry = null;
                }
            }
            catch (Exception ex)
            {
                //WriteMsg("CpSvr8091S", LogType.ERROR, LogDir, ex.ToString(), "CpSvr8091S");
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
            }
        }


        void cpMarketWatchS_Received()
        {
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : cpMarketWatchS_Received");
            //throw new NotImplementedException();
            try
            {
                if (DateTime.Now >= sDateTime && DateTime.Now <= eDateTime)
                {
                    //WriteMsg("CpMarketWatchS", LogType.INFORMATION, LogDir, "CpMarketWatchS 신호 수신", flms["CpMarketWatchS"]);

                    object StockCode = cpMarketWatchS.GetHeaderValue(0);
                    object StockName = cpMarketWatchS.GetHeaderValue(1);
                    object count = cpMarketWatchS.GetHeaderValue(2);

                    SqlDal sd = new SqlDal(dbConnString);
                    List<object> Params = new List<object>();
                    object[] objArry;
                    for (int i = 0; i < int.Parse(count.ToString()); i++)
                    {
                        Params.Add(StockCode);
                        Params.Add(StockName);
                        Params.Add(cpMarketWatchS.GetDataValue(0, i));
                        Params.Add(cpMarketWatchS.GetDataValue(1, i));
                        Params.Add(cpMarketWatchS.GetDataValue(2, i));

                        objArry = Params.ToArray();
                        sd.SaveCpMarketWatchS(objArry);

                        Params.Clear();
                    }
                    //WriteMsg("CpMarketWatchS", LogType.INFORMATION, LogDir, "저장", flms["CpMarketWatchS"]);

                    sd.Close();
                    sd = null;

                    Params = null;
                    objArry = null;
                }
            }
            catch (Exception ex)
            {
                //WriteMsg("CpMarketWatchS", LogType.ERROR, LogDir, ex.ToString(), "CpMarketWatchS");
                EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString());
            }
        }

        //public void WriteMsg(string Prefix, LogType logType, string LogDir, string msg, string logName)
        //{
        //    try
        //    {
        //        //FileLog flm = new FileLog(string.Format("[{0}]", Prefix), LogDir, LogFileInteval.HOURLY);
        //        if (flms.ContainsKey(logName))
        //            flms[logName].write(logType, msg);
        //        else
        //        {
        //            flms.Add(logName, new FileLog(string.Format("[{0}]", logName), LogDir, LogFileInteval.HOURLY));
        //            flms[logName].write(logType, msg);
        //        }
        //        //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("SubscribeStocksRunner", ex.ToString());
        //    }
        //}
    }
}
