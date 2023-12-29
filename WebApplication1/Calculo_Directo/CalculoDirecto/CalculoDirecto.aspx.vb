Imports System.Drawing
Imports System.Net
Imports System.Web.WebPages

Public Class CalculoDirecto

    Inherits System.Web.UI.Page

    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges
    Dim catReworkOrders As CatReworkOrders = New CatReworkOrders
    Dim catUnits As CatUnits = New CatUnits
    Dim expirationDate As Legacy.ExpirationDate = New Legacy.ExpirationDate()
    Dim expirationDateDirect As Legacy.ExpirationDateTest = New Legacy.ExpirationDateTest()
    Dim mfgDate As Legacy.ShortDate = New Legacy.ShortDate()

    Dim userPlaceholder As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)

        If Not Page.IsPostBack Then

        End If
    End Sub

    ' Botón para realizar validaciones y cálculo
    Protected Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        CleanData(False)
        EnableControls(False)

        Dim workOrder As String
        Dim model As String

        Dim area As String = "Estandar"

        Dim plant As String = String.Empty
        Dim months As String = String.Empty
        Dim material As String = String.Empty
        Dim lot As String = String.Empty
        Dim status As String = String.Empty
        Dim workorderModel As String = String.Empty

        Dim dtWorkOrder = catReworkOrders.SelectByOrder(txtWorkOrder.Text)
        Dim dtModel = modelChanges.SelectByModel(txtModel.Text)

        txtWorkOrder.Text = txtWorkOrder.Text.Trim()
        txtModel.Text = txtModel.Text.Trim().ToUpper()
        workOrder = txtWorkOrder.Text
        model = txtModel.Text

        If Not ValidateData(dtWorkOrder, dtModel) Then
            Return
        End If

        ' Se valida que la orden exista en SAP
        If expirationDate.WorkOrderIsValid(workOrder) Then

            ' Se verificar si la fecha de expiración ya fue calculada
            Dim currentExpDate As Legacy.ShortDate
            currentExpDate = expirationDate.QueryExpirationDate(workOrder)

            If Not currentExpDate.IsEmptyDate Then
                lblErrorMessage.Text = "La fecha de expiración para la orden " + workOrder + " ya fue calculada (" + currentExpDate.ToString() + ")"
                Return
            End If

            ' Validar si la orden tiene fecha de manufactura
            mfgDate = expirationDate.GetManufDate(workOrder)
            If expirationDate.GetManufDate(workOrder).IsEmptyDate() Then
                lblErrorMessage.Text = "La orden " + workOrder + " no tiene asignada fecha de manufactura."
                Return
            End If

            ' Validar si la fecha de manufactura es mayor a la fecha actual
            If mfgDate.ToDate() > Today Then
                lblErrorMessage.Text = "La orden [" + workOrder + "] tiene fecha de manufactura [" + mfgDate.ToString() + "] mayor a la fecha actual."
                Return
            End If

            ' Valida que Orden y Catálogo correspondan en ERP
            workorderModel = expirationDate.GetCatalog(workOrder, plant, material, lot)

            If Not workorderModel Is Nothing Then
                If workorderModel.Equals(model) Then

                    ' Valida que exista vida util para el catalogo
                    Dim calcExpDate As Legacy.ShortDate = expirationDateDirect.GetExpirationDate(model, mfgDate, months)
                    If calcExpDate.IsEmptyDate Then
                        lblErrorMessage.Text = "No fue posible calcular la fecha de expiración"
                    Else
                        If (Not String.IsNullOrEmpty(lot)) Then
                            status = "N"
                        Else
                            status = "D"
                        End If
                        expirationDateDirect.Delete(workOrder)
                        expirationDateDirect.Insert(workOrder, calcExpDate.ToDate, userPlaceholder, model, months, plant, lot, status)
                        lblSuccessMessage.Text = "Todo salió bien, fecha de manufactura: " + mfgDate.ToDate.ToString("dd/MMM/yyyy") + " fecha de expiracion: " + calcExpDate.ToDate.ToString("dd/MMM/yyyy") 'agregar cuándo se realizó

                    End If

                Else
                    lblErrorMessage.Text = "El Catálogo / Modelo no pertenece al Número de Orden de Trabajo."
                End If
            Else
                lblErrorMessage.Text = "ERROR! Notifique al soporte de IT."
            End If
        Else
            lblErrorMessage.Text = "No se encontró información en SAP con el Número de Orden de Trabajo ingresado."
        End If


        'lblModalDispWorkOrder.Text = dtWorkOrder.Rows(0).Item(1)
        'lblModalDispModel.Text = dtModel.Rows(0).Item(1)
        'Dim lifespan As Integer = dtModel.Rows(0).Item(2)
        'Dim unit As String = dtModel.Rows(0).Item(3)
        ''''''''Dim unitValue As Integer = dtUnits.Rows(0).Item(3)
        'txtModalCalculation.Text = lifespan.ToString + " " + unit
        'InfoModal.Show()

    End Sub



    ' Botón para reiniciar textboxes de búsqueda
    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        CleanData(True)
        EnableControls(True)
    End Sub

    ' Botón de aceptar para el modal
    Protected Sub btnModalAccept_Click(sender As Object, e As EventArgs) Handles btnModalAccept.Click

    End Sub

    ' Botón de cancelar para el modal
    Protected Sub btnModalCancel_Click(sender As Object, e As EventArgs) Handles btnModalCancel.Click

    End Sub

    Private Function ValidateData(dtWorkOrder As DataTable, dtModel As DataTable) As Boolean
        If txtWorkOrder.Text.IsEmpty Then
            lblErrorMessage.Text = "Favor de escribir un Número de Orden de Trabajo"
            Return False
        End If

        If txtModel.Text.IsEmpty Then
            lblErrorMessage.Text = "Favor de escribir el Catálogo / Modelo"
            Return False
        End If

        If dtWorkOrder.Rows.Count = 0 Then
            lblErrorMessage.Text = "La Orden de Trabajo ingresada no se encuentra cargada en sistema"
            Return False
        End If

        If dtModel.Rows.Count = 0 Then
            lblErrorMessage.Text = "El Modelo ingresado no se encuentra listado dentro de los Modelos Aprobados"
            Return False
        End If

        If dtWorkOrder.Rows(0).Item(3) = False Then
            lblErrorMessage.Text = "La Orden de Trabajo ingresada no se encuentra indicada para Retrabajo"
            Return False
        End If

        Return True
    End Function

    Private Sub CleanData(cleanTextBoxes As Boolean)
        If cleanTextBoxes Then
            txtWorkOrder.Text = ""
            txtModel.Text = ""
        End If
        lblErrorMessage.Text = ""
        lblSuccessMessage.Text = ""
        'btnCalculate.Enabled = True
    End Sub

    Private Sub EnableControls(enable As Boolean)
        btnCalculate.Enabled = enable
    End Sub
End Class
