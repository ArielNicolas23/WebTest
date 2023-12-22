<%@  Page Async="true" Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AgregarModelo.aspx.vb" Inherits="WebApplication1.WebForm1" %>



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

                                            
                         <div runat="server" id="divError" visible="false">
                <asp:Label ID="Label6" runat="server" Font-Size="18pt" Text="Error, no se encontro la informacion solicitada"></asp:Label>

            </div>
            <div runat="server" id="divModelsNew" visible="false">
                <asp:Label ID="Label5" runat="server" Font-Size="18pt" Text="Nueva Configuración de Cálculo Directo" CssClass="catHeader"></asp:Label>

<div style="height: 32px">

</div>     
            <table>
                <tr style="height: 30px;">
                    <td style="width: 128px; text-align: right">
                        <asp:Label ID="lblModel" runat="server" Text="Modelo:" CssClass="catLabel"></asp:Label>
                    </td> 
                    <td style="width: 128px; text-align: right">
                        <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                    </td>
                </tr>
            
                <tr style="height: 30px;">
                    <td style="width: 128px; text-align: right">   
                        <asp:Label ID="lblLifespan" runat="server" Text="Vida Útil:" CssClass="catLabel" ></asp:Label>
                    </td> 
                    <td style="width: 128px; text-align: right">
                        <asp:TextBox ID="txtLifeSpan" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 128px; text-align: right">
                        <asp:Label ID="lblUnit" runat="server" Text="Unidad:" CssClass="catLabel"></asp:Label>
                    </td> 
                    <td style="width: 128px; text-align: right">
                        <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="True" CssClass="catDropDownList">
                        </asp:DropDownList>
                    </td>

                    <td>
                        <asp:LinkButton class="catLinkButtonAccept" ID="ldModel"  runat="server" Text="Agregar <i class='fa fa-plus' data-toggle='tooltip' title='Agregar'></i> "></asp:LinkButton>
                    </td>
                </tr>
            </table>

            <div>
                <br />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="100%"></asp:Label><br />

                <br />
            </div>
        
            <div runat="server" id="divButtons" visible="true" style="width: 750px; margin-bottom: 32px">
                <div style="float: right">
                    <asp:LinkButton class="catLinkButton" ID="lbCancelChange"  runat="server" Text="Limpiar Carga de Modelos &lt;i class='fa fa-rotate-right' data-toggle='tooltip' title='Reiniciar Carga de Modelos'&gt;&lt;/i&gt; "></asp:LinkButton>
                    <asp:LinkButton class="catLinkButtonAccept" ID="lbOpenApprove"  runat="server" Text="Enviar para Aprobación <i class='fa fa-envelope' data-toggle='tooltip' title='Enviar para Aprobación'></i> "></asp:LinkButton>
                </div>
            </div>

            <!-- Cosas-->
            <asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="dgvCatalog" GridLines="None" Width="750px" AllowPaging="True" ID="gvModelos">
                <Columns>
                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                    <asp:BoundField DataField="VidaUtil" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                    <asp:BoundField DataField="IdUnidad" Visible="False" />
                    <asp:CommandField HeaderText="Acción" ShowDeleteButton="True" DeleteText="<i class='fa fa-trash' data-toggle='tooltip' title='Reiniciar Carga de Modelos'></i>" />
                </Columns>
                <RowStyle CssClass="dgvCatalogRowOdd" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle CssClass="dgvPaging"/>
                <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
            </asp:GridView>

            </div>
            <div runat="server" id="divModelEdit" visible="false">
                <asp:Label ID="lblTitle" runat="server"  Font-Size="18pt" Text="Editar Configuración de Cálculo Directo"></asp:Label>

<div style="height: 32px">

</div>     
<table>
    <tr>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label1" runat="server" Text="Modelo:" Width="70px" CssClass="catLabel"></asp:Label>
        </td> 
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:TextBox ID="txtModelEdit" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td style="height: 17px; width: 128px; text-align: right">   
            <asp:Label ID="Label2" runat="server" Text="Vida Útil:" Width="70px" CssClass="catLabel"></asp:Label>
        </td> 
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:TextBox ID="txtLifeSpanEdit" runat="server"></asp:TextBox>
        </td>
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label3" runat="server" Text="Unidad:" CssClass="catLabel"></asp:Label>
        </td> 
        <td style="height: 17px; width: 128px; text-align: right">
            <asp:DropDownList ID="ddlUnitEdit" runat="server" AutoPostBack="True" Height="16px">
            </asp:DropDownList>
        </td>

        <td>
            <asp:LinkButton class="catLinkButtonAccept" ID="ldModelEdit"  runat="server" Text="Agregar <i class='fa fa-plus' data-toggle='tooltip' title='Agregar'></i> "></asp:LinkButton>
        </td>
    </tr>
