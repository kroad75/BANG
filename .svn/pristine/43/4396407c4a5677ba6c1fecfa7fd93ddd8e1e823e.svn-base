using BANG.CYBOSPLUS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using BANG.Common.Log;

namespace BANG.CYBOSPLUS.Trader
{
    public delegate void LOGWRITER(LogType lt, string msg);

    public class Genius
    {
        List<string> cStockCode;
        string cConnString;

        LOGWRITER LR;

        public Genius(string aConnString, List<string> aStockCode, LOGWRITER lr)
        {
            cStockCode = aStockCode;
            cConnString = aConnString;

            if (lr != null)
                LR = new LOGWRITER(lr);

        }

        public Genius(string aConnString, List<string> aStockCode)
        {
            cStockCode = aStockCode;
            cConnString = aConnString;
        }

        public Genius(string aConnString, string aStockCode)
        {
            cStockCode = new List<string>();
            cStockCode.Add(aStockCode);
            cConnString = aConnString;
        }

        public void Process()
        {
            ChecknSellOverDay();

            if (cStockCode.Count > 0)
            {
                LR(LogType.INFORMATION, "Genius Process Start");

                foreach (string sCode in cStockCode)
                {
                    //Thread t = new Thread(this.ChecknBuy);
                    //t.Start(sCode);

                    // 멀티쓰레드 에서 싱글쓰레드로 변경
                    LR(LogType.INFORMATION, "ChecknBuy:" + sCode);
                    ChecknBuy(sCode);
                }

                //ChecknBuy("A001120");

                LR(LogType.INFORMATION, "Genius Process End");
            }

        }

        #region OldCode
        //public void Process(BuyDelegate dele)
        //{
        //    gDele = dele;
        //    if (cStockCode.Count > 0)
        //    {
        //        foreach (string sCode in cStockCode)
        //        {
        //            Thread t = new Thread(this.ChecknBuy);
        //            t.Start(sCode);
        //        }

        //        //ChecknBuy("A035080");
        //    }

        //}

        //void OrderSell()
        //{
        //    ChannelFactory<iCPORDER> factory = new ChannelFactory<iCPORDER>();
        //    string address = "net.tcp://localhost:8080/CPORDER";
        //    factory.Endpoint.Address = new EndpointAddress(address);
        //    factory.Endpoint.Binding = new NetTcpBinding();
        //    factory.Endpoint.Contract.ContractType = typeof(iCPORDER);
        //    iCPORDER channel = factory.CreateChannel();
        //    //channel.Buy("A011070",ORDER_TYPE.MARKETPRICE,10000,5000000);
        //    channel.Sell(2.0);
        //    ((ICommunicationObject)channel).Close();
        //}

        //void OrderBuy(string aStockCode, long currPrice)
        //{
        //    if (currPrice > 0)
        //    {
        //        ChannelFactory<iCPORDER> factory = new ChannelFactory<iCPORDER>();
        //        string address = "net.tcp://localhost:8080/CPORDER";
        //        factory.Endpoint.Address = new EndpointAddress(address);
        //        factory.Endpoint.Binding = new NetTcpBinding();
        //        factory.Endpoint.Contract.ContractType = typeof(iCPORDER);
        //        iCPORDER channel = factory.CreateChannel();
        //        channel.Buy(aStockCode, ORDER_TYPE.MARKETPRICE, currPrice, 1000000);
        //        //channel.Sell(2.0);
        //        ((ICommunicationObject)channel).Close();
        //    }
        //} 
        #endregion

        public void TestGenius(string aStockCode)
        {
            this.ChecknBuy(aStockCode);
        }

