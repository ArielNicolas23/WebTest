Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports AjaxControlToolkit
Public Class CatModuloAprobacion
    Inherits System.Web.UI.Page

    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApproverUser("orizag2"))
        End If
    End Sub

    Protected Sub PopulateGrid(dgv As GridView, dt As DataTable)
        dgv.DataSource = dt
        dgv.DataBind()
    End Sub

    Protected Sub ToggleModelsChanges()
        If (divModelsChanges.Visible) Then
            dgvModelChanges.Visible = False
        Else
            dgvModelChanges.Visible = True
        End If
    End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatus.SelectedIndexChanged
        PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApprovalStatus("orizag2", ddlStatus.SelectedValue))
    End Sub

    Protected Sub dgvPendingApproval_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvPendingApproval.RowCommand
        Select Case e.CommandName
            Case "Action"
                Dim row As DataKeyArray = dgvPendingApproval.DataKeys
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id As Guid = Guid.Parse(row(index).Value.ToString())

                'ToggleModelsChanges()
                PopulateGrid(dgvModelChanges, modelChanges.SelectByIdModelsChangesHeader(id))
                'Dim dt As DataTable = Session("DataTable")

                'Dim rowdt As DataRow = dt.NewRow
                'rowdt("Modelo") = dgvModelChanges.SelectedRow.Cells.Item(0).Text.ToString()
                'rowdt("VidaUtil") = dgvModelChanges.SelectedRow.Cells.Item(1).Text.ToString()
                'rowdt("Unidad") = dgvModelChanges.SelectedRow.Cells.Item(2).Text.ToString()
                'rowdt("Usuario") = dgvModelChanges.SelectedRow.Cells.Item(3).Text.ToString() '"Origin User"        'Agregar función para obtener al usuario
                'rowdt("Ultima Actualiuzacion") = dgvModelChanges.SelectedRow.Cells.Item(4).Text.ToString()
        End Select
    End Sub

    Protected Sub cmdShowPending_Click(sender As Object, e As EventArgs) Handles cmdShowPending.Click

    End Sub

    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click

    End Sub

    Protected Sub cmdRejectChange_Click(sender As Object, e As EventArgs) Handles cmdRejectChange.Click

        RejectModal.Show()
    End Sub

    Protected Sub cmdApproveChange_Click(sender As Object, e As EventArgs) Handles cmdApproveChange.Click
        'MsgBox("Aun no se han seleccionado todos los campos", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Pasos sin completar")
        ApproveModal.Show()
    End Sub

    Protected Sub dgvModelChanges_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvModelChanges.SelectedIndexChanged

    End Sub

    Protected Sub cmdAcceptChange_Click(sender As Object, e As EventArgs) Handles cmdAcceptChange.Click
        Dim approverUser As String
        Dim approverEmail As String
        Dim canInsert As Boolean = True

        canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
        canInsert = ValidateTextBox(txtPassword, lblPassworkError, "Llenar el campo de Contraseña", canInsert)
        If Not (canInsert) Then
            ApproveModal.Show()
        End If

        Dim originUser As String
        originUser = dgvModelChanges.Rows(0).Cells(4).Text.ToString()

        If (Security.UserAD.GetUserExists(originUser, "")) Then
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
                                    .UserName = "orizag2",
                                    .Comment = txtApproveMessage.Text.Trim,
                                    .Link = "<a href=>Fecha De Expiración</a>"
                                    }
        Dim email As New ModuloGeneralEmail

        If email.ConstructEmail(dataMail) Then
            MsgBox("Se ha enviado un correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
        Else
            MsgBox("Ha ocurrido un error al mandar correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
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

    Protected Sub btnRejectChange_Click(sender As Object, e As EventArgs) Handles btnRejectChange.Click
        Dim approverUser As String
        Dim approverEmail As String
        Dim canInsert As Boolean = True

        canInsert = ValidateTextBox(txtUser, lblUserError, "Llenar el campo de Usuario", canInsert)
        canInsert = ValidateTextBox(txtPassword, lblPassworkError, "Llenar el campo de Contraseña", canInsert)
        canInsert = ValidateTextBox(txtApproveMessage, lblApproveMessageError, "Llenar el campo de Mensaje", canInsert)


        If Not (canInsert) Then
            ApproveModal.Show()
        End If

        Dim originUser As String
        originUser = dgvModelChanges.Rows(0).Cells(4).Text.ToString()

        If (Security.UserAD.GetUserExists(originUser, "")) Then
            approverEmail = Security.UserAD.GetUserEmail(originUser)
        Else
            lblModalMessage.Text = "No se encontro al Usuario Originador"
            ApproveModal.Show()
            Return
        End If

        'Validación del propio usuario
        If (Security.UserAD.GetUserExists(txtUser.Text, "")) Then 'Security.UserAD.ValidateUser(txtUser.Text, txtPassword.Text, "ENT\") Then
            approverUser = txtUserReject.Text
        Else
            lblModalMessage.Text = "Usuario o contraseña incorrectos"
            ApproveModal.Show()
            Return
        End If

        Dim dataMail As New ConstructInfo With {
                                    .EmailType = "CambiosPendientes",
                                    .UserName = "orizag2",
                                    .Comment = txtApproveMessage.Text.Trim,
                                    .Link = "<a href=>Fecha De Expiración</a>"
                                    }
        Dim email As New ModuloGeneralEmail

        If email.ConstructEmail(dataMail) Then
            MsgBox("Se ha enviado un correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
        Else
            MsgBox("Ha ocurrido un error al mandar correo a " + txtApprover.Text.Split("||")(0).Trim(), MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Error")
        End If


    End Sub
End Class