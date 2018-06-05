Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Franquias

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

    Public Function InsereFranquia(ByVal Franquia As AppFranquia, ByVal usuario As String, _servicos As String(), _list_cobranca As List(Of AppFranquiaCobranca)) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            strSQL = "insert into FRANQUIAS"
            strSQL = strSQL + " (CODIGO, NOME, VALOR,CODIGO_OPERADORA,  TIPO_FRANQUIA, MAX_MIN, CODIGO_FATURA,CODIGO_PLANO) "
            strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from FRANQUIAS)"
            strSQL = strSQL + " ,'" + Franquia.NOME + "'"
            strSQL = strSQL + " ,to_number('" + Replace(Franquia.VALOR.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"

            If Franquia.CODIGO_FATURA > 0 Then
                strSQL = strSQL + " ,(select codigo_operadora from faturas where codigo_fatura='" + Franquia.CODIGO_FATURA.ToString + "')"
            Else
                'franquia de plano
                strSQL = strSQL + " ,'" + Franquia.CODIGO_OPERADORA.ToString + "'"
            End If

            strSQL = strSQL + " ,'" + Franquia.TIPO_FRANQUIA.ToString + "'"
            strSQL = strSQL + " ,'" + Franquia.MAX_MIN.ToString + "'"
            If Franquia.CODIGO_FATURA > 0 Then
                strSQL = strSQL + " ,'" + Franquia.CODIGO_FATURA.ToString + "'"
            Else
                'franquia de plano
                strSQL = strSQL + " ,null"

            End If

            strSQL = strSQL + " ,'" + Franquia.CodigoPlano.ToString + "'"
            strSQL = strSQL + " )"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            'inserimos o serviços
            If _servicos.Length > 0 Then
                Dim i As Integer = 0
                For i = 0 To _servicos.Length - 1
                    If _servicos(i).ToString <> "" Then
                        strSQL = "insert into FRANQUIAS_SERVICOS"
                        strSQL = strSQL + " (codigo_franquia, servico) "
                        strSQL = strSQL + " values((select nvl(max(CODIGO), 0) from FRANQUIAS)"
                        strSQL = strSQL + " ,'" + _servicos(i).ToString + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If

            'inserimos os serviços de cobranças
            If _list_cobranca.Count > 0 Then
                Dim i As Integer = 0
                For Each _cobranca As AppFranquiaCobranca In _list_cobranca
                    If _servicos(i).ToString <> "" Then
                        strSQL = "insert into FRANQUIAS_COBRANCAS"
                        strSQL = strSQL + " (codigo_franquia, servico,qtd,valor_faturado,valor_contratado,valor_correto) "
                        strSQL = strSQL + " values((select nvl(max(CODIGO), 0) from FRANQUIAS)"
                        strSQL = strSQL + " ,'" + _cobranca.servico.ToString + "'"
                        strSQL = strSQL + " ,'" + _cobranca.qtd.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_faturado.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_contratado.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_correto.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If

            strSQL = "insert into FRANQUIAS_LOG"
            strSQL = strSQL + " (CODIGO, codigo_franquia, nome, valor, codigo_operadora, tipo_franquia, max_min, codigo_fatura,CODIGO_PLANO, tipo_log, usuario, data_log) "
            strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from FRANQUIAS_LOG)"
            strSQL = strSQL + " ,(select nvl(max(CODIGO), 0) from FRANQUIAS)"
            strSQL = strSQL + " ,'" + Franquia.NOME.ToString + "'"
            strSQL = strSQL + " ,to_number('" + Replace(Franquia.VALOR.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " ,'" + Franquia.CODIGO_OPERADORA.ToString + "'"
            strSQL = strSQL + " ,'" + Franquia.TIPO_FRANQUIA.ToString + "'"
            strSQL = strSQL + " ,'" + Franquia.MAX_MIN.ToString + "'"
            strSQL = strSQL + " ,'" + Franquia.CODIGO_FATURA.ToString + "'"
            strSQL = strSQL + " ,'" + Franquia.CodigoPlano.ToString + "'"
            strSQL = strSQL + " ,'N'"
            strSQL = strSQL + " ,'" + usuario + "'"
            strSQL = strSQL + " ,sysdate"
            strSQL = strSQL + " )"

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

    Public Function UpdateFranquia(ByVal Franquia As AppFranquia, ByVal usuario As String, _servicos As String(), _list_cobranca As List(Of AppFranquiaCobranca)) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = String_log(Franquia.Codigo, "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            strSQL = "update FRANQUIAS set"
            strSQL = strSQL + " NOME='" + Franquia.NOME.ToString + "'"
            strSQL = strSQL + ",VALOR=to_number('" + Replace(Franquia.VALOR.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            'strSQL = strSQL + ",CODIGO_OPERADORA=(select codigo_operadora from faturas where codigo_fatura='" + Franquia.CODIGO_FATURA.ToString + "')"
            If Franquia.CODIGO_FATURA > 0 Then
                strSQL = strSQL + " ,CODIGO_OPERADORA=(select codigo_operadora from faturas where codigo_fatura='" + Franquia.CODIGO_FATURA.ToString + "')"
            Else
                'franquia de plano
                strSQL = strSQL + " ,CODIGO_OPERADORA='" + Franquia.CODIGO_OPERADORA.ToString + "'"
            End If
            strSQL = strSQL + ",TIPO_FRANQUIA='" + Franquia.TIPO_FRANQUIA.ToString + "'"
            strSQL = strSQL + ",MAX_MIN='" + Franquia.MAX_MIN.ToString + "'"
            If Franquia.CODIGO_FATURA > 0 Then
                strSQL = strSQL + ",CODIGO_FATURA='" + Franquia.CODIGO_FATURA.ToString + "'"
            End If

            strSQL = strSQL + ",CODIGO_PLANO='" + Franquia.CodigoPlano.ToString + "'"
            strSQL = strSQL + " where CODIGO='" & Franquia.Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            'deleta os serviços
            strSQL = "delete from FRANQUIAS_SERVICOS where codigo_franquia='" & Franquia.Codigo & "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'inserimos o serviços
            If _servicos.Length > 0 Then
                Dim i As Integer = 0
                For i = 0 To _servicos.Length - 1
                    If _servicos(i).ToString <> "" Then
                        strSQL = "insert into FRANQUIAS_SERVICOS"
                        strSQL = strSQL + " (codigo_franquia, servico) "
                        strSQL = strSQL + " values('" & Franquia.Codigo & "'"
                        strSQL = strSQL + " ,'" + _servicos(i).ToString + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If


            'deleta os serviços
            strSQL = "delete from FRANQUIAS_COBRANCAS where codigo_franquia='" & Franquia.Codigo & "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            'inserimos os serviços de cobranças
            If _list_cobranca.Count > 0 Then
                Dim i As Integer = 0
                For Each _cobranca As AppFranquiaCobranca In _list_cobranca
                    If _servicos(i).ToString <> "" Then
                        strSQL = "insert into FRANQUIAS_COBRANCAS"
                        strSQL = strSQL + " (codigo_franquia, servico,qtd,valor_faturado,valor_contratado,valor_correto) "
                        strSQL = strSQL + " values('" & Franquia.Codigo & "'"
                        strSQL = strSQL + " ,'" + _cobranca.servico.ToString + "'"
                        strSQL = strSQL + " ,'" + _cobranca.qtd.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_faturado.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_contratado.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " ,'" + _cobranca.valor_correto.ToString.Replace(".", "").Replace(",", ".").Replace(" ", "") + "'"
                        strSQL = strSQL + " )"

                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next

            End If



            strSQL = String_log(Franquia.Codigo, "B", usuario)
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


    Function String_log(pCodigo As String, Tipo As String, usuario As String) As String
        Dim strSQL As String

        strSQL = "insert into FRANQUIAS_LOG"
        strSQL = strSQL + " (CODIGO, codigo_franquia, nome, valor, codigo_operadora, tipo_franquia, max_min, codigo_fatura, tipo_log, usuario, data_log) "
        strSQL = strSQL + " values((select nvl(max(CODIGO), 0) + 1 from FRANQUIAS_LOG)"
        strSQL = strSQL + ",'" & pCodigo & "'"
        strSQL = strSQL + " ,(select nome from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,(select valor from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,(select codigo_operadora from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,(select tipo_franquia from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,(select max_min from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,(select codigo_fatura from franquias where CODIGO=" + pCodigo + ")"
        strSQL = strSQL + " ,'" & Tipo & "'"
        strSQL = strSQL + " ,'" + usuario + "'"
        strSQL = strSQL + " ,sysdate"
        strSQL = strSQL + " )"

        Return strSQL


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

    

    Public Function ExcluiFranquia(ByVal pcodigo As String, ByVal usuario As String, ByVal data_log As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = String_log(pcodigo, "D", usuario)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete franquias "
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

    

    Public Function VerificaFranquiaIgual(ByVal Franquia As AppFranquia, _servicos As String(), codigo As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim total As String = ""

        Try
            Dim strSQL As String = " select count(*) from franquias p1 "
            strSQL = strSQL + " where p1.codigo_fatura='" & Franquia.CODIGO_FATURA.ToString & "' "
            strSQL = strSQL + " and p1.tipo_franquia='" + Franquia.TIPO_FRANQUIA.ToString + "'"
            strSQL = strSQL + " and p1.max_min='" + Franquia.MAX_MIN.ToString + "'"
            If codigo <> "" Then
                strSQL = strSQL + " and p1.codigo<>'" + codigo + "'"
            End If

            If _servicos.Length > 1 Then
                Dim i As Integer = 0
                Dim _servs As String = ""
                For i = 0 To _servicos.Length - 1
                    If _servicos(i).ToString <> "" Then
                        _servs += "'" & _servicos(i) & "',"
                    End If
                Next
                _servs = _servs.Substring(0, _servs.Length - 1)

                strSQL = strSQL + " and exists (select 0 from franquias_servicos p2 where p2.codigo_franquia in(select codigo_franquia from franquias where codigo_fatura= p1.codigo_fatura) and p2.servico in (" & _servs & ")) "

            End If


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Return reader.Item(0).ToString
                End While
            End Using

        Catch ex As Exception
            Return ""
        End Try
        Return ""

    End Function

    Public Function GetFranquiaById(ByVal pcodigo As Integer) As List(Of AppFranquia)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppFranquia)

        Dim strSQL As String = "select p1.nome"
        strSQL = strSQL + ", nvl(p1.valor, 0) AS valor"
        strSQL = strSQL + ", nvl(p1.tipo_franquia, 0) AS tipo_franquia"
        strSQL = strSQL + ", nvl(p1.max_min, 1) AS max_min"
        strSQL = strSQL + ", nvl(p1.codigo_fatura, 0) AS codigo_fatura"
        strSQL = strSQL + ", nvl(p1.codigo_plano, 0) AS codigo_plano"
        strSQL = strSQL + ", nvl(p1.codigo_operadora, 0) AS codigo_operadora"
        strSQL = strSQL + " FROM franquias p1 "
        strSQL = strSQL + " where p1.codigo='" + pcodigo.ToString() + "' "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppFranquia

                _registro.Codigo = pcodigo
                _registro.NOME = reader.Item("nome").ToString
                _registro.TIPO_FRANQUIA = reader.Item("tipo_franquia").ToString
                _registro.MAX_MIN = reader.Item("max_min").ToString
                _registro.CODIGO_FATURA = reader.Item("CODIGO_FATURA").ToString
                _registro.VALOR = reader.Item("VALOR").ToString
                _registro.CodigoPlano = reader.Item("codigo_plano").ToString
                _registro.CODIGO_OPERADORA = reader.Item("CODIGO_OPERADORA").ToString

                list.Add(_registro)

            End While
        End Using

        Return list
    End Function

    Public Function ReturnServicosByFranquia(ByVal codigo_vas As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = " select p3.servico "
        strSQL = strSQL + " from FRANQUIAS_SERVICOS p3 "
        strSQL = strSQL + " where p3.codigo_franquia='" + codigo_vas + "' "

        strSQL = strSQL + " order by p3.servico "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                'Dim _registro As New Fatura(reader.Item("faturas").ToString, Nothing)
                _result.Add(reader.Item("servico").ToString)
            End While
        End Using

        Return _result
    End Function

    Public Function ReturnServicosByFatura(codigo_fatura As String, tipo_franquia As Integer, Optional tipo_registro As String = "0") As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = "Select p1.tipo_serv2, sum(p1.valor_cdr)total,count(*)qtd "
        strSQL = strSQL + " ,nvl(count(*)*(select v.Valor from vas v where v.nome=p1.tipo_serv2 and v.codigo_operadora=p3.codigo_operadora and rownum<2),0)valor_contratual "
        'valor auditado (faturado-auditado)
        strSQL = strSQL + ",  sum(p1.valor_cdr-nvl(case when p1.aprovada='S' then p1.valor_devolvido else 0 end ,0)) valor_correto "

        'strSQL += "(select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=p3.codigo_fatura and c.linha=p1.rml_numero_a and c.tipo_serv2=p1.tipo_serv2 and c.aprovada='S' "
        'strSQL += " "
        'strSQL += ")valor_correto"

        strSQL = strSQL + " from cdrs_celular p1, faturas_arquivos p2,faturas p3 "
        strSQL = strSQL + " where p1.codigo_conta=p2.codigo_conta and p2.codigo_fatura=p3.codigo_fatura and p2.codigo_fatura='" + codigo_fatura + "' "
        If tipo_franquia = 2 Then
            'franquia de minutos - só traz chamadas
            strSQL = strSQL + " and p1.cdr_codigo=3 "
        ElseIf tipo_franquia = 3 Then
            strSQL = strSQL + " and p1.cdr_codigo=4 "
        End If

        If tipo_registro <> "0" Then
            strSQL = strSQL + " and p1.cdr_codigo IN(" & tipo_registro & ")"
        End If

        strSQL = strSQL + " group by p1.tipo_serv2, p3.codigo_operadora order by p1.tipo_serv2 "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Dim dt As New DataTable
        dt.Load(reader)

        Return dt
    End Function

    Public Function ReturnServicosByOperadora(codigo_operadora As String, tipo_franquia As Integer, Optional tipo_registro As Integer = 0) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = "Select p1.tipo_serv2  "
        strSQL = strSQL + "  "
        strSQL = strSQL + " from cdrs_celular_analitico_mv p1, faturas_arquivos p2,faturas p3 "
        strSQL = strSQL + " where p1.codigo_conta=p2.codigo_conta and p2.codigo_fatura=p3.codigo_fatura and p3.codigo_operadora='" + codigo_operadora + "' "
        If tipo_franquia = 2 Then
            'franquia de minutos - só traz chamadas
            strSQL = strSQL + " and p1.cdr_codigo=3 "
        ElseIf tipo_franquia = 3 Then
            strSQL = strSQL + " and p1.cdr_codigo=4 "
        End If

        If tipo_registro > 0 Then
            strSQL = strSQL + " and p1.cdr_codigo=' " & tipo_registro & "'"
        End If

        strSQL = strSQL + " group by p1.tipo_serv2 "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Dim dt As New DataTable
        dt.Load(reader)

        Return dt
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


    Public Function ReturnCobrancasByFranquia(ByVal codigo_vas As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = " select p3.servico "
        strSQL = strSQL + " from FRANQUIAS_COBRANCAS p3 "
        strSQL = strSQL + " where p3.codigo_franquia='" + codigo_vas + "' "

        strSQL = strSQL + " order by p3.servico "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                'Dim _registro As New Fatura(reader.Item("faturas").ToString, Nothing)
                _result.Add(reader.Item("servico").ToString)
            End While
        End Using

        Return _result
    End Function

End Class

