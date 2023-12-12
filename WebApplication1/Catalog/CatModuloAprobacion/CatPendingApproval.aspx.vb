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
            PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApproverUser("martil205"))
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
        PopulateGrid(dgvPendingApproval, modelChangesHeader.SelectByApprovalStatus("martil205", ddlStatus.SelectedValue))
    End Sub

    Protected Sub dgvPendingApproval_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvPendingApproval.RowCommand
        Select Case e.CommandName
            Case "Action"
                Dim row As DataKeyArray = dgvPendingApproval.DataKeys
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim id As Guid = Guid.Parse(row(index).Value.ToString())

                'ToggleModelsChanges()
                PopulateGrid(dgvModelChanges, modelChanges.SelectByIdModelsChangesHeader(id))
        End Select
    End Sub

    Protected Sub cmdShowPending_Click(sender As Object, e As EventArgs) Handles cmdShowPending.Click

    End Sub

    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click

    End Sub

    Protected Sub cmdRejectChange_Click(sender As Object, e As EventArgs) Handles cmdRejectChange.Click

    End Sub

    Protected Sub cmdApproveChange_Click(sender As Object, e As EventArgs) Handles cmdApproveChange.Click

    End Sub
End Class