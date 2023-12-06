﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CatCalculoDirecto.aspx.vb" Inherits="WebApplication1.CatCalculoDirecto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <h2>
        <asp:Label ID="Label4" runat="server" Text="Catalogo de Configuracion de Calculo Directo" ForeColor="#284775"></asp:Label>
        </h2>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Modelo:" Width="70px" ></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <br />
        </div>
        <asp:Label ID="Label2" runat="server" Text="Vida util:" Width="70px" ></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label3" runat="server" Text="Unidad:"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="unit" DataValueField="unit">
        </asp:DropDownList>
        <!--pos muevan los data source a lo suyo local xd-->
        <!--Ya se que quedo feo pero pos ahi me avisan que mover mañana o hoy lo que sea xd-->
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=J96W2D3-L1\SQLEXPRESS;Initial Catalog=Fechaexpiracion;Integrated Security=True" ProviderName="System.Data.SqlClient" SelectCommand="Select unit from [ExpDate].[CatUnits]"></asp:SqlDataSource>
        <asp:Button ID="Button1" runat="server" Text="Agregar" BackColor="#004680" ForeColor="#FFFFFF"/>
        
        <br />
        <br />
        <br />
        
        <table id="Table1" runat="server" width="935px">
            <tr> 

            <td style="height: 17px; width: 128px;">
                
            </td>
            <td style="height: 17px; width: 128px;">
                
                </td>
            <td style="height: 17px; width: 128px;">
                <asp:Button ID="Button2" runat="server" Text="Cancelar cambio"  BackColor="#E2DED6" ForeColor="#FFFFFF"/>
                <asp:Button ID="Button3" runat="server" Text="Enviar para aprobacion" BackColor="#284775" ForeColor="#FFFFFF"/>
            </td>
                
                
                </tr>
        </table>
        <!-- En la base de datos no habia nada de modelo asi que pss no supe como llenar eso en la tabla pero pos ahi esta todo culero xd-->
        <asp:GridView runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
            <Columns>
                <asp:BoundField DataField="Vida util" HeaderText="Vida util" SortExpression="Vida util"  />
                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad"  />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
            </Columns>
            <HeaderStyle BackColor="#004680" ForeColor="#FFFFFF" Font-Bold="True" />
            <RowStyle BackColor="#55B2FF" ForeColor="#000000" />
            <AlternatingRowStyle BackColor="#9ED3FF" ForeColor="#000000" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=J96W2D3-L1\SQLEXPRESS;Initial Catalog=Fechaexpiracion;Integrated Security=True" ProviderName="System.Data.SqlClient" SelectCommand="SELECT  [UnitValue] as [Vida util],[Unit] as [Unidad], [CreatedBy] as [Usuario] FROM [EXPDate].[CatUnits]"></asp:SqlDataSource>
        <p>
            
            
        </p>
    </form>
</body>
</html>
