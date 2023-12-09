<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="CatModuloAprobacion.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>



<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Fecha pendiente de aprobacion"></asp:Label>
            <div style="height:32px">
            </div>
            <table>
                <tr>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Estatus"></asp:Label>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="estatus" DataTextField="approvalstatus" DataValueField="approvalstatus" Width="69px"></asp:DropDownList>
                        <asp:SqlDataSource ID="estatus" runat="server" ConnectionString="<%$ ConnectionStrings:FechaexpiracionConnectionString %>" SelectCommand="select distinct approvalstatus from ExpDate.ED_ModelsChangesHeader"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>

                       <asp:GridView  ID="gvPendientes" runat="server" AutoGenerateColumns="false"  CellPadding="4" CssClass="dgvCatalog" GridLines="None" Width="680px" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="ChangeNumber" HeaderText="ChangeNumber" SortExpression="ChangeNumber" />
                    <asp:BoundField DataField="OriginUser" HeaderText="OriginUser" SortExpression="OriginUser" />
                    <asp:BoundField DataField="OriginComment" HeaderText="OriginComment" SortExpression="OriginComment" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de edicion" SortExpression="ModifiedOn" />
                    <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />

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




