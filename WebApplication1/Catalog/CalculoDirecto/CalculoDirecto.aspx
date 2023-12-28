<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CalculoDirecto.aspx.vb" Inherits="WebApplication1.CalculoDirecto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>
            <div id="stdlayout">
                <asp:Label ID="lblTitle" runat="server" Text="Cálulo Directo" CssClass="catHeader"></asp:Label>
                <div align="center" style="margin: 30px; clip: rect(auto, auto, auto, auto)">
                    <table class="catLabel" style="text-align: right">
                        <tr style="height: 30px;">
                            <td>
                                <asp:Label ID="lblWorkOrder" runat="server" Text="Orden de Trabajo: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkOrder" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnReset" CssClass="catLinkButton" runat="server" Text="<i class='fa fa-regular fa-rotate-right' data-toggle='tooltip' title='Reiniciar'></i>" />
                            </td>
                        </tr>
                        <tr style="height: 30px;">
                            <td>
                                <asp:Label ID="lblModel" runat="server" Text="Catálogo / Modelo: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <div class="catDivTopMargin">
                        <asp:Button ID="btnCalculate" runat="server" Text="Calcular" CssClass="catButtonAccept"/>
                    </div>

                    <div class="catDivTopMargin">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="catLabel" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblSuccessMessage" runat="server" CssClass="catLabel" ForeColor="Green"></asp:Label>
                    </div>
                </div>
            </div>

            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="InfoModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground" ></ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="modalpan" runat="server" Height="354px" Width="630px" DefaultButton="btnModalAccept">
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
                            <td align="left" class="auto-style4">
                                <asp:Label ID="lblModalWorkOrder" runat="server" Text="Orden:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style2">
                                <asp:Label ID="lblModalDispWorkOrder" runat="server"></asp:Label>
                            </td>

                            <td class="auto-style5">
                                &nbsp;</td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblModalModel" runat="server" Text="Modelo:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style2">
                                <asp:Label ID="lblModalDispModel" runat="server"></asp:Label>
                            </td>

                            <td class="auto-style5">
                                &nbsp;</td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblModalCalculation" runat="server" Text="Cálculo:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style2">
                                <asp:TextBox ID="txtModalCalculation" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                &nbsp;</td>
                        </tr>

                        <tr style="margin-top: 10px; margin-bottom: 10px">
                            <td colspan="3" class="auto-style1">
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
                                            <asp:LinkButton class="catLinkButtonConfirm" ID="btnModalAccept"   runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
                                            <asp:LinkButton class="catLinkButton" ID="btnModalCancel"  runat="server" Text="Cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'></i> "></asp:LinkButton>
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
