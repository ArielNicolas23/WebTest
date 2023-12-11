Imports System.Net
Imports System.Web.Http

Namespace Catalog
    Public Class adController
        Inherits ApiController

        ' GET: api/ad
        Public Function GetValues() As IEnumerable(Of String)

            Dim data = New DataTable()
            data.Columns.Add("Name", GetType(String))
            data.Columns.Add("UserName", GetType(String))
            data.Columns.Add("Email", GetType(String))
            data = Security.UserAD.GetAllUsers("alvara", data)

            Return data.AsEnumerable
        End Function

        ' GET: api/ad/5
        Public Function GetValue(ByVal id As Integer) As String
            Return "value"
        End Function

        ' POST: api/ad
        Public Sub PostValue(<FromBody()> ByVal value As String)

        End Sub

        ' PUT: api/ad/5
        Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        End Sub

        ' DELETE: api/ad/5
        Public Sub DeleteValue(ByVal id As Integer)

        End Sub
    End Class
End Namespace