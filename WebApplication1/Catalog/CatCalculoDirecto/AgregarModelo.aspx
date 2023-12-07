﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AgregarModelo.aspx.vb" Inherits="WebApplication1.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Agregar Modelo de calculo directo"></asp:Label>

<div style="height: 32px">

</div>      
    <table>
              <tr>
                  <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="lblModel" runat="server" Text="Modelo:" Width="70px" ></asp:Label>
                     </td> <td style="height: 17px; width: 128px; text-align: right">
            <asp:TextBox ID="txtModel" runat="server"></asp:TextBox>
                      </td>
                  </tr>
            
        <tr>
            <td style="height: 17px; width: 128px; text-align: right">   
        <asp:Label ID="lblLifespan" runat="server" Text="Vida util:" Width="70px" ></asp:Label>
                 </td> <td style="height: 17px; width: 128px; text-align: right">
        <asp:TextBox ID="txtLifeSpan" runat="server"></asp:TextBox>
                 </td>
            <td style="height: 17px; width: 128px; text-align: right">
        <asp:Label ID="lblUnit" runat="server" Text="Unidad:"></asp:Label>
                 </td> <td style="height: 17px; width: 128px; text-align: right">
        <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="True">
        </asp:DropDownList>
                </td>

        <!--Cosas-->
            <td>
       <asp:Button ID="cmdModel" runat="server" Text="Agregar" BackColor="#004680" ForeColor="#FFFFFF"/>
                </td>
       </tr>
        </table>
        <br />
        <br />
        <br />
        
        <table id="Table1" runat="server" style="width: 680px">
            <tr> 

            <td style="height: 17px; width: 128px;">
                
            </td>
            <td style="height: 17px; width: 120px;">
                
                </td>
            <td style="height: 17px;width: 420px; text-align: right">
                <asp:Button ID="cmdCancelChange" runat="server" Text="Cancelar cambio"  BackColor="#E2DED6" ForeColor="#FFFFFF"/>
                <asp:Button ID="cmdOpenApprove" runat="server" Text="Enviar para aprobacion" BackColor="#284775" ForeColor="#FFFFFF"/>
            </td>
                </tr>
            
        </table>

        <!-- Cosas-->
        <asp:GridView runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="680px" AllowPaging="True" ID="gvModelos">
            <Columns>
                <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                <asp:BoundField DataField="VidaUtil" HeaderText="Vida util" SortExpression="Vida util"  />
                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad"  />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                <asp:CommandField HeaderText="Accion" ShowDeleteButton="True" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
<EditRowStyle BackColor="#999999" />
<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <p>
            
            
        </p>
    <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>

<ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>



<asp:Panel ID="modalpan" runat="server" Style="width:500px; height:450px">
     <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

     </div>
    <div style="background-color:white;  font-size:medium; padding:2px;" class="auto-style1">

        <asp:Label ID="lblModalInstruction" runat="server" Text="Favor de asignar un aprobador para el cambio" ></asp:Label>
        <br />
        <asp:Label ID="lblApprover" runat="server" Text="Aprobador:" Width="80px"></asp:Label>
        <asp:TextBox ID="txtApprover" runat="server"></asp:TextBox>

        <asp:RequiredFieldValidator ID="lblApproverError" runat="server" ControlToValidate="txtApprover" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>

        <br />
        <br />
        <asp:Label ID="lblUser" runat="server" Text="Usuario:" Width="80px"></asp:Label>
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="lblUserError" runat="server" ControlToValidate="txtUser" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" Width="80px"></asp:Label>
        <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="lblPassworkError" runat="server" ControlToValidate="txtPassword" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="lblApproveMessage" runat="server" Text="Comentarios:" Width="80px"></asp:Label>
        <asp:TextBox ID="txtApproveMessage" runat="server" Height="104px"></asp:TextBox>

        <asp:RequiredFieldValidator ID="lblApproveMessageError" runat="server" ControlToValidate="txtApproveMessage" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />

        <br />
        <div align="center">
            <asp:Button ID="cmdAcceptChange" runat="server" Text="Aceptar" BackColor="#53C400" ForeColor="#FFFFFF" ValidationGroup="a"/>
            <asp:Button ID="cmdCancelModal" runat="server" Text="Cancelar" BackColor="#284775" ForeColor="#FFFFFF" />
        </div>
    </div>
    <!--<div style=" background-color:gray; color:white; padding:2px;">
        <asp:Button ID="Buttonpopup" runat="server" Text="X" />
    </div>-->
</asp:Panel>
</asp:Content>
