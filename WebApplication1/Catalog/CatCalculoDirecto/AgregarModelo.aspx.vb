Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports AjaxControlToolkit

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session("DataTable") = Initdt()
            Dim catUnits As CatUnits = New CatUnits()
            ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
            ddlUnit.DataTextField = "Unit"
            ddlUnit.DataValueField = "IdUnit"
            ddlUnit.DataBind()
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

    Protected Sub AddModel_Click(sender As Object, e As EventArgs) Handles cmdModel.Click
        Dim strModelo As String = txtModel.Text.Trim.ToUpper
        Dim strVida As String = txtLifeSpan.Text.Trim.ToUpper
        Dim strUnidad As String = ddlUnit.SelectedItem.Text.Trim.ToUpper


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

    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click
        Session("DataTable") = Initdt()
        gvModelos.DataSource = Session("DataTable")
        gvModelos.DataBind()
    End Sub

    Protected Sub cmdOpenApprove_Click(sender As Object, e As EventArgs) Handles cmdOpenApprove.Click
        ApproveModal.Show()
    End Sub

    Protected Sub cmdCancelModal_Click(sender As Object, e As EventArgs) Handles cmdCancelModal.Click

    End Sub

    Protected Sub cmdAcceptChange_Click(sender As Object, e As EventArgs) Handles cmdAcceptChange.Click

    End Sub

    Protected Sub txtApprover_TextChanged(sender As Object, e As EventArgs) Handles txtApprover.TextChanged


    End Sub

End Class