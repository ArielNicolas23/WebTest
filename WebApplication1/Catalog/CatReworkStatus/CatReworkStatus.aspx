<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatReworkStatus.aspx.vb" Inherits="WebApplication1.Catalog_CatReworkStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Status de Retrabajo"></asp:Label>

<div style="height: 32px">

</div>

<table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblStatus" runat="server" Text="Status de SAP:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:CheckBox ID="chkRework" runat="server" Text="Es Retrabajo" />
        </td>
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnSearch" runat="server" Text="Buscar" />
        </td>
    </tr>
</table>

<div style="height: 32px">

</div>

<div align="left" style="padding: 4px">
    <asp:Button ID="btnAddStatus" runat="server" Text="Agregar Status" />
</div>

<asp:GridView ID="dgvStatusTable" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames = "IdCatReworkStatus">
    <Columns>
               <asp:BoundField DataField="IdCatReworkStatus" HeaderText="IdUnit" ReadOnly="True" visible="false">
   <ItemStyle Width="100px" />
   <HeaderStyle HorizontalAlign="Left" />
</asp:BoundField>
        <asp:BoundField DataField="SAPStatus" HeaderText="SAP Status" ReadOnly="True" SortExpression="sapStatus" />
        <asp:CheckBoxField DataField="IsRework" HeaderText="Es Retrabajo" ReadOnly="True" />
        <asp:ButtonField CommandName="Editar" Text="Editar" ButtonType="Button" />
        <asp:ButtonField ButtonType="Button" CommandName="Eliminar" Text="Eliminar" />
    </Columns>
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <EditRowStyle BackColor="#999999" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
</asp:Content>
