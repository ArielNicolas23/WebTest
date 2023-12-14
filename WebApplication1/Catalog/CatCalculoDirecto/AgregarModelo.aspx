<%@  Page Async="true" Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AgregarModelo.aspx.vb" Inherits="WebApplication1.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
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
                    
                    if ( document.getElementById("MainContent_txtApprover").value.indexOf('||') != -1) {
                        document.getElementById("MainContent_txtUsernameApprover").value =  document.getElementById("MainContent_txtApprover").value.split('||')[1].trim();
                        document.getElementById("MainContent_txtMailApprover").value = document.getElementById("MainContent_txtApprover").value.split('||')[2].trim();
                        document.getElementById("MainContent_txtApprover").value = document.getElementById("MainContent_txtApprover").value.split('||')[0].trim();
                        document.getElementById("btnRefresh").disabled = true;
                     
                        document.getElementById("MainContent_txtApprover").disabled = true;
                        document.getElementById("btnRefresh").disabled = false;
                     }
                }
            </script>

                                            
            <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Configuración de Cálculo Directo"></asp:Label>

            <div style="height: 32px">

            </div>      
            <table>
                <tr>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="lblModel" runat="server" Text="Modelo:" Width="70px" ></asp:Label>
                    </td> 
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                    </td>
                </tr>
            
                <tr>
                    <td style="height: 17px; width: 128px; text-align: right">   
                        <asp:Label ID="lblLifespan" runat="server" Text="Vida Útil:" Width="70px" ></asp:Label>
                    </td> 
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:TextBox ID="txtLifeSpan" runat="server"></asp:TextBox>
                    </td>
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:Label ID="lblUnit" runat="server" Text="Unidad:"></asp:Label>
                    </td> 
                    <td style="height: 17px; width: 128px; text-align: right">
                        <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="True" Height="16px">
                        </asp:DropDownList>
                    </td>

                    <td>
                        <asp:Button ID="cmdModel" runat="server" Text="Agregar" BackColor="#004680" ForeColor="#FFFFFF"/>       
                    </td>
                </tr>
            </table>

            <div>
                <br />

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="100%"></asp:Label><br />

                <br />
            </div>
        
            <div style="width: 750px">
                <div style="float: right">
                    <asp:Button ID="cmdCancelChange" runat="server" Text="Reiniciar Carga de Modelos"  BackColor="#E2DED6" ForeColor="Black"/>
                    <asp:Button ID="cmdOpenApprove" runat="server" Text="Enviar para Aprobación" BackColor="#284775" ForeColor="#FFFFFF"/>
                </div>
            </div>

            <!-- Cosas-->
            <asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="dgvCatalog" GridLines="None" Width="680px" AllowPaging="True" ID="gvModelos">
                <Columns>
                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                    <asp:BoundField DataField="VidaUtil" HeaderText="Vida Útil" SortExpression="VidaUtil" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                    <asp:BoundField DataField="IdUnidad" Visible="False" />
                    <asp:CommandField HeaderText="Acción" ShowDeleteButton="True" />
                </Columns>
                <RowStyle CssClass="dgvCatalogRowOdd" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle CssClass="dgvPaging"/>
                <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
            </asp:GridView>

            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="catModalbackground "></ajaxToolkit:ModalPopupExtender>



            <asp:Panel ID="modalpan" runat="server" Height="354px" Width="630px">
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
