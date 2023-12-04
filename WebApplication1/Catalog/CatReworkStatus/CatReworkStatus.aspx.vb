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
            Case "Editar"
                'Agregar código para cargar la página de edición mandando el ID del registro
                Dim row As DataKeyArray = dgvStatusTable.DataKeys
                Session("isEdit") = "True"
                Session("idStatus") = row(e.CommandArgument).Value.ToString
                Response.Redirect("CatReworkStatusEdit.aspx", False)
                Return
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
        Session("isEdit") = "False"
        Response.Redirect("CatReworkStatusEdit.aspx", False)
    End Sub

End Class