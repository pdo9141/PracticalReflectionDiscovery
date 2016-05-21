<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OrderTaker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Commander's Warehouse</h1>
    </div>

    <asp:PlaceHolder ID="pldrMessages" runat="server" Visible="false">
        <h2 style="color: green"><asp:Literal ID="ltrMessage" runat="server"></asp:Literal></h2>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="pldrErrors" runat="server" Visible="false">
        <asp:Repeater ID="rptErrors" runat="server">
            <HeaderTemplate><ul></HeaderTemplate>
            <ItemTemplate>
                <li><strong><%# DataBinder.Eval(Container.DataItem, "Message") %></strong><br />
                    <%# DataBinder.Eval(Container.DataItem, "TypeName") %><br />
                    <%# DataBinder.Eval(Container.DataItem, "AssemblyName") %></li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>
    </asp:PlaceHolder>

    <div class="row">
        <table cellpadding="8" cellspacing="8">
            <tr>
                <td>Customer:</td>
                <td colspan="2"><asp:DropDownList ID="ddlCustomer" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Rating:</td>
                <td colspan="2"><asp:Literal ID="ltrRating" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Customer Since:</td>
                <td colspan="2"><asp:Literal ID="ltrCustomerSince" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Repeater ID="rptItemsAdded" runat="server" OnItemDataBound="rptItemsAdded_ItemDataBound">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td><asp:Literal ID="ltrProductName" runat="server"></asp:Literal></td>
                                    <td><asp:Literal ID="ltrProductQuantity" runat="server"></asp:Literal></td>
                                    <td><asp:Literal ID="ltrProductTotal" runat="server"></asp:Literal></td>
                                    <td><asp:Button ID="btnDeleteItem" runat="server" Text="Delete" OnClick="btnDeleteItem_Click" /></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>Discount: <asp:TextBox ID="txtDiscount" ReadOnly="true" runat="server"></asp:TextBox>%<br />
                    Total: <asp:Literal ID="ltrTotal" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>Product:</td>
                <td><asp:DropDownList ID="ddlProduct" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddItem_Click" /></td>
                <td rowspan="3"><asp:Button ID="btnSubmitOrder" Text="Submit Order" runat="server" OnClick="btnSubmitOrder_Click" /></td>
            </tr>            
            <tr>
                <td>Unit Price:</td>
                <td><asp:Literal ID="ltrUnitPrice" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Quantity:</td>
                <td><asp:TextBox ID="txtQuantity" runat="server" Text="1"></asp:TextBox></td>
            </tr>
        </table>
    </div>

</asp:Content>
