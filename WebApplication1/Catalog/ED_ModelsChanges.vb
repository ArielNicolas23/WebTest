Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class ED_ModelsChanges
    Protected dbCon As String

    Public Sub New()
        dbCon = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
    End Sub

    Public Sub Insert(
            ByVal IdApprovedModelsChanges As Guid,
            ByVal IdUnidad As Guid,
            ByVal Model As String,
            ByVal Lifespan As Integer,
            ByVal ModelChangeStatus As String,
            ByVal LastUser As String,
            ByVal IsActive As Boolean,
            ByVal CreatedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_Insert"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdApprovedModelsChanges", IdApprovedModelsChanges)
            cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Lifespan", Lifespan)
            cmd.Parameters.AddWithValue("@ModelChangeStatus", ModelChangeStatus)
            cmd.Parameters.AddWithValue("@LastUser", LastUser)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Update(
            ByVal IdModelsChanges As Guid,
            ByVal IdApprovedModelsChanges As Guid,
            ByVal IdUnidad As Guid,
            ByVal Model As String,
            ByVal Lifespan As Integer,
            ByVal ModelChangeStatus As String,
            ByVal LastUser As String,
            ByVal IsActive As Boolean,
            ByVal ModifiedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChanges", IdModelsChanges)
            cmd.Parameters.AddWithValue("@IdApprovedModelsChanges", IdApprovedModelsChanges)
            cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Lifespan", Lifespan)
            cmd.Parameters.AddWithValue("@ModelChangeStatus", ModelChangeStatus)
            cmd.Parameters.AddWithValue("@LastUser", LastUser)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(ByVal IdModelsChanges As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChanges", IdModelsChanges)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function AlreadyExistUnit(ByVal IdModelsChanges As Guid, ByVal Model As String) As Boolean
        Dim result As Boolean

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_SearchByModel"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChanges", IdModelsChanges)
            cmd.Parameters.AddWithValue("@Model", Model)

            conn.Open()

            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If count > 0 Then
                result = True
            Else
                result = False
            End If
        End Using

        Return result
    End Function
End Class
