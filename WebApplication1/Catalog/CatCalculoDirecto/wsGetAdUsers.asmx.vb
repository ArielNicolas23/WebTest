Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
'<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
<ScriptService>
Public Class wsGetAdUsers
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetADUsers(prefixText As String, count As Integer) As List(Of String)
        Dim data = New DataTable()
        data.Columns.Add("Name", GetType(String))
        data.Columns.Add("UserName", GetType(String))
        data.Columns.Add("Email", GetType(String))
        data = Security.UserAD.GetAllUsers(prefixText, data)

        Dim users As List(Of String) = New List(Of String)()
        For Each activeDirectoryData As DataRow In data.Rows
            users.Add(activeDirectoryData("Name") & " || " & activeDirectoryData("UserName"))
        Next
        If users.Count <= 0 Then
            users.Add("Usuario inexistente")
        End If
        Return users

    End Function

End Class