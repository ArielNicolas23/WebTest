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
            Case "Editar"
                'Agregar código para cargar la página de edición mandando el ID del registro
                Dim row As DataKeyArray = dgvStatusTable.DataKeys
                Session("isEdit") = "True"
                Session("idStatus") = row(e.CommandArgument).Value.ToString
                Response.Redirect("CatReworkStatusEdit.aspx", False)
                Return
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
        Session("isEdit") = "False"
        Response.Redirect("CatReworkStatusEdit.aspx", False)
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
End Class