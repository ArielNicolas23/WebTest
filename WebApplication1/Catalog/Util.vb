Option Strict On

Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions


Public Class Util
    ''' <summary>
    ''' Determina si una cadena representa un entero positivo o cero
    ''' </summary>
    ''' <param name="Value">Cadena a ser evaluada.</param>
    ''' <returns>True si la cadena representa un entero positivo o cero. En cualquier otro caso retorna False.</returns>
    ''' <remarks></remarks>
    Public Shared Function IsInteger(ByVal Value As String) As Boolean
        If Value = String.Empty Then Return False
        For Each chr As Char In Value
            If Not Char.IsDigit(chr) Then
                Return False
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Determina si una cadena representa una fecha en formato MM/dd/yyyy
    ''' </summary>
    ''' <param name="Value">Cadena a ser evaluada.</param>
    ''' <returns>True si la cadena representa una fecha en formato MM/dd/yyyy. En cualquier otro caso retorna False.</returns>
    ''' <remarks></remarks>
    Public Shared Function IsDate(ByVal Value As String) As Boolean
        Dim expression As Regex = New Regex("(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d")
        Return expression.IsMatch(Value)
    End Function

    ''' <summary>
    ''' Establece texto y color a un objeto tipo Label
    ''' </summary>
    ''' <param name="pLabel"></param>
    ''' <param name="psColor"></param>
    ''' <param name="psMessage"></param>
    ''' <remarks></remarks>
    Public Shared Sub gfnSetMessage(ByRef pLabel As Label, ByVal psColor As Drawing.Color, ByVal psMessage As String, ByVal pBold As Boolean)
        pLabel.ForeColor = psColor
        pLabel.Font.Bold = pBold
        pLabel.Text = psMessage
        pLabel.Visible = True
    End Sub
End Class
