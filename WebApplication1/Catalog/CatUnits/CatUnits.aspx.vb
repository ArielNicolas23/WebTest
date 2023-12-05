﻿Public Class Catalog_CatUnits
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateGrid()
        End If


    End Sub

    Protected Sub PopulateGrid()

        Dim catUnits As CatUnits = New CatUnits()
        dgvUnits.DataSource = catUnits.SelectAll("", False)
        dgvUnits.DataBind()


        catUnits = Nothing

    End Sub
    Protected Sub dgvUnits_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvUnits.RowCommand
        Select Case e.CommandName
            Case "Editar"
                'Agregar código para cargar la página de edición mandando el ID del registro
                Dim row As DataKeyArray = dgvUnits.DataKeys
                Session("isEdit") = "True"
                Session("idUnit") = row(e.CommandArgument).Value.ToString
                Response.Redirect("CatUnitsEdit.aspx", False)
                Return
            Case "Eliminar"
                'Mostrar mensaje de confirmación para eliminar el registro
                Dim row As DataKeyArray = dgvUnits.DataKeys
                Dim catUnits As CatUnits = New CatUnits()
                catUnits.Delete(Guid.Parse(row(e.CommandArgument).Value.ToString))
                catUnits = Nothing
                GetData()
                'Volver a cargar los datos de la tabla
                Return
            Case Else
                'Nada
                Return
        End Select
    End Sub
    Protected Sub btnAddUnit_Click(sender As Object, e As EventArgs) Handles btnAddUnit.Click
        Session("isEdit") = "False"
        Response.Redirect("CatUnitsEdit.aspx", False)
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strUnit As String = txtUnit.Text.Trim.ToUpper()
        'Dim strUnitValue As String = txtUnitValue.Text.Trim.ToUpper()
        Dim catUnits As CatUnits = New CatUnits()

        dgvUnits.DataSource = catUnits.SelectAll(strUnit, True)
        dgvUnits.DataBind()


        catUnits = Nothing
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        txtUnit.Text = ""
        'txtUnitValue.Text = ""

        Dim catUnits As CatUnits = New CatUnits()
        dgvUnits.DataSource = catUnits.SelectAll("", False)
        dgvUnits.DataBind()


        catUnits = Nothing
    End Sub

    Private Sub GetData()
        Dim catUnits As CatUnits = New CatUnits()
        dgvUnits.DataSource = catUnits.SelectAll("", False)
        dgvUnits.DataBind()
    End Sub
End Class