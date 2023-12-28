Imports System.Net.NetworkInformation

Public Class Catalog_CatReworkStatus
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Traer los datos de la tabla para mostrar en el gridview
        If Not Page.IsPostBack Then
            PopulateGrid("", False, False)
        End If


    End Sub

    Protected Sub PopulateGrid(sapStatus As String, isRework As Boolean, isSearch As Boolean)
        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        dgvStatusTable.DataSource = catReworkStatus.SelectAll(sapStatus, isRework, isSearch)
        dgvStatusTable.DataBind()
    End Sub


    Protected Sub btnAddStatus_Click(sender As Object, e As EventArgs) Handles btnAddStatus.Click
        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        Dim dt As DataTable = catReworkStatus.SelectAll("", False, False)
        Dim row As DataRow
        row = dt.NewRow()
        row("IdCatReworkStatus") = Guid.NewGuid
        row("SAPStatus") = ""
        row("IsRework") = True
        row("IsActive") = True
        dt.Rows.Add(row)
        Dim dv As DataView = dt.AsDataView
        dv.Sort = "SAPStatus" + " " + "ASC"
        dgvStatusTable.DataSource = dv
        dgvStatusTable.EditIndex = 0
        dgvStatusTable.DataBind()
    End Sub

    Protected Sub dgvStatusTable_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvStatusTable.RowEditing
        dgvStatusTable.EditIndex = e.NewEditIndex
        PopulateGrid("", False, False)
    End Sub

    Protected Sub dgvStatusTable_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvStatusTable.RowUpdating
        Try
            lblMessage.Text = ""

            Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
            Dim row As GridViewRow = dgvStatusTable.Rows(e.RowIndex)
            Dim names As DataKeyArray = dgvStatusTable.DataKeys()
            Dim idStatus As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
            Dim strStatus As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
            Dim boolStatus As Boolean = CType((row.Cells(2).Controls(0)), CheckBox).Checked


            If (strStatus = "") Then
                lblMessage.Text = "Favor de escribir un Código de Estatus"
                Return
            End If

            Dim find As String = "IdCatReworkStatus ='" + idStatus.ToString + "'"
            Dim temp As Integer = catReworkStatus.SelectAll("", False, False).Select(find).Count
            If (temp = 0) Then
                If (catReworkStatus.AlreadyExistSAPStatus(idStatus, strStatus)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe un Estatus de SAP con el código ingresado: [" + strStatus + "]"
                Else
                    catReworkStatus.Insert(strStatus, boolStatus, True, "Admin")
                    MsgBox("Se agregó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
                    catReworkStatus = Nothing

                    dgvStatusTable.EditIndex = -1
                    PopulateGrid("", False, False)
                End If
            Else
                If (catReworkStatus.AlreadyExistSAPStatus(idStatus, strStatus)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe un Estatus de SAP con el código ingresado: [" + strStatus + "]"
                Else
                    catReworkStatus.Update(idStatus, strStatus, boolStatus, "Admin")
                    MsgBox("Se actualizó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
                    catReworkStatus = Nothing

                    dgvStatusTable.EditIndex = -1
                    PopulateGrid("", False, False)
                End If
            End If

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try
    End Sub

    Protected Sub dgvStatusTable_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvStatusTable.RowCancelingEdit
        dgvStatusTable.EditIndex = -1
        PopulateGrid("", False, False)
    End Sub
    Protected Sub dgvStatusTable_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvStatusTable.PageIndexChanging
        dgvStatusTable.PageIndex = e.NewPageIndex

        PopulateGrid("", False, False)
    End Sub


    Protected Sub dgvStatusTable_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvStatusTable.RowDeleting
        Dim confirmed As Integer = MsgBox("Se eliminará el registro seleccionado. ¿Desea proceder con el borrado?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Confirmar Borrado")

        If confirmed = MsgBoxResult.Yes Then
            Dim row As DataKeyArray = dgvStatusTable.DataKeys
            Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
            catReworkStatus.Delete(Guid.Parse(row(e.RowIndex).Value.ToString))
            PopulateGrid("", False, False)
        Else
            e.Cancel = True
        End If
    End Sub

    Protected Sub dgvStatusTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvStatusTable.SelectedIndexChanged

    End Sub

    Protected Sub lBtnReset_Click(sender As Object, e As EventArgs) Handles lBtnReset.Click
        txtStatus.Text = ""
        chkRework.Checked = False

        PopulateGrid("", False, False)
    End Sub

    Protected Sub lBtnSearch_Click(sender As Object, e As EventArgs) Handles lBtnSearch.Click
        Dim strStatus As String = txtStatus.Text.Trim.ToUpper()
        Dim boolStatus As Boolean = chkRework.Checked

        PopulateGrid(strStatus, boolStatus, True)
    End Sub

    Protected Sub DropDownListc_SelectedIndexChanged(sender As Object, e As EventArgs)
        dgvStatusTable.AllowPaging = True
        Dim x As DropDownList = sender
        Select Case x.SelectedIndex
            Case 1
                dgvStatusTable.PageSize = 10
                PopulateGrid("", False, False)
            Case 2
                dgvStatusTable.PageSize = 50
                PopulateGrid("", False, False)
            Case 3
                dgvStatusTable.PageSize = 100
                PopulateGrid("", False, False)
            Case 4
                dgvStatusTable.AllowPaging = False
                PopulateGrid("", False, False)
        End Select
        dgvStatusTable.EditIndex = -1
    End Sub
End Class