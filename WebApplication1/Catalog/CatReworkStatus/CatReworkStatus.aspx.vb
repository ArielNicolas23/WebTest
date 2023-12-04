Imports System.Net.NetworkInformation

Public Class Catalog_CatReworkStatus
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Traer los datos de la tabla para mostrar en el gridview
        If Not Page.IsPostBack Then
            PopulateGrid()
        End If


    End Sub

    Protected Sub PopulateGrid()

        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        dgvStatusTable.DataSource = catReworkStatus.SelectAll()
        dgvStatusTable.DataBind()


        catReworkStatus = Nothing

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
            Case "Delete"
                'Mostrar mensaje de confirmación para eliminar el registro
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
        PopulateGrid()
    End Sub

    Protected Sub dgvStatusTable_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvStatusTable.RowUpdating
        Dim row As GridViewRow = dgvStatusTable.Rows(e.RowIndex)
        Dim names As DataKeyArray = dgvStatusTable.DataKeys()
        Dim idStatus As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
        Dim strStatus As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
        Dim boolStatus As Boolean = CType((row.Cells(2).Controls(0)), CheckBox).Checked

        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        catReworkStatus.Update(idStatus, strStatus, boolStatus, "Admin")
        catReworkStatus = Nothing

        dgvStatusTable.EditIndex = -1
        PopulateGrid()
    End Sub

    Protected Sub dgvStatusTable_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvStatusTable.RowCancelingEdit
        dgvStatusTable.EditIndex = -1
        PopulateGrid()
    End Sub

    Protected Sub AgregarEstatus_Click(sender As Object, e As EventArgs) Handles AgregarEstatus.Click
        Dim strStatus As String = addEstatus.Text.Trim.ToUpper()
        Dim boolStatus As Boolean = addRetrabajo.Checked

        Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
        catReworkStatus.Insert(strStatus, boolStatus, True, "Admin")
        catReworkStatus = Nothing

        PopulateGrid()
    End Sub

    Protected Sub dgvStatusTable_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvStatusTable.PageIndexChanging
        dgvStatusTable.PageIndex = e.NewPageIndex

        PopulateGrid()
    End Sub
End Class