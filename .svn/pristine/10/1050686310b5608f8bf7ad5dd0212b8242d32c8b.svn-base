<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OrderStock.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <body>
        <div style="width: 100%">
            <p>
                <h3 style="background-color: yellow;">금일 투자종목 및 전략</h3>
            </p>
            <asp:Button ID="Button1" runat="server" Text="새로고침" OnClick="Button1_Click" Height="40px" Width="100px" />
            <br /><br />
            <asp:GridView ID="GridView4" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" EmptyDataText="No Data">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <Columns>
                    <asp:BoundField DataField="rownum" HeaderText="번호" SortExpression="rownum" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="work_dtts" HeaderText="날짜" SortExpression="work_dtts" />
                    <asp:BoundField DataField="DESC_" HeaderText="종목" SortExpression="DESC_" />
                    <asp:BoundField DataField="STOCKCODE" HeaderText="종목코드" SortExpression="STOCKCODE" />
                    <asp:BoundField DataField="BUYMETHOD" HeaderText="매수전략" SortExpression="BUYMETHOD" />
                    <asp:BoundField DataField="QTY" HeaderText="거래횟수" SortExpression="QTY" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="AVGRATE" HeaderText="누적수익율" SortExpression="AVGRATE" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="RATE" HeaderText="금일수익율" SortExpression="RATE" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="CNT" HeaderText="금일거래횟수" SortExpression="CNT" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="CURRVAL" HeaderText="현재가" SortExpression="CURRVAL" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="DIFFVALUE" HeaderText="등락치" SortExpression="DIFFVALUE" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                    <asp:BoundField DataField="DIFFRATE" HeaderText="등락율" SortExpression="DIFFRATE" ItemStyle-HorizontalAlign="Right" />
                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                SelectCommand="select  rownum
                                        ,to_char(sysdate,'yyyymmdd') work_dtts
                                        ,B.DESC_
                                        ,A.STOCKCODE
                                        ,A.BUYMETHOD      
                                        ,C.QTY
                                        ,C.AVGRATE
                                        ,d.rate                
                                        ,d.cnt
                                        ,FNCURPRICESTOCKCUR_STOCKCODE(a.stockcode) CURRVAL
                                        ,E.DIFFYESTERDAY DIFFVALUE        
                                        ,round(E.DIFFYESTERDAY/(FNCURPRICESTOCKCUR_STOCKCODE(a.stockcode)-E.DIFFYESTERDAY)*100,2) DIFFRATE
                                from    order_stock a join CONFIG_MST_CYBOSPLUS b on A.STOCKCODE=B.VALUE
                                        join score_mst c on A.STOCKCODE=C.STOCKCODE and A.BUYMETHOD=C.BUY_METHOD
                                        left outer join (
                                            select  stockcode,buy_method
                                                    ,sum((round(nvl(sell_point,FNCURPRICESTOCKCUR_STOCKCODE(stockcode))/buy_point*100,2))-100-0.33) rate
                                                    ,count(*) cnt
                                            from    TRADE_HST a
                                            where   (buy_dtts between to_date(to_char(sysdate,'yyyymmdd')||'000000','yyyymmddhh24miss') and to_date(to_char(sysdate,'yyyymmdd')||'235959','yyyymmddhh24miss')
                                                or  sell_dtts between to_date(to_char(sysdate,'yyyymmdd')||'000000','yyyymmddhh24miss') and to_date(to_char(sysdate,'yyyymmdd')||'235959','yyyymmddhh24miss'))
                                                and volume is not null    
                                            group   by stockcode,buy_method  
                                        ) d on A.STOCKCODE=d.stockcode and A.BUYMETHOD=d.buy_method 
                    join STOCKMST_HST_CYBOSPLUS e on A.STOCKCODE=E.STOCKCODE and E.WORK_DTTS > sysdate-1
                    where   e.work_dtts > sysdate-1 and E.RAWID=(select max(rawid) from STOCKMST_HST_CYBOSPLUS where stockcode=a.stockcode and work_dtts > sysdate-1) "
                ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />

        </div>
    </body>
    </html>
</asp:Content>
