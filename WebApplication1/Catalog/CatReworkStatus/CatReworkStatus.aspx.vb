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

    Protected Sub dgvStatusTable_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvStatusTable.RowCommand
        Select Case e.CommandName
            'Case "Editar"
            '    'Agregar código para cargar la página de edición mandando el ID del registro
            '    Dim row As DataKeyArray = dgvStatusTable.DataKeys
            '    Session("isEdit") = "True"
            '    Session("idStatus") = row(e.CommandArgument).Value.ToString
            '    Response.Redirect("CatReworkStatusEdit.aspx", False)
            '    Return
            Case "Eliminar"
                'Mostrar mensaje de confirmación para eliminar el registro
                Dim row As DataKeyArray = dgvStatusTable.DataKeys
                Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
                catReworkStatus.Delete(Guid.Parse(row(e.CommandArgument).Value.ToString))
                PopulateGrid("", False, False)
                'Volver a cargar los datos de la tabla
                Return
            Case Else
                'Nada
                Return
        End Select

    End Sub

    Protected Sub btnAddStatus_Click(sender As Object, e As EventArgs) Handles btnAddStatus.Click
        If divAgregar.Visible Then
            divAgregar.Visible = False
            btnAddStatus.Text = "<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>"
        Else
            divAgregar.Visible = True
            btnAddStatus.Text = "<i class='fa fa-regular fa-minus' data-toggle='tooltip' title='Nuevo campo'></i>"
        End If
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

            If (catReworkStatus.AlreadyExistSAPStatus(idStatus, strStatus)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe un Estatus de SAP con el código ingresado: [" + strStatus + "]"
            Else
                catReworkStatus.Update(idStatus, strStatus, boolStatus, "Admin")
                catReworkStatus = Nothing

                dgvStatusTable.EditIndex = -1
                PopulateGrid("", False, False)
            End If

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try
    End Sub

    Protected Sub dgvStatusTable_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvStatusTable.RowCancelingEdit
        dgvStatusTable.EditIndex = -1
        PopulateGrid("", False, False)
    End Sub

    Protected Sub AgregarEstatus_Click(sender As Object, e As EventArgs) Handles AgregarEstatus.Click
        Dim strStatus As String = addEstatus.Text.Trim.ToUpper()
        Dim boolStatus As Boolean = addRetrabajo.Checked

        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        catReworkStatus.Insert(strStatus, boolStatus, True, "Admin")
        catReworkStatus = Nothing

        PopulateGrid("", False, False)
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strStatus As String = txtStatus.Text.Trim.ToUpper()
        Dim boolStatus As Boolean = chkRework.Checked

        PopulateGrid(strStatus, boolStatus, True)
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtStatus.Text = ""
        chkRework.Checked = False

        PopulateGrid("", False, False)
    End Sub
    Protected Sub dgvStatusTable_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvStatusTable.PageIndexChanging
        dgvStatusTable.PageIndex = e.NewPageIndex

        PopulateGrid("", False, False)
    End Sub

    Protected Sub DropDownListP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListP.SelectedIndexChanged
        dgvStatusTable.AllowPaging = True
        Select Case DropDownListP.SelectedIndex
            Case 0
                dgvStatusTable.PageSize = 10
                PopulateGrid("", False, False)
            Case 1
                dgvStatusTable.PageSize = 50
                PopulateGrid("", False, False)
            Case 2
                dgvStatusTable.PageSize = 100
                PopulateGrid("", False, False)
            Case 3
                dgvStatusTable.AllowPaging = False
                PopulateGrid("", False, False)

        End Select
    End Sub

    Protected Sub dgvStatusTable_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvStatusTable.RowDeleting
        Dim row As DataKeyArray = dgvStatusTable.DataKeys
        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        catReworkStatus.Delete(Guid.Parse(row(e.RowIndex).Value.ToString))
        PopulateGrid("", False, False)
    End Sub
End Class