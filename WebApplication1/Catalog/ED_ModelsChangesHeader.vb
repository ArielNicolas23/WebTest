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
            ByVal OriginUser As String,
            ByVal OriginName As String,
            ByVal OriginEmail As String,
            ByVal OriginComment As String,
            ByVal ApproverUser As String,
            ByVal ApproverName As String,
            ByVal ApproverEmail As String,
            ByVal ApprovalStatus As String,
            ByVal IsActive As Boolean,
            ByVal CreatedBy As String)

        Dim IdModelsChangesHeader As Guid
        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Insert"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@OriginUser", OriginUser)
            cmd.Parameters.AddWithValue("@OriginName", OriginName)
            cmd.Parameters.AddWithValue("@OriginEmail", OriginEmail)
            cmd.Parameters.AddWithValue("@OriginComment", OriginComment)
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)
            cmd.Parameters.AddWithValue("@ApproverName", ApproverName)
            cmd.Parameters.AddWithValue("@ApproverEmail", ApproverEmail)
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

    Private Sub Update(
            ByVal IdModelsChangesHeader As Guid,
            ByVal OriginUser As String,
            ByVal OriginName As String,
            ByVal OriginComment As String,
            ByVal OriginEmail As String,
            ByVal ApproverUser As String,
            ByVal ApproverName As String,
            ByVal ApproverEmail As String,
            ByVal ApproverComment As String,
            ByVal ApprovalStatus As String,
            ByVal ModifiedBy As String,
            ByVal OptionU As Integer) 'no permite poner option

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)
            cmd.Parameters.AddWithValue("@OriginUser", OriginUser)
            cmd.Parameters.AddWithValue("@OriginName", OriginName)
            cmd.Parameters.AddWithValue("@OriginEmail", OriginEmail)
            cmd.Parameters.AddWithValue("@OriginComment", OriginComment)
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)
            cmd.Parameters.AddWithValue("@ApproverName", ApproverName)
            cmd.Parameters.AddWithValue("@ApproverEmail", ApproverEmail)
            cmd.Parameters.AddWithValue("@ApproverComment", ApproverComment)
            cmd.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus)
            cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            cmd.Parameters.AddWithValue("@Option", OptionU)
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub

    Public Sub UpdateApprovalStatus(ByVal IdModelsChangesHeader As Guid, ByVal ApprovalStatus As String, ByVal ModifiedBy As String)
        Update(IdModelsChangesHeader, "", "", "", "", "", "", "", "", ApprovalStatus, ModifiedBy, 2)
    End Sub

    Public Sub UpdateApproveOrReject(ByVal IdModelsChangesHeader As Guid, ByVal ApproverComment As String, ByVal ApprovalStatus As String, ByVal ModifiedBy As String)
        Update(IdModelsChangesHeader, "", "", "", "", "", "", "", ApproverComment, ApprovalStatus, ModifiedBy, 3)
    End Sub

    Public Sub Delete(ByVal IdModelsChangesHeader As Guid)

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_Delete"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)

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

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_SelectByApproverUser"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ApproverUser", ApproverUser)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("ChangeNumber", GetType(String))
            result.Columns.Add("OriginName", GetType(String))
            result.Columns.Add("OriginUser", GetType(String))
            result.Columns.Add("OriginComment", GetType(String))
            result.Columns.Add("ApproverName", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApproverComment", GetType(String))
            result.Columns.Add("ApprovalStatus", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("Action", GetType(String))
            result.Columns.Add("OriginEmail", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdModelsChangesHeader") = reader.GetGuid(0)
                row("ChangeNumber") = reader.GetInt32(1).ToString("D4")
                row("OriginName") = reader.GetString(2)
                row("OriginUser") = reader.GetString(3)
                row("OriginComment") = reader.GetString(4)
                row("ApproverName") = reader.GetString(5)
                row("ApproverUser") = reader.GetString(6)
                row("ApproverComment") = reader.GetString(7)
                row("ApprovalStatus") = reader.GetString(8)

                If (reader.IsDBNull(reader.GetOrdinal("ApprovedOn"))) Then
                    row("ApprovedOn") = "-"
                Else
                    row("ApprovedOn") = reader.GetDateTime(9).ToString("dd/MMM/yyyy")
                End If

                row("ModifiedOn") = reader.GetDateTime(10).ToString("dd/MMM/yyyy")
                row("OriginEmail") = reader.GetString(11)

                Select Case row("ApprovalStatus")
                    Case "Pendiente"
                        row("Action") = "Tomar para revisar"
                    Case "En Revisión"
                        row("Action") = "Liberar"
                    Case Else
                        row("Action") = ""
                End Select

                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

    Public Function SelectByApprovalStatus(
            ByVal User As String,
            Optional ByVal ApprovalStatus As String = Nothing,
            Optional ByVal UserRole As String = Nothing,
            Optional ByVal CreatedOn As Date = Nothing,
            Optional ByVal CreatedOnTo As Date = Nothing,
            Optional ByVal ApprovedOn As Date = Nothing,
            Optional ByVal ApprovedOnTo As Date = Nothing) As DataTable

        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChangesHeader_SelectByApprovalStatus"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@User", User)
            cmd.Parameters.AddWithValue("@ApprovalStatus", If(ApprovalStatus = "Todos", DBNull.Value, ApprovalStatus))
            cmd.Parameters.AddWithValue("@UserRole", If(UserRole = "Ambos", DBNull.Value, UserRole))

            cmd.Parameters.AddWithValue("@CreatedOn", If(CreatedOn = DateTime.MinValue, DBNull.Value, CreatedOn))
            cmd.Parameters.AddWithValue("@CreatedOnTo", If(CreatedOnTo = DateTime.MinValue, DBNull.Value, CreatedOnTo))
            cmd.Parameters.AddWithValue("@ApprovedOn", If(ApprovedOn = DateTime.MinValue, DBNull.Value, ApprovedOn))
            cmd.Parameters.AddWithValue("@ApprovedOnTo", If(ApprovedOnTo = DateTime.MinValue, DBNull.Value, ApprovedOnTo))


            result = New DataTable("Result")
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("ChangeNumber", GetType(String))
            result.Columns.Add("OriginName", GetType(String))
            result.Columns.Add("OriginUser", GetType(String))
            result.Columns.Add("OriginComment", GetType(String))
            result.Columns.Add("ApproverName", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApproverComment", GetType(String))
            result.Columns.Add("ApprovalStatus", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("Action", GetType(String))
            result.Columns.Add("OriginEmail", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdModelsChangesHeader") = reader.GetGuid(0)
                row("ChangeNumber") = reader.GetInt32(1).ToString("D4")
                row("OriginName") = reader.GetString(2)
                row("OriginUser") = reader.GetString(3)
                row("OriginComment") = reader.GetString(4)
                row("ApproverName") = reader.GetString(5)
                row("ApproverUser") = reader.GetString(6)
                row("ApproverComment") = reader.GetString(7)
                row("ApprovalStatus") = reader.GetString(8)

                If (reader.IsDBNull(reader.GetOrdinal("ApprovedOn"))) Then
                    row("ApprovedOn") = "-"
                Else
                    row("ApprovedOn") = reader.GetDateTime(9).ToString("dd/MMM/yyyy")
                End If

                row("ModifiedOn") = reader.GetDateTime(10).ToString("dd/MMM/yyyy")
                row("OriginEmail") = reader.GetString(11)

                If (row("ApproverUser") = User) Then
                    Select Case row("ApprovalStatus")
                        Case "Pendiente"
                            row("Action") = "Tomar para revisar"
                        Case "En Revisión"
                            row("Action") = "Liberar"
                        Case Else
                            row("Action") = ""
                    End Select
                Else
                    row("Action") = ""
                End If

                result.Rows.Add(row)

            End While

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
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("ChangeNumber", GetType(String))
            result.Columns.Add("OriginName", GetType(String))
            result.Columns.Add("OriginUser", GetType(String))
            result.Columns.Add("OriginComment", GetType(String))
            result.Columns.Add("ApproverName", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApproverComment", GetType(String))
            result.Columns.Add("ApprovalStatus", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("Action", GetType(String))
            result.Columns.Add("OriginEmail", GetType(String))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()
                row("IdModelsChangesHeader") = reader.GetGuid(0)
                row("ChangeNumber") = reader.GetInt32(1).ToString("D4")
                row("OriginName") = reader.GetString(2)
                row("OriginUser") = reader.GetString(3)
                row("OriginComment") = reader.GetString(4)
                row("ApproverName") = reader.GetString(5)
                row("ApproverUser") = reader.GetString(6)
                row("ApproverComment") = reader.GetString(7)
                row("ApprovalStatus") = reader.GetString(8)

                If (reader.IsDBNull(reader.GetOrdinal("ApprovedOn"))) Then
                    row("ApprovedOn") = "-"
                Else
                    row("ApprovedOn") = reader.GetDateTime(9).ToString("dd/MMM/yyyy")
                End If

                row("ModifiedOn") = reader.GetDateTime(10).ToString("dd/MMM/yyyy")
                row("OriginEmail") = reader.GetString(11)

                row("Action") = ""

                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function
End Class
