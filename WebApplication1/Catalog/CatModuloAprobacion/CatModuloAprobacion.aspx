<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="CatModuloAprobacion.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>



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
                    <td style="height: 17px; width: 151px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Estatus"></asp:Label>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="estatus" DataTextField="approvalstatus" DataValueField="approvalstatus" Width="91px" Height="17px" style="margin-left: 0px" AutoPostBack="true"></asp:DropDownList><!--autopostback para que funcione el indexchanged-->
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
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Checkbox" runat="server" />

                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="dgvCatalogRowOdd" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle CssClass="dgvPaging"/>
                <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" Text="seleccionar" />
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Label2" PopupControlID="PanelAprobacion" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="PanelAprobacion" runat="server" Style="width:570px; height:450px">
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>
                <div style="background-color:white;  font-size:medium; padding:2px;" class="auto-style1">
                    <asp:GridView ID="gvSeleccionado" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ChangeNumber" HeaderText="ChangeNumber" SortExpression="ChangeNumber" />
                            <asp:BoundField DataField="OriginUser" HeaderText="OriginUser" SortExpression="OriginUser" />
                            <asp:BoundField DataField="OriginComment" HeaderText="OriginComment" SortExpression="OriginComment" />
                            <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de edicion" SortExpression="ModifiedOn" />
                            <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <label> Revisado</label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    
                                    <asp:CheckBox ID="Checkbox" runat="server" />

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>


                    </asp:GridView>
                    <asp:Button ID="BotonAceptarCambio" runat="server" BackColor="#53C400" ForeColor="#FFFFFF" Text="Aceptar"  />
                    <asp:Button ID="BotonCancelarCambio" runat="server" BackColor="#284775" ForeColor="#FFFFFF" Text="Cancelar" />
                </div>

            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




