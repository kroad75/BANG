using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SEARCHDATE"] == null)
            Session["SEARCHDATE"] = DateTime.Now.ToString("yyyyMMdd");

        if (Session["BUYMETHOD"] == null)
            Session["BUYMETHOD"] = "";

        //if (tbSearchDateDetailList.Text != "" && Session["SEARCHDATE"] != null && Session["SEARCHDATE"].ToString() != tbSearchDateDetailList.Text)
        //    Session["SEARCHDATE"] = tbSearchDateDetailList.Text;

        //if (tbBuyConditionDetailList.Text != "" && Session["BUYMETHOD"] != null && Session["BUYMETHOD"].ToString() != tbBuyConditionDetailList.Text)
        //    Session["BUYMETHOD"] = tbBuyConditionDetailList.Text;

        //tbSearchDateDetailList.Text = Session["SEARCHDATE"].ToString();
        //tbBuyConditionDetailList.Text = Session["BUYMETHOD"].ToString();

        //StringBuilder varname1 = new StringBuilder();
        //varname1.Append("select substr(desc_,1,instr(desc_,'|')-1) desc_ \n");
        //varname1.Append("       ,substr(desc_,instr(desc_,'|')+1) STOCKGROUP \n");
        //varname1.Append("       ,A.STOCKCODE \n");
        //varname1.Append("       ,to_char(a.buy_dtts,'yyyy-mm-dd hh24:mi:ss') buy_dtts \n");
        //varname1.Append("       ,A.BUY_POINT \n");
        //varname1.Append("       ,A.BUY_METHOD \n");
        //varname1.Append("       ,to_char(a.SELL_dtts,'yyyy-mm-dd hh24:mi:ss') SELL_dtts \n");
        //varname1.Append("       ,A.SELL_POINT \n");
        //varname1.Append("       ,A.VOLUME \n");
        //varname1.Append("       ,(buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney \n");
        //varname1.Append("       ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate \n");
        //varname1.Append("       ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR \n");
        //varname1.Append("from TRADE_HST a \n");
        //varname1.Append("join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE \n");
        //varname1.Append("where   1=1 \n");
        //if (Session["SEARCHDATE"] == null || Session["SEARCHDATE"].ToString().Length == 0)
        //{
        //    varname1.Append("   and (buy_dtts between to_date(to_char(systimestamp,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss') \n");
        //    varname1.Append("   or  sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss')) \n");
        //}
        //else
        //{
        //    varname1.AppendFormat("   and (buy_dtts between to_date('{0}' ||'000000','yyyymmddhh24miss') and to_date( '{0}' ||'235959','yyyymmddhh24miss') \n", Session["SEARCHDATE"].ToString());
        //    varname1.AppendFormat("   or  sell_dtts between to_date('{0}' ||'000000','yyyymmddhh24miss') and to_date( '{0}' ||'235959','yyyymmddhh24miss')) \n", Session["SEARCHDATE"].ToString());
        //}

        //if (Session["BUYMETHOD"] != null && Session["BUYMETHOD"].ToString() != "" && Session["BUYMETHOD"].ToString() != "999")
        //{
        //    varname1.AppendFormat(" and buy_method = '{0}' \n", Session["BUYMETHOD"].ToString());
        //}


        //varname1.Append("order   by BUY_DTTS desc");

        //SqlDataSource5.SelectCommand = varname1.ToString();
    }
    protected void GridView1_RowCommand1(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SELL")
        {
            // Retrieve the row index stored in the 
            // CommandArgument property.
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button 
            // from the Rows collection.
            GridViewRow row = GridView1.Rows[index];

            // Add code here to add the item to the shopping cart.
            string rawid = row.Cells[0].Text;
            string StockCode = row.Cells[2].Text;

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("UPDATE TRADE_HST ");
            varname1.Append("   SET sell_dtts    = SYSTIMESTAMP, ");
            varname1.AppendFormat("       sell_point   = FnCurPriceStockCur_StockCode('{0}') , ", StockCode);
            varname1.AppendFormat("       profit       = FnCurPriceStockCur_StockCode('{0}') - buy_point, ", StockCode);
            varname1.Append("       FINISHED = 'Y' ");
            varname1.Append(" WHERE     1=1");
            varname1.Append("       AND FINISHED = 'N' ");
            varname1.AppendFormat("       AND rawid = '{0}' ", rawid);

            SqlDataSourceBiz.UpdateCommand = varname1.ToString();
            SqlDataSourceBiz.Update();

            Response.Redirect(Request.RawUrl);

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
        //ScriptManager.RegisterStartupScript(this, GetType(), "refresh", "'window.location.reload(true);", true);
    }

    //protected void tbSearchDateDetailList_TextChanged(object sender, EventArgs e)
    //{
    //    Response.Cookies["UserSettings"]["SEARCHDATE"] = tbSearchDateDetailList.Text;
    //    Response.Cookies["UserSettings"].Expires = DateTime.Now.AddDays(1d);
    //}
    //protected void tbBuyConditionDetailList_TextChanged(object sender, EventArgs e)
    //{
    //    Response.Cookies["UserSettings"]["BUYCONDITION"] = tbBuyConditionDetailList.Text;
    //    Response.Cookies["UserSettings"].Expires = DateTime.Now.AddDays(1d);
    //}
}