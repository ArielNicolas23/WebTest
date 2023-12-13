<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ewapp_login.aspx.vb" Inherits="WebApplication1.ewapp_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<script type="text/javascript">
    //window.resizeTo(790, 690);
</script>
<title>Medtronic Mexico Inicio de Sesión</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <style type="text/css">
        #cllLoginControls
        {
            text-align: center;
        }
    </style>
</head>
<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<form runat="server" id ="Form1" autocomplete="off">
    <asp:ScriptManager ID="ScriptManager1" runat="server"/>   
<!-- Save for Web Slices (Pantalla_de_Login-Prototipo#1(plain).psd) -->
<table id="Table_01" width="766" height="492" border="0" cellpadding="0" cellspacing="0">
	<tr style="height:58px">
		<td colspan="7">
			<img src="images/index_01.gif" width="766" height="58" alt=""></td>
	</tr>
	<tr style="height:47px">
		<td colspan="2">
			<img src="images/index_02.jpg" width="18" height="47" alt=""></td>
		<td width="548" height="47" colspan="3" bgcolor="#0066a4">
	        <asp:Label ID="lblApplicationName" runat="server" Text="Fecha de Expiración, Versión 16.0" 
                
                
                
                
                style="color:#CCCCCC;background-color:#0066a4; font-family:Arial; font-size:x-large; font-weight: bold"></asp:Label>
	    </td>
		<td colspan="2">
			<img src="images/index_04.jpg" width="200" height="47" alt=""></td>
	</tr>
	<tr style="height:94px">
		<td colspan="7">
			<img src="images/index_05.jpg" width="766" height="94" alt=""></td>
	</tr>
	<tr>
		<td colspan="3" rowspan="2">
			<img src="images/index_06.jpg" width="20" height="225" alt=""></td>
		<td>
			<img src="images/index_07.jpg" width="396" height="30" alt=""></td>
		<td colspan="3" rowspan="2">
			<img src="images/index_08.jpg" alt="" width="350" height="225" border="0" usemap="#Map"></td>
	</tr>
	<tr>
		<td width="396" height="195" id="cllLoginControls" >
            <table>
                <tr>
                    <td colspan="2" style="text-align: left; font-family: Arial">
                        Nombre de Usuario:</td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left;">
                        <asp:TextBox ID="TxtUserName" runat="server" Width="94%" 
                            style="text-align: left"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left;font-family: Arial">
                        Contraseña:</td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left;">
                        <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" Width="94%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 159px; height: 20px">
                    </td>
                    <td style="width: 100px; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 159px">
                    </td>
                    <td style="width: 100px">
                        <asp:Button ID="BtnLogin" runat="server" Text="Ingresar" Width="106px" />
                    </td>
                </tr>
            </table>
	  </td>
	</tr>
	<tr>
		<td>
			<img src="images/index_10.jpg" width="10" height="51" alt=""></td>
		<td colspan="5" width="743" height="51">
            <asp:Label ID="LblMessage" runat="server" ForeColor="Red" Height="12px" 
                Width="388px"></asp:Label>
                    <asp:Login ID="Login1" runat="server" Visible="False">
    </asp:Login>

	  </td>
		<td>
			<img src="images/index_12.jpg" width="13" height="51" alt=""></td>
	</tr>
	<tr>
		<td colspan="7">
			<img src="images/index_13.jpg" width="766" height="16" alt=""></td>
	</tr>
	<tr>
		<td>
			<img src="images/spacer.gif" width="10" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="8" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="2" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="396" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="150" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="187" height="1" alt=""></td>
		<td>
			<img src="images/spacer.gif" width="13" height="1" alt=""></td>
	</tr>
</table>
<!-- End Save for Web Slices -->
<map name="Map">
  <area shape="rect" coords="20,168,103,187" href="mailto:dl.tij@medtronic.com">
</map>
</form>
</body>
</html>
