<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatUnitsEdit.aspx.vb" Inherits="WebApplication1.Catalog_CatUnitsEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Agregar nueva Unidad" CssClass="catHeader"></asp:Label>

    <div style="height: 32px">

    </div>

    <table>
        <tr>
            <td style="height: 17px; width: 128px">
                <asp:Label ID="lblUnit" runat="server" Text="Unidad:" CssClass="catLabel"></asp:Label>
            </td>
            <td style="height: 17px; width: 128px">
                <asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 17px; width: 128px">
                <asp:Label ID="lblUnitValue" runat="server" Text="Valor de Unidad:" CssClass="catLabel"></asp:Label>
            </td>
            <td style="height: 17px; width: 128px">
                <asp:TextBox ID="txtUnitValue" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div style="height: 16px">

    </div>

    <div style="padding: 4px; width: 256px" align="right">
        <div style="display: inline-block">
            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="catButton" />
        </div>
        <div style="display: inline-block; margin-left: 20px">
            <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="catButtonAccept"/>
        </div>
    </div>

    <br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label><br />

    <br />
</asp:Content>
