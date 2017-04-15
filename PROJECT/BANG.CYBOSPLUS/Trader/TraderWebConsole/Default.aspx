<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">신고가 신저가</h3>
        </p>
        <table style="width: 100%; ">
            <tr>
                <td style="width: 50%; border: 1px solid red;">
                    <div style="width: 100%">

                        <asp:Button ID="Button5" runat="server" Text="새로고침" OnClick="Button1_Click" Height="40px" Width="100px" />
                        <br /><br />
                        <asp:GridView ID="GridView6" runat="server" Width="100%" CellPadding="3" DataSourceID="SqlDataSource6" AllowSorting="True" EnableModelValidation="True" GridLines="Vertical" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" EmptyDataText="No Data" AllowPaging="True" PageSize="30">
                            <AlternatingRowStyle BackColor="#DCDCDC" />
                            <Columns>
                                <asp:BoundField DataField="EST_DATE" HeaderText="날짜" SortExpression="EST_DATE" />
                                <asp:BoundField DataField="newhighest_count" HeaderText="신고가" SortExpression="newhighest_count" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="NEWLOWEST_COUNT" HeaderText="신저가" SortExpression="NEWLOWEST_COUNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="STOCK_COUNT" HeaderText="종목수" SortExpression="STOCK_COUNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="TOTAL_RATE" HeaderText="전체수익율" SortExpression="TOTAL_RATE" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="UNMANAGED_RATE" HeaderText="비관리종목수익율" SortExpression="UNMANAGED_RATE" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="MANAGED_RATE" HeaderText="관리종목수익율" SortExpression="MANAGED_RATE" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="MANAGED_COUNT" HeaderText="관리종목거래횟수" SortExpression="MANAGED_COUNT" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="systimestamp" HeaderText="현재시간" SortExpression="systimestamp" />
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                            SelectCommand="select  *
                                from    (
                                    select  to_char(sysdate,'yyyymmdd') EST_DATE
                                            ,nvl(sum(NEWHIGHTEST_COUNT),0) newhighest_count
                                            ,nvl(sum(NEWLOWEST_COUNT),0) NEWLOWEST_COUNT
                                            ,FN_GETCURSTOCKCOUNT() STOCK_COUNT
                                            ,nvl(FN_GETTODAYTOTALRATE(),0) TOTAL_RATE
                                            ,nvl(FN_GETTODAYUNMANAGEDRATE(),0) UNMANAGED_RATE
                                            ,nvl(FN_GETTODAYMANAGEDRATE(),0) MANAGED_RATE
                                            ,nvl(FN_GETTODAYMANAGEDCOUNT(),0) MANAGED_COUNT
                                            ,systimestamp
                                    from    (
                                        select  distinct A.STOCKCODE
                                                ,decode(max(A.HIGHEST52WEEKSDATE) over (partition by stockcode),to_char(sysdate,'yyyymmdd'),1,0) NEWHIGHTEST_COUNT
                                                ,decode(max(A.LOWEST52WEEKSDATE)  over (partition by stockcode),to_char(sysdate,'yyyymmdd'),1,0) NEWLOWEST_COUNT
                                        from    STOCKMST_HST_CYBOSPLUS a
                                        where   A.WORK_DTTS between to_date(to_char(sysdate,'yyyymmdd') || '090000','yyyymmddhh24miss') 
                                                                and sysdate
                                    )
                                    union all
                                    select  to_char(v.EST_DATE) 
                                            ,v.newhighest_count
                                            ,v.NEWLOWEST_COUNT
                                            ,v.STOCK_COUNT
                                            ,v.TOTAL_RATE
                                            ,v.UNMANAGED_RATE
                                            ,v.MANAGED_RATE
                                            ,v.MANAGED_COUNT
                                            ,null
                                    from    VW_ESTDATE_RATE v
                                ) a
                                order   by est_date desc "
                            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
                    </div>
                </td>
                <td style="border: 1px solid red;">></td>
            </tr>
        </table>
    </div>
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">현재 보유종목</h3>
        </p>
        <asp:Button ID="Button1" runat="server" Text="새로고침" OnClick="Button1_Click" Height="40px" Width="100px" />
        <br /><br />
        <asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="3" DataSourceID="SqlDataSource1" AllowSorting="True" EnableModelValidation="True" GridLines="Vertical" AutoGenerateColumns="False" DataKeyNames="RAWID" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" OnRowCommand="GridView1_RowCommand1" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="RAWID" HeaderText="RAWID" ReadOnly="True" SortExpression="RAWID" Visible="true" />
                <asp:BoundField DataField="DESC_" HeaderText="종목명" SortExpression="DESC_" />
                <asp:BoundField DataField="STOCKCODE" HeaderText="종목코드" SortExpression="STOCKCODE" />
                <asp:BoundField DataField="BUY_DTTS" HeaderText="매수일시" SortExpression="BUY_DTTS" />
                <asp:BoundField DataField="BUY_POINT" HeaderText="매수가" SortExpression="BUY_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="VOLUME" HeaderText="수량" SortExpression="VOLUME" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="BUY_METHOD" HeaderText="매수전략" SortExpression="BUY_METHOD" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FUND_DESC" HeaderText="펀드설명" SortExpression="FUND_DESC" ItemStyle-HorizontalAlign="Center" />
                <%--<asp:BoundField DataField="TERM" HeaderText="TERM" SortExpression="TERM" />--%>
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TARGET" HeaderText="목표가(2.3%)" SortExpression="TARGET" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="CURR" HeaderText="현재가" SortExpression="CURR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="HoldDays" HeaderText="보유기간" SortExpression="HoldDays" />
                <asp:BoundField DataField="FUND_RATE" HeaderText="펀드수익율" SortExpression="FUND_RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="RatePerDay" HeaderText="일수익율" SortExpression="RatePerDay" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CNT" HeaderText="매매횟수" SortExpression="CNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RANK" HeaderText="수익율순위" SortExpression="RANK" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="AddButton" runat="server"
                            CommandName="SELL"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            Text="팔기" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            SelectCommand="select * from (select a.rawid,desc_
       ,a.stockcode,to_char(a.buy_dtts,'yy/mm/dd.hh24') buy_dtts,a.buy_point,volume,buy_method,F.FUND_DESC
       ,(buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney
       ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate
       ,round(buy_point + (buy_point * 0.023),0) TARGET
       ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR
       ,extract(day from (systimestamp - A.BUY_DTTS)) || '.' || extract(hour from (systimestamp - A.BUY_DTTS)) HoldDays
       ,c.FUND_RATE
       ,c.RatePerDay
       ,c.CNT
       ,c.RANK
from TRADE_HST a 
join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
left outer join fund_mst f on A.BUY_METHOD = F.FUND_NAME
left outer join (
    select  T.BUY_METHOD FUND_NAME
            ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) fund_rate
            ,round(sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) / max(sysdate-to_date(fe.value,'yyyymmdd')),2) RatePerDay   
            ,count(*) CNT
            ,rank() over (order by sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) desc) RANK
    from    TRADE_HST t
        join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
        join FUND_MST_EXT fe on F.RAWID=FE.FUND_RAWID
    where   instr(T.BUY_METHOD,'FUND')>0
        and fe.name='StartDate'
        and t.finished = 'Y'
    group   by T.BUY_METHOD 
    order   by T.BUY_METHOD
) c on A.BUY_METHOD = c.FUND_NAME
where   1=1 and finished='N' and volume is not null
order   by desc_,buy_method,buy_dtts) a order by earnmoney desc "
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">금일 거래현황</h3>
        </p>
        <asp:Button ID="Button3" runat="server" Text="새로고침" OnClick="Button1_Click" Height="40px" Width="100px" />
        <br /><br />
        <asp:GridView ID="GridView4" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource4" AutoGenerateColumns="False" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="TITLE" HeaderText="TITLE" SortExpression="DESC_" />
                <asp:BoundField DataField="BUYCOUNT" HeaderText="매수횟수" SortExpression="count" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="SELLCOUNT" HeaderText="매도횟수" SortExpression="count" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
            SelectCommand="select  unistr('TOTAL') Title ,sum(rate) rate,sum(earnmoney) earnmoney,sum(BUYCOUNT) BUYCOUNT ,sum(SELLCOUNT) SELLCOUNT
