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
    public class gini_GRADIENT
    {
        string cConnString;
        StringBuilder _BIAS = new StringBuilder();

        public string BIAS
        {
            get
            {
                return _BIAS.ToString();
            }
        }

        public gini_GRADIENT(string aConnString)
        {
            cConnString = aConnString;
        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsMin = sd.GetCurrPriceByMin(aStockCode);
                DataSet dsDay = sd.GetCurrPriceByDay(aStockCode);

                if ((dsMin.Tables.Count > 0 && dsMin.Tables[0].Rows.Count > 0)
                    || (dsDay.Tables.Count > 0 && dsDay.Tables[0].Rows.Count > 0)
                  )
                {
                    if (HasStock(aStockCode))
                    {
                        // 해당 종목 보유시에는 구매 Timing 이 아님
                        retbool = false;
                    }
                    else if (BuyCheck(dsMin, dsDay))
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
                EventLog.WriteEntry("Simulator-gini_STOCKMST-GoodTiming", e.ToString());
            }

            return retbool;
        }

        private bool HasStock(string aStockCode)
        {
            SqlDal sd = new SqlDal(cConnString);
            return sd.HasStock(aStockCode);
        }

        private bool BuyCheck(DataSet dsMin, DataSet dsDay)
        {
            bool retBool = false;

            // Minute -1
            double a1 = CheckBias(dsMin.Tables[0], -1);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordMin(a1, "1");
            }

            // Minute -2
            a1 = CheckBias(dsMin.Tables[0], -2);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordMin(a1, "2");
            }

            // Minute -3
            a1 = CheckBias(dsMin.Tables[0], -3);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordMin(a1, "3");
            }


            // Day -7
            a1 = CheckBias(dsDay.Tables[0], -7);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "7");
            }

            // Day -14
            a1 = CheckBias(dsDay.Tables[0], -14);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "14");
            }

            // Day -30
            a1 = CheckBias(dsDay.Tables[0], -30);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "30");
            }

            // Day -60
            a1 = CheckBias(dsDay.Tables[0], -60);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "60");
            }

            // Day -90
            a1 = CheckBias(dsDay.Tables[0], -90);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "90");
            }

            // Day -180
            a1 = CheckBias(dsDay.Tables[0], -180);
            if (a1 > 0)
            {
                retBool = true;
                MakeBuyConditionWordDay(a1, "180");
            }

            return retBool;
        }

        private void MakeBuyConditionWordMin(double a1, string term)
        {
            this._BIAS.AppendFormat("M{0}B{1}", term, int.Parse(Math.Floor(a1).ToString()));
        }

        private void MakeBuyConditionWordDay(double a1, string term)
        {
            this._BIAS.AppendFormat("D{0}B{1}", term, int.Parse(Math.Floor(a1).ToString()));
        }

        private double CheckBias(DataTable dt, int term)
        {
            double retValue = -1;

            DataRow[] drs;
            double xi, yi;
            double sxi = 0, syi = 0, sxiyi = 0, sxi2 = 0;
            double a0, a1;

            //if (WorkType == "D")
            //{
            drs = dt.Select("WORK_DTTS > '" + DateTime.Now.AddDays(term).ToString("yyyy-MM-dd") + "'"
                , "WORK_DTTS");
            //}
            //else
            //{
            //    drs = dt.Select();
            //}

            int mCount = 0;
            foreach (DataRow dr in drs)
            {
                xi = mCount;
                yi = double.Parse(dr["currentprice"].ToString());

                sxi += xi;
                syi += yi;

                sxiyi += xi * yi;
                sxi2 += xi * xi;

                mCount++;
            }

            a0 = (sxi2 * syi - sxiyi * sxi) / (mCount * sxi2 - sxi * sxi);
            a1 = (mCount * sxiyi - sxi * syi) / (mCount * sxi2 - sxi * sxi);

            retValue = a1 / a0 * 100;

            return retValue;
        }
    }
}
