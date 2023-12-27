Imports Microsoft.VisualBasic
Imports System.Data
Imports Legacy.ExpirationDate
Imports System.Collections.Generic
Imports System.Linq


Namespace ModuloGeneral
    Public Class ExpirationDate

        Public Function CalculateExpirationDate(ByVal workOrder As String)
            Dim calculateCompleted As Boolean = False
            Dim strMessage As String = String.Empty
            Dim catalog As String = String.Empty
            Dim material As String = String.Empty


            If WorkOrderExists(workOrder) Then
                catalog = GetCatalog(workOrder)
                material = GetMaterialNo(workOrder)
            Else
                calculateCompleted = False
                strMessage = "Error, orden [" + workOrder + "] no encontrada..."
            End If
        End Function



        ''' <summary>
        ''' Verifica que el numero de orden exista en ERP
        ''' </summary>
        ''' <param name="workOrder"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function WorkOrderExists(ByVal workOrder As String) As Boolean
            Dim exists As Boolean = False
            Dim _message As String = String.Empty
            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()
            ReDim _workOrders(0)
            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"), _
                    ProductionOrder:=_workOrders, _
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"), _
                    Environment:=ConfigurationManager.AppSettings("Environment"), _
                    HandledError:=_message _
                    )

            If (prodsOrder Is Nothing) Then
                Return False
            End If

            If (prodsOrder.Count() > 0) Then
                exists = True
            End If

            Return exists
        End Function

        ''' <summary>
        ''' Obtiene el número de Numero de Material perteneciente al número de orden
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>Número de Material.</returns>
        ''' <remarks>Si retorna Nothing, entonces la búsqueda fallo de alguna manera</remarks>
        Public Function GetMaterialNo(ByVal workOrder As String, Optional ByRef plant As String = "", Optional ByRef material As String = "", Optional ByRef lot As String = "") As String
            Dim catalog As String = Nothing
            Dim _message As String = String.Empty
            Dim MATNR As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()
            Dim _workOrders As String()
            ReDim _workOrders(0)
            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"), _
                    ProductionOrder:=_workOrders, _
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"), _
                    Environment:=ConfigurationManager.AppSettings("Environment"), _
                    HandledError:=_message _
                    )

            If (prodsOrder Is Nothing) Then
                Return Nothing
            End If

            If (prodsOrder.Count() > 0) Then
                If (prodsOrder.FirstOrDefault().WERKS IsNot Nothing) Then
                    plant = prodsOrder.FirstOrDefault().WERKS.Trim()
                End If
                If (prodsOrder.FirstOrDefault().PLNBEZ IsNot Nothing) Then
                    material = prodsOrder.FirstOrDefault().PLNBEZ.Trim()
                End If
                If (prodsOrder.FirstOrDefault().CHARG IsNot Nothing) Then
                    lot = prodsOrder.FirstOrDefault().CHARG.Trim()
                End If
                If (prodsOrder.FirstOrDefault().MATNR IsNot Nothing) Then
                    MATNR = prodsOrder.FirstOrDefault().MATNR.Trim()
                End If
            End If

            Return MATNR
        End Function

        ''' <summary>
        ''' Obtiene el numero de Catalogo de la orden
        ''' </summary>
        ''' <param name="workOrder"></param>
        ''' <param name="plant"></param>
        ''' <param name="material"></param>
        ''' <param name="lot"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCatalog(ByVal workOrder As String, _
                                   Optional ByRef plant As String = "", _
                                   Optional ByRef material As String = "", _
                                   Optional ByRef lot As String = "") As String
            Dim catalog As String = Nothing
            Dim _message As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()

            ReDim _workOrders(0)

            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"), _
                    ProductionOrder:=_workOrders, _
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"), _
                    Environment:=ConfigurationManager.AppSettings("Environment"), _
                    HandledError:=_message _
                    )

            If (prodsOrder Is Nothing) Then
                Return Nothing
            End If

            If (prodsOrder.Count() > 0) Then
                If (prodsOrder.FirstOrDefault().WERKS IsNot Nothing) Then
                    plant = prodsOrder.FirstOrDefault().WERKS.Trim()
                End If
                If (prodsOrder.FirstOrDefault().PLNBEZ IsNot Nothing) Then
                    material = prodsOrder.FirstOrDefault().PLNBEZ.Trim()
                End If
                If (prodsOrder.FirstOrDefault().CHARG IsNot Nothing) Then
                    lot = prodsOrder.FirstOrDefault().CHARG.Trim()
                End If
                If (prodsOrder.FirstOrDefault().MIN Is Nothing) Then
                    If (prodsOrder.FirstOrDefault().PLNBEZ Is Nothing) Then
                        catalog = Nothing
                    Else
                        catalog = prodsOrder.FirstOrDefault().PLNBEZ.Trim()
                    End If
                Else
                    If (Not prodsOrder.FirstOrDefault().MIN.Trim().Equals(String.Empty)) Then
                        catalog = prodsOrder.FirstOrDefault().MIN.Trim()
                    Else
                        If (prodsOrder.FirstOrDefault().PLNBEZ Is Nothing) Then
                            catalog = Nothing
                        Else
                            catalog = prodsOrder.FirstOrDefault().PLNBEZ.Trim()
                        End If
                    End If
                End If
            End If

            Return catalog
        End Function









        Public Function ExpirationDate(ByVal workOrder As String, ByVal isRecalculatedDate As Boolean, ByRef strMessage As String) As Boolean
            Dim currentExpDate As Legacy.ShortDate
            Dim manufactureDate As Legacy.ShortDate
            Dim PartNumber As String = String.Empty
            Dim legacyExpDate As Legacy.ExpirationDate = New Legacy.ExpirationDate()

            'Obtiene la fecha de expiracion de la base de datos
            currentExpDate = legacyExpDate.QueryExpirationDate(workOrder, PartNumber)
            If isRecalculatedDate Then
                If currentExpDate.IsEmptyDate Then
                    strMessage = "La fecha de expiración para la orden [" + workOrder + "] no ha sido calculada."
                    Return False
                End If
            Else
                If Not currentExpDate.IsEmptyDate Then
                    strMessage = "La fecha de expiración para la orden [ " + workOrder + " ] ya fue calculada  [ " + currentExpDate.ToString() + " ] por el componente [ " + PartNumber + " ] "
                    Return False
                End If
            End If

            'Obtiene la fecha de manofactura de la orden
            manufactureDate = legacyExpDate.GetManufDate(workOrder)
            If manufactureDate.IsEmptyDate Then
                strMessage = "La orden [" + workOrder + "] no tiene asignada fecha de manufactura."
                Return False
            End If
            'valida si la fecha de manufactura es mayor a la actual
            If manufactureDate.ToDate() > Today Then
                strMessage = "La orden [" + workOrder + "] tiene feha de manufactura [" + manufactureDate.ToString() + "] mayor a la fecha actual."
                Return False
            End If

            Return True
        End Function


        'Public Function GetCriticalComponents(ByVal material As String, ByRef strMessage As String) As List(Of ModelCatCritFiltrado)
        '    Dim oModuloCalfec As New ModuloCalFecExp
        '    Dim dtCriticalComponent As DataTable
        '    dtCriticalComponent = oModuloCalfec.Select_CriticalComponentsUnionModelCompByModel(material)

        '    If (dtCriticalComponent.Rows.Count <= 0) Then
        '        strMessage = "El número de material [" + material + "] No cuenta con Componentes criticos definidos en los catálogos de componentes criticos."
        '        Return Nothing
        '    End If

        '    Dim componentsFound As New List(Of ModelCatCritFiltrado)
        '    For Each drCompCrit As DataRow In dtCriticalComponent.Rows
        '        Dim objCompCrit As New ModelCatCritFiltrado
        '        objCompCrit.SB2Component = drCompCrit("SB2Component").ToString()
        '        objCompCrit.SB2Model = drCompCrit("SB2Model").ToString()
        '        objCompCrit.CompActive = drCompCrit("CompActive").ToString()
        '        objCompCrit.Range = drCompCrit("Range").ToString()
        '        objCompCrit.IsCalculate = False
        '        componentsFound.Add(objCompCrit)
        '    Next

        '    Return componentsFound
        ''End Function

        'Public Function GetMaterialNumberComponents(ByVal plant As String, ByVal workOrder As String, ByVal componentsFound As List(Of ModelCatCritFiltrado), ByRef strMessage As String)
        '    Dim stepSupplied() As String

        '    If (ConfigurationManager.AppSettings("JMMRouteStepsSupplied").Length > 0) Then
        '        stepSupplied = ConfigurationManager.AppSettings("JMMRouteStepsSupplied").Split(CChar(","))
        '    Else
        '        stepSupplied = Nothing
        '    End If

        '    Dim serPLBatches As wcfBatchCharacteristics.IwcfBatchCharacteristicsClient = Nothing
        '    Dim resPLBatches() As wcfBatchCharacteristics.BatchCharacteristics = Nothing


        '    Dim errorMessage As String = ""
        '    Dim serWOPartListGM As wcfGoodsMovement.IwcfGoodsMovementClient
        '    Dim resWOPartListGMIssued As IEnumerable(Of wcfGoodsMovement.ExpDateBatches)
        '    Dim resWOPartListGMBatches As IEnumerable(Of wcfGoodsMovement.ExpDateBatches)
        '    Dim verifStep As VerificationStep = VerificationStep.None
        '    Dim serWOPLFilteredGMIssued As IEnumerable(Of wcfGoodsMovement.ExpDateBatches) = Nothing

        '    serWOPartListGM = New wcfGoodsMovement.IwcfGoodsMovementClient()

        '    resWOPartListGMIssued = serWOPartListGM.GetComponentIssued( _
        '                                PlantNumber:=plant, _
        '                                ProductionOrder:=workOrder, _
        '                                StepList:=stepSupplied, _
        '                                ApplicationID:=ConfigurationManager.AppSettings("AppID"), _
        '                                Environment:=ConfigurationManager.AppSettings("Environment"), _
        '                                HandledError:=errorMessage _
        '                        )


        '    If (resWOPartListGMIssued IsNot Nothing) Then
        '        If (resWOPartListGMIssued.Count() > 0) Then
        '            'Dim dtWOPartListGMIssued As DataTable = resWOPartListGMIssued.TODATATABLE()

        '            Dim partListFilteredIssed = From WOPL In resWOPartListGMIssued _
        '                                        Join COMP In componentsFound _
        '                                        On WOPL.ComponentNumber Equals COMP.SB2Component _
        '                                        Select WOPL

        '            If (partListFilteredIssed IsNot Nothing) Then
        '                If (partListFilteredIssed.Count() > 0) Then
        '                    Do
        '                        If (verifStep = VerificationStep.None) Then
        '                            verifStep = VerificationStep.Supplied

        '                            serWOPLFilteredGMIssued = From WOGM In partListFilteredIssed _
        '                                                      Where (WOGM.ReqQty > 0 And WOGM.DelvQty = 0) _
        '                                                        And (Not String.IsNullOrEmpty(WOGM.ComponentNumber)) _
        '                                                      Select WOGM
        '                        ElseIf (verifStep = VerificationStep.Supplied) Then
        '                            verifStep = VerificationStep.Negatives

        '                            serWOPLFilteredGMIssued = From WOGM In partListFilteredIssed _
        '                                                      Where (WOGM.DelvQty < 0) _
        '                                                        And (Not String.IsNullOrEmpty(WOGM.ComponentNumber)) _
        '                                                      Select WOGM
        '                        ElseIf (verifStep = VerificationStep.Negatives) Then
        '                            verifStep = VerificationStep.Difference

        '                            serWOPLFilteredGMIssued = From WOGM In partListFilteredIssed _
        '                                                      Where (Math.Abs(WOGM.ReqQty - WOGM.DelvQty) >= 1) _
        '                                                        And (Not String.IsNullOrEmpty(WOGM.ComponentNumber)) _
        '                                                      Select WOGM
        '                        ElseIf (verifStep = VerificationStep.Difference) Then
        '                            verifStep = VerificationStep.Alert

        '                            serWOPLFilteredGMIssued = From WOGM In partListFilteredIssed _
        '                                                      Where (WOGM.ReqQty <> WOGM.DelvQty) _
        '                                                        And (Not String.IsNullOrEmpty(WOGM.ComponentNumber)) _
        '                                                      Select WOGM
        '                        ElseIf (verifStep = VerificationStep.Alert) Then
        '                            verifStep = VerificationStep.Finished
        '                            Exit Do
        '                        End If

        '                        If (serWOPLFilteredGMIssued IsNot Nothing) Then
        '                            If (serWOPLFilteredGMIssued.Count() > 0) Then
        '                                strMessage = String.Empty

        '                                For Each partsNotKitted As wcfGoodsMovement.ExpDateBatches In serWOPLFilteredGMIssued
        '                                    strMessage += "Componente:[" + partsNotKitted.ComponentNumber + "], Cant. Solicitada:[" + partsNotKitted.ReqQty.ToString() + "], Cant. Surtida:[" + partsNotKitted.DelvQty.ToString() + "]" + "<BR />"
        '                                Next

        '                                If (verifStep = VerificationStep.Supplied) Then
        '                                    strMessage = "Error en orden:[" + workOrder + "]" + vbCrLf + "Algunos componentes que influyen la fecha de expiracion en la orden no se han surtido completamente." + "<BR />" + "<BR />" + "Componentes faltantes:" + "<BR />" + "<BR />" + strMessage
        '                                ElseIf (verifStep = VerificationStep.Negatives) Then
        '                                    strMessage = "Error en orden:[" + workOrder + "]" + vbCrLf + "Algunos componentes que influyen la fecha de expiracion en la orden se han surtido con cantidades negativas." + "<BR />" + "<BR />" + "Componentes discrepantes:" + "<BR />" + "<BR />" + strMessage
        '                                ElseIf (verifStep = VerificationStep.Difference) Then
        '                                    strMessage = "Error en orden:[" + workOrder + "]" + vbCrLf + "Algunos componentes que influyen la fecha de expiracion en la orden se han surtido con cantidades discrepantes." + "<BR />" + "<BR />" + "Componentes discrepantes:" + "<BR />" + "<BR />" + strMessage
        '                                ElseIf (verifStep = VerificationStep.Alert) Then
        '                                    strMessage = "Error en orden:[" + workOrder + "]" + vbCrLf + "Algunos componentes que influyen la fecha de expiracion en la orden se han surtido con cantidades discrepantes." + "<BR />" + "<BR />" + "Componentes discrepantes:" + "<BR />" + "<BR />" + strMessage
        '                                End If

        '                                If (verifStep = VerificationStep.Supplied Or verifStep = VerificationStep.Negatives Or verifStep = VerificationStep.Difference) Then
        '                                    'Return message
        '                                End If
        '                            End If
        '                        End If

        '                    Loop Until (verifStep = VerificationStep.Finished)
        '                Else
        '                    strMessage = "Error en orden:[" + workOrder + "], no se encontraron los componentes que influyen la fecha de expiracion en la orden."
        '                    'Return message
        '                End If
        '            Else
        '                strMessage = "Error en orden:[" + workOrder + "], no se encontraron los componentes que influyen la fecha de expiracion en la orden."
        '                'Return message
        '            End If
        '        Else
        '            strMessage = "Error en orden:[" + workOrder + "], no se obtuvo listado de movimientos de componentes de la orden."
        '            'Return message
        '        End If
        '    Else
        '        strMessage = "Error en orden:[" + workOrder + "], no se obtuvo listado de movimientos de componentes de la orden."
        '        'Return message
        '    End If


        '    If strMessage.Trim().Length > 0 Then
        '        Return Nothing
        '    End If




        '    Return Nothing
        'End Function

        Function IsValidCatalog(ByVal catalog As String, ByVal workOrder As String, ByRef strMessage As String) As Boolean
            If catalog Is Nothing Then
                strMessage = "La orden [" + workOrder + "]. No tiene catálogo asignado."
                Return False
            Else
                strMessage = String.Empty
                Return True
            End If
        End Function

        Function IsValidMaterial(ByVal Material As String, ByVal MaterialCapture As String, ByVal workOrder As String, ByRef strMessage As String) As Boolean
            If String.IsNullOrEmpty(Material) Then
                strMessage = "La orden [" + workOrder + "]. No tiene Número de Material asignado asignado."
                Return False
            ElseIf Material.Trim <> MaterialCapture.Trim Then
                strMessage = "El número de material [" + MaterialCapture.Trim + "] no pertenece a la orden [" + workOrder.Trim + "]."
                Return False
            Else
                strMessage = String.Empty
                Return True
            End If
        End Function


    End Class
End Namespace