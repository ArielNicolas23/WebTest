﻿Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder

Public Class Catalog_CatWorkOrders
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Traer los datos de la tabla para mostrar en el gridview
        If Not Page.IsPostBack Then
            PopulateGrid()
        End If


    End Sub

    Protected Sub PopulateGrid()

        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        dgvWorkOrders.DataSource = catReworkOrders.SelectAll()
        dgvWorkOrders.DataBind()


        catReworkOrders = Nothing

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
            Case "Delete"
                'Mostrar mensaje de confirmación para eliminar el registro
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

    End Sub

    Protected Sub dgvWorkOrders_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvWorkOrders.RowUpdating
        Dim row As GridViewRow = dgvWorkOrders.Rows(e.RowIndex)
        Dim names As DataKeyArray = dgvWorkOrders.DataKeys()
        Dim idWo As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
        Dim strWo As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
        Dim strModulo As String = CType((row.Cells(2).Controls(1)), DropDownList).Text.Trim.ToUpper()
        Dim boolRework As Boolean = CType((row.Cells(3).Controls(0)), CheckBox).Checked

        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        lblTitle.Text = "Editar Unidad"
        'Traer la información del registro a editar y desplegar su información
        Dim datos As DataTable = catReworkOrders.SelectOne(idWo)




        catReworkOrders.Update(idWo, strWo, strModulo, boolRework, "Admin")
        catReworkOrders = Nothing

        dgvWorkOrders.EditIndex = -1
        PopulateGrid()
    End Sub

    Protected Sub dgvWorkOrders_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvWorkOrders.RowCancelingEdit
        dgvWorkOrders.EditIndex = -1
        PopulateGrid()
    End Sub

    Protected Sub dgvWorkOrders_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvWorkOrders.RowEditing
        dgvWorkOrders.EditIndex = e.NewEditIndex
        PopulateGrid()
    End Sub

    Protected Sub AgregarOrden_Click(sender As Object, e As EventArgs) Handles AgregarOrden.Click

        Dim strWo As String = addOrden.Text.Trim.ToUpper()
        Dim strModulo As String = addArea.Text.Trim.ToUpper()

        Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
        catReworkOrders.Insert(strWo, strModulo, False, True, "Admin")
        catReworkOrders = Nothing

        PopulateGrid()
    End Sub

    Protected Sub dgvWorkOrders_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvWorkOrders.PageIndexChanging
        dgvWorkOrders.PageIndex = e.NewPageIndex

        PopulateGrid()
    End Sub
End Class