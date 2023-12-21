Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.WebPages
Imports AjaxControlToolkit
Imports Microsoft.Ajax.Utilities

Public Class WebForm1
    Inherits System.Web.UI.Page

    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim headerEdit As DataTable
    Dim modelsEdit As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'inicializa la ventana en modo edicion o nuevo si el url contiene un parametro
        If Not Page.IsPostBack Then
            Session("DataTable") = Initdt()
            Dim urlParam As String = Request.QueryString("id")
            InitView(urlParam.IsNullOrWhiteSpace, urlParam)
        End If
        EnableButtons()
    End Sub
    Protected Sub InitView(isNew As Boolean, id As String)
        Dim catUnits As CatUnits = New CatUnits()
        If isNew Then
            'inicializa ventana para nuevo
            divModelsNew.Visible = True
            ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
            ddlUnit.DataTextField = "Unit"
            ddlUnit.DataValueField = "IdUnit"
            ddlUnit.DataBind()

        Else
            Try
                'inicializa ventana para editar
                'obtiene informacion a partir del id del url
                headerEdit = modelChangesHeader.SelectByIdModelsChangesHeader(Guid.Parse(id))
                modelsEdit = modelChanges.SelectByIdModelsChangesHeader(Guid.Parse(id))
                'Guarda los modelos a editar en una variable de sesion
                Session("Models") = modelsEdit
                Session("HeaderId") = headerEdit.Rows(0).Item(0)

                'Inicializa dropdown
                ddlUnitEdit.DataSource = catUnits.SelectAll("", False).AsDataView
                ddlUnitEdit.DataTextField = "Unit"
                ddlUnitEdit.DataValueField = "IdUnit"
                ddlUnitEdit.DataBind()

                'Funcion que valida el usuario originador con el de la sesion
                Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile)
                Dim actualUser = m_Profile.UserName.Split("\")(1)

                'Valida estatus del cambio y usuario originador
                If headerEdit.Rows(0).Item(3).ToString.ToLower = actualUser.ToString.ToLower And (headerEdit.Rows(0).Item(8).ToString.ToLower = "pendiente" Or headerEdit.Rows(0).Item(8).ToString.ToLower = "rechazado") Then
                    'llena informacion del gridview
                    dgvEdit.DataSource = modelsEdit
                    dgvEdit.DataBind()

                    'genera una variable de sesion para almacenar registros por borrar
                    Dim delt = New ArrayList
                    Session("DelModels") = delt
                    EnableButtonsEdit()
                    divModelEdit.Visible = True
                Else
                    'Vista de error simple
                    divError.Visible = True
                End If

            Catch
                'vista de error - agregar razon
                divError.Visible = True
                Return
            End Try


        End If



    End Sub
    Protected Function Initdt()
        Dim data = New System.Data.DataTable("Result")
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

    Private Sub CleanAddDataEdit()
        lblMessageEdit.Text = ""
        txtModelEdit.Text = ""
        txtLifeSpanEdit.Text = ""
    End Sub

    Private Sub CleanTable()
        Session("DataTable") = Initdt()
        gvModelos.DataSource = Session("DataTable")
        gvModelos.DataBind()
    End Sub

    Private Sub CleanTableEdit()
        Dim temp As System.Data.DataTable = Session("Models")
        temp.Clear()
        Session("Models") = temp
        dgvEdit.DataSource = Session("Models")
        dgvEdit.DataBind()
    End Sub

    Private Sub EnableButtons()
        Dim dt As System.Data.DataTable = Session("DataTable")

        If dt.Rows.Count > 0 Then
            divButtons.Visible = True
        Else
            divButtons.Visible = False
        End If
    End Sub
    Private Sub EnableButtonsEdit()
        Dim dt As System.Data.DataTable = Session("Models")

        If dt.Rows.Count > 0 Then
            divButtonsEdit.Visible = True
        Else
            divButtonsEdit.Visible = False
        End If
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

    Protected Sub CleanModalFieldsEdit(cleanTextBoxes As Boolean)
        lblApproverErrorEdit.Text = ""
        lblUserErrorEdit.Text = ""
        lblPasswordErrorEdit.Text = ""
        lblApproveMessageErrorEdit.Text = ""
        lblModalMessageEdit.Text = ""

        If (cleanTextBoxes) Then
            txtApproverEdit.Text = ""
            txtUsernameApproverEdit.Text = ""
            txtMailApproverEdit.Text = ""
            txtUserEdit.Text = ""
            txtPasswordEdit.Text = ""
            txtApproveMessageEdit.Text = ""
        End If
    End Sub



    Protected Sub gvModelos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvModelos.RowDeleting
        Dim dt As System.Data.DataTable = Session("DataTable")
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
        gvModelos.DataSource = dt
        gvModelos.DataBind()

        Session("DataTable") = dt

        EnableButtons()
    End Sub

    Protected Sub lbCancel_Click(sender As Object, e As EventArgs) Handles lbCancel.Click
        CleanModalFields(True)
        ApproveModal.Hide()
    End Sub

    Protected Sub lbAccept_Click(sender As Object, e As EventArgs) Handles lbAccept.Click
        CleanModalFields(False)

        txtMailApprover.Enabled = False
        txtUser.Enabled = False
        txtPassword.Enabled = False


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
            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim approvedModelsChange As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
            Dim dt As System.Data.DataTable = Session("DataTable")
            Dim foundRepeated As Boolean

            Dim originUser As String             'Agregar función para obtener al usuario
            Dim originName As String
            Dim originEmail As String

            Dim approverUser As String
            Dim approverName As String
            Dim approverEmail As String

            Dim changeNumber As Integer = 1
            Dim originComment As String = txtApproveMessage.Text
            Dim approvalStatus As String = "Pendiente"
            Dim isActive As Boolean = True

            Dim IdModelsChangesHeader As Guid
            Dim idUnit As Guid
            Dim model As String
            Dim lifeSpan As Integer
            Dim modelChangeStatus As String = "Pendiente"

            Dim strApproverName As String = txtApprover.Text

            'Asignación del usuario aprobador
            If Not txtUsernameApprover.Text.IsEmpty Then
                approverUser = txtUsernameApprover.Text
            Else
                lblModalMessage.Text = "No se encontro al Usuario Aprobador"
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True

                ApproveModal.Show()

                Return
            End If


            'Validaciones del usuario aprobador
            If (Security.UserAD.GetUserExists(approverUser, "")) Then
                approverName = txtApprover.Text                                  'Agregar función para buscar el nombre del aprobador
                approverEmail = txtMailApprover.Text
            Else
                lblModalMessage.Text = "No se encontro al Usuario Aprobador"
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True

                ApproveModal.Show()
                Return
            End If


            'Validacion de usuario originador
            Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile) 'Funcion que valida el usuario originador con el de la sesion 
            Dim actualUser = m_Profile.UserName.Split("\")(1)
            If (Not txtUser.Text.ToLower() = actualUser) Then
                lblModalMessage.Text = "Porfavor ingrese el usuario de su sesión"
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True

                ApproveModal.Show()
                Return
            End If

            'Validacion de usuario originador y aprobador
            'If (txtUsernameApprover.Text = actualUser) Then
            'lblModalMessage.Text = "El usuario aprobador no puede ser el mismo que el originador"
            'ApproveModal.Show()
            'Return
            'End If

            'Validación del propio usuario
            If (Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT")) Then   'Agregar función para validar el usuario y contraseña 
                originUser = txtUser.Text
                originName = Security.UserAD.GetUserName(originUser)                           'Agregar función para buscar el nombre del usuario
                originEmail = Security.UserAD.GetUserEmail(originUser)
            Else
                lblModalMessage.Text = "Usuario o contraseña incorrectos"
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True

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
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True
                ApproveModal.Show()
            Else
                IdModelsChangesHeader = approvedModelsChange.Insert(originUser, originName, originEmail, originComment, approverUser, approverName, approverEmail, approvalStatus, isActive, originUser)
                For Each row As DataRow In dt.Rows
                    idUnit = Guid.Parse(row("IdUnidad"))
                    model = row("Modelo")
                    lifeSpan = row("VidaUtil")
                    modelsChange.Insert(IdModelsChangesHeader, idUnit, model, lifeSpan, modelChangeStatus, originUser, originName, originEmail, isActive, originUser)
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
                CleanTable()
                txtMailApprover.Enabled = True
                txtUser.Enabled = True
                txtPassword.Enabled = True

            End If
        Else
            ApproveModal.Show()
        End If
    End Sub



    Protected Sub ldModel_Click(sender As Object, e As EventArgs) Handles ldModel.Click
        Try
            lblMessage.Text = ""

            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim dt As System.Data.DataTable = Session("DataTable")
            Dim foundRepeated As Boolean
            Dim strModel As String = txtModel.Text.Trim.ToUpper
            Dim strLifeSpan As String = txtLifeSpan.Text.Trim.ToUpper
            Dim strUnit As String = ddlUnit.SelectedItem.Text.Trim.ToUpper
            Dim idUnit As String = ddlUnit.SelectedValue
            Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile)
            Dim actualUser = m_Profile.UserName.Split("\")(1)

            If (strModel = "") Then
                lblMessage.Text = "Favor de escribir el nombre del Modelo"
                Return
            End If

            If (Not Regex.IsMatch(strLifeSpan, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numérico para la Vida Útil"
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
                    row("Usuario") = actualUser       'Agregar función para obtener al usuario
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

    Protected Sub lbCancelChange_Click(sender As Object, e As EventArgs) Handles lbCancelChange.Click
        Dim confirmed As Integer = MsgBox("Se reiniciará el proceso carga y se borrarán todos modelos actualmente agregados. ¿Está seguro de continuar?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")

        If confirmed = MsgBoxResult.Yes Then
            CleanTable()
            EnableButtons()
            CleanAddData()
        Else

        End If
    End Sub

    Protected Sub lbOpenApprove_Click(sender As Object, e As EventArgs) Handles lbOpenApprove.Click
        Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
        Dim dt As System.Data.DataTable = Session("DataTable")
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

    Protected Sub gvModelos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvModelos.RowEditing
        gvModelos.EditIndex = e.NewEditIndex
    End Sub

    Protected Sub dgvEdit_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvEdit.RowDeleting
        Dim dt As System.Data.DataTable = Session("Models")
        Dim delt As ArrayList = Session("DelModels")
        delt.Add(dt.Rows(e.RowIndex).Item(0))
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
        dgvEdit.DataSource = dt
        dgvEdit.DataBind()

        Session("DelModels") = delt
        Session("Models") = dt

        EnableButtonsEdit()

    End Sub

    Protected Sub dgvEdit_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles dgvEdit.RowEditing
        dgvEdit.EditIndex = e.NewEditIndex

        Dim dt As System.Data.DataTable = Session("Models")
        dgvEdit.DataSource = dt
        dgvEdit.DataBind()
        Session("Models") = dt

        'Informacion de unidades
        Dim catUnits As CatUnits = New CatUnits()
        Dim unit = dt.Rows(e.NewEditIndex).Item(3).ToString
        Dim units = catUnits.SelectAll("", False).AsDataView


        'Configurar dropdown list
        Dim ddlUnitEditGrid As DropDownList = CType(dgvEdit.Rows(e.NewEditIndex).FindControl("ddlUnitEditGrid"), DropDownList)
        ddlUnitEditGrid.DataSource = units
        ddlUnitEditGrid.DataTextField = "Unit"
        ddlUnitEditGrid.DataValueField = "IdUnit"
        ddlUnitEditGrid.DataBind()
        ddlUnitEditGrid.Items.FindByText(unit).Selected = True

    End Sub

    Protected Sub dgvEdit_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles dgvEdit.RowUpdating
        Try
            lblMessageEdit.Text = ""

            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim dt As System.Data.DataTable = Session("Models")
            Dim rows As GridViewRow = dgvEdit.Rows(e.RowIndex)
            Dim names As DataKeyArray = dgvEdit.DataKeys()
            Dim idModel As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())

            Dim foundRepeated As Boolean
            Dim strModel As String = CType((rows.Cells(1).Controls(0)), System.Web.UI.WebControls.TextBox).Text.Trim.ToUpper()
            Dim strLifeSpan As String = CType((rows.Cells(2).Controls(0)), System.Web.UI.WebControls.TextBox).Text.Trim.ToUpper()
            Dim strUnit As String = CType((rows.Cells(3).Controls(1)), System.Web.UI.WebControls.DropDownList).SelectedItem.Text.Trim.ToUpper()
            Dim idUnit As String = CType((rows.Cells(3).Controls(1)), System.Web.UI.WebControls.DropDownList).SelectedValue.Trim.ToUpper()

            If (strModel = "") Then
                lblMessageEdit.Text = "Favor de escribir el nombre del Modelo"
                Return
            End If

            If (Not Regex.IsMatch(strLifeSpan, "^[0-9 ]+$")) Then
                lblMessageEdit.Text = "Favor de escribir un valor numérico para la Vida Útil"
                Return
            End If

            If (strLifeSpan = "") Then
                lblMessageEdit.Text = "Favor de seleccionar una Unidad para el Modelo"
                Return
            End If

            For Each row As DataRow In dt.Rows
                If (strModel = row(1) And Not (idModel = row(0))) Then
                    foundRepeated = True
                End If
            Next row

            If (foundRepeated) Then
                lblMessageEdit.Text = "El Modelo que intenta ingresar ya se encuentra listado"
            Else
                If (modelsChange.AlreadyExistModelChange(idModel, strModel)) Then
                    lblMessageEdit.Text = "No es posble Actualizar el Modelo debido a que ya existe un Modelo activo registrado con ese nombre: [" + strModel + "]"

                Else


                    dt.Rows(e.RowIndex)("Model") = strModel
                    dt.Rows(e.RowIndex)("Lifespan") = strLifeSpan
                    dt.Rows(e.RowIndex)("Unit") = strUnit
                    dt.Rows(e.RowIndex)("IdUnit") = idUnit
                    dgvEdit.EditIndex = -1
                    dgvEdit.DataSource = dt
                    dgvEdit.DataBind()

                    Session("Models") = dt



                End If
            End If
        Catch ex As Exception
            lblMessageEdit.Text = "Ocurrió un error al intentar agregar los datos: " + ex.Message
        End Try






    End Sub

    Protected Sub dgvEdit_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles dgvEdit.RowCancelingEdit
        dgvEdit.EditIndex = -1
        Dim dt As System.Data.DataTable = Session("Models")
        dgvEdit.DataSource = dt
        dgvEdit.DataBind()
        Session("Models") = dt
    End Sub

    Protected Sub ldModelEdit_Click(sender As Object, e As EventArgs) Handles ldModelEdit.Click
        Try
            lblMessageEdit.Text = ""

            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim dt As System.Data.DataTable = Session("Models")
            Dim foundRepeated As Boolean
            Dim strModel As String = txtModelEdit.Text.Trim.ToUpper
            Dim strLifeSpan As String = txtLifeSpanEdit.Text.Trim.ToUpper
            Dim strUnit As String = ddlUnitEdit.SelectedItem.Text.Trim.ToUpper
            Dim idUnit As String = ddlUnitEdit.SelectedValue
            Dim idModel As Guid = Guid.NewGuid
            Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile)
            Dim actualUser = m_Profile.UserName.Split("\")(1)

            If (strModel = "") Then
                lblMessageEdit.Text = "Favor de escribir el nombre del Modelo"
                Return
            End If

            If (Not Regex.IsMatch(strLifeSpan, "^[0-9 ]+$")) Then
                lblMessageEdit.Text = "Favor de escribir un valor numérico para la Vida Útil"
                Return
            End If

            If (strLifeSpan = "") Then
                lblMessageEdit.Text = "Favor de seleccionar una Unidad para el Modelo"
                Return
            End If

            For Each row As DataRow In dt.Rows
                If (strModel = row(1)) Then
                    foundRepeated = True
                End If
            Next row

            If (foundRepeated) Then
                lblMessageEdit.Text = "El Modelo que intenta ingresar ya se encuentra listado"
            Else
                If (modelsChange.AlreadyExistModelChange(Guid.Empty, strModel)) Then
                    lblMessageEdit.Text = "No es posble ingresar el Modelo debido a que ya existe un Modelo activo registrado con ese nombre: [" + strModel + "]"
                Else
                    Dim row As DataRow = dt.NewRow
                    row("IdModelsChanges") = idModel
                    row("Model") = strModel
                    row("Lifespan") = strLifeSpan
                    row("Unit") = strUnit
                    row("IdUnit") = idUnit
                    row("LastUser") = actualUser 'Agregar función para obtener al usuario

                    dt.Rows.Add(row)

                    dgvEdit.DataSource = dt
                    dgvEdit.DataBind()

                    Session("Models") = dt

                    CleanAddDataEdit()
                End If
            End If
            EnableButtonsEdit()

        Catch ex As Exception
            lblMessageEdit.Text = "Ocurrió un error al intentar agregar los datos: " + ex.Message
        End Try

    End Sub

    Protected Sub lbCancelChangeEdit_Click(sender As Object, e As EventArgs) Handles lbCancelChangeEdit.Click
        Dim confirmed As Integer = MsgBox("Se reiniciará el proceso carga y se borrarán todos modelos actualmente agregados. ¿Está seguro de continuar?", MsgBoxStyle.YesNo + MsgBoxStyle.MsgBoxSetForeground, "Aviso")

        If confirmed = MsgBoxResult.Yes Then
            CleanTableEdit()
            EnableButtonsEdit()
            CleanAddDataEdit()
        Else

        End If
    End Sub

    Protected Sub lbOpenApproveEdit_Click(sender As Object, e As EventArgs) Handles lbOpenApproveEdit.Click
        Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
        Dim dt As System.Data.DataTable = Session("Models")
        Dim foundRepeated As Boolean = False
        Dim listRepeated As String = ""

        For Each row As DataRow In dt.Rows
            If (modelsChange.AlreadyExistModelChange(row("IdModelsChanges"), row("Model"))) Then
                foundRepeated = True
                listRepeated += "[" + row("Model") + "], "
            End If
        Next row

        If (foundRepeated) Then
            listRepeated = listRepeated.Substring(0, listRepeated.Length - 1)
            lblMessageEdit.Text = "Se ha detectado que alguno de los modelos ha sido ingresado al sistema durante el flujo de configuración: " + listRepeated + ". Favor de rectificar los datos"
        Else
            ApproveModalEdit.Show()
        End If
    End Sub

    Protected Sub lbAcceptEdit_Click(sender As Object, e As EventArgs) Handles lbAcceptEdit.Click
        CleanModalFieldsEdit(False)

        txtMailApproverEdit.Enabled = False
        txtUserEdit.Enabled = False
        txtPasswordEdit.Enabled = False


        Dim canInsert As Boolean = True

        'Validaciones de los campos
        canInsert = ValidateTextBox(txtApproverEdit, lblModalMessageEdit, "Favor de buscar y seleccionar a un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtUsernameApproverEdit, lblModalMessageEdit, "Favor de buscar y seleccionar a un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtMailApproverEdit, lblModalMessageEdit, "Favor de buscar y seleccionar a un Aprobador", canInsert)

        canInsert = ValidateTextBox(txtApproverEdit, lblApproverErrorEdit, "Buscar un Aprobador", canInsert)
        canInsert = ValidateTextBox(txtUserEdit, lblUserErrorEdit, "Llenar el campo de Usuario", canInsert)
        canInsert = ValidateTextBox(txtPasswordEdit, lblPasswordErrorEdit, "Llenar el campo de Contraseña", canInsert)
        canInsert = ValidateTextBox(txtApproveMessageEdit, lblApproveMessageErrorEdit, "Llenar el campo de Mensaje", canInsert)

        'Validaciones extra por si acaso
        If (canInsert) Then
            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim approvedModelsChange As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
            Dim dt As System.Data.DataTable = Session("Models")
            Dim delt As System.Data.DataTable = Session("Models")
            Dim foundRepeated As Boolean

            Dim originUser As String             'Agregar función para obtener al usuario
            Dim originName As String
            Dim originEmail As String

            Dim approverUser As String
            Dim approverName As String
            Dim approverEmail As String

            Dim changeNumber As Integer = 1
            Dim originComment As String = txtApproveMessageEdit.Text
            Dim approvalStatus As String = "Pendiente"
            Dim isActive As Boolean = True

            Dim IdModelsChangesHeader As Guid
            Dim IdModel As Guid
            Dim idUnit As Guid
            Dim model As String
            Dim lifeSpan As Integer
            Dim modelChangeStatus As String = "Pendiente"

            Dim strApproverName As String = txtApproverEdit.Text

            'Asignación del usuario aprobador
            If Not txtUsernameApproverEdit.Text.IsEmpty Then
                approverUser = txtUsernameApproverEdit.Text
            Else
                lblModalMessageEdit.Text = "No se encontro al Usuario Aprobador"
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True

                ApproveModalEdit.Show()

                Return
            End If


            'Validaciones del usuario aprobador
            If (Security.UserAD.GetUserExists(approverUser, "")) Then
                approverName = txtApproverEdit.Text                                  'Agregar función para buscar el nombre del aprobador
                approverEmail = txtMailApproverEdit.Text
            Else
                lblModalMessageEdit.Text = "No se encontro al Usuario Aprobador"
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True

                ApproveModalEdit.Show()
                Return
            End If


            'Validacion de usuario originador
            Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile) 'Funcion que valida el usuario originador con el de la sesion 
            Dim actualUser = m_Profile.UserName.Split("\")(1)
            If (Not txtUserEdit.Text.ToLower() = actualUser) Then
                lblModalMessageEdit.Text = "Porfavor ingrese el usuario de su sesión"
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True

                ApproveModalEdit.Show()
                Return
            End If

            'Validacion de usuario originador y aprobador
            'If (txtUsernameApproverEdit.Text = actualUser) Then
            'lblModalMessageEdit.Text = "El usuario aprobador no puede ser el mismo que el originador"
            'ApproveModalEdit.Show()
            'Return
            'End If

            'Validación del propio usuario
            If (Security.UserAD.ValidateUser(txtUserEdit.Text, txtPasswordEdit.Text, "ENT")) Then   'Agregar función para validar el usuario y contraseña 
                originUser = txtUserEdit.Text
                originName = Security.UserAD.GetUserName(originUser)                           'Agregar función para buscar el nombre del usuario
                originEmail = Security.UserAD.GetUserEmail(originUser)
            Else
                lblModalMessageEdit.Text = "Usuario o contraseña incorrectos"
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True

                ApproveModalEdit.Show()
                Return
            End If

            'Validación de registros ya existentes
            For Each row As DataRow In dt.Rows
                If (modelsChange.AlreadyExistModelChange(row("IdModelsChanges"), row("Model"))) Then
                    foundRepeated = True
                End If
            Next row


            If (foundRepeated) Then
                lblModalMessageEdit.Text = "Se ha detectado que uno o varios modelos seleccionados fueron cargados durante el proceso de aprobación. Favor de rectificar."
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True
                ApproveModalEdit.Show()
            Else
                'Updateheader
                IdModelsChangesHeader = Session("HeaderId")
                approvedModelsChange.UpdateApprovalEdit(IdModelsChangesHeader, originUser, originName, originComment, originEmail, approverUser, approverName, approverEmail, "", "Pendiente", actualUser)
                For Each row As DataRow In dt.Rows
                    IdModel = row("IdModelsChanges")
                    idUnit = Guid.Parse(row("IdUnit"))
                    model = row("Model")
                    lifeSpan = row("Lifespan")

                    'Verifica si es actualizacion de registro o nuevo registro
                    Dim updateRow As DataTable = modelsChange.SelectByIdModelsChanges(IdModel)
                    If (updateRow.Rows.Count > 0) Then
                        modelsChange.UpdateEdit(IdModel, IdModelsChangesHeader, Nothing, idUnit, model, lifeSpan, modelChangeStatus, originUser, originName, originEmail, actualUser)
                    Else
                        modelsChange.Insert(IdModelsChangesHeader, idUnit, model, lifeSpan, modelChangeStatus, originUser, originName, originEmail, isActive, originUser)
                    End If


                Next row



                ApproveModalEdit.Hide()

                MsgBox("Se ha completado exitósamente el registro de los cambios", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")


                Dim dataMail As New ConstructInfo With {
                                .EmailType = "CambiosPendientes",
                                .UserName = originUser,
                                .Comment = txtApproveMessage.Text.Trim,
                                .Link = "<a href=>Fecha De Expiración</a>"
                                }
                Dim email As New ModuloGeneralEmail

                If email.ConstructEmail(dataMail) Then
                    MsgBox("Se ha enviado un correo a " + txtApproverEdit.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
                Else
                    MsgBox("Ha ocurrido un error al mandar correo a " + txtApproverEdit.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
                End If

                CleanModalFieldsEdit(True)
                txtMailApproverEdit.Enabled = True
                txtUserEdit.Enabled = True
                txtPasswordEdit.Enabled = True

            End If
        Else
            ApproveModalEdit.Show()
        End If
    End Sub

    Protected Sub lbCancelEdit_Click(sender As Object, e As EventArgs) Handles lbCancelEdit.Click
        CleanModalFieldsEdit(True)
        ApproveModalEdit.Hide()
    End Sub
End Class