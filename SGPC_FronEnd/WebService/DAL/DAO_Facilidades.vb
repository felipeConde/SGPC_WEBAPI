Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Facilidades

    Private _strConn As String = ""
    Private _dao_his As New DAO_Commons

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

    Public Function InsereFacilidade(ByVal facilidade As AppFacilidades, ByVal usuario As String, ByVal list_tipo_lig As List(Of String), ByVal list_faturas As List(Of String), ByVal list_servicos As List(Of String), ByVal list_linhas As List(Of String)) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into vas (CODIGO_VAS,CODIGO_FUNCIONALIDADE,NOME "
            sql = sql + " ,CODIGO_OPERADORA,FRANQUIA,MINUTOS,VALOR,TIPO_MEDIDA,TARIF_CODIGO,FATOR,DT_ATIVACAO,DT_DESATIVACAO,COMPARTILHADO,MAX_MIN,TIPO_FRANQUIA,OCORRENCIAS )  "
            sql = sql + " values ( (select nvl(max(CODIGO_VAS),0)+1 from vas), "
            sql = sql + "'" + facilidade.Funcionalidade + "',"
            sql = sql + "'" + facilidade.Nome + "',"
            sql = sql + "'" + facilidade.Operadora + "',"
            sql = sql + "'" + facilidade.Franquia + "',"
            sql = sql + "'" + facilidade.Minutos + "',"
            sql = sql + "'" + Replace(facilidade.Valor.Replace(".", ""), ",", ".") + "',"
            sql = sql + "'',"
            sql = sql + "'" + facilidade.Tarifa + "',"
            sql = sql + "'" + facilidade.Fator + "',"
            sql = sql + "to_date('" + facilidade.Dt_ativ + "','dd/mm/yyyy hh24:mi:ss'),"
            sql = sql + "to_date('" + facilidade.Dt_des + "','dd/mm/yyyy hh24:mi:ss'),"
            sql = sql + "'" + facilidade.Compartilhado + "',"
            sql = sql + "'" + IIf(facilidade.MaximoMinimo < 1, "", facilidade.MaximoMinimo.ToString) + "',"
            sql = sql + "'" + IIf(facilidade.FranquiaTipo < 1, "", facilidade.FranquiaTipo.ToString) + "',"
            sql = sql + "'" + facilidade.Ocorrencias.ToString + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            For Each tipo_lig As String In list_tipo_lig
                sql = " insert into vas_tarifas(CODIGO_VAS, CODIGO_TIPO_LIGACAO) "
                sql = sql + " values((select nvl(max(CODIGO_VAS),0) from vas), " + tipo_lig + ") "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'insere os servicos
            For Each servico As String In list_servicos
                sql = " insert into VAS_SERVICOS(CODIGO_VAS, SERVICO) "
                sql = sql + " values((select nvl(max(CODIGO_VAS),0) from vas), '" + servico + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'linhas
            'insere os servicos
            _dao_his.strConn = Me.strConn
            For Each ITEM As String In list_linhas
                sql = " insert into VAS_LINHAS(CODIGO_VAS, LINHA) "
                sql = sql + " values((select nvl(max(CODIGO_VAS),0) from vas), '" + ITEM + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

                'se a linha estiver no tb inventário tb é marcada
                'sql = " select count(*) from LINHAS_VAS t where t.codigo_linha in(select l.codigo_linha from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + ITEM + "') "
                'Dim dt As DataTable = _dao_his.myDataTable(sql)

                sql = " select count(*) from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + ITEM + "' and l.codigo_fornecedor in(select f.codigo from fornecedores f where f.codigo_operadora='" & facilidade.Operadora & "') "
                Dim dt2 As DataTable = _dao_his.myDataTable(sql)
                'If dt.Rows(0).Item(0) < 1 And dt2.Rows(0).Item(0) >= 1 Then
                If dt2.Rows(0).Item(0) >= 1 Then
                    'insere
                    sql = " insert into LINHAS_VAS(CODIGO_VAS,CODIGO_OPERADORA, CODIGO_LINHA) "
                    sql = sql + " values((select nvl(max(CODIGO_VAS),0) from vas),'" & facilidade.Operadora & "' ,(select l.codigo_linha from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + ITEM + "' and rownum<2)) "
                    cmd.CommandText = sql
                    cmd.ExecuteNonQuery()

                End If


            Next


            'faturas
            For Each _item As String In list_faturas
                Dim _fatura As String = _item.Split("-")(0)
                Dim _indentConta As String = _item.Split("-")(1)
                Dim _vencimento As String = _item.Split("-")(2)

                sql = " insert into VAS_FATURAS(CODIGO_VAS, FATURA,ident_conta_unica, vencimento,valor) "
                sql = sql + " values((select nvl(max(CODIGO_VAS),0) from vas), '" + _fatura + "', '" + _indentConta.Replace("&nbsp;", "") + "', to_date('" + _vencimento + "','DD/MM/YYYY'),'" & Replace(facilidade.Valor.Replace(".", ""), ",", ".") & "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'insere o plano
            If facilidade.CodigoPlano <> "" Then
                'insere
                sql = " insert into PLANOS_VAS(codigo_plano, codigo_vas,codigo_operadora) "
                sql = sql + " values('" & facilidade.CodigoPlano & "',(select nvl(max(CODIGO_VAS),0) from vas), '" + facilidade.Operadora + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

            End If


            sql = String_log("", "N", usuario)

            cmd.CommandText = sql
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

    Public Function AtualizaFacilidade(ByVal facilidade As AppFacilidades, ByVal usuario As String, ByVal list_tipo_lig As List(Of String), ByVal list_faturas As List(Of String), ByVal list_servicos As List(Of String), ByVal list_linhas As List(Of String)) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(facilidade.Codigo, "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update vas set "
            sql = sql + " CODIGO_FUNCIONALIDADE='" + IIf(String.IsNullOrEmpty(facilidade.Funcionalidade), "", facilidade.Funcionalidade) + "',"
            sql = sql + " NOME='" + facilidade.Nome + "',"
            sql = sql + " CODIGO_OPERADORA= '" + facilidade.Operadora + "',"
            sql = sql + " FRANQUIA='" + IIf(String.IsNullOrEmpty(facilidade.Franquia), "N", facilidade.Franquia) + "',"
            sql = sql + " MINUTOS= '" + IIf(String.IsNullOrEmpty(facilidade.Minutos), "", facilidade.Minutos) + "',"
            sql = sql + " VALOR= '" + Replace(facilidade.Valor.Replace(".", ""), ",", ".") + "',"
            sql = sql + " TIPO_MEDIDA='" + facilidade.Medida + "',"
            sql = sql + " TARIF_CODIGO='" + facilidade.Tarifa + "',"
            sql = sql + " FATOR='" + IIf(String.IsNullOrEmpty(facilidade.Fator), "", facilidade.Fator) + "',"
            sql = sql + " DT_ATIVACAO= to_date('" + facilidade.Dt_ativ + "','dd/mm/yyyy hh24:mi:ss'),"
            sql = sql + " DT_DESATIVACAO= to_date('" + facilidade.Dt_des + "','dd/mm/yyyy hh24:mi:ss'),"
            sql = sql + " COMPARTILHADO='" + facilidade.Compartilhado + "',"
            sql = sql + " MAX_MIN='" + IIf(facilidade.MaximoMinimo < 1, "", facilidade.MaximoMinimo.ToString) + "',"
            sql = sql + " TIPO_FRANQUIA='" + IIf(facilidade.FranquiaTipo < 1, "", facilidade.FranquiaTipo.ToString) + "',"
            sql = sql + " OCORRENCIAS='" + facilidade.Ocorrencias + "' "
            sql = sql + " where CODIGO_VAS = '" + facilidade.Codigo + "'"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " Delete vas_tarifas where codigo_vas='" + facilidade.Codigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " Delete VAS_FATURAS where codigo_vas='" + facilidade.Codigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " Delete VAS_SERVICOS where codigo_vas='" + facilidade.Codigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " Delete VAS_LINHAS where codigo_vas='" + facilidade.Codigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            For Each tipo_lig As String In list_tipo_lig
                sql = " insert into vas_tarifas(CODIGO_VAS, CODIGO_TIPO_LIGACAO) "
                sql = sql + " values('" + facilidade.Codigo + "', " + tipo_lig + ") "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'faturas
            For Each _item As String In list_faturas
                Dim _fatura As String = _item.Split("-")(0)
                Dim _indentConta As String = _item.Split("-")(1)
                Dim _vencimento As String = _item.Split("-")(2)

                sql = " insert into VAS_FATURAS(CODIGO_VAS, FATURA,ident_conta_unica, vencimento,valor) "
                sql = sql + " values('" + facilidade.Codigo + "', '" + _fatura + "', '" + _indentConta.Replace("&nbsp;", "") + "', to_date('" + _vencimento + "','DD/MM/YYYY'),'" & Replace(facilidade.Valor.Replace(".", ""), ",", ".") & "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'SERVICOS
            For Each _item As String In list_servicos
                sql = " insert into VAS_SERVICOS(CODIGO_VAS, SERVICO) "
                sql = sql + " values('" + facilidade.Codigo + "', '" + _item + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            'LINHAS
            _dao_his.strConn = Me.strConn
            For Each _item As String In list_linhas
                sql = " insert into VAS_LINHAS(CODIGO_VAS, LINHA) "
                sql = sql + " values('" + facilidade.Codigo + "', '" + _item + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()



                'se a linha estiver no tb inventário tb é marcada
                sql = " select count(*) from LINHAS_VAS t where t.codigo_linha in(select l.codigo_linha from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + _item + "') and t.codigo_vas='" & facilidade.Codigo & "'"
                Dim dt As DataTable = _dao_his.myDataTable(sql)

                sql = " select count(*) from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + _item + "' and l.codigo_fornecedor in(select f.codigo from fornecedores f where f.codigo_operadora='" & facilidade.Operadora & "') "
                Dim dt2 As DataTable = _dao_his.myDataTable(sql)
                If dt.Rows(0).Item(0) < 1 And dt2.Rows(0).Item(0) >= 1 Then
                    'If dt2.Rows(0).Item(0) >= 1 Then
                    'insere
                    sql = " insert into LINHAS_VAS(CODIGO_VAS,CODIGO_OPERADORA, CODIGO_LINHA) "
                    sql = sql + " values('" & facilidade.Codigo & "','" & facilidade.Operadora & "' ,(select l.codigo_linha from linhas l where replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" + _item + "' and rownum<2)) "
                    cmd.CommandText = sql
                    cmd.ExecuteNonQuery()

                End If

            Next

            'insere o plano
            'delete antes
            sql = " delete from PLANOS_VAS where codigo_vas= " & facilidade.Codigo


            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            If facilidade.CodigoPlano <> "" Then
                'insere
                sql = " insert into PLANOS_VAS(codigo_plano, codigo_vas,codigo_operadora) "
                sql = sql + " values('" & facilidade.CodigoPlano & "','" & facilidade.Codigo & "', '" + facilidade.Operadora + "') "

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

            End If

            sql = String_log(facilidade.Codigo, "B", usuario)

            cmd.CommandText = sql
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

    Public Function ReturnTipoTarifa(ByVal codigo As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim tipo_tarifa As String = ""

        Dim strSQL As String = "Select nvl(tarifacao.tipo_tarifa,0)tipo_tarifa "
        strSQL = strSQL + " from VAS, tarifacao WHERE CODIGO_VAS='" + codigo + "' and VAS.tarif_codigo=tarifacao.codigo(+)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("tipo_tarifa").ToString)
                tipo_tarifa = _registro
            End While
        End Using

        Return tipo_tarifa
    End Function

    Public Function ReturnTipoLigacoes(ByVal codigo As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim cod_tipo_ligacao As New List(Of String)

        Dim strSQL As String = "Select CODIGO_TIPO_LIGACAO "
        strSQL = strSQL + " from VAS_TARIFAS WHERE CODIGO_VAS='" + codigo + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("CODIGO_TIPO_LIGACAO").ToString)
                cod_tipo_ligacao.Add(_registro)
            End While
        End Using

        Return cod_tipo_ligacao
    End Function


    Public Function ReturnFaturasByOper(ByVal codigo_oper As String, ByVal tipo As String) As List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of Fatura)

        Dim strSQL As String = " select distinct p3.descricao, p3.IDENT_CONTA_UNICA, p3.DT_VENCIMENTO vencimento "
        strSQL = strSQL + " from faturas p3 "
        strSQL = strSQL + " where p3.codigo_operadora='" + codigo_oper + "' "
        If tipo <> "" Then
            strSQL = strSQL + " and p3.codigo_tipo='" + tipo + "' "
        End If
        strSQL = strSQL + " order by p3.descricao, vencimento "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New Fatura(reader.Item("descricao").ToString, Nothing)
                _registro.IndentContaUnica = reader.Item("IDENT_CONTA_UNICA").ToString
                _registro.DTVencimento = reader.Item("vencimento").ToString
                _result.Add(_registro)
            End While
        End Using

        Return _result
    End Function

    Public Function ReturnServicos(ByVal codigo_oper As String, ByVal fatura As String, ByVal tipo As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = "Select distinct p1.tipo_serv2 "
        strSQL = strSQL + " from cdrs_celular_analitico_mv p1, faturas_arquivos p2, faturas p3 "
        strSQL = strSQL + " where p1.codigo_conta=p2.codigo_conta and p2.codigo_fatura=p3.codigo_fatura "
        strSQL = strSQL + " and p3.codigo_operadora='" + codigo_oper + "' "
        strSQL = strSQL + " and p1.cdr_codigo=4 "
        If fatura <> "" Then
            strSQL = strSQL + " and p3.descricao in (" + fatura + ") "
        End If
        If tipo <> "" Then
            strSQL = strSQL + " and p3.codigo_tipo='" + tipo + "' "
        End If
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



    Public Function ReturnFaturasByVas(ByVal codigo_vas As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = " select p3.fatura,replace(P3.IDENT_CONTA_UNICA,' ','')IDENT_CONTA_UNICA, TO_CHAR(p3.vencimento,'DD/MM/YYYY')vencimento "
        strSQL = strSQL + " from VAS_FATURAS p3 "
        strSQL = strSQL + " where p3.codigo_vas='" + codigo_vas + "' "

        strSQL = strSQL + " order by p3.fatura "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                'Dim _registro As New Fatura(reader.Item("faturas").ToString, Nothing)
                _result.Add(reader.Item("fatura").ToString & "-" & reader.Item("IDENT_CONTA_UNICA").ToString & "-" & reader.Item("vencimento").ToString)
            End While
        End Using

        Return _result
    End Function

    Public Function ReturnServicosByVas(ByVal codigo_vas As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = " select p3.servico "
        strSQL = strSQL + " from VAS_SERVICOS p3 "
        strSQL = strSQL + " where p3.codigo_vas='" + codigo_vas + "' "

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

    Public Function ReturnLinhasByVas(ByVal codigo_vas As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _result As New List(Of String)

        Dim strSQL As String = " select p3.LINHA "
        strSQL = strSQL + " from VAS_LINHAS p3 "
        strSQL = strSQL + " where p3.codigo_vas='" + codigo_vas + "' "

        strSQL = strSQL + " order by p3.LINHA "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                'Dim _registro As New Fatura(reader.Item("faturas").ToString, Nothing)
                _result.Add(reader.Item("LINHA").ToString)
            End While
        End Using

        Return _result
    End Function


    Public Function ExcluiFacilidade(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(pcodigo, "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM LINHAS_VAS WHERE CODIGO_VAS ='" + pcodigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM VAS_TARIFAS WHERE CODIGO_VAS='" + CStr(pcodigo) + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM VAS_FATURAS WHERE CODIGO_VAS='" + CStr(pcodigo) + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM VAS_LINHAS WHERE CODIGO_VAS='" + CStr(pcodigo) + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM VAS_SERVICOS WHERE CODIGO_VAS='" + CStr(pcodigo) + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM PLANOS_VAS WHERE CODIGO_VAS='" + CStr(pcodigo) + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "DELETE FROM VAS WHERE CODIGO_VAS ='" + pcodigo + "'"
            cmd.CommandText = sql
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

    Public Function GetFacilidadeByID(ByVal pcodigo As Integer) As List(Of AppFacilidades)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppFacilidades)

        Dim strSQL As String = "select CODIGO_VAS,"
        strSQL = strSQL + " nvl(CODIGO_FUNCIONALIDADE, 0)CODIGO_FUNCIONALIDADE, "
        strSQL = strSQL + " nvl(NOME, '')NOME, "
        strSQL = strSQL + " nvl(CODIGO_OPERADORA, 0)CODIGO_OPERADORA, "
        strSQL = strSQL + " nvl(FRANQUIA, 'N')FRANQUIA, "
        strSQL = strSQL + " nvl(MINUTOS, 0)MINUTOS, "
        strSQL = strSQL + " nvl(VALOR, 0)VALOR, "
        strSQL = strSQL + " nvl(TIPO_MEDIDA, '')TIPO_MEDIDA, "
        strSQL = strSQL + " nvl(TARIF_CODIGO,0)TARIF_CODIGO, "
        strSQL = strSQL + " nvl(FATOR, 0)FATOR, "
        strSQL = strSQL + " nvl(DT_ATIVACAO,'')DT_ATIVACAO, "
        strSQL = strSQL + " nvl(DT_DESATIVACAO,'')DT_DESATIVACAO, "
        strSQL = strSQL + " nvl(COMPARTILHADO, '')COMPARTILHADO, "
        strSQL = strSQL + " nvl(TIPO_FRANQUIA, 0)TIPO_FRANQUIA, "
        strSQL = strSQL + " nvl(MAX_MIN,1)MAX_MIN, "
        strSQL = strSQL + " nvl(OCORRENCIAS,'')OCORRENCIAS "
        strSQL = strSQL + " from VAS where CODIGO_VAS='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppFacilidades

                _registro.Codigo = reader.Item("CODIGO_VAS").ToString
                _registro.Nome = reader.Item("NOME").ToString
                _registro.Funcionalidade = reader.Item("CODIGO_FUNCIONALIDADE").ToString
                _registro.Operadora = reader.Item("CODIGO_OPERADORA").ToString
                _registro.Franquia = reader.Item("FRANQUIA").ToString
                _registro.Minutos = reader.Item("MINUTOS").ToString
                _registro.Valor = reader.Item("VALOR").ToString
                _registro.Tarifa = reader.Item("TARIF_CODIGO").ToString
                _registro.Fator = reader.Item("FATOR").ToString
                _registro.Dt_ativ = reader.Item("DT_ATIVACAO").ToString
                _registro.Dt_des = reader.Item("DT_DESATIVACAO").ToString
                _registro.Compartilhado = reader.Item("COMPARTILHADO").ToString
                _registro.FranquiaTipo = reader.Item("TIPO_FRANQUIA").ToString
                _registro.MaximoMinimo = reader.Item("MAX_MIN").ToString
                _registro.Ocorrencias = reader.Item("OCORRENCIAS").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into vas_log (codigo_log, usuario_log, data_log, tipo_log, CODIGO_VAS,CODIGO_FUNCIONALIDADE,NOME "
        sql = sql + " ,CODIGO_OPERADORA,FRANQUIA,MINUTOS,VALOR,TIPO_MEDIDA,TARIF_CODIGO,FATOR,DT_ATIVACAO,DT_DESATIVACAO,COMPARTILHADO, MAX_MIN,TIPO_FRANQUIA,OCORRENCIAS )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from vas_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + " '" + pcodigo + "', "
            sql = sql + " (select CODIGO_FUNCIONALIDADE from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select NOME from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select CODIGO_OPERADORA from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select FRANQUIA from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select MINUTOS from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select VALOR from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " '',"
            sql = sql + " (select TARIF_CODIGO from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select FATOR from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select DT_ATIVACAO from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select DT_DESATIVACAO from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select COMPARTILHADO from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select MAX_MIN from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select nvl(TIPO_FRANQUIA,0) from vas where CODIGO_VAS='" + pcodigo + "'),"
            sql = sql + " (select OCORRENCIAS from vas where CODIGO_VAS='" + pcodigo + "'))"
        Else
            sql = sql + " (select nvl(max(CODIGO_VAS),0) from vas),"
            sql = sql + " (select CODIGO_FUNCIONALIDADE from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select NOME from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select CODIGO_OPERADORA from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select FRANQUIA from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select MINUTOS from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select VALOR from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " '',"
            sql = sql + " (select TARIF_CODIGO from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select FATOR from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select DT_ATIVACAO from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select DT_DESATIVACAO from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select COMPARTILHADO from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select MAX_MIN from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select TIPO_FRANQUIA from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)),"
            sql = sql + " (select OCORRENCIAS from vas where CODIGO_VAS=(select nvl(max(CODIGO_VAS),0) from vas)))"

        End If
        Return sql

    End Function


    Public Function GetFraquiaTipos(ByVal codigo As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppGeneric)

        Dim strSQL As String = " select t.codigo, t.descricao from FRANQUIAS_TIPOS t "
        If Not String.IsNullOrEmpty(codigo) Then
            strSQL = strSQL + " where t.codigo'" + codigo + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString)
                _lista.Add(_registro)

            End While
        End Using

        Return _lista
    End Function
End Class

