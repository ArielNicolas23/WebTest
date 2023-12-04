
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Namespace Standard.JMM

    Public Class Components
        Protected m_Apls As String
        'Commented by Manuel Beltran (11.Apr.2012)
        'Protected m_Neurona As String
        'Protected m_Jde As String

        Public Sub New()
            m_Apls = ConfigurationManager.ConnectionStrings("TestDb").ConnectionString
            'Commented by Manuel Beltran (11.Apr.2012)
            'm_Neurona = ConfigurationManager.ConnectionStrings("Neurona").ConnectionString
            'm_Jde = ConfigurationManager.ConnectionStrings("Jde").ConnectionString
        End Sub

        Public Sub Insert(
            ByVal Unit As String,
            ByVal UnitValue As Integer,
            ByVal IsActive As Boolean,
            ByVal UserName As String)

            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "spED_CatUnits_Insert"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Unit", PartNumber)
                cmd.Parameters.AddWithValue("@UnitValue", Description)
                cmd.Parameters.AddWithValue("@IsActive", IsActive)
                cmd.Parameters.AddWithValue("@CreatedBy", UserName)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Sub

        Public Sub Update( _
            ByVal PartNumber As String, _
            ByVal Description As String, _
            ByVal IsActive As Boolean, _
            ByVal UserName As String)

            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "JMM.Components_Update"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber)
                cmd.Parameters.AddWithValue("@Description", Description)
                cmd.Parameters.AddWithValue("@IsActive", IsActive)
                cmd.Parameters.AddWithValue("@UserName", UserName)

                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Sub


        Public Function Exists(ByVal PartNumber As String) As Boolean
            Dim result As Boolean = False
            Dim oneComponent As DataTable = SelectOne(PartNumber)
            If oneComponent.Rows.Count > 0 Then
                result = True
            End If
            Return result
        End Function


        Public Function SelectOne(ByVal PartNumber As String) As DataTable

            Dim result As DataTable
            Dim row As DataRow

            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "JMM.Components_SelectOne"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@PartNumber", PartNumber)


                result = New DataTable("Result")
                result.Columns.Add("PartNumber", GetType(String))
                result.Columns.Add("Description", GetType(String))
                result.Columns.Add("IsActive", GetType(Boolean))

                conn.Open()

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()

                    row = result.NewRow()
                    row("PartNumber") = reader.GetString(0)
                    row("Description") = reader.GetString(1)
                    row("IsActive") = reader.GetBoolean(2)

                    result.Rows.Add(row)

                End While

            End Using

            Return result
        End Function


        Public Function SelectAll() As DataTable

            Dim result As DataTable
            Dim row As DataRow


            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "spED_CatUnits_SelectAll"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure


                result = New DataTable("Result")
                result.Columns.Add("Unit", GetType(String))
                result.Columns.Add("UnitValue", GetType(Integer))
                result.Columns.Add("IsActive", GetType(Boolean))

                conn.Open()

                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()

                    row = result.NewRow()
                    row("Unit") = reader.GetString(0)
                    row("UnitValue") = reader.GetString(1)
                    row("IsActive") = reader.GetBoolean(2)

                    result.Rows.Add(row)

                End While



            End Using

            Return result
        End Function


        Public Function DropDownList_SelectAll() As DataTable

            Dim result As DataTable
            Dim row As DataRow

            Using conn As New SqlConnection(Me.m_Apls)

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandText = "JMM.Components_SelectAll"
                cmd.Connection = conn
                cmd.CommandType = CommandType.StoredProcedure

                result = New DataTable("Result")
                result.Columns.Add("PartNumber", GetType(String))
                result.Columns.Add("Description", GetType(String))

                conn.Open()

                Dim reader As SqlDataReader = cmd.ExecuteReader()

                row = result.NewRow()
                row("PartNumber") = "---"
                row("Description") = "[Seleccione un componente]"
                result.Rows.Add(row)

                While reader.Read()

                    If reader.GetBoolean(2) = True Then
                        row = result.NewRow()
                        row("PartNumber") = reader.GetString(0)
                        row("Description") = reader.GetString(0) + " [" + reader.GetString(1) + "]"
                        result.Rows.Add(row)
                    End If

                End While

            End Using

            Return result
        End Function

    End Class
End Namespace

