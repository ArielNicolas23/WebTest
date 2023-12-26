Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Diagnostics.Eventing

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
        Session("dtWos") = catReworkOrders.SelectAll(workOrder, area, isRework, isSearch)
        dgvWorkOrders.DataBind()
    End Sub



    Protected Sub btnAddWorkOrder_Click(sender As Object, e As EventArgs) Handles btnAddWorkOrder.Click
        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        Dim dt As DataTable = catReworkOrders.SelectAll("", "", False, False)
        Dim row As DataRow
        row = dt.NewRow()
        row("IdCatReworkOrders") = Guid.NewGuid
        row("WorkOrder") = ""
        row("Area") = ""
        row("IsRework") = True
        row("CreatedOn") = ""
        dt.Rows.Add(row)
        Dim dv As DataView = dt.AsDataView
        dv.Sort = "WorkOrder" + " " + "ASC"
        dgvWorkOrders.EditIndex = 0
        dgvWorkOrders.DataSource = dv
        dgvWorkOrders.DataBind()
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

            If (Not Regex.IsMatch(strWo, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numerico"
                Return
            End If

            If (strModulo = "") Then
                lblMessage.Text = "Favor de seleccioanr un Módulo para la Orden"
                Return
            End If

            Dim find As String = "IdCatReworkOrders ='" + idWo.ToString + "'"
            Dim temp As Integer = catReworkOrders.SelectAll("", "", False, False).Select(find).Count
            If (temp = 0) Then
                If (catReworkOrders.AlreadyExistWorkOrder(idWo, strWo)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Orden de Trabajo con el número ingresado: [" + strWo + "]"
                Else
                    catReworkOrders.Insert(strWo, strModulo, boolRework, True, "Admin")
                    MsgBox("Se agregó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Exito")
                    catReworkOrders = Nothing

                    dgvWorkOrders.EditIndex = -1
                    PopulateGrid("", "", False, False)
                End If
            Else
                If (catReworkOrders.AlreadyExistWorkOrder(idWo, strWo)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Orden de Trabajo con el número ingresado: [" + strWo + "]"
                Else
                    catReworkOrders.Update(idWo, strWo, strModulo, boolRework, "Admin")
                    MsgBox("Se actualizó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
                    catReworkOrders = Nothing

                    dgvWorkOrders.EditIndex = -1
                    PopulateGrid("", "", False, False)
                End If
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

        Dim dt As System.Data.DataTable = Session("dtWos")
        Dim unit = dt.Rows(e.NewEditIndex).Item(2).ToString.ToLower
        PopulateGrid("", "", False, False)
        'Configurar dropdown list
        Dim ddlUnitEditGrid As DropDownList = CType(dgvWorkOrders.Rows(e.NewEditIndex).FindControl("ddlArea"), DropDownList)
        ddlUnitEditGrid.Items.FindByText(unit).Selected = True


    End Sub
    Protected Sub dgvWorkOrders_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvWorkOrders.PageIndexChanging
        dgvWorkOrders.PageIndex = e.NewPageIndex

        PopulateGrid("", "", False, False)
    End Sub


    Protected Sub dgvWorkOrders_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvWorkOrders.RowDeleting
        Dim confirmed As Integer = MsgBox("Se eliminará el registro seleccionado. ¿Desea proceder con el borrado?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Confirmar Borrado")

        If confirmed = MsgBoxResult.Yes Then
            Dim row As DataKeyArray = dgvWorkOrders.DataKeys
            Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
            catReworkOrders.Delete(Guid.Parse(row(e.RowIndex).Value.ToString))
            PopulateGrid("", "", False, False)
        Else
            e.Cancel = True
        End If
    End Sub

    Protected Sub lBtnSearch_Click(sender As Object, e As EventArgs) Handles lBtnSearch.Click
        Dim strWo As String = txtWorkOrder.Text.Trim.ToUpper()
        Dim strModulo As String = cmbArea.Text.Trim.ToUpper()
        Dim boolRework As Boolean = chkRework.Checked

        PopulateGrid(strWo, strModulo, boolRework, True)
    End Sub

    Protected Sub lBtnReset_Click(sender As Object, e As EventArgs) Handles lBtnReset.Click
        txtWorkOrder.Text = ""
        cmbArea.SelectedIndex = 0
        chkRework.Checked = False

        PopulateGrid("", "", False, False)
    End Sub

    Protected Sub DropDownListc_SelectedIndexChanged(sender As Object, e As EventArgs)
        dgvWorkOrders.AllowPaging = True
        Dim x As DropDownList = sender
        Select Case x.SelectedIndex
            Case 1
                dgvWorkOrders.PageSize = 10
                PopulateGrid("", "", False, False)
            Case 2
                dgvWorkOrders.PageSize = 50
                PopulateGrid("", "", False, False)
            Case 3
                dgvWorkOrders.PageSize = 100
                PopulateGrid("", "", False, False)
            Case 4
                dgvWorkOrders.AllowPaging = False
                PopulateGrid("", "", False, False)
        End Select
        dgvWorkOrders.EditIndex = -1
    End Sub
End Class