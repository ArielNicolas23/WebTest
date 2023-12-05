Public Class Catalog_CatWorkOrders
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Traer los datos de la tabla para mostrar en el gridview
        If Not Page.IsPostBack Then
            PopulateGrid("", "", False, False)
        End If
    End Sub

    Protected Sub PopulateGrid(workOrder As String, area As String, isRework As Boolean, isSearch As Boolean)
        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        dgvWorkOrders.DataSource = catReworkOrders.SelectAll(workOrder, area, isRework, isSearch)
        dgvWorkOrders.DataBind()
    End Sub


    Protected Sub dgvWorkOrders_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvWorkOrders.RowCommand
        Select Case e.CommandName
            Case "Editar"
                'Agregar código para cargar la página de edición mandando el ID del registro
                Dim row As DataKeyArray = dgvWorkOrders.DataKeys
                Session("isEdit") = "True"
                Session("idWo") = row(e.CommandArgument).Value.ToString
                Response.Redirect("CatWorkOrdersEdit.aspx", False)
                Return
            Case "Eliminar"
                'Mostrar mensaje de confirmación para eliminar el registro
                Dim row As DataKeyArray = dgvWorkOrders.DataKeys
                Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
                catReworkOrders.Delete(Guid.Parse(row(e.CommandArgument).Value.ToString))
                PopulateGrid("", "", False, False)
                'Volver a cargar los datos de la tabla
                Return
            Case Else
                'Nada
                Return
        End Select
    End Sub

    Protected Sub btnAddWorkOrder_Click(sender As Object, e As EventArgs) Handles btnAddWorkOrder.Click
        Session("idWo") = "False"
        Response.Redirect("CatWorkOrdersEdit.aspx", False)
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strWo As String = txtWorkOrder.Text.Trim.ToUpper()
        Dim strModulo As String = cmbArea.Text.Trim.ToUpper()
        Dim boolRework As Boolean = chkRework.Checked

        PopulateGrid(strWo, strModulo, boolRework, True)
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtWorkOrder.Text = ""
        cmbArea.SelectedIndex = 0
        chkRework.Checked = False

        PopulateGrid("", "", False, False)
    End Sub

End Class