Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports AjaxControlToolkit

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("DataTable") = Initdt()
            Dim catUnits As CatUnits = New CatUnits()
            ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
            ddlUnit.DataTextField = "Unit"
            ddlUnit.DataValueField = "IdUnit"
            ddlUnit.DataBind()
        End If

        EnableButtons()
    End Sub

    Protected Function Initdt()
        Dim data = New DataTable("Result")
        data.Columns.Add("Modelo", GetType(String))
        data.Columns.Add("VidaUtil", GetType(String))
        data.Columns.Add("Unidad", GetType(String))
        data.Columns.Add("Usuario", GetType(String))
        data.Columns.Add("IdUnidad", GetType(String))
        Return data
    End Function

    Private Sub CleanAddData()
        lblMessage.Text = ""
        txtModel.Text = ""
        txtLifeSpan.Text = ""
    End Sub

    Private Sub CleanTable()
        Session("DataTable") = Initdt()
        gvModelos.DataSource = Session("DataTable")
        gvModelos.DataBind()
    End Sub

    Private Sub EnableButtons()
        Dim dt As DataTable = Session("DataTable")

        If dt.Rows.Count > 0 Then
            cmdCancelChange.Enabled = True
            cmdOpenApprove.Enabled = True
        Else
            cmdCancelChange.Enabled = False
            cmdOpenApprove.Enabled = False
        End If
    End Sub

    Protected Sub AddModel_Click(sender As Object, e As EventArgs) Handles cmdModel.Click
        Try
            lblMessage.Text = ""

            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim dt As DataTable = Session("DataTable")
            Dim foundRepeated As Boolean
            Dim strModel As String = txtModel.Text.Trim.ToUpper
            Dim strLifeSpan As String = txtLifeSpan.Text.Trim.ToUpper
            Dim strUnit As String = ddlUnit.SelectedItem.Text.Trim.ToUpper
            Dim idUnit As String = ddlUnit.SelectedValue

            If (strModel = "") Then
                lblMessage.Text = "Favor de escribir el nombre del Modelo"
                Return
            End If

            If (Not Regex.IsMatch(strLifeSpan, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numerico para la Vida Útil"
                Return
            End If

            If (strLifeSpan = "") Then
                lblMessage.Text = "Favor de seleccionar una Unidad para el Modelo"
                Return
            End If

            For Each row As DataRow In dt.Rows
                If (strModel = row("Modelo")) Then
                    foundRepeated = True
                End If
            Next row

            If (foundRepeated) Then
                lblMessage.Text = "El Modelo que intenta ingresar ya se encuentra listado"
            Else
                If (modelsChange.AlreadyExistModelChange(Guid.Empty, strModel)) Then
                    lblMessage.Text = "No es posble ingresar el Modelo debido a que ya existe un Modelo activo registrado con ese nombre: [" + strModel + "]"
                Else
                    Dim row As DataRow = dt.NewRow
                    row("Modelo") = strModel
                    row("VidaUtil") = strLifeSpan
                    row("Unidad") = strUnit
                    row("Usuario") = "Origin User"        'Agregar función para obtener al usuario
                    row("IdUnidad") = idUnit

                    dt.Rows.Add(row)

                    gvModelos.DataSource = dt
                    gvModelos.DataBind()

                    Session("DataTable") = dt

                    EnableButtons()
                    CleanAddData()
                End If
            End If


        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar agregar los datos: " + ex.Message
        End Try

    End Sub

    Protected Sub gvModelos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvModelos.RowDeleting
        Dim dt As DataTable = Session("DataTable")
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
        gvModelos.DataSource = dt
        gvModelos.DataBind()

        Session("DataTable") = dt

        EnableButtons()
    End Sub

    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click
        CleanTable()
        EnableButtons()
    End Sub

    Protected Sub cmdOpenApprove_Click(sender As Object, e As EventArgs) Handles cmdOpenApprove.Click
        Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
        Dim dt As DataTable = Session("DataTable")
        Dim foundRepeated As Boolean = False
        Dim listRepeated As String = ""

        For Each row As DataRow In dt.Rows
            If (modelsChange.AlreadyExistModelChange(Guid.Empty, row("Modelo"))) Then
                foundRepeated = True
                listRepeated += "[" + row("Modelo") + "], "
            End If
        Next row

        If (foundRepeated) Then
            listRepeated = listRepeated.Substring(0, listRepeated.Length - 1)
            lblMessage.Text = "Se ha detectado que alguno de los modelos ha sido ingresado al sistema durante el flujo de configuración: " + listRepeated + ". Favor de rectificar los datos"
        Else
            ApproveModal.Show()
        End If
    End Sub

    Protected Sub cmdCancelModal_Click(sender As Object, e As EventArgs) Handles cmdCancelModal.Click
        CleanModalFields(True)
        ApproveModal.Hide()
    End Sub

    Protected Function ValidateTextBox(txt As TextBox, lbl As Label, errorMessage As String, canInsert As Boolean)
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

    Protected Sub CleanModalFields(cleanTextBoxes As Boolean)
        lblApproverError.Text = ""
        lblUserError.Text = ""
        lblPassworkError.Text = ""
        lblApproveMessageError.Text = ""
        lblModalMessage.Text = ""

        If (cleanTextBoxes) Then
            txtApprover.Text = ""
            txtUser.Text = ""
            txtPassword.Text = ""
            txtApproveMessage.Text = ""
        End If
    End Sub

    Protected Sub cmdAcceptChange_Click(sender As Object, e As EventArgs) Handles cmdAcceptChange.Click
        CleanModalFields(False)

        Dim canInsert As Boolean = True

        'Validaciones de los campos
        canInsert = ValidateTextBox(txtApprover, lblApproverError, "Buscar un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
        canInsert = ValidateTextBox(txtPassword, lblPassworkError, "Llenar el campo de Contraseña", canInsert)
        canInsert = ValidateTextBox(txtApproveMessage, lblApproveMessageError, "Llenar el campo de Mensaje", canInsert)

        'Validaciones extra por si acaso
        If (canInsert) Then
            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim approvedModelsChange As ED_ApprovedModelsChanges = New ED_ApprovedModelsChanges()

            Dim foundRepeated As Boolean
            Dim originUser As String             'Agregar función para obtener al usuario
            Dim approverEmail As String

            Dim dt As DataTable = Session("DataTable")
            Dim changeNumber As Integer = 1
            Dim originComment As String = txtApproveMessage.Text
            Dim approverUser As String = txtApprover.Text
            Dim approvalStatus As String = "Pendiente"
            Dim isActive As Boolean = True

            Dim idApprovedModelsChanges As Guid
            Dim idUnit As Guid
            Dim model As String
            Dim lifeSpan As Integer
            Dim modelChangeStatus As String = "Pendiente"

            'Validaciones del usuario aprobador
            If (Security.UserAD.GetUserExists(txtApprover.Text, "")) Then
                approverEmail = Security.UserAD.GetUserEmail(txtApprover.Text)
            Else
                lblModalMessage.Text = "No se encontro al Usuario Aprobador"
                ApproveModal.Show()
                Return
            End If

            'Validación del propio usuario
            If (Security.UserAD.GetUserExists(txtUser.Text, "")) Then 'Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT\")) Then
                originUser = txtUser.Text
            Else
                lblModalMessage.Text = "Usuario o contraseña incorrectos"
                ApproveModal.Show()
                Return
            End If

            'Validación de registros ya existentes
            For Each row As DataRow In dt.Rows
                If (modelsChange.AlreadyExistModelChange(Guid.Empty, row("Modelo"))) Then
                    foundRepeated = True
                End If
            Next row

            If (foundRepeated) Then
                lblModalMessage.Text = "Se ha detectado que uno o varios modelos seleccionados fueron cargados durante el proceso de aprobación. Favor de rectificar."
                ApproveModal.Show()
            Else
                idApprovedModelsChanges = approvedModelsChange.Insert(changeNumber, originUser, originComment, approverUser, approvalStatus, isActive, originUser)
                For Each row As DataRow In dt.Rows
                    idUnit = Guid.Parse(row("IdUnidad"))
                    model = row("Modelo")
                    lifeSpan = row("VidaUtil")
                    modelsChange.Insert(idApprovedModelsChanges, idUnit, model, lifeSpan, modelChangeStatus, originUser, isActive, originUser)
                Next row

                ApproveModal.Hide()
                MsgBox("Se ha completado exitósamente el registro de los cambios", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
                CleanModalFields(True)
                CleanTable()
            End If
        Else
            ApproveModal.Show()
        End If
    End Sub

    Async Function Test() As Threading.Tasks.Task(Of DataTable)
        Dim data = New DataTable()
        data.Columns.Add("Name", GetType(String))
        data.Columns.Add("UserName", GetType(String))
        data.Columns.Add("Email", GetType(String))
        data = Security.UserAD.GetAllUsers(txtApprover.Text, data)
        Return data
    End Function

    Protected Sub ddlUnit_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnit.SelectedIndexChanged


    End Sub

    'Protected Async Sub txtUser_TextChanged(sender As Object, e As EventArgs) Handles txtUser.TextChanged
    '    If (txtApprover.Text.Trim.Length > 3) Then
    '        txtApprover.DataSource = Await Test()
    '        txtApprover.DataTextField = "Name"
    '        txtApprover.DataValueField = "UserName"
    '        txtApprover.SelectedIndex = 0
    '        txtApprover.DataBind()
    '        Return
    '    End If
    'End Sub
End Class