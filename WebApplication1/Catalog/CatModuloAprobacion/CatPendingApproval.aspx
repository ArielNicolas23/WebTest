<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="CatPendingApproval.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>



<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
        <style>
        .modalbackground
        {
            background-color:black;
            opacity:0.6;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Cambios pendientes de aprobación"></asp:Label>
            <div style="height:32px">
            </div>
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Estatus"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true">
                            <asp:ListItem>Pendiente</asp:ListItem>
                            <asp:ListItem>Aprobado</asp:ListItem>
                            <asp:ListItem>Rechazado</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

            <asp:GridView  ID="dgvPendingApproval" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1050px" AllowPaging="True" DataKeyNames="IdModelsChangesHeader">
                <Columns>
                    <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                    <asp:BoundField DataField="ChangeNumber" HeaderText="No. de Cambio" SortExpression="ChangeNumber" />
                    <asp:BoundField DataField="OriginUser" HeaderText="Originador" SortExpression="OriginUser" />
                    <asp:BoundField DataField="OriginComment" HeaderText="Comentario" SortExpression="OriginComment" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de Edición" SortExpression="ModifiedOn" />
                    <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />
                    <asp:ButtonField CommandName="Action" HeaderText="Acción" ShowHeader="True" DataTextField="Action" />
                    <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                </Columns>
                <RowStyle CssClass="dgvCatalogRowOdd" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle CssClass="dgvPaging"/>
                <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
            </asp:GridView>

            <div style="height: 50px">

            </div>

            <asp:GridView  ID="dgvModelChanges" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="900px" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                    <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                    <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                    <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                    <asp:BoundField DataField="LastUser" HeaderText="Último Usuario" SortExpression="LastUser" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
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




