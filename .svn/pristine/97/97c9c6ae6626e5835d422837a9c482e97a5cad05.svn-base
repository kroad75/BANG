using BANG.Common.Log;
using BANG.CYBOSPLUS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BANG.CYBOSPLUS.Trader
{
    public class GeniusBaseSignal
    {
        string cConnString;
        string oldRawid;
        List<string> gStockCodeList;
        string gStockGroup;

        LOGWRITER LR;

        public GeniusBaseSignal(string aConnString, List<string> aStockCodeList, LOGWRITER lr, string aStockGroup)
        {
            cConnString = aConnString;
            gStockCodeList = aStockCodeList;
            gStockGroup = aStockGroup;

            if (lr != null)
                LR = new LOGWRITER(lr);
        }

        public string Process(string aOldRawid)
        {
            SqlDal sd = new SqlDal(cConnString);
            //List<string> lstStockCode = new List<string>();
            List<Thread> lstT = new List<Thread>();

            //lstStockCode = sd.GetAllStockCode();

            // 분봉 골드크로스/데드크로스 SIGNAL 발생처리// 
            foreach (string sCode in gStockCodeList)
            {
                Thread tMaMin = new Thread(CheckSignal_MA_Minute);
                tMaMin.Start(sCode);
                lstT.Add(tMaMin);
            }

            foreach (Thread sT in lstT)
            {
                sT.Join();
            }
            ///////////////////////////////////////////////

            lstT.Clear();

            // 일봉 골드크로스/데드크로스 SIGNAL 발생처리// 
            if (DateTime.Now.Hour == 9 && DateTime.Now.Minute == 20)
            {
                foreach (string sCode in gStockCodeList)
                {
                    Thread tMaMin = new Thread(CheckSignal_MA_Day);
                    tMaMin.Start(sCode);
                    lstT.Add(tMaMin);
                }

                foreach (Thread sT in lstT)
                {
                    sT.Join();
                }
            }
            ///////////////////////////////////////////////

            // 매수/매도 작업전에 현재 매수된 항목에 대한 매도이벤트 누락여부 검사 // 

            DataSet dsCheckSellItem = sd.GetCheckSellItemBeforeBuy(gStockGroup);
            if (dsCheckSellItem != null && dsCheckSellItem.Tables.Count > 0 && dsCheckSellItem.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCheckSellItem.Tables[0].Rows)
                {
                    string Rawid = dr["RAWID"].ToString();
                    string DateChar = dr["DateChar"].ToString();
                    string StockCode = dr["STOCKCODE"].ToString();
                    string BuyMethod = dr["buy_method"].ToString();

                    string TargetSellEvent = MakeSellWatchCode(BuyMethod);

                    if (sd.HasSellItem(DateChar, StockCode, TargetSellEvent))
                    {
                        sd.SellAction_OneByRawid(Rawid, StockCode);
                    }
                }
            }

            //////////////////////////////////////////////

            string retOldRawid = string.Empty;

            if (aOldRawid == null || aOldRawid.Length == 0)
                this.oldRawid = "0";
            else
                this.oldRawid = aOldRawid;

            DataSet dsCpWatch = sd.GetCpMarketWatch(oldRawid, gStockGroup);

            ChecknBuy(dsCpWatch);

            retOldRawid = getMaxRawid(dsCpWatch);
            return retOldRawid;
        }

        public void TEST_CheckSignal_MA_Minute(string aStockcode)
        {
            CheckSignal_MA_Minute(aStockcode);
        }

        private void CheckSignal_MA_Minute(object aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            string sCode = (string)aStockCode;

            try
            {
                //DataSet dsMA = sd.GetMAData_Minute(sCode);
                DataSet dsMA = sd.GetMAData_Minute_ALLNEW(sCode);

                //ProcessMA_Minute(sd, sCode, dsMA, "1");
                ProcessMA_Minute(sd, sCode, dsMA, "3");
                ProcessMA_Minute(sd, sCode, dsMA, "5");
                ProcessMA_Minute(sd, sCode, dsMA, "7");
                ProcessMA_Minute(sd, sCode, dsMA, "10");

                return;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("GeniusBaseSignal-CheckSignal_MA_Minute", e.ToString());
            }
        }

        private static void ProcessMA_Minute(SqlDal sd, string sCode, DataSet aDsMA, string aMaPeriod)
        {
            DataRow[] drs = aDsMA.Tables[0].Select("MA='" + aMaPeriod + "'");
            if (drs != null && drs.Length > 0)
            {
                string dTime = drs[0]["TIME"].ToString();
                double MA6 = 0;
                double OldMA6_1 = 0;
                double OldMA6_2 = 0;
                double OldMA6_3 = 0;
                double MA9 = 0;
                double OldMA9_1 = 0;
                double OldMA9_2 = 0;
                double OldMA9_3 = 0;
                double MA15 = 0;
                double OldMA15_1 = 0;
                double OldMA15_2 = 0;
                double OldMA15_3 = 0;
                double MA30 = 0;
                double OldMA30_1 = 0;
                double OldMA30_2 = 0;
                double OldMA30_3 = 0;
                //double MA60 = 0;
                //double OldMA60 = 0;

                if (!double.TryParse(drs[0]["MA_6"].ToString(), out MA6))
                    return;

                if (!double.TryParse(drs[1]["MA_6"].ToString(), out OldMA6_1))
                    return;

                if (!double.TryParse(drs[2]["MA_6"].ToString(), out OldMA6_2))
                    return;

                if (!double.TryParse(drs[3]["MA_6"].ToString(), out OldMA6_3))
                    return;

                if (!double.TryParse(drs[0]["MA_9"].ToString(), out MA9))
                    return;

                if (!double.TryParse(drs[1]["MA_9"].ToString(), out OldMA9_1))
                    return;

                if (!double.TryParse(drs[2]["MA_9"].ToString(), out OldMA9_2))
                    return;

                if (!double.TryParse(drs[3]["MA_9"].ToString(), out OldMA9_3))
                    return;

                if (!double.TryParse(drs[0]["MA_15"].ToString(), out MA15))
                    return;

                if (!double.TryParse(drs[1]["MA_15"].ToString(), out OldMA15_1))
                    return;

                if (!double.TryParse(drs[2]["MA_15"].ToString(), out OldMA15_2))
                    return;

                if (!double.TryParse(drs[3]["MA_15"].ToString(), out OldMA15_3))
                    return;

                if (!double.TryParse(drs[0]["MA_30"].ToString(), out MA30))
                    return;

                if (!double.TryParse(drs[1]["MA_30"].ToString(), out OldMA30_1))
                    return;

                if (!double.TryParse(drs[2]["MA_30"].ToString(), out OldMA30_2))
                    return;

                if (!double.TryParse(drs[3]["MA_30"].ToString(), out OldMA30_3))
                    return;

                //if (!double.TryParse(dsMA.Tables[0].Rows[0]["MA_60"].ToString(), out MA60))
                //    return;

                //if (!double.TryParse(dsMA.Tables[0].Rows[1]["MA_60"].ToString(), out OldMA60))
                //    return;

                if (MA6 > MA9 && OldMA6_1 > OldMA9_1 && OldMA6_2 > OldMA9_2)
                {
                    if (OldMA6_3 <= OldMA9_3)
                    {
                        if (MA6 >= MA9 && MA9 >= MA15 && MA15 >= MA30)// && MA6 > MA60)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "D011-" + aMaPeriod);
                        }
                        else if (MA6 > MA15 && MA6 > MA30)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "C011-" + aMaPeriod);
                        }
                    }
                }

                if (MA6 > MA15 && OldMA6_1 > OldMA15_1 && OldMA6_2 > OldMA15_2)
                {
                    if (OldMA6_3 <= OldMA15_3)
                    {
                        if (MA6 >= MA9 && MA9 >= MA15 && MA15 >= MA30)// && MA6 > MA60)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "D011-" + aMaPeriod);
                        }
                        else if (MA6 > MA9 && MA6 > MA30)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "C011-" + aMaPeriod);
                        }
                    }
                }

                if (MA6 > MA30 && OldMA6_1 > OldMA30_1 && OldMA6_2 > OldMA30_2)
                {
                    if (OldMA6_3 <= OldMA30_3)
                    {
                        if (MA6 >= MA9 && MA9 >= MA15 && MA15 >= MA30)// && MA6 > MA60)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "D011-" + aMaPeriod);
                        }
                        else if (MA6 > MA9 && MA6 > MA15)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "C011-" + aMaPeriod);
                        }
                    }
                }

                if (MA6 < MA9)
                {
                    if (OldMA6_1 >= OldMA9_1)
                    {
                        if (MA6 < MA15)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "C021-" + aMaPeriod);
                        }
                    }
                }

                if (MA6 < MA15)
                {
                    if (OldMA6_1 >= OldMA15_1)
                    {
                        if (MA6 < MA9)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "C021-" + aMaPeriod);
                        }
                    }
                }

                if (MA6 < MA30)
                {
                    if (OldMA6_1 >= OldMA30_1)
                    {
                        sd.SaveCpMarketWatchS(sCode, "", dTime, "C021-" + aMaPeriod);
                    }
                }
            }

            return;
        }

        public void TestCheckSignal_MA_Day(object aStockCode)
        {
            CheckSignal_MA_Day(aStockCode);
        }
        private void CheckSignal_MA_Day(object aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            string sCode = (string)aStockCode;

            try
            {
                DataSet dsMA = sd.GetMAData_Day(sCode);

                if (dsMA != null && dsMA.Tables.Count > 0 && dsMA.Tables[0].Rows.Count > 0)
                {
                    string dTime = dsMA.Tables[0].Rows[0]["TIME"].ToString();
                    double MA3 = 0;
                    double OldMA3 = 0;
                    double MA5 = 0;
                    double OldMA5 = 0;
                    double MA20 = 0;
                    double OldMA20 = 0;
                    double MA60 = 0;
                    double OldMA60 = 0;

                    if (!double.TryParse(dsMA.Tables[0].Rows[0]["MA_3"].ToString(), out MA3))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[1]["MA_3"].ToString(), out OldMA3))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[0]["MA_5"].ToString(), out MA5))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[1]["MA_5"].ToString(), out OldMA5))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[0]["MA_20"].ToString(), out MA20))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[1]["MA_20"].ToString(), out OldMA20))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[0]["MA_60"].ToString(), out MA60))
                        return;

                    if (!double.TryParse(dsMA.Tables[0].Rows[1]["MA_60"].ToString(), out OldMA60))
                        return;

                    if (MA5 > MA20)
                    {
                        if (OldMA5 <= OldMA20)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A03");
                        }
                    }
                    else if (MA5 < MA20)
                    {
                        if (OldMA5 >= OldMA20)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A04");
                        }
                    }

                    if (MA5 > MA60)
                    {
                        if (OldMA5 <= OldMA60)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A05");
                        }
                    }
                    else if (MA5 < MA60)
                    {
                        if (OldMA5 >= OldMA60)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A06");
                        }
                    }

                    if (MA3 > MA5)
                    {
                        if (OldMA3 <= OldMA5)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A07");
                        }
                    }
                    else if (MA3 < MA5)
                    {
                        if (OldMA3 >= OldMA5)
                        {
                            sd.SaveCpMarketWatchS(sCode, "", dTime, "A08");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("GeniusBaseSignal-CheckSignal_MA_Day", e.ToString());
            }
        }

        private string getMaxRawid(DataSet dsCpWatch)
        {
            //throw new NotImplementedException();
            string retMaxRawid = string.Empty;

            retMaxRawid = dsCpWatch.Tables[0].Compute("MAX(rawid)", "").ToString();

            return retMaxRawid;
        }

        private void ChecknBuy(DataSet aDsCpWatch)
        {
            DataSet dsRaw = aDsCpWatch;
            SqlDal sd = new SqlDal(cConnString);

            sd.SellAction_Signal_21_22();
            sd.SellAction_Signal_MinusLimit();

            DateTime BuyStartTime = sd.GetBuyStartTime();

            foreach (DataRow dr in dsRaw.Tables[0].Rows)
            {
                string StockCode = dr["STOCKCODE"].ToString();
                string WatchCode = dr["MARKETWATCHCODE"].ToString();

                if (WatchCode == "21" || WatchCode == "29" ||
                    WatchCode == "22" || WatchCode == "43" ||
                    WatchCode == "23" || WatchCode == "81" ||
                    WatchCode == "24" || WatchCode == "82"
                 )
                {
                    if (DateTime.Now >= BuyStartTime)
                    {
                        if (!sd.HasStock_Signal(StockCode, WatchCode))
                        {
                            sd.BuyAction(StockCode, WatchCode);
                        }
                    }
                }
                else
                {
                    if (WatchCode == "41" || WatchCode == "50" ||
                        WatchCode == "45" || WatchCode == "52" ||
                        WatchCode == "46" || WatchCode == "54" ||
                        WatchCode == "48" || WatchCode == "56" ||
                        WatchCode == "58" || WatchCode == "66" ||
                        WatchCode == "60" || WatchCode == "68" ||
                        WatchCode == "62" || WatchCode == "70" ||
                        WatchCode == "64" ||
                        WatchCode == "C011-1" || WatchCode == "C011-3" ||
                        WatchCode == "C011-5" || WatchCode == "C011-7" ||
                        WatchCode == "D011-1" || WatchCode == "D011-3" ||
                        WatchCode == "D011-5" || WatchCode == "D011-7" ||
                        WatchCode == "A03" || WatchCode == "A05" || WatchCode == "A07"
                 )
                    {
                        // Day 기준
                        if (WatchCode == "A03" || WatchCode == "A05" || WatchCode == "A07")
                        {
                            if (!sd.HasStock_Signal(StockCode, WatchCode))
                            {
                                sd.BuyAction(StockCode, WatchCode);
                            }
                        }
                        // 그외 (Min 기준)
                        else if (DateTime.Now >= BuyStartTime)
                        {
                            if (!sd.HasStock_Signal(StockCode, WatchCode))
                            {
                                sd.BuyAction(StockCode, WatchCode);
                            }
                        }
                    }
                    else
                    {
                        string TargetWatchCode = string.Empty;
                        if (WatchCode == "42")
                            TargetWatchCode = "41";
                        else if (WatchCode == "44")
                            TargetWatchCode = "45";
                        else if (WatchCode == "47")
                            TargetWatchCode = "46";
                        else if (WatchCode == "49")
                            TargetWatchCode = "48";
                        else if (WatchCode == "51")
                            TargetWatchCode = "50";
                        else if (WatchCode == "53")
                            TargetWatchCode = "52";
                        else if (WatchCode == "55")
                            TargetWatchCode = "54";
                        else if (WatchCode == "57")
                            TargetWatchCode = "56";
                        else if (WatchCode == "59")
                            TargetWatchCode = "58";
                        else if (WatchCode == "61")
                            TargetWatchCode = "60";
                        else if (WatchCode == "63")
                            TargetWatchCode = "62";
                        else if (WatchCode == "65")
                            TargetWatchCode = "64";
                        else if (WatchCode == "67")
                            TargetWatchCode = "66";
                        else if (WatchCode == "69")
                            TargetWatchCode = "68";
                        else if (WatchCode == "71")
                            TargetWatchCode = "70";
                        else if (WatchCode == "C021-1")
                            TargetWatchCode = "('C011-1','D011-1')";
                        else if (WatchCode == "C021-3")
                            TargetWatchCode = "('C011-3','D011-3')";
                        else if (WatchCode == "C021-5")
                            TargetWatchCode = "('C011-5','D011-5')";
                        else if (WatchCode == "C021-7")
                            TargetWatchCode = "('C011-7','D011-7')";
                        else if (WatchCode == "A04")
                            TargetWatchCode = "A03";
                        else if (WatchCode == "A06")
                            TargetWatchCode = "A05";
                        else if (WatchCode == "A08")
                            TargetWatchCode = "A07";

                        if (sd.HasStock_Signal(StockCode, TargetWatchCode))
                        {
                            sd.SellAction_Signal_41_42(StockCode, TargetWatchCode);
                        }
                    }
                }
            }
        }

        string MakeSellWatchCode(string aBuyMethod)
        {
            string retString = string.Empty;

            if (aBuyMethod == "41")
                retString = "42";
            else if (aBuyMethod == "45")
                retString = "44";
            else if (aBuyMethod == "46")
                retString = "47";
            else if (aBuyMethod == "48")
                retString = "49";
            else if (aBuyMethod == "50")
                retString = "51";
            else if (aBuyMethod == "52")
                retString = "53";
            else if (aBuyMethod == "54")
                retString = "55";
            else if (aBuyMethod == "56")
                retString = "57";
            else if (aBuyMethod == "58")
                retString = "59";
            else if (aBuyMethod == "60")
                retString = "61";
            else if (aBuyMethod == "62")
                retString = "63";
            else if (aBuyMethod == "64")
                retString = "65";
            else if (aBuyMethod == "66")
                retString = "67";
            else if (aBuyMethod == "68")
                retString = "69";
            else if (aBuyMethod == "70")
                retString = "71";
            else if (aBuyMethod == "C011-1" || aBuyMethod == "D011-1")
                retString = "C021-1";
            else if (aBuyMethod == "C011-3" || aBuyMethod == "D011-3")
                retString = "C021-3";
            else if (aBuyMethod == "C011-5" || aBuyMethod == "D011-5")
                retString = "C021-5";
            else if (aBuyMethod == "C011-7" || aBuyMethod == "D011-7")
                retString = "C021-7";
            else if (aBuyMethod == "A03")
                retString = "A04";
            else if (aBuyMethod == "A05")
                retString = "A06";
            else if (aBuyMethod == "A07")
                retString = "A08";

            return retString;
        }
    }
}