        private void ChecknBuy(object aStockCode)
        {
            try
            {
                //string[] MethodList = { "BUY01", "BUY02", "BUY03", "BUY04", "BUY05", "BUY06", "BUY08", "BUY21" };
                System.Collections.Generic.List<bool> judge = new List<bool>();
                string sCode = aStockCode.ToString();

                SqlDal sd = new SqlDal(cConnString);
                //if (sd.HasStock(sCode))
                {
                    //LR(LogType.INFORMATION, sCode + "/S1");
                    ChecknSell(sCode);
                }

                if (DateTime.Now >= sd.GetBuyStartTime() && DateTime.Now <= sd.GetBuyLimitTime())
                {
                    //LR(LogType.INFORMATION, sCode + "/S2");
                    //else
                    {
                        //int loopCount = (int)Math.Pow((double)2, (double)MethodList.Length);

                        gini_STOCKCUR gStockCur = new gini_STOCKCUR(cConnString);
                        //gini_STOCKINDEXIS gStockIndexis = new gini_STOCKINDEXIS(cConnString);
                        gini_STOCKMST gStockMst = new gini_STOCKMST(cConnString);
                        //gini_STOCKJPBID gStockJpBid = new gini_STOCKJPBID(cConnString);
                        gini_CPMARKETWATCHS gCpMarketWatchs = new gini_CPMARKETWATCHS(cConnString);
                        gini_CPSVR7225 gCpSvr7225 = new gini_CPSVR7225(cConnString);
                        gini_CPSVR8091S gCpSvr8091s = new gini_CPSVR8091S(cConnString);
                        gini_UP60MA gUp60Ma = new gini_UP60MA(cConnString);
                        //gini_CHECKSIGNAL gCheckSignal = new gini_CHECKSIGNAL(cConnString);
                        gini_GRADIENT gGradient = new gini_GRADIENT(cConnString);


                        judge.Add(gStockCur.isGoodTiming(sCode));
                        //judge.Add(gStockIndexis.isGoodTiming(sCode));
                        //judge.Add(gStockJpBid.isGoodTiming(sCode));
                        judge.Add(gStockMst.isGoodTiming(sCode));
                        judge.Add(gCpMarketWatchs.isGoodTiming(sCode));
                        judge.Add(gCpSvr7225.isGoodTiming(sCode));
                        judge.Add(gCpSvr8091s.isGoodTiming(sCode));
                        //judge.Add(gCheckSignal.isGoodTiming(sCode));

                        // 마지막에 수행
                        judge.Add(gUp60Ma.isGoodTiming(sCode));
                        judge.Add(gGradient.isGoodTiming(sCode));

                        StringBuilder sb = new StringBuilder();

                        if (judge[0]) sb.Append("BUY01,");
                        //if (judge[1]) sb.Append("BUY02,");
                        //if (judge[2]) sb.Append("BUY03,");
                        if (judge[1]) sb.Append("BUY04,");
                        if (judge[2]) sb.Append("BUY05-" + gCpMarketWatchs.MARKETWATCHCODE + ",");
                        if (judge[3]) sb.Append("BUY06,");
                        if (judge[4]) sb.Append("BUY08,");
                        //if (judge[5]) sb.Append("BUY15-" + gCheckSignal.SIGNAL + ",");

                        // 마지막에 수행
                        if (judge[5]) sb.Append("BUY21-" + gUp60Ma.MAPERIOD + ",");
                        if (judge[6]) sb.Append("BUY22-" + gGradient.BIAS + ",");

                        if (sb.Length > 0)
                        {
                            //LR(LogType.INFORMATION, sCode + "/S3/" + sb.ToString());
                            //if (!sd.GetHasTodayFailHistory(sCode, sb.ToString(0, sb.Length - 1)))
                            //{
                            // BUY21 은 단독 매수 조건이 못되고 보조 조건으로 사용
                            // 따라서 다른 조건은 만족하지 못하고 BUY21 만 만족할 경우에는 매수하지 않음
                            if (!sb.ToString().StartsWith("BUY21-")
                                && !sb.ToString().StartsWith("BUY22-")
                                )
                            {
                                if (sd.HasStockCODEnBUYMETHOD(sCode, sb.ToString(0, sb.Length - 1)))
                                    return;
                                //LR(LogType.INFORMATION, sCode + "/S4");
                                gStockMst.Do_Buy(sCode, sb.ToString(0, sb.Length - 1));
                                //LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, sb.ToString(0, sb.Length - 1)));
                            }
                            //}
                            //else
                            //{
                            //    LR(LogType.INFORMATION, string.Format("동일 종목의 매수조건으로 Fail 이력 존재함 : {0} {1}", sCode, sb.ToString(0, sb.Length - 1)));
                            //}
                        }
                    }

                    #region MyRegion
                    //for (int i = 1; i < loopCount; i++)
                    //{
                    //    switch (i)
                    //    {
                    //        case 1: // 00001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && !judge[4] && !judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockJpBid.Do_Buy(sCode);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03"));
                    //            }

                    //            break;

                    //        case 2: // 010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && !judge[4] && !judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04"));
                    //            }

                    //            break;

                    //        case 3: // 011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && !judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04"));
                    //            }

                    //            break;

                    //        case 4: // 100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && !judge[4] && !judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockCur.Do_Buy(sCode);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01"));
                    //            }

                    //            break;

                    //        case 5: // 101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && !judge[4] && !judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03"));
                    //            }

                    //            break;

                    //        case 6: // 110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && !judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04"));
                    //            }

                    //            break;

                    //        case 7: // 111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && !judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01.BUY03,BUY04"));
                    //            }

                    //            break;

                    //        case 8: // 1000
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && !judge[4] && judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gCpMarketWatchs.Do_Buy(sCode);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY05"));
                    //            }

                    //            break;

                    //        case 9: // 1001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && !judge[4] && judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 10: // 1010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && !judge[4] && judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 11: // 1011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 12: // 1100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && !judge[4] && judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 13: // 1101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && !judge[4] && judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 14: // 1110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 15: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && !judge[4] && judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE));
                    //            }

                    //            break;

                    //        case 16: // 001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && judge[4] && !judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gCpSvr7225.Do_Buy(sCode);
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY06"));
                    //            }

                    //            break;

                    //        case 17: // 010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && judge[4] && !judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY06"));
                    //            }

                    //            break;

                    //        case 18: // 011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && !judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY06"));
                    //            }

                    //            break;

                    //        case 19: // 100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && judge[4] && !judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY06"));
                    //            }

                    //            break;

                    //        case 20: // 101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && judge[4] && !judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY06"));
                    //            }

                    //            break;

                    //        case 21: // 110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && !judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY06"));
                    //            }

                    //            break;

                    //        case 22: // 111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && !judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01.BUY04,BUY06"));
                    //            }

                    //            break;

                    //        case 23: // 1000
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && judge[4] && !judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY04,BUY06"));
                    //            }

                    //            break;

                    //        case 24: // 1001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && judge[4] && judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 25: // 1010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && judge[4] && judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 26: // 1011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 27: // 1100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (!judge[5] && judge[4] && judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 28: // 1101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (!judge[5] && judge[4] && judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 29: // 1110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 30: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 31: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (!judge[5] && judge[4] && judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06"));
                    //            }

                    //            break;

                    //        case 32: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && !judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY08"));
                    //            }

                    //            break;

                    //        case 33: // 00001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && !judge[4] && !judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY08"));
                    //            }

                    //            break;

                    //        case 34: // 010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && !judge[4] && !judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY08"));
                    //            }

                    //            break;

                    //        case 35: // 011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && !judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY08"));
                    //            }

                    //            break;

                    //        case 36: // 100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && !judge[4] && !judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY08"));
                    //            }

                    //            break;

                    //        case 37: // 101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && !judge[4] && !judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY08"));
                    //            }

                    //            break;

                    //        case 38: // 110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && !judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04,BUY08"));
                    //            }

                    //            break;

                    //        case 39: // 111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && !judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01.BUY03,BUY04,BUY08"));
                    //            }

                    //            break;

                    //        case 40: // 1000
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && !judge[4] && judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY05,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY05,BUY08"));
                    //            }

                    //            break;

                    //        case 41: // 1001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && !judge[4] && judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 42: // 1010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && !judge[4] && judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 43: // 1011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 44: // 1100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && !judge[4] && judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 45: // 1101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && !judge[4] && judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 46: // 1110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 47: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && !judge[4] && judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY08"));
                    //            }

                    //            break;

                    //        case 48: // 001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && judge[4] && !judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 49: // 010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && judge[4] && !judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 50: // 011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && !judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 51: // 100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && judge[4] && !judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 52: // 101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && judge[4] && !judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 53: // 110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && !judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 54: // 111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && !judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01.BUY04,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 55: // 1000
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && judge[4] && !judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY04,BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 56: // 1001
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && judge[4] && judge[3] && !judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 57: // 1010
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && judge[4] && judge[3] && !judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 58: // 1011
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && judge[3] && !judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 59: // 1100
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "01");
                    //            if (judge[5] && judge[4] && judge[3] && !judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 60: // 1101
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "10");
                    //            if (judge[5] && judge[4] && judge[3] && judge[2] && !judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 61: // 1110
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && judge[3] && judge[2] && !judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY03,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 62: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && judge[3] && judge[2] && judge[1] && !judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;

                    //        case 63: // 1111
                    //            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + sCode + " : " + "11");
                    //            if (judge[5] && judge[4] && judge[3] && judge[2] && judge[1] && judge[0])
                    //            {
                    //                gStockMst.Do_Buy(sCode, "BUY01,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08");
                    //                LR(LogType.INFORMATION, string.Format("매수 {0} {1}", sCode, "BUY01,,BUY03,BUY04,BUY05" + "-" + gCpMarketWatchs.MARKETWATCHCODE + ",BUY06,BUY08"));
                    //            }

                    //            break;
                    //    }
                    //} 
                    #endregion
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Genius-ChecknBuy", e.ToString());
            }
        }

        private void ChecknSell(string sCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            if (sd.SellAction(sCode) > 0)
                LR(LogType.INFORMATION, string.Format("{0} 종목 매도", sCode));
        }

        private void ChecknSellOverDay()
        {
            SqlDal sd = new SqlDal(cConnString);
            if (sd.SellActionOverDay() > 0)
                LR(LogType.INFORMATION, "OverDay 종목 매도");
        }
    }
}
