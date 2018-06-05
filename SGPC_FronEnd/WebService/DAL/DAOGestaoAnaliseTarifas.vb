Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOGestaoAnaliseTarifas

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

    Public Function GetRelatio0800(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal ptipo_linha As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select nvl(descricao,'NÃO CLASSIFICADA')descricao, decode(tipo,'N','Normal','R','Reduzido')tipo,sum(nvl(valor_cdr,0))valor_faturado,nvl(sum(valor_audit),0)valor_audit, sum(nvl(chamadas,0))Chamadas,sum(nvl(minutos,0))minutos, tarifa,(sum(valor_cdr)-sum(valor_audit))contestar "
        strSQL = strSQL + "   from (select p02.descricao, p01.tipo,p01.chamadas,p01.minutos, p01.codigo_horario,p01.horario,p01.valor_cdr,p01.valor_audit,p02.tipo_tarifa, p02.codigo tipo_ligacao, case when p02.ttm=30 then p02.valor_ttm * 2 else p02.valor_ttm end tarifa "
        strSQL = strSQL + "           from (select (select v.tipo_tarifa "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
        End If
        strSQL = strSQL + "                            and rownum < 2) tipo, "
        strSQL = strSQL + "                        (select v.codigo_horario "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "' "
        End If

        strSQL = strSQL + "                            and rownum < 2) codigo_horario, "
        strSQL = strSQL + "                        p0.horario, "
        strSQL = strSQL + "                        valor_cdr, "
        strSQL = strSQL + "                        valor_audit, minutos, "
        strSQL = strSQL + "                        chamadas "
        strSQL = strSQL + "                   from (select billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                          'DD/MM/YYYY HH24:MI')) horario, "
        strSQL = strSQL + "                                sum(p00.valor_cdr) valor_cdr,sum(p00.valor_audit) valor_audit,sum(chamadas)chamadas,sum(minutos)minutos, "
        strSQL = strSQL + "                                p00.codigo_tipo_ligacao "
        strSQL = strSQL + "                           from ( "
        strSQL = strSQL + "                                 select to_char(sysdate, 'dd/MM/YYYY ') || "
        strSQL = strSQL + "                                         to_char(p1.data_inicio, 'HH24:MI') data_inicio, "
        strSQL = strSQL + "                                         sum(valor_cdr) valor_cdr, "
        strSQL = strSQL + "                                         sum(valor_audit) valor_audit, "
        strSQL = strSQL + "                                         count(*)chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos, "
        strSQL = strSQL + "                                         codigo_tipo_ligacao "
        strSQL = strSQL + "                                   from cdrs_celular p1 where 1=1 "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                  and p1.codigo_conta in "
        strSQL = strSQL + "                                        (select codigo_conta "
        strSQL = strSQL + "                                           from faturas_arquivos "
        strSQL = strSQL + "                                          where codigo_fatura in "
        strSQL = strSQL + "                                                (select codigo_fatura "
        strSQL = strSQL + "                                                   from faturas "
        strSQL = strSQL + "                                                  where 1=1 and codigo_tipo in('4','6')  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                    and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                    and p1.data_inicio is not null "
        strSQL = strSQL + "                                    and p1.codigo_tipo_ligacao in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                  group by to_char(p1.data_inicio, 'HH24:MI'), "
        strSQL = strSQL + "                                            codigo_tipo_ligacao) p00 "
        strSQL = strSQL + "                          group by billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                             'DD/MM/YYYY HH24:MI')), "
        strSQL = strSQL + "                                   p00.codigo_tipo_ligacao) p0 "
        strSQL = strSQL + "                 ) p01, "
        strSQL = strSQL + "                (select distinct p4.codigo, p3.codigo_horario, p2.ttm, p2.valor_ttm, p4.descricao,p1.tipo_tarifa "
        strSQL = strSQL + "                   from tarifas_teste               p2, "
        strSQL = strSQL + "                        horarios_tarifacao_teste    p1, "
        strSQL = strSQL + "                        horarios_tarifacao_op_teste p3, "
        strSQL = strSQL + "                        tipos_ligacao_teste         p4, "
        strSQL = strSQL + "                        tarifacao                   t "
        strSQL = strSQL + "                  where p1.codigo_tarifa = p2.codigo and t.tipo_tarifa in('4','6') "
        strSQL = strSQL + "                    and p3.codigo_horario = p1.codigo "
        strSQL = strSQL + "                    and p3.codigo_tipo_ligacao = p4.codigo "
        strSQL = strSQL + "                    and p4.codigo_tarif = t.codigo           "
        strSQL = strSQL + "                    and p1.horario<>604800                    "
        strSQL = strSQL + "                    ) p02 "
        strSQL = strSQL + "          where p01.codigo_horario = p02.codigo_horario and p02.tipo_tarifa=decode(p01.tipo,'S',1,'R',2,'N',4,'D',8)) "
        strSQL = strSQL + " group by descricao, tipo, tarifa "

        '//////////LOGAÇÕES QUE NÃO ESTÃO NA HORÁRIO TARIFAÇÃO////////////////

        strSQL = strSQL + "UNION select nvl(p2.descricao,'SEM CLASSIFICAÇÃO') descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in('4','6')  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste p55,tipos_ligacao_teste p33, tarifacao p44 where p55.codigo_operadora='" + Convert.ToString(pOperadora) + "' and p55.codigo_tipo_ligacao = p33.codigo and p33.codigo_tarif=p44.codigo and p44.tipo_tarifa in('4','6')) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, p2.descricao,case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "

        strSQL = strSQL + "UNION select nvl(p1.tipo_serv2,'SEM CLASSIFICAÇÃO') descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in('4','6')  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo <> 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        'strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, p1.tipo_serv2,case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "


        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(reader)

        'Dim _row As DataRow
        '_row = dt.NewRow

        '_row.Item("DESCRICAO") = "Total"
        '_row.Item("tipo") = ""
        '_row.Item("valor_faturado") = 0
        '_row.Item("valor_audit") = 0
        '_row.Item("chamadas") = 0
        '_row.Item("minutos") = 0
        '_row.Item("tarifa") = 0
        '_row.Item("contestar") = 0

        'dt.Rows.Add(_row)

        Return dt

    End Function


    Public Function GetRelatio0800_DDDLOCAL(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select p2.descricao descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar, "
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       codigo_tipo_ligacao, case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end tarifa "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If pFatura > 0 Then
            strSQL = strSQL + " and codigo_fatura in('" + Convert.ToString(pFatura) + "') "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, p2.descricao,case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "


        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(reader)

        Return dt

    End Function

    Public Function GetRelatioMovel(ByVal pOperadora As String, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal pTipo As String, ByVal ptipo_linha As String, tipoServ As Boolean, exibeAudit As Boolean) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)
        Dim dt As New DataTable

        Try
            Dim strSQL As String = ""
            strSQL = strSQL + " select p1.cdr_codigo,case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else p1.tipo_serv2 end tipo_serv2,op.descricao operadora,"
            If tipoServ Then
                strSQL = strSQL + " p1.tipo_serv,"
            Else
                strSQL = strSQL + " 1 tipo_serv,"
            End If
            strSQL = strSQL + "nvl(replace(p3.nome_configuracao,'DEFAULT'),'-') categoria,"

            strSQL = strSQL + "   nvl(round(sum(valor_cdr),2),0)valor_faturado,sum(chamadas) Chamadas,round(sum(p1.duracao)/60,2) minutos,"

            'informações da auditoria
            If exibeAudit Then
                strSQL = strSQL + "p2.descricao,round(sum(valor_audit),2)valor_audit,  case when p2.ttm=30 then p2.valor_ttm*2 else p2.valor_ttm end tarifa,round(sum(valor_cdr)-sum(valor_audit),2)contestar, nvl(p2.complemento,'-')complemento"
            Else
                strSQL = strSQL + "'-' descricao, 0 valor_audit,  0 tarifa,0 contestar, '-' complemento"

            End If

            strSQL = strSQL + " from cdrs_celular_analitico_mv p1, tipos_ligacao_teste p2, TARIFACAO p3, operadoras_teste op, faturas_arquivos fa, faturas f "
            strSQL = strSQL + " where p1.codigo_tipo_ligacao=p2.codigo(+) and p2.codigo_tarif=p3.codigo(+) and p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura and f.codigo_operadora=op.codigo(+)"
            strSQL = strSQL + " and p1.codigo_conta in "
            strSQL = strSQL + "                                        (select codigo_conta "
            strSQL = strSQL + "                                           from faturas_arquivos "
            strSQL = strSQL + "                                          where codigo_fatura in "
            strSQL = strSQL + "                                                (select codigo_fatura "
            strSQL = strSQL + "                                                   from faturas "
            strSQL = strSQL + "                                                  where 1 = 1 "

            If Not String.IsNullOrEmpty(pOperadora) Then
                If pOperadora > 0 Then
                    strSQL = strSQL + "and codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
                End If
            End If

            If Not String.IsNullOrEmpty(pFatura) Then
                strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
            End If
            If Not String.IsNullOrEmpty(pTipo) Then
                strSQL = strSQL + " and codigo_tipo in(" + Convert.ToString(pTipo) + ") "
            End If

            If Not String.IsNullOrEmpty(pVencimento) Then
                strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
            End If

            If ptipo_linha <> "0" Then
                strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
            End If

            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            strSQL = strSQL + "                                    )) "
            strSQL = strSQL + "  group by p1.cdr_codigo,p3.nome_configuracao,case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else p1.tipo_serv2 end,op.descricao "
            If tipoServ Then
                strSQL += " ,p1.tipo_serv"
            End If
            If exibeAudit Then
                strSQL += " ,p2.descricao, case when p2.ttm=30 then p2.valor_ttm*2 else p2.valor_ttm end,  nvl(p2.complemento,'-')"
            End If


            strSQL = strSQL + "  order by  p1.cdr_codigo "


            'System.Web.HttpContext.Current.Response.Write(strSQL)
            'System.Web.HttpContext.Current.Response.End()


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            dt.Load(reader)

            Return dt
        Catch ex As Exception

            System.Web.HttpContext.Current.Response.Write(ex.Message)
            System.Web.HttpContext.Current.Response.End()

        End Try
        Return dt

    End Function


    Public Function GetRelatioMovelDetalhes(ByVal pOperadora As String, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal pServ As String, ByVal pTipo As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)
        Dim dt As New DataTable

        Try
            Dim strSQL As String = ""
            strSQL = strSQL + " select  p1.rml_numero_a linha,nvl(p1.numero_b,'SEM NUMERO')numero_b, case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else p1.tipo_serv2 end tipo_serv2,p2.descricao, round(valor_cdr, 2) valor_faturado,round(valor_audit, 2) valor_auditado, p1.data_inicio,p1.data_fim,round((p1.data_fim-p1.data_inicio)*1440,2) minutagem,op.descricao operadora "
            strSQL = strSQL + " from cdrs_celular p1, tipos_ligacao_teste p2,operadoras_teste op, faturas_arquivos fa, faturas f "
            strSQL = strSQL + " where p1.codigo_tipo_ligacao=p2.codigo(+)  and p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura and f.codigo_operadora=op.codigo(+) "
            strSQL = strSQL + " and p1.codigo_conta in "
            strSQL = strSQL + "                                        (select codigo_conta "
            strSQL = strSQL + "                                           from faturas_arquivos "
            strSQL = strSQL + "                                          where codigo_fatura in "
            strSQL = strSQL + "                                                (select codigo_fatura "
            strSQL = strSQL + "                                                   from faturas "
            strSQL = strSQL + "                                                  where 1 = 1 "

            If Not String.IsNullOrEmpty(pOperadora) Then
                If pOperadora > 0 Then
                    strSQL = strSQL + "and codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
                End If
            End If

            If Not String.IsNullOrEmpty(pFatura) Then
                strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
            End If

            If Not String.IsNullOrEmpty(pTipo) Then
                strSQL = strSQL + " and codigo_tipo in(" + Convert.ToString(pTipo) + ") "
            End If

            If Not String.IsNullOrEmpty(pVencimento) Then
                strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
            End If

            If Not String.IsNullOrEmpty(pServ) Then
                strSQL = strSQL + " and trim(case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else p1.tipo_serv2 end) = '" & pServ.Trim & "'"
            End If


            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            strSQL = strSQL + "                                    )) "
            'strSQL = strSQL + "  group by p1.cdr_codigo,case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else p1.tipo_serv2 end,p2.descricao, case when p2.ttm=30 then p2.valor_ttm*2 else p2.valor_ttm end "
            strSQL = strSQL + "  order by  p1.cdr_codigo "


            'System.Web.HttpContext.Current.Response.Write(strSQL)
            'System.Web.HttpContext.Current.Response.End()


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            dt.Load(reader)

            Return dt
        Catch ex As Exception

            System.Web.HttpContext.Current.Response.Write(ex.Message)
            System.Web.HttpContext.Current.Response.End()

        End Try
        Return dt

    End Function

    Public Function GetRelatioMovel_byLinha(ByVal pOperadora As String, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal ptipo_linha As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)
        Dim dt As New DataTable

        Try
            Dim strSQL As String = ""
            strSQL = strSQL + " select p1.rml_numero_a, p1.cdr_codigo,case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else 'Descontos' end tipo_serv2, p2.descricao, p2.descricao, round(sum(valor_cdr),2)valor_faturado,round(sum(valor_audit),2)valor_audit, sum(chamadas) Chamadas,round(sum(p1.duracao)/60,2) minutos, count(*) as total, case when p2.ttm=30 then p2.valor_ttm*2 else p2.valor_ttm end tarifa,round(sum(valor_cdr)-sum(valor_audit),2)contestar "
            strSQL = strSQL + " from cdrs_celular_analitico_mv p1, tipos_ligacao_teste p2 "
            strSQL = strSQL + " where p1.codigo_tipo_ligacao=p2.codigo(+) "
            strSQL = strSQL + " and p1.codigo_conta in "
            strSQL = strSQL + " (select codigo_conta "
            strSQL = strSQL + " from faturas_arquivos "
            strSQL = strSQL + " where codigo_fatura in "
            strSQL = strSQL + " (select codigo_fatura "
            strSQL = strSQL + " from faturas "
            strSQL = strSQL + " where 1 = 1 "

            If Not String.IsNullOrEmpty(pOperadora) Then
                If pOperadora > 0 Then
                    strSQL = strSQL + "and codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
                End If
            End If

            If Not String.IsNullOrEmpty(pFatura) Then
                strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
            End If

            If Not String.IsNullOrEmpty(pVencimento) Then
                strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
            End If

            If ptipo_linha <> "0" Then
                strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
            End If

            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            strSQL = strSQL + "                                    )) "
            strSQL = strSQL + "  group by p1.cdr_codigo,case when p1.cdr_codigo <> '5' and p2.codigo=0 then nvl(p1.tipo_serv2, p1.tipo_serv) when p1.cdr_codigo<>'5' then nvl(p2.descricao,nvl(p1.tipo_serv2,p1.tipo_serv)) else 'Descontos' end,p2.descricao, case when p2.ttm=30 then p2.valor_ttm*2 else p2.valor_ttm end "
            strSQL = strSQL + "  ,p1.rml_numero_a order by  p1.cdr_codigo "


            'System.Web.HttpContext.Current.Response.Write(strSQL)
            'System.Web.HttpContext.Current.Response.End()


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            dt.Load(reader)

            Return dt
        Catch ex As Exception

            System.Web.HttpContext.Current.Response.Write(ex.Message)
            System.Web.HttpContext.Current.Response.End()

        End Try

        Return dt
    End Function

    Public Function GetRelatioFixo(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal ptipo_linha As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select cdrs.descricao, cdrs.tipo, cdrs.valor_faturado,cdrs.valor_audit, cdrs.chamadas, cdrs.minutos,cdrs.tarifa,cdrs.contestar,op.descricao operadora "
        strSQL = strSQL + " from("
        strSQL = strSQL + " select nvl(decode(descricao,'Automatico','NÃO CLASSIFICADA',descricao),'NÃO CLASSIFICADA')descricao, decode(tipo,'N','Normal','R','Reduzido')tipo,sum(nvl(valor_cdr,0))valor_faturado,nvl(sum(valor_audit),0)valor_audit, sum(nvl(chamadas,0))Chamadas,sum(nvl(minutos,0))minutos, tarifa,(sum(valor_cdr)-sum(valor_audit))contestar, codigo_conta "
        strSQL = strSQL + "   from (select p02.descricao, p01.tipo,p01.chamadas,p01.minutos, p01.codigo_horario,p01.horario,p01.valor_cdr,p01.valor_audit,p02.tipo_tarifa, p02.codigo tipo_ligacao, case when p02.ttm=30 then p02.valor_ttm * 2 else p02.valor_ttm end tarifa, p01.codigo_conta "
        strSQL = strSQL + "           from (select (select v.tipo_tarifa "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
        End If
        strSQL = strSQL + "                            and rownum < 2) tipo, "
        strSQL = strSQL + "                        (select v.codigo_horario "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "' "
        End If
        strSQL = strSQL + "                            and rownum < 2) codigo_horario, "
        strSQL = strSQL + "                        p0.horario, "
        strSQL = strSQL + "                        valor_cdr, "
        strSQL = strSQL + "                        valor_audit, minutos, "
        strSQL = strSQL + "                        chamadas, codigo_conta "
        strSQL = strSQL + "                   from (select billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                          'DD/MM/YYYY HH24:MI')) horario, "
        strSQL = strSQL + "                                sum(p00.valor_cdr) valor_cdr,sum(p00.valor_audit) valor_audit,sum(chamadas)chamadas,sum(minutos)minutos, "
        strSQL = strSQL + "                                p00.codigo_tipo_ligacao, p00.codigo_conta "
        strSQL = strSQL + "                           from ( "
        strSQL = strSQL + "                                 select to_char(sysdate, 'dd/MM/YYYY ') || "
        strSQL = strSQL + "                                         to_char(p1.data_inicio, 'HH24:MI') data_inicio, "
        strSQL = strSQL + "                                         sum(valor_cdr) valor_cdr, "
        strSQL = strSQL + "                                         sum(valor_audit) valor_audit, "
        strSQL = strSQL + "                                         count(*)chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos, "
        strSQL = strSQL + "                                         codigo_tipo_ligacao, p1.codigo_conta "
        strSQL = strSQL + "                                   from cdrs_celular p1 where 1=1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If

        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                  and p1.codigo_conta in "
        strSQL = strSQL + "                                        (select codigo_conta "
        strSQL = strSQL + "                                           from faturas_arquivos "
        strSQL = strSQL + "                                          where codigo_fatura in "
        strSQL = strSQL + "                                                (select codigo_fatura "
        strSQL = strSQL + "                                                   from faturas "
        strSQL = strSQL + "                                                  where 1=1 and codigo_tipo in(2,4,6) "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                    and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                    and p1.data_inicio is not null "
        strSQL = strSQL + "                                    and p1.codigo_tipo_ligacao in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                  group by to_char(p1.data_inicio, 'HH24:MI'), "
        strSQL = strSQL + "                                            codigo_tipo_ligacao, p1.codigo_conta) p00 "
        strSQL = strSQL + "                          group by billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                             'DD/MM/YYYY HH24:MI')), "
        strSQL = strSQL + "                                   p00.codigo_tipo_ligacao, p00.codigo_conta) p0 "
        strSQL = strSQL + "                 ) p01, "
        strSQL = strSQL + "                (select distinct p4.codigo, p3.codigo_horario, p2.ttm, p2.valor_ttm, p4.descricao,p1.tipo_tarifa "
        strSQL = strSQL + "                   from tarifas_teste               p2, "
        strSQL = strSQL + "                        horarios_tarifacao_teste    p1, "
        strSQL = strSQL + "                        horarios_tarifacao_op_teste p3, "
        strSQL = strSQL + "                        tipos_ligacao_teste         p4, "
        strSQL = strSQL + "                        tarifacao                   t "
        strSQL = strSQL + "                  where p1.codigo_tarifa = p2.codigo and t.tipo_tarifa=2 "
        strSQL = strSQL + "                    and p3.codigo_horario = p1.codigo "
        strSQL = strSQL + "                    and p3.codigo_tipo_ligacao = p4.codigo "
        strSQL = strSQL + "                    and p4.codigo_tarif = t.codigo           "
        strSQL = strSQL + "                    and p1.horario<>604800                    "
        strSQL = strSQL + "                    ) p02 "
        strSQL = strSQL + "          where p01.codigo_horario = p02.codigo_horario and p02.tipo_tarifa=decode(p01.tipo,'S',1,'R',2,'N',4,'D',8)) "
        strSQL = strSQL + " group by descricao, tipo, tarifa,codigo_conta "

        '//////////LOGAÇÕES QUE NÃO ESTÃO NA HORÁRIO TARIFAÇÃO////////////////

        strSQL = strSQL + "UNION select nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO') descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar, p1.codigo_conta "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in(2,4,6) "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste p55,tipos_ligacao_teste p33, tarifacao p44 where p55.codigo_operadora='" + Convert.ToString(pOperadora) + "' and p55.codigo_tipo_ligacao = p33.codigo and p33.codigo_tarif=p44.codigo and p44.tipo_tarifa=2) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO'),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end, p1.codigo_conta "

        strSQL = strSQL + "UNION select nvl(p1.tipo_serv2,p1.tipo_serv) descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar, codigo_conta "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in(2,4,6)  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo <> 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        'strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(p1.tipo_serv2,p1.tipo_serv),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end, p1.codigo_conta "
        strSQL = strSQL + " )cdrs, faturas_arquivos fa, faturas f, operadoras_teste op "
        strSQL = strSQL + " where cdrs.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura and f.codigo_operadora=op.codigo "




        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(reader)

        Return dt

    End Function

    Public Function GetRelatioFixoBK(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String, ByVal ptipo_linha As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select nvl(decode(descricao,'Automatico','NÃO CLASSIFICADA',descricao),'NÃO CLASSIFICADA')descricao, decode(tipo,'N','Normal','R','Reduzido')tipo,sum(nvl(valor_cdr,0))valor_faturado,nvl(sum(valor_audit),0)valor_audit, sum(nvl(chamadas,0))Chamadas,sum(nvl(minutos,0))minutos, tarifa,(sum(valor_cdr)-sum(valor_audit))contestar "
        strSQL = strSQL + "   from (select p02.descricao, p01.tipo,p01.chamadas,p01.minutos, p01.codigo_horario,p01.horario,p01.valor_cdr,p01.valor_audit,p02.tipo_tarifa, p02.codigo tipo_ligacao, case when p02.ttm=30 then p02.valor_ttm * 2 else p02.valor_ttm end tarifa "
        strSQL = strSQL + "           from (select (select v.tipo_tarifa "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
        End If
        strSQL = strSQL + "                            and rownum < 2) tipo, "
        strSQL = strSQL + "                        (select v.codigo_horario "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "' "
        End If
        strSQL = strSQL + "                            and rownum < 2) codigo_horario, "
        strSQL = strSQL + "                        p0.horario, "
        strSQL = strSQL + "                        valor_cdr, "
        strSQL = strSQL + "                        valor_audit, minutos, "
        strSQL = strSQL + "                        chamadas "
        strSQL = strSQL + "                   from (select billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                          'DD/MM/YYYY HH24:MI')) horario, "
        strSQL = strSQL + "                                sum(p00.valor_cdr) valor_cdr,sum(p00.valor_audit) valor_audit,sum(chamadas)chamadas,sum(minutos)minutos, "
        strSQL = strSQL + "                                p00.codigo_tipo_ligacao "
        strSQL = strSQL + "                           from ( "
        strSQL = strSQL + "                                 select to_char(sysdate, 'dd/MM/YYYY ') || "
        strSQL = strSQL + "                                         to_char(p1.data_inicio, 'HH24:MI') data_inicio, "
        strSQL = strSQL + "                                         sum(valor_cdr) valor_cdr, "
        strSQL = strSQL + "                                         sum(valor_audit) valor_audit, "
        strSQL = strSQL + "                                         count(*)chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos, "
        strSQL = strSQL + "                                         codigo_tipo_ligacao "
        strSQL = strSQL + "                                   from cdrs_celular p1 where 1=1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If

        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                  and p1.codigo_conta in "
        strSQL = strSQL + "                                        (select codigo_conta "
        strSQL = strSQL + "                                           from faturas_arquivos "
        strSQL = strSQL + "                                          where codigo_fatura in "
        strSQL = strSQL + "                                                (select codigo_fatura "
        strSQL = strSQL + "                                                   from faturas "
        strSQL = strSQL + "                                                  where 1=1 and codigo_tipo in(2,4,6) "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                    and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                    and p1.data_inicio is not null "
        strSQL = strSQL + "                                    and p1.codigo_tipo_ligacao in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                  group by to_char(p1.data_inicio, 'HH24:MI'), "
        strSQL = strSQL + "                                            codigo_tipo_ligacao) p00 "
        strSQL = strSQL + "                          group by billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                             'DD/MM/YYYY HH24:MI')), "
        strSQL = strSQL + "                                   p00.codigo_tipo_ligacao) p0 "
        strSQL = strSQL + "                 ) p01, "
        strSQL = strSQL + "                (select distinct p4.codigo, p3.codigo_horario, p2.ttm, p2.valor_ttm, p4.descricao,p1.tipo_tarifa "
        strSQL = strSQL + "                   from tarifas_teste               p2, "
        strSQL = strSQL + "                        horarios_tarifacao_teste    p1, "
        strSQL = strSQL + "                        horarios_tarifacao_op_teste p3, "
        strSQL = strSQL + "                        tipos_ligacao_teste         p4, "
        strSQL = strSQL + "                        tarifacao                   t "
        strSQL = strSQL + "                  where p1.codigo_tarifa = p2.codigo and t.tipo_tarifa=2 "
        strSQL = strSQL + "                    and p3.codigo_horario = p1.codigo "
        strSQL = strSQL + "                    and p3.codigo_tipo_ligacao = p4.codigo "
        strSQL = strSQL + "                    and p4.codigo_tarif = t.codigo           "
        strSQL = strSQL + "                    and p1.horario<>604800                    "
        strSQL = strSQL + "                    ) p02 "
        strSQL = strSQL + "          where p01.codigo_horario = p02.codigo_horario and p02.tipo_tarifa=decode(p01.tipo,'S',1,'R',2,'N',4,'D',8)) "
        strSQL = strSQL + " group by descricao, tipo, tarifa "

        '//////////LOGAÇÕES QUE NÃO ESTÃO NA HORÁRIO TARIFAÇÃO////////////////

        strSQL = strSQL + "UNION select nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO') descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in(2,4,6) "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste p55,tipos_ligacao_teste p33, tarifacao p44 where p55.codigo_operadora='" + Convert.ToString(pOperadora) + "' and p55.codigo_tipo_ligacao = p33.codigo and p33.codigo_tarif=p44.codigo and p44.tipo_tarifa=2) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO'),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "

        strSQL = strSQL + "UNION select nvl(p1.tipo_serv2,p1.tipo_serv) descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If

        If ptipo_linha <> "0" Then
            strSQL = strSQL + " and exists(select 0 from linhas l where replace(replace(replace(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')=p1.rml_numero_a and l.codigo_tipo ='" + ptipo_linha + "')"
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo in(2,4,6)  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo <> 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        'strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(p1.tipo_serv2,p1.tipo_serv),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "


        System.Web.HttpContext.Current.Response.Write(strSQL)
        System.Web.HttpContext.Current.Response.End()


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(reader)

        Return dt

    End Function

    Public Function GetRelatioFixo_old(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pFatura As String, ByVal pVencimento As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select nvl(decode(descricao,'Automatico','NÃO CLASSIFICADA',descricao),'NÃO CLASSIFICADA')descricao, decode(tipo,'N','Normal','R','Reduzido')tipo,sum(nvl(valor_cdr,0))valor_faturado,nvl(sum(valor_audit),0)valor_audit, sum(nvl(chamadas,0))Chamadas,sum(nvl(minutos,0))minutos, tarifa,(sum(valor_cdr)-sum(valor_audit))contestar "
        strSQL = strSQL + "   from (select p02.descricao, p01.tipo,p01.chamadas,p01.minutos, p01.codigo_horario,p01.horario,p01.valor_cdr,p01.valor_audit,p02.tipo_tarifa, p02.codigo tipo_ligacao, case when p02.ttm=30 then p02.valor_ttm * 2 else p02.valor_ttm end tarifa "
        strSQL = strSQL + "           from (select (select v.tipo_tarifa "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "'"
        End If
        strSQL = strSQL + "                            and rownum < 2) tipo, "
        strSQL = strSQL + "                        (select v.codigo_horario "
        strSQL = strSQL + "                           from vtipochamadaporhora v "
        strSQL = strSQL + "                          where v.horario <= p0.horario "
        If pOperadora > 0 Then
            strSQL = strSQL + "and v.oper_codigo_operadora = '" + Convert.ToString(pOperadora) + "' "
        End If
        strSQL = strSQL + "                            and rownum < 2) codigo_horario, "
        strSQL = strSQL + "                        p0.horario, "
        strSQL = strSQL + "                        valor_cdr, "
        strSQL = strSQL + "                        valor_audit, minutos, "
        strSQL = strSQL + "                        chamadas "
        strSQL = strSQL + "                   from (select billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                          'DD/MM/YYYY HH24:MI')) horario, "
        strSQL = strSQL + "                                sum(p00.valor_cdr) valor_cdr,sum(p00.valor_audit) valor_audit,sum(chamadas)chamadas,sum(minutos)minutos, "
        strSQL = strSQL + "                                p00.codigo_tipo_ligacao "
        strSQL = strSQL + "                           from ( "
        strSQL = strSQL + "                                 select to_char(sysdate, 'dd/MM/YYYY ') || "
        strSQL = strSQL + "                                         to_char(p1.data_inicio, 'HH24:MI') data_inicio, "
        strSQL = strSQL + "                                         sum(valor_cdr) valor_cdr, "
        strSQL = strSQL + "                                         sum(valor_audit) valor_audit, "
        strSQL = strSQL + "                                         count(*)chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos, "
        strSQL = strSQL + "                                         codigo_tipo_ligacao "
        strSQL = strSQL + "                                   from cdrs_celular p1 where 1=1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                  and p1.codigo_conta in "
        strSQL = strSQL + "                                        (select codigo_conta "
        strSQL = strSQL + "                                           from faturas_arquivos "
        strSQL = strSQL + "                                          where codigo_fatura in "
        strSQL = strSQL + "                                                (select codigo_fatura "
        strSQL = strSQL + "                                                   from faturas "
        strSQL = strSQL + "                                                  where 1=1 and codigo_tipo='2' "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                    and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                    and p1.data_inicio is not null "
        strSQL = strSQL + "                                    and p1.codigo_tipo_ligacao in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                  group by to_char(p1.data_inicio, 'HH24:MI'), "
        strSQL = strSQL + "                                            codigo_tipo_ligacao) p00 "
        strSQL = strSQL + "                          group by billingaudit.segundosDesdeDomingo(to_date(p00.data_inicio, "
        strSQL = strSQL + "                                                                             'DD/MM/YYYY HH24:MI')), "
        strSQL = strSQL + "                                   p00.codigo_tipo_ligacao) p0 "
        strSQL = strSQL + "                 ) p01, "
        strSQL = strSQL + "                (select distinct p4.codigo, p3.codigo_horario, p2.ttm, p2.valor_ttm, p4.descricao,p1.tipo_tarifa "
        strSQL = strSQL + "                   from tarifas_teste               p2, "
        strSQL = strSQL + "                        horarios_tarifacao_teste    p1, "
        strSQL = strSQL + "                        horarios_tarifacao_op_teste p3, "
        strSQL = strSQL + "                        tipos_ligacao_teste         p4, "
        strSQL = strSQL + "                        tarifacao                   t "
        strSQL = strSQL + "                  where p1.codigo_tarifa = p2.codigo and t.tipo_tarifa=2 "
        strSQL = strSQL + "                    and p3.codigo_horario = p1.codigo "
        strSQL = strSQL + "                    and p3.codigo_tipo_ligacao = p4.codigo "
        strSQL = strSQL + "                    and p4.codigo_tarif = t.codigo           "
        strSQL = strSQL + "                    and p1.horario<>604800                    "
        strSQL = strSQL + "                    ) p02 "
        strSQL = strSQL + "          where p01.codigo_horario = p02.codigo_horario and p02.tipo_tarifa=decode(p01.tipo,'S',1,'R',2,'N',4,'D',8)) "
        strSQL = strSQL + " group by descricao, tipo, tarifa "

        '//////////LOGAÇÕES QUE NÃO ESTÃO NA HORÁRIO TARIFAÇÃO////////////////

        strSQL = strSQL + "UNION select nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO') descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo='2' "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo = 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste p55,tipos_ligacao_teste p33, tarifacao p44 where p55.codigo_operadora='" + Convert.ToString(pOperadora) + "' and p55.codigo_tipo_ligacao = p33.codigo and p33.codigo_tarif=p44.codigo and p44.tipo_tarifa=2) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(decode(p2.descricao,'Automatico',p1.tipo_serv,p2.descricao), 'SEM CLASSIFICAÇÃO'),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "

        strSQL = strSQL + "UNION select nvl(p1.tipo_serv2,p1.tipo_serv) descricao, 'Normal' Tipo,"
        strSQL = strSQL + "                                       sum(valor_cdr) valor_faturado, "
        strSQL = strSQL + "                                       sum(valor_audit) valor_audit,"
        strSQL = strSQL + "                                       count(*) chamadas, sum(nvl(ROUND((p1.data_fim-p1.data_inicio)*(1440),2),0)) minutos,  "
        strSQL = strSQL + "                                       round(case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end,2) tarifa, "
        strSQL = strSQL + "                                       (sum(valor_cdr)-sum(valor_audit)) Contestar "
        strSQL = strSQL + "                                  from cdrs_celular p1, tipos_ligacao_teste p2  "
        strSQL = strSQL + "                                 where 1 = 1  "
        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and p1.grp_codigo='" & pCCusto & "' "
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(System.Web.HttpContext.Current.Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        strSQL = strSQL + "                                   and p1.codigo_conta in "
        strSQL = strSQL + "                                       (select codigo_conta "
        strSQL = strSQL + "                                          from faturas_arquivos "
        strSQL = strSQL + "                                         where codigo_fatura in "
        strSQL = strSQL + "                                               (select codigo_fatura "
        strSQL = strSQL + "                                                  from faturas "
        strSQL = strSQL + "                                                 where 1 = 1 and codigo_tipo='2'  "
        If pOperadora > 0 Then
            strSQL = strSQL + " and codigo_operadora ='" + Convert.ToString(pOperadora) + "'"
        End If
        If Not String.IsNullOrEmpty(pVencimento) Then
            strSQL = strSQL + " and to_char(dt_vencimento,'MM/YYYY')='" + pVencimento + "'"
        End If
        If Not String.IsNullOrEmpty(pFatura) Then
            strSQL = strSQL + " and codigo_fatura in(" + Convert.ToString(pFatura) + ") "
        End If
        strSQL = strSQL + "                                    )) "
        strSQL = strSQL + "                                                   and p1.cdr_codigo <> 3 "
        strSQL = strSQL + "                                   and p1.data_inicio is not null "
        'strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao not in (select distinct codigo_tipo_ligacao from horarios_tarifacao_op_teste) "
        strSQL = strSQL + "                                   and p1.codigo_tipo_ligacao=p2.codigo(+) "
        strSQL = strSQL + "                                   group by p1.codigo_tipo_ligacao, nvl(p1.tipo_serv2,p1.tipo_serv),case when p2.ttm=30 then p2.valor_ttm * 2 else p2.valor_ttm end "


        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()


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
