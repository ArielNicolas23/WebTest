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
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnSearch" runat="server" Text="Buscar" />
        </td>
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnReset" runat="server" Text="Reiniciar" />
        </td>
    </tr>
</table>

<div style="height: 32px">

</div>

<div align="left" style="padding: 4px">
    <asp:LinkButton style="color:#333333;" ID="btnAddUnit" runat="server" Text="<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>" />
</div>

        <div align="left" style="padding: 4px" id="divAgregar" runat="server" visible="false">
    <table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label1" runat="server" Text="Unidad:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="addUnidad" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
    <asp:Label ID="Label2" runat="server" Text="Valor:"></asp:Label>
</td>
<td style="height: 17px; width: 64px">
    <asp:TextBox ID="addValor" runat="server"></asp:TextBox>
</td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Button ID="AgregarUnidad" runat="server"  Text="Agregar campo" ></asp:Button>               
        </td>
    </tr>
</table>
</div>

<asp:GridView ID="dgvUnits" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames = "IdUnit" Width="480px" AllowPaging="True">
    <Columns>
         <asp:BoundField DataField="IdUnit" HeaderText="IdUnidad" ReadOnly="True" visible="false">
     <ItemStyle Width="100px" />
     <HeaderStyle HorizontalAlign="Center" />
  </asp:BoundField>
 <asp:BoundField DataField="Unit" HeaderText="Unidad">
     <ItemStyle Width="200px" />
     <HeaderStyle HorizontalAlign="Center" />
 </asp:BoundField>
 <asp:BoundField DataField="UnitValue" HeaderText="Valor" ReadOnly="false">
     <ItemStyle Width="100px" />
     <HeaderStyle HorizontalAlign="Center" />
 </asp:BoundField>
         <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" CancelText="<i class='fa fa-regular fa-ban' data-toggle='tooltip' title='Cancelar'></i>" UpdateText="<i class='fa fa-regular fa-check' data-toggle='tooltip' title='Actualizar'></i>" EditText="<i class='fa fa-regular fa-edit' data-toggle='tooltip' title='Editar campo'></i>" DeleteText="<i class='fa fa-regular fa-trash' data-toggle='tooltip' title='Eliminar campo'></i>" />
    </Columns>
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <EditRowStyle BackColor="#999999" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
</asp:Content>
