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
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnReset" runat="server" Text="Reiniciar" />
        </td>
    </tr>
</table>

<div>
    <br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label><br />

    <br />
</div>

<div align="left" style="padding: 4px">
    <asp:LinkButton style="color:#333333;" ID="btnAddStatus" runat="server" Text="<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>" />
</div>
    <div align="left" style="padding: 4px" id="divAgregar" runat="server" visible="false">
    <table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label1" runat="server" Text="Status de SAP:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="addEstatus" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:CheckBox ID="addRetrabajo" runat="server" Text="Es Retrabajo" />
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Button ID="AgregarEstatus" runat="server"  Text="Agregar campo" ></asp:Button>          
        </td>
    </tr>
</table>
</div>
    <asp:DropDownList ID="DropDownListP" runat="server" AutoPostBack="True">
    <asp:ListItem>10</asp:ListItem>
    <asp:ListItem>50</asp:ListItem>
    <asp:ListItem>100</asp:ListItem>
    <asp:ListItem>Todos</asp:ListItem>
</asp:DropDownList>
<asp:GridView ID="dgvStatusTable" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames = "IdCatReworkStatus" Width="480px" AllowPaging="True">
    <Columns>
               <asp:BoundField DataField="IdCatReworkStatus" HeaderText="IdUnit" ReadOnly="True" visible="false">
   <ItemStyle Width="100px" />
   <HeaderStyle HorizontalAlign="Left" />
</asp:BoundField>
        <asp:BoundField DataField="SAPStatus" HeaderText="SAP Status" SortExpression="sapStatus" />
        <asp:CheckBoxField DataField="IsRework" HeaderText="Es Retrabajo" />
               <asp:CommandField ShowDeleteButton="True" ButtonType="Link" ShowEditButton="True" CancelText="<i class='fa fa-regular fa-ban' data-toggle='tooltip' title='Cancelar'></i>" UpdateText="<i class='fa fa-regular fa-check' data-toggle='tooltip' title='Actualizar'></i>" EditText="<i class='fa fa-regular fa-edit' data-toggle='tooltip' title='Editar campo'></i>" DeleteText="<i class='fa fa-regular fa-trash' data-toggle='tooltip' title='Eliminar campo'></i>" />
    </Columns>
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <EditRowStyle BackColor="#999999" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
</asp:Content>
