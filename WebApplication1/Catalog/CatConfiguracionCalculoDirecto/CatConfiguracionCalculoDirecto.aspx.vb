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
    Dim modelsChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelsChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim catUnits As CatUnits = New CatUnits()
    Dim userPlaceholder As String

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)

        If Not Page.IsPostBack Then
            Session("SelectedModels") = GenerateTable()
            Session("Models") = GenerateTable()
            PopulateDropDown()
            PopulateGridModals(dgvModelos, modelsChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, Guid.Empty))
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

    ' Llenado de tabla de Modelos aprobados, y verificar si está seleccionado
    Protected Sub PopulateGridModals(dgv As GridView, dt As Data.DataTable)
        Dim dtSelected As Data.DataTable = Session("SelectedModels")

        For Each row As DataRow In dt.Rows
            Dim idPrincipal As String = Convert.ToString(row("IdModelsChanges"))
            Dim filaSeleccionada As DataRow() = dtSelected.Select("IdModelsChanges = '" & idPrincipal & "'")

            If filaSeleccionada.Length > 0 Then
                row("IsChecked") = True
            Else
                row("IsChecked") = False
            End If
        Next

        Session("Models") = dt
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

        PopulateGridModals(dgvModelos, modelsChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, idCatUnits))
    End Sub

    ' Botón para ir a editar
    Protected Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        Dim dataTableSelected As Data.DataTable = Session("SelectedModels")
        If dataTableSelected.Rows.Count > 0 Then
            'dgvSelectedModels.Rows.Count > 0 Then

            Dim rowkeySelected As DataKeyArray = dgvSelectedModels.DataKeys

            Dim dtable As Data.DataTable = GenerateTable()

            Dim i As Integer = dgvSelectedModels.Columns.Count
            Dim RowValues As Object() = {"", "", "", "", "", "", "", "", ""}

            If dgvSelectedModels.Rows.Count > 0 Then
                For index As Integer = 0 To dataTableSelected.Rows.Count - 1
                    'dgvSelectedModels.Rows.Count -1
                    For indexcell As Integer = 0 To 2
                        RowValues(indexcell) = dataTableSelected.Rows(index).Item(indexcell).ToString()
                        'Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(index).DataItem).Values(indexcell).ToString())
                    Next
                    For indexcell As Integer = 3 To i - 2
                        RowValues(indexcell) = dataTableSelected.Rows(index).Item(indexcell).ToString()
                        'dgvSelectedModels.Rows(index).Cells(indexcell).Text
                    Next
                    dtable.Rows.Add(RowValues)
                Next
            End If

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


        Dim dtModelos As Data.DataTable = Session("Models")


        For rowcount As Integer = 0 To dgvModelos.Rows.Count - 1
            Dim idSelected As Guid = Guid.Parse(dt.Rows(e.RowIndex).Item(0).ToString())
            'Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(rowcount).DataItem).Values(0).ToString())
            If idSelected = dtModelos.Rows(rowcount).Item(0) Then
                'Dim dt As Data.DataTable = Session("SelectedModels")
                dtModelos.Rows(rowcount).Item(9) = False

                dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
                Session("SelectedModels") = dt
                Session("Models") = dtModelos
                PopulateGridModals(dgvModelos, dtModelos)
                PopulateGrid(dgvSelectedModels, dt)
                ToggleSelected()
                Exit Sub
            End If
        Next



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

            'puse esto para que se reiniciaran los checks
            'Session("Models") = modelsChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, Guid.Empty)

            Dim idCatUnits As Guid

            If ddlUnit.SelectedValue = "" Then
                idCatUnits = Guid.Empty
            Else
                idCatUnits = Guid.Parse(ddlUnit.SelectedValue)
            End If

            PopulateGridModals(dgvModelos, modelsChanges.SelectByIdModelsChangesApproved(txtModel.Text, txtLifeSpan.Text, idCatUnits))
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

            If (modelsChanges.AlreadyExistModelChange(Guid.Parse(row("IdModelsChanges")), txtEditModel.Text)) Then
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
        Dim confirmed As Integer

        If dt.Rows.Count <= 1 Then
            confirmed = MsgBox("Remover el últmo Modelo hará que se cancele el proceso de Aprobación. ¿Está seguro de continuar?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")
            If confirmed = MsgBoxResult.Yes Then
                Session("SelectedModels") = GenerateTable()
                PopulateGrid(dgvSelectedModels, Session("SelectedModels"))
                ToggleSelected()
            Else

            End If
        Else
            confirmed = MsgBox("¿Está seguro de remover este Modelo de la lista para Aprobar?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")
            If confirmed = MsgBoxResult.Yes Then

                dt.Rows.RemoveAt(e.RowIndex)

                Session("SelectedModels") = dt
                dgvEditModels.DataSource = dt
                dgvEditModels.DataBind()
            Else

            End If
        End If
    End Sub










    Protected Sub OnChangeIsChecked(sender As Object, e As EventArgs)
        Dim checkBox As WebControls.CheckBox = TryCast(sender, WebControls.CheckBox)

        If checkBox IsNot Nothing Then
            Dim row As GridViewRow = DirectCast(checkBox.Parent.Parent, GridViewRow)
            Dim dataTableModels As Data.DataTable = Session("Models")

            Dim id As Guid = Guid.Parse(dataTableModels.Rows(row.RowIndex).Item(0).ToString())
            'Guid.Parse(dgvModelos.DataKeys(row.RowIndex).Value.ToString()) antiguo y funciona
            Dim isChecked As Boolean = checkBox.Checked

            Dim rowkey As DataKeyArray = dgvModelos.DataKeys
            Dim rowkeySelected As DataKeyArray = dgvSelectedModels.DataKeys
            'Dim indexkey As Integer = Convert.ToInt32(e.CommandArgument)
            'Dim id As Guid = Guid.Parse(rowkey(indexkey).Values(0).ToString())
            Dim idHeader As Guid = Guid.Parse(dataTableModels.Rows(row.RowIndex).Item(1).ToString())
            'Guid.Parse(rowkey(row.RowIndex).Values(1).ToString())
            Dim idUnit As Guid = Guid.Parse(dataTableModels.Rows(row.RowIndex).Item(2).ToString())
            'Guid.Parse(rowkey(row.RowIndex).Values(2).ToString())

            Dim datacopy As Data.DataTable = Session("SelectedModels")
            'Dim id As Guid = Guid.Parse(rowkey(indexkey).Value.ToString)
            'Dim idheader As Guid = Guid.Parse(rowkey(indexkey).Values.Keys

            'Dim indexevent As Integer = Convert.ToInt32(e.CommandArgument)
            'Dim row As GridViewRow = dgvModelos.Rows.Item(indexevent)
            If checkBox.Checked = False Then

                For rowcount As Integer = 0 To dgvSelectedModels.Rows.Count - 1
                    Dim idSelected As Guid = Guid.Parse(datacopy.Rows(rowcount).Item(0).ToString())
                    'Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(rowcount).DataItem).Values(0).ToString())
                    If idSelected = id Then
                        'Dim dt As Data.DataTable = Session("SelectedModels")
                        datacopy.Rows.Remove(datacopy.Rows.Item(rowcount))

                        Session("SelectedModels") = datacopy
                        PopulateGrid(dgvSelectedModels, datacopy)
                        ToggleSelected()
                        Exit Sub
                    End If
                Next
            End If
            If checkBox.Checked = True Then
                dataTableModels.Rows(row.RowIndex).Item(9) = True
            End If

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


            If dgvSelectedModels.Rows.Count > 0 Then
                For index As Integer = 0 To datacopy.Rows.Count - 1
                    'dgvSelectedModels.Rows.Count -1
                    For indexcell As Integer = 0 To 2
                        RowValues(indexcell) = datacopy.Rows(index).Item(indexcell).ToString()
                        'Guid.Parse(rowkeySelected(dgvSelectedModels.Rows(index).DataItem).Values(indexcell).ToString())
                    Next
                    For indexcell As Integer = 3 To i - 2
                        RowValues(indexcell) = datacopy.Rows(index).Item(indexcell).ToString()
                        'dgvSelectedModels.Rows(index).Cells(indexcell).Text
                    Next
                    dtable.Rows.Add(RowValues)
                Next
            End If


            dtable.AcceptChanges()
            Session("Models") = dataTableModels
            Session("SelectedModels") = dtable
            PopulateGrid(dgvSelectedModels, dtable)
            ToggleSelected()
        End If
    End Sub










    Protected Sub lbCancel_Click(sender As Object, e As EventArgs) Handles lbCancel.Click
        CleanModalFields(True)
        ApproveModal.Hide()
    End Sub

    Protected Sub lbAccept_Click(sender As Object, e As EventArgs) Handles lbAccept.Click
        CleanModalFields(False)
        EnableTextBoxes(False)

        Dim canInsert As Boolean = True

        'Validaciones de los campos
        canInsert = ValidateTextBox(txtApprover, lblModalMessage, "Favor de buscar y seleccionar a un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtUsernameApprover, lblModalMessage, "Favor de buscar y seleccionar a un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtMailApprover, lblModalMessage, "Favor de buscar y seleccionar a un Aprobador", canInsert)

        canInsert = ValidateTextBox(txtApprover, lblApproverError, "Buscar un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
        canInsert = ValidateTextBox(txtPassword, lblPasswordError, "Llenar el campo de Contraseña", canInsert)
        canInsert = ValidateTextBox(txtApproveMessage, lblApproveMessageError, "Llenar el campo de Mensaje", canInsert)

        'Validaciones extra por si acaso
        If (canInsert) Then
            Dim dt As System.Data.DataTable = Session("SelectedModels")
            Dim foundRepeated As Boolean

            Dim originUser As String
            Dim originName As String
            Dim originEmail As String

            Dim approverUser As String
            Dim approverName As String
            Dim approverEmail As String

            Dim originComment As String = txtApproveMessage.Text
            Dim approvalStatus As String = "Pendiente"
            Dim isActive As Boolean = True

            Dim idModelsChanges As Guid
            Dim idModelsChangesHeader As Guid
            Dim idCatUnits As Guid
            Dim model As String
            Dim lifeSpan As Integer
            Dim modelChangeStatus As String = "Pendiente"

            Dim strApproverName As String = txtApprover.Text

            ' Asignación del usuario aprobador
            If Not txtUsernameApprover.Text.IsEmpty Then
                approverUser = txtUsernameApprover.Text
            Else
                lblModalMessage.Text = "No se encontro al Usuario Aprobador"
                EnableTextBoxes(True)
                ApproveModal.Show()
                Return
            End If


            ' Validaciones del usuario aprobador
            If (Security.UserAD.GetUserExists(approverUser, "")) Then
                approverName = txtApprover.Text
                approverEmail = txtMailApprover.Text
            Else
                lblModalMessage.Text = "No se encontro al Usuario Aprobador"
                EnableTextBoxes(True)
                ApproveModal.Show()
                Return
            End If


            ' Validacion de usuario originador
            Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile)
            Dim actualUser = m_Profile.UserName.Split("\")(1)
            If (Not txtUser.Text.ToLower() = actualUser) Then
                lblModalMessage.Text = "Porfavor ingrese el usuario de su sesión"
                EnableTextBoxes(True)
                ApproveModal.Show()
                Return
            End If

            ' Validacion de usuario originador y aprobador
            If (txtUsernameApprover.Text = actualUser) Then
                lblModalMessage.Text = "El usuario aprobador no puede ser el mismo que el originador"
                EnableTextBoxes(True)
                ApproveModal.Show()
                Return
            End If

            ' Validación del propio usuario
            If (Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT")) Then
                originUser = txtUser.Text
                originName = Security.UserAD.GetUserName(originUser)
                originEmail = Security.UserAD.GetUserEmail(originUser)
            Else
                lblModalMessage.Text = "Usuario o contraseña incorrectos"
                EnableTextBoxes(True)
                ApproveModal.Show()
                Return
            End If

            ' Validación de registros ya existentes
            For Each row As DataRow In dt.Rows
                If (modelsChanges.AlreadyExistModelChange(Guid.Empty, row("Model"))) Then
                    foundRepeated = True
                End If
            Next row


            If (foundRepeated) Then
                lblModalMessage.Text = "Se ha detectado que uno o varios modelos seleccionados fueron cargados durante el proceso de aprobación. Favor de rectificar."
                EnableTextBoxes(True)
                ApproveModal.Show()
            Else
                idModelsChangesHeader = modelsChangesHeader.Insert(originUser, originName, originEmail, originComment, approverUser, approverName, approverEmail, approvalStatus, isActive, originUser)
                For Each row As DataRow In dt.Rows
                    idModelsChanges = Guid.Parse(row("IdModelsChanges"))
                    idCatUnits = Guid.Parse(row("IdCatUnits"))
                    model = row("Model")
                    lifeSpan = row("Lifespan")
                    modelsChanges.UpdateInactivate(idModelsChanges, idModelsChangesHeader, originUser)
                    modelsChanges.Insert(idModelsChangesHeader, idCatUnits, model, lifeSpan, modelChangeStatus, originUser, originName, originEmail, isActive, originUser)
                Next row

                ApproveModal.Hide()

                MsgBox("Se ha completado exitósamente el registro de los cambios", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")


                Dim dataMail As New ConstructInfo With {
                                .EmailType = "CambiosPendientes",
                                .UserName = originUser,
                                .Comment = txtApproveMessage.Text.Trim,
                                .Link = "<a href=>Fecha De Expiración</a>"
                                }
                Dim email As New ModuloGeneralEmail

                If email.ConstructEmail(dataMail) Then
                    MsgBox("Se ha enviado un correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
                Else
                    MsgBox("Ha ocurrido un error al mandar correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
                End If

                CleanModalFields(True)
                EnableTextBoxes(True)

            End If
        Else
            EnableTextBoxes(True)
            ApproveModal.Show()
        End If
    End Sub

    Protected Sub CleanModalFields(cleanTextBoxes As Boolean)
        lblApproverError.Text = ""
        lblUserError.Text = ""
        lblPasswordError.Text = ""
        lblApproveMessageError.Text = ""
        lblModalMessage.Text = ""

        If (cleanTextBoxes) Then
            txtApprover.Text = ""
            txtUsernameApprover.Text = ""
            txtMailApprover.Text = ""
            txtUser.Text = ""
            txtPassword.Text = ""
            txtApproveMessage.Text = ""
        End If
    End Sub

    Protected Function ValidateTextBox(txt As WebControls.TextBox, lbl As WebControls.Label, errorMessage As String, canInsert As Boolean)
        If (txt.Text = "") Then
            lbl.Text = errorMessage
            Return False
        Else
            If (canInsert) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Protected Sub EnableTextBoxes(enable As Boolean)
        txtUser.Enabled = enable
        txtPassword.Enabled = enable
    End Sub

    Protected Sub cmdApproveEdit_Click(sender As Object, e As EventArgs) Handles cmdApproveEdit.Click
        Dim dt As Data.DataTable = Session("SelectedModels")
        Dim foundRepeated As Boolean = False
        Dim listRepeated As String = ""

        For Each row As DataRow In dt.Rows
            If (modelsChanges.AlreadyExistModelChange(Guid.Parse(row("IdModelsChanges")), row("Model"))) Then
                foundRepeated = True
                listRepeated += "[" + row("Model") + "], "
            End If
        Next row

        If (foundRepeated) Then
            listRepeated = listRepeated.Substring(0, listRepeated.Length - 1)
            lblMessage.Text = "Se ha detectado que alguno de los modelos ha sido ingresado al sistema durante el flujo de configuración: " + listRepeated + ". Favor de rectificar los datos"
        Else
            ApproveModal.Show()
        End If
    End Sub
End Class