Public Class Catalog_CatUnits
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateGrid("", False)
        End If


    End Sub

    Protected Sub PopulateGrid(unit As String, isSearch As Boolean)
        Dim catUnits As CatUnits = New CatUnits()
        dgvUnits.DataSource = catUnits.SelectAll(unit, isSearch)
        dgvUnits.DataBind()
    End Sub

    Protected Sub dgvUnits_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvUnits.RowCommand
        Select Case e.CommandName
            Case "Editar"
                ''Agregar código para cargar la página de edición mandando el ID del registro
                'Dim row As DataKeyArray = dgvUnits.DataKeys
                'Session("isEdit") = "True"
                'Session("idUnit") = row(e.CommandArgument).Value.ToString
                'Response.Redirect("CatUnitsEdit.aspx", False)
                'Return
            Case "Eliminar"
                'Mostrar mensaje de confirmación para eliminar el registro
                Dim row As DataKeyArray = dgvUnits.DataKeys
                Dim catUnits As CatUnits = New CatUnits()
                catUnits.Delete(Guid.Parse(row(e.CommandArgument).Value.ToString))
                PopulateGrid("", False)
                'Volver a cargar los datos de la tabla
                Return
            Case Else
                'Nada
                Return
        End Select
    End Sub

    Protected Sub btnAddUnit_Click(sender As Object, e As EventArgs) Handles btnAddUnit.Click
        If divAgregar.Visible Then
            divAgregar.Visible = False
            btnAddUnit.Text = "<i class='fa fa-regular fa-plus' data-toggle='tooltip' title='Nuevo campo'></i>"
        Else
            divAgregar.Visible = True
            btnAddUnit.Text = "<i class='fa fa-regular fa-minus' data-toggle='tooltip' title='Nuevo campo'></i>"
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strUnit As String = txtUnit.Text.Trim.ToUpper()

        PopulateGrid(strUnit, True)
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtUnit.Text = ""

        PopulateGrid("", False)
    End Sub

    Protected Sub dgvUnits_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvUnits.RowUpdating
        Try
            lblMessage.Text = ""

            Dim catUnits As CatUnits = New CatUnits()
            Dim row As GridViewRow = dgvUnits.Rows(e.RowIndex)
            Dim names As DataKeyArray = dgvUnits.DataKeys()
            Dim idUnit As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
            Dim strUnit As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
            Dim strUnitValue As String = CType((row.Cells(2).Controls(0)), TextBox).Text.Trim.ToUpper()

            If (strUnit = "") Then
                lblMessage.Text = "Favor de escribir un nombre para la Unidad"
                Return
            End If

            If (strUnitValue = "") Then
                lblMessage.Text = "Favor de escribir un valor para la Unidad"
                Return
            End If

            If (catUnits.AlreadyExistUnit(idUnit, strUnit)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
            Else
                catUnits.Update(idUnit, strUnit, strUnitValue, True, "Admin")
                catUnits = Nothing

                dgvUnits.EditIndex = -1
                PopulateGrid("", False)
            End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para el valor de la Unidad"

        Catch ex As OverflowException
            lblMessage.Text = "El valor de la Unidad es demasiado grande para guardar"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try



    End Sub

    Protected Sub dgvUnits_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvUnits.RowEditing
        dgvUnits.EditIndex = e.NewEditIndex
        PopulateGrid("", False)
    End Sub

    Protected Sub dgvUnits_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvUnits.RowCancelingEdit
        dgvUnits.EditIndex = -1
        PopulateGrid("", False)
    End Sub

    Protected Sub AgregarUnidad_Click(sender As Object, e As EventArgs) Handles AgregarUnidad.Click
        Dim strUnit As String = addUnidad.Text.Trim.ToUpper()
        Dim strUnitValue As String = addValor.Text.Trim.ToUpper()

        Dim catUnits As CatUnits = New CatUnits()
        catUnits.Insert(strUnit, strUnitValue, True, "Admin")
        catUnits = Nothing

        PopulateGrid("", False)
    End Sub

    Protected Sub dgvUnits_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvUnits.PageIndexChanging
        dgvUnits.PageIndex = e.NewPageIndex

        PopulateGrid("", False)
    End Sub
End Class