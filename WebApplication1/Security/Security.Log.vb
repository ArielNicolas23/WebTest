Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Namespace Security

    Public Class ApplicationLog

        ''' <summary>
        ''' Enumeración basada en la tabla [tblals-Events]
        ''' SI SE AGREGA UN NUEVO TIPO DE EVENTO, AGREGARLO EN LA TABLA ESPECIFICADA
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EventType
            Other = 0
            Login = 1
            Logoff = 2
            Screenin = 3
            Execute = 4
            Insert = 5
            Update = 6
        End Enum

        ''' <summary>
        ''' Registra los movimientos/acciones importantes del usuario en la aplicación
        ''' </summary>
        ''' <param name="userName">Nombre de usuario del sistema APLS</param>
        ''' <param name="eventType">Tipo de evento (login, logoff, screenin)</param>
        ''' <param name="details">Descripción del evento en particular</param>
        ''' <returns>'True' si la función se ejecutó sin problemas. 'False' si la función se ejecutó con problemas.</returns>
        ''' <remarks></remarks>
        Public Shared Function LogEvent(ByVal userName As String,
                                        ByVal eventType As EventType,
                                        ByVal details As String) As Boolean
            Return LogEvent("Expiration Date version 8.0", userName, eventType, details, Environment.MachineName, Environment.UserName)
        End Function

        ''' <summary>
        ''' Registra los movimientos/acciones importantes del usuario en la aplicación
        ''' </summary>
        ''' <param name="source">Nombre de la aplicación</param>
        ''' <param name="userName">Nombre de usuario del sistema APLS</param>
        ''' <param name="eventType">Tipo de evento (login, logoff, screenin)</param>
        ''' <param name="details">Descripción del evento en particular</param>
        ''' <returns>'True' si la función se ejecutó sin problemas. 'False' si la función se ejecutó con problemas.</returns>
        ''' <remarks></remarks>
        Public Shared Function LogEvent(ByVal source As String,
                                        ByVal userName As String,
                                        ByVal eventType As EventType,
                                        ByVal details As String) As Boolean
            Return LogEvent(source, userName, eventType, details, Environment.MachineName, Environment.UserName)
        End Function


        ''' <summary>
        ''' Registra los movimientos/acciones importantes del usuario en la aplicación
        ''' </summary>
        ''' <param name="source">Nombre de la aplicación</param>
        ''' <param name="userName">Nombre de usuario del sistema APLS</param>
        ''' <param name="eventType">Tipo de evento (login, logoff, screenin)</param>
        ''' <param name="details">Descripción del evento en particular</param>
        ''' <param name="workStation">Estación de trabajo</param>
        ''' <param name="windowsUserName">Nombre de usuario de windows</param>
        ''' <returns>'True' si la función se ejecutó sin problemas. 'False' si la función se ejecutó con problemas.</returns>
        ''' <remarks></remarks>
        Public Shared Function LogEvent(ByVal source As String,
                                        ByVal userName As String,
                                        ByVal eventType As EventType,
                                        ByVal details As String,
                                        ByVal workStation As String,
                                        ByVal windowsUserName As String) As Boolean
            Dim ret As Boolean = False

            Dim m_Apls As String
            m_Apls = ConfigurationManager.ConnectionStrings("AplsProd").ConnectionString

            Dim sqlstr As String
            sqlstr =
            "insert into [tblals-EventLog] " +
            "(Source, UserName, EventType, EventDate, Details, WorkStation, WindowsUserName) " +
            "values " +
            "(@Source, @UserName, @EventType, getdate(), @Details, @WorkStation, @WindowsUserName)"

            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@Source", source)
            command.Parameters.AddWithValue("@UserName", userName)
            command.Parameters.AddWithValue("@EventType", CType(eventType, Int32))
            command.Parameters.AddWithValue("@Details", details.Substring(0, CInt(IIf(details.Length > 200, 199, details.Length))))
            command.Parameters.AddWithValue("@WorkStation", workStation)
            command.Parameters.AddWithValue("@WindowsUserName", windowsUserName)

            Try
                connection.Open()
                command.ExecuteNonQuery()
                ret = True
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
            Return ret
        End Function
    End Class


End Namespace