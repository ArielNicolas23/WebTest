Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder
Imports System.Data.SqlClient

Public Class RolesClass
    Dim _connectionString As String
    Dim _theError As String

    Dim cmd As SqlCommand
    Dim cnn As SqlConnection
    Dim prm As SqlParameter

    Public Function GetUsers() As DataTable
        If IsConnectionEmpty() Then
            Return Nothing
        End If
        Dim result As DataTable
        Dim row As DataRow
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As SqlCommand = New SqlCommand()
            cmd.CommandText = "Sec.GetUsers"
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            result = New DataTable("Result")
            result.Columns.Add("Username", GetType(String))
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            While reader.Read()
                row = result.NewRow()
                row("Username") = reader.GetString(1)
                result.Rows.Add(row)
            End While
        End Using
        Return result
    End Function
    Public Sub AddUser(ByVal UserName As String)
        If IsConnectionEmpty() Then
            Return
        End If
        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.CreateUser", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        prm = New SqlParameter("@pUserName", Data.SqlDbType.VarChar, 50)
        prm.Value = UserName
        cmd.Parameters.Add(prm)

        prm = New SqlParameter("@oStat", Data.SqlDbType.VarChar, 100, Data.ParameterDirection.Output)
        prm.Value = String.Empty
        cmd.Parameters.Add(prm)

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()

            ' obtener el parametro de salida
            _theError = cmd.Parameters("@oStat").Value.ToString()

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try

    End Sub

    Public Sub DeleteUser(ByVal UserID As Guid)
        If IsConnectionEmpty() Then
            Return
        End If
        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.DeleteUser", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        prm = New SqlParameter("@pUserID", Data.SqlDbType.UniqueIdentifier, 50)
        prm.Value = UserID
        cmd.Parameters.Add(prm)

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try
    End Sub

    Public Sub AddToRole(ByVal Roles As IEnumerable(Of Guid), ByVal UserID As Guid)
        If IsConnectionEmpty() Then
            Return
        End If

        cnn = New SqlConnection(_connectionString)
        cnn.Open()

        Try
            For Each role As Guid In Roles

                cmd = New SqlCommand("Sec.SetUserInRole", cnn)
                cmd.CommandType = Data.CommandType.StoredProcedure

                prm = New SqlParameter("@pRoleId", Data.SqlDbType.VarChar, 50)
                prm.Value = role.ToString()

                cmd.Parameters.Add(prm)

                prm = New SqlParameter("@pUserId", Data.SqlDbType.VarChar, 100, Data.ParameterDirection.Output)
                prm.Value = UserID.ToString()

                cmd.Parameters.Add(prm)
                cmd.ExecuteNonQuery()

            Next
        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try
    End Sub

    Public Sub RemoveFromRole(ByVal Roles As IEnumerable(Of Guid), ByVal UserID As Guid)
        If IsConnectionEmpty() Then
            Return
        End If

        cnn = New SqlConnection(_connectionString)
        cnn.Open()

        Try

            For Each role As Guid In Roles

                cmd = New SqlCommand("Sec.DeleteUserFromRole", cnn)
                cmd.CommandType = Data.CommandType.StoredProcedure

                prm = New SqlParameter("@pRoleId", Data.SqlDbType.VarChar, 50)
                prm.Value = role.ToString()

                cmd.Parameters.Add(prm)

                prm = New SqlParameter("@pUserId", Data.SqlDbType.VarChar, 100, Data.ParameterDirection.Output)
                prm.Value = UserID.ToString()

                cmd.Parameters.Add(prm)
                cmd.ExecuteNonQuery()

            Next
        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try
    End Sub

    Public Function GetRolesForUsername(ByVal username As String) As String()
        If IsConnectionEmpty() Then
            Return Nothing
        End If

        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.GetUserRoles_FromName", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure
        Dim dr As SqlDataReader = Nothing
        Dim resList As New List(Of String)

        prm = New SqlParameter("@pUserName", Data.SqlDbType.VarChar, 50)
        prm.Value = username

        cmd.Parameters.Add(prm)


        Try
            cnn.Open()
            dr = cmd.ExecuteReader()

            While dr.Read()
                resList.Add(dr(1).ToString())
            End While

            dr.Close()

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try

        Return resList.ToArray()
    End Function

    Public Function IsUserInRole(ByVal UserName As String, ByVal RoleName As String) As Boolean
        Dim blRes As Boolean = False

        If IsConnectionEmpty() Then
            Return False
        End If

        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.IsUserInRole", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        prm = New SqlParameter("@pUserName", Data.SqlDbType.VarChar, 50)
        prm.Value = UserName
        cmd.Parameters.Add(prm)

        prm = New SqlParameter("@pRoleName", Data.SqlDbType.VarChar, 100)
        prm.Value = RoleName
        cmd.Parameters.Add(prm)

        prm = New SqlParameter("@oIsUserInRole", Data.SqlDbType.Bit)
        prm.Direction = Data.ParameterDirection.Output
        prm.Value = String.Empty
        cmd.Parameters.Add(prm)

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()

            ' obtener el parametro de salida
            blRes = Convert.ToBoolean(cmd.Parameters("@oIsUserInRole").Value.ToString())

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try

        Return blRes
    End Function

    Public Function UserExists(ByVal userName As String) As Boolean
        Dim isUserExists As Boolean = False
        If IsConnectionEmpty() Then
            Return False
        End If

        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.UserExists", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        prm = New SqlParameter("@pUserName", Data.SqlDbType.VarChar, 50)
        prm.Value = userName
        cmd.Parameters.Add(prm)

        prm = New SqlParameter("@oStat", Data.SqlDbType.Bit)
        prm.Direction = Data.ParameterDirection.Output
        prm.Value = String.Empty
        cmd.Parameters.Add(prm)
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()

            ' obtener el parametro de salida
            isUserExists = Convert.ToBoolean(cmd.Parameters("@oStat").Value.ToString())

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try

        Return isUserExists
    End Function

    Public Function GetUserID(ByVal userName As String) As Guid
        Dim ID As New Guid

        If IsConnectionEmpty() Then
            Return Nothing
        End If

        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.GetUserID", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure
        Dim dr As SqlDataReader = Nothing

        prm = New SqlParameter("@pUserName", Data.SqlDbType.VarChar, 50)
        prm.Value = userName

        cmd.Parameters.Add(prm)
        Try
            cnn.Open()
            dr = cmd.ExecuteReader()

            While dr.Read()
                ID = New Guid(dr(0).ToString())
            End While

            dr.Close()

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try
        Return ID
    End Function

    Public Function GetRoleID(ByVal roleName As String) As Guid
        Dim roleId As New Guid
        'GetRolID

        If IsConnectionEmpty() Then
            Return Nothing
        End If

        cnn = New SqlConnection(_connectionString)
        cmd = New SqlCommand("Sec.GetRolID", cnn)
        cmd.CommandType = Data.CommandType.StoredProcedure
        Dim dr As SqlDataReader = Nothing

        prm = New SqlParameter("@pRolName", Data.SqlDbType.VarChar, 50)
        prm.Value = roleName

        cmd.Parameters.Add(prm)
        Try
            cnn.Open()
            dr = cmd.ExecuteReader()

            While dr.Read()
                roleId = New Guid(dr(0).ToString())
            End While

            dr.Close()

        Catch ex As Exception
            _theError = "Ocurrio un error mientras se ejecutaba la operacion. "
        Finally
            cmd.Dispose()
            cnn.Close()
            cnn.Dispose()
        End Try
        Return roleId
    End Function

    Function IsConnectionEmpty() As Boolean
        If String.IsNullOrEmpty(_connectionString) Then
            _theError = "La conexion no esta configurada, no es posible completar la operacion"
            Return True
        End If
        Return False
    End Function

    Public Property ConnectionString() As String
        Get
            Return _connectionString
        End Get
        Set(ByVal value As String)
            _connectionString = value
        End Set
    End Property

    Public Property TheError() As String
        Get
            Return _theError
        End Get
        Set(ByVal value As String)
            _theError = value
        End Set
    End Property

End Class

