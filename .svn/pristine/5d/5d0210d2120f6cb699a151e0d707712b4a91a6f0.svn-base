using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;

namespace BANG.CYBOSPLUS.DAL
{
    public class SqlDal
    {
        string dbConnStr = null;
        SqlDatabase db = null;

        public SqlDal(string dbConnStr)
        {
            try
            {
                this.dbConnStr = dbConnStr;
                db = new SqlDatabase(this.dbConnStr);
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

        public DateTime GetStartTime()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(System.Data.CommandType.StoredProcedure, "GETCONFIG_STARTTIME").ToString();

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
                retString = db.ExecuteScalar(System.Data.CommandType.StoredProcedure, "GETCONFIG_ENDTIME").ToString();

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
                retString = db.ExecuteScalar(System.Data.CommandType.StoredProcedure, "GETCONFIG_SYSTEMDOWNTIME").ToString();

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
                retString = db.ExecuteScalar("GETCONFIG_LOGDIR", new object[] { "DATACOLLECTOR" }).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public string GetLogDir(string aCategory)
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar("GETCONFIG_LOGDIR", new object[] { aCategory }).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public string GetDataCollectorEXEPath()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(CommandType.Text, "select	value from CONFIG_MST_CYBOSPLUS where	category = 'DATACOLLECTOR' and code = 'EXEFILEPATH'").ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public string GetCybosPlusVBSPath()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(CommandType.Text, "select	value from CONFIG_MST_CYBOSPLUS where	category = 'GlobalCybosPlusManager' and code = 'CybosPlusVBSPath'").ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public string GetNeedRestartDataCollector()
        {
            string retString = string.Empty;
            try
            {
                retString = db.ExecuteScalar(CommandType.Text, "select	value from CONFIG_MST_CYBOSPLUS where category='DATACOLLECTOR' and code='RESTARTME' ").ToString();

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public void SetRestartMe_DataCollector()
        {
            try
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SetRestartMe_DataCollector");                

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void ResetRestartMe_DataCollector()
        {
            try
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "ResetRestartMe_DataCollector");

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public int GetRemainSubscribe()
        {
            int retString = 0;
            try
            {
                retString = int.Parse(db.ExecuteScalar(CommandType.StoredProcedure, "GETCONFIG_REMAINSUBSCRIBE").ToString());

            }
            catch (Exception e)
            {
                throw e;
            }

            return retString;
        }

        public void SaveCp7225(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_CPSVR7225", Params);
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
                db.ExecuteNonQuery("INSERT_CPSVR8081", Params);
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
                db.ExecuteNonQuery("INSERT_CPMARKETWATCHS", Params);
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
                db.ExecuteNonQuery("INSERT_CPSVR8091S", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpSvr8111KS(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_CPSVR8111KS", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpSvr8111S(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_CPSVR8111S", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveCpSvrNew7221S(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_CPSVRNEW7221S", Params);
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
                DataSet ds = db.ExecuteDataSet(CommandType.StoredProcedure, "GETCONFIG_INDUSTRYCODE");
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

        public List<String> GetStockCode()
        {
            List<String> retList = new List<string>();
            try
            {
                DataSet ds = db.ExecuteDataSet(CommandType.StoredProcedure, "GETCONFIG_STOCKCODE");
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

        public void SaveExpectIndexS(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_EXPECTINDEXS_HST_CYBOSPLUS", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Savek200Expect(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_K200EXPECT", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SavekStockAdkS(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_STOCKADKS_HST_CYBOSPLUS", Params);
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
                db.ExecuteNonQuery("INSERT_STOCKADS_HST_CYBOSPLUS", Params);
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
                db.ExecuteNonQuery("INSERT_STOCKCUR_HST_SUBSC_CYBOSPLUS", Params);
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
                db.ExecuteNonQuery("INSERT_STOCKINDEXIS_HST_CYBOSPLUS", Params);
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
                db.ExecuteNonQuery("INSERT_STOCKJPBID_HST_CYBOSPLUS", Params);
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
                db.ExecuteNonQuery("INSERT_STOCKMEMBER_HST_SUBSC_CYBOSPLUS", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveStockMst(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("INSERT_STOCKMST_HST_CYBOSPLUS", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateRemainSubscribe(Object[] Params)
        {
            try
            {
                db.ExecuteNonQuery("UPDATE_CONFIG_REMAINSUBSCRIBE", Params);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataSet AnalysisGetStockCurData(string sDateTime, string eDateTime, string StockCode)
        {
            DataSet dsRet = null;

            try
            {
                dsRet = db.ExecuteDataSet("AnalysisGetStockCurData", new object[] { sDateTime, eDateTime, StockCode });
            }
            catch (Exception e)
            {
                throw e;
            }

            return dsRet;
        }

        public void AnalysisInsertBuyPoint(string StockCode,long StockCurRawid)
        {
            try
            {
                db.ExecuteNonQuery("AnalysisInsertBuyPoint", new object[] { StockCode, StockCurRawid });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void AnalysisInsertFinishedDate(string FinishedDate)
        {
            try
            {
                db.ExecuteNonQuery("AnalysisInsertFinishedDate", new object[] { FinishedDate });
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public DateTime AnalysisGetMaxFinishDay()
        {
            DateTime retDt = DateTime.Now;

            try
            {
                string s = db.ExecuteScalar("AnalysisGetMaxFinishDay").ToString();
                retDt = new DateTime(int.Parse(s.Split('-')[0]), int.Parse(s.Split('-')[1]), int.Parse(s.Split('-')[2]));
            }
            catch (Exception e)
            {
                throw e;
            }

            return retDt;
        }

        public object Query(string query)
        {
            object retO = null;
            try
            {
                retO = db.ExecuteScalar(CommandType.Text, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return retO;
        }

        public DataSet QueryDataSet(string query)
        {
            DataSet retO = null;
            try
            {
                retO = db.ExecuteDataSet(CommandType.Text, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return retO;
        }

        public int QueryNonQuery(string query) 
        {
            int retO = 0;
            try
            {
                retO = db.ExecuteNonQuery(CommandType.Text, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            return retO;
        }
    }
}
