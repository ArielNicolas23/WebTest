
Imports Microsoft.VisualBasic
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions

Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Imports System.Web.UI
Imports System.DirectoryServices

Namespace Security

    Public Structure UserProfile
        Public UserName As String
        Public UserFullName As String
        Public IsAdministrator As Boolean
        Public IsSupervisor As Boolean
        Public IsStandardUser As Boolean
        Public IsCoatingSupervisorUser As Boolean
        Public IsRestrictedUser As Boolean
        Public IsCPSUsers As Boolean
        Public IsCPSQualityUsers As Boolean
        Public IsUPNUser As Boolean

        Public ReadOnly Property IsSystemUser() As Boolean
            Get
                If IsAdministrator Or
                    IsCoatingSupervisorUser Or
                    IsRestrictedUser Or
                    IsStandardUser Or
                    IsSupervisor Or
                    IsCPSUsers Or
                    IsCPSQualityUsers Or
                    IsUPNUser Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public IsValid As Boolean
    End Structure


    Public Class Authorization

        Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String],
             ByVal lpszDomain As [String], ByVal lpszPassword As [String],
             ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer,
             ByRef phToken As IntPtr) As Boolean

        <DllImport("kernel32.dll")>
        Public Shared Function FormatMessage(ByVal dwFlags As Integer, ByRef lpSource As IntPtr,
            ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByRef lpBuffer As [String],
            ByVal nSize As Integer, ByRef Arguments As IntPtr) As Integer

        End Function

        Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

        Public Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal ExistingTokenHandle As IntPtr,
                ByVal SECURITY_IMPERSONATION_LEVEL As Integer,
                ByRef DuplicateTokenHandle As IntPtr) As Boolean

        Private Shared lastWin32Error As Integer


        Private Shared principal As WindowsPrincipal

        Private Shared profile As Security.UserProfile

        ''' <summary>
        ''' Retorna un objeto WindowsPrincipal para el usuario que fue validado con el método Logon(...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property WindowsPrincipal() As WindowsPrincipal
            Get
                Return principal
            End Get

        End Property

        Public Shared ReadOnly Property UserProfile() As UserProfile
            Get
                Return profile
            End Get
        End Property





        ''' <summary>
        ''' Autentifica un nombre de usuario y contraseña en un dominio determinado.
        ''' </summary>
        ''' <param name="userName">Nombre de usuario</param>
        ''' <param name="password">Contraseña</param>
        ''' <param name="domainName">Dominio</param>
        ''' <returns>True si el usuario es válido. False en cualquier otro caso.</returns>
        ''' <remarks></remarks>
        Public Shared Function Logon(ByVal userName As String, ByVal password As String, ByVal domainName As String) As Boolean
            Dim success As Boolean = True
            Dim tokenHandle As New IntPtr(0)

            profile.IsValid = False

            Try
                Const LOGON32_PROVIDER_DEFAULT As Integer = 0
                Const LOGON32_LOGON_INTERACTIVE As Integer = 2

                tokenHandle = IntPtr.Zero

                Dim returnValue As Boolean = LogonUser(userName, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, tokenHandle)

                If False = returnValue Then
                    lastWin32Error = Marshal.GetLastWin32Error()
                    success = False
                Else
                    Dim newId As New WindowsIdentity(tokenHandle)
                    principal = New WindowsPrincipal(newId)
                    GetUserProfile()
                End If

                If tokenHandle <> IntPtr.Zero Then
                    CloseHandle(tokenHandle)
                End If

            Catch ex As Exception
                Throw New System.Exception("Error code " + ex.Message)
            End Try

            Return success
        End Function

        ''' <summary>
        ''' Obtiene el perfil de roles del usario. También obtiene el nombre completo del usuario.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub GetUserProfile()
            Dim userName As String
            Dim firstName As String
            Dim lastName As String
            Dim administratorsRole As String
            Dim supervisorsRole As String
            Dim usersRole As String
            Dim coatingSupervisorRole As String
            Dim restrictedRole As String
            Dim cpsUserRole As String
            Dim CPSQualityUsersRole As String
            Dim upnUserRole As String

            ' Obtiene el nombre de usuario
            profile.UserName = principal.Identity.Name

            ' Obtiene el nombre completo del usuario
            'Dim User As Object
            'User = GetObject("WinNT://" + principal.Identity.Name.Replace("\", "/") + ",user")
            'profile.UserFullName = User.Fullname

            'userName = principal.Identity.Name.Trim().ToUpper().Replace("ENT\", "")
            userName = principal.Identity.Name

            firstName = GetUserInfo(userName, "givenName")
            lastName = GetUserInfo(userName, "sn")

            profile.UserFullName = lastName + ", " + firstName

            ' Mapear los roles de la aplicación con los grupos de windows asignados a
            ' la aplicación.
            administratorsRole = ConfigurationManager.AppSettings("Administrators")
            supervisorsRole = ConfigurationManager.AppSettings("Supervisors")
            usersRole = ConfigurationManager.AppSettings("StandardUsers")
            coatingSupervisorRole = ConfigurationManager.AppSettings("CoatingSupervisorUsers")
            restrictedRole = ConfigurationManager.AppSettings("Restricted")
            cpsUserRole = ConfigurationManager.AppSettings("CPSUsers")
            CPSQualityUsersRole = ConfigurationManager.AppSettings("CPSQualityUsers")
            upnUserRole = ConfigurationManager.AppSettings("UPNUsers")

            profile.IsAdministrator = principal.IsInRole(administratorsRole)
            profile.IsSupervisor = principal.IsInRole(supervisorsRole)
            profile.IsStandardUser = principal.IsInRole(usersRole)
            profile.IsCoatingSupervisorUser = principal.IsInRole(usersRole)
            profile.IsRestrictedUser = principal.IsInRole(restrictedRole)
            profile.IsCPSUsers = principal.IsInRole(cpsUserRole)
            profile.IsCPSQualityUsers = principal.IsInRole(CPSQualityUsersRole)
            profile.IsUPNUser = principal.IsInRole(upnUserRole)
            profile.IsValid = True

        End Sub


        ''' <summary>
        ''' Formate y devuelve el mensaje de error correspondiente al código de error suministrado como 
        ''' parámetro.
        ''' </summary>
        ''' <param name="errorCode">Código de error.</param>
        ''' <returns>Mensaje de error</returns>
        ''' <remarks></remarks>
        Public Shared Function GetErrorMessage(ByVal errorCode As Integer) As String
            Dim FORMAT_MESSAGE_ALLOCATE_BUFFER As Integer = &H100
            Dim FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
            Dim FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000

            Dim messageSize As Integer = 255
            Dim lpMsgBuf As String = ""
            Dim dwFlags As Integer = FORMAT_MESSAGE_ALLOCATE_BUFFER Or FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS

            Dim ptrlpSource As IntPtr = IntPtr.Zero
            Dim prtArguments As IntPtr = IntPtr.Zero

            Dim retVal As Integer = FormatMessage(dwFlags, ptrlpSource, errorCode, 0, lpMsgBuf, messageSize, prtArguments)
            If 0 = retVal Then
                Throw New System.Exception("Failed to format message for error code " + errorCode.ToString() + ". ")
            End If

            Return lpMsgBuf
        End Function

        ''' <summary>
        ''' Devuelve el mensaje de error generado cuando el usuario no es autentificado por el método Logon(...) 
        ''' </summary>
        ''' <returns>Mensaje de error</returns>
        ''' <remarks>Utilizar cuando el método Logon(...) retorne False</remarks>
        Public Shared Function GetErrorMessage() As String
            Return "Intento de acceso falló con código de error : [" + lastWin32Error.ToString() + "] " + GetErrorMessage(lastWin32Error)
        End Function

        Public Shared Function GetUserInfo(ByVal inSAM As String, ByVal inType As String) As String

            Dim ret As String = "Null"

            Dim ldapPath As String = ConfigurationManager.AppSettings("LdapPath")
            Dim directoryEntryUserName As String = ConfigurationManager.AppSettings("DirectoryEntryUserName")
            Dim directoryEntryPassword As String = ConfigurationManager.AppSettings("DirectoryEntryPassword")


            Try

                'Dim sPath As String = "LDAP://core.medtronic.com/ou=Users,ou=Tijuana,ou=MEX,ou=MIT,dc=ent,dc=core,dc=medtronic,dc=com"



                Dim SamAccount As String = Right(inSAM, Len(inSAM) - InStr(inSAM, "\"))

                'SamAccount = inSAM

                'Dim myDirectory As New DirectoryEntry(sPath, "Enterprise Admin", "Password") 'pass the user account and password for your Enterprise admin.
                'Dim mySearcher As New DirectorySearcher(myDirectory)

                'Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath)

                Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath, directoryEntryUserName, directoryEntryPassword)
                'Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath, "ENT\itdefault", "Edcrfv02")
                Dim mySearcher As New DirectorySearcher(myDirectory)

                Dim mySearchResultColl As SearchResultCollection
                Dim mySearchResult As SearchResult
                Dim myResultPropColl As ResultPropertyCollection
                Dim myResultPropValueColl As ResultPropertyValueCollection
                'Build LDAP query
                mySearcher.Filter = ("(&(objectClass=user)(samaccountname=" & SamAccount & "))")
                mySearchResultColl = mySearcher.FindAll()
                'I expect only one user from search result
                Select Case mySearchResultColl.Count
                    Case 0
                        Return ret
                        Exit Function
                    Case Is > 1
                        Return ret
                        Exit Function
                End Select

                'Get the search result from the collection
                mySearchResult = mySearchResultColl.Item(0)

                'Get the Properites, they contain the usefull info
                myResultPropColl = mySearchResult.Properties

                'displayname, mail
                'Retrieve from the properties collection the display name and email of the user
                myResultPropValueColl = myResultPropColl.Item("displayname")
                Return CStr(myResultPropValueColl.Item(0))

            Catch ex As System.Exception

                Dim errorMessage As String = ex.Message

                'do some error return here.
            End Try

            Return ret
        End Function

    End Class


    Public Class UserAD
        Public Shared Function GetAllUsers(ByVal strUserFilter As String, ByVal dt As DataTable) As DataTable
            '    Dim dt As DataTable = New DataTable
            Dim ldapPath As String = ConfigurationManager.AppSettings("LdapPath")
            Dim directoryEntryUserName As String = ConfigurationManager.AppSettings("DirectoryEntryUserName")
            Dim directoryEntryPassword As String = ConfigurationManager.AppSettings("DirectoryEntryPassword")
            Try
                Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath, directoryEntryUserName, directoryEntryPassword)

                Dim mySearcher As DirectorySearcher = New DirectorySearcher(myDirectory)
                Dim strFilter As String = "(&(objectCategory=Person)(objectClass=User)(|(cn=" + strUserFilter + "*)(sAMAccountName=" + strUserFilter + "*)))"
                mySearcher = New DirectorySearcher(strFilter)
                mySearcher.SizeLimit = 10
                mySearcher.PropertiesToLoad.Add("cn")
                mySearcher.PropertiesToLoad.Add("mail")
                mySearcher.PropertiesToLoad.Add("samaccountname")
                mySearcher.PropertiesToLoad.Add("displayname")
                Dim userLevel As DirectoryEntry
                For Each result As SearchResult In mySearcher.FindAll()
                    userLevel = result.GetDirectoryEntry()
                    Dim row As DataRow = dt.NewRow()
                    row.Item("Name") = userLevel.Properties("displayname")(0)
                    row.Item("UserName") = userLevel.Properties("samaccountname")(0)
                    row.Item("Email") = userLevel.Properties("mail")(0)

                    dt.Rows.Add(row)
                Next
            Catch ex As Exception

            End Try
            Return dt
        End Function

        Public Shared Function GetUserEmail(ByVal strUserName As String) As String
            Dim strEmail As String = String.Empty
            Dim ldapPath As String = ConfigurationManager.AppSettings("LdapPath")
            Dim directoryEntryUserName As String = ConfigurationManager.AppSettings("DirectoryEntryUserName")
            Dim directoryEntryPassword As String = ConfigurationManager.AppSettings("DirectoryEntryPassword")
            Try
                Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath, directoryEntryUserName, directoryEntryPassword)
                Dim mySearcher As DirectorySearcher = New DirectorySearcher(myDirectory)
                Dim strFilter As String = "(&(objectCategory=Person)(objectClass=User)(|(sAMAccountName=" + strUserName + "*)))"
                mySearcher = New DirectorySearcher(strFilter)
                mySearcher.PropertiesToLoad.Add("mail")

                Dim userLevel As DirectoryEntry
                For Each result As SearchResult In mySearcher.FindAll()
                    userLevel = result.GetDirectoryEntry()
                    strEmail = userLevel.Properties("mail")(0).ToString
                Next
            Catch ex As Exception

            End Try
            Return strEmail
        End Function

        Public Shared Function GetUserExists(ByVal strUserName As String, ByRef strMessage As String) As Boolean
            GetUserExists = False
            Dim ldapPath As String = ConfigurationManager.AppSettings("LdapPath")
            Dim directoryEntryUserName As String = ConfigurationManager.AppSettings("DirectoryEntryUserName")
            Dim directoryEntryPassword As String = ConfigurationManager.AppSettings("DirectoryEntryPassword")
            Try
                Dim myDirectory As DirectoryEntry = New DirectoryEntry(ldapPath, directoryEntryUserName, directoryEntryPassword)
                Dim mySearcher As DirectorySearcher = New DirectorySearcher(myDirectory)
                Dim strFilter As String = "(samAccountName=" & strUserName & ")"
                mySearcher = New DirectorySearcher(strFilter)
                mySearcher.PropertiesToLoad.Add("samAccountName")
                Dim sr As DirectoryServices.SearchResult = mySearcher.FindOne()
                If sr Is Nothing Then
                    strMessage = "No se encontro al usuario " + strUserName
                    Return False
                End If
                'Dim userLevel As DirectoryEntry
                'For Each result As SearchResult In mySearcher.FindAll()
                '    userLevel = result.GetDirectoryEntry()
                'Next
                GetUserExists = True
            Catch ex As Exception
                strMessage = ex.Message.ToString
                GetUserExists = False
            End Try
            Return GetUserExists
        End Function

        Public Shared Function ValidateUser(ByVal userName As String, ByVal password As String, ByVal domainName As String) As Boolean
            Dim success As Boolean = True
            Dim tokenHandle As New IntPtr(0)
            Dim returnValue As Boolean = False
            Try
                Const LOGON32_PROVIDER_DEFAULT As Integer = 0
                Const LOGON32_LOGON_INTERACTIVE As Integer = 2

                tokenHandle = IntPtr.Zero
                returnValue = LogonUser(userName, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, tokenHandle)
            Catch ex As Exception
                returnValue = False
            End Try
            Return returnValue
        End Function

        Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String],
             ByVal lpszDomain As [String], ByVal lpszPassword As [String],
             ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer,
             ByRef phToken As IntPtr) As Boolean

    End Class



End Namespace

