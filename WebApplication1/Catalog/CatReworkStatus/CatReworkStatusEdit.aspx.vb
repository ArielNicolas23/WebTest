Public Class Catalog_CatReworkStatusEdit
    Inherits System.Web.UI.Page
    Dim isEdit As Boolean
    Dim idStatus As Guid?
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Esta variable debe venir de la otra pantalla para checar si es agregar o editar
        isEdit = CType(Session.Item("isEdit"), Boolean)
        lblMessage.Text = ""

        'Debemos traer el ID del registro que vamos a editar
        If isEdit Then
            idStatus = Guid.Parse(Session.Item("idStatus"))
        End If
        If Not Page.IsPostBack Then
            If isEdit Then
                Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
                lblTitle.Text = "Editar Unidad"
                'Traer la información del registro a editar y desplegar su información
                Dim datos As DataTable = catReworkStatus.SelectOne(idStatus)
                txtStatus.Text = datos.Rows(0).ItemArray(1).ToString
                chkIsRework.Checked = datos.Rows(0).ItemArray(2)
                catReworkStatus = Nothing
            End If
        End If

    End Sub



    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try
            lblMessage.Text = ""

            Dim catReworkStatus As CatReworkStatus = New CatReworkStatus()
            Dim strStatus As String = txtStatus.Text.Trim.ToUpper()
            Dim boolStatus As Boolean = chkIsRework.Checked

            If (strStatus = "") Then
                lblMessage.Text = "Favor de escribir un Código de Estatus"
                Return
            End If

            If (catReworkStatus.AlreadyExistSAPStatus(strStatus)) Then
                lblMessage.Text = "No es posble guardar los cambios debido a que ya existe un Estatus de SAP con el código ingresado: [" + strStatus + "]"
            Else
                If isEdit Then
                    catReworkStatus.Update(idStatus, strStatus, boolStatus, "Admin")
                    catReworkStatus = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatReworkStatus.aspx", True)
                Else
                    catReworkStatus.Insert(strStatus, boolStatus, True, "Admin")
                    catReworkStatus = Nothing
                    Session("isEdit") = "False"
                    Response.Redirect("CatReworkStatus.aspx", True)
                End If
            End If

        Catch ex As Exception
            lblMessage.Text = "Ocurrió un error al intentar guardar los datos: " + ex.Message

        End Try
    End Sub



    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'Agregar código para regresar a la ventana anterior
        Session("isEdit") = "False"
        Response.Redirect("CatReworkStatus.aspx", True)
    End Sub

End Class