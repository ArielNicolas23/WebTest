Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder

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
                ''Agregar código para cargar la página de edición mandando el ID del registro
                'Dim row As DataKeyArray = dgvWorkOrders.DataKeys
                'Session("isEdit") = "True"
                'Session("idWo") = row(e.CommandArgument).Value.ToString
                'Response.Redirect("CatWorkOrdersEdit.aspx", False)
                'Return
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
        If divAgregar.Visible Then
            divAgregar.Visible = False
            btnAddWorkOrder.Text = "<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>"
        Else
            divAgregar.Visible = True
            btnAddWorkOrder.Text = "<i class='fa fa-regular fa-minus' data-toggle='tooltip' title='Nuevo campo'></i>"
        End If
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

    Protected Sub dgvWorkOrders_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvWorkOrders.RowUpdating
        Try
            lblMessage.Text = ""

            Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
            Dim row As GridViewRow = dgvWorkOrders.Rows(e.RowIndex)
            Dim names As DataKeyArray = dgvWorkOrders.DataKeys()
            Dim idWo As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
            Dim strWo As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
            Dim strModulo As String = CType((row.Cells(2).Controls(1)), DropDownList).Text.Trim.ToUpper()
            Dim boolRework As Boolean = CType((row.Cells(3).Controls(0)), CheckBox).Checked

            If (strWo = "") Then
                lblMessage.Text = "Favor de escribir una Orden de Trabajo"
                Return
            End If

            If (strWo.Length < 9 Or strWo.Length > 9) Then
                lblMessage.Text = "El número de Orden de Trabajo debe contener 9 dígitos"
                Return
            End If

            If (strModulo = "") Then
                lblMessage.Text = "Favor de seleccioanr un Módulo para la Orden"
                Return
            End If

            If (catReworkOrders.AlreadyExistWorkOrder(idWo, strWo)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Orden de Trabajo con el número ingresado: [" + strWo + "]"
            Else
                catReworkOrders.Update(idWo, strWo, strModulo, boolRework, "Admin")
                catReworkOrders = Nothing

                dgvWorkOrders.EditIndex = -1
                PopulateGrid("", "", False, False)
            End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para la Orden de Trabajo"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try

    End Sub

    Protected Sub dgvWorkOrders_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvWorkOrders.RowCancelingEdit
        dgvWorkOrders.EditIndex = -1
        PopulateGrid("", "", False, False)
    End Sub

    Protected Sub dgvWorkOrders_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvWorkOrders.RowEditing
        dgvWorkOrders.EditIndex = e.NewEditIndex
        PopulateGrid("", "", False, False)
    End Sub

    Protected Sub AgregarOrden_Click(sender As Object, e As EventArgs) Handles AgregarOrden.Click

        Dim strWo As String = addOrden.Text.Trim.ToUpper()
        Dim strModulo As String = addArea.Text.Trim.ToUpper()

        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        catReworkOrders.Insert(strWo, strModulo, False, True, "Admin")
        catReworkOrders = Nothing

        PopulateGrid("", "", False, False)
    End Sub

    Protected Sub dgvWorkOrders_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvWorkOrders.PageIndexChanging
        dgvWorkOrders.PageIndex = e.NewPageIndex

        PopulateGrid("", "", False, False)
    End Sub
End Class