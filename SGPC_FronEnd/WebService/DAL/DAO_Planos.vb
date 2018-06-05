Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic


Public Class DAO_Planos

    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

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

    ''' <summary>
    ''' Retorna a lista de fornecedores
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPlanosByFornecedor(ByVal pCodigoFornecedor As Integer) As List(Of AppPlano)

        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppPlano)

        Dim strSQL As String = ""
        strSQL = strSQL + "select codigo_plano,nvl(t.plano,'-')plano, t.codigo_operadora from OPERADORAS_PLANOS t "
        strSQL = strSQL + " where t.codigo_operadora=(select nvl(codigo_operadora,0) from fornecedores where codigo='" & pCodigoFornecedor & "')"
        strSQL = strSQL + " order by t.plano"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppPlano(reader.Item("codigo_plano").ToString, reader.Item("plano").ToString, reader.Item("codigo_operadora").ToString)
                _list.Add(_registro)
            End While
        End Using

        Return _list
    End Function

End Class
