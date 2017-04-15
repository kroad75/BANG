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
        StringBuilder sb = new StringBuilder();
        sb.Append(" select  A.STOCK_CODE ");
        sb.Append("         ,B.DESC_ ");
        sb.Append("         ,A.ACTION ");
        sb.Append("         ,A.FUND_NAME ");
        sb.Append("         ,A.WORK_DTTS ");
        sb.Append("         ,to_char(T.BUY_DTTS,'yyyymmdd') BUY_DTTS ");
        sb.Append("         ,T.BUY_POINT ");
        sb.Append("         ,to_char(T.SELL_DTTS,'yyyymmdd') SELL_DTTS ");
        sb.Append("         ,T.SELL_POINT ");
        sb.Append("         ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate ");
        sb.Append(" from    ORDER_STOCK_FUND_ACTION a  ");
        sb.Append("     left outer join CONFIG_MST_CYBOSPLUS b on A.STOCK_CODE=B.VALUE ");
        sb.Append("     left outer join TRADE_HST t on A.STOCK_CODE=T.STOCKCODE and A.FUND_NAME= T.BUY_METHOD and A.WORK_DTTS = to_char(decode(A.ACTION,'SELL',T.SELL_DTTS,T.BUY_DTTS),'yyyymmdd') ");
        sb.Append(" where   1=1 ");

        if (DropDownList3.SelectedValue != "ALL")
        {
            sb.AppendFormat(" and A.STOCK_CODE='{0}' ", DropDownList3.SelectedValue);
        }

        if (DropDownList4.SelectedValue != "ALL")
        {
            sb.AppendFormat(" and A.ACTION='{0}' ", DropDownList4.SelectedValue);
        }

        if (DropDownList5.SelectedValue != "ALL")
        {
            sb.AppendFormat(" and A.FUND_NAME='{0}' ", DropDownList5.SelectedValue);
        }

        if (DropDownList6.SelectedValue != "ALL")
        {
            sb.AppendFormat(" and A.WORK_DTTS='{0}' ", DropDownList6.SelectedValue);
        }

        if (DropDownList7.SelectedValue != "ALL")
        {
            sb.AppendFormat(" and T.FINISHED='{0}' ", DropDownList7.SelectedValue);
        }

        sb.Append(" order   by A.WORK_DTTS desc,A.FUND_NAME,A.ACTION ");
        SqlDataSource5.SelectCommand = sb.ToString();
        GridView3.DataBind();

        if (DropDownList1.SelectedValue == "ALL")
        {
            if (DropDownList2.SelectedValue == "ALL")
                SqlDataSource1.SelectCommand = @"select  T.BUY_METHOD FUND_NAME
                                                    ,MAX(F.FUND_DESC) FUND_DESC
                                                    ,T.STOCKCODE
                                                    ,max(C.DESC_) STOCKNAME
                                                    ,T.FINISHED
                                                    ,count(*) count
                                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate  
                                            from    TRADE_HST t
                                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                                join CONFIG_MST_CYBOSPLUS c on T.STOCKCODE=C.VALUE
                                            where   instr(T.BUY_METHOD,'FUND')>0
                                            group   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED
                                            order   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED";
            else
                SqlDataSource1.SelectCommand = @"select  T.BUY_METHOD FUND_NAME
                                                    ,MAX(F.FUND_DESC) FUND_DESC
                                                    ,T.STOCKCODE
                                                    ,max(C.DESC_) STOCKNAME
                                                    ,T.FINISHED
                                                    ,count(*) count
                                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate  
                                            from    TRADE_HST t
                                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                                join CONFIG_MST_CYBOSPLUS c on T.STOCKCODE=C.VALUE
                                            where   instr(T.BUY_METHOD,'FUND')>0 
                                            and   T.BUY_METHOD = '" + DropDownList2.SelectedValue + "' "
                                          + " group   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED "
                                          + " order   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED ";
        }
        else
        {
            if (DropDownList2.SelectedValue == "ALL")
                SqlDataSource1.SelectCommand = @"select  T.BUY_METHOD FUND_NAME
                                                    ,MAX(F.FUND_DESC) FUND_DESC
                                                    ,T.STOCKCODE
                                                    ,max(C.DESC_) STOCKNAME
                                                    ,T.FINISHED
                                                    ,count(*) count
                                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate  
                                            from    TRADE_HST t
                                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                                join CONFIG_MST_CYBOSPLUS c on T.STOCKCODE=C.VALUE
                                            where   instr(T.BUY_METHOD,'FUND')>0
                                                and T.FINISHED='" + DropDownList1.SelectedValue + "'"
                                            + " group   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED "
                                            + " order   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED ";
            else
                SqlDataSource1.SelectCommand = @"select  T.BUY_METHOD FUND_NAME
                                                    ,MAX(F.FUND_DESC) FUND_DESC
                                                    ,T.STOCKCODE
                                                    ,max(C.DESC_) STOCKNAME
                                                    ,T.FINISHED
                                                    ,count(*) count
                                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate  
                                            from    TRADE_HST t
                                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                                join CONFIG_MST_CYBOSPLUS c on T.STOCKCODE=C.VALUE
                                            where   instr(T.BUY_METHOD,'FUND')>0
                                                and T.FINISHED='" + DropDownList1.SelectedValue + "'"
                                          + " and   T.BUY_METHOD = '" + DropDownList2.SelectedValue + "' "
                                          + " group   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED "
                                          + " order   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED ";

        }

        GridView1.DataBind();
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