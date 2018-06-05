Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOServicos
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

    Public Function InsereServico(ByVal pservico As AppServicos) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into SOLICITACOES_SERVICOS(CODIGO"
            strSQL = strSQL + ",SERVICO,TIPO_SOLICITACAO_CODIGO) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from SOLICITACOES_SERVICOS)"
            strSQL = strSQL + ",'" + pservico.Servico.ToUpper + "'"
            strSQL = strSQL + ",'" + pservico.TipoSolicitacao.ToString + "'"
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

    Public Function AtualizaServico(ByVal pservico As AppServicos) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update SOLICITACOES_SERVICOS set "
            strSQL = strSQL + "SERVICO='" + pservico.Servico.ToUpper + "'"
            strSQL = strSQL + ",TIPO_SOLICITACAO_CODIGO='" + pservico.TipoSolicitacao.ToString + "'"
            strSQL = strSQL + " where CODIGO = '" + pservico.Codigo.ToString + "'"

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

    Public Function ExcluiServico(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete SOLICITACOES_SERVICOS "
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

    Public Function GetServicoById(ByVal pcodigo As Integer) As List(Of AppServicos)
        Dim connection As New OleDbConnection(strConn)
        Dim listServico As New List(Of AppServicos)

        Dim strSQL As String = "select s.CODIGO"
        strSQL = strSQL + ", s.SERVICO, s.TIPO_SOLICITACAO_CODIGO, ts.SOLICITACAO "
        strSQL = strSQL + "from SOLICITACOES_SERVICOS s "
        strSQL = strSQL + "inner join SOLICITACOES_TIPOS ts on s.TIPO_SOLICITACAO_CODIGO = ts.CODIGO "
        If pcodigo > 0 Then
            strSQL = strSQL + "where s.CODIGO = " + Convert.ToString(pcodigo)
        Else
            strSQL = strSQL + "order by s.SERVICO"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppServicos(reader.Item("CODIGO").ToString, reader.Item("SERVICO").ToString, reader.Item("TIPO_SOLICITACAO_CODIGO").ToString)
                listServico.Add(_registro)
            End While
        End Using

        Return listServico
    End Function

    Public Function GetServico(ByVal pservico As String) As AppServicos
        Dim _registro As AppServicos = Nothing
        Try
            Dim strSQL As String = "select s.CODIGO"
            strSQL = strSQL + ", s.SERVICO, s.TIPO_SOLICITACAO_CODIGO, ts.SOLICITACAO "
            strSQL = strSQL + "from SOLICITACOES_SERVICOS s "
            strSQL = strSQL + "inner join SOLICITACOES_TIPOS ts on s.TIPO_SOLICITACAO_CODIGO = ts.CODIGO "
            strSQL = strSQL + "where s.SERVICO like '%" + pservico.ToUpper + "'"

            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    _registro = New AppServicos(reader.Item("CODIGO").ToString, reader.Item("SERVICO").ToString, reader.Item("TIPO_SOLICITACAO_CODIGO").ToString)
                End While
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return _registro
    End Function

    Public Function GetComboServico() As List(Of AppServicos)
        Dim connection As New OleDbConnection(strConn)
        Dim listServico As New List(Of AppServicos)

        Dim strSQL As String = "select 0 as CODIGO, '...' as SERVICO from dual union "
        strSQL = strSQL + "select CODIGO, SERVICO "
        strSQL = strSQL + "from SOLICITACOES_SERVICOS "
        strSQL = strSQL + "order by SERVICO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppServicos(reader.Item("CODIGO").ToString, reader.Item("SERVICO").ToString)
                listServico.Add(_registro)
            End While
        End Using

        Return listServico
    End Function

    Public Function GetComboTipoSolicitacao() As List(Of AppTipoSolicitacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listTipoSolicitacao As New List(Of AppTipoSolicitacao)

        Dim strSQL As String = "select 0 as CODIGO, '...' as SOLICITACAO from dual union "
        strSQL = strSQL + "select CODIGO, SOLICITACAO "
        strSQL = strSQL + "from SOLICITACOES_TIPOS "
        strSQL = strSQL + "order by SOLICITACAO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTipoSolicitacao(reader.Item("CODIGO").ToString, reader.Item("SOLICITACAO").ToString)
                listTipoSolicitacao.Add(_registro)
            End While
        End Using

        Return listTipoSolicitacao
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