from    (
    select (buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney
           ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate
           ,case when buy_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss') then 1 else 0 end BUYCOUNT
           ,case when sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss') then 1 else 0 end SELLCOUNT
    from TRADE_HST a 
    join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
    where   1=1
       and ( buy_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss')
        or  sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss'))
       and volume is not null
)"
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
        <br />
        <asp:GridView ID="GridView3" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource3" AutoGenerateColumns="False" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="DESC_" HeaderText="종목명" SortExpression="DESC_" />
                <asp:BoundField DataField="STOCKCODE" HeaderText="종목코드" SortExpression="STOCKCODE" />
                <asp:BoundField DataField="BUY_METHOD" HeaderText="매수전략" SortExpression="BUY_METHOD" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="COUNT" HeaderText="거래횟수" SortExpression="count" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="CURR" HeaderText="현재가" SortExpression="CURR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
            SelectCommand="select  desc_,stockcode,buy_method,sum(rate) rate,sum(earnmoney) earnmoney,sum(count) count,max(curr) curr
from    (
    select desc_
           ,a.stockcode
           ,A.BUY_METHOD 
           ,(buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney
           ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate
           ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR
           ,1 count
    from TRADE_HST a 
    join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
    where   1=1
       and ( buy_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss')
        or  sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') || '000000','yyyymmddhh24miss') and to_date(to_char(systimestamp,'yyyymmdd') || '235959','yyyymmddhh24miss'))
       and finished='Y'
       and volume is not null
)
group   by desc_,stockcode,buy_method
order   by desc_,stockcode,buy_method
"
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>

    <div style="width: 100%;">
        <p>
            <h3 style="background-color: yellow;">금일 거래내역 상세(주문대상만)</h3>
        </p>
        <asp:Button ID="Button2" runat="server" Text="새로고침" OnClick="Button1_Click" Height="40px" Width="100px" />
        <br /><br />
        <asp:GridView ID="GridView2" runat="server" Width="100%" CellPadding="3" DataSourceID="SqlDataSource2" AllowSorting="True" EnableModelValidation="True" GridLines="Vertical" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" OnRowCommand="GridView1_RowCommand1" EmptyDataText="No Data" AllowPaging="True" PageSize="20">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="RANK" HeaderText="수익율순위" SortExpression="RANK" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="DESC_" HeaderText="종목명" SortExpression="DESC_" />
                <asp:BoundField DataField="STOCKCODE" HeaderText="종목코드" SortExpression="STOCKCODE" />
                <asp:BoundField DataField="BUY_DTTS" HeaderText="매수일시" SortExpression="BUY_DTTS" />
                <asp:BoundField DataField="BUY_POINT" HeaderText="매수가" SortExpression="BUY_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="BUY_METHOD" HeaderText="매수전략" SortExpression="BUY_METHOD" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SELL_DTTS" HeaderText="매도일시" SortExpression="SELL_DTTS" />
                <asp:BoundField DataField="SELL_POINT" HeaderText="매도가" SortExpression="SELL_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="VOLUME" HeaderText="수량" SortExpression="VOLUME" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CURR" HeaderText="현재가" SortExpression="CURR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            SelectCommand="select desc_
       ,A.STOCKCODE
       ,to_char(a.buy_dtts,'yy/mm/dd.hh24') buy_dtts
       ,A.BUY_POINT
       ,A.BUY_METHOD
       ,to_char(a.SELL_dtts,'yy/mm/dd.hh24') SELL_dtts
       ,A.SELL_POINT
       ,A.VOLUME
       ,(buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney
       ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate                     
       ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR
       ,c.RANK
from TRADE_HST a 
join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
join (
    select  T.BUY_METHOD FUND_NAME
            ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) fund_rate
            ,round(sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) / max(sysdate-to_date(fe.value,'yyyymmdd')),2) RatePerDay   
            ,count(*) CNT
            ,rank() over (order by sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) desc) RANK
    from    TRADE_HST t
        join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
        join FUND_MST_EXT fe on F.RAWID=FE.FUND_RAWID
    where   instr(T.BUY_METHOD,'FUND')>0
        and fe.name='StartDate'
        and t.finished = 'Y'
    group   by T.BUY_METHOD 
    order   by T.BUY_METHOD
) c on A.BUY_METHOD=c.FUND_NAME 
where   1=1
   and B.CATEGORY='STOCK'
   and B.CODE='CODE'
   and (buy_dtts between to_date(to_char(systimestamp-1,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss')
   or  sell_dtts between to_date(to_char(systimestamp-1,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss'))
    and volume is not null
order   by BUY_DTTS desc"
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>

    <%--<div style="width: 100%;">
        <p>
            <h3 style="background-color: yellow;">금일 거래내역 상세(전체)</h3>
        </p>
        <asp:Button ID="Button4" runat="server" Text="새로고침" OnClick="Button1_Click" />&nbsp;&nbsp;
        날짜:<asp:TextBox ID="tbSearchDateDetailList" runat="server" ></asp:TextBox>&nbsp;&nbsp;
        매수조건:<asp:TextBox ID="tbBuyConditionDetailList" runat="server" ></asp:TextBox>&nbsp;&nbsp;
        <asp:GridView ID="GridView5" runat="server" Width="100%" CellPadding="3" DataSourceID="SqlDataSource5" AllowSorting="True" EnableModelValidation="True" GridLines="Vertical" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" OnRowCommand="GridView1_RowCommand1" EmptyDataText="No Data" AllowPaging="True" PageSize="20">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="DESC_" HeaderText="종목명" SortExpression="DESC_" />
                <asp:BoundField DataField="STOCKGROUP" HeaderText="종목그룹" SortExpression="STOCKGROUP" />
                <asp:BoundField DataField="STOCKCODE" HeaderText="종목코드" SortExpression="STOCKCODE" />
                <asp:BoundField DataField="BUY_DTTS" HeaderText="매수일시" SortExpression="BUY_DTTS" />
                <asp:BoundField DataField="BUY_POINT" HeaderText="매수가" SortExpression="BUY_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="BUY_METHOD" HeaderText="매수전략" SortExpression="BUY_METHOD" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SELL_DTTS" HeaderText="매도일시" SortExpression="SELL_DTTS" />
                <asp:BoundField DataField="SELL_POINT" HeaderText="매도가" SortExpression="SELL_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="VOLUME" HeaderText="수량" SortExpression="VOLUME" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CURR" HeaderText="현재가" SortExpression="CURR" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource5" runat="server"
            SelectCommand="select substr(desc_,1,instr(desc_,'|')-1) desc_
       ,substr(desc_,instr(desc_,'|')+1) STOCKGROUP
       ,A.STOCKCODE
       ,to_char(a.buy_dtts,'yyyy-mm-dd hh24:mi:ss') buy_dtts
       ,A.BUY_POINT
       ,A.BUY_METHOD
       ,to_char(a.SELL_dtts,'yyyy-mm-dd hh24:mi:ss') SELL_dtts
       ,A.SELL_POINT
       ,A.VOLUME
       ,(buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100) earnmoney
       ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate                     
       ,FNCURPRICESTOCKCUR_STOCKCODE(stockcode) CURR
from TRADE_HST a 
join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
where   1=1
   and (buy_dtts between to_date(to_char(systimestamp,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss')
   or  sell_dtts between to_date(to_char(systimestamp,'yyyymmdd') ||'000000','yyyymmddhh24miss') and to_date( to_char(systimestamp,'yyyymmdd') ||'235959','yyyymmddhh24miss'))
order   by BUY_DTTS desc"
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>--%>

    <asp:SqlDataSource ID="SqlDataSourceBiz" runat="server"
        ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
</body>
</html>
</asp:Content>