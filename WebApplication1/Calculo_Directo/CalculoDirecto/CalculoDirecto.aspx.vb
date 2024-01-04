Imports System.Drawing
Imports System.Net
Imports System.Web.WebPages

Public Class CalculoDirecto

    Inherits System.Web.UI.Page

    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges
    Dim catReworkOrders As CatReworkOrders = New CatReworkOrders
    Dim catUnits As CatUnits = New CatUnits
    Dim catReworkStatus As CatReworkStatus = New CatReworkStatus
    Dim expirationDate As Legacy.ExpirationDate = New Legacy.ExpirationDate()
    Dim expirationDateDirect As Legacy.ExpirationDateTest = New Legacy.ExpirationDateTest()

    Dim area As String = "Directo"
    Dim userPlaceholder As String

    Dim workOrder As String
    Dim model As String
    Dim plant As String
    Dim months As String
    Dim calcExpDate As Legacy.ShortDate
    Dim mfgDate As Legacy.ShortDate
    Dim modelStatus As String
    Dim lot As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)

        If Not Page.IsPostBack Then

        End If
    End Sub

    ' Botón para realizar validaciones y cálculo
    Protected Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        CalculateExpirationDate(True)
        txtWorkOrder.Enabled = False
        txtModel.Enabled = False
    End Sub



    ' Botón para reiniciar textboxes de búsqueda
    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        CleanControlsData(True)
        ButtonsVisibility(True, False, False)
        ResetVariables()
        txtWorkOrder.Enabled = True
        txtModel.Enabled = True
    End Sub

    ' Botón de aceptar para el modal
    Protected Sub btnModalAccept_Click(sender As Object, e As EventArgs) Handles btnModalAccept.Click

    End Sub

    ' Botón de cancelar para el modal
    Protected Sub btnModalCancel_Click(sender As Object, e As EventArgs) Handles btnModalCancel.Click

    End Sub

    Private Sub InsertCalculation()
        SetVariables()

        If (Not String.IsNullOrEmpty(lot)) Then
            modelStatus = "N"
        Else
            modelStatus = "D"
        End If

        expirationDateDirect.Delete(workOrder)
        expirationDateDirect.Insert(workOrder, calcExpDate.ToDate, userPlaceholder, model, months, plant, lot, modelStatus)

        lblSuccessMessage.Text = "Se ha confirmado la Fecha de Expiración para la Orden de Trabajo " + workOrder + "<br />" + "Fecha de manufactura: " + mfgDate.ToDate.ToString("dd/MMM/yyyy") + "<br />" + "Fecha de expiracion: " + calcExpDate.ToDate.ToString("dd/MMM/yyyy")

        ButtonsVisibility(False, False, False)
        ResetVariables()
    End Sub

    Private Sub AssignSessionVariables(workOrder As String, model As String, plant As String, months As String, calcExpDate As Legacy.ShortDate, mfgDate As Legacy.ShortDate, modelStatus As String, lot As String)
        Session("workOrder") = workOrder
        Session("model") = model
        Session("plant") = plant
        Session("months") = months
        Session("calcExpDate") = calcExpDate
        Session("mfgDate") = mfgDate
        Session("modelStatus") = modelStatus
        Session("lot") = lot
    End Sub

    Private Sub SetVariables()
        workOrder = Session("workOrder")
        model = Session("model")
        plant = Session("plant")
        months = Session("months")
        calcExpDate = Session("calcExpDate")
        mfgDate = Session("mfgDate")
        modelStatus = Session("modelStatus")
        lot = Session("lot")
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

        If Not dtWorkOrder.Rows(0).Item(2).ToString.ToLower.Contains(area.ToLower) Then
            lblErrorMessage.Text = "La Orden de Trabajo ingresada solo puede ser procesada en el área de Cálculo " + dtWorkOrder.Rows(0).Item(2).ToString()
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

    ' Validaciones del estatus de SAP
    ' validateRework en true si se requiere validar si el estatus encontrado es Rework en catálogo
    ' validateRelease en true si se requiere checar que sea Rel (por confirmar)
    Private Function ValidateSAPStatus(SAPStatus As String, validateRework As Boolean, validateRelease As Boolean)
        Dim statusArray As String() = SAPStatus.Split(" "c)
        Dim dtCatReworkStatus

        For Each status As String In statusArray
            dtCatReworkStatus = catReworkStatus.SelectBySAPStatus(SAPStatus)

            If dtCatReworkStatus.Rows.Count = 0 Then
                lblErrorMessage.Text = "El Estatus [" + SAPStatus + "] de la Orden de Trabajo ingresada no se encuentra listado en el catálogo de Estatus"
                Return False
            End If

            If dtCatReworkStatus.Rows(0).Item(1) = False And validateRework Then
                lblErrorMessage.Text = "No es posible realizar el Cálculo debido a que el Estatus [" + SAPStatus + "] de la Orden de Trabajo ingresada no pertenece a un Estatus de Retrabajo"
                Return False
            End If
        Next

        If validateRelease Then
            If Not SAPStatus.Contains("Rel") Then
                lblErrorMessage.Text = "No es posible realizar el Cálculo debido a que el Estatus [" + SAPStatus + "] de la Orden de Trabajo no se encuentra en Release [Rel]"
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub CleanControlsData(cleanTextBoxes As Boolean)
        If cleanTextBoxes Then
            txtWorkOrder.Text = ""
            txtModel.Text = ""
        End If
        lblErrorMessage.Text = ""
        lblSuccessMessage.Text = ""
        'btnCalculate.Enabled = True
    End Sub

    Private Sub ResetVariables()
        Session("workOrder") = String.Empty
        Session("model") = String.Empty
        Session("plant") = String.Empty
        Session("months") = String.Empty
        Session("calcExpDate") = New Legacy.ShortDate
        Session("modelStatus") = String.Empty
        Session("lot") = String.Empty
    End Sub

    Private Sub ButtonsVisibility(showCalculate As Boolean, showConfirm As Boolean, showRecalculate As Boolean)
        btnCalculate.Visible = showCalculate
        btnAcceptCalculation.Visible = showConfirm
        btnRecaulculate.Visible = showRecalculate
    End Sub

    Protected Sub btnAcceptCalculation_Click(sender As Object, e As EventArgs) Handles btnAcceptCalculation.Click
        InsertCalculation()
    End Sub

    Protected Sub btnRecaulculate_Click(sender As Object, e As EventArgs) Handles btnRecaulculate.Click
        CalculateExpirationDate(False)
    End Sub

    Private Sub CalculateExpirationDate(verifyExistCalculation As Boolean)
        CleanControlsData(False)

        Dim material As String = String.Empty
        Dim workOrderStatus As String = String.Empty
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
        If expirationDateDirect.WorkOrderIsValid(workOrder, workOrderStatus) Then

            ' Validaciones del estatus
            If Not ValidateSAPStatus(workOrderStatus, False, Not verifyExistCalculation) Then
                Return
            End If

            ' Se verificar si la fecha de expiración ya fue calculada
            Dim currentExpDate As Legacy.ShortDate
            currentExpDate = expirationDate.QueryExpirationDate(workOrder)

            If verifyExistCalculation Then
                If Not currentExpDate.IsEmptyDate Then
                    lblErrorMessage.Text = "La fecha de expiración para la orden " + workOrder + " ya fue calculada (" + currentExpDate.ToDate.ToString("dd/MMM/yyyy") + ")"  'AGREGAR CUÁNDO SE REALIZÓ

                    If chkAdmin.Checked = True Then ' AQUÍ DEBE IR LA PARTE DEL ADMINISTRADOR PERO LO MANEJAMOS MIENTRAS CON EL CHECK
                        lblErrorMessage.Text = lblErrorMessage.Text + "<br /><br />" + "Si requiere calcular nuevamente la Fecha de Expiración, presione Recalcular Fecha"
                        ButtonsVisibility(False, False, True)
                    End If
                    Return
                End If
            End If


            ' Validar si la orden tiene fecha de manufactura
            mfgDate = expirationDate.GetManufDate(workOrder)
            If expirationDate.GetManufDate(workOrder).IsEmptyDate() Then
                lblErrorMessage.Text = "La orden " + workOrder + " no tiene asignada fecha de manufactura."
                Return
            End If

            ' Validar si la fecha de manufactura es mayor a la fecha actual
            If mfgDate.ToDate() > Today Then
                lblErrorMessage.Text = "La orden [" + workOrder + "] tiene fecha de manufactura [" + mfgDate.ToDate.ToString("dd/MMM/yyyy") + "] mayor a la fecha actual."
                Return
            End If

            ' Valida que Orden y Catálogo correspondan en ERP
            workorderModel = expirationDate.GetCatalog(workOrder, plant, material, lot)

            If Not workorderModel Is Nothing Then
                If workorderModel.Equals(model) Then

                    ' Valida que exista vida util para el catalogo
                    calcExpDate = expirationDateDirect.GetExpirationDate(model, mfgDate, months)
                    If calcExpDate.IsEmptyDate Then
                        lblErrorMessage.Text = "No fue posible calcular la fecha de expiración"
                    Else
                        lblSuccessMessage.Text = "Cálculo generado con éxito" + "<br />" + "Fecha de manufactura: " + mfgDate.ToDate.ToString("dd/MMM/yyyy") + "<br />" + "Fecha de expiracion: " + calcExpDate.ToDate.ToString("dd/MMM/yyyy")
                        AssignSessionVariables(workOrder, model, plant, months, calcExpDate, mfgDate, modelStatus, lot)
                        ButtonsVisibility(False, True, False)
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
    End Sub
End Class
