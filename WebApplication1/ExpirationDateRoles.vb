Imports System.Web.Security
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
'Imports System.Web.SessionState


Public Class ExpirationDateRoles
    Inherits RoleProvider

    Dim clsUsers As New RolesClass
    Dim m_Profile As Security.UserProfile

    Public Sub New()
        clsUsers.ConnectionString = _
            ConfigurationManager.ConnectionStrings("AplsProd").ConnectionString


        'm_Profile = CType(HttpSessionState("UserProfile"), Security.UserProfile)
    End Sub


    Public Overrides Sub AddUsersToRoles(ByVal usernames() As String, ByVal roleNames() As String)

    End Sub

    Public Overrides Property ApplicationName() As String
        Get
            Return "Expiration Date"
        End Get
        Set(ByVal value As String)
            Throw New NotImplementedException()
        End Set
    End Property


    Public Overrides Sub CreateRole(ByVal roleName As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function DeleteRole(ByVal roleName As String, ByVal throwOnPopulatedRole As Boolean) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function FindUsersInRole(ByVal roleName As String, ByVal usernameToMatch As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetAllRoles() As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetRolesForUser(ByVal username As String) As String()
        Dim roles As String() = Nothing
        Try
            m_Profile = CType(HttpContext.Current.Session("UserProfile"), Security.UserProfile)

            roles = clsUsers.GetRolesForUsername(username:= _
                System.Text.RegularExpressions.Regex.Replace(m_Profile.UserName, "ent\\", "", RegexOptions.IgnoreCase))
        Catch ex As Exception
            roles = Nothing
        End Try
        Return roles
    End Function

    Public Overrides Function GetUsersInRole(ByVal roleName As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function IsUserInRole(ByVal username As String, ByVal roleName As String) As Boolean
        Dim blRes As Boolean = False

        Try
            m_Profile = CType(HttpContext.Current.Session("UserProfile"), Security.UserProfile)

            blRes = clsUsers.IsUserInRole( _
                System.Text.RegularExpressions.Regex.Replace(m_Profile.UserName, "ent\", "", RegexOptions.IgnoreCase) _
            , roleName)

        Catch ex As Exception

        End Try

        Return blRes
    End Function

    Public Overrides Sub RemoveUsersFromRoles(ByVal usernames() As String, ByVal roleNames() As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function RoleExists(ByVal roleName As String) As Boolean
        Throw New NotImplementedException()
    End Function

End Class
