Option Strict On

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Linq

Namespace Legacy
    Public Class ExpirationDate
        Protected m_Apls As String
        'Commented by Manuel Beltran (11.Apr.2012)
        'Protected m_Neurona As String
        Protected m_Jde As String
        Protected dbCon As String                            'cambiocambio

        Public Enum VerificationStep
            None = 0
            Supplied = 1
            Negatives = 2
            Difference = 3
            Alert = 4
            Finished = 5
        End Enum

        Public Sub New()
            m_Apls = ConfigurationManager.ConnectionStrings("AplsProd").ConnectionString
            'Commented by Manuel Beltran (11.Apr.2012)
            'm_Neurona = ConfigurationManager.ConnectionStrings("Neurona").ConnectionString
            m_Jde = ConfigurationManager.ConnectionStrings("Jde").ConnectionString

            dbCon = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
        End Sub

        ''' <summary>
        ''' Determina si una orden de trabajo pertenece al área de Recubrimiento (Carmeda/Trillium)
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo</param>
        ''' <returns>True si pertenece al área de Carmeda</returns>
        ''' <remarks></remarks>
        Public Function IsCoatingWorkOrder(ByVal workOrder As String) As Boolean
            Dim isCoating As Boolean = False
            Dim mrpController As String = String.Empty
            Dim mrpsAllowed As String() = Nothing
            Dim _message As String = String.Empty

            Dim proxyProdOrder As wcfProductionOrderDetails.IwcfProductionOrderDetailsClient _
                = New wcfProductionOrderDetails.IwcfProductionOrderDetailsClient()

            Dim _workOrders As String()

            ReDim _workOrders(0)

            _workOrders(0) = workOrder

            mrpsAllowed = ConfigurationManager.AppSettings("MRPsAllowed").Split(CChar(","))

            Dim prodsOrder As IEnumerable(Of wcfProductionOrderDetails.drv_ProdOrdDetMatMstBscDat) _
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"), _
                    ProductionOrder:=_workOrders, _
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"), _
                    Environment:=ConfigurationManager.AppSettings("Environment"), _
                    HandledError:=_message _
                  )

            If (Not prodsOrder Is Nothing) Then
                If (prodsOrder.Count() > 0) Then
                    If (Not prodsOrder.FirstOrDefault().DISPO Is Nothing) Then
                        mrpController = prodsOrder.FirstOrDefault().DISPO.Trim()
                        If (mrpsAllowed _
                            .Where(Function(x) x.Equals(mrpController)) _
                            .Count() > 0) Then
                            isCoating = True
                        End If
                    End If
                End If
            End If

            Return isCoating
        End Function

        ''' <summary>
        ''' Determina si una orden de trabajo pertenece al área de Recubrimiento (Carmeda/Trillium)
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo</param>
        ''' <returns>True si pertenece al área de Carmeda</returns>
        ''' <remarks></remarks>
        <Obsolete("Este metodo se utilizaba con la conexion a JDE")> _
        Public Function IsCoatingWorkOrder_Deprecated(ByVal workOrder As String) As Boolean

            Dim isCoating As Boolean = False
            Dim sqlstr As String = _
            "SELECT " + _
            "   AMFPRDDTA.F4801.WADOCO AS Orden " + _
            "FROM AMFPRDDTA.F4801, AMFPRDDTA.F3112 " + _
            "WHERE " + _
            "   AMFPRDDTA.F3112.WLDOCO = AMFPRDDTA.F4801.WADOCO AND " + _
            "   AMFPRDDTA.F4801.WADOCO = '" + workOrder + "' AND " + _
            "   AMFPRDDTA.F4801.WAMCU  = '        2100' AND" + _
            "   (AMFPRDDTA.F3112.WLMCU = '  2100CCA010' OR " + _
            "   AMFPRDDTA.F3112.WLMCU  = '  2100CTR010')"

            Dim connection As OleDbConnection = New OleDbConnection
            Dim command As OleDbCommand
            Dim reader As OleDbDataReader
            connection.ConnectionString = m_Jde
            command = New OleDbCommand(sqlstr, connection)

            Try
                connection.Open()
                reader = command.ExecuteReader()
                If reader.Read() Then
                    isCoating = True
                End If
            Catch oledbex As OleDbException
                Throw (oledbex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
            Return isCoating
        End Function

        '' OJO - FUNCION IMPORTANTE
        Public Function GetModuloItem(ByVal catalog As String) As String

            Dim Areaitem As String = Nothing

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
                            Areaitem = reader.GetString(0)
                        End If
                    End If

                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return Areaitem
        End Function

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
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
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

        Private Function GetsMRPController(ByVal workOrder As String) As String
            Dim MRP As String = String.Empty

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
                Return String.Empty
            End If

            If (prodsOrder.Count() > 0) Then
                MRP = prodsOrder(0).DISPO
            End If

            Return MRP
        End Function


        ''' <summary>
        ''' Determina si el número de orden se encuentra en la base de datos de JDE
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>True si el número de orden existe.. False si no existe.</returns>
        ''' <remarks></remarks>
        <Obsolete("Este metodo se utilizaba con la conexion a JDE")> _
        Private Function WorkOrderExists_Deprecated(ByVal workOrder As String) As Boolean
            Dim exists As Boolean = False

            Dim connection As OleDbConnection = New OleDbConnection
            Dim command As OleDbCommand
            Dim reader As OleDbDataReader
            Dim sqlstr As String
            sqlstr = "SELECT WALITM AS CATALOG FROM AMFPRDDTA.F4801 WHERE WADOCO= '" + workOrder + "' AND TRIM(WAMCU) = '2100'"
            connection.ConnectionString = m_Jde
            command = New OleDbCommand(sqlstr, connection)

            Try
                connection.Open()
                reader = command.ExecuteReader()
                If reader.Read() Then
                    exists = True
                End If
            Catch oledbex As OleDbException
                Throw (oledbex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
            Return exists
        End Function

        ''' <summary>
        ''' Obtiene el número de catálogo perteneciente al número de orden
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>Número de catálogo.</returns>
        ''' <remarks>Si retorna Nothing, entonces la búsqueda fallo de alguna manera</remarks>
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

        ''' <summary>
        ''' Obtiene el número de catálogo perteneciente al número de orden
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo.</param>
        ''' <returns>Número de catálogo.</returns>
        ''' <remarks>Si retorna Nothing, entonces la búsqueda fallo de alguna manera</remarks>
        <Obsolete("Este metodo se utilizaba con la conexion a JDE")> _
        Public Function GetCatalog_Deprecated(ByVal workOrder As String) As String

            Dim catalog As String = Nothing

            Dim connection As OleDbConnection = New OleDbConnection
            Dim command As OleDbCommand
            Dim reader As OleDbDataReader
            Dim sqlstr As String
            sqlstr = "SELECT WALITM AS CATALOG FROM AMFPRDDTA.F4801 WHERE WADOCO= '" + workOrder + "'"
            connection.ConnectionString = m_Jde
            command = New OleDbCommand(sqlstr, connection)

            Try
                connection.Open()
                reader = command.ExecuteReader()
                If reader.Read() Then
                    catalog = reader.GetString(0)
                End If
            Catch oledbex As OleDbException
                Throw (oledbex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
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
                = proxyProdOrder.GetProdOrderMatMstBscDat_MIN( _
                    PlantNumber:=ConfigurationManager.AppSettings("SAPPlant"), _
                    ProductionOrder:=_workOrders, _
                    ApplicationID:=ConfigurationManager.AppSettings("AppIDAux"), _
                    Environment:=ConfigurationManager.AppSettings("Environment"), _
                    HandledError:=_message _
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
        ''' Obtiene los meses registrados para un catálogo.
        ''' </summary>
        ''' <param name="catalog">Número de catálogo</param>
        ''' <returns>Cantidad de meses registrados para el catálogo.</returns>
        ''' <remarks>Si retorna un valor de 0, entonces el catálogo no se encuentra registrado en la base de datos.</remarks>
        Public Function GetShelfLife(ByVal catalog As String) As Int32
            Dim Months As Int32 = Nothing

            Dim sqlstr As String
            sqlstr = "Select Meses from TblCatStd where Catalogo= @Catalog"

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
                    ' Obtiene los meses asignados al catálogo y la fecha actual del servidor de base de datos.

                    If reader.Read Then
                        If Not reader.IsDBNull(0) Then
                            Months = Convert.ToInt32(reader.GetDecimal(0))
                        End If
                    End If

                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return Months
        End Function

        ''' <summary>
        ''' Obtiene los meses registrados para un catálogo.
        ''' </summary>
        ''' <param name="catalog">Número de catálogo</param>
        ''' <returns>Cantidad de meses registrados para el catálogo.</returns>
        ''' <remarks>Si retorna un valor de 0, entonces el catálogo no se encuentra registrado en la base de datos.</remarks>
        Public Function CeckIfCatalogIsUse(ByVal catalog As String) As String
            Dim lsModule As String = Nothing

            Dim sqlstr As String
            sqlstr = "[dbo].[usp_GetMaterialsInUse]"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.AddWithValue("@Material", catalog)

            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    ' Obtiene los meses asignados al catálogo y la fecha actual del servidor de base de datos.

                    If reader.Read Then
                        If Not reader.IsDBNull(0) Then
                            lsModule = reader.GetString(1)
                        End If
                    End If

                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return lsModule
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
        ''' Agrega un catálogo junto con sus méses de vida útil o si el catálogo ya
        ''' existe, se actualiza la cantidad de meses.
        ''' </summary>
        ''' <param name="Catalog">Número de catálogo</param>
        ''' <param name="Months">Meses de vida útil del catálogo</param>
        ''' <remarks></remarks>
        Public Sub UpdateShelfLife(ByVal catalog As String, ByVal months As Int32, ByVal UOMID As Int32, ByVal area As Int32)
            Dim plsql As String = _
                "if exists(select Meses from TblCatStd where Catalogo = @Catalog) " + _
                "begin " + _
                "	update TblCatStd set Meses = @Months, Area = @Area, UOMID = @UOMID where Catalogo = @Catalog " + _
                "end " + _
                "else " + _
                "begin " + _
                "	insert into TblCatStd (Catalogo, Meses, Area, UOMID) values ( @Catalog, @Months, @Area, @UOMID ) " + _
                "end"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = plsql
            command.Parameters.AddWithValue("@Catalog", catalog)
            command.Parameters.AddWithValue("@Months", months)
            command.Parameters.AddWithValue("@Area", area)
            command.Parameters.AddWithValue("@UOMID", UOMID)

            Try
                connection.Open()
                command.ExecuteNonQuery()
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Calcula la fecha de expiración de un catálogo basandose en la fecha actual
        ''' </summary>
        ''' <param name="Catalog">Número de catálogo</param>
        ''' <returns>La fecha de expiración calculada para el catálogo</returns>
        ''' <remarks></remarks>
        Public Function GetExpirationDate(ByVal catalog As String, _
                                          ByVal ManufDate As ShortDate, _
                                          ByRef Months As String) As ShortDate
            Dim ExpirationDate As ShortDate

            Dim sqlstr As String
            sqlstr = "Select Meses, UOMID, GETDATE() from TblCatStd where Catalogo=@Catalog"

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
                    If reader.Read() Then

                        '----------------
                        ' Obtiene los meses asignados al catálogo y la fecha actual del servidor de base de datos.
                        '----------------
                        Dim QtyToAdd As Int32 = Convert.ToInt32(reader.GetDecimal(0))
                        Dim UOMID As Integer = reader.GetInt32(1)
                        'Dim Today As Date = reader.GetDateTime(2)

                        Months = QtyToAdd.ToString()
                        '----------------
                        ' Calcula una fecha temporal de fecha de expiración
                        '----------------
                        'Dim TempExpDate As Date = DateAdd(DateInterval.Month, Months, Today)
                        Dim TempExpDate As Date
                        Dim sTempExpDate As ShortDate = New ShortDate()

                        If (UOMID.Equals(1) Or UOMID.Equals(2)) Then
                            If (UOMID.Equals(1)) Then
                                TempExpDate = DateAdd(DateInterval.Day, QtyToAdd, ManufDate.ToDate())
                            ElseIf (UOMID.Equals(2)) Then
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
                    End If
                End Using

            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try

            Return ExpirationDate
        End Function

        ''' <summary>
        ''' Actualiza la tabla de fecha de expiración
        ''' </summary>
        ''' <param name="username">Nombre de usuario</param>
        ''' <param name="Order">Número de orden de trabajo</param>
        ''' <param name="Catalog">Número de catálogo</param>
        ''' <param name="ExpDate">Fecha de expiración de la orden</param>
        ''' <remarks></remarks>
        Public Sub UpdateExpDate(ByVal username As String, _
                                 ByVal order As String, _
                                 ByVal catalog As String, _
                                 ByVal expDate As ShortDate, _
                                 ByVal plant As String, _
                                 ByVal months As String, _
                                 ByVal lot As String)

            'Commented by Manuel Beltran (30.Mar.2012)
            'Changed query to include more parameters and not rename previos record
            'Dim plsql As String = _
            '"update TblExpdate SET Orden = @Order + 'X' WHERE Orden = @Order " + _
            '"INSERT INTO TblExpDate (Orden, ExpDate, Usuario, FechaHora, Partnumber)  VALUES  (@Order, cast(@ExpDate as smalldatetime), @UserName, getdate(), @Catalog)"

            Dim expdateObj As Legacy.ExpirationDate = New Legacy.ExpirationDate()
            Dim manufDate As Legacy.ShortDate = New Legacy.ShortDate()
            manufDate = expdateObj.GetManufDate(order)

            'Prepare transaction for inserting expiration date
            Dim plsql As String = _
                "DELETE FROM [dbo].[TblExpDate] WHERE ([Orden] = @Order) " + vbCrLf + _
                "INSERT INTO [dbo].[TblExpDate] ( " + vbCrLf + _
                "   [Orden],      [ExpDate], [Usuario],   [FechaHora], " + vbCrLf + _
                "   [Partnumber], [Meses],   [Plant],     [Lote], " + vbCrLf + _
                "   [UpdDate],    [Status],  [ErrorCode], [ErrorReason] " + vbCrLf + _
                " )  VALUES  ( " + vbCrLf + _
                "   @Order, cast(@ExpDate as smalldatetime), @UserName, @manufDate, " + vbCrLf + _
                "   @Catalog,    @months,    @plant,      @lot, "

            'If lot number is not empty then insert it "New" if it's empty then inserted as "Delivered"
            If (Not String.IsNullOrEmpty(lot)) Then
                plsql = plsql + _
                    "   NULL,        'N',        NULL,        NULL " + vbCrLf + _
                    ")"
            Else
                plsql = plsql + _
                    "   GETDATE(),   'D',        NULL,        NULL " + vbCrLf + _
                    ")"
            End If


            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand(plsql)
            command.Connection = connection
            command.CommandText = plsql
            command.Parameters.AddWithValue("@Order", order)
            command.Parameters.AddWithValue("@Catalog", catalog)
            command.Parameters.AddWithValue("@ExpDate", expDate.ToString())
            command.Parameters.AddWithValue("@UserName", username)
            command.Parameters.AddWithValue("@months", months)
            command.Parameters.AddWithValue("@plant", plant)
            command.Parameters.AddWithValue("@lot", lot)
            command.Parameters.AddWithValue("@manufDate", manufDate.ToDate())

            Try
                connection.Open()
                command.ExecuteNonQuery()

            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Consulta la fecha de expiración de una orden de trabajo
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo</param>
        ''' <returns>Fecha de expiración de la orden</returns>
        ''' <remarks></remarks>
        Public Function QueryExpirationDate(ByVal workOrder As String) As ShortDate
            Dim ExpirationDate As ShortDate

            Dim sqlstr As String
            sqlstr = "Select ExpDate from TblExpDate where Orden = @WorkOrder"

            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(dbCon)             'cambiocambio
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@WorkOrder", workOrder)

            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then

                        '----------------
                        ' Obtiene la fecha de expiración de la base de datos
                        '----------------

                        Dim ExpDate As Date = reader.GetDateTime(0)
                        ExpirationDate.Month = Month(ExpDate)
                        ExpirationDate.Day = Day(ExpDate)
                        ExpirationDate.Year = Year(ExpDate)
                    End If
                End Using

            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
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

        ''' <summary>
        ''' Consulta la fecha de expiración de una orden de trabajo
        ''' </summary>
        ''' <param name="workOrder">Número de orden de trabajo</param>
        ''' <returns>Fecha de expiración de la orden</returns>
        ''' <remarks></remarks>
        Public Function QueryExpirationDate(ByVal workOrder As String, ByRef PartNumber As String) As ShortDate
            Dim ExpirationDate As ShortDate
            Dim sqlstr As String
            sqlstr = "Select ExpDate,PartNumber from TblExpDate where Orden = @WorkOrder"
            'Commented by Manuel Beltran (11.Apr.2012)
            'Dim connection As SqlConnection = New SqlConnection(m_Neurona)
            Dim connection As SqlConnection = New SqlConnection(m_Apls)
            Dim command As SqlCommand = New SqlCommand()
            command.Connection = connection
            command.CommandText = sqlstr
            command.Parameters.AddWithValue("@WorkOrder", workOrder)
            Try
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        '----------------
                        ' Obtiene la fecha de expiración de la base de datos
                        '----------------
                        Dim ExpDate As Date = reader.GetDateTime(0)
                        ExpirationDate.Month = Month(ExpDate)
                        ExpirationDate.Day = Day(ExpDate)
                        ExpirationDate.Year = Year(ExpDate)
                        PartNumber = reader.GetString(1)
                    End If
                End Using
            Catch sqlex As SqlException
                Throw (sqlex)
            Finally
                If Not (connection Is Nothing) Then connection.Close()
            End Try
            Return ExpirationDate
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
