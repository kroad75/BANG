<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Fund.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">펀드별 수익율(매도완료)</h3>
        </p>
        <asp:Button ID="Button3" runat="server" Text="새로고침" OnClick="Button1_Click" />
        <asp:GridView ID="GridView4" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource4" AutoGenerateColumns="False" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="FUND_NAME" HeaderText="펀드이름" SortExpression="FUND_NAME" />
                <asp:BoundField DataField="FUND_DESC" HeaderText="펀드설명" SortExpression="FUND_DESC" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="STARTDATE" HeaderText="펀드시작일" SortExpression="STARTDATE" />
                <asp:BoundField DataField="FUNDINGDAYS" HeaderText="펀드유지일수" SortExpression="FUNDINGDAYS" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RATEPERDAY" HeaderText="일평균수익율" SortExpression="RATEPERDAY" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CNT" HeaderText="횟수" SortExpression="CNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RANK" HeaderText="수익율순위" SortExpression="RANK" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="TOTAL_RATE" HeaderText="수익율누적" SortExpression="TOTAL_RATE" ItemStyle-HorizontalAlign="Right" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
            SelectCommand="select  T.BUY_METHOD FUND_NAME
                                    ,max(F.FUND_DESC) FUND_DESC
                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate
                                    ,max(FE.Value) StartDate
                                    ,round(max(sysdate-to_date(fe.value,'yyyymmdd')),2) FundingDays
                                    ,round(sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) / max(sysdate-to_date(fe.value,'yyyymmdd')),2) RatePerDay   
                                    ,count(*) CNT
                                    ,rank() over (order by sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) desc) RANK
                                    ,SUM(sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)) over () TOTAL_RATE
                            from    TRADE_HST t
                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                join FUND_MST_EXT fe on F.RAWID=FE.FUND_RAWID
                            where   instr(T.BUY_METHOD,'FUND')>0
                                and fe.name='StartDate'
                                and t.finished = 'Y'
                            group   by T.BUY_METHOD 
                            order   by T.BUY_METHOD "
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>
    <br />
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">펀드별 수익율(보유중)</h3>
        </p>
        <asp:Button ID="Button2" runat="server" Text="새로고침" OnClick="Button1_Click" />
        <asp:GridView ID="GridView2" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource2" AutoGenerateColumns="False" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="FUND_NAME" HeaderText="펀드이름" SortExpression="FUND_NAME" />
                <asp:BoundField DataField="FUND_DESC" HeaderText="펀드설명" SortExpression="FUND_DESC" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                <%--<asp:BoundField DataField="STARTDATE" HeaderText="펀드시작일" SortExpression="STARTDATE" />--%>
                <%--<asp:BoundField DataField="FUNDINGDAYS" HeaderText="펀드유지일수" SortExpression="FUNDINGDAYS" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RATEPERDAY" HeaderText="일평균수익율" SortExpression="RATEPERDAY" ItemStyle-HorizontalAlign="Right" />--%>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            SelectCommand="select  T.BUY_METHOD FUND_NAME
                                    ,max(F.FUND_DESC) FUND_DESC
                                    ,sum((buy_point*volume)*(((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33)/100)) earnmoney
                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate
                                    ,max(FE.Value) StartDate
                                    ,round(max(sysdate-to_date(fe.value,'yyyymmdd')),2) FundingDays
                                    ,round(sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) / max(sysdate-to_date(fe.value,'yyyymmdd')),2) RatePerDay   
                            from    TRADE_HST t
                                join FUND_MST f on T.BUY_METHOD=F.FUND_NAME
                                join FUND_MST_EXT fe on F.RAWID=FE.FUND_RAWID
                            where   instr(T.BUY_METHOD,'FUND')>0
                                and fe.name='StartDate'
                                and t.finished = 'N'
                            group   by T.BUY_METHOD 
                            order   by T.BUY_METHOD "
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>
    <br />
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">매매일지</h3>
        </p>
        <asp:Button ID="Button4" runat="server" Text="새로고침" OnClick="Button1_Click" />
        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" AppendDataBoundItems="true" DataSourceID="SqlDataSource6" DataTextField="STOCK_CODE" DataValueField="STOCK_CODE">
            <asp:ListItem>ALL</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" >
            <asp:ListItem>ALL</asp:ListItem>
            <asp:ListItem>BUY</asp:ListItem>
            <asp:ListItem>SELL</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList5" runat="server" AutoPostBack="True" AppendDataBoundItems="true" DataSourceID="SqlDataSource7" DataTextField="FUND_NAME" DataValueField="FUND_NAME" >
            <asp:ListItem>ALL</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList6" runat="server" AutoPostBack="True" AppendDataBoundItems="true" DataSourceID="SqlDataSource8" DataTextField="WORK_DTTS" DataValueField="WORK_DTTS" >
            <asp:ListItem>ALL</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList7" runat="server" AutoPostBack="True">
            <asp:ListItem>ALL</asp:ListItem>
            <asp:ListItem>Y</asp:ListItem>
            <asp:ListItem>N</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT &quot;WORK_DTTS&quot; FROM &quot;ORDER_STOCK_FUND_ACTION&quot; ORDER BY &quot;WORK_DTTS&quot; desc"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT &quot;FUND_NAME&quot; FROM &quot;FUND_MST&quot; ORDER BY &quot;FUND_NAME&quot;"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT DISTINCT &quot;STOCK_CODE&quot; FROM &quot;ORDER_STOCK_FUND_ACTION&quot; ORDER BY &quot;STOCK_CODE&quot;"></asp:SqlDataSource>
        <asp:GridView ID="GridView3" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource5" AutoGenerateColumns="False" EmptyDataText="No Data" AllowPaging="True" PageSize="20">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="STOCK_CODE" HeaderText="종목코드" SortExpression="STOCK_CODE" />
                <asp:BoundField DataField="DESC_" HeaderText="종목명" SortExpression="DESC_" />
                <asp:BoundField DataField="ACTION" HeaderText="매매" SortExpression="ACTION" />
                <asp:BoundField DataField="FUND_NAME" HeaderText="펀드이름" SortExpression="FUND_NAME" />
                <asp:BoundField DataField="WORK_DTTS" HeaderText="작업일" SortExpression="WORK_DTTS" />
                <asp:BoundField DataField="BUY_DTTS" HeaderText="매수일" SortExpression="BUY_DTTS" />
                <asp:BoundField DataField="BUY_POINT" HeaderText="매수가" SortExpression="BUY_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="SELL_DTTS" HeaderText="매도일" SortExpression="SELL_DTTS" />
                <asp:BoundField DataField="SELL_POINT" HeaderText="매도가" SortExpression="SELL_POINT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource5" runat="server"
            SelectCommand="select  A.STOCK_CODE
                                    ,B.DESC_
                                    ,A.ACTION
                                    ,A.FUND_NAME
                                    ,A.WORK_DTTS 
                                    ,to_char(T.BUY_DTTS,'yyyymmdd') BUY_DTTS
                                    ,T.BUY_POINT
                                    ,to_char(T.SELL_DTTS,'yyyymmdd') SELL_DTTS
                                    ,T.SELL_POINT        
                                    ,(round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33 rate
                                from    ORDER_STOCK_FUND_ACTION a 
                                left outer join CONFIG_MST_CYBOSPLUS b on A.STOCK_CODE=B.VALUE
                                left outer join TRADE_HST t on A.STOCK_CODE=T.STOCKCODE and A.FUND_NAME= T.BUY_METHOD and A.WORK_DTTS = to_char(decode(A.ACTION,'SELL',T.SELL_DTTS,T.BUY_DTTS),'yyyymmdd')
                                where   1=1 
                                order   by A.WORK_DTTS desc,A.FUND_NAME,A.ACTION "
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>
    <br />
    <div style="width: 100%">
        <p>
            <h3 style="background-color: yellow;">펀드별 종목별 수익율</h3>
        </p>
        <asp:Button ID="Button1" runat="server" Text="새로고침" OnClick="Button1_Click" />
        Finished&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
            <asp:ListItem>ALL</asp:ListItem>
            <asp:ListItem>Y</asp:ListItem>
            <asp:ListItem Selected="True">N</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" AppendDataBoundItems="true" DataSourceID="SqlDataSource3" DataTextField="FUND_NAME" DataValueField="FUND_NAME">
            <asp:ListItem>ALL</asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT &quot;FUND_NAME&quot; FROM &quot;FUND_MST&quot; ORDER BY &quot;FUND_NAME&quot;"></asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" EmptyDataText="No Data">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="FUND_NAME" HeaderText="펀드이름" SortExpression="FUND_NAME" />
                <asp:BoundField DataField="FUND_DESC" HeaderText="펀드설명" SortExpression="FUND_DESC" />
                <asp:BoundField DataField="STOCKCODE" HeaderText="STOCKCODE" SortExpression="STOCKCODE" />
                <asp:BoundField DataField="STOCKNAME" HeaderText="STOCKNAME" SortExpression="STOCKNAME" />
                <asp:BoundField DataField="FINISHED" HeaderText="FINISHED" SortExpression="FINISHED" />
                <asp:BoundField DataField="COUNT" HeaderText="횟수" SortExpression="count" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="earnmoney" HeaderText="차익" SortExpression="earnmoney" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                <asp:BoundField DataField="RATE" HeaderText="수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            SelectCommand="select  T.BUY_METHOD FUND_NAME
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
                        order   by T.BUY_METHOD,T.STOCKCODE,T.FINISHED"
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
    </div>            
</body>
</html>
</asp:Content>