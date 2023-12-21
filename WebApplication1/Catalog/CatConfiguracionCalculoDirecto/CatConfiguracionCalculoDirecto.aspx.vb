Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.WebPages
Imports AjaxControlToolkit
Imports Microsoft.Ajax.Utilities
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

Public Class CatConfiguracionCalculoDirecto
    Inherits System.Web.UI.Page

    Dim objApp As Excel.Application
    Dim objBook As Excel._Workbook

    ' Variables de las conexiones a BD
    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim catUnits As CatUnits = New CatUnits()
    Dim userPlaceholder As String

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)

        If Not Page.IsPostBack Then
            Session("SelectedModels") = GenerateTable()
            PopulateDropDown()
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, Guid.Empty))
        End If
    End Sub

    ' Llenado de dropdown
    Protected Sub PopulateDropDown()
        ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
        ddlUnit.DataTextField = "Unit"
        ddlUnit.DataValueField = "IdUnit"
        ddlUnit.DataBind()
    End Sub

    ' Llenado de tablas
    Protected Sub PopulateGrid(dgv As GridView, dt As Data.DataTable)
        dgv.DataSource = dt
        dgv.DataBind()
    End Sub

    ' Toggle para tabla de seleccionados
    Protected Sub ToggleSelected()
        If dgvSelectedModels.Rows.Count > 0 Then
            divSelectedModels.Visible = True
        Else
            divSelectedModels.Visible = False
        End If
    End Sub

    ' Toggle para sección de edición
    Protected Sub ToggleEdition(showEditing As Boolean)
        If showEditing Then
            lblTitle.Text = "Edición de Modelos Aprobados"
            divHeader.Visible = False
            divApprovedModels.Visible = False
            divEditModels.Visible = True
        Else
            lblTitle.Text = "Catálogo de Configuración de Cálculo Directo"
            divHeader.Visible = True
            divApprovedModels.Visible = True
            divEditModels.Visible = False
        End If
    End Sub

    ' Base de las tablas
    Protected Function GenerateTable()
        Dim dataTable As New System.Data.DataTable

        dataTable.Columns.Add(New DataColumn("IdModelsChanges"))
        dataTable.Columns.Add(New DataColumn("IdModelsChangesHeader"))
        dataTable.Columns.Add(New DataColumn("IdCatUnits"))
        dataTable.Columns.Add(New DataColumn("Model"))
        dataTable.Columns.Add(New DataColumn("Lifespan"))
        dataTable.Columns.Add(New DataColumn("Unit"))
        dataTable.Columns.Add(New DataColumn("LastUser"))
        dataTable.Columns.Add(New DataColumn("ApproverUser"))
        dataTable.Columns.Add(New DataColumn("ApprovedOn"))

        Return dataTable
    End Function










    ' Búsqueda de modelos
    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim idCatUnits As Guid

        If ddlUnit.SelectedValue = "" Then
            idCatUnits = Guid.Empty
        Else
            idCatUnits = Guid.Parse(ddlUnit.SelectedValue)
        End If

        PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, idCatUnits))
    End Sub

    ' Botón para ir a editar
    Protected Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        If dgvSelectedModels.Rows.Count > 0 Then

            Dim rowkeySelected As DataKeyArray = dgvSelectedModels.DataKeys

            Dim dtable As Data.DataTable = GenerateTable()

            Dim i As Integer = dgvSelectedModels.Columns.Count
            Dim RowValues As Object() = {"", "", "", "", "", "", "", "", ""}
            For Each copyrow In dgvSelectedModels.Rows
                'creo que los for se pueden sacar del for each pero lo dejo como estaba funcional
                For index As Integer = 0 To dgvSelectedModels.Rows.Count - 1
                    For indexcell As Integer = 0 To 2
                        RowValues(indexcell) = Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(index).DataItem).Values(indexcell).ToString())
                    Next
                Next
                For index As Integer = 3 To i - 2
                    RowValues(index) = copyrow.Cells(index).Text
                Next
                dtable.Rows.Add(RowValues)
            Next

            dtable.AcceptChanges()

            Session("SelectedModels") = dtable
            PopulateGrid(dgvEditModels, dtable)
            ToggleEdition(True)
        Else
            MsgBox("Se necesitan seleccionar uno o más modelos para editar", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Aviso")
        End If
    End Sub

    ' Método para exportar la tabla a una hoja de excel
    Protected Sub cmdExportExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExportExcel.Click
        Dim objBooks As Excel.Workbooks
        Dim objSheets As Excel.Sheets
        Dim objSheet As Excel._Worksheet
        Dim range As Excel.Range

        ' Create a new instance of Excel and start a new workbook.
        objApp = New Excel.Application()
        objBooks = objApp.Workbooks
        objBook = objBooks.Add
        objSheets = objBook.Worksheets
        objSheet = objSheets(1)

        'Get the range where the starting cell has the address
        'm_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
        range = objSheet.Range("A1", Reflection.Missing.Value)
        range = range.Resize(dgvModelos.Rows.Count + 1, dgvModelos.Columns.Count)

        If True Then 'Me.FillWithStrings.Checked = False) Then
            'Create an array.
            Dim saRet(dgvModelos.Rows.Count, dgvModelos.Columns.Count) As String

            'Fill the array.
            Dim iRow As Long
            Dim iCol As Long

            For iCol = 0 To dgvModelos.Columns.Count - 1

                saRet(0, iCol) = dgvModelos.Columns(iCol).HeaderText
            Next iCol


            For iRow = 0 To dgvModelos.Rows.Count - 1
                For iCol = 0 To dgvModelos.Columns.Count - 1

                    'Put a counter in the cell.
                    saRet(iRow + 1, iCol) = dgvModelos.Rows(iRow).Cells(iCol).Text
                Next iCol
            Next iRow

            'Set the range value to the array.
            range.Value = saRet

        Else
            'Create an array.
            Dim saRet(5, 5) As String

            'Fill the array.
            Dim iRow As Long
            Dim iCol As Long
            For iRow = 0 To 5
                For iCol = 0 To 5

                    'Put the row and column address in the cell.
                    saRet(iRow, iCol) = iRow.ToString() + "|" + iCol.ToString()
                Next iCol
            Next iRow

            'Set the range value to the array.
            range.Value = saRet
        End If

        'Return control of Excel to the user.
        objApp.Visible = True
        objApp.UserControl = True

        'Clean up a little.
        range = Nothing
        objSheet = Nothing
        objSheets = Nothing
        objBooks = Nothing
    End Sub










    Protected Sub dgvModelos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvModelos.RowCommand

        Select Case e.CommandName
            Case "Select"

                Dim data As Data.DataTable = Session("Test")
                'Dim datarow As DataRow = data.Rows(e.CommandSource)

                Dim rowkey As DataKeyArray = dgvModelos.DataKeys
                Dim rowkeySelected As DataKeyArray = dgvSelectedModels.DataKeys
                Dim indexkey As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id As Guid = Guid.Parse(rowkey(indexkey).Values(0).ToString())
                Dim idHeader As Guid = Guid.Parse(rowkey(indexkey).Values(1).ToString())
                Dim idUnit As Guid = Guid.Parse(rowkey(indexkey).Values(2).ToString())
                'Dim id As Guid = Guid.Parse(rowkey(indexkey).Value.ToString)
                'Dim idheader As Guid = Guid.Parse(rowkey(indexkey).Values.Keys

                Dim indexevent As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = dgvModelos.Rows.Item(indexevent)

                For Each checkrow In dgvSelectedModels.Rows
                    If row.Cells(3).Text = checkrow.Cells(3).Text Then
                        MsgBox("Este modelo ya esta seleccionado", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Modelo ya seleccionado")
                        Exit Sub
                    End If
                Next

                Dim dtable As Data.DataTable = GenerateTable()

                Dim i As Integer = row.Cells.Count

                Dim RowValues As Object() = {"", "", "", "", "", "", "", "", ""}

                For index As Integer = 0 To 2
                    If index = 0 Then
                        RowValues(index) = id
                    End If

                    If index = 1 Then
                        RowValues(index) = idHeader
                    End If

                    If index = 2 Then
                        RowValues(index) = idUnit
                    End If
                Next

                For index As Integer = 3 To i - 2
                    RowValues(index) = row.Cells(index).Text
                Next

                Dim dRow As DataRow
                dRow = dtable.Rows.Add(RowValues)

                For Each copyrow In dgvSelectedModels.Rows
                    'creo que los for se pueden sacar del for each pero lo dejo como estaba funcional
                    For index As Integer = 0 To dgvSelectedModels.Rows.Count - 1
                        For indexcell As Integer = 0 To 2
                            RowValues(indexcell) = Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(index).DataItem).Values(indexcell).ToString())
                        Next
                    Next
                    For index As Integer = 3 To i - 2
                        RowValues(index) = copyrow.Cells(index).Text
                    Next
                    dtable.Rows.Add(RowValues)
                Next

                dtable.AcceptChanges()

                Session("SelectedModels") = dtable
                PopulateGrid(dgvSelectedModels, dtable)
                ToggleSelected()
        End Select
    End Sub





    Protected Sub dgvSelectedModels_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvSelectedModels.RowDeleting
        Dim dt As Data.DataTable = Session("SelectedModels")
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))

        Session("SelectedModels") = dt
        PopulateGrid(dgvSelectedModels, dt)
        ToggleSelected()
    End Sub

    Protected Sub cmdResetSelected_Click(sender As Object, e As EventArgs) Handles cmdResetSelected.Click
        Dim confirmed As Integer = MsgBox("Se reiniciará el proceso de selección de modelos. ¿Desea deseleccionar todo y empezar de nuevo?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")

        If confirmed = MsgBoxResult.Yes Then
            Session("SelectedModels") = GenerateTable()
            PopulateGrid(dgvSelectedModels, Session("SelectedModels"))
            ToggleSelected()
        Else

        End If
    End Sub




    Protected Sub cmdCancelEdit_Click(sender As Object, e As EventArgs) Handles cmdCancelEdit.Click
        Dim confirmed As Integer = MsgBox("Se reiniciará el proceso de selección de modelos. ¿Desea deseleccionar todo y empezar de nuevo?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")

        If confirmed = MsgBoxResult.Yes Then
            Session("SelectedModels") = GenerateTable()
            PopulateGrid(dgvSelectedModels, Session("SelectedModels"))
            ToggleEdition(False)
            ToggleSelected()
        Else

        End If
    End Sub

    Protected Sub dgvEditModels_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvEditModels.RowEditing
        dgvEditModels.EditIndex = e.NewEditIndex

        Dim dt As Data.DataTable = Session("SelectedModels")
        dgvEditModels.DataSource = dt
        dgvEditModels.DataBind()

        Dim unit = dt.Rows(e.NewEditIndex).Item(5).ToString
        Dim units = catUnits.SelectAll("", False).AsDataView

        Dim ddlEdit As DropDownList = DirectCast(dgvEditModels.Rows(e.NewEditIndex).FindControl("txtEditUnit"), DropDownList)
        ddlEdit.DataSource = units
        ddlEdit.DataTextField = "Unit"
        ddlEdit.DataValueField = "IdUnit"
        ddlEdit.DataBind()
        ddlEdit.Items.FindByText(unit).Selected = True
    End Sub

    Protected Sub dgvEditModels_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvEditModels.RowCancelingEdit
        dgvEditModels.EditIndex = -1
        dgvEditModels.DataSource = DirectCast(Session("SelectedModels"), Data.DataTable)
        dgvEditModels.DataBind()
    End Sub

    Protected Sub dgvEditModels_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvEditModels.RowUpdating
        Dim dt As Data.DataTable = DirectCast(Session("SelectedModels"), Data.DataTable)
        Dim row As DataRow = dt.Rows(e.RowIndex)

        Dim txtEditModel As WebControls.TextBox = DirectCast(dgvEditModels.Rows(e.RowIndex).FindControl("txtEditModel"), WebControls.TextBox)
        Dim txtEditLifespan As WebControls.TextBox = DirectCast(dgvEditModels.Rows(e.RowIndex).FindControl("txtEditLifespan"), WebControls.TextBox)
        Dim ddlEditUnit As DropDownList = DirectCast(dgvEditModels.Rows(e.RowIndex).FindControl("txtEditUnit"), DropDownList)

        Try
            lblMessage.Text = ""

            If (txtEditModel.Text = "") Then
                lblMessage.Text = "Favor de escribir un nombre para el Modelo"
                Return
            End If

            If (txtEditLifespan.Text = "") Then
                lblMessage.Text = "Favor de escribir un valor para la Vida Útil"
                Return
            End If

            If (ddlEditUnit.Text = "") Then
                lblMessage.Text = "Favor de seleccionar una Unidad"
                Return
            End If

            If (Not Regex.IsMatch(txtEditLifespan.Text, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numérico para la Vida Útil"
                Return
            End If

            If (modelChanges.AlreadyExistModelChange(Guid.Parse(row("IdModelsChanges")), txtEditModel.Text)) Then
                lblMessage.Text = "No es posble ingresar el Modelo debido a que ya existe un Modelo activo registrado con ese nombre: [" + txtEditModel.Text + "]"
                Return
            End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para el valor de la Unidad"

        Catch ex As OverflowException
            lblMessage.Text = "El valor de la Unidad es demasiado grande para guardar"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try

        row("IdCatUnits") = ddlEditUnit.SelectedValue
        row("Model") = txtEditModel.Text
        row("Lifespan") = txtEditLifespan.Text
        row("Unit") = ddlEditUnit.SelectedItem

        dgvEditModels.EditIndex = -1

        Session("SelectedModels") = dt
        dgvEditModels.DataSource = dt
        dgvEditModels.DataBind()
    End Sub

    Protected Sub dgvEditModels_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvEditModels.RowDeleting
        Dim dt As Data.DataTable = DirectCast(Session("SelectedModels"), Data.DataTable)

        dt.Rows(e.RowIndex).Delete()

        Session("SelectedModels") = dt
        dgvEditModels.DataSource = dt
        dgvEditModels.DataBind()
    End Sub



    'Protected Sub dgvEditModels_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvEditModels.RowEditing
    '    dgvEditModels.EditIndex = e.NewEditIndex
    '    PopulateEditGrid()
    'End Sub

    'Protected Sub dgvEditModels_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvEditModels.RowUpdating

    'End Sub


    'Protected Sub dgvEditModels_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles dgvEditModels.RowUpdated

    'End Sub

    'Protected Sub dgvEditModels_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvEditModels.RowCancelingEdit
    '    dgvEditModels.EditIndex = -1
    '    dgvEditModels.DataSource = dgvEditModels
    '    dgvEditModels.DataBind()
    'End Sub




End Class