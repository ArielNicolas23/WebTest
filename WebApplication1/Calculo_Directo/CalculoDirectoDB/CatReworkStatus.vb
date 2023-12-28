Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class CatReworkStatus
    Protected dbCon As String

    Public Sub New()
        dbCon = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString

    End Sub

    Public Sub Insert(
            ByVal SAPStatus As String,
            ByVal IsRework As Boolean,
            ByVal IsActive As Boolean,
            ByVal ModifiedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_Insert"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@SAPStatus", SAPStatus)
            cmd.Parameters.AddWithValue("@IsRework", IsRework)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", ModifiedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Update(
            ByVal IdCatReworkStatus As Guid,
            ByVal SAPStatus As String,
            ByVal IsRework As Boolean,
            ByVal ModifiedBy As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatReworkStatus", IdCatReworkStatus)
            cmd.Parameters.AddWithValue("@SAPStatus", SAPStatus)
            cmd.Parameters.AddWithValue("@IsRework", IsRework)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(ByVal IdCatReworkStatus As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatReworkStatus", IdCatReworkStatus)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function SelectOne(ByVal IdCatUnits As Guid) As DataTable

        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_Select"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatReworkStatus", IdCatUnits)


            result = New DataTable("Result")
            result.Columns.Add("IdCatReworkStatus", GetType(Guid))
            result.Columns.Add("SAPStatus", GetType(String))
            result.Columns.Add("IsRework", GetType(String))
            result.Columns.Add("IsActive", GetType(Boolean))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdCatReworkStatus") = reader.GetGuid(0)
                row("SAPStatus") = reader.GetString(1)
                row("IsRework") = reader.GetBoolean(2)
                row("IsActive") = reader.GetBoolean(3)

                result.Rows.Add(row)

            End While

        End Using

        Return result
    End Function

    Public Function SelectAll(
            ByVal SAPStatus As String,
            ByVal IsRework As Boolean,
            ByVal IsSearch As Boolean) As DataTable

        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_SelectAll"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@SAPStatus", SAPStatus)
            cmd.Parameters.AddWithValue("@IsRework", IsRework)
            cmd.Parameters.AddWithValue("@IsSearch", IsSearch)


            result = New DataTable("Result")

            result.Columns.Add("IdCatReworkStatus", GetType(Guid))
            result.Columns.Add("SAPStatus", GetType(String))
            result.Columns.Add("IsRework", GetType(String))
            result.Columns.Add("IsActive", GetType(Boolean))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()

                row("IdCatReworkStatus") = reader.GetGuid(0)
                row("SAPStatus") = reader.GetString(1)
                row("IsRework") = reader.GetBoolean(2)
                row("IsActive") = reader.GetBoolean(3)

                result.Rows.Add(row)

            End While
        End Using

        Return result
    End Function

    Public Function AlreadyExistSAPStatus(ByVal idCatReworkStatus As Guid, ByVal SAPStatus As String) As Boolean

        Dim result As Boolean

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatReworkStatus_SearchBySAPStatus"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatReworkStatus", idCatReworkStatus)
            cmd.Parameters.AddWithValue("@SAPStatus", SAPStatus)

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
