<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CatWorkOrders.aspx.vb" Inherits="WebApplication1.Catalog_CatWorkOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="UpdatePanelGeneral" runat="server">
        <ContentTemplate>

        <asp:Label ID="lblTitle" runat="server" Font-Size="18pt" Text="Catálogo de Órdenes de Retrabajo" CssClass="catHeader"></asp:Label>

<div style="height: 32px">

</div>

            <div class="catDivHeader">
                <asp:Label ID="lblWorkOrder" CssClass="catLabel" runat="server" Text="Orden de Trabajo:"></asp:Label>
                <asp:TextBox ID="txtWorkOrder" runat="server"></asp:TextBox>
                <asp:DropDownList ID="cmbArea" runat="server" Width="128px" CssClass="catDropDownList">
                    <%--< Estandar, Retrabajo, Modulo General, Directo, CPS >--%> 
                    <asp:ListItem>CPS</asp:ListItem>
                    <asp:ListItem>Directo</asp:ListItem>
                    <asp:ListItem>Estandar</asp:ListItem>
                    <asp:ListItem>Modulo General</asp:ListItem>
                    <asp:ListItem>Retrabajo</asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox ID="chkRework" runat="server" Text="Orden Retrabajada" />
                <asp:LinkButton ID="lBtnSearch" CssClass="catLinkButton" runat="server" Text="<i class='fa fa-search' data-toggle='tooltip' title='Buscar'></i> " />
                <asp:LinkButton ID="lBtnReset" CssClass="catLinkButton" runat="server" Text="<i class='fa fa-regular fa-rotate-right' data-toggle='tooltip' title='Reiniciar'></i>" />
            </div>

<div>
    <br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="12px" Width="479px"></asp:Label><br />

    <br />
</div>

<div align="left" style="padding: 4px">
    <asp:LinkButton style="color:#333333;" ID="btnAddWorkOrder" runat="server" Text="<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>" />
</div>

           

<asp:GridView ID="dgvWorkOrders" runat="server" AutoGenerateColumns="False" CssClass="dgvCatalog" GridLines="None" DataKeyNames="IdCatReworkOrders" Width="750px" AllowPaging="True">
    <Columns>
                       <asp:BoundField DataField="IdCatReworkOrders" HeaderText="IdCatReworkOrders" ReadOnly="True" visible="false">
   <ItemStyle Width="100px" />
   <HeaderStyle HorizontalAlign="Left" />
</asp:BoundField>
        <asp:BoundField DataField="WorkOrder" HeaderText="Orden de Trabajo" SortExpression="workOrder"  />

        <asp:TemplateField HeaderText="Módulo">
            <ItemTemplate>
                <asp:Label  ID="lblArea" runat="server" Text='<%# Bind("Area") %>'>[lblArea]</asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList DataField="Area"  ID="ddlArea" runat="server" Width="128px" Enabled="true" >
                    <asp:ListItem>cps</asp:ListItem>
                    <asp:ListItem>directo</asp:ListItem>
                    <asp:ListItem>estandar</asp:ListItem>
                    <asp:ListItem>modulo general</asp:ListItem>
                    <asp:ListItem>retrabajo</asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField DataField="IsRework" HeaderText="Fue Retrabajada" />
        <asp:BoundField DataField="CreatedOn" HeaderText="Cargada el" ReadOnly="True" SortExpression="createdOn" />
                       <asp:TemplateField ShowHeader="True">
                                                           <HeaderTemplate >
                 <asp:DropDownList ID="DropDownListc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListc_SelectedIndexChanged">
                     <asp:ListItem>----</asp:ListItem>
    <asp:ListItem>10</asp:ListItem>
    <asp:ListItem>50</asp:ListItem>
    <asp:ListItem>100</asp:ListItem>
    <asp:ListItem>Todos</asp:ListItem>
</asp:DropDownList>
                                                               </HeaderTemplate >
                           <EditItemTemplate>
                               <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="&lt;i class='fa fa-regular fa-check' style='color:#333333;' data-toggle='tooltip' title='Actualizar'&gt;&lt;/i&gt;"></asp:LinkButton>
                               &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="&lt;i class='fa fa-regular fa-ban' style='color:#333333;' data-toggle='tooltip' title='Cancelar'&gt;&lt;/i&gt;"></asp:LinkButton>
                           </EditItemTemplate>
                           <ItemTemplate>
                               <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="&lt;i class='fa fa-regular fa-edit' style='color:#333333;' data-toggle='tooltip' title='Editar campo'&gt;&lt;/i&gt;"></asp:LinkButton>
                               &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="&lt;i class='fa fa-regular fa-trash' style='color:#333333;' data-toggle='tooltip' title='Eliminar campo'&gt;&lt;/i&gt;"></asp:LinkButton>
                           </ItemTemplate>
                       </asp:TemplateField>
    </Columns>
     <RowStyle CssClass="dgvCatalogRowOdd" />
     <EditRowStyle BackColor="#999999" />
     <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
     <PagerStyle CssClass="dgvPaging"/>
     <AlternatingRowStyle CssClass="dgvCatalogRowEven" />
</asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
