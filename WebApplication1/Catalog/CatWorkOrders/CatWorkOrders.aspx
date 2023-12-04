<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatWorkOrders.aspx.vb" Inherits="WebApplication1.Catalog_CatWorkOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Órdenes de Retrabajo"></asp:Label>

<div style="height: 32px">

</div>

<table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblWorkOrder" runat="server" Text="Orden de Trabajo:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="txtWorkOrder" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblArea" runat="server" Text="Módulo:"></asp:Label>
        </td>
        <td style="height: 17px; width: 128px">
            <asp:DropDownList ID="cmbArea" runat="server" Width="128px">
                <asp:ListItem>Módulo 1</asp:ListItem>
                <asp:ListItem>Módulo 2</asp:ListItem>
                <asp:ListItem>Módulo 3</asp:ListItem>
                <asp:ListItem>Módulo 4</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="height: 17px; width: 196px; text-align: right">
            <asp:CheckBox ID="chkRework" runat="server" Text="Orden Retrabajada" />
        </td>
        <td style="height: 17px; width: 64px">
            <asp:Button ID="btnSearch" runat="server" Text="Buscar" />
        </td>
    </tr>
</table>

<div style="height: 32px">

</div>

<div align="left" style="padding: 4px">
    <asp:LinkButton style="color:#333333;" ID="btnAddWorkOrder" runat="server" Text="<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>" />
</div>

            <div align="left" style="padding: 4px" id="divAgregar" runat="server" visible="false">
    <table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label1" runat="server" Text="Orden:"></asp:Label>
        </td>
        <td style="height: 17px; width: 64px">
            <asp:TextBox ID="addOrden" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
    <asp:Label ID="Label2" runat="server" Text="Módulo:"></asp:Label>
</td>
<td style="height: 17px; width: 64px">
    <asp:DropDownList  ID="addArea" runat="server" Width="128px" Enabled="true" >
 <asp:ListItem>Módulo 1</asp:ListItem>
 <asp:ListItem>Módulo 2</asp:ListItem>
 <asp:ListItem>Módulo 3</asp:ListItem>
 <asp:ListItem>Módulo 4</asp:ListItem>
 </asp:DropDownList>
</td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Button ID="AgregarOrden" runat="server"  Text="Agregar campo" ></asp:Button>               
        </td>
    </tr>
</table>
</div>

<asp:GridView ID="dgvWorkOrders" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="IdCatReworkOrders" Width="680px" AllowPaging="True">
    <Columns>
                       <asp:BoundField DataField="IdCatReworkOrders" HeaderText="IdCatReworkOrders" ReadOnly="True" visible="false">
   <ItemStyle Width="100px" />
   <HeaderStyle HorizontalAlign="Left" />
</asp:BoundField>
        <asp:BoundField DataField="WorkOrder" HeaderText="Orden de Trabajo" SortExpression="workOrder"  />
                       <asp:TemplateField HeaderText="Módulo">
                           <ItemTemplate>
                               <asp:Label  ID="lblArea" runat="server" Text='<%# Bind("Area") %>'>[lblArea]</asp:Label>
                           </ItemTemplate>
                           <EditItemTemplate>
                               <asp:DropDownList DataField="Area"  ID="Area" runat="server" Width="128px" Enabled="true" >
                                <asp:ListItem>Módulo 1</asp:ListItem>
                                <asp:ListItem>Módulo 2</asp:ListItem>
                                <asp:ListItem>Módulo 3</asp:ListItem>
                                <asp:ListItem>Módulo 4</asp:ListItem>
                                </asp:DropDownList>
                               </EditItemTemplate>
                       </asp:TemplateField>
        <asp:CheckBoxField DataField="IsRework" HeaderText="Fue Retrabajada" />
        <asp:BoundField DataField="CreatedOn" HeaderText="Cargada el" ReadOnly="True" SortExpression="createdOn" />
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
