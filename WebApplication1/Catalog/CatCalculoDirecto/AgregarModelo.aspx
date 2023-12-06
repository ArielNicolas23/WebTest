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
        <asp:DropDownList ID="ddlUnidad" runat="server" DataSourceID="SqlDataSource1" DataTextField="unit" DataValueField="unit">
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
        <asp:GridView runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" CellPadding="4" ForeColor="#333333" GridLines="None" Width="680px" AllowPaging="True" ID="gvModelos" OnRowDeleting="Unnamed1_RowDeleting">
            <Columns>
                <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                <asp:BoundField DataField="Vida util" HeaderText="Vida util" SortExpression="Vida util"  />
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
</asp:Content>
