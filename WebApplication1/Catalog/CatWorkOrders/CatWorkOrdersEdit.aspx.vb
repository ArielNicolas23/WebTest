Imports System.Net.NetworkInformation

Public Class Catalog_CatWorkOrdersEdit
    Inherits System.Web.UI.Page
    Dim isEdit As Boolean
    Dim idWo As Guid?

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Esta variable debe venir de la otra pantalla para checar si es agregar o editar
        isEdit = CType(Session.Item("isEdit"), Boolean)
        lblMessage.Text = ""

        'Debemos traer el ID del registro que vamos a editar
        If isEdit Then
            idWo = Guid.Parse(Session.Item("idWo"))
        End If
        If Not Page.IsPostBack Then
            If isEdit Then
                Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
                lblTitle.Text = "Editar Unidad"
                'Traer la información del registro a editar y desplegar su información
                Dim datos As DataTable = catReworkOrders.SelectOne(idWo)
                txtWorkOrder.Text = datos.Rows(0).ItemArray(1).ToString
                Select Case datos.Rows(0).ItemArray(2).ToString
                    Case "MÓDULO 1"
                        cmbArea.SelectedIndex = 0
                    Case "MÓDULO 2"
                        cmbArea.SelectedIndex = 1
                    Case "MÓDULO 3"
                        cmbArea.SelectedIndex = 2
                    Case "MÓDULO 4"
                        cmbArea.SelectedIndex = 3
                End Select

                chkIsRework.Checked = datos.Rows(0).ItemArray(3)
                    catReworkOrders = Nothing
                End If
            End If

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try
            lblMessage.Text = ""

            Dim catReworkOrders As CatReworkOrders = New CatReworkOrders()
            Dim strWo As String = Convert.ToInt32(txtWorkOrder.Text.Trim.ToUpper())
            Dim strModulo As String = cmbArea.Text.Trim.ToUpper()
            Dim boolRework As Boolean = chkIsRework.Checked

            If (strWo = "") Then
                lblMessage.Text = "Favor de escribir una Orden de Trabajo"
                Return
            End If

            If (strWo.Length < 9 Or strWo.Length > 9) Then
                lblMessage.Text = "El número de Orden de Trabajo debe contener 9 dígitos"
                Return
            End If

            If (strModulo = "") Then
                lblMessage.Text = "Favor de seleccioanr un Módulo para la Orden"
                Return
            End If

            If (catReworkOrders.AlreadyExistWorkOrder(Nothing, strWo)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe una Orden de Trabajo con el número ingresado: [" + strWo + "]"
            Else
                If isEdit Then
                    catReworkOrders.Update(idWo, strWo, strModulo, boolRework, "Admin")
                    catReworkOrders = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatWorkOrders.aspx", True)
                Else
                    catReworkOrders.Insert(strWo, strModulo, boolRework, True, "Admin")
                    catReworkOrders = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatWorkOrders.aspx", True)
                End If
            End If

        Catch ex As FormatException
            lblMessage.Text = "Favor de escribir un número válido para la Orden de Trabajo"

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message
        End Try

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Session("isEdit") = "False"
        Response.Redirect("CatWorkOrders.aspx", True)
    End Sub

End Class