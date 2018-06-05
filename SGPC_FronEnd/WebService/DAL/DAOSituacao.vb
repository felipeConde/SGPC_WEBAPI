Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOSituacao
    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    'Private _strConn As String = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=server;"
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

    Public Function InsereSituacao(ByVal psituacao As AppSituacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into SOLICITACOES_SITUACOES(CODIGO"
            strSQL = strSQL + ",SITUACAO,DESCRICAO) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from SOLICITACOES_SITUACOES)"
            strSQL = strSQL + ",'" + psituacao.Situacao.ToUpper + "'"
            strSQL = strSQL + ",'" + psituacao.Descricao.ToUpper + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
    End Function

    Public Function AtualizaSituacao(ByVal psituacao As AppSituacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update SOLICITACOES_SITUACOES set "
            strSQL = strSQL + "SITUACAO='" + psituacao.Situacao.ToUpper + "'"
            strSQL = strSQL + ",DESCRICAO='" + psituacao.Descricao.ToUpper + "'"
            strSQL = strSQL + " where CODIGO = '" + psituacao.Codigo.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
    End Function

    Public Function ExcluiSituacao(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete SOLICITACOES_SITUACOES "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetSituacaoById(ByVal pcodigo As Integer) As List(Of AppSituacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSituacao As New List(Of AppSituacao)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ",SITUACAO,DESCRICAO"
        strSQL = strSQL + " from SOLICITACOES_SITUACOES "
        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)
        Else
            strSQL = strSQL + "order by SITUACAO"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSituacao(reader.Item("CODIGO").ToString, reader.Item("SITUACAO").ToString, reader.Item("DESCRICAO").ToString)
                listSituacao.Add(_registro)
            End While
        End Using

        Return listSituacao
    End Function

    Public Function GetSituacao(ByVal psituacao As String) As AppSituacao
        Dim _registro As AppSituacao = Nothing
        Try
            Dim strSQL As String = "select CODIGO"
            strSQL = strSQL + ",SITUACAO,DESCRICAO"
            strSQL = strSQL + " from SOLICITACOES_SITUACOES "
            strSQL = strSQL + "where SITUACAO = '" + psituacao.ToUpper + "'"

            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    _registro = New AppSituacao(reader.Item("CODIGO").ToString, reader.Item("SITUACAO").ToString, reader.Item("DESCRICAO").ToString)
                End While
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return _registro
    End Function

    Public Function GetComboSituacao() As List(Of AppSituacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSituacao As New List(Of AppSituacao)

        Dim strSQL As String = "select 0 as CODIGO, '...' as DESCRICAO, '.' as SITUACAO from dual union "
        strSQL = strSQL + "select CODIGO, DESCRICAO, SITUACAO"
        strSQL = strSQL + " from SOLICITACOES_SITUACOES "
        strSQL = strSQL + "order by SITUACAO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSituacao(reader.Item("CODIGO").ToString, reader.Item("SITUACAO").ToString, reader.Item("DESCRICAO").ToString)
                listSituacao.Add(_registro)
            End While
        End Using

        Return listSituacao
    End Function

    'Public Function GravaLog(ByVal pTipo As String, ByVal pAutor As String, ByVal pfatura As Fatura, ByVal pCodigo As String) As Boolean
    '    Dim connection As New OleDbConnection(strConn)
    '    Try
    '        Dim strSQL As String = "insert into faturas_controle_log select '" & pTipo & "', fatura,codigo_operadora, codigo_tipo,intervalo_mes,data_inicio,debito_automatico,dia_vencimento,codigo_estado,data_fim,febraban,codigo_fatura_controle where codigo_fatura_controle='" & pCodigo & "'"

    '        Dim cmd As OleDbCommand = connection.CreateCommand
    '        cmd.CommandText = strSQL
    '        connection.Open()
    '        cmd.ExecuteNonQuery()
    '        connection.Close()
    '        cmd.Dispose()
    '        Return True
    '    Catch ex As Exception
    '        connection.Close()
    '        Return False
    '    End Try
    'End Function

End Class
