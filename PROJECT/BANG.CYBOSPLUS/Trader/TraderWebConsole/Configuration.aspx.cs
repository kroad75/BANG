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
        if (!Page.IsPostBack)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select  * from CONFIG_MST_CYBOSPLUS where   category='TRADER' ");

            SqlDataSource1.SelectCommand = varname1.ToString();
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);

            DataRow[] dr = dv.Table.Select("code='BALANCE'");
            tbMaxMoney.Text = string.Format("{0:C}", int.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='BUDGET'");
            tbBudget.Text = string.Format("{0:C}", int.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='BUYCONDITION_QTY'");
            tbMinCount.Text = string.Format("{0}", int.Parse(dr[0]["VALUE"].ToString()));

            //dr = dv.Table.Select("code='BUYCONDITION_QTY_SIGNAL'");
            //tbMinCountSignal.Text = string.Format("{0}", int.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='ORDERSTOCKCOUNT-DATA'");
            tbQtyData.Text = string.Format("{0}", int.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='ORDERSTOCKCOUNT-SIGNAL'");
            tbQtySignal.Text = string.Format("{0}", int.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='SELLEARNINGRATE_MINUS'");
            tbSellRateMinus.Text = string.Format("{0:N3}", double.Parse(dr[0]["VALUE"].ToString()));

            dr = dv.Table.Select("code='SELLEARNINGRATE_MINUS_SIGNAL'");
            tbSellRateMinusSignal.Text = string.Format("{0:N3}", double.Parse(dr[0]["VALUE"].ToString()));

            SqlDataSource6.SelectCommand = @"select  DAY || '.' || HOUR || ':' || MINUTE from    (    select extract(day from (systimestamp - max(A.WORK_DTTS)) ) DAY          ,extract(hour from (systimestamp - max(A.WORK_DTTS)) ) HOUR          ,extract(minute from (systimestamp - max(A.WORK_DTTS)) ) MINUTE    from CPMARKETWATCHS a) ";
            DataView dvv = (DataView)SqlDataSource6.Select(DataSourceSelectArguments.Empty);
            lbMarketWatch.Text = dvv[0][0].ToString();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        StringBuilder varname1 = new StringBuilder();
        varname1.Append("update CONFIG_MST_CYBOSPLUS set value = " + tbQtyData.Text + " where code='ORDERSTOCKCOUNT-DATA' ");

        SqlDataSource1.UpdateCommand = varname1.ToString();
        SqlDataSource1.Update();

        Response.Redirect(Request.RawUrl);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        StringBuilder varname1 = new StringBuilder();
        varname1.Append("update CONFIG_MST_CYBOSPLUS set value = " + tbQtySignal.Text + " where code='ORDERSTOCKCOUNT-SIGNAL' ");

        SqlDataSource1.UpdateCommand = varname1.ToString();
        SqlDataSource1.Update();

        Response.Redirect(Request.RawUrl);
    }
    protected void btRefresh_Click(object sender, EventArgs e)
    {
        //SqlDataSource5.Select(new DataSourceSelectArguments());
        Response.Redirect(Request.RawUrl);
    }
    protected void btnSellRateMinus_Click(object sender, EventArgs e)
    {
        StringBuilder varname1 = new StringBuilder();
        varname1.Append("update CONFIG_MST_CYBOSPLUS set value = '" + tbSellRateMinus.Text + "' where code='SELLEARNINGRATE_MINUS' ");

        SqlDataSource1.UpdateCommand = varname1.ToString();
        SqlDataSource1.Update();

        Response.Redirect(Request.RawUrl);
    }
    protected void btnSellRateMinusSignal_Click(object sender, EventArgs e)
    {
        StringBuilder varname1 = new StringBuilder();
        varname1.Append("update CONFIG_MST_CYBOSPLUS set value = '" + tbSellRateMinusSignal.Text + "' where code='SELLEARNINGRATE_MINUS_SIGNAL' ");

        SqlDataSource1.UpdateCommand = varname1.ToString();
        SqlDataSource1.Update();

        Response.Redirect(Request.RawUrl);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if(tbAddStockCode.Text=="" || tbAddStockName.Text=="")
        {
            return;
        }

        string stockGroup = ddlStockGroup.SelectedValue;
        string stockCode = tbAddStockCode.Text;
        string stockName = tbAddStockName.Text;

        string InsertSql =
            string.Format(" Insert into CYBOSPLUS.CONFIG_MST_CYBOSPLUS   (RAWID, CATEGORY, CODE, VALUE, DESC_) Values   (CONFIG_MST_CYBOSPLUS_RAWID_SEQ.nextval, 'STOCK', 'CODE', '{0}', '{1}|{2}') "
            , stockCode
            , stockName
            , stockGroup
            );

        SqlDataSource1.InsertCommand = InsertSql;
        SqlDataSource1.Insert();

        tbAddStockCode.Text = "";
        tbAddStockName.Text = "";

        Response.Redirect(Request.RawUrl);
    }
}