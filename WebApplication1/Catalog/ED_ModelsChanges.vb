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
            ByVal IdModelsChangesHeader As Guid,
            ByVal IdUnit As Guid,
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
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)
            cmd.Parameters.AddWithValue("@IdUnidad", IdUnit)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Lifespan", Lifespan)
            cmd.Parameters.AddWithValue("@ModelChangeStatus", ModelChangeStatus)
            cmd.Parameters.AddWithValue("@LastUser", LastUser)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub

    Public Sub Update(
            ByVal IdModelsChanges As Guid,
            ByVal IdModelsChangesHeader As Guid,
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
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)
            cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad)
            cmd.Parameters.AddWithValue("@Model", Model)
            cmd.Parameters.AddWithValue("@Lifespan", Lifespan)
            cmd.Parameters.AddWithValue("@ModelChangeStatus", ModelChangeStatus)
            cmd.Parameters.AddWithValue("@LastUser", LastUser)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
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
            conn.Close()
            conn.Dispose()
        End Using
    End Sub

    Public Function AlreadyExistModelChange(ByVal IdModelsChanges As Guid, ByVal Model As String) As Boolean
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

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

    Public Function SelectByIdModelsChangesHeader(ByVal IdModelsChangesHeader As Guid) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_SelectByIdModelsChangesHeader"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChange", GetType(Guid))
            result.Columns.Add("Model", GetType(String))
            result.Columns.Add("Lifespan", GetType(String))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("LastUser", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()

                row("IdModelsChange") = reader.GetGuid(0)
                row("Model") = reader.GetString(1)
                row("Lifespan") = reader.GetInt32(2)
                row("Unit") = reader.GetString(3)
                row("LastUser") = reader.GetString(4)
                row("ModifiedOn") = reader.GetDateTime(5).ToString("dd/MMM/yyyy")

                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function
End Class
