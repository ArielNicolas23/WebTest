Imports System.Drawing
Imports System.Net
Imports System.Web.WebPages

Public Class CalculoDirecto

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim modelschanges = New ED_ModelsChanges
        Dim catreworkorders = New CatReworkOrders
        Dim catUnits = New CatUnits
        Dim dtOrder = catreworkorders.SelectByOrder(CalcOrden.Text)
        Dim dtModels = modelschanges.SelectByModel(CalcModelo.Text)
        'Dim idUnit As Guid = dtModels.Rows(0).Item(4)
        'Dim dtUnits = catUnits.SelectOne(idUnit)

        If CalcOrden.Text.IsEmpty Then
            Label3.Text = "Error"
            Return
        End If

        If CalcModelo.Text.IsEmpty Then
            Label3.Text = "Error"
            Return
        End If

        If dtOrder.Rows.Count = 0 Then
            Label3.Text = "No se encontro la orden"
            Return
        End If

        If dtModels.Rows.Count = 0 Then
            Label3.Text = "No se encontro el modelo"
            Return
        End If

        If dtOrder.Rows(0).Item(3) = False Then
            Label3.Text = "La orden no es para retrabajo"
            Return
        End If

        lblDispOrden.Text = dtOrder.Rows(0).Item(1)
        lblDispModelo.Text = dtModels.Rows(0).Item(1)
        Dim lifespan As Integer = dtModels.Rows(0).Item(2)
        Dim unit As String = dtModels.Rows(0).Item(3)
        'Dim unitValue As Integer = dtUnits.Rows(0).Item(3)
        txtCalculo.Text = lifespan.ToString + " " + unit
        InfoModal.Show()

    End Sub

    Protected Sub lbCancel_Click(sender As Object, e As EventArgs) Handles lbCancel.Click
        InfoModal.Hide()
    End Sub
End Class