</table>

<div>
    <br />

    <asp:Label ID="lblMessageEdit" runat="server" ForeColor="Red" Height="12px" Width="100%"></asp:Label><br />

    <br />
</div>
        
<div runat="server" id="divButtonsEdit" visible="true" style="width: 750px; margin-bottom: 32px">
    <div style="float: right">
        <asp:LinkButton class="catLinkButton" ID="lbCancelChangeEdit"  runat="server" Text="Limpiar Carga de Modelos &lt;i class='fa fa-rotate-right' data-toggle='tooltip' title='Reiniciar Carga de Modelos'&gt;&lt;/i&gt; "></asp:LinkButton>
        <asp:LinkButton class="catLinkButtonAccept" ID="lbOpenApproveEdit"  runat="server" CssClass="catButtonAccept" Text="Enviar para Aprobación <i class='fa fa-envelope' data-toggle='tooltip' title='Enviar para Aprobación'></i> "></asp:LinkButton>
    </div>
</div>

<!-- Cosas-->
<asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames = "IdModelsChanges" CssClass="dgvCatalog" GridLines="None" Width="750px" AllowPaging="True" ID="dgvEdit">
    <Columns>
        <asp:BoundField DataField="IdModelsChanges" HeaderText="IdModelsChanges" ReadOnly="True" visible="false"/>
        <asp:BoundField DataField="Model" HeaderText="Modelo" />
        <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="VidaUtil" />
        <asp:TemplateField HeaderText="Unidad" SortExpression="Unidad">
            <EditItemTemplate>
                <asp:DropDownList ID="ddlUnitEditGrid" runat="server">
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="Usuario" ReadOnly="True" />
        <asp:CommandField HeaderText="Acción" ShowDeleteButton="True" DeleteText="<i class='fa fa-trash' data-toggle='tooltip' title='Reiniciar Carga de Modelos'></i>" EditText="&lt;i class='fa fa-regular fa-edit' style='color:#333333;' data-toggle='tooltip' title='Editar campo'&gt;&lt;/i&gt;" ShowEditButton="True" CancelText="&lt;i class='fa fa-regular fa-ban' style='color:#333333;' data-toggle='tooltip' title='Cancelar edicion'&gt;&lt;/i&gt;" UpdateText="&lt;i class='fa fa-regular fa-check' style='color:#333333;' data-toggle='tooltip' title='Editar campo'&gt;&lt;/i&gt;" />
        <asp:CheckBoxField HeaderText="IsDeleted" InsertVisible="False" ShowHeader="False" Visible="False" />
    </Columns>
    <RowStyle CssClass="dgvCatalogRowOdd" />
    <EditRowStyle BackColor="#999999" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle CssClass="dgvPaging"/>
    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
