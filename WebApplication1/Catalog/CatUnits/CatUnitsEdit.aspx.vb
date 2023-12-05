Public Class Catalog_CatUnitsEdit
    Inherits System.Web.UI.Page
    Dim isEdit As Boolean
    Dim idUnit As Guid?
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Esta variable debe venir de la otra pantalla para checar si es agregar o editar
        isEdit = CType(Session.Item("isEdit"), Boolean)
        lblMessage.Text = ""

        'Debemos traer el ID del registro que vamos a editar
        If isEdit Then
            idUnit = Guid.Parse(Session.Item("idUnit"))
        End If
        If Not Page.IsPostBack Then
            If isEdit Then
                Dim catUnits As CatUnits = New CatUnits()
                lblTitle.Text = "Editar Unidad"
                'Traer la información del registro a editar y desplegar su información
                Dim datos As DataTable = catUnits.SelectOne(idUnit)
                txtUnit.Text = datos.Rows(0).ItemArray(1).ToString
                txtUnitValue.Text = datos.Rows(0).ItemArray(2).ToString
                catUnits = Nothing
            End If
        End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            lblMessage.Text = ""

            Dim catUnits As CatUnits = New CatUnits()
            Dim strUnit As String = txtUnit.Text.Trim.ToUpper()
            Dim strUnitValue As String = Convert.ToInt16(txtUnitValue.Text.Trim.ToUpper())

            If (strUnit = "") Then
                lblMessage.Text = "Favor de escribir un nombre para la Unidad"
                Return
            End If

            If (strUnitValue = "") Then
                lblMessage.Text = "Favor de escribir un valor para la Unidad"
                Return
            End If

            If (catUnits.AlreadyExistUnit(strUnit)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Unidad con el nombre ingresado: [" + strUnit + "]"
            Else
                If isEdit Then
                    catUnits.Update(idUnit, strUnit, strUnitValue, True, "Admin")
                    catUnits = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatUnits.aspx", True)
                Else
                    catUnits.Insert(strUnit, strUnitValue, True, "Admin")
                    catUnits = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatUnits.aspx", True)
                End If
            End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para el valor de la Unidad"

        Catch ex As OverflowException
            lblMessage.Text = "El valor de la Unidad es demasiado grande para guardar"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Session("isEdit") = "False"
        Response.Redirect("CatUnits.aspx", True)
    End Sub

End Class