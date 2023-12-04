<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatUnits.aspx.vb" Inherits="WebApplication1.Catalog_CatUnits" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Unidades"></asp:Label>

<div style="height: 32px">

</div>

<table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblUnit" runat="server" Text="Unidad:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblUnitValue" runat="server" Text="Valor de Unidad:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="txtUnitValue" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnSearch" runat="server" Text="Buscar" />
        </td>
    </tr>
</table>

<div style="height: 32px">

</div>

<div align="left" style="padding: 4px">
    <asp:Button ID="btnAddUnit" runat="server" Text="Agregar Unidad" />
</div>

<asp:GridView ID="dgvUnits" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames = "IdUnit">
    <Columns>
         <asp:BoundField DataField="IdUnit" HeaderText="IdUnit" ReadOnly="True" visible="false">
     <ItemStyle Width="100px" />
     <HeaderStyle HorizontalAlign="Left" />
  </asp:BoundField>
 <asp:BoundField DataField="Unit" HeaderText="Unit">
     <ItemStyle Width="100px" />
     <HeaderStyle HorizontalAlign="Left" />
 </asp:BoundField>
 <asp:BoundField DataField="UnitValue" HeaderText="UnitValue" ReadOnly="true">
     <ItemStyle Width="200px" />
     <HeaderStyle HorizontalAlign="Left" />
 </asp:BoundField>
 <asp:CheckBoxField DataField="IsActive" HeaderText="Activo" ReadOnly="true">
     <ItemStyle HorizontalAlign="Center" Width="50px" />
     <HeaderStyle HorizontalAlign="Center" />
 </asp:CheckBoxField>
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
