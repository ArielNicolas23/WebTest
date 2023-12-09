Public Class CatCalculoDirecto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ModalPopupExtender1.Show()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If TextBox3.Text = "" Or
            TextBox4.Text = "" Or
            TextBox5.Text = "" Or
            TextBox6.Text = "" Then

            ModalPopupExtender1.Show()
        Else

        End If


    End Sub
End Class