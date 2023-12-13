Public Class ewapp_login
    Inherits System.Web.UI.Page

    Dim m_Profile As Security.UserProfile

    Protected Sub BtnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLogin.Click
        LblMessage.Text = String.Empty
        If Not (TxtUserName.Text.Trim.Equals(String.Empty) Or TxtPassword.Text.Trim.Equals(String.Empty)) Then
            If Security.Authorization.Logon(TxtUserName.Text, TxtPassword.Text, "ENT") Then

                'autenticando para efectos de los providers
                'Dim memCurrentProvider As MembershipProvider
                'memCurrentProvider = Membership.Providers("AspNetADMemProv")
                'memCurrentProvider.ValidateUser(TxtUserName.Text, TxtPassword.Text)


                Session("UserProfile") = Security.Authorization.UserProfile
                m_Profile = CType(Session("UserProfile"), Security.UserProfile)

                FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.User.Identity.Name, False)
                'HttpContext.Current.User = Authorization.WindowsPrincipal

                'If m_Profile.IsSystemUser Then
                '    FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.User.Identity.Name, False)
                'Else
                '    LblMessage.Text = "El usuario no está autorizado para usar esta aplicación."
                'End If

            Else
                LblMessage.Text = Security.Authorization.GetErrorMessage()
            End If
        End If
    End Sub

End Class
