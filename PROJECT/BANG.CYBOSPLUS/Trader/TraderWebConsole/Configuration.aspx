<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Configuration.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <body>
        <div style="width: 100%">
            <p>
                <asp:Button ID="btRefresh" runat="server" Text="새로고침" OnClick="btRefresh_Click" />
                <h3 style="background-color: yellow;">투자정보</h3>

                <asp:Label ID="Label1" runat="server" Text="전체예산" Width="300px"></asp:Label>
                <asp:TextBox ID="tbBudget" runat="server" Style="text-align: right" ReadOnly="true"></asp:TextBox><br />

                <asp:Label ID="Label2" runat="server" Text="현재투자 가능금액" Width="300px"></asp:Label>
                <asp:TextBox ID="tbMaxMoney" runat="server" Style="text-align: right" ReadOnly="true"></asp:TextBox><br />

                <asp:Label ID="Label3" runat="server" Text="주문조건-최소거래횟수" Width="300px"></asp:Label>
                <asp:TextBox ID="tbMinCount" runat="server" Style="text-align: right" ReadOnly="true"></asp:TextBox><br />

                <asp:Label ID="Label4" runat="server" Text="주문조건-최소거래횟수-신호" Width="300px"></asp:Label>
                <asp:TextBox ID="tbMinCountSignal" runat="server" Style="text-align: right" ReadOnly="true"></asp:TextBox><br />
            </p>

            <p>
                <h3 style="background-color: yellow;">투자 파라메터</h3>

                <asp:Label ID="Label5" runat="server" Text="DATA 기반 주문대상 종목수" Width="300px"></asp:Label>
                <asp:TextBox ID="tbQtyData" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="변경" OnClick="Button1_Click1" Style="height: 21px" /><br />

                <asp:Label ID="Label6" runat="server" Text="SIGNAL 기반 주문대상 종목수" Width="300px"></asp:Label>
                <asp:TextBox ID="tbQtySignal" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="변경" OnClick="Button2_Click" /><br />

                <asp:Label ID="Label7" runat="server" Text="DATA 기반 매도 손해율" Width="300px"></asp:Label>
                <asp:TextBox ID="tbSellRateMinus" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:Button ID="btnSellRateMinus" runat="server" Text="변경" Style="height: 21px" OnClick="btnSellRateMinus_Click" /><br />

                <asp:Label ID="Label8" runat="server" Text="SIGNAL 기반 매도 손해율" Width="300px"></asp:Label>
                <asp:TextBox ID="tbSellRateMinusSignal" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:Button ID="btnSellRateMinusSignal" runat="server" Text="변경" Style="height: 21px" OnClick="btnSellRateMinusSignal_Click" /><br />


            </p>

            <p>
                <h3 style="background-color: yellow;">종목관리</h3>

                <asp:Label ID="Label9" runat="server" Text="종목추가(좀목코드/종목명)" Width="300px"></asp:Label>
                <asp:DropDownList ID="ddlStockGroup" runat="server" DataSourceID="SqlDataSource2" DataTextField="GROUPNAME" DataValueField="GROUPNAME"></asp:DropDownList>                
                <asp:TextBox ID="tbAddStockCode" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:TextBox ID="tbAddStockName" runat="server" Style="text-align: right"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" Text="추가" Style="height: 21px" OnClick="Button3_Click" /><br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource3" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:BoundField DataField="GROUPNAME" HeaderText="GROUPNAME" SortExpression="GROUPNAME" />
                        <asp:BoundField DataField="COUNT" HeaderText="COUNT" SortExpression="COUNT" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            </asp:GridView>            
            </p>

            <p>
                <h3 style="background-color: yellow;">Data Collect 감시</h3>
                <asp:Button ID="Button4" runat="server" Text="새로고침" OnClick="btRefresh_Click" Height="40px" Width="100px" />
                <asp:Label ID="lbMarketWatch" runat="server" Text=""></asp:Label>
                <br /><br />
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource4" EnableModelValidation="True" GridLines="Vertical" AllowSorting="True" AllowPaging="True">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:BoundField DataField="STOCKCODE" HeaderText="STOCKCODE" SortExpression="STOCKCODE" />
                        <asp:BoundField DataField="DAY" HeaderText="DAY" SortExpression="DAY" />
                        <asp:BoundField DataField="HOUR" HeaderText="HOUR" SortExpression="HOUR" />
                        <asp:BoundField DataField="MINUTE" HeaderText="MINUTE" SortExpression="MINUTE" />
                        <asp:BoundField DataField="DESC_" HeaderText="DESC_" SortExpression="DESC_" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            </asp:GridView>            
            </p>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" />
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT &quot;GROUPNAME&quot; FROM &quot;VW_GETSTOCKGROUP&quot;"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="SELECT &quot;GROUPNAME&quot;, &quot;COUNT&quot; FROM &quot;VW_GETSTOCKGROUP&quot;"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" 
            SelectCommand=" select  a.*,b.desc_ 
                            from DCMONITORING a left outer join CONFIG_MST_CYBOSPLUS b on a.stockcode=b.value
                            where   stockcode is not null
                            order   by day desc,hour desc,minute desc ">
        </asp:SqlDataSource>
        <%--<asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="MAKE_DCMONITORING" SelectCommandType="StoredProcedure"></asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" ProviderName="<%$ ConnectionStrings:NorthwindConnectionString.ProviderName %>" SelectCommand="" ></asp:SqlDataSource>
    </body>
    </html>
</asp:Content>
