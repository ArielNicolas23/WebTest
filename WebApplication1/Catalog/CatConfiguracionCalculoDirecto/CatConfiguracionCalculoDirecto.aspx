<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CatConfiguracionCalculoDirecto.aspx.vb" MasterPageFile="~/MasterPage.master" Inherits="WebApplication1.CatConfiguracionCalculoDirecto" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" CssClass="catHeader" Text="Catálogo de Configuración de Cálculo Directo"></asp:Label>

            <div style="height: 32px">

            </div>
            
            <div id="divHeader" runat="server">
                <div runat="server" class="catDivHeader">
                    <asp:Label ID="lblModel" runat="server" Text="Modelo:" CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="txtModel" runat="server" ></asp:TextBox>

                    <asp:Label ID="lblLifespan" runat="server" Text="Vida Útil:" CssClass="catLabel"></asp:Label>
                    <asp:TextBox ID="txtLifeSpan" runat="server" TextMode="Number"></asp:TextBox>

                    <asp:Label ID="lblUnit" runat="server" Text="Unidad:" CssClass="catLabel"></asp:Label>
                    <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="True" CssClass="catDropDownList" AppendDataBoundItems="true">
                        <asp:ListItem Text="Todas" Value="" /> 
                    </asp:DropDownList>

                    <asp:LinkButton ID="btnSearch" runat="server" class="catLinkButton" Text="Buscar <i class='fa fa-search' data-toggle='tooltip' title='Buscar'></i> " ></asp:LinkButton>
                </div>

                <div style="height: 32px">

                </div>
            </div>


            <div id="divApprovedModels" runat="server">
                <table style="padding: 20px">
                    <tr>
                        <td style="vertical-align: top;">
                            <div style="width: 750px; padding: 0px 25px">
                                <div class="catDivHeader">
                                    <div style="width: 40%; float: left; text-align: left">
                                        <asp:Label ID="lblModels" runat="server" Text="Modelos Aprobados" CssClass="catHeaderSub"></asp:Label>
                                    </div>
                                    <div style="margin-left: 40%; text-align: right">
                                        <asp:Button ID="cmdExportExcel" runat="server" Text="Exportar a Excel"  CssClass="catButton" />
                                        <asp:Button ID="cmdEdit" runat="server" Text="Ir a Editar" CssClass="catButtonAccept" />
                                    </div>
                                </div>
                                <asp:GridView ID="dgvModelos" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" AllowPaging="True" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
                                    <Columns>
                                        <asp:BoundField DataField="IdModelsChanges" Visible="False"  />
                                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False"  />
                                        <asp:BoundField DataField="IdCatUnits" Visible="False" />
                                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
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

                        <td style="vertical-align: top;">
                            <div id="divSelectedModels" style="width: 750px; padding: 0px 25px" runat="server" visible="false">
                                <div class="catDivHeader">
                                    <div style="width: 40%; float: left; text-align: left">
                                        <asp:Label ID="lblSelectedModels" runat="server" Text="Modelos Seleccionados" CssClass="catHeaderSub"></asp:Label>
                                    </div>
                                    <div style="margin-left: 40%; text-align: right">
                                        <asp:Button ID="cmdResetSelected" runat="server" Text="Reiniciar Seleccionados"  CssClass="catButton" />
                                    </div>
                                </div>
                                <asp:GridView  runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="750px" AllowPaging="True" ID="dgvSelectedModels" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
                                    <Columns>
                                        <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False"  />
                                        <asp:BoundField DataField="IdCatUnits" Visible="False" />
                                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                                        
                                        <asp:CommandField HeaderText="Acción" ShowDeleteButton="True" />
                                        
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

            <div id="divEditModels" runat="server" visible="false">
                <%-- Aquí la nueva tabla --%>
                <div class="catDivHeader">
                    <asp:Label ID="Label1" runat="server" Text="Edición de Modelos" CssClass="catHeaderSub"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label>
                </div>
                <asp:GridView  runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="600px" AllowPaging="True" ID="dgvEditModels" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChanges" Visible="False" ReadOnly="true" />
                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                        <asp:BoundField DataField="IdUnit" HeaderText="IdUnidad" ReadOnly="True" visible="false" />
                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                        <asp:TemplateField>
                            <EditItemTemplate>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
