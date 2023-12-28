<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatReworkStatus.aspx.vb" Inherits="WebApplication1.Catalog_CatReworkStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
    <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Status de Retrabajo" CssClass="catHeader"></asp:Label>

<div style="height: 32px">

</div>
            <div class="catDivHeader">
                <asp:Label ID="lblStatus" runat="server" Text="Status de SAP:" CssClass="catLabel"></asp:Label>
                <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
                <asp:CheckBox ID="chkRework" runat="server" Text="Es Retrabajo"  />
                <asp:LinkButton ID="lBtnSearch"  CssClass="catLinkButton" runat="server" Text="<i class='fa fa-search' data-toggle='tooltip' title='Buscar'></i>" />
                <asp:LinkButton ID="lBtnReset" CssClass="catLinkButton" runat="server" Text="<i class='fa fa-regular fa-rotate-right' data-toggle='tooltip' title='Reiniciar'></i>" />
            </div>

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
            &nbsp;</td>
        <td style="height: 17px; width: 64px">
            &nbsp;</td>
        <td style="height: 17px; width: 128px; text-align: right">
            &nbsp;</td>
        <td style="height: 17px; width: 128px; text-align: right">
            &nbsp;</td>
    </tr>
</table>
</div>
<asp:GridView ID="dgvStatusTable" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog"   GridLines="None" DataKeyNames = "IdCatReworkStatus" Width="450px" AllowPaging="True">
    <Columns>
               <asp:BoundField DataField="IdCatReworkStatus" HeaderText="IdUnit" ReadOnly="True" visible="false">
   
   <HeaderStyle HorizontalAlign="Left" />
</asp:BoundField>
        <asp:BoundField DataField="SAPStatus" HeaderText="SAP Status" SortExpression="sapStatus" ></asp:BoundField>
        <asp:CheckBoxField DataField="IsRework" HeaderText="Es Retrabajo" ></asp:CheckBoxField >
               <asp:TemplateField ShowHeader="True">
                                <HeaderTemplate >
                 <asp:DropDownList ID="DropDownListc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListc_SelectedIndexChanged">
                     <asp:ListItem>----</asp:ListItem>
    <asp:ListItem>10</asp:ListItem>
    <asp:ListItem>50</asp:ListItem>
    <asp:ListItem>100</asp:ListItem>
    <asp:ListItem>Todos</asp:ListItem>
</asp:DropDownList>
             </HeaderTemplate>
                   <EditItemTemplate >

                       <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="&lt;i class='fa fa-regular fa-check' style='color:#333333;' data-toggle='tooltip' title='Actualizar'&gt;&lt;/i&gt;"></asp:LinkButton>
                       &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="&lt;i class='fa fa-regular fa-ban' style='color:#333333;' data-toggle='tooltip' title='Cancelar'&gt;&lt;/i&gt;"></asp:LinkButton>
                   </EditItemTemplate>
                   <ItemTemplate>
                       <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="&lt;i class='fa fa-regular fa-edit' style='color:#333333;' data-toggle='tooltip' title='Editar campo'&gt;&lt;/i&gt;"></asp:LinkButton>
                       &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="&lt;i class='fa fa-regular fa-trash' style='color:#333333;' data-toggle='tooltip' title='Eliminar campo'&gt;&lt;/i&gt;"></asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
    </Columns>
    <RowStyle CssClass="dgvCatalogRowOdd" />
     <EditRowStyle BackColor="#999999" />
     <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
     <PagerStyle CssClass="dgvPaging"/>
     <AlternatingRowStyle CssClass="dgvCatalogRowEven" />

</asp:GridView>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
