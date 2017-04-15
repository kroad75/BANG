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
    public class gini_CPSVR7225
    {
        List<string> cStockCode;
        string cConnString;

        public gini_CPSVR7225(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_CPSVR7225(string aConnString, List<string> aStockCode)
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
                DataSet dsCur = sd.GetLatestCpSvr7225();
                DataSet dsBuy06 = sd.GetBUY06(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy06.Tables.Count > 0 && dsBuy06.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy06))
                        {
                            retbool = true;
                        }
                        else
                        {
                            retbool = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_CPSVR7225-GoodTiming", e.ToString());
            }

            return retbool;
        }
        private void ChecknBuy(object aStockCode)
        {
            try
            {
                string sCode = aStockCode.ToString();

                SqlDal sd = new SqlDal(cConnString);
                DataSet dsCur = sd.GetLatestCpSvr7225();
                DataSet dsBuy06 = sd.GetBUY06(sCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy06.Tables.Count > 0 && dsBuy06.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(sCode))
                        {
                            ChecknSell(sCode);
                        }
                        else
                        {
                            if (BuyCheck(dsCur, dsBuy06))
                            {
                                if (DateTime.Now <= sd.GetBuyLimitTime())
                                {
                                    Do_Buy(sCode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("gini_CPSVR7225-ChecknBuy", e.ToString());
            }
        }

        private void ChecknSell(string sCode)
        {
            //throw new NotImplementedException();
            SqlDal sd = new SqlDal(cConnString);

            //if (sd.SellAction(sCode) > 0)
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));

            sd.SellAction(sCode);
        }

        public void Do_Buy(string sCode)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY06", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY06");

        }

        public void Do_Buy(string sCode, string aBuyMethod)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - " + aBuyMethod, sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, aBuyMethod);

        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy06)
        {
            bool retBool = false;

            if (long.Parse(dsCur.Tables[0].Rows[0]["MARKETFRN"].ToString())
                > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_MARKETFRN"].ToString()))
            {
                if (long.Parse(dsCur.Tables[0].Rows[0]["MARKETGIGWAN"].ToString())
                    > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_MARKETGIGWAN"].ToString()))
                {
                    if (long.Parse(dsCur.Tables[0].Rows[0]["KOSDAQFRN"].ToString())
                        > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_KOSDAQFRN"].ToString()))
                    {
                        if (long.Parse(dsCur.Tables[0].Rows[0]["KOSDAQGIGWAN"].ToString())
                          > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_KOSDAQGIGWAN"].ToString()))
                        {
                            if (long.Parse(dsCur.Tables[0].Rows[0]["PROGRAMDIFF"].ToString())
                              > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_PROGRAMDIFF"].ToString()))
                            {
                                if (long.Parse(dsCur.Tables[0].Rows[0]["PROGRAMNONDIFF"].ToString())
                                  > long.Parse(dsBuy06.Tables[0].Rows[0]["AVG_CPSVR7225_PROGRAMNONDIFF"].ToString()))
                                {
                                    retBool = true;
                                }
                            }
                        }
                    }
                }
            }

            return retBool;
        }
    }
}
