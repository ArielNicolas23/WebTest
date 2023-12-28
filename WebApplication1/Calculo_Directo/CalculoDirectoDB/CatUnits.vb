Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class CatUnits
    Protected dbCon As String

    Public Sub New()
        dbCon = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
    End Sub

    Public Sub Insert(
            ByVal Unit As String,
            ByVal UnitQty As String,
            ByVal IsActive As Boolean,
            ByVal CreatedBy As String,
            ByVal UnitValue As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_Insert"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Unit", Unit)
            cmd.Parameters.AddWithValue("@UnitQty", UnitQty)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@UnitValue", UnitValue)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Update(
            ByVal IdUnit As Guid,
            ByVal Unit As String,
            ByVal UnitQty As String,
            ByVal IsActive As Boolean,
            ByVal CreatedBy As String,
            ByVal UnitValue As String)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatUnits", IdUnit)
            cmd.Parameters.AddWithValue("@Unit", Unit)
            cmd.Parameters.AddWithValue("@UnitQty", UnitQty)
            cmd.Parameters.AddWithValue("@ModifiedBy", CreatedBy)
            cmd.Parameters.AddWithValue("@UnitValue", UnitValue)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(ByVal IdUnit As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatUnits", IdUnit)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function SelectOne(ByVal IdCatUnits As Guid) As DataTable

        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_Select"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatUnits", IdCatUnits)

            result = New DataTable("Result")
            result.Columns.Add("IdUnit", GetType(Guid))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("UnitQty", GetType(String))
            result.Columns.Add("IsActive", GetType(Boolean))
            result.Columns.Add("UnitValue", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdUnit") = reader.GetGuid(0)
                row("Unit") = reader.GetString(1)
                row("UnitQty") = reader.GetInt16(2)
                row("IsActive") = reader.GetBoolean(3)
                row("UnitValue") = reader.ToString(9)

                result.Rows.Add(row)

            End While

        End Using

        Return result
    End Function

    Public Function SelectAll(
            ByVal Unit As String,
            ByVal IsSearch As Boolean) As DataTable

        Dim result As DataTable
        Dim row As DataRow


        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_SelectAll"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Unit", Unit)
            cmd.Parameters.AddWithValue("@IsSearch", IsSearch)

            result = New DataTable("Result")

            result.Columns.Add("IdUnit", GetType(Guid))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("UnitQty", GetType(String))
            result.Columns.Add("IsActive", GetType(Boolean))
            result.Columns.Add("UnitValue", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()

                row("IdUnit") = reader.GetGuid(0)
                row("Unit") = reader.GetString(1)
                row("UnitQty") = reader.GetInt16(2)
                row("IsActive") = reader.GetBoolean(3)
                row("UnitValue") = reader.GetString(9)

                result.Rows.Add(row)

            End While

        End Using

        Return result
    End Function

    Public Function AlreadyExistUnit(ByVal idCatUnits As Guid, ByVal unit As String) As Boolean

        Dim result As Boolean

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_CatUnits_SearchByUnit"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdCatUnits", idCatUnits)
            cmd.Parameters.AddWithValue("@Unit", unit)

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