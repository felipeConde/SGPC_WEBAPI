Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic

Public Class DAOFornecedores

    Private _strConn As String = ""

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


    Public Function GetFornecorees() As List(Of appFornecedor)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of appFornecedor)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_fantasia as descricao "
        strSQL = strSQL + " from fornecedores o"
        strSQL = strSQL + " order by nome_fantasia"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New appFornecedor(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

End Class
