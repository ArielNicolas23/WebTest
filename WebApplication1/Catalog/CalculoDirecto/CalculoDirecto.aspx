<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="CalculoDirecto.aspx.vb" Inherits="WebApplication1.CalculoDirecto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   <div id="stdlayout">

       <asp:Label ID="lblPageName" runat="server" Text="Calulo Directo" Font-Size="18pt"></asp:Label>
       <br />
       <div align="center" style="margin: 30px; clip: rect(auto, auto, auto, auto)">
       <table>
           
           <tr>
               <td align="Left">
    <asp:Label ID="Label1" runat="server" Text="Orden de Trabajo: "></asp:Label>
</td>
               <td>
                   <asp:TextBox ID="CalcOrden" runat="server"></asp:TextBox>
               </td>
               <td>
                   <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
               </td>
           </tr>
           <tr>
               <td align="Left">
    <asp:Label ID="Label2" runat="server" Text="Catalogo/Modelo: "></asp:Label>
</td>
    <td>
        <asp:TextBox ID="CalcModelo" runat="server"></asp:TextBox>
    </td>
    <td>
                   <asp:Button ID="Button1" runat="server" Text="Calcular" />
    </td>
</tr>
       </table>
       <br />
           </div>
   </div>
               <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
           <ajaxToolkit:ModalPopupExtender ID="InfoModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground" ></ajaxToolkit:ModalPopupExtender>
           <asp:Panel ID="modalpan" runat="server" Height="354px" Width="630px" DefaultButton="lbAccept">
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
                               <asp:Label ID="lblOrden" runat="server" Text="Orden:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style2">
                               <asp:Label ID="lblDispOrden" runat="server"></asp:Label>
                           </td>

                           <td class="auto-style5">
                               &nbsp;</td>
                       </tr>

                       <tr>
                           <td align="left" style="width: 100px">
                               <asp:Label ID="lblModelo" runat="server" Text="Modelo:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style2">
                               <asp:Label ID="lblDispModelo" runat="server"></asp:Label>
                           </td>

                           <td class="auto-style5">
                               &nbsp;</td>
                       </tr>

                       <tr>
                           <td align="left" style="width: 100px">
                               <asp:Label ID="lblApproveMessage" runat="server" Text="Calculo:" Width="100px" Font-Bold="True"></asp:Label>
                           </td>

                           <td align="left" class="auto-style2">
                               <asp:TextBox ID="txtCalculo" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                           </td>

                           <td class="auto-style5">
                               &nbsp;</td>
                       </tr>

                       <tr style="margin-top: 10px; margin-bottom: 10px">
                           <td colspan="3" class="auto-style1">
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
      <asp:LinkButton class="catLinkButtonConfirm" ID="lbAccept"   runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
      <asp:LinkButton class="catLinkButton" ID="lbCancel"  runat="server" Text="Cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'></i> "></asp:LinkButton>
        
    </ContentTemplate>
</asp:UpdatePanel> 
                            
                               </div>
                           </td>
                       </tr>
                   </table>
               </div>
           </asp:Panel>
</asp:Content>
