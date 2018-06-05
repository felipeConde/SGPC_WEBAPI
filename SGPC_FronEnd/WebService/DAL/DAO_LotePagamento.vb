Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Collections.Generic

Public Class DAO_LotePagamento


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


    Public Function GerarLote() As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select lpad(nvl(max(num_lote),0)+1,10,0)NUM_LOTE from LOTE_PAGAMENTO t "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("NUM_LOTE").ToString
            End While
        End Using

            Return _result

    End Function

    Public Function RetornaMenorVencimento(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select min(f.dt_vencimento) as vencimento from lote_pagamento_faturas lp, faturas f "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "'  and f.codigo_fatura = lp.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("vencimento").ToString
            End While
        End Using

        Return _result

    End Function


    Public Function RetornaSomaValores(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select nvl(sum(f.valor),0) as valor from lote_pagamento_faturas lp, faturas f "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "'  and f.codigo_fatura = lp.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaDataAbertura(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select min(c.data_abertura) as data_abertura from lote_pagamento_faturas lp, faturas f, contestacao c "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("data_abertura").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaValorContestado(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select nvl(sum(c.valor_contestado),0) as valor_contestado from lote_pagamento_faturas lp, faturas f, contestacao c "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor_contestado").ToString
            End While
        End Using

        Return _result

    End Function


    Public Function RetornaDataRetorno(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select min(c.data_conclusao) as data_conclusao from lote_pagamento_faturas lp, faturas f, contestacao c "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("data_conclusao").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaContestStatus(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select min(ct.descricao) as descricao from lote_pagamento_faturas lp, faturas f, contestacao c, contestacao_status ct"
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "'  and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura and ct.codigo = c.status"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("descricao").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaValorProcedente(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select nvl(sum(c.valor_devolvido ),0) as valor_devolvido from lote_pagamento_faturas lp, faturas f, contestacao c "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor_devolvido").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaValorImprocedente(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select nvl(sum(cl.valor_faturado),0) as valor_faturado  from lote_pagamento_faturas lp, faturas f, contestacao c, contestacao_linhas cl "
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura and cl.codigo_contestacao = c.id"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor_faturado").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaValorTotalaPagar(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select nvl(sum(f.valor)-sum(c.valor_devolvido),0) as valor_pagar  from lote_pagamento_faturas lp, faturas f, contestacao c"
        strSQL = strSQL + " where lp.num_lote='" & lote_code & "' and f.codigo_fatura = lp.codigo_fatura and c.codigo_fatura = f.codigo_fatura"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor_pagar").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function RetornaServicos(ByVal lote_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ","

        Dim strSQL As String = " "
        strSQL = strSQL + " select distinct t.tipo_serv2 from CONTESTACAO_LINHAS t "
        strSQL = strSQL + " where t.codigo_contestacao in (select c.id from contestacao c where c.codigo_fatura in "
        strSQL = strSQL + " (select lp.codigo_fatura from lote_pagamento_faturas lp where lp.num_lote='" & lote_code & "' )) "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = _result & ", " & reader.Item("tipo_serv2").ToString
            End While
        End Using

        _result = _result & "."
        Return _result.Replace(",, ", "")

    End Function

    Public Function InsereLote(ByVal lote As AppLote, ByVal faturas As String, ByVal vencimento_original As String, ByVal data As String, ByVal autor As String, ByVal laudo As AppLoteLaudo, valor_cobrado As Double, valor_apagar As Double, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim transaction As OleDbTransaction = Nothing

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.Transaction = transaction


            Dim strSQL As String = "insert into LOTE_PAGAMENTO (NUM_LOTE,DATA,AUTOR, MES_ANO,COD_OPERADORA,VALOR_COBRADO,VALOR_PAGAR)"
            strSQL = strSQL & " values ("
            strSQL = strSQL & " '" + lote.Num_Lote.ToString + "'"
            strSQL = strSQL & " ,to_date('" & data.ToString & "','DD/MM/YYYY HH24:MI:SS')"
            strSQL = strSQL & " ,'" + autor.ToString + "'"
            strSQL = strSQL & " ,'" + lote.MesANo.ToString + "'"
            strSQL = strSQL & " ,'" + lote.CodOperadora.ToString + "'"
            strSQL = strSQL + " ,to_number('" + Replace(valor_cobrado.ToString.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " ,to_number('" + Replace(valor_apagar.ToString.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL & " )"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            Dim faturas_str() As String = faturas.Split(",")
            Dim i As Integer = 0

            For i = 0 To faturas_str.Length - 1
                strSQL = "insert into LOTE_PAGAMENTO_FATURAS (NUM_LOTE,CODIGO_FATURA,VENCIMENTO_ORIGINAL,DATA,AUTOR)"
                strSQL = strSQL & " values ("
                strSQL = strSQL & " '" + lote.Num_Lote.ToString + "'"
                strSQL = strSQL & " ,'" + faturas_str(i) + "'"
                strSQL = strSQL & " ,'" + vencimento_original.ToString + "'"
                strSQL = strSQL & " ,to_date('" & data.ToString & "','DD/MM/YYYY HH24:MI:SS')"
                strSQL = strSQL & " ,'" + autor.ToString + "'"
                strSQL = strSQL & " )"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Next

            'se o vencimento não original insere na tabela de laudo

            If Not laudo Is Nothing Then

                strSQL = "insert into LOTE_PAGAMENTO_LAUDO (NUM_LOTE,PROTOLOCO,NOVO_VENCIMENTO,CODIGO_JUSTIFICATIVA,RESULTADO_DETALHADO,OBS,DATA,AUTOR)"
                strSQL = strSQL & " values ("
                strSQL = strSQL & " '" + lote.Num_Lote.ToString + "'"
                strSQL = strSQL & " ,'" + laudo.Protocolo.ToString + "'"
                strSQL = strSQL & " ,to_date('" & laudo.NOVO_VENCIMENTO.ToString & "','DD/MM/YYYY HH24:MI:SS')"
                strSQL = strSQL & " ,'" + laudo.CODIGO_JUSTIFICATIVA.ToString + "'"
                strSQL = strSQL & " ,'" + laudo.RESULTADO_DETALHADO.ToString + "'"
                strSQL = strSQL & " ,'" + laudo.OBS.ToString + "'"
                strSQL = strSQL & " ,to_date('" & data.ToString & "','DD/MM/YYYY HH24:MI:SS')"
                strSQL = strSQL & " ,'" + autor.ToString + "'"
                strSQL = strSQL & " )"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()


            End If


            'atualiza o campo LOTE na tabela faturas e status para pgto encaminhado
            For i = 0 To faturas_str.Length - 1
                strSQL = "UPDATE FATURAS SET LOTE='" + lote.Num_Lote.ToString + "' "
                strSQL += " WHERE CODIGO_FATURA='" + faturas_str(i).ToString + "'"
                strSQL += " AND LOTE IS NULL"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Next




            For i = 0 To faturas_str.Length - 1
                strSQL = "UPDATE FATURAS set codigo_status='4' "
                'se tiver novo vencimento
                If Not laudo Is Nothing Then
                    If laudo.NOVO_VENCIMENTO <> "" And IsDate(laudo.NOVO_VENCIMENTO) Then
                        strSQL += ",dt_novo_vencimento=to_date('" & laudo.NOVO_VENCIMENTO.ToString & "','DD/MM/YYYY HH24:MI:SS')"
                    End If
                End If


                strSQL += " WHERE CODIGO_FATURA='" + faturas_str(i).ToString + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Next



            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            cmd.Dispose()

            Return InsertLotePagamentoLog(lote, "N", log_string)

            Return True

        Catch ex As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            connection.Close()
            Return False
        End Try

    End Function


    Public Function RetornaFaturasLote(ByVal faturas As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ","

        Dim strSQL As String = " "
        strSQL = strSQL + " select NUM_LOTE,p1.descricao  from LOTE_PAGAMENTO_FATURAS t, faturas p1 "
        strSQL = strSQL + " where t.codigo_fatura=p1.codigo_fatura "
        strSQL = strSQL + " and p1.codigo_fatura in (" & faturas & ")"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = _result & vbNewLine & "Lote: " & reader.Item("NUM_LOTE").ToString & "Fatura: " & reader.Item("descricao").ToString
            End While
        End Using

        _result = _result & "."
        Return _result.Replace(",, ", "")

    End Function

    Public Function RetornaLoteBYID(ByVal lote As String) As AppLote
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New AppLote

        Dim strSQL As String = " "
        strSQL = strSQL + " select t.num_lote, t.data,t.mes_ano,nvl(t.cod_operadora,0)cod_operadora,nvl(t.valor_cobrado,0)valor_cobrado,nvl(t.valor_pagar,0)valor_pagar  "
        strSQL = strSQL + " from LOTE_PAGAMENTO t "
        strSQL = strSQL + " where t.num_lote='" & lote & "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result.Num_Lote = reader.Item("NUM_LOTE").ToString
                _result.MesANo = reader.Item("mes_ano").ToString
                _result.CodOperadora = reader.Item("cod_operadora").ToString
                _result.Valor_Cobrado = reader.Item("valor_cobrado").ToString
                _result.Valor_Apagar = reader.Item("valor_pagar").ToString

            End While
        End Using
        Return _result
    End Function

    Public Function ExcluiLote(ByVal _registro As AppLote, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)


        If InsertLotePagamentoLog(_registro, "D", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "delete LOTE_PAGAMENTO "
            strSQL = strSQL + "where num_lote = '" + Convert.ToString(_registro.Num_Lote) + "'"

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


    Public Function InsertLotePagamentoLog(ByVal _registro As AppLote, ByVal insert As Char, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into LOTE_PAGAMENTO_LOG(CODIGO_LOG, TIPO_LOG"
            strSQL = strSQL + ", USUARIO "
            strSQL = strSQL + ", DATA_LOG "

            strSQL = strSQL + ", num_lote "
            strSQL = strSQL + ", mes_ano "
            strSQL = strSQL + ", cod_operadora "
            strSQL = strSQL + ", valor_cobrado "
            strSQL = strSQL + ", valor_pagar "
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO_LOG),0)+1 from LOTE_PAGAMENTO_LOG)"
            'Tipo_log
            strSQL = strSQL + ",'" + insert + "'"
            strSQL = strSQL + ",'" + log_string.Item(1).ToString + "'"
            strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + _registro.Num_Lote + "'"
            strSQL = strSQL + ",'" + _registro.MesANo + "'"
            strSQL = strSQL + ",'" + _registro.CodOperadora + "'"
            strSQL = strSQL + " ,to_number('" + Replace(_registro.Valor_Cobrado.ToString.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + " ,to_number('" + Replace(_registro.Valor_Apagar.ToString.Replace(".", ""), ".", ",").Replace(" ", "").Replace("R$", "").Replace(" ", "") + "','9999999999D9999999999999999999999','NLS_NUMERIC_CHARACTERS = '',.''')"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

End Class
