Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.WebPages
Imports AjaxControlToolkit
Imports Microsoft.Ajax.Utilities

Public Class CatConfiguracionCalculoDirecto
    Inherits System.Web.UI.Page

    ' Variables de las conexiones a BD
    Dim modelChangesHeader As ED_ModelsChangesHeader = New ED_ModelsChangesHeader()
    Dim modelChanges As ED_ModelsChanges = New ED_ModelsChanges()
    Dim userPlaceholder As String

    ' Carga de página
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userProfile = CType(Session("UserProfile"), Security.UserProfile)
        userPlaceholder = userProfile.UserName.Split("\")(1)
        If Not Page.IsPostBack Then
            Dim catUnits As CatUnits = New CatUnits()
            ddlUnit.DataSource = catUnits.SelectAll("", False).AsDataView
            ddlUnit.DataTextField = "Unit"
            ddlUnit.DataValueField = "IdUnit"
            ddlUnit.DataBind()
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApproved())
        End If
    End Sub

    ' Llenado de tablas
    Protected Sub PopulateGrid(dgv As GridView, dt As DataTable)
        dgv.DataSource = dt
        dgv.DataBind()
    End Sub

    Protected Sub OnChangeIsChecked(sender As Object, e As EventArgs)
        Dim checkBox As CheckBox = TryCast(sender, CheckBox)

        If checkBox IsNot Nothing Then
            Dim row As GridViewRow = DirectCast(checkBox.Parent.Parent, GridViewRow)
            Dim id As Guid = Guid.Parse(dgvModelos.DataKeys(row.RowIndex).Value.ToString())
            Dim isChecked As Boolean = checkBox.Checked

            modelChanges.UpdateIsChecked(id, isChecked)
        End If
    End Sub

    Protected Sub dgvModelos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgvModelos.SelectedIndexChanged

    End Sub

    Protected Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click

        If txtModel.Text = "" And txtLifeSpan.Text = "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 3))
        End If
        If txtModel.Text <> "" And txtLifeSpan.Text = "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, "", Guid.Parse(ddlUnit.Text), 5))
        End If
        If txtModel.Text = "" And txtLifeSpan.Text <> "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter("", txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 6))
        End If
        If txtModel.Text <> "" And txtLifeSpan.Text <> "" Then
            PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter(txtModel.Text, txtLifeSpan.Text, Guid.Parse(ddlUnit.Text), 7))
        End If
        'PopulateGrid(dgvModelos, modelChanges.SelectByIdModelsChangesHeaderApprovedFilter())
    End Sub
End Class