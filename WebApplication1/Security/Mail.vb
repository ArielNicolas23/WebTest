Imports Microsoft.VisualBasic
Imports System.Net.Mail
Imports System.Reflection
Imports System.IO
Imports System.Web.Hosting
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq


Public Class ModuloGeneralEmail
    Public Function ConstructEmail(ByVal data As ConstructInfo) As Boolean
        Dim mailInfo As New MailInfo
        Dim email As New ModuloGeneralEmail
        Dim emailData As New MailData
        Dim strMessage As String = String.Empty

        mailInfo = emailData.EmailData_Get(data.EmailType)
        mailInfo.mSendTo = Security.UserAD.GetUserEmail(data.UserName)
        mailInfo.mMessage = mailInfo.mMessage.Replace("{link}", "Test")
        If data.EmailType = "CambiosPendientes" Or data.EmailType = "CambiosRechazados" Then
            mailInfo.mMessage = mailInfo.mMessage.Replace("{Comments}", data.Comment)
        End If

        Dim s = SendEmail(mailInfo, strMessage)
        Return s
    End Function

    Public Function SendEmail(ByVal info As MailInfo, ByRef message As String) As Boolean
        Dim returnValue As Boolean = False
        Try
            Dim smtpClient As SmtpClient = New SmtpClient(ConfigurationManager.AppSettings("MailServer"))
            Dim mailMessage As MailMessage = New MailMessage()
            mailMessage.From = New MailAddress(ConfigurationManager.AppSettings("EmailFrom"))
            mailMessage.Subject = info.mSubject
            Dim htmlBody As String = GetHtmlBody(info)
            Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(htmlBody, Nothing, "text/html")
            Dim imageResource As LinkedResource = New LinkedResource(HostingEnvironment.MapPath("~/Images/mailHeader.jpg"))
            imageResource.ContentId = "HDIImage"
            htmlView.LinkedResources.Add(imageResource)
            mailMessage.AlternateViews.Add(htmlView)
            mailMessage.IsBodyHtml = True
            mailMessage.To.Add(info.mSendTo)
            smtpClient.Send(mailMessage)
            returnValue = True
            message = "Correo enviado"
        Catch ex As Exception
            message = String.Concat("El correo no pudo ser enviado", ex.Message)
            returnValue = False
        End Try
        Return returnValue
    End Function

    Public Function GetHtmlBody(ByVal info As MailInfo) As String
        Dim body As String = String.Empty
        Dim strPath As String = HostingEnvironment.MapPath("~/Catalog/CatCalculoDirecto/htmlMailTemplate.htm")
        Dim currentAssembly As Assembly = Assembly.GetEntryAssembly()
        currentAssembly = Assembly.GetCallingAssembly()
        Dim targetValue As Object() = currentAssembly.GetCustomAttributes(GetType(AssemblyCompanyAttribute), inherit:=True)
        targetValue = currentAssembly.GetCustomAttributes(GetType(AssemblyTitleAttribute), inherit:=True)

        Using reader As StreamReader = New StreamReader(strPath)
            body = reader.ReadToEnd()
        End Using

        body = body.Replace("{title}", info.mTitle)
        body = body.Replace("{message}", info.mMessage)
        body = body.Replace("{year}", DateTime.Today.Year.ToString())
        body = body.Replace("{appName}", (CType(targetValue(0), AssemblyTitleAttribute)).Title)
        Return body
    End Function
End Class

Public Class MailInfo

    Private Title As String
    Public Property mTitle() As String
        Get
            Return Title
        End Get
        Set(ByVal value As String)
            Title = value
        End Set
    End Property

    Private Message As String
    Public Property mMessage() As String
        Get
            Return Message
        End Get
        Set(ByVal value As String)
            Message = value
        End Set
    End Property

    Private Subject As String
    Public Property mSubject() As String
        Get
            Return Subject
        End Get
        Set(ByVal value As String)
            Subject = value
        End Set
    End Property

    Private SendTo As String
    Public Property mSendTo() As String
        Get
            Return SendTo
        End Get
        Set(ByVal value As String)
            SendTo = value
        End Set
    End Property
End Class

