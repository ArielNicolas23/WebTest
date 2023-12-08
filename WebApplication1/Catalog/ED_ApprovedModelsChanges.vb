Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class ED_ApprovedModelsChanges
    Protected dbCon As String

    Public Sub New()
        dbCon = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
    End Sub

    Public Function Insert(
            ByVal ChangeNumber As Integer,
            ByVal OriginUser As String,
            ByVal OriginComment As String,
            ByVal ApproverUser As String,
            ByVal ApprovalStatus As String,
            ByVal IsActive As Boolean,
            ByVal CreatedBy As String)

        Dim idApprovedModelsChanges As Guid
        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ApprovedModelsChanges_Insert"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ChangeNumber", ChangeNumber)
            cmd.Parameters.AddWithValue("@OriginUser", OriginUser)
            cmd.Parameters.AddWithValue("@OriginComment", OriginComment)
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)
            cmd.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            conn.Open()
            idApprovedModelsChanges = CType(cmd.ExecuteScalar(), Guid)
            conn.Close()
            conn.Dispose()
        End Using

        Return idApprovedModelsChanges
    End Function

    Public Sub Update(
            ByVal IdApprovedModelsChanges As Guid,
            ByVal ChangeNumber As Integer,
            ByVal OriginUser As String,
            ByVal OriginComment As String,
            ByVal ApproverUser As String,
            ByVal ApproverComment As String,
            ByVal ApprovalStatus As String,
            ByVal ModifiedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ApprovedModelsChanges_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdApprovedModelsChanges", IdApprovedModelsChanges)
            cmd.Parameters.AddWithValue("@ChangeNumber", ChangeNumber)
            cmd.Parameters.AddWithValue("@OriginUser", OriginUser)
            cmd.Parameters.AddWithValue("@OriginComment", OriginComment)
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)
            cmd.Parameters.AddWithValue("@ApproverComment", ApproverComment)
            cmd.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus)
            cmd.Parameters.AddWithValue("@@ModifiedBy", ModifiedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub

    Public Sub Delete(ByVal IdApprovedModelsChanges As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ApprovedModelsChanges_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdApprovedModelsChanges", IdApprovedModelsChanges)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub
End Class
