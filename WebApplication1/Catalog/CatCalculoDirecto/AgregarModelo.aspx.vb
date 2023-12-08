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

        EnableButtons()
    End Sub

    Protected Function Initdt()
        Dim data = New DataTable("Result")
        data.Columns.Add("Modelo", GetType(String))
        data.Columns.Add("VidaUtil", GetType(String))
        data.Columns.Add("Unidad", GetType(String))
        data.Columns.Add("Usuario", GetType(String))
        data.Columns.Add("IdUnidad", GetType(String))
        Return data
    End Function

    Private Sub CleanAddData()
        lblMessage.Text = ""
        txtModel.Text = ""
        txtLifeSpan.Text = ""
    End Sub

    Private Sub CleanTable()
        Session("DataTable") = Initdt()
        gvModelos.DataSource = Session("DataTable")
        gvModelos.DataBind()
    End Sub

    Private Sub EnableButtons()
        Dim dt As DataTable = Session("DataTable")

        If dt.Rows.Count > 0 Then
            cmdCancelChange.Enabled = True
            cmdOpenApprove.Enabled = True
        Else
            cmdCancelChange.Enabled = False
            cmdOpenApprove.Enabled = False
        End If
    End Sub

    Protected Sub AddModel_Click(sender As Object, e As EventArgs) Handles cmdModel.Click
        Try
            lblMessage.Text = ""

            Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
            Dim strModel As String = txtModel.Text.Trim.ToUpper
            Dim strLifeSpan As String = txtLifeSpan.Text.Trim.ToUpper
            Dim strUnit As String = ddlUnit.SelectedItem.Text.Trim.ToUpper
            Dim idUnit As String = ddlUnit.SelectedValue

            If (strModel = "") Then
                lblMessage.Text = "Favor de escribir el nombre del Modelo"
                Return
            End If

            If (Not Regex.IsMatch(strLifeSpan, "^[0-9 ]+$")) Then
                lblMessage.Text = "Favor de escribir un valor numerico para la Vida Útil"
                Return
            End If

            If (strLifeSpan = "") Then
                lblMessage.Text = "Favor de seleccionar una Unidad para el Modelo"
                Return
            End If

            If (modelsChange.AlreadyExistModelChange(Nothing, strModel)) Then
                lblMessage.Text = "No es posble ingresar el Modelo debido a que ya existe un Modelo activo registrado con ese nombre: [" + strModel + "]"
            Else
                Dim dt As DataTable = Session("DataTable")
                Dim row As DataRow = dt.NewRow
                row("Modelo") = strModel
                row("VidaUtil") = strLifeSpan
                row("Unidad") = strUnit
                row("Usuario") = "Origin User"        'Agregar función para obtener al usuario
                row("IdUnidad") = idUnit

                dt.Rows.Add(row)

                gvModelos.DataSource = dt
                gvModelos.DataBind()

                Session("DataTable") = dt

                EnableButtons()
                CleanAddData()
            End If
        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar agregar los datos: " + ex.Message
        End Try

    End Sub

    Protected Sub gvModelos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvModelos.RowDeleting
        Dim dt As DataTable = Session("DataTable")
        dt.Rows.Remove(dt.Rows.Item(e.RowIndex))
        gvModelos.DataSource = dt
        gvModelos.DataBind()

        Session("DataTable") = dt

        EnableButtons()
    End Sub

    Protected Sub cmdCancelChange_Click(sender As Object, e As EventArgs) Handles cmdCancelChange.Click
        CleanTable()
        EnableButtons()
    End Sub

    Protected Sub cmdOpenApprove_Click(sender As Object, e As EventArgs) Handles cmdOpenApprove.Click
        Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
        Dim dt As DataTable = Session("DataTable")
        Dim foundRepeated As Boolean = False
        Dim listRepeated As String = ""

        For Each row As DataRow In dt.Rows
            If (False) Then 'modelsChange.AlreadyExistModelChange(Nothing, row("Modelo"))) Then
                foundRepeated = True
                listRepeated += "[" + row("Modelo") + "], "
            End If
        Next row

        If (foundRepeated) Then
            listRepeated = listRepeated.Substring(0, listRepeated.Length - 1)
            lblMessage.Text = "Se ha detectado que alguno de los modelos ha sido ingresado al sistema durante el flujo de configuración: " + listRepeated + ". Favor de rectificar los datos"
        Else
            ApproveModal.Show()
        End If
    End Sub

    Protected Sub cmdCancelModal_Click(sender As Object, e As EventArgs) Handles cmdCancelModal.Click
        ApproveModal.Hide()
    End Sub

    Protected Sub cmdAcceptChange_Click(sender As Object, e As EventArgs) Handles cmdAcceptChange.Click
        'Validaciones de los usuarios

        Dim modelsChange As ED_ModelsChanges = New ED_ModelsChanges()
        Dim approvedModelsChange As ED_ApprovedModelsChanges = New ED_ApprovedModelsChanges()

        Dim dt As DataTable = Session("DataTable")
        Dim changeNumber As Integer = 1
        Dim originUser As String = "Origin User"             'Agregar función para obtener al usuario
        Dim originComment As String = txtApproveMessage.Text
        Dim approverUser As String = txtApprover.Text
        Dim approvalStatus As String = "Pendiente"
        Dim isActive As String = True

        Dim idApprovedModelsChanges As Guid
        Dim idUnit As Guid
        Dim model As String
        Dim lifeSpan As Integer
        Dim modelChangeStatus As String = "Pendiente"

        'Validaciones extra por si acaso


        idApprovedModelsChanges = approvedModelsChange.Insert(changeNumber, originUser, originComment, approverUser, approvalStatus, isActive, originUser)
        For Each row As DataRow In dt.Rows
            idUnit = Guid.Parse(row("IdUnidad"))
            model = row("Modelo")
            lifeSpan = row("VidaUtil")
            modelsChange.Insert(idApprovedModelsChanges, idUnit, model, lifeSpan, modelChangeStatus, originUser, isActive, originUser)
        Next row

        ApproveModal.Hide()
        MsgBox("Se ha completado exitósamente el registro de los cambios", MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, "Completado")
        CleanTable()
    End Sub
End Class