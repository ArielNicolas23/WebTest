Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.WebPages
Imports AjaxControlToolkit
Imports Microsoft.Ajax.Utilities

Public Class CatModuloAprobacion
    Inherits System.Web.UI.Page

    ' Variables de las conexiones a BD
    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim userPlaceholder As String

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)
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

    Protected Sub ApproveOrReject(sender As Object, e As EventArgs) Handles cmdApproveChange.Click
        Dim btn As Button = TryCast(sender, Button)

        Select Case btn.CommandName
            Case "Approve"
                Dim isMissingModel As Boolean = False
                Dim modelsList As String = True
                For Each row As GridViewRow In dgvModelChanges.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim check As CheckBox = TryCast(row.Cells(6).FindControl("IsChecked"), CheckBox)
                        If check.Checked Then

                        Else
                            modelsList += "[" + row.Cells(1).Text + "], "
                            isMissingModel = True
                        End If
                    End If
                Next row

                If isMissingModel Then
                    modelsList = modelsList.Substring(0, modelsList.Length - 1)
                    MsgBox("Aún no se han verificados todos los modelos." + vbNewLine + "Modelos faltantes por verificar: " + modelsList, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Modelos sin verificar")
                    Return
                End If

                cmdAcceptChange.Text = "Aprobar"
                lblModalInstruction.Text = "Favor de ingresar sus credenciales para confirmar la Aprobación"
                ApproveModal.Show()

            Case "Reject"
                cmdAcceptChange.Text = "Rechazar"
                lblModalInstruction.Text = "Favor de ingresar sus credenciales para confirmar el Rechazo"
                ApproveModal.Show()
        End Select
        CleanModalFields(True)

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
        CleanModalFields(False)

        Dim canInsert As Boolean = True

        ' Validaciones de los campos
        Select Case cmdAcceptChange.Text
            Case "Aprobar"
                canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
                canInsert = ValidateTextBox(txtPassword, lblPasswordError, "Llenar el campo de Contraseña", canInsert)

            Case "Rechazar"
                canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
                canInsert = ValidateTextBox(txtPassword, lblPasswordError, "Llenar el campo de Contraseña", canInsert)
                canInsert = ValidateTextBox(txtApproveMessage, lblApproveMessageError, "Llenar el campo de Mensaje", canInsert)

        End Select

        If Not canInsert Then
            ApproveModal.Show()
            Return
        End If

        Dim row As DataKeyArray = dgvPendingApproval.DataKeys
        Dim idHeader As Guid = Guid.Parse(row(0).Value.ToString())
        Dim idModel As Guid
        Dim originUser As String = userPlaceholder
        Dim comment As String = txtApproveMessage.Text
        Dim confirmMessage As String = ""

        Dim m_Profile = CType(Session("UserProfile"), Security.UserProfile)
        Dim actualUser = m_Profile.UserName.Split("\")(1)

        'Validacion de usuario originador
        If (Not txtUser.Text = actualUser) Then
            lblModalMessage.Text = "Porfavor ingrese el usuario de su sesión"
            ApproveModal.Show()
            Return
        End If

        'Validación del propio usuario
        If (Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT")) Then

        Else
            lblModalMessage.Text = "Usuario o contraseña incorrectos"
            ApproveModal.Show()
            Return
        End If

        Select Case cmdAcceptChange.Text
            Case "Aprobar"
                modelChangesHeader.UpdateApproveOrReject(idHeader, comment, "Aprobado", userPlaceholder)
                confirmMessage = "Se han Aprobado todos los modelos incluidos en este cambio. Se le notificará al usuario originador"

            Case "Rechazar"
                modelChangesHeader.UpdateApproveOrReject(idHeader, comment, "Rechazado", userPlaceholder)
                For Each modelRow As GridViewRow In dgvModelChanges.Rows
                    'idModel = Guid.Parse(modelRow.Cells(0).Text)
                    idModel = Guid.Parse(dgvModelChanges.DataKeys(modelRow.RowIndex).Value.ToString())
                    modelChanges.UpdateIsChecked(idModel, False)
                Next modelRow
                confirmMessage = "Se han Rechazado todos los modelos incluidos en este cambio. Se le notificará al usuario originador para su futura atención"

        End Select

        ToggleModelsChanges(True)
        ApproveModal.Hide()
        MsgBox(confirmMessage, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")

        Dim dataMail As New ConstructInfo With {
                            .EmailType = "CambiosPendientes",
                            .UserName = originUser,
                            .Comment = txtApproveMessage.Text.Trim,
                            .Link = "<a href=>Fecha De Expiración</a>"
                            }
        Dim email As New ModuloGeneralEmail

        If email.ConstructEmail(dataMail) Then
            MsgBox("Se ha enviado un correo a el originador " + originUser, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
        Else
            MsgBox("Ha ocurrido un error al mandar correo a el originador " + originUser, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
        End If

        PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApproverUser(userPlaceholder))
        ToggleModelsChanges(True)
    End Sub

    ' Método para validar campos de aprobación
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

    ' Método para limpiar campos
    Protected Sub CleanModalFields(cleanTextBoxes As Boolean)
        lblUserError.Text = ""
        lblPasswordError.Text = ""
        lblApproveMessageError.Text = ""
        lblModalMessage.Text = ""

        If (cleanTextBoxes) Then
            txtUser.Text = ""
            txtPassword.Text = ""
            txtApproveMessage.Text = ""
        End If
    End Sub

    Protected Sub cmdCancelModal_Click(sender As Object, e As EventArgs) Handles cmdCancelModal.Click
        CleanModalFields(True)
        ApproveModal.Hide()
    End Sub


End Class