<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AgregarModelo.aspx.vb" Inherits="WebApplication1.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Agregar Modelo de calculo directo"></asp:Label>

<div style="height: 32px">

</div>      
    <table>
              <tr>
                  <td style="height: 17px; width: 128px; text-align: right">
            <asp:Label ID="Label1" runat="server" Text="Modelo:" Width="70px" ></asp:Label>
                     </td> <td style="height: 17px; width: 128px; text-align: right">
            <asp:TextBox ID="txtModelo" runat="server"></asp:TextBox>
                      </td>
                  </tr>
            
        <tr>
            <td style="height: 17px; width: 128px; text-align: right">   
        <asp:Label ID="Label2" runat="server" Text="Vida util:" Width="70px" ></asp:Label>
                 </td> <td style="height: 17px; width: 128px; text-align: right">
        <asp:TextBox ID="txtVida" runat="server"></asp:TextBox>
                 </td>
            <td style="height: 17px; width: 128px; text-align: right">
        <asp:Label ID="Label3" runat="server" Text="Unidad:"></asp:Label>
                 </td> <td style="height: 17px; width: 128px; text-align: right">
        <asp:DropDownList ID="ddlUnidad" runat="server" AutoPostBack="True">
        </asp:DropDownList>
                </td>

        <!--pos muevan los data source a lo suyo local xd-->
        <!--Ya se que quedo feo pero pos ahi me avisan que mover mañana o hoy lo que sea xd-->
            <td>
       <asp:Button ID="addModelo" runat="server" Text="Agregar" BackColor="#004680" ForeColor="#FFFFFF"/>
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
                <asp:Button ID="Button2" runat="server" Text="Cancelar cambio"  BackColor="#E2DED6" ForeColor="#FFFFFF"/>
                <asp:Button ID="Button3" runat="server" Text="Enviar para aprobacion" BackColor="#284775" ForeColor="#FFFFFF"/>
            </td>
                </tr>
            
        </table>
        <!-- En la base de datos no habia nada de modelo asi que pss no supe como llenar eso en la tabla pero pos ahi esta todo culero xd-->
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
    <asp:Label ID="Label9" runat="server" Text=""></asp:Label>

<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Label9" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>



<asp:Panel ID="modalpan" runat="server" Style="width:500px; height:450px">
     <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

     </div>
    <div style="background-color:white;  font-size:medium; padding:2px;" class="auto-style1">

        <asp:Label ID="Label10" runat="server" Text="Favor de asignar un aprobador para el cambio" ></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Text="Aprobador:" Width="80px"></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox3" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>

        <br />
        <br />
        <asp:Label ID="Label6" runat="server" Text="Usuario:" Width="80px"></asp:Label>
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox4" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="Label7" runat="server" Text="Contraseña:" Width="80px"></asp:Label>
        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox5" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="Label8" runat="server" Text="Comentarios:" Width="80px"></asp:Label>
        <asp:TextBox ID="TextBox6" runat="server" Height="104px"></asp:TextBox>

        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBox6" ErrorMessage="Campo se encuentra vacio" ForeColor="Red" ValidationGroup="a"></asp:RequiredFieldValidator>
        <br />

        <br />
        <div align="center">
            <asp:Button ID="Button4" runat="server" Text="Aceptar" BackColor="#53C400" ForeColor="#FFFFFF" ValidationGroup="a"/>
            <asp:Button ID="Button5" runat="server" Text="Cancelar" BackColor="#284775" ForeColor="#FFFFFF" />
        </div>
    </div>
    <!--<div style=" background-color:gray; color:white; padding:2px;">
        <asp:Button ID="Buttonpopup" runat="server" Text="X" />
    </div>-->
</asp:Panel>
</asp:Content>