</asp:GridView>

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
                                <ajaxToolkit:AutoCompleteExtender ID="txtSearchAD_AutoCompleteExtender" runat="server" TargetControlID="txtApprover" ServiceMethod="GetADUsers" ServicePath="wsGetAdUsers.asmx" MinimumPrefixLength="3" CompletionInterval="500" EnableCaching="false" CompletionSetCount="10" CompletionListElementID="AutoCompleteContainer">
                                    
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
                                 <asp:UpdateProgress ID="updProgress"
 AssociatedUpdatePanelID="updateInProcess"
 runat="server">
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


                       <asp:Label ID="lblModalEdit" runat="server"></asp:Label>
           <ajaxToolkit:ModalPopupExtender ID="ApproveModalEdit" runat="server" TargetControlID="lblModalEdit" PopupControlID="ModalPanEdit" PopupDragHandleControlID="headerdivEdit" BackgroundCssClass="modalbackground" ></ajaxToolkit:ModalPopupExtender>
           <asp:Panel ID="ModalPanEdit" runat="server" Height="354px" Width="630px">
               <div id="headerdivEdit" style= "background-color:#284775;  font-size:medium; padding:2px; height:20px;">

               </div>

               <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                   <table class="auto-style3" width="630">
                       <tr>
                           <td colspan="3">
                               <asp:Label ID="Label7" runat="server" Text="Favor de asignar un aprobador para el cambio" Font-Bold="True" ></asp:Label>
                           </td>
                       </tr>

                       <tr>
                           <td align="left" style="width: 100px">
                               <asp:Label ID="Label8" runat="server" Text="Aprobador:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style6">
                               <div class="button-group">
                                   <asp:TextBox ID="txtApproverEdit" runat="server" Width="260px" onchange="ADUserFoundEdit()" ></asp:TextBox>
                                   <button type="button" title="Limpiar campo de Aprobador" class="catModalButton" id="btnRefreshEdit" onclick="ClearADFields()" width="15px" disabled="disabled"><i class="fa fa-refresh"></i></button>
                               </div>
                               <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtApproverEdit" ServiceMethod="GetADUsers" ServicePath="wsGetAdUsers.asmx" MinimumPrefixLength="3" CompletionInterval="500" EnableCaching="false" CompletionSetCount="10" CompletionListElementID="AutoCompleteContainerEdit">
                                   
                               </ajaxToolkit:AutoCompleteExtender>

                               <div id="AutoCompleteContainerEdit">

                               </div>
                           </td>

                           <td class="auto-style5">
                               <asp:Label ID="lblApproverErrorEdit" runat="server" ForeColor="Red"></asp:Label>
                           </td>
                       </tr>

                       <tr>
                           <td>

                           </td>

                           <td align="left" class="auto-style6">
                               <asp:TextBox ID="txtUsernameApproverEdit" runat="server" Width="260px" Enabled="False"></asp:TextBox>
                           </td>
                       </tr>

                       <tr>
                           <td>

                           </td>

                           <td align="left" class="auto-style6">
                               <asp:TextBox ID="txtMailApproverEdit" runat="server" Width="260px" Enabled="False"></asp:TextBox>
                           </td>
                       </tr>

                       <tr>
                           <td align="left" class="auto-style4">
                               <asp:Label ID="Label10" runat="server" Text="Usuario:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style6">
                               <asp:TextBox ID="txtUserEdit" runat="server" Width="260px" CausesValidation="True"></asp:TextBox>
                           </td>

                           <td class="auto-style5">
                               <asp:Label ID="lblUserErrorEdit" runat="server" ForeColor="Red"></asp:Label>
                           </td>
                       </tr>

                       <tr>
                           <td align="left" style="width: 100px">
                               <asp:Label ID="Label12" runat="server" Text="Contraseña:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style6">
                               <asp:TextBox ID="txtPasswordEdit" runat="server" Width="260px" TextMode="Password"></asp:TextBox>
                           </td>

                           <td class="auto-style5">
                               <asp:Label ID="lblPasswordErrorEdit" runat="server" ForeColor="Red"></asp:Label>
                           </td>
                       </tr>

                       <tr>
                           <td align="left" style="width: 100px">
                               <asp:Label ID="Label14" runat="server" Text="Comentarios:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style6">
                               <asp:TextBox ID="txtApproveMessageEdit" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                           </td>

                           <td class="auto-style5">
                               <asp:Label ID="lblApproveMessageErrorEdit" runat="server" ForeColor="Red"></asp:Label>
                           </td>
                       </tr>

                       <tr style="margin-top: 10px; margin-bottom: 10px">
                           <td colspan="3">
                                <asp:UpdateProgress ID="UpdateProgress1"
AssociatedUpdatePanelID="updateInProcess"
runat="server">
    <ProgressTemplate>           

      Procesando          
    </ProgressTemplate>
</asp:UpdateProgress>
                               <asp:Label ID="lblModalMessageEdit" runat="server" ForeColor="Red" Width="504px"></asp:Label>
                           </td>
                       </tr>
                  
                       <tr>
                           <td colspan="3">
                               <div align="center">
                                   
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
      <asp:LinkButton class="catLinkButtonConfirm" ID="lbAcceptEdit"  runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
      <asp:LinkButton class="catLinkButton" ID="lbCancelEdit"  runat="server" Text="Cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'></i> "></asp:LinkButton>

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
