<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="CatPendingApproval.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>



<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Cambios pendientes de aprobación" CssClass="catHeader"></asp:Label>
            <div style="height:32px">
            </div>
            <div style="float: left; margin-bottom: 10px"">
                <asp:Label ID="lblStatus" runat="server" Text="Estatus" CssClass="catLabel"></asp:Label>
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true">
                    <asp:ListItem>Pendiente</asp:ListItem>
                    <asp:ListItem>Aprobado</asp:ListItem>
                    <asp:ListItem>Rechazado</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="cmdShowPending" runat="server" Text="Mostrar solo Cambios Pendientes" CssClass="catButton"/>
            </div>

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

            <div runat="server" id="divModelsChanges" style="margin-top: 50px" visible="true">
                <div style="width: 900px; margin-bottom: 10px">
                    <div style="float: right">
                        <asp:Button ID="cmdCancelChange" runat="server" Text="Seleccionar otro Cambio" CssClass="catButton"/>
                        <asp:Button ID="cmdRejectChange" runat="server" Text="Rechazar Cambios"  CssClass="catButton"/>
                        <asp:Button ID="cmdApproveChange" runat="server" Text="Aprobar Cambios" CssClass="catButtonAccept"/>
                    </div>
                </div>
                <asp:GridView  ID="dgvModelChanges" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1050px" AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                        <asp:BoundField DataField="LastUser" HeaderText="Último Usuario" SortExpression="LastUser" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
                        <asp:CheckBoxField DataField="IsChecked" SortExpression="IsChecked" />
                    </Columns>
                    <RowStyle CssClass="dgvCatalogRowOdd" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="dgvPaging"/>
                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




