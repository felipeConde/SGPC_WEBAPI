Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic

Public Class EstadoDAO
    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Private _strConn As String = ""
    'Private strConn As String = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=cl;"
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

    Public Function GetEstados() As List(Of Estado)
        Dim strSQL As String = "select codigo_estado, descricao from estados order by descricao"
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        Dim list As New List(Of Estado)
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                list.Add(New Estado(reader.Item(0), reader.Item(1)))
            End While
        End Using
        Return list
    End Function

    Public Function GetEstadoByDescricao(ByVal pDescricao As String) As Estado
        Dim strSQL As String = "select codigo_estado, descricao from estados where upper(descricao)='" & pDescricao.ToUpper & "' order by descricao"
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        Dim estado As Estado = Nothing
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                estado = New Estado(reader.Item(0), reader.Item(1))
            End While
        End Using
        Return estado
    End Function

    Public Function GetEstadosFromCidades() As List(Of Estado)
        Dim strSQL As String = "select distinct uf from cidades order by UF"
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        Dim list As New List(Of Estado)
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                list.Add(New Estado(reader.Item(0), reader.Item(1)))
            End While
        End Using
        Return list
    End Function
End Class
