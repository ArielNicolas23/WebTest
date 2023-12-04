Public Class Catalog_CatUnitsEdit
    Inherits System.Web.UI.Page
    Dim isEdit As Boolean
    Dim idUnit As Guid?
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Esta variable debe venir de la otra pantalla para checar si es agregar o editar
        isEdit = CType(Session.Item("isEdit"), Boolean)
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
        Dim strUnit As String = txtUnit.Text.Trim.ToUpper()
        Dim strUnitValue As String = txtUnitValue.Text.Trim.ToUpper()

        If isEdit Then
            Dim catUnits As CatUnits = New CatUnits()
            catUnits.Update(idUnit, strUnit, strUnitValue, True, "Admin")
            catUnits = Nothing
            Session("isEdit") = "False"
            Response.Redirect("CatUnits.aspx", True)
        Else
            Dim catUnits As CatUnits = New CatUnits()
            catUnits.Insert(strUnit, strUnitValue, True, "Admin")
            catUnits = Nothing
            Session("isEdit") = "False"
            Response.Redirect("CatUnits.aspx", True)
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Session("isEdit") = "False"
        Response.Redirect("CatUnits.aspx", True)
    End Sub

End Class