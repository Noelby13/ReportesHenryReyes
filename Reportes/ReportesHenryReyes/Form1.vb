Imports Microsoft.Reporting.WinForms
Public Class Form1
    Dim tblEmp As New BdHRDataSet.RptJobEmployeesDataTable
    Dim rptEmpleados As New BdHRDataSetTableAdapters.RptJobEmployeesTableAdapter
    Dim tbl As New DataTable
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        llenarGrid()
    End Sub

    Sub llenarGrid()
        rptEmpleados.Fill(tblEmp)
        DgvRegistro.DataSource = tblEmp
        DgvRegistro.Refresh()
        GroupBox2.Text = "Registros Guardados :  &" & DgvRegistro.Rows.Count
    End Sub

    Private Function crearTabla(query) As DataTable

        Dim tbl As New DataTable("tblEmp")

        tbl.Columns.Add("job_title")
        tbl.Columns.Add("first_name")
        tbl.Columns.Add("last_name")
        tbl.Columns.Add("email")
        tbl.Columns.Add("phone_number")
        tbl.Columns.Add("hire_date")
        tbl.Columns.Add("salary")

        Dim fila As DataRow
        For Each emp In query
            fila = tbl.NewRow
            fila("job_title") = emp.job_title
            fila("first_name") = emp.first_name
            fila("last_name") = emp.last_name
            fila("email") = emp.email
            fila("phone_number") = emp.phone_number
            fila("hire_date") = emp.hire_date
            fila("salary") = emp.salary
            tbl.Rows.Add(fila)

        Next

        Return tbl
    End Function

    Private Sub realizarBusqueda()
        Dim dato As String = TxtDato.Text & "*"
        Dim campo As String = "emp.job_title"
        Dim query = From emp In tblEmp
                    Select emp.job_title, emp.first_name, emp.last_name, emp.email, emp.phone_number, emp.hire_date, emp.salary

        Select Case CmbCampo.SelectedIndex
            Case -1 Or 0
                query = From emp In tblEmp
                        Where emp.job_title Like dato
                        Select emp.job_title, emp.first_name, emp.last_name, emp.email, emp.phone_number, emp.hire_date, emp.salary

            Case 2
                query = From emp In tblEmp
                        Where emp.last_name Like dato
                        Select emp.job_title, emp.first_name, emp.last_name, emp.email, emp.phone_number, emp.hire_date, emp.salary
        End Select

        tbl = crearTabla(query)
        DgvRegistro.DataSource = tbl
        DgvRegistro.Refresh()
        GroupBox2.Text = "Registros Encontrados :  &" & DgvRegistro.Rows.Count
    End Sub

    Sub verReporte(ByVal t As DataTable, ByVal nombreDS As String, ByVal nombreRpto As String)
        Try
            Dim rpt As New ReportDataSource(nombreDS, t)
            With FrmVistaPrevia
                .ReportViewer1.LocalReport.DataSources.Clear()
                .ReportViewer1.LocalReport.DataSources.Add(rpt)
                .ReportViewer1.LocalReport.ReportPath = nombreRpto
                .ReportViewer1.Refresh()

            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al cargar el reporte")
        End Try
    End Sub

    Private Sub BtnMostrar_Click(sender As Object, e As EventArgs) Handles BtnMostrar.Click
        realizarBusqueda()
    End Sub
End Class
