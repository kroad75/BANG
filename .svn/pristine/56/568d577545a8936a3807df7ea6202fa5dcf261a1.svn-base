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
    public class gini_CPMARKETWATCHS
    {
        List<string> cStockCode;
        string cConnString;
        string gMarketWatchCode = string.Empty;

        public string MARKETWATCHCODE
        {
            get
            {
                return gMarketWatchCode;
            }
        }

        public gini_CPMARKETWATCHS(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_CPMARKETWATCHS(string aConnString, List<string> aStockCode)
        {
            cStockCode = aStockCode;
            cConnString = aConnString;
        }

        public void Process()
        {
            if (cStockCode.Count > 0)
            {
                foreach (string sCode in cStockCode)
                {
                    //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 확인", sCode));
                    Thread t = new Thread(this.ChecknBuy);
                    t.Start(sCode);
                    //t.Start("A000660");
                }
            }

        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestCpMarketWatchS(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (HasStock(aStockCode))
                    {
                        // 해당 종목 보유시에는 구매 Timing 이 아님
                        retbool = false;
                    }
                    else if (BuyCheck(dsCur))
                    {
                        retbool = true;
                    }
                    else
                    {
                        retbool = false;
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPMARKETWATCHS-GoodTiming", e.ToString());
            }

            return retbool;
        }
        private void ChecknBuy(object aStockCode)
        {
            try
            {
                SqlDal sd = new SqlDal(cConnString);

                if (DateTime.Now <= sd.GetBuyLimitTime())
                {
                    string sCode = aStockCode.ToString();

                    DataSet dsCur = sd.GetLatestCpMarketWatchS(sCode);

                    if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(sCode))
                        {
                            ChecknSell(sCode);
                        }
                        else
                        {
                            if (BuyCheck(dsCur))
                            {
                                Do_Buy(sCode, dsCur.Tables[0].Rows[0]["MARKETWATCHCODE"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPMARKETWATCHS-ChecknBuy", e.ToString());
            }
        }

        private void ChecknSell(string sCode)
        {
            //throw new NotImplementedException();
            SqlDal sd = new SqlDal(cConnString);
            if (sd.SellAction(sCode) > 0)
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));
        }

        public void Do_Buy(string sCode, string WatchCode)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY05", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY05" + "-" + WatchCode);

        }

        public void Do_Buy(string sCode)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY05", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY05" + "-" + gMarketWatchCode);

        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur)
        {
            bool retBool = false;
            try
            {
                if (dsCur.Tables[0].Rows[0]["WorkType"].ToString() == "N")
                {
                    retBool = true;
                    this.gMarketWatchCode = dsCur.Tables[0].Rows[0]["MARKETWATCHCODE"].ToString();
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPMARKETWATCHS-BuyCheck", e.ToString());
            }
            return retBool;
        }
    }
}
