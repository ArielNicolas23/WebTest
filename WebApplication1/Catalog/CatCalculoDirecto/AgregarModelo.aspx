﻿<%@  Page Async="true" Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AgregarModelo.aspx.vb" Inherits="WebApplication1.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <div class="catHeader">
                <asp:Label ID="lblTitle" runat="server" Text="Catálogo de Configuración de Cálculo Directo"></asp:Label>
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
        
            <div style="width: 680px">
                <div style="float: right">
                    <asp:Button ID="cmdCancelChange" runat="server" Text="Reiniciar Carga de Modelos"  BackColor="#E2DED6" ForeColor="Black"/>
                    <asp:Button ID="cmdOpenApprove" runat="server" Text="Enviar para Aprobación" BackColor="#284775" ForeColor="#FFFFFF"/>
                </div>
            </div>

            <!-- Cosas-->
            <asp:GridView runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="750px" AllowPaging="True" ID="gvModelos">
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

            <p>
            
            
            </p>

            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>



            <asp:Panel ID="modalpan" runat="server" Style="width:550px; height:450px">
    
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>

                <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                    <table style="width: 550px">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblModalInstruction" runat="server" Text="Favor de asignar un aprobador para el cambio" ></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" style="width: 170px">
                                <asp:Label ID="lblApprover" runat="server" Text="Aprobador:" Width="162px"></asp:Label>
                            </td>
                            <td align="left" style="width: 170px">
                                <asp:TextBox ID="txtApprover" runat="server" Width="162px"></asp:TextBox>
                                <%--<ajaxToolkit:ComboBox ID=s"txtApprover" runat="server" Width="128px" AutoCompleteMode="SuggestAppend" AutoPostBack="True" ItemInsertLocation="OrdinalText">

                                </ajaxToolkit:ComboBox>--%>
                            </td>
                            <td>
                                <asp:Label ID="lblApproverError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblApproverError" runat="server" ControlToValidate="txtApprover" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" style="width: 170px">
                                <asp:Label ID="lblUser" runat="server" Text="Usuario:" Width="162px"></asp:Label>
                            </td>
                            <td align="left" style="width: 170px">
                                <asp:TextBox ID="txtUser" runat="server" Width="162px" CausesValidation="True"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblUserError" runat="server" ControlToValidate="txtUser" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
 
                        <tr>
                            <td align="right" style="width: 170px">
                                <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" Width="162px"></asp:Label>
                            </td>
                            <td align="left" style="width: 170px">
                                <asp:TextBox ID="txtPassword" runat="server" Width="162px" TextMode="Password"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPassworkError" runat="server" ForeColor="Red"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="lblPassworkError" runat="server" ControlToValidate="txtPassword" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>

                        <tr>
                            <td align="right" style="width: 162px">
                                <asp:Label ID="lblApproveMessage" runat="server" Text="Comentarios:" Width="162px"></asp:Label>
                            </td>
                            <td align="left" style="width: 170px">
                                <asp:TextBox ID="txtApproveMessage" runat="server" Height="104px" Width="162px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
