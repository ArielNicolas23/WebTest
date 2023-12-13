<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="CatPendingApproval.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Cambios pendientes de aprobación" CssClass="catHeader"></asp:Label>

            <div style="height:32px">

            </div>

            <div class="catDivHeader">
                <asp:Label ID="lblStatus" runat="server" Text="Estatus: " CssClass="catLabel" ></asp:Label>
                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" CssClass="catDropDownList">
                    <asp:ListItem>Todos</asp:ListItem>
                    <asp:ListItem>Pendiente</asp:ListItem>
                    <asp:ListItem>En Revisión</asp:ListItem>
                    <asp:ListItem>Aprobado</asp:ListItem>
                    <asp:ListItem>Rechazado</asp:ListItem>
                </asp:DropDownList>

                <asp:Label ID="lblRole" runat="server" Text="Rol de Usuario: " CssClass="catLabel" ></asp:Label>
                <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" CssClass="catDropDownList">
                    <asp:ListItem>Ambos</asp:ListItem>
                    <asp:ListItem>Originador</asp:ListItem>
                    <asp:ListItem>Aprobador</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="cmdSearch" runat="server" Text="Buscar Cambios" CssClass="catButton"/>
            </div>

            <div>
                <asp:GridView  ID="dgvPendingApproval" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1600px" AllowPaging="True" DataKeyNames="IdModelsChangesHeader">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                        <asp:BoundField DataField="ChangeNumber" HeaderText="No. de Cambio" SortExpression="ChangeNumber" />
                        <asp:BoundField DataField="OriginUser" HeaderText="Originador" SortExpression="OriginUser" />
                        <asp:BoundField DataField="OriginComment" HeaderText="Comentario" SortExpression="OriginComment" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de Edición" SortExpression="ModifiedOn" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />
                        <asp:ButtonField CommandName="Action" HeaderText="Acción" ShowHeader="True" DataTextField="Action" />
                        <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" />
                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                    </Columns>
                    <RowStyle CssClass="dgvCatalogRowOdd" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="dgvPaging"/>
                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                </asp:GridView>
            </div>

            <div runat="server" id="divModelsChanges" style="margin-top: 50px" visible="false">
                <div class="catDivHeader" style="width: 1200px; text-align: right">
                        <asp:Button ID="cmdCancelChange" runat="server" Text="Seleccionar otro Cambio" CssClass="catButton"/>
                        <asp:Button ID="cmdRejectChange" runat="server" Text="Rechazar Cambios"  CssClass="catButton"/>
                        <asp:Button ID="cmdApproveChange" runat="server" Text="Aprobar Cambios" CssClass="catButtonAccept"/>
                </div>

                <asp:GridView  ID="dgvModelChanges" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1200px" AllowPaging="True" DataKeyNames="IdModelsChanges">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                        <asp:BoundField DataField="LastUser" HeaderText="Último Usuario" SortExpression="LastUser" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="IsChecked" runat="server" AutoPostBack="true" Checked='<%#Convert.ToBoolean(Eval("IsChecked"))%>' OnCheckedChanged="OnChangeIsChecked"/>
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



            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="modalpan" runat="server" Height="354px" Width="630px">
    
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>

                <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                    <table class="auto-style3" width="630">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblModalInstruction" runat="server" Text="Favor de ingresar sus credenciales para confirmar la aprobacion" Font-Bold="True" ></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <!--<td align="left" style="width: 100px">
                                <asp:Label ID="lblApprover" runat="server" Text="Aprobador:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>
                            <td align="left" class="auto-style6">
                                <div class="button-group">
                                                                           <asp:TextBox ID="txtApprover" runat="server" Width="260px" onchange="ADUserFound()" ></asp:TextBox>
                                <button type="button" title="Limpiar campo de Aprobador" class="btn" id="btnRefresh" onclick="ClearADFields()" width="15px" disabled="disabled"><i class="fa fa-refresh"></i></button>
                                    </div>
                                                   <ajaxToolkit:AutoCompleteExtender ID="txtSearchAD_AutoCompleteExtender" runat="server" TargetControlID="txtApprover" ServiceMethod="GetADUsers" ServicePath="wsGetAdUsers.asmx"
    MinimumPrefixLength="3" CompletionInterval="500" EnableCaching="false" CompletionSetCount="10" CompletionListElementID="AutoCompleteContainer"  >
</ajaxToolkit:AutoCompleteExtender>
        <div id="AutoCompleteContainer"></div>
                   
                            </td>
                            <td class="auto-style5">
                                <asp:Label ID="lblApproverError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblApproverError" runat="server" ControlToValidate="txtApprover" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>-->

                        <tr>
                            <td align="left" class="auto-style4" style="height: 26px">
                                <asp:Label ID="lblUser" runat="server" Text="Usuario:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>
                            <td align="left" class="auto-style6" style="width: 319px; height: 26px;">
                                <asp:TextBox ID="txtUser" runat="server" Width="260px" CausesValidation="True"></asp:TextBox>
                            </td>
                            <td class="auto-style5" style="height: 26px">
                                <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblUserError" runat="server" ControlToValidate="txtUser" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
 
                        <tr>
                            <td align="left" style="width: 100px; height: 26px;">
                                <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>
                            <td align="left" class="auto-style6" style="width: 319px; height: 26px;">
                                <asp:TextBox ID="txtPassword" runat="server" Width="260px" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="auto-style5" style="height: 26px">
                                <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblPassworkError" runat="server" ControlToValidate="txtPassword" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblApproveMessage" runat="server" Text="Comentarios:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>
                            <td align="left" class="auto-style6" style="width: 319px">
                                <asp:TextBox ID="txtApproveMessage" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td class="auto-style5">
                                <asp:Label ID="lblApproveMessageError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblApproveMessageError" runat="server" ControlToValidate="txtApproveMessage" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>

                        <tr style="margin-top: 10px; margin-bottom: 10px">
                            <td colspan="3">
                                <asp:Label ID="lblModalMessage" runat="server" ForeColor="Red" Width="504px"></asp:Label>
                            </td>
                        </tr>
                   
                        <tr>
                            <td colspan="3">
                                <div align="center">
                                    <asp:Button ID="cmdAcceptChange" runat="server" BackColor="#53C400" ForeColor="#FFFFFF" Text="Aceptar" />
                                    <asp:Button ID="cmdCancelModal" runat="server" BackColor="#284775" ForeColor="#FFFFFF" Text="Cancelar" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!--<div style=" background-color:gray; color:white; padding:2px;">
                    <asp:Button ID="Buttonpopup" runat="server" Text="X" />
                </div>-->
            </asp:Panel>
            <!--rechazado-->
            <br />



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




