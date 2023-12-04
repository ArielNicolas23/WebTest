<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatWorkOrdersEdit.aspx.vb" Inherits="WebApplication1.Catalog_CatWorkOrdersEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Agregar nueva Orden de Trabajo"></asp:Label>

   <div style="height: 32px">

   </div>

   <table>
       <tr>
           <td style="height: 17px; width: 128px">
               <asp:Label ID="lblWorkOrder" runat="server" Text="Orden de Trabajo:"></asp:Label>
           </td>
           <td style="height: 17px; width: 128px">
               <asp:TextBox ID="txtWorkOrder" runat="server"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td style="height: 17px; width: 128px">
               <asp:Label ID="lblArea" runat="server" Text="Módulo:"></asp:Label>
           </td>
           <td style="height: 17px; width: 128px">
               <asp:DropDownList ID="cmbArea" runat="server" Width="128px">
                   <asp:ListItem>Módulo 1</asp:ListItem>
                   <asp:ListItem>Módulo 2</asp:ListItem>
                   <asp:ListItem>Módulo 3</asp:ListItem>
                   <asp:ListItem>Módulo 4</asp:ListItem>
               </asp:DropDownList>
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
</asp:Content>
