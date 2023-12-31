﻿Imports Microsoft.Ajax.Utilities
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
            ByVal LastUserName As String,
            ByVal LastUserEmail As String,
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
            cmd.Parameters.AddWithValue("@LastUserName", LastUserName)
            cmd.Parameters.AddWithValue("@LastUserEmail", LastUserEmail)
            cmd.Parameters.AddWithValue("@IsActive", IsActive)
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy)

            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        End Using
    End Sub

    Public Sub UpdateIsChecked(ByVal IdModelsChanges As Guid, ByVal IsChecked As Boolean)
        Update(IdModelsChanges, Guid.Empty, Guid.Empty, Guid.Empty, "", 0, "", "", "", "", IsChecked, "", 2)
    End Sub

    Public Sub UpdateApprove(ByVal IdModelsChanges As Guid, ByVal ModifiedBy As String)
        Update(IdModelsChanges, Guid.Empty, Guid.Empty, Guid.Empty, "", 0, "Aprobado", "", "", "", True, ModifiedBy, 3)
    End Sub

    Public Sub UpdateReject(ByVal IdModelsChanges As Guid, ByVal ModifiedBy As String)
        Update(IdModelsChanges, Guid.Empty, Guid.Empty, Guid.Empty, "", 0, "Rechazado", "", "", "", True, ModifiedBy, 4)
    End Sub

    Public Sub UpdateInactivate(ByVal IdModelsChanges As Guid, ByVal ChangedOnIdModelsChangesHeader As Guid, ByVal ModifiedBy As String)
        Update(IdModelsChanges, Guid.Empty, ChangedOnIdModelsChangesHeader, Guid.Empty, "", 0, "Inactivo", "", "", "", False, ModifiedBy, 5)
    End Sub

    Public Sub UpdateEdit(ByVal IdModelsChanges As Guid,
                          ByVal IdModelsChangesHeader As Guid?,
                          ByVal ChangedOnIdModelsChangesHeader As Guid?,
                          ByVal IdUnidad As Guid?,
                          ByVal Model As String,
                          ByVal Lifespan As Integer,
                          ByVal ModelChangeStatus As String,
                          ByVal LastUser As String,
                          ByVal LastUserName As String,
                          ByVal LastUserEmail As String,
                          ByVal ModifiedBy As String) 'Informacion por enviar
        Update(IdModelsChanges, IdModelsChangesHeader, ChangedOnIdModelsChangesHeader, IdUnidad, Model, Lifespan, ModelChangeStatus, LastUser, LastUserName, LastUserEmail, False, ModifiedBy, 1)
    End Sub

    Private Sub Update(
            ByVal IdModelsChanges As Guid?,
            ByVal IdModelsChangesHeader As Guid?,
            ByVal ChangedOnIdModelsChangesHeader As Guid?,
            ByVal IdUnidad As Guid?,
            ByVal Model As String,
            ByVal Lifespan As Integer,
            ByVal ModelChangeStatus As String,
            ByVal LastUser As String,
            ByVal LastUserName As String,
            ByVal LastUserEmail As String,
            ByVal IsChecked As Boolean,
            ByVal ModifiedBy As String,
            ByVal OptionU As Integer) 'no permite poner option
        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_Update"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChanges", IdModelsChanges)
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)
            If Not ChangedOnIdModelsChangesHeader.HasValue Then
                cmd.Parameters.AddWithValue("@ChangedOnIdModelsChangesHeader", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@ChangedOnIdModelsChangesHeader", ChangedOnIdModelsChangesHeader)
            End If

            If Not IdUnidad.HasValue Then
                cmd.Parameters.AddWithValue("@IdUnidad", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@IdUnidad", IdUnidad)
            End If

            If Not Model.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@Model", Model)
            Else
                cmd.Parameters.AddWithValue("@Model", DBNull.Value)
            End If

            If Lifespan Then
                cmd.Parameters.AddWithValue("@Lifespan", Lifespan)
            Else
                cmd.Parameters.AddWithValue("@Lifespan", DBNull.Value)
            End If
            If Not ModelChangeStatus.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@ModelChangeStatus", ModelChangeStatus)
            Else
                cmd.Parameters.AddWithValue("@ModelChangeStatus", DBNull.Value)
            End If
            If Not LastUser.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@LastUser", LastUser)
            Else
                cmd.Parameters.AddWithValue("@LastUser", DBNull.Value)
            End If
            If Not LastUserName.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@LastUserName", LastUserName)
            Else
                cmd.Parameters.AddWithValue("@LastUserName", DBNull.Value)
            End If
            If Not LastUserEmail.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@LastUserEmail", LastUserEmail)
            Else
                cmd.Parameters.AddWithValue("@LastUserEmail", DBNull.Value)
            End If
            If Not IsChecked = Nothing Then
                cmd.Parameters.AddWithValue("@IsChecked", IsChecked)
            Else
                cmd.Parameters.AddWithValue("@IsChecked", False)
            End If
            If Not ModifiedBy.IsNullOrWhiteSpace Then
                cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy)
            Else
                cmd.Parameters.AddWithValue("@ModifiedBy", DBNull.Value)
            End If
            cmd.Parameters.AddWithValue("@Option", OptionU)

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

    Public Function SelectByModel(ByVal Model As String) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_SelectByModel"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Model", Model)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChanges", GetType(Guid))
            result.Columns.Add("Model", GetType(String))
            result.Columns.Add("Lifespan", GetType(String))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("IdUnit", GetType(String))
            result.Columns.Add("LastUserName", GetType(String))
            result.Columns.Add("LastUser", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("IsChecked", GetType(Boolean))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()

                row("IdModelsChanges") = reader.GetGuid(0)
                row("Model") = reader.GetString(1)
                row("Lifespan") = reader.GetInt32(2)
                row("Unit") = reader.GetString(3)
                row("IdUnit") = reader.GetGuid(4)
                row("LastUserName") = reader.GetString(5)
                row("LastUser") = reader.GetString(6)
                row("ModifiedOn") = reader.GetDateTime(7).ToString("dd/MMM/yyyy")
                row("IsChecked") = reader.GetBoolean(8)

                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

    Public Function SelectByIdModelsChanges(ByVal IdModelsChanges As Guid) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_SearchById"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChanges", IdModelsChanges)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChanges", GetType(Guid))
            result.Columns.Add("Model", GetType(String))
            result.Columns.Add("Lifespan", GetType(String))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("IdUnit", GetType(String))
            result.Columns.Add("LastUserName", GetType(String))
            result.Columns.Add("LastUser", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("IsChecked", GetType(Boolean))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()

                row = result.NewRow()

                row("IdModelsChanges") = reader.GetGuid(0)
                row("Model") = reader.GetString(1)
                row("Lifespan") = reader.GetInt32(2)
                row("Unit") = reader.GetString(3)
                row("IdUnit") = reader.GetGuid(4)
                row("LastUserName") = reader.GetString(5)
                row("LastUser") = reader.GetString(6)
                row("ModifiedOn") = reader.GetDateTime(7).ToString("dd/MMM/yyyy")
                row("IsChecked") = reader.GetBoolean(8)

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
            cmd.CommandText = "spED_ED_ModelsChanges_SelectByIdModelsChangesHeader"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@IdModelsChangesHeader", IdModelsChangesHeader)

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChanges", GetType(Guid))
            result.Columns.Add("Model", GetType(String))
            result.Columns.Add("Lifespan", GetType(String))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("IdUnit", GetType(String))
            result.Columns.Add("LastUserName", GetType(String))
            result.Columns.Add("LastUser", GetType(String))
            result.Columns.Add("ModifiedOn", GetType(String))
            result.Columns.Add("IsChecked", GetType(Boolean))

            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            While reader.Read()

                row = result.NewRow()

                row("IdModelsChanges") = reader.GetGuid(0)
                row("Model") = reader.GetString(1)
                row("Lifespan") = reader.GetInt32(2)
                row("Unit") = reader.GetString(3)
                row("IdUnit") = reader.GetGuid(4)
                row("LastUserName") = reader.GetString(5)
                row("LastUser") = reader.GetString(6)
                row("ModifiedOn") = reader.GetDateTime(7).ToString("dd/MMM/yyyy")
                row("IsChecked") = reader.GetBoolean(8)

                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

    Public Function SelectByIdModelsChangesApproved(ByVal model As String, ByVal lifespan As String, ByVal idCatUnits As Guid) As DataTable
        Dim result As DataTable
        Dim row As DataRow

        Using conn As New SqlConnection(Me.dbCon)

            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "spED_ED_ModelsChanges_SelectByApproved"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Model", If(model = "", DBNull.Value, model))
            cmd.Parameters.AddWithValue("@Lifespan", If(lifespan = "", DBNull.Value, lifespan))
            cmd.Parameters.AddWithValue("@IdCatUnits", If(idCatUnits = Guid.Empty, DBNull.Value, idCatUnits))

            result = New DataTable("Result")
            result.Columns.Add("IdModelsChanges", GetType(Guid))
            result.Columns.Add("IdModelsChangesHeader", GetType(Guid))
            result.Columns.Add("IdCatUnits", GetType(Guid))
            result.Columns.Add("Model", GetType(String))
            result.Columns.Add("Lifespan", GetType(String))
            result.Columns.Add("Unit", GetType(String))
            result.Columns.Add("LastUser", GetType(String))
            result.Columns.Add("ApproverUser", GetType(String))
            result.Columns.Add("ApprovedOn", GetType(String))
            result.Columns.Add("IsChecked", GetType(Boolean))
            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            While reader.Read()

                row = result.NewRow()

                row("IdModelsChanges") = reader.GetGuid(0)
                row("IdModelsChangesHeader") = reader.GetGuid(1)
                row("IdCatUnits") = reader.GetGuid(2)
                row("Model") = reader.GetString(3)
                row("Lifespan") = reader.GetInt32(4)
                row("Unit") = reader.GetString(5)
                row("LastUser") = reader.GetString(6)
                row("ApproverUser") = reader.GetString(7)
                row("ApprovedOn") = reader.GetDateTime(8).ToString("dd/MMM/yyyy")
                row("IsChecked") = False
                result.Rows.Add(row)

            End While

            conn.Close()
            conn.Dispose()
        End Using

        Return result
    End Function

End Class
