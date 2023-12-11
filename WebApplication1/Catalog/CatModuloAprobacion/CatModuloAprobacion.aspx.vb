Imports System.Data.SqlClient
Imports System.Diagnostics.Eventing
Imports System.Threading.Tasks
Imports AjaxControlToolkit
Public Class CatModuloAprobacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("DataTable") = Initdt()
        Dim conn As New SqlConnection("Data Source=J96W2D3-L1\SQLEXPRESS;Initial Catalog=FechaExpiracion;Integrated Security=True")
        conn.Open()
        Dim cmd As New SqlCommand("spED_ED_ModelsChangesHeader_select", conn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Aprobador", "orizag2")
        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        dt.Clear()
        da.Fill(dt)
        gvPendientes.DataSource = dt
        gvPendientes.DataBind()
        cmd.ExecuteNonQuery()
        conn.Close()



    End Sub

    Protected Function Initdt()
        Dim data = New DataTable("Result")
        data.Columns.Add("ChangeNumber", GetType(String))
        data.Columns.Add("OriginUser", GetType(String))
        data.Columns.Add("OriginComment", GetType(String))
        data.Columns.Add("ApproverUser", GetType(String))
        data.Columns.Add("ModifiedOn", GetType(String))
        data.Columns.Add("ApprovalStatus", GetType(String))
        Return data
    End Function

    Protected Sub gvPendientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPendientes.SelectedIndexChanged

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

        Dim conn As New SqlConnection("Data Source=J96W2D3-L1\SQLEXPRESS;Initial Catalog=FechaExpiracion;Integrated Security=True")
        conn.Open()
        Dim cmd As New SqlCommand("spED_ED_ModelsChangesHeader_SelectByApprovalStatus", conn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Aprobador", "orizag2")
        cmd.Parameters.AddWithValue("@Estatus", DropDownList1.Text)
        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        dt.Clear()
        da.Fill(dt)
        gvPendientes.DataSource = dt
        gvPendientes.DataBind()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ModalPopupExtender1.Show()
        Dim conn As New SqlConnection("Data Source=J96W2D3-L1\SQLEXPRESS;Initial Catalog=FechaExpiracion;Integrated Security=True")
        conn.Open()
        Dim cmd As New SqlCommand("spED_ED_ModelsChangesHeader_SelectByID", conn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@ID", "0bb769b7-075a-46d9-9845-c53e1c1e3306")
        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        dt.Clear()
        da.Fill(dt)
        gvSeleccionado.DataSource = dt
        gvSeleccionado.DataBind()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub
End Class