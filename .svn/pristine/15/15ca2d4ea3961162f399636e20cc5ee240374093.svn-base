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
    public class gini_UP60MA
    {
        string cConnString;
        string _MAPERIOD = string.Empty;

        public string MAPERIOD
        {
            get
            {
                return _MAPERIOD;
            }
        }

        public gini_UP60MA(string aConnString)
        {
            cConnString = aConnString;
        }

        public bool isGoodTiming(string aStockCode)
        {
            bool retbool = false;

            try
            {
                SqlDal sd = new SqlDal(cConnString);
                DataSet dsMA = sd.GetMAData_Minute(aStockCode);

                if (dsMA.Tables.Count > 0 && dsMA.Tables[0].Rows.Count > 0)
                {
                    if (HasStock(aStockCode))
                    {
                        // 해당 종목 보유시에는 구매 Timing 이 아님
                        retbool = false;
                    }
                    else if (BuyCheck(dsMA))
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

        private bool BuyCheck(DataSet dsMA)
        {
            bool retBool = false;

            double currPrice = double.Parse(dsMA.Tables[0].Rows[0]["CURRENTPRICE"].ToString());
            double ma60Price = double.Parse(dsMA.Tables[0].Rows[0]["MA_60"].ToString());
            double ma20Price = double.Parse(dsMA.Tables[0].Rows[0]["MA_20"].ToString());

            if (currPrice > ma20Price)
            {
                retBool = true;
                _MAPERIOD = _MAPERIOD + "20";
            }

            if (currPrice > ma60Price)
            {
                retBool = true;
                _MAPERIOD = _MAPERIOD + "60";
            }

            return retBool;
        }
    }
}
