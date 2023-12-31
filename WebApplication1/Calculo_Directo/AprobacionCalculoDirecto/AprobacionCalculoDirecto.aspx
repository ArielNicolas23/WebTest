﻿<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="AprobacionCalculoDirecto.aspx.vb" Inherits="WebApplication1.CatModuloAprobacion" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">

     // Get the instance of PageRequestManager.
     var prm = Sys.WebForms.PageRequestManager.getInstance();
     // Add initializeRequest and endRequest
     prm.add_initializeRequest(prm_InitializeRequest);

     // Called when async postback begins
     function prm_InitializeRequest(sender, args) {
         // Disable button that caused a postback
         var x = document.getElementById("MainContent_lbEdit");
         x.className += " disabled-link";

         var x = document.getElementById("MainContent_lbCancel0");
         x.className += " disabled-link";

         var x = document.getElementById("MainContent_lbAccept");
         x.className += " disabled-link";

         var x = document.getElementById("MainContent_lbCancel");
         x.className += " disabled-link";


     }
     </script>
    <style>
        .disabled-link {
  pointer-events: none;
}
        .scroll  
{
weight: 800px;
height: 400px; 
overflow: scroll; 
}
        .modalbackground
        {
            background-color:black;
            opacity:0.6;
        }
        .data tr:nth-child(even){background-color: #f2f2f2;}
        .grid-container {
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-gap: 20px;
}
        .data th {
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: left;
  background-color: #04AA6D;
  color: white;
}
        .auto-style1 {
            width: 650px;
        }
        .auto-style2 {
            height: 23px;
        }
        .auto-style3 {
            width: 291px;
        }
        .auto-style4 {
            width: 289px;
        }
        .auto-style5 {
            width: 351px;
        }
        .auto-style6 {
            height: 23px;
            width: 255px;
        }
        .auto-style7 {
            width: 130px;
        }
        .auto-style8 {
            height: 23px;
            width: 130px;
        }
        .auto-style9 {
            width: 255px;
        }
        .auto-style10 {
            width: 151px;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Text="Cambios pendientes de Aprobación" CssClass="catHeader"></asp:Label>

            <div style="height:32px">

            </div>

            <div runat="server" id="divFilterHeader" visible="true">
                <div class="catDivHeader">
                    <asp:Label ID="lblStatus" runat="server" Text="Estatus: " CssClass="catLabel" ></asp:Label>
                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" CssClass="catDropDownList">
                        <asp:ListItem>Todos</asp:ListItem>
                        <asp:ListItem>Pendiente</asp:ListItem>
                        <asp:ListItem>En Revisión</asp:ListItem>
                        <asp:ListItem>Aprobado</asp:ListItem>
                        <asp:ListItem>Rechazado</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label ID="lblRole" runat="server" Text="Rol de Usuario: " CssClass="catLabel" ></asp:Label>
                    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" CssClass="catDropDownList">
                        <asp:ListItem>Ambos</asp:ListItem>
                        <asp:ListItem>Originador</asp:ListItem>
                        <asp:ListItem>Aprobador</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label ID="lblDateFilters" runat="server" Text="Buscar por Fecha: " CssClass="catLabel" ></asp:Label>
                    <asp:CheckBox ID="chkDateFilters" runat="server" AutoPostBack="true"/>
                    <asp:LinkButton class="catLinkButton" ID="lBtnSearc"  runat="server" Visible="False" Text="Buscar <i class='fa fa-search' data-toggle='tooltip' title='Buscar'></i> "></asp:LinkButton>
                </div>

                <div runat="server" id="divDateFilters"  class="catDivHeader" visible="false">
                    <asp:Label ID="lblCreatedOn" runat="server" Text="Fecha de Origen: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="txtCreatedOn" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="btnCreatedOn" runat="server" OnClick="OpenCalendar" class="catLinkButton" Text="<i class='fa fa-calendar' data-toggle='tooltip' title='Seleccionar Fecha'></i> " />
                    <asp:Calendar ID="cldCreatedOn" runat="server" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                        <TodayDayStyle BackColor="#CCCCCC" />
                    </asp:Calendar>

                    <asp:Label ID="lblCreatedOnTo" runat="server" Text="A: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="txtCreatedOnTo" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="btnCreatedOnTo" runat="server" OnClick="OpenCalendar" class="catLinkButton" Text="<i class='fa fa-calendar' data-toggle='tooltip' title='Seleccionar Fecha'></i> " />
                    <asp:Calendar ID="cldCreatedOnTo" runat="server" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                        <TodayDayStyle BackColor="#CCCCCC" />
                    </asp:Calendar>

                    <asp:Label ID="lblApprovedOn" runat="server" Text="Fecha de Aprobación: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="txtApprovedOn" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="btnApprovedOn" runat="server" OnClick="OpenCalendar" class="catLinkButton" Text="<i class='fa fa-calendar' data-toggle='tooltip' title='Seleccionar Fecha'></i> " />
                    <asp:Calendar ID="cldApprovedOn" runat="server" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                        <TodayDayStyle BackColor="#CCCCCC" />
                    </asp:Calendar>

                    <asp:Label ID="lblApprovedOnTo" runat="server" Text="A: " CssClass="catLabel" ></asp:Label>
                    <asp:TextBox ID="txtApprovedOnTo" runat="server"></asp:TextBox>
                    <asp:LinkButton ID="btnApprovedOnTo" runat="server" OnClick="OpenCalendar" class="catLinkButton" Text="<i class='fa fa-calendar' data-toggle='tooltip' title='Seleccionar Fecha'></i> " />
                    <asp:Calendar ID="cldApprovedOnTo" runat="server" Visible="False" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                        <TodayDayStyle BackColor="#CCCCCC" />
                    </asp:Calendar>
                </div>
            </div>


            <div runat="server" ID="divdgvPendingApproval">
                <asp:GridView  ID="dgvPendingApproval" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="2000px" AllowPaging="True" DataKeyNames="IdModelsChangesHeader,OriginEmail">
                    <Columns>
                        <asp:BoundField DataField="IdModelsChangesHeader" Visible="False" ReadOnly="True" />
                        <asp:BoundField DataField="ChangeNumber" HeaderText="No. de Cambio" SortExpression="ChangeNumber" />
                        <asp:BoundField DataField="OriginName" HeaderText="Nombre Originador" SortExpression="OriginName" />
                        <asp:BoundField DataField="OriginUser" HeaderText="Usuario Originador" SortExpression="OriginUser" />
                        <asp:BoundField DataField="ModifiedOn" HeaderText="Fecha de Edición" SortExpression="ModifiedOn" />
                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Estatus" SortExpression="ModifiedOn" />
                        <asp:ButtonField CommandName="Action" HeaderText="Acción" ShowHeader="True" DataTextField="Action" />
                        <asp:BoundField DataField="ApproverName" HeaderText="Nombre Aprobador" SortExpression="ApproverName" />
                        <asp:BoundField DataField="ApproverUser" HeaderText="Usuario Aprobador" SortExpression="ApproverUser" />
                        <asp:BoundField DataField="ApprovedOn" HeaderText="Fecha de Aprobación" SortExpression="ApprovedOn" />
                        <asp:BoundField DataField="OriginEmail" ReadOnly="True" Visible="False" />
                    </Columns>
                    <RowStyle CssClass="dgvCatalogRowOdd" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle CssClass="dgvPaging"/>
                    <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                </asp:GridView>
            </div>
            <div runat="server" id="divInfoTable"  style="display: table; width: 1000px;" visible="False">
                <div>
                    <table style="margin-left: 30px; width: 650px; margin-bottom: 20px;" class="auto-style1">
                        <tr>
                            <td class="auto-style9" style="background-color: #517aa9; color: white; font-weight: bold;">Numero de cambio:</td>
                            <td class="auto-style10">
                                <asp:Label ID="lblInfoChange" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="" style="background-color: #517aa9; color: white; font-weight: bold;">Estatus:</td>
                            <td class="" runat="server" id="tdInfoStatus">
                                <asp:Label ID="lblInfoStatus" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div  style="float:left; width: 350px; margin-left: 30px; margin-right: 30px;">
                    <table class="data" style="width: 350px">
                        <tr>
                            <td colspan="2" style="background-color: #517aa9; color: white; font-weight: bold;">Datos Originador</td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Nombre:</td>
                            <td class="auto-style2" align="left">
                                <asp:Label ID="lblOrigName" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style8">Usuario:</td>
                            <td class="auto-style2" align="left">
                                <asp:Label ID="lblOrigUser" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Fecha de Edición:</td>
                            <td align="left">
                                <asp:Label ID="lblOrigDateEdit" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Comentarios:</td>
                            <td align="left">
                                <asp:Label ID="lblOrigComment" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div  style="float:left; width: 350px; margin-left: 30px; margin-right: 30px;">
                    <table class="data" style="width: 350px">
                        <tr>
                            <td colspan="2" style="background-color: #517aa9; color: white; font-weight: bold;">Datos Aprobador</td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Nombre:</td>
                            <td align="left">
                                <asp:Label ID="lblAprName" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Usuario:</td>
                            <td align="left">
                                <asp:Label ID="lblAprUser" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td class="auto-style7">Fecha de Edición:</td>
                            <td align="left">
                                <asp:Label ID="lblAprDateEdit" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style7">Comentarios:</td>
                            <td align="left">
                                <asp:Label ID="lblAprComment" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>

            </div>
                        <div runat="server" id="divModelsChanges" style="margin-top: 50px" visible="false">
                <div style="width: 2%;  float: left;">
                    <asp:Label ID="lblMargin" runat="server" Text=">" CssClass="catIcon"></asp:Label>
                </div>

                <div style="margin-left: 2%;">
                    <div class="catDivHeader" style="width: 1400px;">
                        <div style="width: 30%; float: left; text-align: left">
                            <asp:Label ID="lblModalsChanges" runat="server" Text="Modelos por Verificar" CssClass="catHeaderSub"></asp:Label>
                        </div>
                        <div style="margin-left: 30%; text-align: right">
                            <asp:LinkButton class="catLinkButton" ID="lBtnCancelChange"  runat="server"  Text="Seleccionar otro Cambio  "></asp:LinkButton>
                            <asp:LinkButton class="catLinkButton" ID="lBtnRejectChange"  runat="server"  CommandName="Reject" OnClick="ApproveOrReject" Text="Rechazar Cambios "></asp:LinkButton>
                            <asp:LinkButton class="catLinkButtonAccept" ID="lBtnApproveChange"  runat="server"  CommandName="Approve" OnClick="ApproveOrReject" Text="Aprobar Cambios "></asp:LinkButton>
                            <!--<asp:Button ID="cmdCancelChange" runat="server" Text="Seleccionar otro Cambio" CssClass="catButton"/>-->
                            <!--<asp:Button ID="cmdRejectChange" runat="server" Text="Rechazar Cambios" CssClass="catButton" CommandName="Reject" OnClick="ApproveOrReject"/>-->
                            <!--<asp:Button ID="cmdApproveChange" runat="server" Text="Aprobar Cambios" CssClass="catButtonAccept" CommandName="Approve" OnClick="ApproveOrReject"/>-->

                        </div>
                    </div>
                    <div class="scroll">
                    <asp:GridView  ID="dgvModelChanges" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1400px" DataKeyNames="IdModelsChanges">
                        <Columns>
                            <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                            <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                            <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                            <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                            <asp:BoundField DataField="LastUserName" HeaderText="Nombre del Usuario" SortExpression="LastUserName" />
                            <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                            <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
                            <asp:TemplateField HeaderText="Verificar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="IsChecked" runat="server" AutoPostBack="true" Checked='<%#Convert.ToBoolean(Eval("IsChecked"))%>' OnCheckedChanged="OnChangeIsChecked"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="dgvCatalogRowOdd" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle CssClass="dgvPaging"/>
                        <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
                    </asp:GridView>
                        </div>
                </div>

            </div>
            <div runat="server" id="divModelsView" style="margin-top: 50px" visible="false">
    <div style="width: 2%;  float: left;">
        <asp:Label ID="Label1" runat="server" Text=">" CssClass="catIcon"></asp:Label>
    </div>

    <div style="margin-left: 2%;">
        <div class="catDivHeader" style="width: 1400px;">
            <div style="width: 30%; float: left; text-align: left">
                <asp:Label ID="Label2" runat="server" Font-Size="18pt" Text="Detalles de Modelos" CssClass="catHeaderSub"></asp:Label>
            </div>
            <div style="margin-left: 30%; text-align: right">
                <asp:LinkButton class="catLinkButton" ID="lbtnDetallesCancelChange"  runat="server"  Text="Seleccionar otro Cambio  "></asp:LinkButton>
                <asp:LinkButton class="catLinkButtonAccept" ID="lBtnEdit"  runat="server"  CommandName="Approve" Text="Editar  "></asp:LinkButton>
                <!--<asp:Button ID="Button1" runat="server" Text="Seleccionar otro Cambio" CssClass="catButton"/>-->
                <!--<asp:Button ID="Button3" runat="server" Text="Editar" CssClass="catButtonAccept" CommandName="Approve"/>-->

            </div>
        </div>
        <div class="scroll">
        <asp:GridView  ID="dgvModelView" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" Width="1400px" DataKeyNames="IdModelsChanges">
            <Columns>
                <asp:BoundField DataField="IdModelsChanges" Visible="False" />
                <asp:BoundField DataField="Model" HeaderText="Modelo" SortExpression="Model" />
                <asp:BoundField DataField="Lifespan" HeaderText="Vida Útil" SortExpression="Lifespan" />
                <asp:BoundField DataField="Unit" HeaderText="Unidad" SortExpression="Unit" />
                <asp:BoundField DataField="LastUserName" HeaderText="Nombre del Usuario" SortExpression="LastUserName" />
                <asp:BoundField DataField="LastUser" HeaderText="Usuario" SortExpression="LastUser" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="Última Actualización" SortExpression="ModifiedOn" />
                <asp:TemplateField HeaderText="Verificar">
                    <ItemTemplate>
                        <asp:CheckBox ID="IsChecked" runat="server" AutoPostBack="true" Checked='<%#Convert.ToBoolean(Eval("IsChecked"))%>' OnCheckedChanged="OnChangeIsChecked" Enabled="False"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="dgvCatalogRowOdd" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle CssClass="dgvPaging"/>
            <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
        </asp:GridView>
            </div>
    </div>

</div>



            <asp:Label ID="lblModal" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="ApproveModal" runat="server" TargetControlID="lblModal" PopupControlID="modalpan" PopupDragHandleControlID="headerdiv" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>

            <asp:Panel ID="modalpan" runat="server" Height="300px" Width="630px" DefaultButton="lbAccept">
                <div id="headerdiv" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">

                </div>

                <div style="background-color:#f7f6f3;  font-size:medium; padding:2px;" class="auto-style1">
                    <table class="auto-style3" width="630">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblModalInstruction" runat="server" Font-Bold="True" ></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" class="auto-style4">
                                <asp:Label ID="lblUser" runat="server" Text="Usuario:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtUser" runat="server" Width="260px" CausesValidation="True"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblUserError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
 
                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblPassword" runat="server" Text="Contraseña:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtPassword" runat="server" Width="260px" TextMode="Password"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblApproveMessage" runat="server" Text="Comentarios:" Width="100px" Font-Bold="True"></asp:Label>
                            </td>

                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtApproveMessage" runat="server" Height="104px" Width="260px" TextMode="MultiLine"></asp:TextBox>
                            </td>

                            <td class="auto-style5">
                                <asp:Label ID="lblApproveMessageError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>

                        <tr style="margin-top: 10px; margin-bottom: 10px">

                            <td colspan="3" class="auto-style2">
                                                                <asp:UpdateProgress ID="UpdateProgress"
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
                                    <asp:LinkButton class="catLinkButtonConfirm" ID="lbAccept"  runat="server" Text="Aceptar <i class='fa fa-check' data-toggle='tooltip' title='Aceptar'></i> "></asp:LinkButton>
                                    <asp:LinkButton class="catLinkButton" ID="lbCancel"  runat="server" Text="Cancelar <i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'></i> "></asp:LinkButton>
        
    </ContentTemplate>
</asp:UpdatePanel>                              
    </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

            
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            <ajaxToolkit:ModalPopupExtender ID="Modaledit" runat="server" TargetControlID="Label3" PopupControlID="editModal" PopupDragHandleControlID="headerdiv0" BackgroundCssClass="modalbackground"></ajaxToolkit:ModalPopupExtender>


            <asp:Panel ID="editModal" runat="server" Height="300px" Width="630px" DefaultButton="lbEdit">
                <div id="headerdiv0" style="background-color:#284775;  font-size:medium; padding:2px; height:20px;">
                </div>
                <div class="auto-style1" style="background-color:#f7f6f3;  font-size:medium; padding:2px;">
                    <table class="auto-style3" width="630">
                        <tr>
                            <td colspan="3" class="auto-style1">
                                <asp:Label ID="lblModalInstruction0" runat="server" Font-Bold="True">Cambio rechazado por:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="auto-style4">Nombre:</td>
                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtUserEdit" runat="server" CausesValidation="True" Enabled="False" Width="260px"></asp:TextBox>
                            </td>
                            <td class="auto-style5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" class="auto-style4">Usuario:</td>
                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtUserEdit0" runat="server" CausesValidation="True" Enabled="False" Width="260px"></asp:TextBox>
                            </td>
                            <td class="auto-style5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:Label ID="lblApproveMessage0" runat="server" Font-Bold="True" Text="Comentarios:" Width="100px"></asp:Label>
                            </td>
                            <td align="left" class="auto-style6">
                                <asp:TextBox ID="txtApproveMessageEdit" runat="server" Height="104px" ReadOnly="True" TextMode="MultiLine" Width="260px"></asp:TextBox>
                            </td>
                            <td class="auto-style5">
                                <asp:Label ID="lblApproveMessageError0" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr style="margin-top: 10px; margin-bottom: 10px">
                            <td colspan="3">
                                                                <asp:UpdateProgress ID="UpdateProgressEdit"
AssociatedUpdatePanelID="updateInProcessEdit"
runat="server">
    <ProgressTemplate>           

      Procesando          
    </ProgressTemplate>
</asp:UpdateProgress>
                                <asp:Label ID="lblModalMessage0" runat="server" ForeColor="Red" Width="504px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div align="center">
                                                                    <asp:UpdatePanel ID="updateInProcessEdit" runat="server">
<ContentTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" class="catLinkButtonConfirm" Text="Ir a Editar &lt;i class='fa fa-check' data-toggle='tooltip' title='Aceptar'&gt;&lt;/i&gt; "></asp:LinkButton>
                                    <asp:LinkButton ID="lbCancel0" runat="server" class="catLinkButton" Text="Cancelar &lt;i class='fa fa-window-close' data-toggle='tooltip' title='Cancelar'&gt;&lt;/i&gt; "></asp:LinkButton>
    
    </ContentTemplate>
</asp:UpdatePanel>                             
    </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
