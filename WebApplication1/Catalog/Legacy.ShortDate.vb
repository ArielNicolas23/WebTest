Imports Microsoft.VisualBasic

Namespace Legacy

    ''' <summary>
    ''' Representa una fecha en formato corto (mm/dd/yyyy)
    ''' Esta estructura se diseñó para poder ejercer un control mas preciso
    ''' sobre los elementos individuales de una fecha (mes, día y año).
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure ShortDate
        Private mDay As Int32
        Private mMonth As Int32
        Private mYear As Int32

        ''' <summary>
        ''' Día
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Day() As Int32
            Get
                Return mDay
            End Get
            Set(ByVal Value As Int32)
                mDay = Value
            End Set
        End Property

        ''' <summary>
        ''' Mes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Month() As Int32
            Get
                Return mMonth
            End Get
            Set(ByVal Value As Int32)
                mMonth = Value
            End Set
        End Property

        ''' <summary>
        ''' Año
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Year() As Int32
            Get
                Return mYear
            End Get
            Set(ByVal Value As Int32)
                mYear = Value
            End Set
        End Property

        ''' <summary>
        ''' Clears local variables data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearData()
            mDay = 0
            mMonth = 0
            mYear = 0
        End Sub

        ''' <summary>
        ''' Para crear un objeto de tipo ShortDate a partir de una cadena que representa
        ''' una fecha con el siguiente formato 'mm/dd/yyyy'
        ''' </summary>
        ''' <param name="ExpDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ExpDate As String)
            ClearData()

            If (Not IsDate(ExpDate)) Then Return

            mDay = Microsoft.VisualBasic.Day(Convert.ToDateTime(ExpDate))
            mMonth = Microsoft.VisualBasic.Month(Convert.ToDateTime(ExpDate))
            mYear = Microsoft.VisualBasic.Year(Convert.ToDateTime(ExpDate))
        End Sub

        ''' <summary>
        ''' Para crear un objeto de tipo ShortDate a partir de un objeto DateTime.
        ''' </summary>
        ''' <param name="ExpDate"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ExpDate As DateTime)
            ClearData()
            mDay = Microsoft.VisualBasic.Day(ExpDate)
            mMonth = Microsoft.VisualBasic.Month(ExpDate)
            mYear = Microsoft.VisualBasic.Year(ExpDate)
        End Sub

        ''' <summary>
        ''' Para determinar si el objeto ShortDate representa una fecha nula.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsEmptyDate() As Boolean
            If mMonth = 0 And mDay = 0 And mYear = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Obtiene una representación del objeto ShortDate en cadena con el siguiente
        ''' formato 'mm/dd/yyyy'.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function ToString() As String
            Return mMonth.ToString("00") + "/" + mDay.ToString("00") + "/" + mYear.ToString("0000")
        End Function

        ''' <summary>
        ''' Obtiene una representación del objeto ShortDate en formato Date
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function ToDate() As Date
            Dim DateAsString As String
            Dim cultureFormat As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            DateAsString = mMonth.ToString("00") + "/" + mDay.ToString("00") + "/" + mYear.ToString("0000")
            Return DateTime.Parse(DateAsString, cultureFormat)
        End Function
    End Structure
End Namespace
