Imports System.Data
Imports System.IO
'Imports Microsoft.Office.Interop
Imports System.Web.Services
'Imports System.Runtime.InteropServices
'Imports DocumentFormat.OpenXml.Packaging
'Imports DocumentFormat.OpenXml.Spreadsheet
Imports System.Data.OleDb
Imports WebApplication1.Global_asax
Imports WebApplication1.Security

Public Class CatCalculoDirecto
    Inherits System.Web.UI.Page
    Dim m_Profile As Security.UserProfile
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ModalPopupExtender1.Show()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If (Security.UserAD.GetUserExists(TextBox3.Text, "")) Then
            Dim res As String = Security.UserAD.GetUserEmail(TextBox3.Text)
            TextBox6.Text = res
        Else
            TextBox6.Text = "No se encontro el usuario"
            Return
        End If
        If TextBox3.Text = "" Or
            TextBox4.Text = "" Or
            TextBox5.Text = "" Or
            TextBox6.Text = "" Then

            ModalPopupExtender1.Show()
        Else

        End If
        Dim a As New ModuloGeneral.ModuloGeneralEmail

    End Sub
End Class