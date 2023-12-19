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
    Dim userPlaceholder As String

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)
        lblSelectedModels.Visible = False
        If Not Page.IsPostBack Then
            Dim catUnits As CatUnits = New CatUnits()
            ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
            ddlUnit.DataTextField = "Unit"
            ddlUnit.DataValueField = "IdUnit"
            ddlUnit.DataBind()
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedUnitID())
        End If
    End Sub

    ' Llenado de tablas
    Protected Sub PopulateGrid(dgv As GridView, dt As System.Data.DataTable)
        dgv.DataSource = dt
        dgv.DataBind()
    End Sub

    Protected Sub OnChangeIsChecked(sender As Object, e As EventArgs)

    End Sub

    Protected Sub dgvModelos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvModelos.SelectedIndexChanged

    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        If txtModel.Text = "" And txtLifeSpan.Text = "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 3))
        End If
        If txtModel.Text <> "" And txtLifeSpan.Text = "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, "", Guid.Parse(ddlUnit.Text), 5))
        End If
        If txtModel.Text = "" And txtLifeSpan.Text <> "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter("", txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 6))
        End If
        If txtModel.Text <> "" And txtLifeSpan.Text <> "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 7))
        End If



    End Sub

    Protected Sub cmdExportExcel_Click(ByVal sender As System.Object,
  ByVal e As System.EventArgs) Handles cmdExportExcel.Click

        'PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter())

        'Dim app As Microsoft.Office.Interop.Excel._Application = New Microsoft.Office.Interop.Excel.Application()

        'Dim workbook As Microsoft.Office.Interop.Excel._Workbook = app.Workbooks.Add(Type.Missing)

        'Dim worksheet As Microsoft.Office.Interop.Excel._Worksheet = Nothing

        'app.Visible = True

        'worksheet = workbook.Sheets("Sheet1")
        'worksheet = workbook.ActiveSheet

        'Dim Today As DateTime = DateTime.Today
        'worksheet.Name = "Modelos_FE" + Today.ToString("dd-MM-yyyy")

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

        If (Me.FillWithStrings.Checked = False) Then
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






        ''''''''''''''''''''''''''''''''''''

    End Sub

    Protected Sub dgvModelos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvModelos.RowCommand
        ' Grey out expired training courses
        'Dim rowa As DataKeyArray = dgvModelos.DataKeys


        lblSelectedModels.Visible = True

        Dim indexevent As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = dgvModelos.Rows.Item(indexevent)



        For Each checkrow In dgvSelectedModels.Rows

            If row.Cells(3).Text = checkrow.Cells(3).Text Then
                MsgBox("Este modelo ya esta seleccionado", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Modelo ya seleccionado")
                Exit Sub
            End If

        Next




        'Create datatable and columns
        Dim dtable As New System.Data.DataTable

        dtable.Columns.Add(New DataColumn("IdModelsChanges"))
        dtable.Columns.Add(New DataColumn("IdModelsChangesHeader"))
        dtable.Columns.Add(New DataColumn("IdCatUnits"))
        dtable.Columns.Add(New DataColumn("Model"))
        dtable.Columns.Add(New DataColumn("Lifespan"))
        dtable.Columns.Add(New DataColumn("Unit"))
        dtable.Columns.Add(New DataColumn("LastUser"))
        dtable.Columns.Add(New DataColumn("ApproverUser"))
        dtable.Columns.Add(New DataColumn("ApprovedOn"))



        'Create counter to prevent out of bounds exception
        Dim i As Integer = Row.Cells.Count

        'Create object for RowValues
        Dim RowValues As Object() = {"", "", "", "", "", "", "", "", ""}

        'Fill row values appropriately
        For index As Integer = 0 To i - 2
            RowValues(index) = row.Cells(index).Text
        Next

        'create new data row
        Dim dRow As DataRow
        dRow = dtable.Rows.Add(RowValues)

        For Each copyrow In dgvSelectedModels.Rows

            For index As Integer = 0 To i - 2

                RowValues(index) = copyrow.Cells(index).Text
            Next
            dtable.Rows.Add(RowValues)
        Next


        dtable.AcceptChanges()

        'now bind datatable to gridview... 
        dgvSelectedModels.DataSource = dtable
        dgvSelectedModels.DataBind()



    End Sub


    'esto aun no jala
    Protected Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click

        dgvEditModels.DataSource = CopySameTable(1)
        dgvEditModels.DataBind()

        dgvModelos.Visible = False
        dgvSelectedModels.Visible = False
        lblModels.Visible = False
    End Sub

    Protected Sub PopulateEditGrid()
        'Dim dt As CatConfiguracionCalculoDirecto = New CatConfiguracionCalculoDirecto
        dgvEditModels.DataSource = CopySameTable(2)
        dgvEditModels.DataBind()
    End Sub

    Protected Sub dgvEditModels_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvEditModels.RowEditing
        dgvEditModels.EditIndex = e.NewEditIndex
        PopulateEditGrid()
    End Sub

    Protected Function CopySameTable(optionCopy As Integer)
        'Dim rowa As DataKeyArray = dgvModelos.DataKeys
        'Dim indexevent As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = dgvSelectedModels.Rows.Item(0)


        Dim dtable As New System.Data.DataTable

        dtable.Columns.Add(New DataColumn("IdModelsChanges"))
        dtable.Columns.Add(New DataColumn("IdModelsChangesHeader"))
        dtable.Columns.Add(New DataColumn("IdCatUnits"))
        dtable.Columns.Add(New DataColumn("Model"))
        dtable.Columns.Add(New DataColumn("Lifespan"))
        dtable.Columns.Add(New DataColumn("Unit"))
        dtable.Columns.Add(New DataColumn("LastUser"))
        dtable.Columns.Add(New DataColumn("ApproverUser"))
        dtable.Columns.Add(New DataColumn("ApprovedOn"))



        'Create counter to prevent out of bounds exception
        Dim i As Integer = row.Cells.Count

        'Create object for RowValues
        Dim RowValues As Object() = {"", "", "", "", "", "", "", "", ""}

        'Fill row values appropriately
        'For index As Integer = 0 To i - 2
        'RowValues(index) = row.Cells(index).Text
        'Next

        'create New Data row
        'Dim dRow As DataRow
        'dRow = dtable.Rows.Add(RowValues)
        If optionCopy = 1 Then
            For Each copyrow In dgvSelectedModels.Rows

                For index As Integer = 0 To i - 1

                    RowValues(index) = copyrow.Cells(index).Text
                Next
                dtable.Rows.Add(RowValues)
            Next


            dtable.AcceptChanges()
            Return dtable

        End If
        If optionCopy = 2 Then
            For Each copyrow In dgvEditModels.Rows

                For index As Integer = 0 To i - 1

                    RowValues(index) = copyrow.Cells(index).Text
                Next
                dtable.Rows.Add(RowValues)
            Next
            dtable.AcceptChanges()
            Return dtable

        End If
        If optionCopy = 3 Then
            For Each copyrow In dgvEditModels.Rows

                For index As Integer = 0 To i - 1

                    RowValues(index) = copyrow.Cells(index).Text
                Next
                dtable.Rows.Add(RowValues)
            Next
            dtable.AcceptChanges()
            Return dtable

        End If




    End Function

    Protected Sub dgvEditModels_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvEditModels.RowUpdating
        Try
            'lblMessage.Text = ""

            Dim catUnits As CatUnits = New CatUnits()
            Dim row As GridViewRow = dgvEditModels.Rows(e.RowIndex)
            'Dim names As DataKeyArray = dgvEditModels.DataKeys()
            'Dim idUnit As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
            Dim strModel As String = CType((row.Cells(3).Controls(0)), WebControls.TextBox).Text.Trim.ToUpper()
            Dim strLifespan As String = CType((row.Cells(4).Controls(0)), WebControls.TextBox).Text.Trim.ToUpper()
            Dim strUnit As String = CType((row.Cells(5).Controls(0)), WebControls.TextBox).Text.Trim.ToUpper()

            If (strModel = "") Then
                lblMessage.Text = "Favor de escribir un nombre para el Modelo"
                Return
            End If

            If (strLifespan = "") Then
                lblMessage.Text = "Favor de escribir un valor para la Vida Util"
                Return
            End If

            If (strUnit = "") Then
                lblMessage.Text = "Favor de escribir un nombre para la Unidad"
                Return
            End If

            If (Not Regex.IsMatch(strLifespan, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numerico"
                Return
            End If
            'Dim find As String = "IdUnit ='" + idUnit.ToString + "'"
            'Dim temp As Integer = catUnits.SelectAll("", False).Select(find).Count
            'If (temp = 0) Then
            'If (catUnits.AlreadyExistUnit(idUnit, strUnit)) Then
            'lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
            'Else
            'catUnits.Insert(strUnit, strLifespan, True, "Admin")
            'MsgBox("Se agregó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
            'catUnits = Nothing

            'dgvEditModels.EditIndex = -1
            'CopySameTable(2)
            'PopulateGrid("", False)
            'End If
            'Else
            'If (catUnits.AlreadyExistUnit(idUnit, strUnit)) Then
            'lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
            'Else
            'catUnits.Update(idUnit, strUnit, strLifespan, True, "Admin")
            dgvEditModels.EditIndex = -1
            MsgBox("Se actualizó el registro con éxito", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Éxito")
            catUnits = Nothing
            e.Cancel = True

            'PopulateGrid(dgvEditModels, modelChanges.SelectByIdModelsChangesHeaderApprovedUnitID())

            CopySameTable(2)

            'PopulateGrid("", False)
            'End If
            'End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para el valor de la Unidad"

        Catch ex As OverflowException
            lblMessage.Text = "El valor de la Unidad es demasiado grande para guardar"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try
    End Sub

    Protected Sub UpdateDgvAfterUpdate()

    End Sub


    Protected Sub dgvEditModels_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles dgvEditModels.RowUpdated

    End Sub

    Protected Sub dgvEditModels_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvEditModels.RowCancelingEdit
        dgvEditModels.EditIndex = -1
        dgvEditModels.DataSource = dgvEditModels
        dgvEditModels.DataBind()
    End Sub
End Class