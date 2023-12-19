<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CatConfiguracionCalculoDirecto.aspx.vb" MasterPageFile="~/MasterPage.master" Inherits="WebApplication1.CatConfiguracionCalculoDirecto" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Configuración de Cálculo Directo"></asp:Label>

            <div style="height: 32px">
            </div>      
            <table>
                <tr>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="lblModel" runat="server" Text="Modelo:" Width="70px"></asp:Label>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="lblLifespan" runat="server" Text="Vida Útil:" Width="70px"></asp:Label>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:TextBox ID="txtLifeSpan" runat="server"></asp:TextBox>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="lblUnit" runat="server" Text="Unidad:" Width="70px"></asp:Label>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="True" Height="16px" style="margin-bottom: 0px"></asp:DropDownList>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Button ID="cmdSearch" runat="server" Text="Buscar" BackColor="#004680" ForeColor="#FFFFFF" />
                    </td>
                </tr>
            </table>

            <div style="width: 750px">
                <div style="float: right">
                    <asp:CheckBox ID="FillWithStrings" runat="server" visible="false"/>
                    <asp:Button ID="cmdExportExcel" runat="server" Text="Exportar a Excel"  BackColor="#E2DED6" ForeColor="Black" />
                    <asp:Button ID="cmdEdit" runat="server" Text="  Ir a Editar  " BackColor="#284775" ForeColor="#FFFFFF" />
                </div>
            </div>

            <div id="divApprovedModels">
                <table style="padding: 20px">
                    <tr>
                        <td>
                            <div>
                                <asp:Label ID="lblModels" runat="server" Font-Size="18pt" Text="Modelos Aprobados" CssClass="catHeaderSub"></asp:Label>
                                <asp:GridView ID="dgvModelos" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="700px" AllowPaging="True">
                                    <Columns>
                                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                                        <asp:BoundField DataField="IdModelsChanges" Visible="False" ReadOnly="true" />
                                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                                        <asp:CommandField HeaderText="Acción" ShowSelectButton="True" />
                                    </Columns>
                                    <RowStyle CssClass="dgvCatalogRowOdd" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle CssClass="dgvPaging"/>
                                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td>
                            <div id="divSelectedModels">
                                <asp:Label ID="lblSelectedModels" runat="server" Font-Size="18pt" Text="Modelos Seleccionados" CssClass="catHeaderSub"></asp:Label>
                                <asp:GridView  runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="600px" AllowPaging="True" ID="dgvSelectedModels">
                                    <Columns>
                                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                                        <asp:BoundField DataField="IdModelsChanges" Visible="False" ReadOnly="true" />
                                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                                    </Columns>
                                    <RowStyle CssClass="dgvCatalogRowOdd" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle CssClass="dgvPaging"/>
                                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divEditModels">
                <%-- Aquí la nueva tabla --%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
