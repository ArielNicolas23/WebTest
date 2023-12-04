<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebApplication1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   <div id="stdlayout">

    <asp:Label ID="lblPageName" runat="server" Text="Agregar Unidad" Font-Size="18pt"></asp:Label>
    <br />
    <br /> 
    <table>
        <tr>
            <td style="width: 126px">
                <asp:Label ID="Label1" runat="server" Text="Unidad" Width="119px"></asp:Label></td>
            <td style="width: 14px">
            </td>
            <td style="width: 173px">
                &nbsp;<asp:TextBox ID="TxtComponent" runat="server" Width="100px"></asp:TextBox>&nbsp;
            </td>
            <td style="width: 9px">
            </td>
            <td style="width: 178px">
                <%--<asp:LinkButton ID="Lnk_ewapp_menu" runat="server">Regresar a página anterior</asp:LinkButton>--%>
                </td>
        </tr>
        <tr>
            <td style="width: 126px">
                <asp:Label ID="Label2" runat="server" Text="Valor" Width="119px"></asp:Label></td>
            <td style="width: 14px">
            </td>
            <td style="width: 173px">
                <asp:TextBox ID="TxtDescription" runat="server" Width="100px"></asp:TextBox></td>
            <td style="width: 9px">
            </td>
            <td style="width: 178px">
            </td>
        </tr>
        <tr>
            <td style="width: 126px; height: 21px">
            </td>
            <td style="width: 14px; height: 21px">
            </td>
            <td style="width: 173px; height: 21px">
            </td>
            <td style="width: 9px; height: 21px">
            </td>
            <td style="width: 178px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 126px">
            </td>
            <td style="width: 14px">
            </td>
            <td style="text-align: right;">
                <asp:Button ID="BtnAdd" runat="server" Text="Agregar" Width="106px" /></td>
            <td style="width: 9px">
            </td>
            <td style="width: 178px">
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="LblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
        ForeColor="#333333" GridLines="None" Width="480px" DataKeyNames = "IdUnit">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="IdUnit" HeaderText="IdUnit" ReadOnly="True" visible="false">
                <ItemStyle Width="100px" />
                <HeaderStyle HorizontalAlign="Left" />
             </asp:BoundField>
            <asp:BoundField DataField="Unit" HeaderText="Unit">
                <ItemStyle Width="100px" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="UnitValue" HeaderText="UnitValue">
                <ItemStyle Width="200px" />
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:CheckBoxField DataField="IsActive" HeaderText="Activo">
                <ItemStyle HorizontalAlign="Center" Width="50px" />
                <HeaderStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CommandField ShowEditButton="True" />
        </Columns>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>    
    </div>
</asp:Content>
