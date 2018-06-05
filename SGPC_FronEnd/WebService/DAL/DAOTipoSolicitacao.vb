Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAOTipoSolicitacao
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

    Public Function InsereTipoSolicitacao(ByVal ptiposolicitacao As AppTipoSolicitacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into SOLICITACOES_TIPOS(CODIGO"
            strSQL = strSQL + ",SOLICITACAO) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from SOLICITACOES_TIPOS)"
            strSQL = strSQL + ",'" + ptiposolicitacao.Solicitacao.ToUpper + "'"
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

    Public Function AtualizaTipoSolicitacao(ByVal ptiposolicitacao As AppTipoSolicitacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update SOLICITACOES_TIPOS set "
            strSQL = strSQL + "SOLICITACAO='" + ptiposolicitacao.Solicitacao.ToUpper + "'"
            strSQL = strSQL + " where CODIGO = '" + ptiposolicitacao.Codigo.ToString + "'"

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

    Public Function ExcluiTipoSolicitacao(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete SOLICITACOES_TIPOS "
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

    Public Function GetTipoSolicitacaoById(ByVal pcodigo As Integer) As List(Of AppTipoSolicitacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listTipoSolicitacao As New List(Of AppTipoSolicitacao)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ", SOLICITACAO "
        strSQL = strSQL + "from SOLICITACOES_TIPOS "
        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)
        Else
            strSQL = strSQL + "order by SOLICITACAO"
        End If

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

    Public Function GetTipoSolicitacao(ByVal psolicitacao As String) As AppTipoSolicitacao
        Dim _registro As AppTipoSolicitacao = Nothing
        Try
            Dim strSQL As String = "select CODIGO"
            strSQL = strSQL + ", SOLICITACAO "
            strSQL = strSQL + "from SOLICITACOES_TIPOS "
            strSQL = strSQL + "where SOLICITACAO like '%" + psolicitacao.ToUpper + "'"

            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    _registro = New AppTipoSolicitacao(reader.Item("CODIGO").ToString, reader.Item("SOLICITACAO").ToString)
                End While
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return _registro
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
