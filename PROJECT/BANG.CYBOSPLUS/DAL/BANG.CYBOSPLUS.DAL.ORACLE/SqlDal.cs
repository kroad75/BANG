using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BANG.CYBOSPLUS.DAL
{
    public class SqlDal
    {
        string dbConnStr = null;
        Microsoft.Practices.EnterpriseLibrary.Data.Oracle.OracleDatabase db = null;

        public SqlDal(string dbConnStr)
        {
            try
            {
                this.dbConnStr = dbConnStr;
                db = new Microsoft.Practices.EnterpriseLibrary.Data.Oracle.OracleDatabase(dbConnStr);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Close()
        {
            db = null;
        }

        public DataSet Test()
        {
            DataSet retDS = new DataSet();

            return retDS;
        }

        public void SaveOrderHst(Object[] Params)
        {
            string sql = string.Empty;
            try
            {
                sql = string.Format("INSERT INTO cybosplus.ORDER_HST (RAWID, WORK_DTTS, ACCOUNT_NAME, STOCK_NAME, VOLUME, PRICE, ORDER_NO, ONE_ORDER_NO, ACCOUNT_NO, GOODCODE, STOCKCODE, TRADECODE, CONTRACTCODE, CREDITCODE, MODIFYCODE, CASHCODE, CALLPRICECODE, ORDERCONDITIONCODE, DAICHULIL, JANGBUGA, MAXSELLVOLUME, JANGO ) VALUES ( cybosplus.ORDER_HST_SEQ.nextval, systimestamp,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString());

                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SaveOrderHst", e.ToString() + Environment.NewLine + sql);
                throw e;
            }
        }

        public void SaveCpMarketWatchS(string aStockcode, string aStockName, string aTime, string aMarketWatchCode)
        {
            string sql = string.Empty;
            try
            {
                sql = string.Format("INSERT INTO cybosplus.CPMARKETWATCHS (RAWID, STOCKCODE, STOCKNAME, TIME,WORKTYPE, MARKETWATCHCODE, WORK_DTTS ) VALUES ( cybosplus.CPMARKETWATCHS_RAWID_SEQ.nextval,'{0}','{1}','{2}','110','{3}', systimestamp ) "
                    , aStockcode, aStockName, aTime, aMarketWatchCode);

                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SaveOrderHst", e.ToString() + Environment.NewLine + sql);
                throw e;
            }
        }

        public int MarkSellFlag(string aRawid)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET SELLED    = 'Y' ");
                varname1.AppendFormat(" WHERE     rawid = {0} ", aRawid);

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int ResetBalance()
        {
            int retInt = 0;

            try
            {
                retInt = db.ExecuteNonQuery(
                    CommandType.StoredProcedure
                    , "RESET_BALANCE"
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", e.ToString());
                throw e;
            }

            return retInt;

        }

        public int MarkBuyFlag(string aRawid)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET ORDERED    = 'Y' ");
                varname1.AppendFormat(" WHERE     rawid = {0} ", aRawid);

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public bool isCanBuy(long money)
        {
            bool retbool = false;
            long bal = 0;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  value from CONFIG_MST_CYBOSPLUS ");
            varname1.Append("where   category='TRADER' and code='BALANCE' ");

            try
            {
                bal = long.Parse(db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                ).ToString());

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                bal = 0;
            }

            if (bal > money)
                retbool = true;
            else
                retbool = false;

            return retbool;
        }

        public bool isHoliday()
        {
            bool retbool = false;
            int bal = 0;
            string today = DateTime.Now.ToString("yyyyMMdd");

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) from HOLIDAY ");
            varname1.AppendFormat("where   SYSTEM='CP' and DAY='{0}' ", today);

            try
            {
                bal = int.Parse(db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                ).ToString());

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                bal = 0;
            }

            if (bal > 0)
                retbool = true;
            else
                retbool = false;

            return retbool;
        }

        public bool isOverTotalEarningRate()
        {
            bool retbool = false;
            DataSet dsTemp = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" select  stockcode,rate from earningrate_mst where   stockcode='TOTAL' ");

            try
            {
                dsTemp = db.ExecuteDataSet(CommandType.Text, varname1.ToString());
                if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                {
                    double trate = double.Parse(dsTemp.Tables[0].Rows[0]["RATE"].ToString());

                    StringBuilder varname2 = new StringBuilder();
                    varname2.Append(" select  value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='TOTALEARNINGRATE' ");
                    double setingRate = double.Parse(db.ExecuteScalar(CommandType.Text, varname2.ToString()).ToString());

                    if (setingRate < trate)
                        retbool = true;
                    else
                        retbool = false;
                }
                else
                    retbool = false;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                retbool = false;
            }

            return retbool;
        }

        public bool isOverStockEarningRate(string aStockcode)
        {
            bool retbool = false;
            DataSet dsTemp = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.AppendFormat(" select  stockcode,rate from earningrate_mst where   stockcode='{0}' ", aStockcode);

            try
            {
                dsTemp = db.ExecuteDataSet(CommandType.Text, varname1.ToString());
                if (dsTemp != null && dsTemp.Tables.Count > 0 & dsTemp.Tables[0].Rows.Count > 0)
                {
                    double trate = double.Parse(dsTemp.Tables[0].Rows[0]["RATE"].ToString());

                    StringBuilder varname2 = new StringBuilder();
                    varname2.Append(" select  value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='STOCKEARNINGRATE' ");
                    double setingRate = double.Parse(db.ExecuteScalar(CommandType.Text, varname2.ToString()).ToString());

                    if (setingRate < trate)
                        retbool = true;
                    else
                        retbool = false;
                }
                else
                    retbool = false;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                retbool = false;
            }

            return retbool;
        }

        public bool isDayBottom(string aStockcode)
        {
            bool retbool = false;
            int BottomCount = 0;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) from STOCKMST_HST_CYBOSPLUS \n");
            varname1.Append("where   work_dtts > sysdate-(select value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='DAYBOTTOMRANGE') \n");
            varname1.AppendFormat("    and stockcode = '{0}' \n", aStockcode);
            varname1.Append("    and daybottom>=currentprice \n");
            varname1.Append("    and (select value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='BUYDAYBOTTOM') = 'Y' \n");
            varname1.Append("    and currentprice != 0 ");

            try
            {
                BottomCount = int.Parse(db.ExecuteScalar(CommandType.Text, varname1.ToString()).ToString());

                if (BottomCount > 0)
                    retbool = true;
                else
                    retbool = false;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                retbool = false;
            }

            return retbool;
        }

        public bool isGoodStockStatus(string aStockcode)
        {
            bool retbool = false;
            int BottomCount = 0;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) from STOCKMST_HST_CYBOSPLUS \n");
            varname1.Append("where   work_dtts > sysdate-3 \n");
            varname1.AppendFormat("    and stockcode = '{0}' \n", aStockcode);
            varname1.Append("    and (select value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='CHECKSTOCKSTATUS') = 'Y' \n");
            varname1.Append("    and ( cb != '48' \n");
            varname1.Append("       or managementcode != '78' \n");
            varname1.Append("       or investmentwarning != '49' \n");
            varname1.Append("       or holdtrading != '78' \n");
            varname1.Append("       or badfaithnoticecode != '48' \n");
            varname1.Append("    )");

            try
            {
                BottomCount = int.Parse(db.ExecuteScalar(CommandType.Text, varname1.ToString()).ToString());

                if (BottomCount > 0)
                    retbool = false;
                else
                    retbool = true;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                retbool = false;
            }

            return retbool;
        }

        public bool isTradingToday(string aStockcode)
        {
            bool retbool = false;
            int BottomCount = 0;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) from STOCKCUR_HST_SUBSC_CYBOSPLUS \n");
            varname1.Append("where   work_dtts > sysdate-3/24 \n");
            varname1.AppendFormat("    and stockcode = '{0}' \n", aStockcode);

            try
            {
                BottomCount = int.Parse(db.ExecuteScalar(CommandType.Text, varname1.ToString()).ToString());

                if (BottomCount > 0)
                    retbool = true;
                else
                    retbool = false;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString() + Environment.NewLine + e.ToString());
                retbool = false;
            }

            return retbool;
        }

        public DataSet GetBuyItem()
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  a.rawid,a.stockcode,a.buy_point,a.buy_method,b.amount,a.volume \n");
            varname1.Append("from    TRADE_HST a join order_stock b on A.STOCKCODE=b.stockcode and A.BUY_METHOD=b.buymethod \n");
            varname1.Append("where   1=1 \n");
            varname1.Append("    and buy_dtts > (systimestamp - 1/24/60) \n");
            //varname1.Append("    and buy_dtts > (systimestamp - 1) \n");
            varname1.Append("    and (ORDERED is null or ORDERED != 'Y') \n");
            varname1.Append("    and volume is not null \n");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetCurrPriceByMin(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" select  A.WORK_DTTS, A.STOCKCODE, A.CURRENTPRICE  \n");
            varname1.Append(" from    STOCKMST_HST_CYBOSPLUS a                  \n");
            varname1.AppendFormat(" where   stockcode='{0}'                     \n", aStockCode);
            varname1.Append("         and   work_dtts > sysdate-3               \n");
            varname1.Append("         and   currentprice != 0                   \n");
            varname1.Append(" order   by rawid                                  \n");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetCurrPriceByDay(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  * \n");
            varname1.Append("from    ( \n");
            varname1.Append("    select  A.WORK_DTTS, A.STOCKCODE, A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.AppendFormat("    where   stockcode='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') = '1510' \n");
            varname1.Append("        and work_dtts > sysdate-180 \n");
            varname1.Append("    union all \n");
            varname1.Append("    select  A.WORK_DTTS, A.STOCKCODE, A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.AppendFormat("    where   rawid = (select max(rawid) from STOCKMST_HST_CYBOSPLUS where stockcode='{0}' and work_dtts > sysdate-3) \n", aStockCode);
            varname1.Append(") \n");
            varname1.Append("order   by work_dtts");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetAllDailyPrice(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TO_CHAR (work_dtts, 'yyyymmdd') today,MAX (currentprice) price \n");
            varname1.Append("    FROM STOCKMST_HST_CYBOSPLUS \n");
            varname1.AppendFormat("   WHERE     stockcode = '{0}' \n", aStockCode);
            varname1.Append("         AND TO_CHAR (work_dtts, 'hh24mi') >= '1501' \n");
            varname1.Append("         AND TO_CHAR (work_dtts, 'hh24mi') <= '1530' \n");
            varname1.Append("GROUP BY TO_CHAR (work_dtts, 'yyyymmdd') \n");
            varname1.Append("ORDER BY TO_CHAR (work_dtts, 'yyyymmdd')");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Minute(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("from ( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("    where   1=1 \n");
            varname1.Append("        and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("        and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            varname1.Append("        and mod(to_number(to_char(work_dtts,'mi')),5) = 0 \n");
            varname1.Append("    order   by A.WORK_DTTS desc \n");
            varname1.Append(") a");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Minute_3MIN(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("from ( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("    where   1=1 \n");
            varname1.Append("        and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("        and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            varname1.Append("        and mod(to_number(to_char(work_dtts,'mi')),3) = 0 \n");
            varname1.Append("    order   by A.WORK_DTTS desc \n");
            varname1.Append(") a");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Minute_1MIN(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("from ( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("    where   1=1 \n");
            varname1.Append("        and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("        and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            varname1.Append("    order   by A.WORK_DTTS desc \n");
            varname1.Append(") a");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Minute_ALLNEW(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  * from \n");
            varname1.Append("( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("            ,'5' MA \n");
            varname1.Append("    from ( \n");
            varname1.Append("        select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE ,rownum seq \n");
            varname1.Append("        from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("        where   1=1 \n");
            varname1.Append("            and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("      and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("            and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            //varname1.Append("            and mod(to_number(to_char(work_dtts,'mi')),5) = 0 \n");
            varname1.Append("        order   by A.WORK_DTTS desc \n");
            varname1.Append("    ) a where   mod(seq,5)=0 or seq=1 \n");
            varname1.Append(") \n");
            varname1.Append("where   rownum < 10 \n");
            varname1.Append("union all \n");
            varname1.Append("select  * from \n");
            varname1.Append("( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("            ,'3' MA \n");
            varname1.Append("    from ( \n");
            varname1.Append("        select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE ,rownum seq \n");
            varname1.Append("        from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("        where   1=1 \n");
            varname1.Append("            and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("      and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("            and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            //varname1.Append("            and mod(to_number(to_char(work_dtts,'mi')),3) = 0 \n");
            varname1.Append("        order   by A.WORK_DTTS desc \n");
            varname1.Append("    ) a where   mod(seq,3)=0 or seq=1 \n");
            varname1.Append(") \n");
            varname1.Append("where   rownum < 10 \n");
            varname1.Append("union all \n");
            varname1.Append("select  * from \n");
            varname1.Append("( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("            ,'10' MA \n");
            varname1.Append("    from ( \n");
            varname1.Append("        select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE ,rownum seq \n");
            varname1.Append("        from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("        where   1=1 \n");
            varname1.Append("            and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("      and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("            and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            //varname1.Append("            and mod(to_number(to_char(work_dtts,'mi')),10) = 0 \n");
            varname1.Append("        order   by A.WORK_DTTS desc \n");
            varname1.Append("    ) a where   mod(seq,10)=0 or seq=1 \n");
            varname1.Append(") \n");
            varname1.Append("where   rownum < 10 \n");
            varname1.Append("union all \n");
            varname1.Append("select  * from \n");
            varname1.Append("( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("            ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("            ,'7' MA \n");
            varname1.Append("    from ( \n");
            varname1.Append("        select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE ,rownum seq \n");
            varname1.Append("        from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("        where   1=1 \n");
            varname1.Append("            and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("      and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("            and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            //varname1.Append("            and mod(to_number(to_char(work_dtts,'mi')),7) = 0 \n");
            varname1.Append("        order   by A.WORK_DTTS desc \n");
            varname1.Append("    ) a where   mod(seq,7)=0 or seq=1 \n");
            varname1.Append(") \n");
            varname1.Append("where   rownum < 10");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Minute_7MIN(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  6 following  ),3) MA_6 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  9 following  ),3) MA_9 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 10 following  ),3) MA_10 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 15 following  ),3) MA_15 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 30 following  ),3) MA_30 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("from ( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("    where   1=1 \n");
            varname1.Append("        and A.WORK_DTTS > sysdate-3 \n");
            varname1.AppendFormat("        and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') between '0900' and '1500' \n");
            varname1.Append("        and mod(to_number(to_char(work_dtts,'mi')),7) = 0 \n");
            varname1.Append("    order   by A.WORK_DTTS desc \n");
            varname1.Append(") a");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetMAData_Day(string aStockCode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  3 following  ),3) MA_3 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and  5 following  ),3) MA_5 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 20 following  ),3) MA_20 \n");
            varname1.Append("        ,round(avg(currentprice) over (partition by stockcode order by work_dtts desc rows between 1 following  and 60 following  ),3) MA_60 \n");
            varname1.Append("from ( \n");
            varname1.Append("    select  A.WORK_DTTS,A.STOCKCODE,A.TIME,A.CURRENTPRICE \n");
            varname1.Append("    from    STOCKMST_HST_CYBOSPLUS a \n");
            varname1.Append("    where   1=1 \n");
            varname1.Append("        and A.WORK_DTTS > sysdate-90 \n");
            varname1.AppendFormat("        and A.STOCKCODE='{0}' \n", aStockCode);
            varname1.Append("        and to_char(work_dtts,'hh24mi') = '1500' \n");
            varname1.Append("    order   by A.WORK_DTTS desc \n");
            varname1.Append(") a");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetSellItem()
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  rawid,stockcode,volume,sell_point \n");
            varname1.Append("from    TRADE_HST \n");
            varname1.Append("where   1=1 \n");
            varname1.Append("    and sell_dtts > (systimestamp - 1/24/60) \n");
            //varname1.Append("    and buy_dtts > (systimestamp - 1) \n");
            varname1.Append("    and (SELLED is null or SELLED != 'Y') \n");
            varname1.Append("    and volume is not null \n");

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public DataSet GetPredicRawData(string aStockcode)
        {
            DataSet retDT;

            StringBuilder varname1 = new StringBuilder();
            varname1.AppendFormat("select  target_date,result from PREDICTION_RAWDATA where stockcode='{0}' order   by target_date \n", aStockcode);

            try
            {
                retDT = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDT;
        }

        public bool GetIsSideCar()
        {
            bool retBool = false;

            StringBuilder query = new StringBuilder();
            query.Append(" select  FNGETSIDECAR from dual ");

            try
            {
                string str = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()
                ).ToString();

                if (str.Trim().ToUpper() == "Y")
                    retBool = true;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retBool;
        }

        public bool GetIsSideCar2()
        {
            bool retBool = false;

            StringBuilder query = new StringBuilder();
            query.Append(" select  FNGETSIDECAR2 from dual ");

            try
            {
                string str = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()
                ).ToString();

                if (str.Trim().ToUpper() == "Y")
                    retBool = true;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retBool;
        }

        public DateTime GetBuyStartTime()
        {
            DateTime retDT;
            string retDB = string.Empty;

            StringBuilder query = new StringBuilder();
            query.Append(" select  value from cybosplus.config_mst_cybosplus where category='TRADER' and code='BUYSTARTTIME' ");

            try
            {
                retDB = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()
                ).ToString();

                retDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(retDB.Substring(0, 2)), int.Parse(retDB.Substring(2, 2)), 0);

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDT;
        }

        public DateTime GetBuyLimitTime()
        {
            DateTime retDT;
            string retDB = string.Empty;

            StringBuilder query = new StringBuilder();
            query.Append(" select  value from config_mst_cybosplus where category='TRADER' and code='BUYTIME' ");

            try
            {
                retDB = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()
                ).ToString();

                retDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(retDB.Substring(0, 2)), int.Parse(retDB.Substring(2, 2)), 0);

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDT;
        }

        public int ResetOrderStock()
        {
            int retInt = 0;

            try
            {
                retInt = db.ExecuteNonQuery(
                    CommandType.StoredProcedure
                    , "reset_orderstock"
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", "reset_orderstock");
                throw e;
            }

            return retInt;

        }

        public int SaveScoreMstHistory()
        {
            int retInt = 0;

            try
            {
                retInt = db.ExecuteNonQuery(
                    CommandType.StoredProcedure
                    , "CYBOSPLUS.SAVE_SCORE_MST_HISTORY"
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", "reset_orderstock");
                throw e;
            }

            return retInt;

        }

        public int PlusBalance(long money)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.AppendFormat("update  CONFIG_MST_CYBOSPLUS set value=value+{0} ", money);
                varname1.Append("       where  category='TRADER' and code='BALANCE' ");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int MinusBalance(long money)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.AppendFormat("update  CONFIG_MST_CYBOSPLUS set value=value-{0} ", money);
                varname1.Append("       where  category='TRADER' and code='BALANCE' ");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellAction1440()
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.Append("       sell_point   = FnCurPriceStockCur_StockCode(stockcode) , ");
                varname1.Append("       profit       = FnCurPriceStockCur_StockCode(stockcode) - buy_point, ");
                varname1.Append("       FINISHED = 'Y' ");
                //varname1.AppendFormat(" WHERE     stockcode = '{0}' ", aStockCode);
                varname1.Append(" WHERE     1=1 ");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.Append(" and buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='DAILYTRADINGRATE') <= FnCurPriceStockCur_StockCode(stockcode) ");
                varname1.Append(" and (instr(BUY_METHOD,'BUY')>0 or BUY_METHOD in ('21','22','23','24','29','43','81','82'))  ");
                //varname1.AppendFormat("       AND (   (buy_point * 0.983)  >= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                //varname1.AppendFormat("            OR (buy_point * 1.023) <= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                //varname1.Append("           )");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellActionIn0930()
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.Append("       sell_point   = FnCurPriceStockCur_StockCode(stockcode) , ");
                varname1.Append("       profit       = FnCurPriceStockCur_StockCode(stockcode) - buy_point, ");
                varname1.Append("       FINISHED = 'Y' ");
                varname1.Append(" WHERE     1=1 ");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.Append("       AND (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_PLUS') )  <= FnCurPriceStockCur_StockCode(stockcode) ");
                varname1.Append("       and instr(BUY_METHOD,'BUY')>0  ");
                //varname1.AppendFormat("            OR (buy_point * 1.023) <= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                //varname1.Append("           )");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellActionOverDay()
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.Append("       sell_point   = FnCurPriceStockCur_StockCode(stockcode) , ");
                varname1.Append("       profit       = FnCurPriceStockCur_StockCode(stockcode) - buy_point, ");
                varname1.Append("       FINISHED = 'Y' ");
                //varname1.AppendFormat(" WHERE     stockcode = '{0}' ", aStockCode);
                varname1.Append(" WHERE     1=1 ");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.Append(" and buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='DAILYTRADINGRATE') <= FnCurPriceStockCur_StockCode(stockcode) ");
                varname1.Append(" and buy_dtts + 6/24 < systimestamp ");
                varname1.Append(" and (instr(BUY_METHOD,'BUY')>0 or BUY_METHOD in ('21','22','23','24','29','43','81','82'))  ");
                //varname1.AppendFormat("       AND (   (buy_point * 0.983)  >= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                //varname1.AppendFormat("            OR (buy_point * 1.023) <= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                //varname1.Append("           )");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellAction(string aStockCode)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.AppendFormat("       sell_point   = FnCurPriceStockCur_StockCode('{0}') , ", aStockCode);
                varname1.AppendFormat("       profit       = FnCurPriceStockCur_StockCode('{0}') - buy_point, ", aStockCode);
                varname1.Append("       FINISHED = 'Y' ");
                varname1.AppendFormat(" WHERE     stockcode = '{0}' ", aStockCode);
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.AppendFormat("       AND (   (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_MINUS') )  >= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                varname1.AppendFormat("            OR (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_PLUS') ) <= FnCurPriceStockCur_StockCode('{0}') ", aStockCode);
                varname1.Append("           )");
                varname1.Append("       and instr(BUY_METHOD,'BUY')>0  ");
                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellAction_Signal_21_22()
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.Append("       sell_point   = FnCurPriceStockCur_StockCode(STOCKCODE) , ");
                varname1.Append("       profit       = FnCurPriceStockCur_StockCode(STOCKCODE) - buy_point, ");
                varname1.Append("       FINISHED = 'Y' ");
                varname1.Append(" WHERE     1=1");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.Append("       AND (   (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_MINUS_SIGNAL') )  >= FnCurPriceStockCur_StockCode(STOCKCODE) ");
                varname1.Append("            OR (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_PLUS_SIGNAL') ) <= FnCurPriceStockCur_StockCode(STOCKCODE) ");
                varname1.Append("           )");
                varname1.Append("       AND BUY_METHOD in ('21','22','23','24','29','43','81','82') ");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellAction_Signal_MinusLimit()
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.Append("       sell_point   = FnCurPriceStockCur_StockCode(STOCKCODE) , ");
                varname1.Append("       profit       = FnCurPriceStockCur_StockCode(STOCKCODE) - buy_point, ");
                varname1.Append("       FINISHED = 'Y' ");
                varname1.Append(" WHERE     1=1");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.Append("       AND (buy_point * (select to_number(value) from CONFIG_MST_CYBOSPLUS where category='TRADER' and code='SELLEARNINGRATE_MINUS_SIGNAL') )  >= FnCurPriceStockCur_StockCode(STOCKCODE) ");
                varname1.Append("       AND BUY_METHOD not in ('21','22','23','24','29','43','81','82','A03','A05','A07') ");
                varname1.Append("       AND instr(BUY_METHOD,'BUY') = 0 ");

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public int SellAction_Signal_41_42(string aStockCode, string aWatchCode)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.AppendFormat("       sell_point   = FnCurPriceStockCur_StockCode('{0}') , ", aStockCode);
                varname1.AppendFormat("       profit       = FnCurPriceStockCur_StockCode('{0}') - buy_point, ", aStockCode);
                varname1.Append("       FINISHED = 'Y' ");
                varname1.Append(" WHERE     1=1");
                varname1.Append("       AND FINISHED = 'N' ");

                if (aWatchCode.Contains(","))
                    varname1.AppendFormat("       AND BUY_METHOD in {0} ", aWatchCode);
                else
                    varname1.AppendFormat("       AND BUY_METHOD = '{0}' ", aWatchCode);

                varname1.AppendFormat("       AND STOCKCODE = '{0}' ", aStockCode);

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }
        public int SellAction_OneByRawid(string aRawid, string aStockCode)
        {
            int retInt = 0;
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("UPDATE TRADE_HST ");
                varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
                varname1.AppendFormat("       sell_point   = FnCurPriceStockCur_StockCode('{0}') , ", aStockCode);
                varname1.AppendFormat("       profit       = FnCurPriceStockCur_StockCode('{0}') - buy_point, ", aStockCode);
                varname1.Append("       FINISHED = 'Y' ");
                varname1.Append(" WHERE     1=1");
                varname1.Append("       AND FINISHED = 'N' ");
                varname1.AppendFormat("       AND rawid = '{0}' ", aRawid);

                retInt = db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retInt;

        }

        public string GetConfigValue(string aCategory, string aCode)
        {
            string retDS = string.Empty;

            StringBuilder query = new StringBuilder();
            query.AppendFormat(" select  value from config_mst_cybosplus where category='{0}' and code='{1}' ", aCategory, aCode);

            try
            {
                retDS = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()
                ).ToString();

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public void InsertTodayTradeSummary()
        {
            StringBuilder varname1 = new StringBuilder();

            try
            {
                varname1.Append("insert into TRADE_SUMMARY \n");
                varname1.Append("select  TRADE_SUMMARY_RAWID_SEQ.NEXTVAL \n");
                varname1.Append("        ,sysdate,stockcode,buy_method,succcount,failcount,total,rate \n");
                varname1.Append("from \n");
                varname1.Append("( \n");
                varname1.Append("    select  stockcode \n");
                varname1.Append("            ,buy_method \n");
                varname1.Append("            ,sum(succcount) succcount \n");
                varname1.Append("            ,sum(failcount) failcount \n");
                varname1.Append("            ,count(*)  total \n");
                varname1.Append("            ,sum(rate) rate \n");
                varname1.Append("    from    ( \n");
                varname1.Append("        select  rawid,stockcode,buy_method,buy_point,sell_point,round(((sell_point-buy_point)/buy_point*100)-0.3,2) rate \n");
                varname1.Append("                ,case when round(((sell_point-buy_point)/buy_point*100)-0.3,2) >= 1 then 1 else 0 end SuccCount \n");
                varname1.Append("                ,case when round(((sell_point-buy_point)/buy_point*100)-0.3,2) < 1 then 1 else 0 end failCount \n");
                varname1.Append("        from    trade_hst \n");
                varname1.Append("    ) \n");
                varname1.Append("    group   by stockcode,buy_method \n");
                varname1.Append(")");

                db.ExecuteNonQuery(
                    CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

        }

        public void BuyAction(string aStockCode, string aBuyMethod)
        {
            string SQL = string.Empty;
            try
            {
                SQL = "insert into cybosplus.TRADE_HST " +
                            "select  cybosplus.TRADE_HST_RAWID_SEQ.nextval " +
                            "        ,'" + aStockCode + "' " +
                            "        ,systimestamp " +
                            "        ,FnCurPriceStockCur_StockCode('" + aStockCode + "') " +
                            "        ,null " +
                            "        ,null " +
                    //"        ,500 " +
                            "        ,(select  round(amount/ FNCURPRICESTOCKCUR_STOCKCODE(stockcode),0)  from ORDER_STOCK where   stockcode='" + aStockCode + "' and buymethod='" + aBuyMethod + "') " +
                            "        ,null " +
                            "        ,'N' " +
                            "        ,'" + aBuyMethod + "' " +
                            "        ,'N','N' " +
                            "from dual ";

                db.ExecuteNonQuery(
                    CommandType.Text
                    , SQL
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", SQL);
                throw e;
            }

        }

        //public void BuyAction_Signal(string aStockCode, string aBuyMethod)
        //{
        //    string SQL = string.Empty;
        //    try
        //    {
        //        SQL = "insert into cybosplus.TRADE_HST_SIGNAL " +
        //                    "select  cybosplus.TRADE_HST_SIGNAL_RAWID_SEQ.nextval " +
        //                    "        ,'" + aStockCode + "' " +
        //                    "        ,systimestamp " +
        //                    "        ,FnCurPriceStockCur_StockCode('" + aStockCode + "') " +
        //                    "        ,null " +
        //                    "        ,null " +
        //            //"        ,500 " +
        //                    "        ,null " +
        //                    "        ,null " +
        //                    "        ,'N' " +
        //                    "        ,'" + aBuyMethod + "' " +
        //                    "        ,'N','N' " +
        //                    "from dual ";

        //        db.ExecuteNonQuery(
        //            CommandType.Text
        //            , SQL
        //        );

        //    }
        //    catch (Exception e)
        //    {
        //        EventLog.WriteEntry("SQLDAL", SQL);
        //        throw e;
        //    }

        //}

        public DataSet GetLatestCpMarketWatchS(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  stockcode,decode(worktype,'99','C','N') WorkType,marketwatchcode,work_dtts from CPMARKETWATCHS \n");
            varname1.Append("where   work_dtts > sysdate-1/24/60 \n");
            varname1.AppendFormat("    and stockcode='{0}' \n", aStockCode);
            varname1.Append("    and marketwatchcode in ('10','12','21','22','23','24','28','29','41','43','45','46','48','50','52','54','56','58','60','62','64','66','68','70','81','83') \n");
            varname1.Append("order   by work_dtts desc");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestStockJpBid(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  stockcode,volume \n");
            varname1.Append("        ,sellremains1,buyremains1 \n");
            varname1.Append("        ,sellremains2,buyremains2 \n");
            varname1.Append("        ,sellremains3,buyremains3 \n");
            varname1.Append("        ,sellremains4,buyremains4 \n");
            varname1.Append("        ,sellremains5,buyremains5 \n");
            varname1.Append("        ,totalsellremains,totalbuyremains \n");
            varname1.Append("        ,sellremains6,buyremains6 \n");
            varname1.Append("        ,sellremains7,buyremains7 \n");
            varname1.Append("        ,sellremains8,buyremains8 \n");
            varname1.Append("        ,sellremains9,buyremains9 \n");
            varname1.Append("        ,sellremains10,buyremains10 \n");
            varname1.Append("from    STOCKJPBID_HST_CYBOSPLUS \n");
            varname1.AppendFormat("where   stockcode='{0}' \n", aStockCode);
            varname1.AppendFormat("    and work_dtts = (select max(work_dtts) from STOCKJPBID_HST_CYBOSPLUS where stockcode='{0}' and work_Dtts > sysdate-7)", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestCpSvr7225()
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  a.MARKETFRN,a.MARKETGIGWAN,A.KOSDAQFRN,A.KOSDAQGIGWAN,A.PROGRAMDIFF,A.PROGRAMNONDIFF,A.PREFRN,A.PREGIGWAN,A.OPTIONFRN,A.OPTIONGIGWAN ");
            query.Append(" from    CYBOSPLUS.CPSVR7225 a  ");
            query.Append(" where   rawid=(select max(rawid) from CPSVR7225 where work_Dtts > sysdate-7) ");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestCpSvr8091S(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  stockcode,time,sum(onlybuy) SUM_ONLYBUY ,sum(frnonly) SUM_FRNONLY ");
            query.Append(" from    cpsvr8091s a  ");
            query.AppendFormat(" where   stockcode = '{0}' ", aStockCode);
            query.AppendFormat(" and work_dtts = (select max(work_Dtts) from cpsvr8091s where stockcode='{0}' and work_dtts > sysdate-7) ", aStockCode);
            query.Append(" group   by stockcode,time ");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestStockMst(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  a.stockcode,a.stockname,A.DIFFYESTERDAY,A.FOREIGNERSELLTOTAL,A.FOREIGNERBUYTOTAL ");
            query.Append(" from    CYBOSPLUS.STOCKMST_HST_CYBOSPLUS a  ");
            query.AppendFormat(" where   stockcode = '{0}' ", aStockCode);
            query.AppendFormat(" and work_dtts = (select max(work_dtts) from cybosplus.STOCKMST_HST_CYBOSPLUS where stockcode='{0}' and work_Dtts > sysdate-7)", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestStockIndexIS(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append("select  A.INDUSTRYCODE, a.index_ IDX,a.DIFFYESTERDAY DIFF,a.VOLUME VOL,a.MONEY MONEY \n");
            query.Append("from    CYBOSPLUS.STOCKINDEXIS_HST_CYBOSPLUS a \n");
            query.Append("where   1=1 \n");
            query.Append("    and (industrycode,work_dtts) in \n");
            query.Append("       ( \n");
            query.Append("        SELECT industrycode,MAX (work_dtts) \n");
            query.Append("        FROM cybosplus.STOCKINDEXIS_HST_CYBOSPLUS \n");
            query.Append("        WHERE 1=1 \n");
            query.Append("            AND work_Dtts > SYSDATE - 7 \n");
            query.Append("        group by industrycode \n");
            query.Append("       ) \n");
            query.Append("order   by industrycode ");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public long GetCurPriceStockCur(string aStockCode)
        {
            long retDS = -1;

            StringBuilder query = new StringBuilder();
            query.Append(" select  currprice ");
            query.Append(" from    cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS ");
            query.AppendFormat(" where   stockcode = '{0}' ", aStockCode);
            query.AppendFormat(" and work_dtts = (select max(work_dtts) from cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS where stockcode='{0}' and work_Dtts > sysdate-7)", aStockCode);

            try
            {
                retDS = long.Parse(db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , query.ToString()).ToString()
                );


            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetLatestStockCur(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  stockcode,stockname,currprice,stackedvolume,stackedmoney,stackedsellvolume,stackedbuyvolume ");
            query.Append(" from    cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS ");
            query.AppendFormat(" where   stockcode = '{0}' ", aStockCode);
            query.AppendFormat(" and work_dtts = (select max(work_dtts) from cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS where stockcode='{0}' and work_Dtts > sysdate-7)", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY03(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from cybosplus.BUY03_STOCKJPBID ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = ");
            query.Append(" (   select max(workdate) from cybosplus.BUY03_STOCKJPBID ");
            query.Append(" where to_date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetTradingStatus(string aStartDatetime, string aEndStartDatetime, bool aFinished, bool aOnlyOrderStock)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select a.stockcode code,to_char(a.buy_dtts,'yyyy-mm-dd hh24:mi:ss') bdate,a.buy_point bpt,to_char(a.sell_dtts,'yyyy-mm-dd hh24:mi:ss') sdate ,a.finished f,a.buy_method bm,a.ordered buy,a.selled sell \n");
            varname1.Append("       ,(decode(sell_dtts,null,sysdate,sell_dtts) - buy_dtts) TERM \n");
            varname1.Append("       ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100 rate \n");
            varname1.Append("       ,buy_point + (buy_point * 0.023) TARGET \n");
            varname1.Append("       ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR \n");
            varname1.Append("from TRADE_HST a \n");
            varname1.Append("join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE \n");
            varname1.Append("where   1=1 \n");
            varname1.AppendFormat("    and (buy_dtts between to_date('{0}','yyyymmddhh24miss') and to_date('{1}','yyyymmddhh24miss') \n", aStartDatetime, aEndStartDatetime);
            varname1.AppendFormat("    or  sell_dtts between to_date('{0}','yyyymmddhh24miss') and to_date('{1}','yyyymmddhh24miss')) \n", aStartDatetime, aEndStartDatetime);

            if (aFinished)
                varname1.Append("    and finished='N' \n");
            //else
            //    varname1.Append("    and finished='N' \n");

            if (aOnlyOrderStock)
            {
                varname1.Append("    and (stockcode,buy_method) in (select stockcode,buymethod from order_stock) \n");
                varname1.Append("    and volume is not null");
            }

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetCpMarketWatch(string aOldRawid, string aStockGroup)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  * \n");
            varname1.Append("from    CPMARKETWATCHS \n");
            varname1.Append("where   1=1 \n");
            varname1.Append("    and work_dtts between to_timestamp(to_char(systimestamp,'yyyymmdd')||'000000','yyyymmddhh24miss') and to_timestamp(to_char(systimestamp,'yyyymmdd')||'235959','yyyymmddhh24miss') \n");
            //varname1.Append("    and work_dtts between to_timestamp('20150522'||'000000','yyyymmddhh24miss') and to_timestamp('20150522'||'235959','yyyymmddhh24miss') \n");
            varname1.Append("    and worktype = '110' \n");
            varname1.AppendFormat(" and rawid > {0} \n", aOldRawid);
            varname1.AppendFormat(" and stockcode in (select  value from    CONFIG_MST_CYBOSPLUS where   category='STOCK' and code='CODE'     and instr(desc_,'{0}')>0) ", aStockGroup);
            varname1.Append("order   by rawid");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetCheckSellItemBeforeBuy(string aStockGroup)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  a.rawid,a.stockcode,to_char(A.BUY_DTTS,'yyyymmddhh24miss') DateChar,a.buy_method \n");
            varname1.Append("from    TRADE_HST a \n");
            varname1.Append("where   finished='N' \n");
            varname1.Append("    and instr(buy_method,'BUY') = 0 \n");
            varname1.Append("    AND BUY_METHOD not in ('21','22','23','24','29','43','81','82') \n");
            varname1.AppendFormat(" and stockcode in (select  value from    CONFIG_MST_CYBOSPLUS where   category='STOCK' and code='CODE'     and instr(desc_,'{0}')>0) ", aStockGroup);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetCpMarketWatchByStockCode(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  * \n");
            varname1.Append("from    CPMARKETWATCHS \n");
            varname1.Append("where   1=1 \n");
            varname1.Append("    and work_dtts > sysdate-1/24/60 \n");
            varname1.Append("    and worktype = '110' \n");
            varname1.AppendFormat("    and stockcode = '{0}' \n", aStockCode);
            varname1.Append("order   by rawid");

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY08(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from BUY08_CPSVR8091S ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = (select max(workdate) from BUY08_CPSVR8091S where to_Date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY06(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from cybosplus.BUY06_CPSVR7225 ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = ");
            query.Append(" (   select max(workdate) from cybosplus.BUY06_CPSVR7225 ");
            query.Append(" where to_date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY04(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from cybosplus.BUY04_STOCKMST ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = ");
            query.Append(" (   select max(workdate) from cybosplus.BUY04_STOCKMST ");
            query.Append(" where to_date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY02(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from cybosplus.BUY02_STOCKINDEXIS ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = ");
            query.Append(" (   select max(workdate) from cybosplus.BUY02_STOCKINDEXIS ");
            query.Append(" where to_date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public DataSet GetBUY01(string aStockCode)
        {
            DataSet retDS = new DataSet();

            StringBuilder query = new StringBuilder();
            query.Append(" select  * from cybosplus.BUY01_STOCKCUR ");
            query.AppendFormat(" where   stockcode='{0}' ", aStockCode);
            query.Append(" and workdate = ");
            query.Append(" (   select max(workdate) from cybosplus.BUY01_STOCKCUR ");
            //query.Append(" where to_date(workdate,'yyyy-mm-dd') > sysdate-30 ");
            query.AppendFormat(" where  stockcode='{0}') ", aStockCode);
            //query.AppendFormat(" and stockcode='{0}') ", aStockCode);

            try
            {
                retDS = db.ExecuteDataSet(
                    System.Data.CommandType.Text
                    , query.ToString()
                );

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", query.ToString());
                throw e;
            }

            return retDS;
        }

        public bool HasStock(string aStockCode)
        {
            return false;
            //string retString = string.Empty;
            //bool retBool = false;
            //string SQL = "SELECT count(*) FROM cybosplus.TRADE_HST WHERE STOCKCODE = '" + aStockCode + "' and FINISHED='N' and instr(buy_method,'BUY')>0 ";
            //try
            //{
            //    retString = db.ExecuteScalar(
            //        System.Data.CommandType.Text
            //        , SQL
            //    ).ToString();

            //    if (int.Parse(retString) > 0)
            //    {
            //        retBool = true;
            //    }

            //}
            //catch (Exception e)
            //{
            //    EventLog.WriteEntry("SQLDAL", SQL);
            //    throw e;
            //}

            //return retBool;
        }

        public bool HasStockCODEnBUYMETHOD(string aStockCode, string aBuyMethod)
        {
            string retString = string.Empty;
            bool retBool = false;
            string SQL = "SELECT count(*) FROM cybosplus.TRADE_HST WHERE STOCKCODE = '" + aStockCode + "' and FINISHED='N' and buy_method = '" + aBuyMethod + "' ";
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , SQL
                ).ToString();

                if (int.Parse(retString) > 0)
                {
                    retBool = true;
                }

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", SQL);
                throw e;
            }

            return retBool;
        }

        public bool HasStock_Signal(string aStockCode, string aWatchCode)
        {
            string retString = string.Empty;
            bool retBool = false;
            string SQL = string.Empty;

            if (aWatchCode.Contains(","))
                SQL = "SELECT count(*) FROM cybosplus.TRADE_HST WHERE STOCKCODE = '" + aStockCode + "' and BUY_METHOD in " + aWatchCode + " and FINISHED='N' ";
            else
                SQL = "SELECT count(*) FROM cybosplus.TRADE_HST WHERE STOCKCODE = '" + aStockCode + "' and BUY_METHOD = '" + aWatchCode + "' and FINISHED='N' ";
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , SQL
                ).ToString();

                if (int.Parse(retString) > 0)
                {
                    retBool = true;
                }

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", SQL);
                throw e;
            }

            return retBool;
        }

        public bool HasSellItem(string aDateChar, string aStockcode, string aSellEvent)
        {
            string retString = string.Empty;
            bool retBool = false;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) from CPMARKETWATCHS \n");
            varname1.AppendFormat("where   work_dtts > to_date('{0}','yyyymmddhh24miss') \n", aDateChar);
            varname1.AppendFormat("    and stockcode='{0}' \n", aStockcode);
            varname1.Append("    and worktype='110' \n");
            varname1.AppendFormat("    and marketwatchcode='{0}' ", aSellEvent);

            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , varname1.ToString()
                ).ToString();

                if (int.Parse(retString) > 0)
                {
                    retBool = true;
                }

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retBool;
        }

        public bool GetisDailyTrading()
        {
            bool retbool = false;
            try
            {
                string retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'TRADER' AND CODE = 'DAILYTRADING' "
                ).ToString();

                if (retString.Trim().ToUpper() == "Y")
                    retbool = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", e.ToString());
            }

            return retbool;
        }

        public bool GetisStopMinusSellIn0930()
        {
            bool retbool = false;
            try
            {
                string retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'TRADER' AND CODE = 'STOPMINUSSELLIN0930' "
                ).ToString();

                if (retString.Trim().ToUpper() == "Y")
                    retbool = true;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", e.ToString());
            }

            return retbool;
        }

        public DateTime GetStartTime()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'DATACOLLECTOR' AND CODE = 'STARTTIME'"
                ).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return new DateTime(
                DateTime.Now.Year
                , DateTime.Now.Month
                , DateTime.Now.Day
                , int.Parse(retString.Substring(0, 2))
                , int.Parse(retString.Substring(2, 2))
                , int.Parse(retString.Substring(4, 2)));
        }

        public DateTime GetEndTime()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'DATACOLLECTOR' AND CODE = 'ENDTIME'"
                ).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return new DateTime(
                DateTime.Now.Year
                , DateTime.Now.Month
                , DateTime.Now.Day
                , int.Parse(retString.Substring(0, 2))
                , int.Parse(retString.Substring(2, 2))
                , int.Parse(retString.Substring(4, 2)));
        }

        public string GetSysdownTime()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'DATACOLLECTOR' AND CODE = 'SYSTEMDOWNTIME'"
                ).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public string GetLogDir()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(
                    System.Data.CommandType.Text
                    , "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'DATACOLLECTOR' AND CODE = 'LOGDIR'"
                ).ToString();


            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public bool GetHasTodayFailHistoryInSomeMin(string aStockCode, string aBuyMethod)
        {
            bool retBool = false;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  count(*) \n");
            varname1.Append("from    TRADE_HST a \n");
            varname1.Append("where   sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss') \n");
            varname1.Append("    and finished='Y' \n");
            varname1.Append("    and volume is not null \n");
            varname1.AppendFormat("    and stockcode='{0}' \n", aStockCode);
            varname1.AppendFormat("    and buy_method='{0}' \n", aBuyMethod);
            varname1.Append("    and profit < 0");
            varname1.Append("    and sell_dtts + (select value from CONFIG_MST_CYBOSPLUS where category='TRADER' and code = 'MINSTOPBUYAGAIN') / 1440 > systimestamp \n");

            try
            {
                int i = 0;
                i = int.Parse(db.ExecuteScalar(System.Data.CommandType.Text, varname1.ToString()).ToString());

                if (i > 0)
                    retBool = true;

            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SQLDAL", varname1.ToString());
                throw e;
            }

            return retBool;
        }

        public void SetRestartMe_DataCollector()
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , "Update cybosplus.CONFIG_MST_CYBOSPLUS  SET VALUE = '1' WHERE category = 'DATACOLLECTOR' AND CODE = 'RESTARTME'"
                );

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void SaveCp7225(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.CPSVR7225 ( rawid,TIME, MARKETFRN, MARKETGIGWAN, KOSDAQFRN, KOSDAQGIGWAN, PROGRAMDIFF, PROGRAMNONDIFF, PREFRN, PREGIGWAN, OPTIONFRN, OPTIONGIGWAN ) VALUES ( cybosplus.CPSVR7225_RAWID_SEQ.nextval,'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCp8081(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.CPSVR8081 (rawid, RANK, STOCKCODE, STOCKNAME, DIFFYESTERDAY, DIFFRATEYESTERDAY, CURRPRICE, TRADINGVOLUME, CONSTRONG1DAY, CONSTRONG5DAY, CONSTRONG20DAY, CONSTRONG59DAY ) VALUES (cybosplus.CPSVR8081_RAWID_SEQ.nextval,  '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpMarketWatchS(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.CPMARKETWATCHS (rawid, STOCKCODE, STOCKNAME, TIME, WORKTYPE, MARKETWATCHCODE ) VALUES (cybosplus.CPMARKETWATCHS_RAWID_SEQ.nextval, '{0}', '{1}', '{2}', '{3}', '{4}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpSvr8091S(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.CPSVR8091S ( rawid,TIME, MEMBERNAME, STOCKCODE, STOCKNAME, BUYSELL, BUYSELLVOLUME, ONLYBUY, BUYSIGNAL, FRNONLY ) VALUES ( cybosplus.CPSVR8091S_RAWID_SEQ.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpSvr8111KS(Object[] Params)
        {
            string sql = string.Empty;
            try
            {
                sql = string.Format("INSERT INTO cybosplus.CPSVR8111KS (rawid, DATE_, TIME, ITEM1, ITEM2, ITEM3, ITEM4, ITEM5, ITEM6, ITEM7, ITEM8, ITEM9, ITEM10, ITEM11, ITEM12, ITEM13, ITEM14, ITEM15, ITEM16, ITEM17, ITEM18, ITEM19, ITEM20, ITEM21, ITEM22, ITEM23, ITEM24, ITEM25, ITEM26, ITEM27, ITEM28, ITEM29, ITEM30, ITEM31, ITEM32, ITEM33, ITEM34, ITEM35, ITEM36, ITEM37, ITEM38, ITEM39, ITEM40, ITEM41, ITEM42, ITEM43, ITEM44, ITEM45, ITEM46, ITEM47, ITEM48, ITEM49, ITEM50, ITEM51, ITEM52, ITEM53, ITEM54  ) VALUES ( cybosplus.CPSVR8111KS_RAWID_SEQ.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}','{10}','{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}','{19}','{20}','{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}','{29}','{30}','{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}','{39}','{40}','{41}', '{42}', '{43}', '{44}', '{45}', '{46}', '{47}', '{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString(), Params[20].ToString(), Params[21].ToString(), Params[22].ToString(), Params[23].ToString(), Params[24].ToString(), Params[25].ToString(), Params[26].ToString(), Params[27].ToString(), Params[28].ToString(), Params[29].ToString(), Params[30].ToString(), Params[31].ToString(), Params[32].ToString(), Params[33].ToString(), Params[34].ToString(), Params[35].ToString(), Params[36].ToString(), Params[37].ToString(), Params[38].ToString(), Params[39].ToString(), Params[40].ToString(), Params[41].ToString(), Params[42].ToString(), Params[43].ToString(), Params[44].ToString(), Params[45].ToString(), Params[46].ToString(), Params[47].ToString(), Params[48].ToString(), Params[49].ToString(), Params[50].ToString(), Params[51].ToString(), Params[52].ToString(), Params[53].ToString(), Params[54].ToString(), Params[55].ToString());

                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SaveCpSvr8111KS", e.ToString() + Environment.NewLine + sql);
                throw e;
            }
        }

        public void SaveCpSvr8111S(Object[] Params)
        {
            string sql = string.Empty;
            try
            {
                sql = string.Format("INSERT INTO cybosplus.CPSVR8111S (rawid, DATE_, TIME, ITEM1, ITEM2, ITEM3, ITEM4, ITEM5, ITEM6, ITEM7, ITEM8, ITEM9, ITEM10, ITEM11, ITEM12, ITEM13, ITEM14, ITEM15, ITEM16, ITEM17, ITEM18, ITEM19, ITEM20, ITEM21, ITEM22, ITEM23, ITEM24, ITEM25, ITEM26, ITEM27, ITEM28, ITEM29, ITEM30, ITEM31, ITEM32, ITEM33, ITEM34, ITEM35, ITEM36, ITEM37, ITEM38, ITEM39, ITEM40, ITEM41, ITEM42, ITEM43, ITEM44, ITEM45, ITEM46, ITEM47, ITEM48, ITEM49, ITEM50, ITEM51, ITEM52, ITEM53, ITEM54  ) VALUES (cybosplus.CPSVR8111S_RAWID_SEQ.nextval,  '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}','{10}','{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}','{19}','{20}','{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}','{29}','{30}','{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}','{39}','{40}','{41}', '{42}', '{43}', '{44}', '{45}', '{46}', '{47}', '{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString(), Params[20].ToString(), Params[21].ToString(), Params[22].ToString(), Params[23].ToString(), Params[24].ToString(), Params[25].ToString(), Params[26].ToString(), Params[27].ToString(), Params[28].ToString(), Params[29].ToString(), Params[30].ToString(), Params[31].ToString(), Params[32].ToString(), Params[33].ToString(), Params[34].ToString(), Params[35].ToString(), Params[36].ToString(), Params[37].ToString(), Params[38].ToString(), Params[39].ToString(), Params[40].ToString(), Params[41].ToString(), Params[42].ToString(), Params[43].ToString(), Params[44].ToString(), Params[45].ToString(), Params[46].ToString(), Params[47].ToString(), Params[48].ToString(), Params[49].ToString(), Params[50].ToString(), Params[51].ToString(), Params[52].ToString(), Params[53].ToString(), Params[54].ToString(), Params[55].ToString());

                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SaveCpSvr8111S", e.ToString() + Environment.NewLine + sql);
                throw e;
            }
        }

        public void SaveCpSvrNew7221S(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.CPSVRNEW7221S (rawid, MARKETTYPE, TIME, INDEX_, SELLVOLUME, SELLMONEY, BUYVOLUME, BUYMONEY ) VALUES ( cybosplus.CPSVRNEW7221S_RAWID_SEQ.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<String> GetIndustryCode()
        {
            List<String> retList = new List<string>();
            try
            {
                DataSet ds = db.ExecuteDataSet(CommandType.Text, "SELECT code FROM cybosplus.CODE_MST_CYBOSPLUS WHERE category = 'INDUSTRYCODE'");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    retList.Add(dr["code"].ToString());
                }

                ds.Dispose();
                ds = null;
            }
            catch (Exception e)
            {
                throw e;
            }

            return retList;
        }

        public List<String> GetStockCode(string aStockGroup)
        {
            List<String> retList = new List<string>();
            try
            {
                DataSet ds = db.ExecuteDataSet(CommandType.Text, "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'STOCK' AND CODE = 'CODE' AND instr(desc_,'" + aStockGroup + "') > 0 ");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    retList.Add(dr["value"].ToString());
                }

                ds.Dispose();
                ds = null;
            }
            catch (Exception e)
            {
                throw e;
            }

            return retList;
        }

        public List<String> GetAllStockCode()
        {
            List<String> retList = new List<string>();
            try
            {
                DataSet ds = db.ExecuteDataSet(CommandType.Text, "SELECT VALUE FROM cybosplus.CONFIG_MST_CYBOSPLUS WHERE CATEGORY = 'STOCK' AND CODE = 'CODE' ");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    retList.Add(dr["value"].ToString());
                }

                ds.Dispose();
                ds = null;
            }
            catch (Exception e)
            {
                throw e;
            }

            return retList;
        }

        public void SavekStockAdkS(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKADKS_HST_CYBOSPLUS (rawid, UPSTOCKS, TOPSTOCKS, DONTMOVESTOCKS, DOWNSTOCKS, LOWESTSTOCKS, TOTALTRADINGVOLUME, TOTALTRADINGMONEY ) VALUES ( cybosplus.STOCKADKS_HST_CYBOSPLUS_RAWID_.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SavekStockAds(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKADS_HST_CYBOSPLUS (rawid, UPSTOCKS, TOPSTOCKS, DONTMOVESTOCKS, DOWNSTOCKS, LOWESTSTOCKS, TOTALTRADINGVOLUME, TOTALTRADINGMONEY ) VALUES ( cybosplus.STOCKADS_HST_CYBOSPLUS_RAWID_S.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockCur(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS (rawid, STOCKCODE, STOCKNAME, DIFFYESTERDAY, TIME, STARTPRICE, HIGHPRICE, LOWPRICE, SELLCALLPRICE, BUYCALLPRICE, STACKEDVOLUME, STACKEDMONEY, CURRPRICE, STATUS, STACKEDSELLVOLUME, STACKEDBUYVOLUME, TICKTRADEVOLUME, TIMESECOND, EXPECTEDTRADEFLAG, MARKETFLAG, PREMARKETVOLUME, DIFFSIGNAL, LPHOLDVOLUME, LPHOLDVOLUMEDIFF, LPHOLDRATE, TRADINGSTATUS, STACKEDSELLVOLUME2, STACKEDBUYVOLUME2 ) VALUES ( cybosplus.STOCKCUR_HST_SUBSC_CYBOSPLUS_R.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}','{8}','{9}','{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}','{17}','{18}','{19}','{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString(), Params[20].ToString(), Params[21].ToString(), Params[22].ToString(), Params[23].ToString(), Params[24].ToString(), Params[25].ToString(), Params[26].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockIndexis(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKINDEXIS_HST_CYBOSPLUS (rawid, TIME, INDEX_, DIFFYESTERDAY, VOLUME, MONEY, INDUSTRYNAME, INDUSTRYCODE ) VALUES ( cybosplus.STOCKINDEXIS_HST_CYBOSPLUS_RAW.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockJpbid(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKJPBID_HST_CYBOSPLUS (rawid, STOCKCODE, TIME, VOLUME, SELLCALLPRICE1, BUYCALLPRICE1, SELLREMAINS1, BUYREMAINS1, SELLCALLPRICE2, BUYCALLPRICE2, SELLREMAINS2, BUYREMAINS2, SELLCALLPRICE3, BUYCALLPRICE3, SELLREMAINS3, BUYREMAINS3, SELLCALLPRICE4, BUYCALLPRICE4, SELLREMAINS4, BUYREMAINS4, SELLCALLPRICE5, BUYCALLPRICE5, SELLREMAINS5, BUYREMAINS5, TOTALSELLREMAINS, TOTALBUYREMAINS, OUTTIMETOTALSELLREMAINS, OUTTIMETOTALBUYREMAINS, SELLCALLPRICE6, BUYCALLPRICE6, SELLREMAINS6, BUYREMAINS6, SELLCALLPRICE7, BUYCALLPRICE7, SELLREMAINS7, BUYREMAINS7, SELLCALLPRICE8, BUYCALLPRICE8, SELLREMAINS8, BUYREMAINS8, SELLCALLPRICE9, BUYCALLPRICE9, SELLREMAINS9, BUYREMAINS9, SELLCALLPRICE10, BUYCALLPRICE10, SELLREMAINS10, BUYREMAINS10, LPSELLREMAINS1, LPBUYREMAINS1, LPSELLREMAINS2, LPBUYREMAINS2, LPSELLREMAINS3, LPBUYREMAINS3, LPSELLREMAINS4, LPBUYREMAINS4, LPSELLREMAINS5, LPBUYREMAINS5, LPSELLREMAINS6, LPBUYREMAINS6, LPSELLREMAINS7, LPBUYREMAINS7, LPSELLREMAINS8, LPBUYREMAINS8, LPSELLREMAINS9, LPBUYREMAINS9, LPSELLREMAINS10, LPBUYREMAINS10, LPSELLREMAINSSUM, LPBUYREMAINSSUM ) VALUES ( cybosplus.STOCKJPBID_HST_CYBOSPLUS_RAWID.nextval, '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString(), Params[20].ToString(), Params[21].ToString(), Params[22].ToString(), Params[23].ToString(), Params[24].ToString(), Params[25].ToString(), Params[26].ToString(), Params[27].ToString(), Params[28].ToString(), Params[29].ToString(), Params[30].ToString(), Params[31].ToString(), Params[32].ToString(), Params[33].ToString(), Params[34].ToString(), Params[35].ToString(), Params[36].ToString(), Params[37].ToString(), Params[38].ToString(), Params[39].ToString(), Params[40].ToString(), Params[41].ToString(), Params[42].ToString(), Params[43].ToString(), Params[44].ToString(), Params[45].ToString(), Params[46].ToString(), Params[47].ToString(), Params[48].ToString(), Params[49].ToString(), Params[50].ToString(), Params[51].ToString(), Params[52].ToString(), Params[53].ToString(), Params[54].ToString(), Params[55].ToString(), Params[56].ToString(), Params[57].ToString(), Params[58].ToString(), Params[59].ToString(), Params[60].ToString(), Params[61].ToString(), Params[62].ToString(), Params[63].ToString(), Params[64].ToString(), Params[65].ToString(), Params[66].ToString(), Params[67].ToString(), Params[68].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockMember(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("INSERT INTO cybosplus.STOCKMEMBER_HST_SUBSC_CYBOSPLU (rawid, STOCKCODE, INDEX_, TIME, SELLMEMBER, BUYMEMBER, TOTALSELLQTY, TOTALBUYQTY ) VALUES ( cybosplus.STOCKMEMBER_HST_SUBSC_CYBOSP_1.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockMst(Object[] Params)
        {
            string sql = string.Empty;
            try
            {
                sql = string.Format("INSERT INTO cybosplus.STOCKMST_HST_CYBOSPLUS (rawid, STOCKCODE, STOCKNAME, DSSTOCKCODE, GROUPCODE, TIME, DEPARTMENT, BIGMEDIUMSMALL, DAYTOP, DAYBOTTOM, YESTERDAYEND, CURRENTPRICE, DIFFYESTERDAY, STARTPRICE, HIGHESTINDAY, LOWESTINDAY, SELLCALLPRICE, BUYCALLPRICE, STOCKEDVOLUME, STOCKEDTRADINGVALUE, EPS, NEWHIGHEST, NEWHIGHESTDATE, NEWLOWEST, NEWLOWESTDATE, CREDITMARKETS, SETTLEMENTMONTH, BASISPRICE, PER, STOCKSLISTED, CAPITALSTOCKLISTED, FOREIGNERDATE, FOREIGNERTIME, FOREIGNERSTOCKSLISTED, FOREIGNERSTOCKSORDERED, FOREIGNERORDERLIMITQTY, FOREIGNERORDERLIMITRATE, FOREIGNERORDERAVAILABLEQTY, FOREIGNERORDERAVAILABLERATE, STOCKITKINDCODE, UNITCOUNTERFORTRADING, STATUSCODE, DEPARTMENTCODE, YESTERDAYVOLUME, HIGHEST52WEEKS, HIGHEST52WEEKSDATE, LOWEST52WEEKS, LOWEST52WEEKSDATE, VENTURECOMPANY, KOSPI200, FACEVALUE, EXPPRICE, EXPPRICEDIFFYESTERDAY, EXPVOLUME, EXPPRICECODE, MARKETCODE, TREASURYSTOCK, TREASURYSTOCKVOLUME, FOREIGNERSELLTOTAL, FOREIGNERBUYTOTAL, CREDITBALANCERATE, CB, MANAGEMENTCODE, INVESTMENTWARNING, HOLDTRADING, BADFAITHNOTICECODE, BPS, WORK_DTTS ) VALUES ( cybosplus.STOCKMST_HST_CYBOSPLUS_RAWID_S.nextval, '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}','{60}','{61}','{62}','{63}','{64}','{65}',sysdate ) "
                    , Params[0].ToString(), Params[1].ToString(), Params[2].ToString(), Params[3].ToString(), Params[4].ToString(), Params[5].ToString(), Params[6].ToString(), Params[7].ToString(), Params[8].ToString(), Params[9].ToString(), Params[10].ToString(), Params[11].ToString(), Params[12].ToString(), Params[13].ToString(), Params[14].ToString(), Params[15].ToString(), Params[16].ToString(), Params[17].ToString(), Params[18].ToString(), Params[19].ToString(), Params[20].ToString(), Params[21].ToString(), Params[22].ToString(), Params[23].ToString(), Params[24].ToString(), Params[25].ToString(), Params[26].ToString(), Params[27].ToString(), Params[28].ToString(), Params[29].ToString(), Params[30].ToString(), Params[31].ToString(), Params[32].ToString(), Params[33].ToString(), Params[34].ToString(), Params[35].ToString(), Params[36].ToString(), Params[37].ToString(), Params[38].ToString(), Params[39].ToString(), Params[40].ToString(), Params[41].ToString(), Params[42].ToString(), Params[43].ToString(), Params[44].ToString(), Params[45].ToString(), Params[46].ToString(), Params[47].ToString(), Params[48].ToString(), Params[49].ToString(), Params[50].ToString(), Params[51].ToString(), Params[52].ToString(), Params[53].ToString(), Params[54].ToString(), Params[55].ToString(), Params[56].ToString(), Params[57].ToString(), Params[58].ToString(), Params[59].ToString(), Params[60].ToString(), Params[61].ToString(), Params[62].ToString(), Params[63].ToString(), Params[64].ToString(), Params[65].ToString());

                db.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("SaveStockMst", e.ToString() + Environment.NewLine + sql);
                throw e;
            }
        }

        public void UpdateRemainSubscribe(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery(
                    CommandType.Text
                    , string.Format("UPDATE cybosplus.CONFIG_MST_CYBOSPLUS SET VALUE = {0}  WHERE category = 'DATACOLLECTOR' AND code = 'REMAINSUBSCRIBE' "
                    , Params[0].ToString())
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
