Option Strict On

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Linq

Namespace Legacy
    Public Class ExpirationDateTest
        Protected m_Apls As String
        Protected m_Jde As String

        Public Enum VerificationStep
            None = 0
            Supplied = 1
            Negatives = 2
            Difference = 3
            Alert = 4
            Finished = 5
        End Enum

        Public Sub New()
            m_Apls = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
        End Sub


        ''' <summary>
        ''' Determina si un número de orden de trabajo es válido. Evalua lo siguiente:
        ''' - Que no sea una cadena vacía.
        ''' - Que todos los caracteres sean números.
        ''' - Que existe en ERP.
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>True si el número de orden es válido. False si no es válido.</returns>
        ''' <remarks></remarks>
        Public Function WorkOrderIsValid(ByVal workOrder As String) As Boolean
            Dim isValid As Boolean = False
            If Not workOrder.Equals(String.Empty) Then
                If Util.IsInteger(workOrder) Then
                    isValid = WorkOrderExists(workOrder)
                End If
            End If
            Return isValid
        End Function

        ''' <summary>
        ''' Determina si el número de orden se encuentra en la base de datos de ERP
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>True si el número de orden existe.. False si no existe.</returns>
        ''' <remarks></remarks>
        Private Function WorkOrderExists(ByVal workOrder As String) As Boolean
            Dim exists As Boolean = False
            Dim _message As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()

            ReDim _workOrders(0)

            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN(
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"),
                    ProductionOrder:=_workOrders,
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"),
                    Environment:=ConfigurationManager.AppSettings("Environment"),
                    HandledError:=_message
                    )
            If (prodsOrder Is Nothing) Then
                Return False
            End If

            If (prodsOrder.Count() > 0) Then
                exists = True
            End If
            Return exists
        End Function

        Private Function GetsMRPController(ByVal workOrder As String) As String
            Dim MRP As String = String.Empty

            Dim _message As String = String.Empty
            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()
            ReDim _workOrders(0)
            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN(
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"),
                    ProductionOrder:=_workOrders,
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"),
                    Environment:=ConfigurationManager.AppSettings("Environment"),
                    HandledError:=_message
                    )


            If (prodsOrder Is Nothing) Then
                Return String.Empty
            End If

            If (prodsOrder.Count() > 0) Then
                MRP = prodsOrder(0).DISPO
            End If

            Return MRP
        End Function


        ''' <summary>
        ''' Obtiene el número de catálogo perteneciente al número de orden
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>Número de catálogo.</returns>
        ''' <remarks>Si retorna Nothing, entonces la búsqueda fallo de alguna manera</remarks>
        Public Function GetCatalog(ByVal workOrder As String,
                                   Optional ByRef plant As String = "",
                                   Optional ByRef material As String = "",
                                   Optional ByRef lot As String = "") As String
            Dim catalog As String = Nothing
            Dim _message As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()

            ReDim _workOrders(0)

            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN(
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"),
                    ProductionOrder:=_workOrders,
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"),
                    Environment:=ConfigurationManager.AppSettings("Environment"),
                    HandledError:=_message
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

        ''' <summary>
        ''' Obtiene la fecha de manufactura perteneciente al número de orden
        ''' Manuel Beltran (Dic/2012)
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>Manufacturing date.</returns>
        ''' <remarks>Si retorna Nothing, entonces la búsqueda fallo de alguna manera</remarks>
        Public Function GetManufDate(ByVal workOrder As String) As ShortDate
            Dim sManufDate As String = String.Empty
            Dim sdManufDate As ShortDate = New ShortDate()
            Dim _message As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()

            ReDim _workOrders(0)

            _workOrders(0) = workOrder

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN(
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"),
                    ProductionOrder:=_workOrders,
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"),
                    Environment:=ConfigurationManager.AppSettings("Environment"),
                    HandledError:=_message
                    )

            If (prodsOrder Is Nothing) Then
                sdManufDate = New ShortDate()
            End If

            If (prodsOrder.Count() > 0) Then
                If (Not prodsOrder.FirstOrDefault().FTRMI Is Nothing) Then
                    If (Not prodsOrder.FirstOrDefault().FTRMI.Trim().Equals(String.Empty)) Then
                        sManufDate = prodsOrder.FirstOrDefault().FTRMI.Trim()
                        sdManufDate = New ShortDate(sManufDate)
                    End If
                End If
            End If

            Return sdManufDate
        End Function

        ''' <summary>
        ''' Obtiene el nombre del area asignada a ese material
        ''' </summary>
        ''' <param name="catalog">Material</param>
        ''' <returns>Nombre de area</returns>
        ''' <remarks>Si no encuentra catalogo o numero de area entonces regresa valor nulo</remarks>
        Public Function GetArea(ByVal catalog As String) As String
            Dim Area As String = Nothing

            Dim sqlstr As String
            sqlstr = "SELECT TblCatStdAreas.Name_Area FROM TblCatStd INNER JOIN TblCatStdAreas ON TblCatStd.Area = TblCatStdAreas.ID_Area WHERE TblCatStd.Catalogo = @Catalog"
            'sqlstr = "Select Meses from TblCatStd where Catalogo= @Catalog"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@Catalog", catalog)

            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    ' Obtiene el area asignada al catálogo y la fecha actual del servidor de base de datos.

                    If reader.Read Then
                        If Not reader.IsDBNull(0) Then
                            Area = reader.GetString(0)
                        End If
                    End If


                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return Area
        End Function

        ''' <summary>
        ''' Obtiene la unidad de medida para el calculo, puede ser: 1) Dias, 2) Meses
        ''' </summary>
        ''' <param name="catalog">Material</param>
        ''' <returns>Unidad de medida (1 ó 2)</returns>
        ''' <remarks>En caso de no encontrar el material regresa -1</remarks>
        Public Function GetUOMID(ByVal catalog As String) As Int32
            Dim uomid As Int32 = -1

            Dim sqlstr As String
            sqlstr = "SELECT TblCatStd.UOMID FROM TblCatStd WHERE TblCatStd.Catalogo = @Catalog"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@Catalog", catalog)

            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    ' Obtiene el area asignada al catálogo y la fecha actual del servidor de base de datos.

                    If reader.Read Then
                        If Not reader.IsDBNull(0) Then
                            uomid = reader.GetInt32(0)
                        End If
                    End If

                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return uomid
        End Function

        ''' <summary>
        ''' Obtiene el nombre de la unidad de medida, Puede ser: "Dias" or "Meses"
        ''' </summary>
        ''' <param name="uomid">ID de unidad de medida</param>
        ''' <returns>Nombre de unidad de medida</returns>
        ''' <remarks>En caso de no encontrarlo regresa cadena vacía</remarks>
        Public Function GetUOMName(ByVal uomid As Int32) As String
            Dim uomName As String = String.Empty

            Dim sqlstr As String
            sqlstr = "SELECT TblCatStdUOM.UOMName FROM TblCatStdUOM WHERE TblCatStdUOM.UOMID = @UOMID"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@UOMID", uomid)

            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    ' Obtiene el area asignada al catálogo y la fecha actual del servidor de base de datos.

                    If reader.Read Then
                        If Not reader.IsDBNull(0) Then
                            uomName = reader.GetString(0)
                        End If
                    End If

                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return uomName
        End Function

        ''' <summary>
        ''' Calcula la fecha de expiración de un catálogo basandose en la fecha actual
        ''' </summary>
        ''' <param name="Catalog">Número de catálogo</param>
        ''' <returns>La fecha de expiración calculada para el catálogo</returns>
        ''' <remarks></remarks>
        Public Function GetExpirationDate(ByVal Model As String,
                                          ByVal ManufDate As ShortDate,
                                          ByRef Months As String) As ShortDate
            Dim ExpirationDate As ShortDate


            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "spED_ED_ModelsChanges_SelectByModel"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Model", Model)
                conn.Open()

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()

                    '----------------
                    ' Obtiene los meses asignados al catálogo y la fecha actual del servidor de base de datos.
                    '----------------
                    Dim LifeSpan As Integer = reader.GetInt32(2)
                    Dim UnitQty As Short = reader.GetInt16(9)
                    Dim QtyToAdd As Integer = LifeSpan * UnitQty
                    Dim UnitValue As String = reader.GetString(10)

                    Months = QtyToAdd.ToString()
                    '----------------
                    ' Calcula una fecha temporal de fecha de expiración
                    '----------------
                    'Dim TempExpDate As Date = DateAdd(DateInterval.Month, Months, Today)
                    Dim TempExpDate As Date
                    Dim sTempExpDate As ShortDate = New ShortDate()

                    If (UnitValue.Equals("DIAS") Or UnitValue.Equals("MESES")) Then
                        If (UnitValue.Equals("DIAS")) Then
                            TempExpDate = DateAdd(DateInterval.Day, QtyToAdd, ManufDate.ToDate())
                        ElseIf (UnitValue.Equals("MESES")) Then
                            TempExpDate = DateAdd(DateInterval.Month, QtyToAdd, ManufDate.ToDate())
                        End If

                        'Comentado por Edgar Tirado Cota para no calcular el ultimo dia del mes
                        'sTempExpDate.Day = LastDayOfMonth(Month(TempExpDate), Year(TempExpDate))
                        sTempExpDate.Day = TempExpDate.Day
                        sTempExpDate.Month = TempExpDate.Month
                        sTempExpDate.Year = TempExpDate.Year
                    End If

                    '----------------
                    ' Calcula la fecha de expiración de la siguiente manera:
                    '   1. Calcula el mes de la fecha de expiración.
                    '   2. Calcula el último día del mes de la fecha de expiración.
                    '   3. Calcula el año de la fecha de expiración.
                    '----------------
                    'ExpirationDate.Month = Month(TempExpDate)
                    'ExpirationDate.Day = LastDayOfMonth(Month(TempExpDate), Year(TempExpDate))
                    'ExpirationDate.Year = Year(TempExpDate)
                    ExpirationDate = sTempExpDate


                End While

                conn.Close()
                conn.Dispose()
            End Using

            Return ExpirationDate
        End Function

        ''' <summary>
        ''' Calcula el último día de un mes determinado de un año determinado
        ''' </summary>
        ''' <param name="Month">Mes del año</param>
        ''' <param name="Year">Año en el que se encuentra el mes</param>
        ''' <returns>Ultimo día del mes</returns>
        ''' <remarks></remarks>
        Public Function LastDayOfMonth(ByVal month As Integer, ByVal year As Integer) As Integer
            Dim Residuo As Integer
            Dim returnValue As Integer = 0

            Select Case month
                Case 1 : returnValue = 31
                Case 2 : Residuo = year Mod 4
                    If Residuo <> 0 Then
                        returnValue = 28
                    Else
                        returnValue = 29
                    End If
                Case 3 : returnValue = 31
                Case 4 : returnValue = 30
                Case 5 : returnValue = 31
                Case 6 : returnValue = 30
                Case 7 : returnValue = 31
                Case 8 : returnValue = 31
                Case 9 : returnValue = 30
                Case 10 : returnValue = 31
                Case 11 : returnValue = 30
                Case 12 : returnValue = 31
            End Select

            LastDayOfMonth = returnValue
        End Function


        Public Function GetOrderMRP(ByVal workOrder As String, ByRef MRP As String) As Boolean
            Dim isValid As Boolean = False
            Dim searchMRP As String = GetsMRPController(workOrder)

            If Not String.IsNullOrEmpty(searchMRP) Then
                MRP = searchMRP
                isValid = True
            End If
            Return isValid
        End Function
    End Class
End Namespace
