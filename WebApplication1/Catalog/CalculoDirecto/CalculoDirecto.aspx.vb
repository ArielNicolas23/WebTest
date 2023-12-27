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

        Dim expdate As Legacy.ExpirationDate = New Legacy.ExpirationDate()
        Dim catalog As String
        Dim Modulo As String = "Estandar"
        Dim Area As String
        Dim workOrder As String
        Dim plant As String = String.Empty
        Dim months As String = String.Empty
        Dim material As String = String.Empty
        Dim lot As String = String.Empty
        Dim manufDate As Legacy.ShortDate = New Legacy.ShortDate()

        Dim modelschanges = New ED_ModelsChanges
        Dim catreworkorders = New CatReworkOrders
        Dim catUnits = New CatUnits
        Dim dtOrder = catreworkorders.SelectByOrder(CalcOrden.Text)
        Dim dtModels = modelschanges.SelectByModel(CalcModelo.Text)
        'Dim idUnit As Guid = dtModels.Rows(0).Item(4)
        'Dim dtUnits = catUnits.SelectOne(idUnit)

        CalcModelo.Text = CalcModelo.Text.Trim().ToUpper()
        CalcOrden.Text = CalcOrden.Text.Trim()
        workOrder = CalcOrden.Text

        If expdate.WorkOrderIsValid(workOrder) Then

            Dim currentExpDate As Legacy.ShortDate
            currentExpDate = expdate.QueryExpirationDate(workOrder)

            '----------------
            ' Valida si la fecha de expiracion de la orden ya fue calculada
            '----------------

            If Not currentExpDate.IsEmptyDate Then
                Label3.Text = "La fecha de expiración para la orden " + CalcOrden.Text + " ya fue calculada (" + currentExpDate.ToString() + ")"
                Return
            End If

            'Validar si la orden tiene fecha de manufactura
            manufDate = expdate.GetManufDate(workOrder)
            If expdate.GetManufDate(workOrder).IsEmptyDate() Then
                Label3.Text = "La orden " + workOrder + " no tiene asignada fecha de manufactura."
                Return
            End If

            'Validar si la fecha de manufactura es mayor a la fecha actual
            If manufDate.ToDate() > Today Then
                Label3.Text = "La orden [" + workOrder + "] tiene fecha de manufactura [" + manufDate.ToString() + "] mayor a la fecha actual."
                Return
            End If

            'If Not (expdate.IsMinimedWorkOrder(TxtWorkOrder.Text) Or expdate.IsCoatingWorkOrder(TxtWorkOrder.Text) Or expdate.IsSprinterRXCatalog(Me.TxtCatalog.Text) Or expdate.IsLegendRXCatalog(Me.TxtCatalog.Text)) Then
            '----------------
            ' Valida que orden y catalogo correspondan en ERP
            '----------------

            catalog = expdate.GetCatalog(workOrder, plant, material, lot)
            If Not catalog Is Nothing Then
                If catalog.Equals(CalcModelo.Text) Then


                    '----------------
                    ' Valida que exista vida util para el catalogo
                    '----------------

                    Dim calcExpDate As Legacy.ShortDate = expdate.GetExpirationDate(catalog, manufDate, months)
                    If calcExpDate.IsEmptyDate Then

                        Label3.Text = "La vida útil del catálogo no fue encontrada."

                    Else
                        '----------------
                        ' Valida que la fecha de expiracion para el catalogo deba ser calculada en este modulo
                        '----------------

                    End If

                Else
                    Label3.Text = "El número catálogo no pertenece al número de orden de trabajo."
                End If
            Else
                Label3.Text = "ERROR! Notifique al soporte de IT."
            End If
            'Else
            'LblMessage.Text = "El número de orden de trabajo suministrado no puede ser procesado en este módulo. Verifique si es un productoMinimed o Carmeda/Trillium para usar el módulo correcto."
            'End If
        Else
            Label3.Text = "El número de orden de trabajo no es válido."
        End If

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
