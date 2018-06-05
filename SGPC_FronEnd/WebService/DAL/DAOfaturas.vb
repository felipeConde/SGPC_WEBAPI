Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

Public Class DAOFaturas

    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Private dao2 As New GestaoDAL

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

    Public Function Inserefatura(ByVal _fatura As Fatura) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = ""
            strSQL = "insert into faturas (codigo_fatura,descricao,codigo_cliente,codigo_operadora,codigo_estado,valor,dt_vencimento, codigo_plano, cnpj_cliente, nome_cliente,ativa,intervalo_mes,codigo_tipo,codigo_fornecedor,periodica,febraban,codigo_status,nota_fiscal,valor_pago,data_pgto,codigo_servico,op,dt_criacao,lote,data_financeiro,obs,justificativa, justificativa_pgto, DT_NOVO_VENCIMENTO) "
            strSQL = strSQL & " values ("
            strSQL = strSQL & "(select nvl(max(codigo_fatura),0)+1 from faturas),"
            strSQL = strSQL & "'" & _fatura.Fatura & "',"
            strSQL = strSQL & "'" & _fatura.Fatura & "',"
            strSQL = strSQL & "(select codigo_operadora from fornecedores where codigo= '" & _fatura.Codigo_Fornecedor & "'),"
            strSQL = strSQL & "'" & _fatura.Estado.Codigo & "',"
            strSQL = strSQL & "to_number('" & Replace(_fatura.Valor, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''),"
            'strSQL = strSQL & "to_date('" & _fatura.DtReferencia.ToShortDateString & "','dd/mm/yyyy'),"
            strSQL = strSQL & "to_date('" & _fatura.DTVencimento.ToShortDateString & "','dd/mm/yyyy'), "
            strSQL = strSQL & "'" & _fatura.Codigo_Plano & "',"
            strSQL = strSQL & "'" & _fatura.CNPJ & "',"
            strSQL = strSQL & "'" & _fatura.NomeCliente & "',"
            strSQL = strSQL & "'S',"
            strSQL = strSQL & "'" & _fatura.IntevaloMes & "',"
            strSQL = strSQL & "'" & _fatura.CodigoTipo & "',"
            strSQL = strSQL & "'" & _fatura.Codigo_Fornecedor & "',"
            strSQL = strSQL & "'N',"
            strSQL = strSQL & "'" & _fatura.Febraban & "',"
            If _fatura.Codigo_Status > 0 Then
                strSQL = strSQL & "'" & _fatura.Codigo_Status & "',"
            Else
                strSQL = strSQL & " null ,"
            End If
            strSQL = strSQL & "'" & _fatura.NotaFiscal & "',"
            strSQL = strSQL & "to_number('" & Replace(_fatura.Valor_Pago, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''),"

            If Not String.IsNullOrEmpty(_fatura.Data_pgto) And IsDate(_fatura.Data_pgto) Then
                strSQL = strSQL & "to_date('" & _fatura.Data_pgto.ToShortDateString & "','dd/mm/yyyy'), "
            Else
                strSQL = strSQL & " null ,"
            End If
            If _fatura.Codigo_Servico > 0 Then
                strSQL = strSQL & "'" & _fatura.Codigo_Servico & "',"
            Else
                strSQL = strSQL & " null ,"
            End If
            strSQL = strSQL & "'" & _fatura.Op & "',"
            strSQL = strSQL & "sysdate, "
            strSQL = strSQL & "'" & _fatura.Lote & "',"
            If Not String.IsNullOrEmpty(_fatura.DT_Financeiro) And IsDate(_fatura.DT_Financeiro) Then
                strSQL = strSQL & "to_date('" & _fatura.DT_Financeiro.ToShortDateString & "','dd/mm/yyyy') ,"
            Else
                strSQL = strSQL & " null, "
            End If
            strSQL = strSQL & "'" & _fatura.OBS & "',"
            strSQL = strSQL & "'" & _fatura.Justificativa & "',"
            If _fatura.Justificativa_PGTO > 0 Then
                strSQL = strSQL & "'" & _fatura.Justificativa_PGTO & "',"
            Else
                strSQL = strSQL & " null, "
            End If
            If Not String.IsNullOrEmpty(_fatura.DT_Novo_Vencimento) And IsDate(_fatura.DT_Novo_Vencimento) Then
                strSQL = strSQL & "to_date('" & _fatura.DT_Novo_Vencimento.ToShortDateString & "','dd/mm/yyyy') "
            Else
                strSQL = strSQL & " null "
            End If
            strSQL = strSQL & ")"

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

    Public Function AtualizaFatura(ByVal _fatura As Fatura) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = ""
            strSQL = "update faturas set "
            strSQL = strSQL & "descricao='" & _fatura.Fatura & "',"
            'strSQL = strSQL & "codigo_cliente='" & _fatura.Fatura & "',"
            strSQL = strSQL & "valor=to_number('" & Replace(_fatura.Valor, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''),"
            'strSQL = strSQL & "dt_referencia=to_date('" & _fatura.DtReferencia & "','dd/mm/yyyy'),"
            strSQL = strSQL & "dt_vencimento=to_date('" & Format(_fatura.DTVencimento, "dd/MM/yyyy") & "','dd/mm/yyyy'),"
            strSQL = strSQL & "codigo_operadora=(select codigo_operadora from fornecedores where codigo= '" & _fatura.Codigo_Fornecedor & "'),"
            strSQL = strSQL & "codigo_estado='" & _fatura.Estado.Codigo & "', "
            strSQL = strSQL & "codigo_plano='" & _fatura.Codigo_Plano & "', "
            strSQL = strSQL & "cnpj_cliente='" & _fatura.CNPJ.ToString & "', "
            strSQL = strSQL & "nome_cliente='" & _fatura.NomeCliente.ToString & "', "
            strSQL = strSQL & "ativa='" & _fatura.Ativa & "', "
            strSQL = strSQL & "intervalo_mes='" & _fatura.IntevaloMes & "', "
            strSQL = strSQL & "codigo_tipo='" & _fatura.CodigoTipo & "', "
            strSQL = strSQL & "codigo_fornecedor='" & _fatura.Codigo_Fornecedor & "', "
            strSQL = strSQL & "febraban='" & _fatura.Febraban & "', "
            strSQL = strSQL & "codigo_status='" & _fatura.Codigo_Status & "', "
            strSQL = strSQL & "valor_pago=to_number('" & Replace(_fatura.Valor_Pago, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''),"
            strSQL = strSQL & "data_pgto=to_date('" & Format(_fatura.Data_pgto, "dd/MM/yyyy") & "','dd/mm/yyyy'),"
            strSQL = strSQL & "nota_fiscal='" & _fatura.NotaFiscal.ToString & "', "
            strSQL = strSQL & "lote='" & _fatura.Lote.ToString & "', "
            If Not String.IsNullOrEmpty(_fatura.DT_Financeiro) Then
                strSQL = strSQL & "data_financeiro=to_date('" & Format(_fatura.DT_Financeiro, "dd/MM/yyyy") & "','dd/mm/yyyy'),"
            End If
            If Not String.IsNullOrEmpty(_fatura.DT_Novo_Vencimento) Then
                strSQL = strSQL & "DT_NOVO_VENCIMENTO=to_date('" & Format(_fatura.DT_Novo_Vencimento, "dd/MM/yyyy") & "','dd/mm/yyyy'),"
            End If
            If _fatura.Codigo_Servico > 0 Then
                strSQL = strSQL & "codigo_servico='" & _fatura.Codigo_Servico & "', "
            End If
            strSQL = strSQL & "op='" & _fatura.Op.ToString & "', "
            strSQL = strSQL & "OBS='" & _fatura.OBS.ToString & "', "
            If _fatura.Justificativa_PGTO > 0 Then
                strSQL = strSQL & "Justificativa_PGTO='" & _fatura.Justificativa_PGTO & "', "
            Else
                strSQL = strSQL & "Justificativa_PGTO=null, "
            End If
            If _fatura.Codigo_Status = 4 Then
                strSQL = strSQL & " data_encaminhamento= case when data_encaminhamento is null then sysdate else data_encaminhamento end, "
            End If
            strSQL = strSQL & "JUSTIFICATIVA='" & _fatura.Justificativa.ToString & "' "
            strSQL = strSQL & " where codigo_fatura='" & _fatura.ID & "'"

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


    ''' <summary>
    ''' Insere a fatura ser excluída no agendamento
    ''' </summary>
    ''' <param name="pcodigo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExcluiFatura(ByVal pcodigo As Integer, ByVal pAutor As String) As Boolean
        Dim _tarefa As New Tarefa

        Try
            _tarefa.Descricao = "Apagar Faturas"
            _tarefa.Codtarefa = 2
            _tarefa.Autor = pAutor

            dao2.StrConn = strConn

            'pega as faturas selecionadas
            Dim _faturas As New List(Of Fatura)
            Dim _fatura As Fatura = GetFaturaById(pcodigo)
            _faturas.Add(_fatura)

            _tarefa.Faturas = _faturas
            Dim msg As String = ""

            msg = dao2.InsereAgendamento(_tarefa, 0)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function GetFaturaById(ByVal pcodigo As Integer) As Fatura
        Dim connection As New OleDbConnection(strConn)
        Dim _registro As New Fatura

        Dim strSQL As String = ""
        strSQL = strSQL + " select "
        strSQL = strSQL + "   p1.codigo_fatura,to_char(p1.dt_referencia,'dd/mm/yyyy')dt_referencia,to_char(p1.dt_vencimento,'dd/mm/yyyy')dt_vencimento,nvl(p1.codigo_fornecedor,0)codigo_fornecedor,"
        strSQL = strSQL + "   p1.codigo_estado,p1.codigo_operadora,nvl(p1.descricao,' ')descricao, nvl(p1.valor,'0')valor,nvl(p1.codigo_plano,'0')codigo_plano,nvl(p1.CNPJ_CLIENTE,' ')CNPJ_CLIENTE,nvl(p1.codigo_tipo,'1')codigo_tipo,nvl(p1.NOME_CLIENTE,' ')NOME_CLIENTE,nvl(p1.ativa,'N')ativa,nvl(p1.febraban,'N')febraban,nvl(p1.pago,'N')pago,nvl(p1.nota_fiscal,' ')nota_fiscal,nvl(p1.INTERVALO_MES,1)INTERVALO_MES, nvl(p2.nome_arquivo,'')nome_arquivo,nvl(p1.codigo_status,'0')codigo_status,nvl(p1.codigo_servico,'0')codigo_servico,nvl(p1.nota_fiscal, ' ') NOTA_FISCAL,nvl(p1.VALOR_PAGO, '0') VALOR_PAGO,nvl(p1.OP, ' ') OP,to_char(p1.DATA_PGTO,'dd/mm/yyyy')DATA_PGTO, nvl(p1.lote,'')lote,to_char(p1.data_financeiro,'dd/mm/yyyy')DATA_FINANCEIRO, nvl(p1.obs,'')OBS, nvl(p1.justificativa, 0) JUSTIFICATIVA, nvl(Justificativa_PGTO,-1)Justificativa_PGTO,to_char(p1.DT_NOVO_VENCIMENTO,'dd/mm/yyyy')DT_NOVO_VENCIMENTO  "
        strSQL = strSQL + " from faturas p1, faturas_arquivos p2"
        strSQL = strSQL + " where p1.codigo_fatura=p2.codigo_fatura(+)"

        If pcodigo > 0 Then
            strSQL = strSQL + " and p1.codigo_fatura = " + Convert.ToString(pcodigo) + ""
        ElseIf pcodigo = 0 Then
            strSQL = strSQL + "order by codigo_fatura"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                '_registro = New Fatura(reader.Item("CODIGO_FATURA").ToString, reader.Item("DESCRICAO").ToString, Nothing, reader.Item("INTERVALO_MES").ToString, reader.Item("CODIGO_OPERADORA").ToString, 0, reader.Item("CODIGO_TIPO").ToString, New Estado(reader.Item("CODIGO_estado").ToString, ""), "", "", 1, reader.Item("valor").ToString, Nothing, "", reader.Item("cnpj_cliente").ToString, reader.Item("nome_cliente").ToString, reader.Item("codigo_fornecedor").ToString, reader.Item("nome_arquivo").ToString, reader.Item("dt_vencimento").ToString, reader.Item("codigo_status").ToString, reader.Item("codigo_servico").ToString, reader.Item("NOTA_FISCAL").ToString, reader.Item("VALOR_PAGO").ToString, reader.Item("OP").ToString, IIf(String.IsNullOrEmpty(reader.Item("DATA_PGTO").ToString), Nothing, reader.Item("DATA_PGTO").ToString))
                _registro = New Fatura(reader.Item("CODIGO_FATURA").ToString, reader.Item("DESCRICAO").ToString, Nothing, reader.Item("INTERVALO_MES").ToString, IIf(String.IsNullOrEmpty(reader.Item("CODIGO_OPERADORA").ToString), 0, reader.Item("CODIGO_OPERADORA").ToString), 0, reader.Item("CODIGO_TIPO").ToString, New Estado(reader.Item("CODIGO_estado").ToString, ""), "", "", 1, reader.Item("valor").ToString, Nothing, "", reader.Item("cnpj_cliente").ToString, reader.Item("nome_cliente").ToString, reader.Item("codigo_fornecedor").ToString, reader.Item("nome_arquivo").ToString, reader.Item("dt_vencimento").ToString, reader.Item("codigo_status").ToString, reader.Item("codigo_servico").ToString, reader.Item("NOTA_FISCAL").ToString, reader.Item("VALOR_PAGO").ToString, reader.Item("OP").ToString, IIf(String.IsNullOrEmpty(reader.Item("DATA_PGTO").ToString), Nothing, reader.Item("DATA_PGTO").ToString), reader.Item("JUSTIFICATIVA"))
                _registro.Lote = reader.Item("LOTE").ToString
                _registro.DT_Financeiro = IIf(String.IsNullOrEmpty(reader.Item("DATA_FINANCEIRO").ToString), Nothing, reader.Item("DATA_FINANCEIRO").ToString)
                _registro.OBS = reader.Item("OBS").ToString
                _registro.Justificativa = reader.Item("JUSTIFICATIVA").ToString
                _registro.Justificativa_PGTO = reader.Item("Justificativa_PGTO").ToString
                _registro.DT_Novo_Vencimento = IIf(String.IsNullOrEmpty(reader.Item("DT_NOVO_VENCIMENTO").ToString), Nothing, reader.Item("DT_NOVO_VENCIMENTO").ToString)

            End While
        End Using

        Return _registro
    End Function

    Public Function GetFaturaGridAnalise(ByVal codigo_operadora As String, ByVal pvencimento As Date) As List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of Fatura)

        Dim strSQL As String = ""
        strSQL = strSQL + " select * from faturas p1"
        strSQL = strSQL + " where p1.dt_vencimento >=to_date('" & pvencimento & "','MM/YYYY')"
        strSQL = strSQL + " and codigo_operadora='" + codigo_operadora + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New Fatura
            End While
        End Using

        Return list
    End Function



    Public Function GetFornecedores() As List(Of AppFornecedor)

        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppFornecedor)
        Dim _registro As New AppFornecedor

        Dim strSQL As String = ""
        strSQL = strSQL + " select codigo, nvl(nome_fantasia,' ')nome_fantasia "
        strSQL = strSQL + " from fornecedores "
        strSQL = strSQL + " order by nome_fantasia"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro = New AppFornecedor(reader.Item("codigo").ToString, reader.Item("nome_fantasia").ToString)
                _lista.Add(_registro)
            End While
        End Using
        Return _lista

    End Function


    Public Function GetFaturasTipo() As List(Of AppFaturasTipo)

        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppFaturasTipo)
        Dim _registro As New AppFaturasTipo

        Dim strSQL As String = ""
        strSQL = strSQL + " select codigo_tipo, nvl(Tipo,' ')Tipo "
        strSQL = strSQL + " from faturas_tipo where codigo_tipo>0  "
        strSQL = strSQL + " order by codigo_tipo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro = New AppFaturasTipo(reader.Item("codigo_tipo").ToString, reader.Item("tipo").ToString)
                _lista.Add(_registro)
            End While
        End Using
        Return _lista

    End Function

    Public Function GetFaturasServicos() As List(Of AppFaturaServico)

        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppFaturaServico)
        Dim _registro As New AppFaturaServico

        Dim strSQL As String = ""
        strSQL = strSQL + " select codigo_servico, nvl(servico_desc,' ')servico_desc "
        strSQL = strSQL + " from faturas_servicos "
        strSQL = strSQL + " order by codigo_servico"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro = New AppFaturaServico(reader.Item("codigo_servico").ToString, reader.Item("servico_desc").ToString)
                _lista.Add(_registro)
            End While
        End Using
        Return _lista

    End Function

    Public Function GetFaturasStatus() As List(Of AppFaturasStatus)

        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppFaturasStatus)
        Dim _registro As New AppFaturasStatus

        Dim strSQL As String = ""
        strSQL = strSQL + " select codigo_status, nvl(status_desc,' ')status_desc "
        strSQL = strSQL + " from faturas_status "
        strSQL = strSQL + " order by codigo_status"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro = New AppFaturasStatus(reader.Item("codigo_status").ToString, reader.Item("status_desc").ToString)
                _lista.Add(_registro)
            End While
        End Using
        Return _lista

    End Function

    Public Function FaturaLOG(ByVal _fatura As Fatura, ByVal pAtutor As String, ByVal pTipo As String) As String

        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            strSQL = "faturalog('" & pAtutor & "','" & pTipo & "','" & _fatura.ID & "')"

            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()
            'Return rowsAffect
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:AtualizaRelatorios Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            'Return -1
            Return "Erro Função:[AtualizaRelatorios]: " & ex.Message
        End Try
    End Function

    Public Function GetFaturasManuais(ByVal pOperadoras As Integer, ByVal pdataIncio As String, ByVal pdataFim As String, ByVal ptipo As Integer, ByVal pPeriodoCarregada As String, ByVal pStatusPagamento As String, ByVal pNotaFiscal As String, ByVal pCodigoCliente As String, ByVal pIdentContaUnica As String) As List(Of Fatura)
        Dim result As Boolean = False
        Dim listFaturas As New List(Of Fatura)
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = ""

            strSQL = "SELECT  p1.codigo_fatura codigo,nvl(p1.descricao,'-')fatura, p1.CODIGO_OPERADORA,nvl(p3.nome_fantasia,'-')operadora, nvl(p2.TIPO,'-')tipo,nvl(p1.valor,0)valor,to_char(p1.dt_vencimento,'DD')dia_vencimento, to_char(p1.dt_vencimento,'dd/MM/YYYY')dt_vencimento,nvl(p1.codigo_tipo,0)codigo_tipo, p1.codigo_estado,nvl(e.descricao,'-')estado,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc FROM FATURAS p1, faturas_tipo p2, fornecedores p3, estados e, faturas_status fs where p1.codigo_status=fs.codigo_status(+) and (p1.codigo_tipo = p2.codigo_tipo) and p1.CODIGO_FORNECEDOR = p3.codigo and p1.PERIODICA='N' and p1.codigo_estado=e.codigo_estado(+)"
            If pOperadoras > 0 Then
                strSQL = strSQL + " and p1.CODIGO_OPERADORA in (select nvl(CODIGO_OPERADORA,-1)CODIGO_OPERADORA from fornecedores where  CODIGO='" & pOperadoras & "')"
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

            connection = New OleDbConnection(strConn)
            connection.Open()
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            Dim reader2 As OleDbDataReader
            reader2 = cmd2.ExecuteReader
            Using connection
                While reader2.Read
                    Dim _novafatura As Fatura = New Fatura(reader2.Item("fatura"), reader2.Item("dt_vencimento"), 1, reader2.Item("CODIGO_OPERADORA"), reader2.Item("operadora"))
                    _novafatura.Carregada = 5
                    _novafatura.Valor = reader2.Item("valor")
                    _novafatura.DiaVencimento = reader2.Item("dia_vencimento")
                    _novafatura.ID = reader2.Item("codigo")
                    _novafatura.CodigoTipo = reader2.Item("codigo_TIPO")
                    _novafatura.Tipo = reader2.Item("TIPO")
                    _novafatura.Codigo_Status = reader2.Item("codigo_status")
                    _novafatura.Status_desc = reader2.Item("status_desc")
                    Dim _estado As New Estado(reader2.Item("codigo_estado").ToString, reader2.Item("estado").ToString)
                    _novafatura.Estado = _estado
                    listFaturas.Add(_novafatura)
                End While
            End Using
            connection.Close()
            cmd2.Dispose()
        Catch ex As Exception
            connection.Close()
        End Try
        Return listFaturas
    End Function


    Public Function VerificaManual(ByVal pCodigo As Integer) As Boolean

        Dim connection As New OleDbConnection(strConn)


        Dim strSQL As String = ""
        strSQL = strSQL + " select 0 from faturas p1 where p1.periodica='N' and p1.codigo_fatura='" & pCodigo & "' "
        strSQL = strSQL + " and p1.codigo_fatura not in (select p2.codigo_fatura from contestacao p2) "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            If reader.HasRows Then
                Return True
            End If
        End Using
        Return False

    End Function

    Public Function VerificaDuplicada(ByVal pCodigo_Fornecedor As Integer, ByVal pTipoFatura As Integer, ByVal pVencimento As Date, ByVal pCodEstado As Integer, ByVal pDescricao As String, ByVal pValor As Double, ByVal pIndentContaUnica As String, Optional ByVal pCodigofatura As Integer = -1) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""
        Dim result As Boolean = False

        Try
            strSQL = "select codigo_fatura, codigo_operadora, descricao from faturas where codigo_operadora=(select codigo_operadora from fornecedores where codigo= '" & pCodigo_Fornecedor & "') and codigo_tipo='" & pTipoFatura & "' and dt_vencimento =to_date('" & pVencimento.Day & "/" & pVencimento.Month & "/" & pVencimento.Year & "','DD/MM/YYYY')  and codigo_estado='" & pCodEstado & "'  "
            If pDescricao <> "" Then
                strSQL = strSQL & " and lower(descricao)= '" & pDescricao.ToLower & "'"
            End If
            strSQL = strSQL & "  and VALOR=" & Replace(CStr(CDbl(pValor)), ",", ".") & ""
            If Not String.IsNullOrEmpty(pIndentContaUnica) Then
                strSQL = strSQL & " and IDENT_CONTA_UNICA='" & pIndentContaUnica & "'"
            End If

            If pCodigofatura > 0 Then
                strSQL = strSQL & " and codigo_fatura<> '" & pCodigofatura & "'"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL

            Dim reader As OleDbDataReader
            Dim listFaturas As New List(Of Fatura)
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                If reader.HasRows Then
                    result = True
                End If
            End Using
            connection.Close()
        Catch ex As Exception
            connection.Close()
        End Try

        Return result

    End Function



End Class
