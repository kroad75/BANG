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
    public class gini_STOCKJPBID
    {
        List<string> cStockCode;
        string cConnString;

        public gini_STOCKJPBID(string aConnString)
        {
            cConnString = aConnString;
        }

        public gini_STOCKJPBID(string aConnString, List<string> aStockCode)
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
                DataSet dsCur = sd.GetLatestStockJpBid(aStockCode);
                DataSet dsBuy03 = sd.GetBUY03(aStockCode);

                if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                {
                    if (dsBuy03.Tables.Count > 0 && dsBuy03.Tables[0].Rows.Count > 0)
                    {
                        if (HasStock(aStockCode))
                        {
                            // 해당 종목 보유시에는 구매 Timing 이 아님
                            retbool = false;
                        }
                        else if (BuyCheck(dsCur, dsBuy03))
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
                EventLog.WriteEntry("Simulator-gini_STOCKJPBID-GoodTiming", e.ToString());
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

                    DataSet dsCur = sd.GetLatestStockJpBid(sCode);
                    DataSet dsBuy03 = sd.GetBUY03(sCode);

                    if (dsCur.Tables.Count > 0 && dsCur.Tables[0].Rows.Count > 0)
                    {
                        if (dsBuy03.Tables.Count > 0 && dsBuy03.Tables[0].Rows.Count > 0)
                        {
                            if (HasStock(sCode))
                            {
                                ChecknSell(sCode);
                            }
                            else
                            {
                                if (BuyCheck(dsCur, dsBuy03))
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
                EventLog.WriteEntry("Simulator-gini_STOCKJPBID-ChecknBuy", e.ToString());
            }
        }

        private void ChecknSell(string sCode)
        {
            //throw new NotImplementedException();
            SqlDal sd = new SqlDal(cConnString);
            if (sd.SellAction(sCode) > 0)
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매도", sCode));
        }

        public void Do_Buy(string sCode)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + " : " + string.Format("{0} 종목 매입 - BUY03", sCode));
            SqlDal sd = new SqlDal(cConnString);
            sd.BuyAction(sCode, "BUY03");

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

        private bool BuyCheck(DataSet dsCur, DataSet dsBuy03)
        {
            bool retBool = false;
            try
            {
                if (long.Parse(dsCur.Tables[0].Rows[0]["volume"].ToString())
                    > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_VOLUME"].ToString()))
                {
                    if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains1"].ToString())
                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS1"].ToString()))
                    {
                        if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains1"].ToString())
                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS1"].ToString()))
                        {
                            if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains2"].ToString())
                              < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS2"].ToString()))
                            {
                                if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains2"].ToString())
                                  > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS2"].ToString()))
                                {
                                    if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains3"].ToString())
                                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS3"].ToString()))
                                    {
                                        if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains3"].ToString())
                                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS3"].ToString()))
                                        {
                                            if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains4"].ToString())
                                              < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS4"].ToString()))
                                            {
                                                if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains4"].ToString())
                                                  > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS4"].ToString()))
                                                {
                                                    if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains5"].ToString())
                                                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS5"].ToString()))
                                                    {
                                                        if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains5"].ToString())
                                                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS5"].ToString()))
                                                        {
                                                            if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains6"].ToString())
                                                              < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS6"].ToString()))
                                                            {
                                                                if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains6"].ToString())
                                                                  > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS6"].ToString()))
                                                                {
                                                                    if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains7"].ToString())
                                                                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS7"].ToString()))
                                                                    {
                                                                        if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains7"].ToString())
                                                                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS7"].ToString()))
                                                                        {
                                                                            if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains8"].ToString())
                                                                              < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS8"].ToString()))
                                                                            {
                                                                                if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains8"].ToString())
                                                                                  > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS8"].ToString()))
                                                                                {
                                                                                    if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains9"].ToString())
                                                                                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS9"].ToString()))
                                                                                    {
                                                                                        if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains9"].ToString())
                                                                                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS9"].ToString()))
                                                                                        {
                                                                                            if (long.Parse(dsCur.Tables[0].Rows[0]["sellremains10"].ToString())
                                                                                              < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_SELLREMAINS10"].ToString()))
                                                                                            {
                                                                                                if (long.Parse(dsCur.Tables[0].Rows[0]["buyremains10"].ToString())
                                                                                                  > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_BUYREMAINS10"].ToString()))
                                                                                                {
                                                                                                    if (long.Parse(dsCur.Tables[0].Rows[0]["totalsellremains"].ToString())
                                                                                                      < long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_TOTALSELLREMAIN"].ToString()))
                                                                                                    {
                                                                                                        if (long.Parse(dsCur.Tables[0].Rows[0]["totalbuyremains"].ToString())
                                                                                                          > long.Parse(dsBuy03.Tables[0].Rows[0]["AVG_STOCKJPBID_TOTALBUYREMAINS"].ToString()))
                                                                                                        {
                                                                                                            retBool = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                EventLog.WriteEntry("Simulator-gini_STOCKJPBID-BuyCheck", e.ToString());
            }
            return retBool;
        }
    }
}
