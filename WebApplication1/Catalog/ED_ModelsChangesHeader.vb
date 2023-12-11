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

    Public Function SelectByApproverUser(ByVal ApproverUser As String) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            'Dim rowApproverUser As String
            'Dim rowOriginUser As String
            'Dim roeApprovalStatus As String


            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_SelectByApproverUser"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("ChangeNumber", GetType(Int32))
            result.Columns.Add("OriginUser", GetType(String))
            result.Columns.Add("OriginComment", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApproverComment", GetType(String))
            result.Columns.Add("ApprovalStatus", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("Action", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdModelsChangesHeader") = reader.GetGuid(0)
                row("ChangeNumber") = reader.GetInt32(1)
                row("OriginUser") = reader.GetString(2)
                row("OriginComment") = reader.GetString(3)
                row("ApproverUser") = reader.GetString(4)
                row("ApproverComment") = reader.GetString(5)
                row("ApprovalStatus") = reader.GetString(6)
                If (reader.IsDBNull(reader.GetOrdinal("ApprovedOn"))) Then
                    row("ApprovedOn") = "-"
                Else
                    row("ApprovedOn") = reader.GetDateTime(7).ToString("dd/MMM/yyyy")
                End If
                row("ModifiedOn") = reader.GetDateTime(13).ToString("dd/MMM/yyyy")
                row("Action") = "Tomar para revisar"
                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

    Public Function SelectByApprovalStatus(ByVal User As String, ByVal ApprovalStatus As String) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_SelectByApprovalStatus"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@User", User)
            cmd.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("ChangeNumber", GetType(Int32))
            result.Columns.Add("OriginUser", GetType(String))
            result.Columns.Add("OriginComment", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApproverComment", GetType(String))
            result.Columns.Add("ApprovalStatus", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("Action", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdModelsChangesHeader") = reader.GetGuid(0)
                row("ChangeNumber") = reader.GetInt32(1)
                row("OriginUser") = reader.GetString(2)
                row("OriginComment") = reader.GetString(3)
                row("ApproverUser") = reader.GetString(4)
                row("ApproverComment") = reader.GetString(5)
                row("ApprovalStatus") = reader.GetString(6)
                If (reader.IsDBNull(reader.GetOrdinal("ApprovedOn"))) Then
                    row("ApprovedOn") = "-"
                Else
                    row("ApprovedOn") = reader.GetDateTime(7).ToString("dd/MMM/yyyy")
                End If
                row("ModifiedOn") = reader.GetDateTime(13).ToString("dd/MMM/yyyy")
                row("Action") = "Tomar para revisar"
                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function
End Class
