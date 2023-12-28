Imports System.Diagnostics.Eventing
Imports Microsoft.Ajax.Utilities
Imports Newtonsoft.Json.Linq

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


    Protected Sub btnAddUnit_Click(sender As Object, e As EventArgs) Handles btnAddUnit.Click
        Dim catUnits As CatUnits = New CatUnits()
        Dim dt As DataTable = catUnits.SelectAll("", False)
        Dim row As DataRow
        row = dt.NewRow()
        row("IdUnit") = Guid.NewGuid
        row("Unit") = ""
        row("UnitValue") = 0
        row("IsActive") = True
        dt.Rows.Add(row)
        Dim dv As DataView = dt.AsDataView
        dv.Sort = "Unit" + " " + "ASC"
        dgvUnits.EditIndex = 0
        dgvUnits.DataSource = dv
        dgvUnits.DataBind()
    End Sub

    Protected Sub dgvUnits_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvUnits.RowUpdating
        Try
            lblMessage.Text = ""

            Dim catUnits As CatUnits = New CatUnits()
            Dim row As GridViewRow = dgvUnits.Rows(e.RowIndex)
            Dim names As DataKeyArray = dgvUnits.DataKeys()
            Dim idUnit As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
            Dim strUnit As String = CType((row.Cells(1).Controls(0)), TextBox).Text.Trim.ToUpper()
            Dim strUnitQty As String = CType((row.Cells(2).Controls(0)), TextBox).Text.Trim.ToUpper()
            Dim strUnitValue As String = CType((row.Cells(3).Controls(1)), DropDownList).Text.Trim.ToUpper()

            If (strUnit = "") Then
                lblMessage.Text = "Favor de escribir un nombre para la Unidad"
                Return
            End If

            If (strUnitQty = "") Then
                lblMessage.Text = "Favor de escribir un valor para la Unidad"
                Return
            End If
            If (strUnitQty = "0") Then
                lblMessage.Text = "El valor de la unidad debe ser un numero mayor a 0"
                Return
            End If

            If (Not Regex.IsMatch(strUnitQty, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numerico"
                Return
            End If
            Dim find As String = "IdUnit ='" + idUnit.ToString + "'"
            Dim temp As Integer = catUnits.SelectAll("", False).Select(find).Count
            If (temp = 0) Then
                If (catUnits.AlreadyExistUnit(idUnit, strUnit)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
                Else
                    catUnits.Insert(strUnit, strUnitQty, True, "Admin", strUnitValue)
                    MsgBox("Se agregó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
                    catUnits = Nothing

                    dgvUnits.EditIndex = -1
                    PopulateGrid("", False)
                End If
            Else
                If (catUnits.AlreadyExistUnit(idUnit, strUnit)) Then
                    lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
                Else
                    catUnits.Update(idUnit, strUnit, strUnitQty, True, "Admin", strUnitValue)
                    MsgBox("Se actualizó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
                    catUnits = Nothing

                    dgvUnits.EditIndex = -1
                    PopulateGrid("", False)
                End If
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


    Protected Sub dgvUnits_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvUnits.PageIndexChanging
        dgvUnits.PageIndex = e.NewPageIndex

        PopulateGrid("", False)
    End Sub

    Protected Sub dgvUnits_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvUnits.RowDeleting
        Dim confirmed As Integer = MsgBox("Se eliminará el registro seleccionado. ¿Desea proceder con el borrado?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Confirmar Borrado")

        If confirmed = MsgBoxResult.Yes Then
            Dim row As DataKeyArray = dgvUnits.DataKeys
            Dim catUnits As CatUnits = New CatUnits()
            catUnits.Delete(Guid.Parse(row(e.RowIndex).Value.ToString))
            PopulateGrid("", False)
        Else
            e.Cancel = True
        End If

    End Sub

    Protected Sub lBtnSearch_Click(sender As Object, e As EventArgs) Handles lBtnSearch.Click
        Dim strUnit As String = txtUnit.Text.Trim.ToUpper()

        PopulateGrid(strUnit, True)
    End Sub

    Protected Sub lBtnReset_Click(sender As Object, e As EventArgs) Handles lBtnReset.Click
        txtUnit.Text = ""

        PopulateGrid("", False)
    End Sub

    Protected Sub DropDownListc_SelectedIndexChanged(sender As Object, e As EventArgs)
        dgvUnits.AllowPaging = True
        Dim x As DropDownList = sender
        Select Case x.SelectedIndex
            Case 1
                dgvUnits.PageSize = 10
                PopulateGrid("", False)
            Case 2
                dgvUnits.PageSize = 50
                PopulateGrid("", False)
            Case 3
                dgvUnits.PageSize = 100
                PopulateGrid("", False)
            Case 4
                dgvUnits.AllowPaging = False
                PopulateGrid("", False)
        End Select
        dgvUnits.EditIndex = -1
    End Sub
End Class