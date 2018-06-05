Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic


Public Class FaturasControleDAL

    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

    Private _strConn As String = ""
    Private sqlLog As String = ""
    Private _daoCommons As New DAO_Commons

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


    Public Function InsereFaturaControle2(ByVal pCodigo As Integer, ByVal pFatura As String, ByVal pCodigoOperadora As Integer, ByVal pCodigoTipo As Integer, ByVal pIntervaloMes As Integer, ByVal pdataInicio As Date) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            'Dim strSQL As String = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values ('" & pCodigoVas & "','" & pCodigoOperadora & "','" & pCodigoLinha & "')"
            Dim strSQL As String = "insert into faturas_controle(codigo_faturas_controle,fatura,codigo_operadora,codigo_tipo,intervalo_mes,data_inicio) values ((select nvl(max(codigo_faturas_controle),0)+1 from faturas_controle),'" & pFatura & "','" & pCodigoOperadora & "','" & pCodigoTipo & "','" & pIntervaloMes & "','" & pdataInicio.Year & "/" & pdataInicio.Month & " / " & pdataInicio.Day & "')"

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

    Public Function InsereFaturaControle(ByVal pFatura As Fatura) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            'Dim strSQL As String = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values ('" & pCodigoVas & "','" & pCodigoOperadora & "','" & pCodigoLinha & "')"
            Dim strSQL As String = ""
            If Not pFatura.DataFim = Nothing Then
                strSQL = "insert into faturas_controle(codigo_faturas_controle,fatura,codigo_operadora,codigo_tipo,intervalo_mes,data_inicio,data_fim,febraban,codigo_estado,debito_automatico,codigo_debito,dia_vencimento,cnpj_cliente,nome_cliente,codigo_servico, CICLO_INI,CICLO_FIM) "
                strSQL = strSQL + " values ((select nvl(max(codigo_faturas_controle),0)+1 from faturas_controle)"
                strSQL = strSQL + " ,'" & pFatura.Fatura & "','" & pFatura.CodigoOperadora & "','" & pFatura.CodigoTipo & "','" & pFatura.IntevaloMes & "','" & pFatura.DtReferencia.Year & "/" & pFatura.DtReferencia.Month & " / " & pFatura.DtReferencia.Day & "'"
                strSQL = strSQL + " ,'" & pFatura.DataFim.Year & "/" & pFatura.DataFim.Month & " / " & pFatura.DataFim.Day & "','" & pFatura.Febraban & "','" & pFatura.Estado.Codigo & "','" & pFatura.DebitoAutomatico & "','" & pFatura.IndentContaUnica & "'"
                strSQL = strSQL + " ,'" & pFatura.DiaVencimento & "','" & pFatura.CNPJ & "','" & pFatura.NomeCliente & "','" & pFatura.Codigo_Servico & "','" & pFatura.Ciclo_ini & "','" & pFatura.Ciclo_fim & "')"
            Else
                strSQL = "insert into faturas_controle(codigo_faturas_controle,fatura,codigo_operadora,codigo_tipo,intervalo_mes,data_inicio,febraban,codigo_estado,debito_automatico,codigo_debito,dia_vencimento,cnpj_cliente,nome_cliente,codigo_servico,CICLO_INI,CICLO_FIM) "
                strSQL = strSQL + " values ((select nvl(max(codigo_faturas_controle),0)+1 from faturas_controle),'" & pFatura.Fatura & "','" & pFatura.CodigoOperadora & "','" & pFatura.CodigoTipo & "','" & pFatura.IntevaloMes & "'"
                strSQL = strSQL + ",'" & pFatura.DtReferencia.Year & "/" & pFatura.DtReferencia.Month & " / " & pFatura.DtReferencia.Day & "','" & pFatura.Febraban & "','" & pFatura.Estado.Codigo & "','" & pFatura.DebitoAutomatico & "'"
                strSQL = strSQL + ",'" & pFatura.IndentContaUnica & "','" & pFatura.DiaVencimento & "','" & pFatura.CNPJ & "','" & pFatura.NomeCliente & "','" & pFatura.Codigo_Servico & "','" & pFatura.Ciclo_ini & "','" & pFatura.Ciclo_fim & "')"
            End If


            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True
        Catch ex As Exception
            EscreveLog("Erro na aplicação: " & ex.Message)

            connection.Close()
            Return False
        End Try
    End Function

    Public Function AtualizaFaturaControle(ByVal pFatura As Fatura) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try

            Dim strSQL As String = "update faturas_controle set "
            strSQL = strSQL & " fatura='" & pFatura.Fatura & "',"
            strSQL = strSQL & " codigo_operadora='" & pFatura.CodigoOperadora & "',"
            strSQL = strSQL & " codigo_tipo='" & pFatura.CodigoTipo & "',"
            strSQL = strSQL & " intervalo_mes='" & pFatura.IntevaloMes & "',"
            strSQL = strSQL & " data_inicio='" & pFatura.DtReferencia.Year & "/" & pFatura.DtReferencia.Month & "/" & pFatura.DtReferencia.Day & "',"
            If Not pFatura.DataFim = Nothing Then
                strSQL = strSQL & " data_fim='" & pFatura.DataFim.Year & "/" & pFatura.DataFim.Month & "/" & pFatura.DataFim.Day & "',"
            Else
                strSQL = strSQL & " data_fim=null,"
            End If
            strSQL = strSQL & " febraban='" & pFatura.Febraban & "',"
            strSQL = strSQL & " codigo_estado='" & pFatura.Estado.Codigo & "',"
            strSQL = strSQL & " debito_automatico='" & pFatura.DebitoAutomatico & "',"
            strSQL = strSQL & " codigo_debito='" & pFatura.IndentContaUnica & "',"
            strSQL = strSQL & " cnpj_cliente='" & pFatura.CNPJ & "',"
            strSQL = strSQL & " nome_cliente='" & pFatura.NomeCliente & "',"
            strSQL = strSQL & " dia_vencimento='" & pFatura.DiaVencimento & "', "
            strSQL = strSQL & " codigo_servico='" & pFatura.Codigo_Servico & "', "
            strSQL = strSQL & " ciclo_ini='" & pFatura.Ciclo_ini & "', "
            strSQL = strSQL & " ciclo_fim='" & pFatura.Ciclo_fim & "' "
            strSQL = strSQL & " where codigo_faturas_controle ='" & pFatura.ID & "'"


            'Dim connection As New Data.OleDb.o
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

    Public Function ExcluiFaturaControle(ByVal pId As String) As Boolean
        Try

            Dim strSQL As String = "delete faturas_controle  "
            strSQL = strSQL & " where codigo_faturas_controle ='" & pId & "'"

            Dim connection As New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
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


    Public Function GetFaturasControle(ByVal pOperadora As String) As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)



        Dim strSQL As String = ""
        strSQL = "SELECT p1.ciclo_ini, p1.ciclo_fim, p1.CODIGO_FATURAS_CONTROLE, p1.FATURA, p1.CODIGO_OPERADORA,p3.descricao, p1.INTERVALO_MES, p2.TIPO, p1.DATA_INICIO, P1.DATA_FIM,P1.FEBRABAN, p1.codigo_tipo FROM FATURAS_CONTROLE p1, faturas_tipo p2, operadoras_teste p3 where(p1.codigo_tipo = p2.codigo_tipo) and p1.codigo_operadora = p3.codigo"

        If pOperadora > 0 Then
            strSQL = strSQL + " and p1.CODIGO_OPERADORA='" & pOperadora & "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _fatura As New Fatura(reader.Item("CODIGO_FATURAS_CONTROLE").ToString, reader.Item("FATURA").ToString, reader.Item("DATA_INICIO").ToString, reader.Item("INTERVALO_MES").ToString, reader.Item("CODIGO_OPERADORA").ToString, reader.Item("descricao").ToString, reader.Item("codigo_tipo").ToString)
                _fatura.DataFim = IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim"))
                _fatura.Ciclo_ini = reader.Item("ciclo_ini").ToString
                _fatura.Ciclo_fim = reader.Item("ciclo_fim").ToString
                listFaturasControle.Add(_fatura)
            End While
        End Using

        Return listFaturasControle

    End Function



    Public Function GetFaturasNaocadastradas(ByVal pOperadoras As Integer, ByVal pdataIncio As String, ByVal pdataFim As String) As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = ""
            strSQL = "SELECT p1.FATURA, p1.CODIGO_OPERADORA,p1.INTERVALO_MES, p1.CODIGO_TIPO, p1.DATA_INICIO,nvl(p2.descricao,'-')operadora, p1.data_fim,nvl(p3.tipo,'SEM TIPO')tipo FROM FATURAS_CONTROLE p1, operadoras_teste p2, faturas_tipo p3 where p1.codigo_operadora=p2.codigo  and p1.CODIGO_TIPO=p3.codigo_tipo "

            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
            End If

            If Not String.IsNullOrEmpty(pdataIncio) Then
                strSQL = strSQL + " and p1.DATA_INICIO >=to_date('" & pdataIncio & "','MM/YYYY')"
            End If
            If Not String.IsNullOrEmpty(pdataFim) Then
                strSQL = strSQL + " and nvl(p1.data_fim,sysdate) <=to_date('" & pdataFim & "','MM/YYYY')"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _fatura As New Fatura(reader.Item(0).ToString, reader.Item(4).ToString, reader.Item(2).ToString, reader.Item(1).ToString, reader.Item("operadora").ToString)
                    _fatura.DataFim = IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim"))
                    _fatura.Tipo = reader.Item("tipo")
                    listFaturasControle.Add(_fatura)
                End While
            End Using
            connection.Close()
            'varre a lista de controle de faturas e verifica quais faturas não foram carregadas
            For Each Fatura As Fatura In listFaturasControle
                Dim myDate As Date = Fatura.DtReferencia
                Dim DatafIm As Date = Fatura.DataFim
                If String.IsNullOrEmpty(Fatura.DataFim) Or Fatura.DataFim.ToShortDateString = "01/01/0001" Then
                    DatafIm = Date.Now
                End If

                While myDate <= Date.Now And myDate <= DatafIm

                    strSQL = "Select 0 from faturas where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "'"
                    connection = New OleDbConnection(strConn)
                    connection.Open()
                    Dim cmd2 As OleDbCommand = connection.CreateCommand
                    cmd2.CommandText = strSQL
                    Dim reader2 As OleDbDataReader
                    reader2 = cmd2.ExecuteReader
                    Using connection
                        If Not reader2.HasRows Then
                            If (DatafIm = Nothing Or myDate <= DatafIm) Then
                                Dim _novafaturas As Fatura = New Fatura(Fatura.Fatura, myDate, Fatura.IntevaloMes, Fatura.CodigoOperadora, Fatura.Operadora)
                                _novafaturas.Tipo = Fatura.Tipo
                                listFaturasNaoCadastradas.Add(_novafaturas)
                            End If
                        End If
                    End Using
                    connection.Close()
                    myDate = myDate.AddMonths(Fatura.IntevaloMes)

                End While

            Next

            cmd.Dispose()
        Catch ex As Exception
            connection.Close()
        End Try
        Return listFaturasNaoCadastradas
    End Function

    Public Function GetFaturasManuais(ByVal pOperadoras As Integer, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal ptipo As Integer, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String, Optional ByVal pjustificativa As String = "", Optional lote As String = "") As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)

        'Try

        Dim sqlContestacao As String = ""
        sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,sum(nvl(p11.valor_contestado,0))valor_contestado"
        sqlContestacao = sqlContestacao + " from faturas          p10, contestacao      p11 "
        sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+)"
        sqlContestacao = sqlContestacao + " group by  p10.codigo_fatura)p100"

        Dim strSQL As String = ""

        strSQL = "SELECT distinct DECODE(p1.JUSTIFICATIVA, '0' , 'SEM JUSTIFICATIVA', '1','Carregada Manualmente','2','Febraban não Recebido','3','Febraban com erro','4','Não possui Febraban' )as justificativa,p1.codigo_fatura codigo,nvl(p1.descricao,'-')fatura, nvl(p1.CODIGO_OPERADORA,0)CODIGO_OPERADORA,nvl(p3.nome_fantasia,'-')operadora, nvl(p2.TIPO,'-')tipo,nvl(p1.valor,0)valor,to_char(p1.dt_vencimento,'DD')dia_vencimento, to_char(p1.dt_vencimento,'dd/MM/YYYY')dt_vencimento,nvl(p1.codigo_tipo,0)codigo_tipo, nvl(p1.codigo_estado,0)codigo_estado,nvl(e.descricao,'-')estado,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc,nvl(P1.valor_pago,0)valor_pago, nvl(p1.lote,' ')lote,nvl(p1.op,' ')op,to_char(p1.data_financeiro,'dd/MM/YYYY')data_financeiro,case when ((p1.codigo_status<>'4') and (p1.dt_vencimento < sysdate)) then p1.valor else 0 end VALOR_PROVISIONADO,nvl(p11.servico_desc,' ')servico_desc,nvl(gt.status,'') status_agendamento, p100.valor_contestado FROM FATURAS p1, faturas_tipo p2, fornecedores p3, estados e, faturas_status fs, faturas_servicos p11,GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt,  " + sqlContestacao + " where t.codigo_tarefa=gt.codigo(+) and  t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+) and p1.codigo_servico=p11.codigo_servico(+) and (p1.codigo_tipo = p2.codigo_tipo) and p1.CODIGO_FORNECEDOR = p3.codigo and p1.PERIODICA='N' and p1.codigo_estado=e.codigo_estado(+) and p1.codigo_fatura=p100.codigo_fatura"

        'se a mesma estiver cadastrada no controle de faturas nao aperece na lista
        strSQL = strSQL + " and not exists (select 0 from faturas_controle where fatura=p1.descricao and p1.codigo_estado=codigo_estado and codigo_operadora=p1.codigo_operadora and (dia_vencimento=to_char(p1.dt_vencimento,'DD') or (dia_vencimento in (28,29,30,31) and to_char(p1.dt_vencimento,'DD') in (28,29,30,31))) and p1.dt_vencimento between data_inicio and nvl(data_fim,add_months(sysdate,2)))"


        If pOperadoras > 0 Then
            strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
        End If
        If pjustificativa <> "0" And pjustificativa <> Nothing Then
            strSQL = strSQL + " and p1.JUSTIFICATIVA='" & pjustificativa & "'"
        End If
        If ptipo > 0 Then
            strSQL = strSQL + " and p1.codigo_tipo='" & ptipo & "'"
        End If
        If Not String.IsNullOrEmpty(pdataIncio) Then
            strSQL = strSQL + " and p1.dt_vencimento >=to_date('" & pdataIncio & "','MM/YYYY')"
        End If
        If Not String.IsNullOrEmpty(pdataFim) Then
            strSQL = strSQL + " and trunc(to_date(p1.dt_vencimento), 'MONTH') <=to_date('" & pdataFim & "','MM/YYYY')"
        End If
        If pPeriodoCarregada <> "" Then
            strSQL = strSQL + "and trunc(p1.dt_criacao)>=trunc(sysdate)-" & pPeriodoCarregada & ""
        End If
        If pStatusPagamento <> "" Then
            strSQL = strSQL + "and p1.codigo_status='" & pStatusPagamento & "'"
        End If
        If pNotaFiscal <> "" Then
            strSQL = strSQL + "and p1.nota_fiscal like'" & pNotaFiscal & "%'"
        End If
        If pCodigoCliente <> "" Then
            strSQL = strSQL + "and p1.codigo_cliente like '" & pCodigoCliente & "%'"
        End If
        If pIdentContaUnica <> "" Then
            strSQL = strSQL + "and p1.ident_conta_unica like '" & pIdentContaUnica & "%'"
        End If
        If lote <> "" Then
            'pesquisa lote de encaminhamento
            strSQL = strSQL + " and exists ( select 0 from LOTE_PAGAMENTO_FATURAS t where t.num_lote='" & lote & "' and t.codigo_fatura=p1.codigo_fatura)"

        End If


        'strSQL = strSQL + " order by fatura, dt_vencimento "

        strSQL = strSQL + " order by operadora,estado,tipo,dia_vencimento,fatura   "

        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()

        connection = New OleDbConnection(strConn)
        connection.Open()
        Dim cmd2 As OleDbCommand = connection.CreateCommand
        cmd2.CommandText = strSQL
        Dim reader2 As OleDbDataReader
        reader2 = cmd2.ExecuteReader
        Using connection
            While reader2.Read
                Dim _novafatura As Fatura = New Fatura(reader2.Item("fatura"), reader2.Item("dt_vencimento"), 1, reader2.Item("CODIGO_OPERADORA"), reader2.Item("operadora"))
                _novafatura.Carregada = 7
                _novafatura.Valor = reader2.Item("valor")
                _novafatura.ValorCorreto = 0
                _novafatura.DiaVencimento = reader2.Item("dia_vencimento")
                _novafatura.ID = reader2.Item("codigo")
                _novafatura.CodigoTipo = reader2.Item("codigo_TIPO")
                _novafatura.Tipo = reader2.Item("TIPO")
                _novafatura.Codigo_Status = reader2.Item("codigo_status")
                _novafatura.Status_desc = reader2.Item("status_desc")
                _novafatura.Servico_Desc = reader2.Item("Servico_Desc")
                _novafatura.DTVencimento = reader2.Item("dt_vencimento")
                _novafatura.Valor_Pago = reader2.Item("valor_pago")
                _novafatura.ValorContestado = reader2.Item("valor_contestado")
                _novafatura.Lote = reader2.Item("lote").ToString
                _novafatura.Op = reader2.Item("op").ToString
                _novafatura.ValorProvisionado = reader2.Item("valor_provisionado")
                _novafatura.Justificativa = reader2.Item("JUSTIFICATIVA").ToString
                _novafatura.Status_Agendamento = reader2.Item("status_agendamento").ToString
                _novafatura.DT_Financeiro = IIf(IsDate(reader2.Item("data_financeiro").ToString), reader2.Item("data_financeiro").ToString, Nothing)
                _novafatura.Staus_Ativacao = "CARREGADA MANUALMENTE"
                Dim _estado As New Estado(reader2.Item("codigo_estado").ToString, reader2.Item("estado").ToString)
                _novafatura.Estado = _estado
                listFaturas.Add(_novafatura)
            End While
        End Using
        connection.Close()
        cmd2.Dispose()
        'Catch ex As Exception
        'connection.Close()
        'System.Web.HttpContext.Current.Response.Write("Erro GetFaturasManuais:" & ex.Message)
        'System.Web.HttpContext.Current.Response.End()
        'End Try
        Return listFaturas
    End Function
    Public Function GetTodasfaturas(ByVal pOperadoras As String, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal pTipo As String, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String, Optional ByVal pjustificativa As String = "", Optional ByVal pNomeCliente As String = "", Optional ByVal pStatusContestacao As String = "", Optional buscaCompetencia As String = "", Optional lote As String = "", Optional num_linha As String = "", Optional codigoEstado As String = "") As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        Dim dataAgora As Date = Date.Now
        _daoCommons.strConn = strConn

        If Not String.IsNullOrEmpty(pdataFim) Then
            dataAgora = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim
        End If

        Try
            Dim strSQL As String = ""
            strSQL = "SELECT p1.FATURA, p1.CODIGO_OPERADORA,p1.INTERVALO_MES, p1.CODIGO_TIPO, to_char(p1.DATA_INICIO,'dd/mm/yyyy')DATA_INICIO,p2.descricao operadora,nvl(p1.dia_vencimento,1)dia_vencimento, data_fim, nvl(p3.tipo,'SEM TIPO')tipo,p1.codigo_estado, nvl(e.descricao,'-')estado,nvl(p1.CNPJ_CLIENTE,' ')cnpj,p1.codigo_faturas_controle  FROM FATURAS_CONTROLE p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)  "

            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
            End If
            If pTipo > 0 Then
                strSQL = strSQL + " and p3.codigo_tipo='" & pTipo & "'"
            End If
            If pCodigoCliente <> "" Then
                strSQL = strSQL + "and p1.fatura like '" & pCodigoCliente & "%'"
            End If
            If pNomeCliente <> "" Then
                strSQL = strSQL + "and p1.nome_cliente like '" & pNomeCliente & "%'"
            End If
            If codigoEstado <> "" Then
                strSQL = strSQL + "and p1.codigo_estado='" & codigoEstado & "'"
            End If

            'filtra adata de inicio e fim
            If Not String.IsNullOrEmpty(pdataIncio) Then
                'strSQL = strSQL + "and to_date(to_char(p1.DATA_INICIO,'mm/yyyy'),'mm/yyyy')<=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
                'strSQL = strSQL + "and to_date(to_char(p1.DATA_INICIO,'mm/yyyy'),'mm/yyyy')>=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
            End If
            If Not String.IsNullOrEmpty(pdataIncio) Then
                strSQL = strSQL + "and to_date(to_char(nvl(p1.DATA_FIM,ADD_MONTHS(sysdate,3)),'mm/yyyy'),'mm/yyyy')>=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
            End If
            strSQL = strSQL + " order by p2.descricao,e.descricao,p3.codigo_tipo,p1.dia_vencimento,p1.FATURA   "




            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            sqlLog = strSQL

            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _fatura As Fatura = New Fatura(reader.Item(0).ToString, reader.Item(4).ToString, reader.Item(2).ToString, reader.Item(1).ToString, reader.Item("operadora").ToString)
                    _fatura.DiaVencimento = reader.Item("dia_vencimento").ToString
                    _fatura.DataFim = IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim"))
                    _fatura.Tipo = reader.Item("tipo").ToString
                    _fatura.ID = reader.Item("codigo_faturas_controle")
                    Dim _estado As New Estado(reader.Item("codigo_estado").ToString, reader.Item("estado").ToString)
                    _fatura.Estado = _estado
                    _fatura.CNPJ = reader.Item("CNPJ").ToString
                    listFaturasControle.Add(_fatura)
                End While
            End Using
            'System.Web.HttpContext.Current.Response.Write("Passou")
            connection.Close()
            'varre a lista de controle de faturas e verifica quais faturas não foram carregadas
            For Each Fatura As Fatura In listFaturasControle
                Dim myDate As Date = Fatura.DtReferencia
                Dim DatafIm As Date = Fatura.DataFim
                Dim faturamentoIndevido As Boolean = False
                If String.IsNullOrEmpty(Fatura.DataFim) Or Fatura.DataFim.ToShortDateString = "01/01/0001" Or Fatura.DataFim.ToShortDateString = "1/1/0001" Then

                    If Not String.IsNullOrEmpty(pdataFim) Then
                        DatafIm = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim

                    Else
                        DatafIm = Date.Now
                        DatafIm = DateTime.Now.AddMonths(3)
                    End If

                End If

                'se o fim do ciclo é menor que a data atual esta fatura não deveria ter sido carregada
                If DatafIm < Now.Date Then
                    DatafIm = DateTime.Now.AddMonths(3)
                End If

                While myDate <= dataAgora And myDate <= DatafIm

                    Dim strDiavencimento As String = Fatura.DiaVencimento

                    'se for dia 30 ou dia 31 coloca o ultimo dia do mês
                    If (Fatura.DiaVencimento = "31" Or Fatura.DiaVencimento = "30" Or Fatura.DiaVencimento = "29") And Convert.ToInt32(Fatura.DiaVencimento) > Date.DaysInMonth(myDate.Year, myDate.Month) Then

                        strDiavencimento = Date.DaysInMonth(myDate.Year, myDate.Month)
                        'strDiavencimento = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))

                    End If

                    'vamos pegar o proximo dia util do mês
                    'myDate = GetProximoDiaUtil(Format(myDate, "dd/MM/yyyy"))
                    'strDiavencimento = Format(myDate, "dd")

                    'strSQL = "Select valor,nvl((select sum(valor_cdr) from cdrs_celular_analitico_mv where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=faturas.codigo_fatura)),0)valor_carregado,to_char(dt_vencimento,'dd')dt_vencimento  from faturas where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "'"
                    'strSQL = "Select p1.codigo_fatura, p1.valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  nvl((select distinct nota_fiscal from cdrs_celular_resumo where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=p1.codigo_fatura) and rownum<2),'-')as nota_fiscal,p1.codigo_tipo from faturas p1, faturas_tipo p3 where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"
                    strSQL = "Select distinct nvl(p1.valor_pago, 0) as valor_pago,p1.codigo_fatura, p1.valor, case when p1.valor_correto=0 then p1.valor else nvl(p1.valor_correto, p1.valor) end valor_correto,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  '-' as nota_fiscal,p1.codigo_tipo, nvl(p1.cnpj_cliente,' ')cnpj,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc, nvl(gt.status,'') status_agendamento, nvl(p1.febraban,'N')febraban, nvl(p1.periodica,'N')periodica,nvl(ct.descricao,'NÃO REALIZADA')status_contestacao  "

                    'strSQL = strSQL + "from faturas p1, faturas_tipo p3, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt where t.codigo_tarefa=gt.codigo(+) and  t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+) and upper(p1.descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"

                    strSQL = strSQL + "from faturas p1, faturas_tipo p3, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt,VCONTESTACOESFATURAS c,CONTESTACAO_STATUS ct where t.codigo_tarefa=gt.codigo(+) and  t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+) and p1.codigo_fatura=c.codigo_fatura(+) and c.status=ct.codigo(+) and upper(p1.descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and   "

                    If buscaCompetencia = "S" Then
                        strSQL += " (to_char(dt_referencia,'MM/YYYY')='" & FormataNumero(myDate.Month) & "/" & myDate.Year & "')"
                    Else
                        strSQL += " (to_char(dt_vencimento,'DD/MM/YYYY')=to_char(next_business_day(to_date('" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "','DD/MM/YYYY')),'DD/MM/YYYY') or to_char(dt_vencimento,'DD/MM/YYYY')= '" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "')"

                        'Sulamerica
                        'strSQL += "  to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' "

                    End If


                    strSQL += " and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"

                    If pPeriodoCarregada <> "" Then
                        strSQL = strSQL + "and trunc(p1.dt_criacao)>=trunc(sysdate)-" & pPeriodoCarregada & ""
                    End If

                    If pStatusPagamento <> "" Then
                        strSQL = strSQL + "and p1.codigo_status='" & pStatusPagamento & "'"
                    End If

                    If pNotaFiscal <> "" Then
                        strSQL = strSQL + "and p1.nota_fiscal like'" & pNotaFiscal & "%'"
                    End If
                    If pCodigoCliente <> "" Then
                        'strSQL = strSQL + "and p1.codigo_cliente like '" & pCodigoCliente & "%'"
                        strSQL = strSQL + "and p1.descricao like '" & pCodigoCliente & "%'"
                    End If
                    If pIdentContaUnica <> "" Then
                        strSQL = strSQL + "and p1.ident_conta_unica like '" & pIdentContaUnica & "%'"
                    End If
                    If pjustificativa <> "0" And pjustificativa <> Nothing Then
                        strSQL = strSQL + " and p1.JUSTIFICATIVA='" & pjustificativa & "'"
                    End If

                    If pStatusContestacao <> "" Then
                        strSQL = strSQL + " and c.status= '" & pStatusContestacao & "'"
                    End If

                    If lote <> "" Then
                        'pesquisa lote de encaminhamento
                        strSQL = strSQL + " and exists ( select 0 from LOTE_PAGAMENTO_FATURAS t where t.num_lote like '" & lote & "%' and t.codigo_fatura=p1.codigo_fatura)"

                    End If

                    If num_linha <> "" Then
                        strSQL = strSQL + " and exists ( select 0 from cdrs_celular_analitico_mv c, faturas_arquivos fa where c.codigo_conta=fa.codigo_conta and fa.codigo_fatura=p1.codigo_fatura and c.rml_numero_a='" & num_linha & "')"
                    End If

                    If codigoEstado <> "" Then
                        strSQL = strSQL + "and p1.codigo_estado='" & codigoEstado & "'"
                    End If


                    connection = New OleDbConnection(strConn)
                    connection.Open()
                    Dim cmd2 As OleDbCommand = connection.CreateCommand
                    cmd2.CommandText = strSQL
                    sqlLog = strSQL
                    Dim reader2 As OleDbDataReader
                    reader2 = cmd2.ExecuteReader


                    If Not String.IsNullOrEmpty(pdataFim) Then
                        strSQL = strSQL + " and nvl(p1.data_fim,to_date('" & Date.DaysInMonth(dataAgora.Year, dataAgora.Month) & "/" & pdataFim & "','DD/MM/YYYY')) <=to_date('" & pdataFim & "','MM/YYYY')"
                    End If


                    Dim myfaturas As Fatura = New Fatura(Fatura.Fatura, myDate, Fatura.IntevaloMes, Fatura.CodigoOperadora, Fatura.Operadora)
                    'verifica se a fatura foi carregada
                    Using connection

                        myfaturas.Carregada = 1

                        myfaturas.Valor = Fatura.Valor
                        myfaturas.DiaVencimento = Fatura.DiaVencimento
                        myfaturas.Estado = Fatura.Estado
                        myfaturas.CNPJ = Fatura.CNPJ
                        'If (DatafIm = Nothing Or myDate <= DatafIm) Then
                        If Not reader2.HasRows Then
                            'a fatura não foi carregada

                            'vamos verificar se a fatura foi prorrogada
                            Dim sql As String = "select t.justificativa from FATURAS_AVISOS t where t.codigo_faturas_controle='" & Fatura.ID & "' and to_char(t.vencimento,'DD/mm/YYYY')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "' and rownum<2"
                            Dim dt As DataTable = _daoCommons.myDataTable(sql)

                            If dt.Rows.Count > 0 Then
                                'fatura prorrogada
                                If lote <> "" Then
                                    'não carregadas não entram na busca com lotes
                                    myDate = myDate.AddMonths(Fatura.IntevaloMes)
                                    Continue While
                                End If
                                myfaturas.Carregada = 8
                                myfaturas.OBS = dt.Rows(0).Item("justificativa").ToString
                            Else
                                'nao carregada
                                If lote <> "" Or num_linha <> "" Then
                                    'não carregadas não entram na busca com lotes
                                    myDate = myDate.AddMonths(Fatura.IntevaloMes)
                                    Continue While
                                End If
                                myfaturas.Carregada = 2
                            End If
                            myfaturas.Tipo = Fatura.Tipo

                        Else
                            While reader2.Read
                                myfaturas.ID = reader2.Item("codigo_fatura").ToString
                                myfaturas.Valor = reader2.Item("valor").ToString
                                myfaturas.Valorcarregado = reader2.Item("valor_carregado").ToString
                                myfaturas.DiaVencimento = reader2.Item("dt_vencimento").ToString
                                myfaturas.NotaFiscal = reader2.Item("nota_fiscal").ToString
                                myfaturas.CodigoTipo = reader2.Item("codigo_tipo").ToString
                                myfaturas.Tipo = reader2.Item("tipo").ToString
                                myfaturas.CNPJ = reader2.Item("cnpj").ToString
                                myfaturas.Codigo_Status = reader2.Item("codigo_status").ToString
                                myfaturas.Status_desc = reader2.Item("status_desc").ToString
                                myfaturas.Status_Agendamento = reader2.Item("status_agendamento").ToString
                                myfaturas.Febraban = reader2.Item("febraban").ToString
                                myfaturas.Valor_Pago = reader2.Item("valor_pago").ToString
                                myfaturas.StatusContestacao = reader2.Item("status_contestacao").ToString
                                'valor da auditoria
                                myfaturas.ValorCorreto = reader2.Item("valor_correto").ToString

                                If reader2.Item("periodica").ToString <> "S" Then
                                    myfaturas.Carregada = 5
                                End If

                                'se os valores são diferentes ocorreu algum erro ao carregar
                                'If FormatNumber(myfaturas.Valor, 2) <> FormatNumber(myfaturas.Valorcarregado, 2) And myfaturas.Febraban = "S" Then
                                If FormatNumber(myfaturas.Valor, 2) <> FormatNumber(myfaturas.Valorcarregado, 2) And reader2.Item("periodica").ToString = "S" Then
                                    myfaturas.Carregada = 4
                                End If

                            End While

                        End If
                        If String.IsNullOrEmpty(pdataFim) Then
                            ' pdataFim = Month(DatafIm) & "/" & Year(DatafIm)
                        End If

                        If Not String.IsNullOrEmpty(pdataIncio) Then
                            If String.IsNullOrEmpty(pdataFim) Then
                                'pdataFim = pdataIncio
                                pdataFim = FormatDateTime(DatafIm, DateFormat.ShortDate).ToString.Substring(3)
                            End If
                            If pdataFim.Length < 7 Then
                                pdataFim = "0" & pdataFim

                            End If

                            If myDate >= New Date(pdataIncio.Substring(3, 4), pdataIncio.Substring(0, 2), 1) And myDate <= New Date(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2), Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))) Then

                                If myfaturas.Carregada = 1 And myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001" Then
                                    'nao nao foi carregada não deve aparecer no grid
                                    myfaturas.Carregada = 6
                                End If
                                If Not ((myfaturas.Carregada = 2 Or myfaturas.Carregada = 8) And (myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001")) Then
                                    listFaturasNaoCadastradas.Add(myfaturas)
                                End If


                            End If
                        Else
                            If (myfaturas.Carregada = 1 Or myfaturas.Carregada = 5) And myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001" Then
                                'nao nao foi carregada não deve aparecer no grid
                                myfaturas.Carregada = 6
                            End If
                            If Not ((myfaturas.Carregada = 2 Or myfaturas.Carregada = 8) And (myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001")) Then
                                listFaturasNaoCadastradas.Add(myfaturas)
                            End If


                        End If


                        'End If

                    End Using
                    connection.Close()
                    myDate = myDate.AddMonths(Fatura.IntevaloMes)
                    If Fatura.IntevaloMes = 0 Then
                        Exit While
                    End If

                End While
            Next

            cmd.Dispose()
        Catch ex As Exception
            EscreveLog("Erro na aplicação: " & ex.Message)
            EscreveLog("Query: " & sqlLog.ToString)

            connection.Close()
            System.Web.HttpContext.Current.Response.Write("Erro GetTodasfaturas:" & ex.Message)
            System.Web.HttpContext.Current.Response.End()
        End Try
        Return listFaturasNaoCadastradas
    End Function

    Private Function GetProximoDiaUtil(ByVal pdata As String) As Date
        Dim connection As New OleDbConnection(strConn)
        Dim _data As Date


        Dim strSQL As String = ""
        strSQL = "select to_char(next_business_day(to_date('" & pdata & " ','DD/MM/YYYY')),'DD/MM/YYYY') from dual"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _data = New Date(reader.Item(0).ToString.Substring(6, 4), reader.Item(0).ToString.Substring(3, 2), reader.Item(0).ToString.Substring(0, 2))
            End While
        End Using
        'System.Web.HttpContext.Current.Response.Write("Passou")
        connection.Close()

        Return _data

    End Function

    Public Function GetFaturasControleNaoCadastradas(ByVal pOperadoras As String, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal ptipo As String, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String, Optional buscaCompetencia As String = "", Optional lote As String = "", Optional num_linha As String = "", Optional codigoEstado As String = "") As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        Dim NF As String = ""
        Try

            Dim sqlContestacao As String = ""
            'sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,max(nvl(p11.valor_contestado,0))valor_contestado"
            sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,sum(nvl(nvl(p11.valor_devolvido,p11.valor_devolvido), 0))valor_contestado"
            sqlContestacao = sqlContestacao + " from faturas          p10, VCONTESTACOESFATURASLINHAS      p11"
            'sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+) "
            sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+)  "
            sqlContestacao = sqlContestacao + " group by  p10.codigo_fatura)p100"


            Dim sqlContestacaoAprovada As String = ""
            sqlContestacaoAprovada = sqlContestacaoAprovada + " (select p10.codigo_fatura,sum(nvl(nvl(p11.valor_devolvido,p11.valor_devolvido), 0))valor_contestado_aprovado"
            sqlContestacaoAprovada = sqlContestacaoAprovada + " from faturas          p10, VCONTESTACOESFATURASLINHAS      p11 "
            'sqlContestacaoAprovada = sqlContestacaoAprovada + " where p10.codigo_fatura = p11.codigo_fatura(+) and p11.aprovada='S'"
            sqlContestacaoAprovada = sqlContestacaoAprovada + " where p10.codigo_fatura = p11.codigo_fatura(+)  "
            sqlContestacaoAprovada = sqlContestacaoAprovada + "  and (p11.aprovada = 'S' or p11.id is null )"
            sqlContestacaoAprovada = sqlContestacaoAprovada + " group by  p10.codigo_fatura)p101"


            Dim strSQL As String = ""
            'strSQL = "SELECT p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,p1.valor,to_char(p1.dt_vencimento,'dd') dt_vencimento,nvl((select distinct nota_fiscal from cdrs_celular_resumo where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=p1.codigo_fatura )and rownum<2),'-')nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and nvl(p1.periodica,'S')='S' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)"
            'strSQL = "SELECT distinct p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,nvl(p1.valor,0)valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p1.valor_correto,0)valor_correto, NVL(P1.VALOR_PAGO,0)VALOR_PAGO,to_char(p1.dt_vencimento,'dd') dt_vencimento,'-' nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado, nvl(P1.CNPJ_cliente,' ')CNPJ, nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,0)status_desc, nvl(gt.status,'') status_agendamento,nvl(ct.descricao,'NÃO REALIZADA')status_contestacao,  NVL(P12.Servico_Desc,'')Servico_Desc  FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt,VCONTESTACOESFATURAS c,CONTESTACAO_STATUS ct,  faturas_servicos p12 where t.codigo_tarefa=gt.codigo(+) and   p1.codigo_servico = p12.codigo_servico(+) and t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+)  and p1.codigo_operadora=p2.codigo and nvl(p1.periodica,'S')='S' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+) and p1.codigo_fatura=c.codigo_fatura(+) and c.status=ct.codigo(+) "
            strSQL = "SELECT distinct p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,nvl(p1.valor,0)valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p1.valor_correto,0)valor_correto, NVL(P1.VALOR_PAGO,0)VALOR_PAGO,to_char(p1.dt_vencimento,'dd') dt_vencimento,'-' nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado, nvl(P1.CNPJ_cliente,' ')CNPJ, nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,0)status_desc, nvl(gt.status,'') status_agendamento,nvl(ct.descricao,'NÃO REALIZADA')status_contestacao,  NVL(P12.Servico_Desc,'')Servico_Desc,p100.valor_contestado,nvl(p101.valor_contestado_aprovado,0)valor_contestado_aprovado  FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt," + sqlContestacao + ", " + sqlContestacaoAprovada + " ,VCONTESTACOESFATURAS c,CONTESTACAO_STATUS ct,  faturas_servicos p12 where  p1.codigo_fatura=p100.codigo_fatura(+) and p1.codigo_fatura=p101.codigo_fatura(+) and t.codigo_tarefa=gt.codigo(+) and   p1.codigo_servico = p12.codigo_servico(+) and t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+)  and p1.codigo_operadora=p2.codigo and nvl(p1.periodica,'S')='S' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+) and p1.codigo_fatura=c.codigo_fatura(+) and c.status=ct.codigo(+) "

            'strSQL = "SELECT distinct p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,p1.valor,nvl(p1.valor_carregado,0)valor_carregado,to_char(p1.dt_vencimento,'dd') dt_vencimento,'-' nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado, nvl(P1.CNPJ_cliente,' ')CNPJ, nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,0)status_desc, nvl(gt.status,'') status_agendamento FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt where t.codigo_tarefa=gt.codigo(+) and  t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+)  and p1.codigo_operadora=p2.codigo  and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)"

            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
            End If
            If ptipo > 0 Then
                strSQL = strSQL + " and p3.codigo_tipo='" & ptipo & "'"
            End If

            If Not String.IsNullOrEmpty(pdataIncio) Then

                If buscaCompetencia = "S" Then
                    strSQL = strSQL + " and p1.dt_referencia >=to_date('" & pdataIncio & "','MM/YYYY')"
                Else
                    strSQL = strSQL + " and p1.dt_vencimento >=to_date('" & pdataIncio & "','MM/YYYY')"
                End If



            End If
            If Not String.IsNullOrEmpty(pdataFim) Then
                Dim _dia As String = IIf(Len(Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))) < 2, "0" & Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)), Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)))
                If buscaCompetencia = "S" Then
                    strSQL = strSQL + " and nvl(p1.dt_referencia,sysdate) <=to_date('" & _dia & "/" & pdataFim & "','DD/MM/YYYY')"
                Else

                    strSQL = strSQL + " and nvl(p1.dt_vencimento,sysdate) <=to_date('" & _dia & "/" & pdataFim & "','DD/MM/YYYY')"
                End If

            End If

            If pPeriodoCarregada <> "" Then
                strSQL = strSQL + "and trunc(p1.dt_criacao)>=trunc(sysdate)-" & pPeriodoCarregada & ""
            End If
            If pStatusPagamento <> "" Then
                strSQL = strSQL + "and p1.codigo_status='" & pStatusPagamento & "'"
            End If
            If pNotaFiscal <> "" Then
                strSQL = strSQL + "and p1.nota_fiscal like'" & pNotaFiscal & "%'"
            End If
            If pCodigoCliente <> "" Then
                'strSQL = strSQL + "and p1.codigo_cliente like '" & pCodigoCliente & "%'"
                strSQL = strSQL + "and p1.descricao like '" & pCodigoCliente & "%'"
            End If
            If pIdentContaUnica <> "" Then
                strSQL = strSQL + "and p1.ident_conta_unica like '" & pIdentContaUnica & "%'"
            End If

            If lote <> "" Then
                'pesquisa lote de encaminhamento
                strSQL = strSQL + " and exists ( select 0 from LOTE_PAGAMENTO_FATURAS t where t.num_lote='" & lote & "' and t.codigo_fatura=p1.codigo_fatura)"

            End If

            If num_linha <> "" Then
                strSQL = strSQL + " and exists ( select 0 from cdrs_celular_analitico_mv c, faturas_arquivos fa where c.codigo_conta=fa.codigo_conta and fa.codigo_fatura=p1.codigo_fatura and c.rml_numero_a='" & num_linha & "')"
            End If

            If codigoEstado <> "" Then
                strSQL = strSQL + "and p1.codigo_estado='" & codigoEstado & "'"
            End If



            'strSQL = strSQL + " order by p1.descricao, p1.dt_vencimento "
            strSQL = strSQL + "  order by p2.descricao,estado,p1.codigo_tipo,p1.dt_vencimento,p1.descricao   "

            'System.Web.HttpContext.Current.Response.Write(strSQL)
            'System.Web.HttpContext.Current.Response.End()


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _fatura As Fatura = New Fatura(reader.Item(0).ToString, reader.Item(4).ToString, reader.Item(2).ToString, reader.Item(1).ToString, reader.Item("operadora").ToString)
                    _fatura.Valor = reader.Item("valor").ToString
                    _fatura.DiaVencimento = reader.Item("dt_vencimento").ToString
                    _fatura.NotaFiscal = reader.Item("nota_fiscal").ToString
                    _fatura.ID = reader.Item("codigo_fatura").ToString
                    _fatura.CodigoTipo = reader.Item("codigo_tipo").ToString
                    _fatura.Tipo = reader.Item("tipo").ToString
                    _fatura.CNPJ = reader.Item("cnpj").ToString
                    _fatura.Codigo_Status = reader.Item("codigo_status").ToString
                    _fatura.Status_desc = reader.Item("status_desc").ToString
                    _fatura.DTVencimento = reader.Item("data_inicio").ToString
                    _fatura.Status_Agendamento = reader.Item("status_agendamento").ToString
                    _fatura.Valorcarregado = reader.Item("valor_carregado").ToString
                    _fatura.ValorCorreto = reader.Item("valor_correto").ToString
                    _fatura.Status_Agendamento = reader.Item("status_agendamento").ToString
                    _fatura.StatusContestacao = reader.Item("status_contestacao").ToString
                    _fatura.ValorContestado = reader.Item("valor_contestado").ToString
                    _fatura.ValorContestadoAprovado = reader.Item("valor_contestado_aprovado").ToString

                    _fatura.Servico_Desc = reader.Item("Servico_Desc").ToString
                    _fatura.Valor_Pago = reader.Item("VALOR_PAGO").ToString
                    Dim _estado As New Estado(reader.Item("codigo_estado").ToString, reader.Item("estado").ToString)
                    _fatura.Estado = _estado
                    listFaturasControle.Add(_fatura)
                End While
            End Using
            connection.Close()
            'varre a lista de controle de faturas e verifica quais faturas não foram carregadas
            For Each Fatura As Fatura In listFaturasControle

                Dim strDiavencimento As String = Fatura.DiaVencimento
                If (Fatura.DiaVencimento = "31" Or Fatura.DiaVencimento = "30" Or Fatura.DiaVencimento = "29") Then

                    strDiavencimento = Date.DaysInMonth(Fatura.DTVencimento.Year, Fatura.DTVencimento.Month)
                    'strDiavencimento = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))

                End If


                strSQL = "Select 0 from faturas_controle where upper(fatura)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and codigo_estado='" & Fatura.Estado.Codigo & "'"
                If (Fatura.DiaVencimento = "31" Or Fatura.DiaVencimento = "30" Or Fatura.DiaVencimento = "29") Then
                    strSQL = strSQL + " and dia_vencimento in (28,29,30,31)  "
                ElseIf (Fatura.DiaVencimento = "28" And Fatura.DTVencimento.Month = "02" And Not System.DateTime.IsLeapYear(Fatura.DTVencimento.Year)) Then
                    strSQL = strSQL + " and dia_vencimento in (28,29,30,31)  "
                Else
                    strSQL = strSQL + " and ( dia_vencimento='" & strDiavencimento & "'  "
                    strSQL = strSQL + " or (to_char(next_business_day(to_date(NVL(case when DIA_VENCIMENTO>=28 then CAST(to_char(LAST_DAY(to_date('" & Fatura.DTVencimento.Month & "/" & Fatura.DTVencimento.Year & "','MM/YYYY')),'DD') as int) else DIA_VENCIMENTO end,'01') || '/" & Fatura.DTVencimento.Month & "/" & Fatura.DTVencimento.Year & "','DD/MM/YYYY')),'DD')='" & FormataNumero(strDiavencimento) & "') ) "
                End If
                strSQL = strSQL + " and data_inicio<=to_date('" & Fatura.DTVencimento.Month & "/" & Fatura.DTVencimento.Year & "','MM/YYYY')"
                strSQL = strSQL + " and nvl(data_fim,sysdate+3600)>=to_date('" & Fatura.DTVencimento.Month & "/" & Fatura.DTVencimento.Year & "','MM/YYYY')"

                'EscreveLog("qUERY " & strSQL)

                connection = New OleDbConnection(strConn)
                connection.Open()
                Dim cmd2 As OleDbCommand = connection.CreateCommand
                cmd2.CommandText = strSQL
                Dim reader2 As OleDbDataReader
                reader2 = cmd2.ExecuteReader

                'verifica se a fatura foi carregada
                Using connection
                    If Not reader2.HasRows Then
                        'a fatura não foi cadastrada
                        Dim myfaturas As Fatura = New Fatura(Fatura.Fatura, Fatura.DtReferencia, Fatura.IntevaloMes, Fatura.CodigoOperadora, Fatura.Operadora)
                        myfaturas.Carregada = 3
                        myfaturas.Valor = Fatura.Valor
                        myfaturas.DiaVencimento = Fatura.DiaVencimento
                        myfaturas.NotaFiscal = Fatura.NotaFiscal
                        myfaturas.ID = Fatura.ID
                        myfaturas.CodigoTipo = Fatura.CodigoTipo
                        myfaturas.Tipo = Fatura.Tipo
                        myfaturas.Estado = Fatura.Estado
                        myfaturas.CNPJ = Fatura.CNPJ
                        myfaturas.Codigo_Status = Fatura.Codigo_Status
                        myfaturas.DTVencimento = Fatura.DTVencimento
                        myfaturas.Status_desc = Fatura.Status_desc
                        myfaturas.Valorcarregado = Fatura.Valorcarregado
                        myfaturas.ValorCorreto = Fatura.ValorCorreto
                        myfaturas.Status_Agendamento = Fatura.Status_Agendamento
                        myfaturas.StatusContestacao = Fatura.StatusContestacao
                        myfaturas.Servico_Desc = Fatura.Servico_Desc
                        myfaturas.Valor_Pago = Fatura.Valor_Pago
                        'myfaturas.Staus_Ativacao = "CARREGADA E NÃO CADASTRADA"
                        myfaturas.ValorContestado = Fatura.ValorContestado
                        myfaturas.ValorContestadoAprovado = Fatura.ValorContestadoAprovado
                        listFaturasNaoCadastradas.Add(myfaturas)
                    End If
                End Using
                connection.Close()
            Next
            cmd.Dispose()
        Catch ex As Exception
            EscreveLog("Erro na aplicação: " & ex.Message)
            connection.Close()
            System.Web.HttpContext.Current.Response.Write("Erro Função2 GetFaturasControleNaoCadastradas:" & ex.Message)
            System.Web.HttpContext.Current.Response.End()
        End Try
        Return listFaturasNaoCadastradas
    End Function


    Public Function GetTodasfaturasControleContas(ByVal pOperadoras As String, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal pTipo As String, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String) As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        Dim dataAgora As Date = Date.Now
        'Dim _DATAINICIO As Date
        'Dim _DATAFIM As Date

        If Not String.IsNullOrEmpty(pdataFim) Then
            dataAgora = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim
        End If

        Try
            Dim strSQL As String = ""
            strSQL = "SELECT p1.FATURA, p1.CODIGO_OPERADORA,p1.INTERVALO_MES, p1.CODIGO_TIPO, to_char(p1.DATA_INICIO,'dd/mm/yyyy')DATA_INICIO,p2.descricao operadora,nvl(p1.dia_vencimento,1)dia_vencimento, data_fim, nvl(p3.tipo,'SEM TIPO')tipo,p1.codigo_estado, nvl(e.descricao,'-')estado,nvl(p1.CNPJ_CLIENTE,' ')cnpj  FROM FATURAS_CONTROLE p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)"

            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
            End If
            If pTipo > 0 Then
                strSQL = strSQL + " and p3.codigo_tipo='" & pTipo & "'"
            End If
            If pCodigoCliente <> "" Then
                strSQL = strSQL + "and p1.fatura like '" & pCodigoCliente & "%'"
            End If

            'filtra adata de inicio e fim
            If Not String.IsNullOrEmpty(pdataIncio) Then
                'strSQL = strSQL + "and to_date(to_char(p1.DATA_INICIO,'mm/yyyy'),'mm/yyyy')<=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
                'strSQL = strSQL + "and to_date(to_char(p1.DATA_INICIO,'mm/yyyy'),'mm/yyyy')>=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
            End If
            If Not String.IsNullOrEmpty(pdataIncio) Then
                strSQL = strSQL + "and to_date(to_char(nvl(p1.DATA_FIM,ADD_MONTHS(sysdate,3)),'mm/yyyy'),'mm/yyyy')>=to_date('" & pdataIncio.Substring(0, 2) & "/" & pdataIncio.Substring(3, 4) & "','MM/YYYY')"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _fatura As Fatura = New Fatura(reader.Item(0).ToString, reader.Item(4).ToString, reader.Item(2).ToString, reader.Item(1).ToString, reader.Item("operadora").ToString)
                    _fatura.DiaVencimento = reader.Item("dia_vencimento").ToString
                    _fatura.DataFim = IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim"))
                    _fatura.Tipo = reader.Item("tipo").ToString
                    Dim _estado As New Estado(reader.Item("codigo_estado").ToString, reader.Item("estado").ToString)
                    _fatura.Estado = _estado
                    _fatura.CNPJ = reader.Item("CNPJ").ToString
                    listFaturasControle.Add(_fatura)
                End While
            End Using
            'System.Web.HttpContext.Current.Response.Write("Passou")
            connection.Close()
            'varre a lista de controle de faturas e verifica quais faturas não foram carregadas
            For Each Fatura As Fatura In listFaturasControle
                Dim myDate As Date = Fatura.DtReferencia
                Dim DatafIm As Date = Fatura.DataFim
                If String.IsNullOrEmpty(Fatura.DataFim) Or Fatura.DataFim.ToShortDateString = "01/01/0001" Or Fatura.DataFim.ToShortDateString = "1/1/0001" Then

                    If Not String.IsNullOrEmpty(pdataFim) Then
                        DatafIm = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim

                    Else
                        DatafIm = Date.Now
                        DatafIm = DateTime.Now.AddMonths(3)
                    End If

                End If

                'se o fim do ciclo é menor que a data atual esta fatura não deveria ter sido carregada
                If DatafIm < Now.Date Then
                    DatafIm = DateTime.Now.AddMonths(3)
                End If

                While myDate <= dataAgora And myDate <= DatafIm

                    Dim strDiavencimento As String = Fatura.DiaVencimento

                    'se for dia 30 ou dia 31 coloca o ultimo dia do mês
                    If (Fatura.DiaVencimento = "31" Or Fatura.DiaVencimento = "30") And Convert.ToInt32(Fatura.DiaVencimento) > Date.DaysInMonth(myDate.Year, myDate.Month) Then

                        strDiavencimento = Date.DaysInMonth(myDate.Year, myDate.Month)
                        'strDiavencimento = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))

                    End If

                    'Dim sqlContestacao As String = ""
                    'sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,sum(nvl(p11.valor_contestado,0))valor_contestado"
                    'sqlContestacao = sqlContestacao + " from faturas          p10, contestacao      p11 "
                    'sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+)"
                    'sqlContestacao = sqlContestacao + " group by  p10.codigo_fatura)p100"

                    Dim sqlContestacao As String = ""
                    'sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,max(nvl(p11.valor_contestado,0))valor_contestado"
                    sqlContestacao = sqlContestacao + " (select p10.codigo_fatura,sum(nvl(nvl(p11.valor_devolvido,p11.valor_devolvido), 0))valor_contestado"
                    sqlContestacao = sqlContestacao + " from faturas          p10, VCONTESTACOESFATURASLINHAS      p11"
                    'sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+) "
                    sqlContestacao = sqlContestacao + " where p10.codigo_fatura = p11.codigo_fatura(+)  "
                    sqlContestacao = sqlContestacao + " group by  p10.codigo_fatura)p100"


                    Dim sqlContestacaoAprovada As String = ""
                    sqlContestacaoAprovada = sqlContestacaoAprovada + " (select p10.codigo_fatura,sum(nvl(nvl(p11.valor_devolvido,p11.valor_devolvido), 0))valor_contestado_aprovado"
                    sqlContestacaoAprovada = sqlContestacaoAprovada + " from faturas          p10, VCONTESTACOESFATURASLINHAS      p11 "
                    'sqlContestacaoAprovada = sqlContestacaoAprovada + " where p10.codigo_fatura = p11.codigo_fatura(+) and p11.aprovada='S'"
                    sqlContestacaoAprovada = sqlContestacaoAprovada + " where p10.codigo_fatura = p11.codigo_fatura(+)  "
                    sqlContestacaoAprovada = sqlContestacaoAprovada + " and (p11.aprovada = 'S' or p11.id is null )"
                    sqlContestacaoAprovada = sqlContestacaoAprovada + " group by  p10.codigo_fatura)p101"

                    'strSQL = "Select valor,nvl((select sum(valor_cdr) from cdrs_celular_analitico_mv where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=faturas.codigo_fatura)),0)valor_carregado,to_char(dt_vencimento,'dd')dt_vencimento  from faturas where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "'"
                    'strSQL = "Select p1.codigo_fatura, p1.valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  nvl((select distinct nota_fiscal from cdrs_celular_resumo where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=p1.codigo_fatura) and rownum<2),'-')as nota_fiscal,p1.codigo_tipo from faturas p1, faturas_tipo p3 where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"
                    strSQL = "Select distinct p1.codigo_fatura,nvl(p1.ident_conta_unica,'')conta_unica, p1.valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  '-' as nota_fiscal,p1.codigo_tipo, nvl(p1.cnpj_cliente,' ')cnpj,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc, nvl(p1.op,'')op, nvl(p1.lote,'')lote,NVL(P12.Servico_Desc,'')Servico_Desc,case when ((p1.codigo_status<>'4') and (p1.dt_vencimento < sysdate)) then p1.valor else 0 end VALOR_PROVISIONADO, NVL(P1.VALOR_PAGO,0)VALOR_PAGO, NVL(P1.OP,'')OP, NVL(P1.LOTE,'')LOTE,to_char(p1.data_financeiro,'DD/MM/YYYY')data_financeiro,p100.valor_contestado,nvl(p101.valor_contestado_aprovado,0)valor_contestado_aprovado,(select 1 from contestacao where status=1 and codigo_fatura=p100.codigo_fatura and rownum<2) as status_contestacao  from faturas p1, faturas_tipo p3, faturas_status fs,faturas_servicos p12, " + sqlContestacao + ", " + sqlContestacaoAprovada + " where p1.codigo_status=fs.codigo_status(+) and p1.codigo_fatura=p100.codigo_fatura(+) and p1.codigo_fatura=p101.codigo_fatura(+) and  p1.codigo_servico = p12.codigo_servico(+) and upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' "

                    'strSQL += " And to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' and p1.CODIGO_TIPO=p3.codigo_tipo(+) and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"
                    strSQL += " and (to_char(dt_vencimento,'DD/MM/YYYY')=to_char(next_business_day(to_date('" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "','DD/MM/YYYY')),'DD/MM/YYYY') or to_char(dt_vencimento,'DD/MM/YYYY')= '" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "')"
                    strSQL += " and p1.CODIGO_TIPO=p3.codigo_tipo(+) and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"


                    'VCONTESTACOESFATURAS

                    If pPeriodoCarregada <> "" Then
                        strSQL = strSQL + "and trunc(p1.dt_criacao)>=trunc(sysdate)-" & pPeriodoCarregada & ""
                    End If

                    If pStatusPagamento <> "" Then
                        strSQL = strSQL + "and p1.codigo_status='" & pStatusPagamento & "'"
                    End If

                    If pNotaFiscal <> "" Then
                        strSQL = strSQL + "and p1.nota_fiscal like'" & pNotaFiscal & "%'"
                    End If
                    'If pCodigoCliente <> "" Then
                    '    strSQL = strSQL + "and p1.codigo_cliente like '" & pCodigoCliente & "%'"
                    'End If
                    If pIdentContaUnica <> "" Then
                        strSQL = strSQL + "and p1.ident_conta_unica like '" & pIdentContaUnica & "%'"
                    End If

                    'System.Web.HttpContext.Current.Response.Write(strSQL)
                    'System.Web.HttpContext.Current.Response.End()

                    connection = New OleDbConnection(strConn)
                    connection.Open()
                    Dim cmd2 As OleDbCommand = connection.CreateCommand
                    cmd2.CommandText = strSQL
                    Dim reader2 As OleDbDataReader
                    reader2 = cmd2.ExecuteReader


                    If Not String.IsNullOrEmpty(pdataFim) Then
                        strSQL = strSQL + " and nvl(p1.data_fim,to_date('" & Date.DaysInMonth(dataAgora.Year, dataAgora.Month) & "/" & pdataFim & "','DD/MM/YYYY')) <=to_date('" & pdataFim & "','MM/YYYY')"
                    End If


                    Dim myfaturas As Fatura = New Fatura(Fatura.Fatura, myDate, Fatura.IntevaloMes, Fatura.CodigoOperadora, Fatura.Operadora)
                    myfaturas.DTVencimento = myDate
                    myfaturas.DtReferencia = Fatura.DtReferencia
                    'verifica se a fatura foi carregada
                    Using connection

                        myfaturas.Carregada = 1

                        myfaturas.Valor = Fatura.Valor
                        myfaturas.DiaVencimento = Fatura.DiaVencimento
                        myfaturas.Estado = Fatura.Estado
                        myfaturas.CNPJ = Fatura.CNPJ
                        'If (DatafIm = Nothing Or myDate <= DatafIm) Then
                        If Not reader2.HasRows Then
                            'a fatura não foi carregada
                            myfaturas.Carregada = 2
                            myfaturas.Tipo = Fatura.Tipo
                            myfaturas.Staus_Ativacao = "NÂO CARREGADA"
                            myfaturas.Status_desc = "FATURA NÃO RECEBIDA"
                        Else
                            While reader2.Read
                                myfaturas.ID = reader2.Item("codigo_fatura").ToString
                                myfaturas.Valor = reader2.Item("valor").ToString
                                myfaturas.Valorcarregado = reader2.Item("valor_carregado").ToString
                                myfaturas.DiaVencimento = reader2.Item("dt_vencimento").ToString
                                myfaturas.NotaFiscal = reader2.Item("nota_fiscal").ToString
                                myfaturas.CodigoTipo = reader2.Item("codigo_tipo").ToString
                                myfaturas.Tipo = reader2.Item("tipo").ToString
                                myfaturas.CNPJ = reader2.Item("cnpj").ToString
                                myfaturas.Codigo_Status = reader2.Item("codigo_status").ToString
                                myfaturas.Status_desc = reader2.Item("status_desc").ToString
                                myfaturas.Servico_Desc = reader2.Item("Servico_Desc").ToString
                                myfaturas.ValorProvisionado = reader2.Item("valor_provisionado").ToString
                                myfaturas.Valor_Pago = reader2.Item("VALOR_PAGO").ToString
                                myfaturas.Op = reader2.Item("op").ToString
                                myfaturas.Lote = reader2.Item("lote").ToString
                                myfaturas.DT_Financeiro = IIf(IsDate(reader2.Item("data_financeiro").ToString), reader2.Item("data_financeiro").ToString, Nothing)
                                myfaturas.IndentContaUnica = reader2.Item("conta_unica").ToString

                                myfaturas.DataFim = Fatura.DataFim
                                myfaturas.ValorContestado = reader2.Item("valor_contestado").ToString
                                myfaturas.ValorContestadoAprovado = reader2.Item("valor_contestado_aprovado").ToString

                                If reader2.Item("status_contestacao").ToString = "1" Then
                                    myfaturas.Status_desc = "AG RET DE CONTESTAÇÃO"
                                End If

                                If Fatura.DataFim < Date.Now And Fatura.DataFim > "01/01/0001" Then
                                    myfaturas.Staus_Ativacao = "CANCELADO"
                                Else
                                    myfaturas.Staus_Ativacao = "ATIVO"
                                End If


                                'se os valores são diferentes ocorreu algum erro ao carregar
                                If FormatNumber(myfaturas.Valor, 2) <> FormatNumber(myfaturas.Valorcarregado, 2) Then
                                    'myfaturas.Carregada = 4
                                End If

                            End While

                        End If
                        If String.IsNullOrEmpty(pdataFim) Then
                            'pdataFim = Month(DatafIm) & "/" & Year(DatafIm)
                        End If

                        If Not String.IsNullOrEmpty(pdataIncio) Then
                            If String.IsNullOrEmpty(pdataFim) Then
                                'pdataFim = pdataIncio

                                pdataFim = FormatDateTime(DatafIm, DateFormat.ShortDate).ToString.Substring(3)
                            End If
                            If pdataFim.Length < 7 Then
                                pdataFim = "0" & pdataFim

                            End If
                            If myDate >= New Date(pdataIncio.Substring(3, 4), pdataIncio.Substring(0, 2), 1) And myDate <= New Date(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2), Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))) Then

                                If myfaturas.Carregada = 1 And myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001" Then
                                    'nao nao foi carregada não deve aparecer no grid
                                    myfaturas.Carregada = 6
                                End If
                                If Not ((myfaturas.Carregada = 2 Or myfaturas.Carregada = 8) And (myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001")) Then
                                    listFaturasNaoCadastradas.Add(myfaturas)
                                End If


                            End If
                        Else
                            If (myfaturas.Carregada = 1 Or myfaturas.Carregada = 5) And myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001" Then
                                'nao nao foi carregada não deve aparecer no grid
                                myfaturas.Carregada = 6
                            End If
                            If Not ((myfaturas.Carregada = 2 Or myfaturas.Carregada = 8) And (myDate > Fatura.DataFim And Fatura.DataFim.ToShortDateString <> "01/01/0001")) Then
                                listFaturasNaoCadastradas.Add(myfaturas)
                            End If


                        End If


                        'End If

                    End Using
                    connection.Close()
                    myDate = myDate.AddMonths(Fatura.IntevaloMes)
                    If Fatura.IntevaloMes = 0 Then
                        Exit While
                    End If

                End While
            Next

            cmd.Dispose()
        Catch ex As Exception
            connection.Close()
            System.Web.HttpContext.Current.Response.Write("Erro Função GetTodasfaturasControleContas:" & ex.Message)
            System.Web.HttpContext.Current.Response.End()
        End Try
        Return listFaturasNaoCadastradas
    End Function


    Function FormataNumero(ByVal pNumero As Integer) As String
        Dim numero As String = ""
        If pNumero < 10 Then
            numero = "0" & pNumero
        Else
            numero = pNumero
        End If

        Return numero

    End Function

    Public Function GetFaturaControleById(ByVal pCodigo As Integer) As Fatura
        Dim fatura As Fatura = Nothing
        Try
            Dim strSQL As String = "select p1.ciclo_ini,p1.ciclo_fim,p1.CODIGO_FATURAS_CONTROLE,p1.fatura,p1.data_inicio,p1.intervalo_mes,p1.codigo_operadora,p1.codigo_tipo,nvl(p1.codigo_estado,0)codigo_estado,nvl(p1.codigo_debito,' ')codigo_debito,nvl(p1.debito_automatico,'N')debito_automatico,nvl(p1.dia_vencimento,0)dia_vencimento,nvl(p2.descricao,' ')estado, data_fim, nvl(febraban,'N')febraban, nvl(cnpj_cliente,' ')cnpj_cliente,nvl(nome_cliente,' ')nome_cliente, nvl(p1.codigo_servico,'-1')codigo_servico from faturas_controle p1, estados p2 where p1.codigo_estado=p2.codigo_estado(+) and p1.CODIGO_FATURAS_CONTROLE='" & pCodigo & "' "
            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    fatura = New Fatura(reader.Item("CODIGO_FATURAS_CONTROLE"), reader.Item("fatura"), reader.Item("data_inicio"), reader.Item("intervalo_mes"), reader.Item("codigo_operadora"), "", reader.Item("codigo_tipo"), New Estado(reader.Item("codigo_estado"), reader.Item("estado")), reader.Item("codigo_debito"), reader.Item("debito_automatico"), reader.Item("dia_vencimento"), 0, IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim")), reader.Item("febraban"), reader.Item("cnpj_cliente"), reader.Item("nome_cliente"))
                    fatura.Codigo_Servico = reader.Item("CODIGO_SERVICO")
                    fatura.Ciclo_ini = reader.Item("ciclo_ini").ToString
                    fatura.Ciclo_fim = reader.Item("ciclo_fim").ToString
                End While
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return fatura
    End Function

    Public Function ExcluiFaturaControle(ByVal pCodigo As Integer) As Boolean
        Try
            Dim strSQL As String = "delete from faturas_controle where CODIGO_FATURAS_CONTROLE='" & pCodigo & "'"
            Dim connection As New OleDbConnection(strConn)
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


    Public Function GravaLog(ByVal pTipo As String, ByVal pAutor As String, ByVal pfatura As Fatura, ByVal pCodigo As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            'Dim strSQL As String = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values ('" & pCodigoVas & "','" & pCodigoOperadora & "','" & pCodigoLinha & "')"
            Dim strSQL As String = "insert into faturas_controle_log select '" & pTipo & "', fatura,codigo_operadora, codigo_tipo,intervalo_mes,data_inicio,debito_automatico,dia_vencimento,codigo_estado,data_fim,febraban,codigo_fatura_controle where codigo_fatura_controle='" & pCodigo & "'"


            'Dim connection As New Data.OleDb.o
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


    Public Function AtualizaValorIndevido() As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try

            Dim strSQL As String = "AtualizaValorCarregado"

            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandType = CommandType.StoredProcedure
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

    Public Function getCodFornecedorByOperadora(ByVal pOperadora As String) As Integer
        Dim result As Integer = 0
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = ""
        strSQL = "SELECt nvl(p1.codigo,0)codigo from fornecedores p1 "

        If pOperadora > 0 Then
            strSQL = strSQL + " where p1.CODIGO_OPERADORA='" & pOperadora & "'"
        End If


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                result = reader.Item(0).ToString
            End While
        End Using

        Return result

    End Function


    Public Function getArquivoLogSQLByfatura(ByVal pCodigoFatura As Integer, ByVal PCampo As String) As String
        Dim result As String = ""
        Dim connection As New OleDbConnection(strConn)
        Dim arquivoCol As Integer = 0 ' 

        Dim strSQL As String = ""
        strSQL = "select   "

        If PCampo.ToUpper = "DISCART" Then
            strSQL = strSQL + " p1.arquivo_discart "
        ElseIf PCampo.ToUpper = "BAD" Then
            strSQL = strSQL + " p1.arquivo_bad "
        ElseIf PCampo.ToUpper = "LOG" Then
            strSQL = strSQL + " p1.arquivo_log "
        ElseIf PCampo.ToUpper = "CTL" Then
            strSQL = strSQL + " p1.arquivo_ctl "
        End If

        strSQL = strSQL + " from faturas_log_sqlloader p1"

        strSQL = strSQL + " where p1.CODIGO_FATURA='" & pCodigoFatura & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            reader.Read()
            'result = reader.Item(0).ToString

            If reader.HasRows Then
                Try
                    Dim b(reader.GetBytes(arquivoCol, 0, Nothing, 0, Integer.MaxValue) - 1) As Byte
                    reader.GetBytes(arquivoCol, 0, b, 0, b.Length)
                    result = System.Text.Encoding.Default.GetString(b)
                Catch ex As Exception

                End Try

            End If
            reader.Close()
            connection.Close()



        End Using

        Return result

    End Function


    Public Function GetFaturasControleJaCadastrada(ByVal pOperadora As String, ByVal pCodigoCliente As String, ByVal pCodigoEstado As Integer, ByVal pTipo As Integer, ByVal pDiaVenc As Integer, ByVal pCODIGO_FATURAS_CONTROLE As String, ByVal pInicio As String, ByVal pfim As String) As Boolean
        Dim result As Boolean = False
        Dim connection As New OleDbConnection(strConn)


        Try

            Dim strSQL As String = ""
            strSQL = "SELECT p1.CODIGO_FATURAS_CONTROLE, p1.FATURA, p1.CODIGO_OPERADORA,p3.descricao, p1.INTERVALO_MES, p2.TIPO, p1.DATA_INICIO, P1.DATA_FIM,P1.FEBRABAN, p1.codigo_tipo FROM FATURAS_CONTROLE p1, faturas_tipo p2, operadoras_teste p3 where(p1.codigo_tipo = p2.codigo_tipo) and p1.codigo_operadora = p3.codigo"
            strSQL = strSQL + " and p1.CODIGO_OPERADORA='" & pOperadora & "'"
            strSQL = strSQL + " and p1.FATURA='" & pCodigoCliente & "'"
            strSQL = strSQL + " and p1.dia_vencimento='" & pDiaVenc & "'"
            strSQL = strSQL + " and p1.CODIGO_estado='" & pCodigoEstado & "'"
            strSQL = strSQL + " and p1.CODIGO_tipo='" & pTipo & "'"
            strSQL = strSQL + " and p1.data_inicio=to_date('" & pInicio & "','DD/MM/YYYY')"
            If Not String.IsNullOrEmpty(pfim) Then
                strSQL = strSQL + " and p1.data_fim=to_date('" & pfim & "','DD/MM/YYYY')"
            End If

            If Not String.IsNullOrEmpty(pCODIGO_FATURAS_CONTROLE) Then

                strSQL = strSQL + " and p1.CODIGO_FATURAS_CONTROLE<>'" & pCODIGO_FATURAS_CONTROLE & "'"

            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection

                If reader.HasRows Then
                    result = True
                End If
                connection.Close()
            End Using

            Return result

        Catch ex As Exception
            EscreveLog("Erro na aplicação: " & ex.Message)
            EscreveLog("Query: " & sqlLog.ToString)

            connection.Close()
            Return False
        End Try



    End Function


    Public Function GetFaturaByCodigo(ByVal pCodigo As String) As Fatura
        Dim connection As New OleDbConnection(strConn)
        Dim _fatura As New Fatura

        Dim strSQL As String = ""
        'strSQL = "SELECT p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,p1.valor,to_char(p1.dt_vencimento,'dd') dt_vencimento,nvl((select distinct nota_fiscal from cdrs_celular_resumo where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=p1.codigo_fatura )and rownum<2),'-')nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and nvl(p1.periodica,'S')='S' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)"
        strSQL = "SELECT p1.descricao fatura, p1.CODIGO_OPERADORA,'1', p1.CODIGO_TIPO, p1.dt_vencimento data_inicio,p2.descricao operadora,p1.valor,to_char(p1.dt_vencimento,'dd') dt_vencimento,'-' nota_fiscal,p1.codigo_fatura,p1.codigo_tipo,nvl(p3.tipo,'SEM TIPO')tipo, p1.codigo_estado, nvl(e.descricao,'-')estado, nvl(P1.CNPJ_cliente,' ')CNPJ, nvl(P1.nome_cliente,' ')nome_cliente  FROM FATURAS p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)"

        If pCodigo > 0 Then
            strSQL = strSQL + " and codigo_fatura='" & pCodigo & "'"
        End If


        'System.Web.HttpContext.Current.Response.Write(strSQL)
        'System.Web.HttpContext.Current.Response.End()

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _fatura.Fatura = reader.Item("fatura").ToString
                _fatura.CNPJ = reader.Item("CNPJ").ToString
                _fatura.Estado = New Estado(reader.Item("codigo_estado").ToString, reader.Item("estado").ToString)
                _fatura.CodigoTipo = reader.Item("codigo_tipo").ToString
                _fatura.DiaVencimento = reader.Item("dt_vencimento").ToString
                _fatura.NomeCliente = reader.Item("nome_cliente").ToString
            End While
            connection.Close()
        End Using
        Return _fatura


    End Function

    Public Function AtualizaServicos(ByVal _faturas As List(Of Fatura), ByVal pCodigoservico As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = ""
            strSQL = "update faturas set "
            strSQL = strSQL & "codigo_servico='" & pCodigoservico & "' "
            strSQL = strSQL & " where "

            If _faturas.Count > 0 Then
                'filtrou fatura
                strSQL = strSQL + " codigo_fatura  in("
                For Each item As Fatura In _faturas
                    strSQL = strSQL & item.ID & ","
                Next
                strSQL = strSQL.Substring(0, strSQL.Length - 1)
                strSQL = strSQL + " )"
            End If

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

    Public Function AtualizaStatusPgto(ByVal _faturas As List(Of Fatura), ByVal pCodigostatus As String, ByVal pDataPgto As String, ByVal pOP As String, ByVal pdataFinanceiro As String, ByVal pLote As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = ""
            strSQL = "update faturas set "
            strSQL = strSQL & "codigo_status='" & pCodigostatus & "' "
            If Not String.IsNullOrEmpty(pDataPgto) Then
                strSQL = strSQL & ",data_pgto=to_date('" & pDataPgto & "','DD/MM/YYYY') "
            End If
            If Not String.IsNullOrEmpty(pOP) Then
                strSQL = strSQL & ",OP='" & pOP.ToString & "' "
            End If

            If Not String.IsNullOrEmpty(pdataFinanceiro) Then
                strSQL = strSQL & ",data_financeiro=to_date('" & pdataFinanceiro & "','DD/MM/YYYY') "
            End If
            If Not String.IsNullOrEmpty(pLote) Then
                strSQL = strSQL & ",lote='" & pLote.ToString & "' "
            End If

            strSQL = strSQL & " where "

            If _faturas.Count > 0 Then
                'filtrou fatura
                strSQL = strSQL + " codigo_fatura  in("
                For Each item As Fatura In _faturas
                    strSQL = strSQL & item.ID & ","
                Next
                strSQL = strSQL.Substring(0, strSQL.Length - 1)
                strSQL = strSQL + " )"
            End If

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


#Region "NovasFuncionalidade"

    Public Function GeFaturasNaoRecebidasPrazo(ByVal pOperadoras As String, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal pTipo As String, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String, ByVal pPrazo As Integer, Optional ByVal pjustificativa As String = "", Optional pSortName As String = "", Optional pSortDirection As String = "") As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturasControle As New List(Of Fatura)
        Dim listFaturasNaoCadastradas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        'Dim dataAgora As Date = Date.Now.AddYears(-10)
        Dim dataAgora As Date = Date.Now

        'pOperadoras = 12
        If Not String.IsNullOrEmpty(pdataFim) Then
            dataAgora = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim
        End If

        Try
            Dim strSQL As String = ""
            strSQL = "SELECT p1.FATURA, p1.CODIGO_OPERADORA,p1.INTERVALO_MES, p1.CODIGO_TIPO, to_char(p1.DATA_INICIO,'dd/mm/yyyy')DATA_INICIO,p2.descricao operadora,nvl(lpad(p1.dia_vencimento,2,0),1)dia_vencimento, data_fim, nvl(p3.tipo,'SEM TIPO')tipo,p1.codigo_estado, nvl(e.descricao,'-')estado,nvl(p1.CNPJ_CLIENTE,' ')cnpj,p1.codigo_faturas_controle  FROM FATURAS_CONTROLE p1, operadoras_teste p2, faturas_tipo p3, estados e where p1.codigo_operadora=p2.codigo and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado=e.codigo_estado(+)  "
            strSQL = strSQL + "and not exists (select 0 from faturas_avisos fa where fa.codigo_faturas_controle=p1.codigo_faturas_controle and fa.data_aviso>=sysdate) "


            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
                'strSQL = strSQL + " and p1.CODIGO_OPERADORA='" & pOperadoras & "'"
            End If
            If pTipo > 0 Then
                strSQL = strSQL + " and p3.codigo_tipo='" & pTipo & "'"
            End If
            If pCodigoCliente <> "" Then
                strSQL = strSQL + "and p1.fatura like '" & pCodigoCliente & "%'"
            End If

            If pSortName <> "" Then
                If pSortName.ToUpper = "DIAVENCIMENTO" Then
                    pSortName = "dia_vencimento"
                End If
                If pSortName.ToUpper = "DTREFERENCIA" Then
                    pSortName = "DATA_INICIO"
                End If
                If pSortName.ToUpper = "ESTADO.DESCRICAO" Then
                    pSortName = "estado"
                End If
                strSQL = strSQL + "order by " & pSortName & "  " & pSortDirection
            Else
                strSQL = strSQL + "order by p1.codigo_faturas_controle "
            End If



            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _fatura As Fatura = New Fatura(reader.Item(0).ToString, reader.Item(4).ToString, reader.Item(2).ToString, reader.Item(1).ToString, reader.Item("operadora").ToString)
                    _fatura.DiaVencimento = reader.Item("dia_vencimento").ToString
                    _fatura.DataFim = IIf(reader.Item("data_fim").ToString = "", Nothing, reader.Item("data_fim"))
                    _fatura.Tipo = reader.Item("tipo").ToString
                    _fatura.ID = reader.Item("codigo_faturas_controle")
                    Dim _estado As New Estado(reader.Item("codigo_estado").ToString, reader.Item("estado").ToString)
                    _fatura.Estado = _estado
                    _fatura.CNPJ = reader.Item("CNPJ").ToString
                    listFaturasControle.Add(_fatura)
                End While
            End Using
            'System.Web.HttpContext.Current.Response.Write("Passou")
            connection.Close()
            'varre a lista de controle de faturas e verifica quais faturas não foram carregadas
            For Each Fatura As Fatura In listFaturasControle
                Dim myDate As Date = Fatura.DtReferencia
                Dim DatafIm As Date = Fatura.DataFim
                If String.IsNullOrEmpty(Fatura.DataFim) Or Fatura.DataFim.ToShortDateString = "01/01/0001" Or Fatura.DataFim.ToShortDateString = "1/1/0001" Then

                    If Not String.IsNullOrEmpty(pdataFim) Then
                        DatafIm = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2)) & "/" & pdataFim
                    Else
                        'DatafIm = Date.Now
                        DatafIm = DateTime.Now.AddMonths(3)
                    End If

                End If

                While myDate <= dataAgora And myDate <= DatafIm

                    Dim strDiavencimento As String = Fatura.DiaVencimento

                    'se for dia 30 ou dia 31 coloca o ultimo dia do mês
                    If (Fatura.DiaVencimento = "31" Or Fatura.DiaVencimento = "30") And Convert.ToInt32(Fatura.DiaVencimento) > Date.DaysInMonth(myDate.Year, myDate.Month) Then

                        strDiavencimento = Date.DaysInMonth(myDate.Year, myDate.Month)
                        'strDiavencimento = Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))

                    End If

                    'vamos pegar o proximo dia util do mês
                    'myDate = GetProximoDiaUtil(Format(myDate, "dd/MM/yyyy"))
                    'strDiavencimento = Format(myDate, "dd")

                    'strSQL = "Select valor,nvl((select sum(valor_cdr) from cdrs_celular_analitico_mv where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=faturas.codigo_fatura)),0)valor_carregado,to_char(dt_vencimento,'dd')dt_vencimento  from faturas where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "'"
                    'strSQL = "Select p1.codigo_fatura, p1.valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  nvl((select distinct nota_fiscal from cdrs_celular_resumo where codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura=p1.codigo_fatura) and rownum<2),'-')as nota_fiscal,p1.codigo_tipo from faturas p1, faturas_tipo p3 where upper(descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "' and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"
                    strSQL = "Select distinct p1.codigo_fatura, p1.valor,nvl(p1.valor_carregado,0)valor_carregado,nvl(p3.tipo,'SEM TIPO')tipo, to_char(p1.dt_vencimento,'dd')dt_vencimento,  '-' as nota_fiscal,p1.codigo_tipo, nvl(p1.cnpj_cliente,' ')cnpj,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc, nvl(gt.status,'') status_agendamento, nvl(p1.febraban,'N')febraban, nvl(p1.periodica,'N')periodica  "

                    strSQL = strSQL + "from faturas p1, faturas_tipo p3, faturas_status fs, GESTAO_TAREFAS_FATURAS t, gestao_agendamentos_tarefas gt where t.codigo_tarefa=gt.codigo(+) and  t.codigo_fatura(+)=p1.codigo_fatura and p1.codigo_status=fs.codigo_status(+) and upper(p1.descricao)='" & Fatura.Fatura.ToUpper & "' and codigo_operadora='" & Fatura.CodigoOperadora & "' "

                    'strSQL = strSQL + " and to_char(dt_vencimento,'MM')='" & FormataNumero(myDate.Month) & "' and  to_char(dt_vencimento,'YYYY')='" & myDate.Year & "' and  to_char(dt_vencimento,'DD')='" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "'"
                    strSQL += " and (to_char(dt_vencimento,'DD/MM/YYYY')=to_char(next_business_day(to_date('" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "','DD/MM/YYYY')),'DD/MM/YYYY') or to_char(dt_vencimento,'DD/MM/YYYY')= '" & IIf(strDiavencimento.ToString.Length < 2, "0" & strDiavencimento, strDiavencimento) & "/" & FormataNumero(myDate.Month) & "/" & myDate.Year & "')"


                    strSQL = strSQL + " and p1.CODIGO_TIPO=p3.codigo_tipo and p1.codigo_estado='" & Fatura.Estado.Codigo & "'"




                    If pPeriodoCarregada <> "" Then
                        strSQL = strSQL + "and trunc(p1.dt_criacao)>=trunc(sysdate)-" & pPeriodoCarregada & ""
                    End If

                    If pStatusPagamento <> "" Then
                        strSQL = strSQL + "and p1.codigo_status='" & pStatusPagamento & "'"
                    End If

                    If pNotaFiscal <> "" Then
                        strSQL = strSQL + "and p1.nota_fiscal like'" & pNotaFiscal & "%'"
                    End If
                    If pCodigoCliente <> "" Then
                        strSQL = strSQL + "and p1.codigo_cliente like '" & pCodigoCliente & "%'"
                    End If
                    If pIdentContaUnica <> "" Then
                        strSQL = strSQL + "and p1.ident_conta_unica like '" & pIdentContaUnica & "%'"
                    End If
                    If pjustificativa <> "0" And pjustificativa <> Nothing Then
                        strSQL = strSQL + " and p1.JUSTIFICATIVA='" & pjustificativa & "'"
                    End If
                    'If pPrazo > 0 Then
                    '    strSQL = strSQL + " and (p1.dt_vencimento+" & pPrazo.ToString & ")<=sysdate"
                    'End If


                    connection = New OleDbConnection(strConn)
                    connection.Open()
                    Dim cmd2 As OleDbCommand = connection.CreateCommand
                    cmd2.CommandText = strSQL
                    Dim reader2 As OleDbDataReader
                    reader2 = cmd2.ExecuteReader


                    Dim myfaturas As Fatura = New Fatura(Fatura.Fatura, myDate, Fatura.IntevaloMes, Fatura.CodigoOperadora, Fatura.Operadora)
                    'verifica se a fatura foi carregada
                    Using connection

                        myfaturas.Carregada = 1
                        myfaturas.ID = Fatura.ID
                        myfaturas.Valor = Fatura.Valor
                        myfaturas.DiaVencimento = Fatura.DiaVencimento
                        myfaturas.Estado = Fatura.Estado
                        myfaturas.CNPJ = Fatura.CNPJ
                        'If (DatafIm = Nothing Or myDate <= DatafIm) Then
                        If Not reader2.HasRows Then
                            'a fatura não foi carregada
                            myfaturas.Carregada = 2
                            myfaturas.Tipo = Fatura.Tipo
                            'myfaturas.DTVencimento = myDate


                            If String.IsNullOrEmpty(pdataFim) Then
                                ' pdataFim = Month(DatafIm) & "/" & Year(DatafIm)
                            End If

                            If Not String.IsNullOrEmpty(pdataIncio) Then
                                If String.IsNullOrEmpty(pdataFim) Then
                                    pdataFim = pdataIncio
                                End If
                                If pdataFim.Length < 7 Then
                                    pdataFim = "0" & pdataFim

                                End If

                                'If myDate >= New Date(pdataIncio.Substring(3, 4), pdataIncio.Substring(0, 2), 1) And myDate <= New Date(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2), Date.DaysInMonth(pdataFim.Substring(3, 4), pdataFim.Substring(0, 2))) Then

                                listFaturasNaoCadastradas.Add(myfaturas)
                                'End If
                            Else
                                listFaturasNaoCadastradas.Add(myfaturas)
                            End If


                            'End If
                        End If
                    End Using
                    connection.Close()
                    myDate = myDate.AddMonths(Fatura.IntevaloMes)
                    If Fatura.IntevaloMes = 0 Then
                        Exit While
                    End If

                End While
            Next
            'listFaturasNaoCadastradas.Sort(Function(x, y) x.Operadora)

            'listFaturasNaoCadastradas.Sort()

            'If pSortName.ToUpper = "FATURA" Then


            'End If
            'If pSortName.ToUpper = "OPERADORA" Then
            '    listFaturasNaoCadastradas.Sort(Function(x, y) x.Operadora.CompareTo(y.Operadora))
            'End If

           

            cmd.Dispose()
        Catch ex As Exception
            EscreveLog("Erro na aplicação: " & ex.Message)
            connection.Close()
            System.Web.HttpContext.Current.Response.Write("Erro Função GeFaturasNaoRecebidasPrazo:" & ex.Message)
            System.Web.HttpContext.Current.Response.End()
        End Try
        Return listFaturasNaoCadastradas
    End Function


    Private Sub EscreveLog(ByVal pMSG As String)
        Try


            Dim log As IO.StreamWriter
            'Dim caminhoLog As String = Application.StartupPath & ConfigurationManager.AppSettings("nomeArquivo").ToString
            Dim caminhoLog As String = AppDomain.CurrentDomain.BaseDirectory + "logGlobal.txt"
            If Not IO.File.Exists(caminhoLog) Then
                log = IO.File.CreateText(caminhoLog)
                log.WriteLine(Date.Now + "-" + pMSG)
            Else

                log = New IO.StreamWriter(caminhoLog, True, System.Text.Encoding.UTF8)
                log.WriteLine(Date.Now + "-" + pMSG)

            End If
            log.Close()
            log.Dispose()
        Catch ex As Exception

        End Try
    End Sub
#End Region



End Class