Public Class ConstructInfo
    Private mEmailType As String
    Public Property EmailType() As String
        Get
            Return mEmailType
        End Get
        Set(ByVal value As String)
            mEmailType = value
        End Set
    End Property

    Private mUserName As String
    Public Property UserName() As String
        Get
            Return mUserName
        End Get
        Set(ByVal value As String)
            mUserName = value
        End Set
    End Property

    Private mComment As String
    Public Property Comment() As String
        Get
            Return mComment
        End Get
        Set(ByVal value As String)
            mComment = value
        End Set
    End Property

    Private mChangeNo As String
    Public Property ChangeNo() As String
        Get
            Return mChangeNo
        End Get
        Set(ByVal value As String)
            mChangeNo = value
        End Set
    End Property
    Private mLink As String
    Public Property Link() As String
        Get
            Return mLink
        End Get
        Set(ByVal value As String)
            mLink = value
        End Set
    End Property

End Class
Public Class MailData
    Protected m_Apls As String
    Dim _TheError As String

    Public Sub New()
        m_Apls = ConfigurationManager.ConnectionStrings("AplsProd").ConnectionString
    End Sub

    Public Function EmailData_Get(ByVal type As String) As MailInfo
        Dim mailInfo As New MailInfo
        Dim sqlCmd As New SqlCommand
        Dim dsResult As New DataSet
        Try
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.CommandText = "spED_EmailData_Get"
            sqlCmd.Parameters.AddWithValue("@Type", type)
            dsResult = ExecuteQuerty(sqlCmd)
            If dsResult.Tables.Count > 0 Then
                mailInfo = PopulateEmailData(dsResult.Tables(0))
                Return mailInfo
            Else
                _TheError = "No se pudo recuperar la información del correo."
                Return Nothing
            End If
        Catch ex As Exception
            _TheError = FormatErrorMessage(ex)
            Return Nothing
        Finally
            sqlCmd.Connection.Close()
            sqlCmd.Connection.Dispose()
        End Try
    End Function

    Private Function ExecuteQuerty(ByVal sqlCmd As SqlCommand) As DataSet
        Dim ds As New DataSet()
        Dim sqlConn As New SqlConnection
        Dim sqlDa As New SqlDataAdapter
        Try
            sqlConn = New SqlConnection(m_Apls)
            sqlCmd.Connection = sqlConn
            sqlCmd.Connection.Open()
            sqlDa = New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(ds)
        Catch ex As Exception
            _TheError = FormatErrorMessage(ex)
            ds.Tables.Add(0)
        Finally
            sqlCmd.Connection.Close()
            sqlCmd.Connection.Dispose()
        End Try
        Return ds
    End Function

    Private Function FormatErrorMessage(ByVal ex As Exception) As String
        Dim returnMessage As String = String.Empty
        Dim _message As String = String.Empty
        Dim _messageInner As String = String.Empty
        Dim _stackTrace As String = String.Empty

        If ex.Message IsNot Nothing Then
            _message = ex.Message
        End If
        If ex.InnerException IsNot Nothing Then
            If ex.InnerException.Message IsNot Nothing Then
                _messageInner = ex.InnerException.Message
            End If
        End If
        If ex.StackTrace IsNot Nothing Then
            _stackTrace = ex.StackTrace
        End If
        returnMessage = (Convert.ToString("" & Convert.ToString(" Error:[")) & _message) + "]" & vbCr & vbLf + (If(_messageInner.Length > 0, (Convert.ToString("InnerException:[") & _messageInner) + "]", "")) + vbCr & vbLf + (If(_stackTrace.Length > 0, (Convert.ToString("Source:[") & _stackTrace) + "]", ""))
        Return returnMessage
    End Function

    Private Function PopulateEmailData(ByVal dt As DataTable) As MailInfo
        Dim mailInfo As New MailInfo
        Try
            mailInfo = dt.AsEnumerable().[Select](Function(row) New MailInfo With {.mMessage = row.Field(Of String)("EmailMessage"), .mSubject = row.Field(Of String)("EmailSubject"), .mTitle = row.Field(Of String)("EmailTitle")}).FirstOrDefault()
        Catch ex As Exception
            _TheError = FormatErrorMessage(ex)
        End Try
        Return mailInfo
    End Function

End Class