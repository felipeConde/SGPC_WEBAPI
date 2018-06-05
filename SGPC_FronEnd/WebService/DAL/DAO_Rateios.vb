Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Rateios

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

    Public Function InsereRateio(ByVal Rateio As AppRateio, ByVal usuario As String, _servicos As String(), _linhas As String(), Optional codigo_franquia As String = "", Optional sobra As String = "") As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            strSQL = "insert into Rateio_faturas"
            strSQL = strSQL + " (CODIGO, descricao, valor, individual, data_criacao, codigo_fatura, linha_tipo, num_linha, rateio_tipo,sobra) "
            strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from Rateio_Faturas)"
            strSQL = strSQL + " ,'" + Rateio.Descricao + "'"
            strSQL = strSQL + " ,to_number('" + Replace(Rateio.Valor.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " ,'" + Rateio.Individual + "'"
            strSQL = strSQL + " ,to_date('" + Rateio.data_criacao + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,'" + Rateio.Codigo_fatura + "'"
            strSQL = strSQL + " ,'" + Rateio.Linha_tipo + "'"
            strSQL = strSQL + " ,'" + Rateio.Num_Linha + "'"
            strSQL = strSQL + " ,'" + Rateio.Rateio_tipo + "'"
            If sobra <> "" Then
                strSQL = strSQL + " ,'" + sobra + "'"
            Else
                strSQL = strSQL + " ,''"
            End If

            strSQL = strSQL + " )"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            'inserimos o serviços
            If _servicos.Length > 0 Then
                Dim i As Integer = 0
                For i = 0 To _servicos.Length - 1
                    If _servicos(i).ToString <> "" Then
                        strSQL = "insert into RATEIO_FATURAS_SERVICOS"
                        strSQL = strSQL + " (codigo_rateio, servico) "
                        strSQL = strSQL + " values((select nvl(max(CODIGO), 0) from Rateio_Faturas)"
                        strSQL = strSQL + " ,'" + _servicos(i).ToString + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If

            'inserimos as linhas que fazem parte do rateio
            If _linhas.Length > 0 Then
                Dim i As Integer = 0
                For i = 0 To _linhas.Length - 1
                    If _linhas(i).ToString <> "" Then
                        strSQL = "insert into RATEIO_FATURAS_LINHAS"
                        strSQL = strSQL + " (codigo_rateio, linha) "
                        strSQL = strSQL + " values((select nvl(max(CODIGO), 0) from Rateio_Faturas)"
                        strSQL = strSQL + " ,'" + _linhas(i).ToString + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If

            strSQL = "insert into rateios_log"
            strSQL = strSQL + " (CODIGO, codigo_rateio, descricao, valor, individual, data_criacao, codigo_fatura, linha_tipo, num_linha, rateio_tipo, tipo_log, usuario, data_log) "
            strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from Rateios_log)"
            strSQL = strSQL + " ,(select nvl(max(CODIGO), 0) from Rateio_Faturas)"
            strSQL = strSQL + " ,'" + Rateio.Descricao + "'"
            strSQL = strSQL + " ,to_number('" + Replace(Rateio.Valor.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " ,'" + Rateio.Individual + "'"
            strSQL = strSQL + " ,to_date('" + Rateio.data_criacao + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,'" + Rateio.Codigo_fatura + "'"
            strSQL = strSQL + " ,'" + Rateio.Linha_tipo + "'"
            strSQL = strSQL + " ,'" + Rateio.Num_Linha + "'"
            strSQL = strSQL + " ,'" + Rateio.Rateio_tipo + "'"
            strSQL = strSQL + " ,'N'"
            strSQL = strSQL + " ,'" + usuario + "'"
            strSQL = strSQL + " ,to_date('" + Rateio.data_criacao + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " )"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            'se for rateio de franquia faz update em franquias_rateios

            If codigo_franquia <> "" Then
                strSQL = "update FRANQUIAS_RATEIOS "
                strSQL = strSQL + " set codigo_rateio=(select nvl(max(CODIGO), 0) from Rateio_Faturas)"
                strSQL = strSQL + " where codigo_fatura='" & Rateio.Codigo_fatura.ToString & "'"
                strSQL = strSQL + " and codigo_franquia= '" & codigo_franquia & "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If


            ''INSERE NO AGENDAMENTO
            'strSQL = "insert into gestao_agendamentos_tarefas (codigo,data,descricao,autor,status,cod_tarefa) values ((select nvl(max(codigo),0)+1 from gestao_agendamentos_tarefas),sysdate,'ATUALIZAÇÃO RATEIO','" & usuario & "','0','4') "
            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

            'strSQL = "insert into gestao_tarefas_faturas (codigo_tarefa,codigo_fatura,ativo) values ((select nvl(max(codigo),0) from gestao_agendamentos_tarefas),'" & Rateio.Codigo_fatura & "','S') "
            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Return True

    End Function


    Public Function InsereAgendamentos(ByVal Codigo_fatura As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction



            'INSERE NO AGENDAMENTO
            strSQL = "insert into gestao_agendamentos_tarefas (codigo,data,descricao,autor,status,cod_tarefa) values ((select nvl(max(codigo),0)+1 from gestao_agendamentos_tarefas),sysdate,'ATUALIZAÇÃO RATEIO','" & usuario & "','0','4') "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into gestao_tarefas_faturas (codigo_tarefa,codigo_fatura,ativo) values ((select nvl(max(codigo),0) from gestao_agendamentos_tarefas),'" & Codigo_fatura & "','S') "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Return True

    End Function

    'Public Function AtualizaRateio(ByVal Rateio As AppRateio, ByVal usuario As String) As Boolean
    '    Dim connection As New OleDbConnection(strConn)

    '    Try
    '        Dim strSQL As String = "update Rateio_faturas set "
    '        strSQL = strSQL + " valor=to_number('" + Replace(Rateio.Valor, ".", ",") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
    '        strSQL = strSQL + " descricao='" + Rateio.Descricao + "',"
    '        strSQL = strSQL + " individual='" + Rateio.Individual + "',"
    '        strSQL = strSQL + " data_criacao= to_date('" + Rateio.data_criacao + "','dd/mm/yyyy hh24:mi:ss'),"
    '        strSQL = strSQL + " codigo_fatura='" + Rateio.Codigo_fatura + "',"
    '        strSQL = strSQL + " linha_tipo='" + Rateio.Linha_tipo + "', "
    '        strSQL = strSQL + " num_linha='" + Rateio.Num_Linha + "'"
    '        strSQL = strSQL + " where CODIGO = '" + Rateio.Codigo + "' "

    '        Dim cmd As OleDbCommand = connection.CreateCommand
    '        cmd.CommandText = strSQL
    '        connection.Open()
    '        cmd.ExecuteNonQuery()
    '        connection.Close()
    '        cmd.Dispose()

    '    Catch ex As Exception
    '        connection.Close()
    '        Return False
    '    End Try

    '    Return True

    'End Function

    Public Function ExcluiRateio(ByVal pcodigo As String, ByVal usuario As String, ByVal data_log As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = "insert into rateios_log"
            strSQL = strSQL + " (CODIGO, codigo_rateio, descricao, valor, individual, data_criacao, codigo_fatura, linha_tipo, num_linha, rateio_tipo, tipo_log, usuario, data_log) "
            strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from Rateios_log)"
            strSQL = strSQL + " ,'" + pcodigo + "'"
            strSQL = strSQL + " ,(select descricao from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select valor from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select individual from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select data_criacao from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select codigo_fatura from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select linha_tipo from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select num_linha from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select rateio_tipo from Rateio_faturas where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,'D'"
            strSQL = strSQL + " ,'" + usuario + "'"
            strSQL = strSQL + " ,to_date('" + data_log + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " )"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete rateio_faturas "
            strSQL = strSQL + "where Codigo = " + pcodigo

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

    End Function

    Public Function ReturnTotalNumeroA(ByVal pcodigo As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim total As String = ""

        Try
            Dim strSQL As String = " select count(*) as total from("
            strSQL = strSQL + " select distinct rml_numero_a "
            strSQL = strSQL + " from(cdrs_celular_analitico_mv) "
            strSQL = strSQL + " where codigo_conta in "
            strSQL = strSQL + " (select codigo_conta from faturas_arquivos where codigo_fatura = '" + pcodigo + "')) "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    total = reader.Item("total").ToString
                End While
            End Using

            Return total

        Catch ex As Exception
            Return total
        End Try
    End Function

    Public Function VerificaRateioIgual(ByVal rateio As AppRateio) As String
        Dim connection As New OleDbConnection(strConn)
        Dim total As String = ""

        Try
            Dim strSQL As String = " select data_criacao from rateio_faturas"
            strSQL = strSQL + " where valor=to_number('" + Replace(rateio.Valor.Replace(".", ""), ".", ",").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " and codigo_fatura='" + rateio.Codigo_fatura + "'"
            strSQL = strSQL + " and num_linha='" + rateio.Num_Linha + "'"
            strSQL = strSQL + " and descricao='" + rateio.Descricao + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Return reader.Item("data_criacao").ToString
                End While
            End Using

        Catch ex As Exception
            Return ""
        End Try
        Return ""

    End Function

    Public Function GetRateioById(ByVal pcodigo As Integer, ByRef codigo_cliente As String) As List(Of AppRateio)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppRateio)

        Dim strSQL As String = "select r.CODIGO"
        strSQL = strSQL + ", nvl(r.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(r.linha_tipo, 0) AS linha_tipo"
        strSQL = strSQL + ", nvl(r.valor, 0) AS valor"
        strSQL = strSQL + ", nvl(r.individual, 'S') AS individual"
        strSQL = strSQL + ", nvl(r.data_criacao, '') AS data_criacao"
        strSQL = strSQL + ", nvl(r.codigo_fatura, '') AS codigo_fatura"
        strSQL = strSQL + ", nvl(f.codigo_cliente, '') AS codigo_cliente"
        strSQL = strSQL + ", nvl(r.num_linha, '') AS num_linha"
        strSQL = strSQL + " FROM RATEIO_FATURAS r, FATURAS f "
        strSQL = strSQL + " WHERE r.CODIGO ='" + pcodigo.ToString() + "' "
        strSQL = strSQL + " AND r.codigo_fatura = f.codigo_fatura(+)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRateio

                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Descricao = reader.Item("descricao").ToString
                '_registro.Codigo_operadora = reader.Item("codigo_operadora").ToString
                '_registro.Codigo_tipo = reader.Item("codigo_tipo").ToString
                _registro.Linha_tipo = reader.Item("linha_tipo").ToString
                '_registro.Competencia = reader.Item("competencia").ToString
                _registro.Valor = reader.Item("valor").ToString
                _registro.Individual = reader.Item("individual").ToString
                _registro.data_criacao = reader.Item("data_criacao").ToString
                _registro.Codigo_fatura = reader.Item("codigo_fatura").ToString
                codigo_cliente = reader.Item("codigo_cliente").ToString
                _registro.Num_Linha = reader.Item("num_linha").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Sub GetValue_Description(ByVal pcodigo As String, ByRef value As String, ByRef description As String)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppRateio)

        Dim strSQL As String = "select descricao, valor from ("
        strSQL = strSQL + " select cd.rml_numero_a as Linha, cd.tipo_serv as descricao, sum(cd.valor_cdr) as valor"
        strSQL = strSQL + " from faturas f, faturas_arquivos fa, cdrs_celular cd"
        strSQL = strSQL + " where f.codigo_fatura = fa.codigo_fatura"
        strSQL = strSQL + " and fa.codigo_conta = cd.codigo_conta"
        strSQL = strSQL + " and cd.cdr_codigo <> '3'"
        strSQL = strSQL + " and f.codigo_fatura = '" + pcodigo + "'"
        strSQL = strSQL + " group by cd.rml_numero_a,cd.tipo_serv)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                value = reader.Item("valor").ToString
                description = reader.Item("descricao").ToString
            End While
        End Using

    End Sub

    Public Function ReturnServicosByFatura(codigo_fatura As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = "Select distinct p1.tipo_serv2 "
        strSQL = strSQL + " from cdrs_celular_analitico_mv p1, faturas_arquivos p2 "
        strSQL = strSQL + " where p1.codigo_conta=p2.codigo_conta and p2.codigo_fatura='" + codigo_fatura + "' "
        strSQL = strSQL + " order by p1.tipo_serv2 "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Dim dt As New DataTable
        dt.Load(reader)

        Return dt
    End Function

End Class

