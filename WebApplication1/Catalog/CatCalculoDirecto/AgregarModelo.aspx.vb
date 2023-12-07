Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("DataTable") = Initdt()
            Dim catUnits As CatUnits = New CatUnits()
            ddlUnidad.DataSource = CatUnits.SelectAll("", False).AsDataView
            ddlUnidad.DataTextField = "Unit"
            ddlUnidad.DataValueField = "IdUnit"
            ddlUnidad.DataBind()
        End If
    End Sub

    Protected Function Initdt()
        Dim data = New DataTable("Result")
        data.Columns.Add("Modelo", GetType(String))
        data.Columns.Add("VidaUtil", GetType(String))
        data.Columns.Add("Unidad", GetType(String))
        data.Columns.Add("Usuario", GetType(String))
        Return data
    End Function

    Protected Sub AddModelo_Click(sender As Object, e As EventArgs) Handles addModelo.Click
        Dim strModelo As String = txtModelo.Text.Trim.ToUpper
        Dim strVida As String = txtVida.Text.Trim.ToUpper
        Dim strUnidad As String = ddlUnidad.SelectedItem.Text.Trim.ToUpper


        Dim dt As DataTable = Session("DataTable")
        Dim row As DataRow = dt.NewRow
        row("Modelo") = strModelo
        row("VidaUtil") = strVida
        row("Unidad") = strUnidad
        row("Usuario") = "Admin"

        dt.Rows.Add(row)

        gvModelos.DataSource = dt
        gvModelos.DataBind()

        Session("DataTable") = dt
    End Sub

    Protected Sub gvModelos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvModelos.RowDeleting
        Dim dt As DataTable = Session("DataTable")
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
        gvModelos.DataSource = dt
        gvModelos.DataBind()

        Session("DataTable") = dt

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Session("DataTable") = Initdt()
        gvModelos.DataSource = Session("DataTable")
        gvModelos.DataBind()
    End Sub
End Class