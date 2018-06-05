Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Collections.Generic


Public Class DAO_Commons
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Private tipo_ordem As String = "TAMANHO"


    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Public Function RetornaConexao() As String
        Return strConn
    End Function


    Public Function myDataTable(ByVal SQL As String) As DataTable
        Dim cn As OleDbConnection
        Dim dsTemp As DataSet
        Dim dsCmd As OleDbDataAdapter

        cn = New OleDbConnection(strConn)
        cn.Open()

        dsCmd = New OleDbDataAdapter(SQL, cn)
        dsTemp = New DataSet()
        dsCmd.Fill(dsTemp, "myQuery")
        cn.Close()
        Return dsTemp.Tables(0)
    End Function

End Class
