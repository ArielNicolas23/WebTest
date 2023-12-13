Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports AjaxControlToolkit
Public Class CatModuloAprobacion
    Inherits System.Web.UI.Page

    ' Variables de las conexiones a BD
    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim userPlaceholder As String = "martil205"             'Pa que pongan su usuario aquí

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApproverUser(userPlaceholder))
        End If
    End Sub

    ' Llenado de tablas
    Protected Sub PopulateGrid(dgv As GridView, dt As DataTable)
        dgv.DataSource = dt
        dgv.DataBind()
    End Sub

    ' Método para ocultar el panel con el detalle de los cambios
    Protected Sub ToggleModelsChanges(hide As Boolean)
        If hide Then
            divModelsChanges.Visible = False
        Else
            divModelsChanges.Visible = True
        End If
    End Sub

    ' Método para ejecutar la acción del Header seleccionado
    Protected Sub dgvPendingApproval_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvPendingApproval.RowCommand
        Select Case e.CommandName
            Case "Action"
                Dim row As DataKeyArray = dgvPendingApproval.DataKeys
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id As Guid = Guid.Parse(row(index).Value.ToString())

                ToggleModelsChanges(False)

                modelChangesHeader.UpdateApprovalStatus(id, "En Revisión", userPlaceholder)
                PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByIdModelsChangesHeader(id))
                PopulateGrid(dgvModelChanges, modelChanges.SelectByIdModelsChangesHeader(id))
        End Select
    End Sub

    ' Método del botón de buscar por filtros
    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApprovalStatus(userPlaceholder, ddlStatus.SelectedValue))
    End Sub

    ' Método para cancelar la revisión de los cambios y seleccionar otro cambio
    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click
        PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApproverUser(userPlaceholder))
        ToggleModelsChanges(True)
    End Sub

    ' Método del botón de rechazar cambios
    Protected Sub cmdRejectChange_Click(sender As Object, e As EventArgs) Handles cmdRejectChange.Click
        cmdAcceptChange.Text = "Rechazar"
        txtUser.Text = ""
        txtPassword.Text = ""
        txtApproveMessage.Text = ""
        lblUserError.Text = ""
        lblPasswordError.Text = ""
        lblApproverError.Text = ""
        lblModalInstruction.Text = "Favor de ingresar sus credenciales para confirmar el rechazo"
        ApproveModal.Show()
        'RejectModal.Show()
    End Sub

    ' Método del botón de aprobar cambios
    Protected Sub cmdApproveChange_Click(sender As Object, e As EventArgs) Handles cmdApproveChange.Click
        Dim missingModel As Boolean = False
        cmdAcceptChange.Text = "Aprobar"
        txtUser.Text = ""
        txtPassword.Text = ""
        txtApproveMessage.Text = ""
        lblUserError.Text = ""
        lblPasswordError.Text = ""
        lblApproverError.Text = ""
        lblModalInstruction.Text = "Favor de ingresar sus credenciales para confirmar la aprobacion"
        For Each row As GridViewRow In dgvModelChanges.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim check As CheckBox = TryCast(row.Cells(6).FindControl("IsChecked"), CheckBox)
                If check.Checked Then

                Else
                    'Llenar mensaje con modelos faltantes
                    missingModel = True
                End If
            End If
        Next row

        If missingModel Then
            MsgBox("Aun no se han verificados todos los modelos", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Pasos sin completar")
            Return
        End If

        ApproveModal.Show()
    End Sub

    ' Método que actualiza el IsChecked del modelo seleccionado
    Protected Sub OnChangeIsChecked(sender As Object, e As EventArgs)
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)

        If checkBox IsNot Nothing Then
            Dim row As GridViewRow = DirectCast(checkBox.Parent.Parent, GridViewRow)
            Dim id As Guid = Guid.Parse(dgvModelChanges.DataKeys(row.RowIndex).Value.ToString())
            Dim isChecked As Boolean = checkBox.Checked

            modelChanges.UpdateIsChecked(id, isChecked)
        End If
    End Sub


    Protected Sub cmdAcceptChange_Click(sender As Object, e As EventArgs) Handles cmdAcceptChange.Click
        Dim approverUser As String
        Dim approverEmail As String
        Dim canInsert As Boolean = True


        If cmdAcceptChange.Text = "Aprobar" Then
            canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
            canInsert = ValidateTextBox(txtPassword, lblPasswordError, "Llenar el campo de Contraseña", canInsert)
        Else
            canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
            canInsert = ValidateTextBox(txtPassword, lblPasswordError, "Llenar el campo de Contraseña", canInsert)
            canInsert = ValidateTextBox(txtApproveMessage, lblApproveMessageError, "Llenar el campo de Mensaje", canInsert)
        End If
        If Not (canInsert) Then
            ApproveModal.Show()
            Exit Sub
        End If

        Dim originUser As String
        originUser = dgvModelChanges.Rows(0).Cells(4).Text.ToString()

        If (Security.UserAD.GetUserExists("orizag2", "")) Then
            approverEmail = Security.UserAD.GetUserEmail(originUser)
        Else
            lblModalMessage.Text = "No se encontro al Usuario Aprobador"
            ApproveModal.Show()
            Return
        End If

        'Validación del propio usuario
        If (Security.UserAD.GetUserExists(txtUser.Text, "")) Then 'Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT\") Then
            approverUser = txtUser.Text
        Else
            lblModalMessage.Text = "Usuario o contraseña incorrectos"
            ApproveModal.Show()
            Return
        End If

        Dim dataMail As New ConstructInfo With {
                                    .EmailType = "CambiosPendientes",
                                    .UserName = userPlaceholder,
                                    .Comment = txtApproveMessage.Text.Trim,
                                    .Link = "<a href=>Fecha De Expiración</a>"
                                    }
        Dim email As New ModuloGeneralEmail

        If email.ConstructEmail(dataMail) Then
            MsgBox("Se ha enviado un correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
        Else
            MsgBox("Ha ocurrido un error al mandar correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
        End If

        Dim id As Guid = Guid.Parse("6feda1e6-f767-4988-ab49-de93ad311746")
        'Guid.Parse(rows(Index).Value.ToString())

        Dim idHeader As Guid = Guid.Parse("3170bd92-2c35-4148-a38a-2cf6caad13d7")
        'Guid.Parse(row(index).Value.ToString())

        If cmdAcceptChange.Text = "Aprobar" Then

            modelChangesHeader.UpdateApprovalStatus(idHeader, "Aprobado", userPlaceholder)
            ToggleModelsChanges(False)
        Else
            'update header

            modelChangesHeader.UpdateApprovalStatus(idHeader, "Rechazado", userPlaceholder)
            ToggleModelsChanges(False)

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

    Protected Sub cmdCancelModal_Click(sender As Object, e As EventArgs) Handles cmdCancelModal.Click

    End Sub


End Class