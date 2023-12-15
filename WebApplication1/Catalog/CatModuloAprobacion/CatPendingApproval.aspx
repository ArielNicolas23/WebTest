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
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Cambios pendientes de Aprobación" CssClass="catHeader"></asp:Label>

            <div style="height:32px">

            </div>

            <div runat="server" id="divFilterHeader" visible="true">
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

                    <asp:Label ID="lblDateFilters" runat="server" Text="Buscar por Fecha: " CssClass="catLabel" ></asp:Label>
                    <asp:CheckBox ID="chkDateFilters" runat="server" AutoPostBack="true"/>
                    <asp:LinkButton style="color:#333333;" ID="lBtnSearc"  runat="server" Text="Buscar <i class='fa fa-search' data-toggle='tooltip' title='Buscar'></i> "></asp:LinkButton>
                </div>

                <div runat="server" id="divDateFilters"  class="catDivHeader" visible="false">
                    <asp:Label ID="lblCreatedOn" runat="server" Text="Fecha de Origen: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Button" />
                    <asp:Calendar ID="cldCreatedOn" runat="server" Visible="false"></asp:Calendar>

                    <asp:Label ID="lblCreatedOnTo" runat="server" Text="A: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    <asp:Button ID="Button2" runat="server" Text="Button" />
                    <asp:Calendar ID="cldCreatedOnTo" runat="server" Visible="false"></asp:Calendar>

                    <asp:Label ID="lblApprovedOn" runat="server" Text="Fecha de Aprobación: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    <asp:Button ID="Button3" runat="server" Text="Button" />
                    <asp:Calendar ID="cldApprovedOn" runat="server" Visible="false"></asp:Calendar>

                    <asp:Label ID="lblApprovedOnTo" runat="server" Text="A: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    <asp:Button ID="Button4" runat="server" Text="Button" />
                    <asp:Calendar ID="cldApprovedOnTo" runat="server" Visible="false"></asp:Calendar>
                </div>
            </div>


            <div>
                <asp:GridView  ID="dgvPendingApproval" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="2000px" AllowPaging="True" DataKeyNames="IdModelsChangesHeader,OriginEmail">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                        <asp:BoundField DataField="ChangeNumber" HeaderText="No. de Cambio" SortExpression="ChangeNumber" />
                        <asp:BoundField DataField="OriginName" HeaderText="Nombre Originador" SortExpression="OriginName" />
                        <asp:BoundField DataField="OriginUser" HeaderText="Usuario Originador" SortExpression="OriginUser" />
                        <asp:BoundField DataField="OriginComment" HeaderText="Comentario" SortExpression="OriginComment" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de Edición" SortExpression="ModifiedOn" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />
                        <asp:ButtonField CommandName="Action" HeaderText="Acción" ShowHeader="True" DataTextField="Action" />
                        <asp:BoundField DataField="ApproverName" HeaderText="Nombre Aprobador" SortExpression="ApproverName" />
                        <asp:BoundField DataField="ApproverUser" HeaderText="Usuario Aprobador" SortExpression="ApproverUser" />
                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                        <asp:BoundField DataField="OriginEmail" Visible="False" ReadOnly="True" />
                    </Columns>
                    <RowStyle CssClass="dgvCatalogRowOdd" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="dgvPaging"/>
                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                </asp:GridView>
            </div>

            <div runat="server" id="divModelsChanges" style="margin-top: 50px" visible="false">
                <div style="width: 2%;  float: left;">
                    <asp:Label ID="lblMargin" runat="server" Text=">" CssClass="catIcon"></asp:Label>
                </div>

                <div style="margin-left: 2%;">
                    <div class="catDivHeader" style="width: 1400px;">
                        <div style="width: 30%; float: left; text-align: left">
                            <asp:Label ID="lblModalsChanges" runat="server" Font-Size="18pt" Text="Modelos por Verificar" CssClass="catHeaderSub"></asp:Label>
                        </div>
                        <div style="margin-left: 30%; text-align: right">
                            <asp:Button ID="cmdCancelChange" runat="server" Text="Seleccionar otro Cambio" CssClass="catButton"/>
                            <asp:Button ID="cmdRejectChange" runat="server" Text="Rechazar Cambios" CssClass="catButton" CommandName="Reject" OnClick="ApproveOrReject"/>
                            <asp:Button ID="cmdApproveChange" runat="server" Text="Aprobar Cambios" CssClass="catButtonAccept" CommandName="Approve" OnClick="ApproveOrReject"/>

                        </div>
                    </div>

                    <asp:GridView  ID="dgvModelChanges" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1400px" AllowPaging="True" DataKeyNames="IdModelsChanges">
                        <Columns>
                            <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                            <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                            <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                            <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                            <asp:BoundField DataField="LastUserName" HeaderText="Nombre del Usuario" SortExpression="LastUserName" />
                            <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                            <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
                            <asp:TemplateField HeaderText="Verificar">
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

            </div>



            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="modalpan" runat="server" Height="300px" Width="630px">
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>

                <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                    <table class="auto-style3" width="630">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblModalInstruction" runat="server" Font-Bold="True" ></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" class="auto-style4">
                                <asp:Label ID="lblUser" runat="server" Text="Usuario:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtUser" runat="server" Width="260px" CausesValidation="True"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
 
                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtPassword" runat="server" Width="260px" TextMode="Password"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblApproveMessage" runat="server" Text="Comentarios:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtApproveMessage" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblApproveMessageError" runat="server" ForeColor="Red"></asp:Label>
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
                                    
                                    <asp:LinkButton style="color:#53C400;" ID="lbAccept"  runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
                                    
                                    <asp:LinkButton style="color:#284775;" ID="lbCancel"  runat="server" Text="cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='cancelar'></i> "></asp:LinkButton>
                                    


                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




