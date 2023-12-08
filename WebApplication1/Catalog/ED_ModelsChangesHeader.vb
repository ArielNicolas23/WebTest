Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class ED_ModelsChangesHeader
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

        Dim IdModelsChangesHeader As Guid
        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Insert"
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
            IdModelsChangesHeader = CType(cmd.ExecuteScalar(), Guid)
            conn.Close()
            conn.Dispose()
        End Using

        Return IdModelsChangesHeader
    End Function

    Public Sub Update(
            ByVal IdModelsChangesHeader As Guid,
            ByVal ChangeNumber As Integer,
            ByVal OriginUser As String,
            ByVal OriginComment As String,
            ByVal ApproverUser As String,
            ByVal ApproverComment As String,
            ByVal ApprovalStatus As String,
            ByVal ModifiedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@@IdModelsChangesHeader", IdModelsChangesHeader)
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

    Public Sub Delete(ByVal IdModelsChangesHeader As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@@IdModelsChangesHeader", IdModelsChangesHeader)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub
End Class
