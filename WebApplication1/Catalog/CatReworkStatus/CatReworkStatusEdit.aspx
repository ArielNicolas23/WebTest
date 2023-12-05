<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatReworkStatusEdit.aspx.vb" Inherits="WebApplication1.Catalog_CatReworkStatusEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Agregar nuevo Status de Retrabajo"></asp:Label>

    <div style="height: 32px">

    </div>

    <table>
        <tr>
            <td style="height: 17px; width: 128px">
                <asp:Label ID="lblStatus" runat="server" Text="Código de Status:"></asp:Label>
            </td>
            <td style="height: 17px; width: 128px">
                <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblIsRework" runat="server" Text="Es Retrabajo:"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsRework" runat="server" />
            </td>
        </tr>
    </table>

    <div style="height: 16px">

    </div>

    <div style="padding: 4px; width: 256px" align="right">
        <div style="display: inline-block">
            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
        </div>
        <div style="display: inline-block; margin-left: 20px">
            <asp:Button ID="btnSave" runat="server" Text="Guardar" />
        </div>
    </div>

    <br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label><br />

    <br />

</asp:Content>
