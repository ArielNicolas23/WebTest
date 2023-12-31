﻿Imports System.Drawing
Imports System.Net

Public Class _Default

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateGrid()
        End If
    End Sub


    Protected Sub PopulateGrid()

        Dim components As CatUnits = New CatUnits()
        GridView1.DataSource = components.SelectAll("", False)
        GridView1.DataBind()


        components = Nothing

    End Sub
    <System.Web.Services.WebMethod>
    Public Shared Function GetADUsers(prefixText As String, count As Integer) As List(Of String)
        Dim data = New DataTable()
        data.Columns.Add("Name", GetType(String))
        data.Columns.Add("UserName", GetType(String))
        data.Columns.Add("Email", GetType(String))
        data = Security.UserAD.GetAllUsers(prefixText, data)

        Dim users As List(Of String) = New List(Of String)()
        For Each activeDirectoryData As DataRow In data.Rows
            users.Add(activeDirectoryData("Name") & " █ " & activeDirectoryData("UserName"))
        Next

        Return users
    End Function


    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click

        LblMessage.Text = String.Empty

        Dim strComponent As String = TxtComponent.Text.Trim.ToUpper()
        Dim strDescription As String = TxtDescription.Text.Trim.ToUpper()

        TxtComponent.Text = String.Empty
        TxtDescription.Text = String.Empty

        If strComponent.Equals(String.Empty) Or strDescription.Equals(String.Empty) Then
            Return
        End If

        Dim components As CatUnits = New CatUnits()

        components.Insert(strComponent, strDescription, True, "Admin", "")
        LblMessage.ForeColor = Color.Blue
            LblMessage.Text = "El componente ha sido agregado exitosamente."


            components = Nothing

        PopulateGrid()
    End Sub

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
            GridView1.EditIndex = -1
            PopulateGrid()
        End Sub

        Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing

            GridView1.EditIndex = e.NewEditIndex
            PopulateGrid()

        End Sub

        Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
        Dim names As DataKeyArray = GridView1.DataKeys()
        Dim idUnit As Guid = Guid.Parse(names.Item(e.RowIndex).Value.ToString())
        Dim unit As String = CType((row.Cells(1).Controls(0)), TextBox).Text
        Dim unitValue As String = CType((row.Cells(2).Controls(0)), TextBox).Text
        Dim isActive As Boolean = CType((row.Cells(3).Controls(0)), CheckBox).Checked

        Dim components As CatUnits = New CatUnits()
        components.Update(idUnit, unit, unitValue, isActive, "Admin", "")
        components = Nothing

            GridView1.EditIndex = -1
            PopulateGrid()
        End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    'Protected Sub Lnk_ewapp_menu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lnk_ewapp_menu.Click
    '    LblMessage.Text = String.Empty
    '    If m_Profile.IsAdministrator Or m_Profile.IsSupervisor Or m_Profile.IsStandardUser Then
    '        Response.Redirect("./ewapp_stdPediatricsSubBom.aspx")
    '    Else
    '        LblMessage.Text = "Acceso negado al módulo solicitado."
    '    End If
    'End Sub

End Class
