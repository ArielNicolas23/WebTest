<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CatConfiguracionCalculoDirecto.aspx.vb" MasterPageFile="~/MasterPage.master" Inherits="WebApplication1.CatConfiguracionCalculoDirecto" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .modalbackground
        {
            background-color:black;
            opacity:0.6;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <script type="text/javascript">

                // Get the instance of PageRequestManager.
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                // Add initializeRequest and endRequest
                prm.add_initializeRequest(prm_InitializeRequest);

                // Called when async postback begins
                function prm_InitializeRequest(sender, args) {
                    // Disable button that caused a postback
                    var x = document.getElementById("MainContent_lbAccept");
                    x.className += " disabled-link";

                    var x = document.getElementById("MainContent_lbCancel");
                    x.className += " disabled-link";

                }
                function ClearADFieldsEdit() {
                    //CLEAR DATA
                    document.getElementById("MainContent_txtApproverEdit").value = '';
                    document.getElementById("MainContent_txtUsernameApproverEdit").value = '';
                    document.getElementById("MainContent_txtMailApproverEdit").value = '';
                    //ENABLE FIELDS AND DISABLE REFRESH BUTTON
                    document.getElementById("MainContent_txtApproverEdit").disabled = false;
                    document.getElementById("btnRefreshEdit").disabled = true;
                }
                function ADUserFoundEdit() {

                    if (document.getElementById("MainContent_txtApproverEdit").value.indexOf('||') != -1) {
                        document.getElementById("MainContent_txtUsernameApproverEdit").value = document.getElementById("MainContent_txtApproverEdit").value.split('||')[1].trim();
                        document.getElementById("MainContent_txtMailApproverEdit").value = document.getElementById("MainContent_txtApproverEdit").value.split('||')[2].trim();
                        document.getElementById("MainContent_txtApproverEdit").value = document.getElementById("MainContent_txtApproverEdit").value.split('||')[0].trim();
                        document.getElementById("btnRefreshEdit").disabled = true;

                        document.getElementById("MainContent_txtApproverEdit").disabled = true;
                        document.getElementById("btnRefreshEdit").disabled = false;
                    }
                }

                function ClearADFields() {
                    //CLEAR DATA
                    document.getElementById("MainContent_txtApprover").value = '';
                    document.getElementById("MainContent_txtUsernameApprover").value = '';
                    document.getElementById("MainContent_txtMailApprover").value = '';
                    //ENABLE FIELDS AND DISABLE REFRESH BUTTON
                    document.getElementById("MainContent_txtApprover").disabled = false;
                    document.getElementById("btnRefresh").disabled = true;
                }
                function ADUserFound() {

                    if (document.getElementById("MainContent_txtApprover").value.indexOf('||') != -1) {
                        document.getElementById("MainContent_txtUsernameApprover").value = document.getElementById("MainContent_txtApprover").value.split('||')[1].trim();
                        document.getElementById("MainContent_txtMailApprover").value = document.getElementById("MainContent_txtApprover").value.split('||')[2].trim();
                        document.getElementById("MainContent_txtApprover").value = document.getElementById("MainContent_txtApprover").value.split('||')[0].trim();
                        document.getElementById("btnRefresh").disabled = true;

                        document.getElementById("MainContent_txtApprover").disabled = true;
                        document.getElementById("btnRefresh").disabled = false;
                    }
                }
            </script>
            <style>
.disabled-link {
  pointer-events: none;
}
</style>
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
                                <asp:GridView ID="dgvModelos" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" AllowPaging="False" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
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
                                <asp:GridView  runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="750px" AllowPaging="False" ID="dgvSelectedModels" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
                                    <Columns>
                                        <asp:CommandField HeaderText="Acción" ShowDeleteButton="True" DeleteText="&lt;i class='fa fa-regular fa-trash' style='color:#333333;' data-toggle='tooltip' title='Deseleccionar'&gt;&lt;/i&gt;"/>
                                        <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False"  />
                                        <asp:BoundField DataField="IdCatUnits" Visible="False" />
                                        <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />
                                        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                                        <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />
                                        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
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
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divEditModels" runat="server" visible="false">
                <%-- Aquí la nueva tabla --%>
                <div style="width: 750px; padding: 0px 25px">
                    <div class="catDivHeader">
                        <div style="width: 30%; float: left; text-align: left">
                            <asp:Label ID="lblEditModels" runat="server" Text="Edición de Modelos" CssClass="catHeaderSub"></asp:Label>
                        </div>
                        <div style="margin-left: 30%; text-align: right">
                            <asp:Button ID="cmdCancelEdit" runat="server" Text="Cancelar Edición"  CssClass="catButton" />
                            <asp:Button ID="cmdApproveEdit" runat="server" Text="Aprobar Cambios" CssClass="catButtonAccept" />
                        </div>
                    </div>
                    <div style="margin: 10px 0px">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <asp:GridView  runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" AllowPaging="False" ID="dgvEditModels" DataKeyNames="IdModelsChanges,IdModelsChangesHeader,IdCatUnits">
                        <Columns>
                            <asp:BoundField DataField="IdModelsChanges" Visible="False" ReadOnly="True" />
                            <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                            <asp:BoundField DataField="IdCatUnits" HeaderText="IdUnidad" ReadOnly="True" visible="False" />
                            <%--<asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Modelo" />--%>
                            <asp:TemplateField HeaderText="Modelo">
                                <ItemTemplate>
                                    <asp:Label ID="lblEditModel" runat="server" Text='<%# Eval("Model") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditModel" runat="server" Text='<%# Bind("Model") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />--%>
                            <asp:TemplateField HeaderText="Vida Útil">
                                <ItemTemplate>
                                    <asp:Label ID="lblEditLifespan" runat="server" Text='<%# Eval("Lifespan") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditLifespan" runat="server" Text='<%# Bind("Lifespan") %>' TextMode="Number"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unidad" />--%>
                            <asp:TemplateField HeaderText="Unidad">
                                <ItemTemplate>
                                    <asp:Label ID="lblEditUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="txtEditUnit" runat="server">

                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" ReadOnly="True"/>
                            <asp:BoundField DataField="ApproverUser" HeaderText="Aprobador" SortExpression="ApproverUser" ReadOnly="True"/>
                            <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" ReadOnly="True"/>
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
            </div>






            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground" ></ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="modalpan" runat="server" Height="354px" Width="630px">
                <div id="headerdiv" style= "background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>

                <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                    <table class="auto-style3" width="630">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblModalInstruction" runat="server" Text="Favor de asignar un aprobador para el cambio" Font-Bold="True" ></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblApprover" runat="server" Text="Aprobador:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <div class="button-group">
                                    <asp:TextBox ID="txtApprover" runat="server" Width="260px" onchange="ADUserFound()" ></asp:TextBox>
                                    <button type="button" title="Limpiar campo de Aprobador" class="catModalButton" id="btnRefresh" onclick="ClearADFields()" width="15px" disabled="disabled"><i class="fa fa-refresh"></i></button>
                                </div>
                                <ajaxToolkit:AutoCompleteExtender ID="txtSearchAD_AutoCompleteExtender" runat="server" TargetControlID="txtApprover" ServiceMethod="GetADUsers" ServicePath="../CatCalculoDirecto/wsGetAdUsers.asmx" MinimumPrefixLength="3" CompletionInterval="500" EnableCaching="false" CompletionSetCount="10" CompletionListElementID="AutoCompleteContainer">
                                    
                                </ajaxToolkit:AutoCompleteExtender>

                                <div id="AutoCompleteContainer">

                                </div>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblApproverError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td>

                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtUsernameApprover" runat="server" Width="260px" Enabled="False">

                                </asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>

                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtMailApprover" runat="server" Width="260px" Enabled="False">

                                </asp:TextBox>
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
                                <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updateInProcess" runat="server"> 
                                    <ProgressTemplate>
                                        Procesando          
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Label ID="lblModalMessage" runat="server" ForeColor="Red" Width="504px"></asp:Label>
                            </td>
                        </tr>
                   
                        <tr>
                            <td colspan="3">
                                <div align="center">
                                    <asp:UpdatePanel ID="updateInProcess" runat="server">
                                        <ContentTemplate>
                                            <asp:LinkButton class="catLinkButtonConfirm" ID="lbAccept"  runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
                                            <asp:LinkButton class="catLinkButton" ID="lbCancel"  runat="server" Text="Cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'></i> "></asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel> 
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
