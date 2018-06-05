Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Contestacoes
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

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

#Region "cadastro"
    Public Function GetContestacoesByID(ByVal pCodigo As Integer) As List(Of Contestacao)
        Dim result As Boolean = False
        Dim connection As New OleDbConnection(strConn)
        Dim listContestacao As New List(Of Contestacao)

        Try
            Dim strSQL As String = "select c.ID,"
            strSQL = strSQL & "c.DATA_CONT,c.COMPETENCIA,c.FATURA,nvl(c.CODIGO_CONTA,-1)CODIGO_CONTA,c.DOCUMENT,c.AUTOR,c.CODIGO_USUARIO,c.CODIGO_TIPO,c.CODIGO_FATURA"
            strSQL = strSQL & ",nvl(c.VALOR_COBRADO,0) as VALOR_COBRADO,nvl(c.VALOR_CONTESTADO,0) as VALOR_CONTESTADO,nvl(c.VALOR_DEVOLVIDO,0) as VALOR_DEVOLVIDO"
            strSQL = strSQL & ",c.STATUS,c.NUMERO_CHAMADO_OPER,c.PROTOCOLO,nvl(c.DATA_ABERTURA,'') as DATA_ABERTURA,nvl(c.DATA_CONCLUSAO,'') as DATA_CONCLUSAO"
            strSQL = strSQL & ",c.NUMERO_PROTOCOLO,c.TIPO_DEVOLUCAO,c.NUM_DEPOSITO,c.NUM_FATURA_CREDITO,nvl(c.FATURA_CREDITO_DATA,'') as FATURA_CREDITO_DATA,nvl(c.REFATURAMENTO_VALOR,0) as REFATURAMENTO_VALOR,c.OUTROS,c.TARIFAS,c.ARQUIVO_URL"
            strSQL = strSQL & ",nvl(f.DT_VENCIMENTO,'') as DT_VENCIMENTO,nvl(f.VALOR,0) as VALOR_FATURA,f.CODIGO_CLIENTE"
            strSQL = strSQL & ",o.CODIGO as CODIGO_OPERADORA,o.DESCRICAO as OPERADORA "
            strSQL = strSQL & "from CONTESTACAO c "
            strSQL = strSQL & "left join FATURAS f on c.CODIGO_FATURA = f.CODIGO_FATURA "
            strSQL = strSQL & "left join OPERADORAS_TESTE o on f.CODIGO_OPERADORA = o.CODIGO "
            strSQL = strSQL & "where c.ID = '" & pCodigo & "'"

            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _Contestacao As New Contestacao(reader.Item("ID").ToString, reader.Item("DATA_CONT").ToString, reader.Item("COMPETENCIA").ToString, reader.Item("FATURA").ToString, reader.Item("CODIGO_CONTA").ToString, reader.Item("AUTOR").ToString, reader.Item("CODIGO_USUARIO").ToString, reader.Item("CODIGO_TIPO").ToString, reader.Item("CODIGO_FATURA").ToString, reader.Item("VALOR_COBRADO").ToString, reader.Item("VALOR_CONTESTADO").ToString, reader.Item("VALOR_DEVOLVIDO").ToString, reader.Item("STATUS").ToString, reader.Item("NUMERO_CHAMADO_OPER").ToString, reader.Item("PROTOCOLO").ToString, IIf(String.IsNullOrEmpty(reader.Item("DATA_ABERTURA").ToString), Nothing, reader.Item("DATA_ABERTURA").ToString), IIf(String.IsNullOrEmpty(reader.Item("DATA_CONCLUSAO").ToString), Nothing, reader.Item("DATA_CONCLUSAO").ToString), reader.Item("NUMERO_PROTOCOLO").ToString, reader.Item("TIPO_DEVOLUCAO").ToString, reader.Item("NUM_DEPOSITO").ToString, reader.Item("NUM_FATURA_CREDITO").ToString, IIf(String.IsNullOrEmpty(reader.Item("FATURA_CREDITO_DATA").ToString), Nothing, reader.Item("FATURA_CREDITO_DATA").ToString), reader.Item("REFATURAMENTO_VALOR").ToString, reader.Item("OUTROS").ToString, reader.Item("TARIFAS").ToString, reader.Item("ARQUIVO_URL").ToString, reader.Item("DT_VENCIMENTO").ToString, reader.Item("VALOR_FATURA").ToString, reader.Item("CODIGO_CLIENTE").ToString, reader.Item("CODIGO_OPERADORA").ToString, reader.Item("OPERADORA").ToString)
                    listContestacao.Add(_Contestacao)
                End While
            End Using
            connection.Close()

        Catch ex As Exception
            connection.Close()

        End Try

        Return listContestacao

    End Function

    Public Function UpdateContestacoesByID(ByVal pCodigo As Integer, ByVal pStatus As Integer, ByVal pNumero_Chamado_Oper As String, ByVal pData_Abertura As Date, ByVal pData_Conclusao As Date, ByVal pValor_Devolvido As Double, ByVal pTipo_Devolucao As String, ByVal pNum_Deposito As String, ByVal pNum_Fatura_Credito As String, ByVal pFatura_Credito_Data As Date, ByVal pRefaturamento_Valor As Double, ByVal pOutros As String) As String

        Try
            Dim strSQL As String = "update CONTESTACAO set "
            strSQL = strSQL & "STATUS = '" & pStatus & "',"
            strSQL = strSQL & "NUMERO_CHAMADO_OPER = '" & pNumero_Chamado_Oper & "',"
            strSQL = strSQL & "DATA_ABERTURA = to_date('" & IIf(pData_Abertura = Nothing, "", pData_Abertura) & "','dd/mm/yyyy'),"
            strSQL = strSQL & "DATA_CONCLUSAO = to_date('" & IIf(pData_Conclusao = Nothing, "", pData_Conclusao) & "','dd/mm/yyyy'),"
            strSQL = strSQL & "VALOR_DEVOLVIDO = replace('" & pValor_Devolvido & "',',','.'),"
            strSQL = strSQL & "TIPO_DEVOLUCAO = '" & pTipo_Devolucao & "',"
            strSQL = strSQL & "NUM_DEPOSITO = '" & pNum_Deposito & "',"
            strSQL = strSQL & "NUM_FATURA_CREDITO = '" & pNum_Fatura_Credito & "',"
            strSQL = strSQL & "FATURA_CREDITO_DATA = to_date('" & IIf(pFatura_Credito_Data = Nothing, "", pFatura_Credito_Data) & "','dd/mm/yyyy'),"
            strSQL = strSQL & "REFATURAMENTO_VALOR = replace('" & pRefaturamento_Valor & "',',','.'),"
            strSQL = strSQL & "OUTROS = '" & pOutros & "' "
            strSQL = strSQL & "where ID = '" & pCodigo & "'"

            Dim connection As New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"

        Catch ex As Exception
            Return ex.Message

        End Try

    End Function

    Public Function DeleteContestacoesByID(ByVal pCodigo As Integer) As String

        Try

            'antes zera na cdrs
            Dim sql As String = " update CDRS_CELULAR t "
            sql += " set t.aprovada=null,"
            sql += " t.valor_devolvido=0"
            sql += " where t.codigo_conta in(select codigo_conta from faturas_arquivos where codigo_fatura in(select c.codigo_fatura from contestacao c where c.id='" & pCodigo & "')) "
           
            Dim connection As New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()

            'deleta
            Dim strSQL As String = "delete from CONTESTACAO "
            strSQL = strSQL & "where ID = '" & pCodigo & "'"

            connection = New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
            cmd = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"

        Catch ex As Exception
            Return ex.Message

        End Try
    End Function

    Public Function InsereLog(ByVal autor As String, ByVal tipo As String, ByVal codigo As Integer) As String

        Try
            Dim sql As String = ""
            sql = "insert into CONTESTACAO_LOG (ID,DATA_CONT,COMPETENCIA,FATURA,CODIGO_CONTA,DOCUMENT,AUTOR,CODIGO_USUARIO,CODIGO_TIPO,CODIGO_FATURA,VALOR_COBRADO,VALOR_CONTESTADO,VALOR_DEVOLVIDO,STATUS,NUMERO_CHAMADO_OPER,PROTOCOLO,DATA_ABERTURA,DATA_CONCLUSAO,NUMERO_PROTOCOLO,TIPO_DEVOLUCAO,NUM_DEPOSITO,NUM_FATURA_CREDITO,FATURA_CREDITO_DATA,REFATURAMENTO_VALOR,OUTROS,TARIFAS,ARQUIVO_URL,DATA_LOG,AUTOR_LOG,TIPO_LOG,CODIGO_LOG) "
            sql = sql + " (select * from (select ID,DATA_CONT,COMPETENCIA,FATURA,CODIGO_CONTA,DOCUMENT,AUTOR,CODIGO_USUARIO,CODIGO_TIPO,CODIGO_FATURA,VALOR_COBRADO,VALOR_CONTESTADO,VALOR_DEVOLVIDO,STATUS,NUMERO_CHAMADO_OPER,PROTOCOLO,DATA_ABERTURA,DATA_CONCLUSAO,NUMERO_PROTOCOLO,TIPO_DEVOLUCAO,NUM_DEPOSITO,NUM_FATURA_CREDITO,FATURA_CREDITO_DATA,REFATURAMENTO_VALOR,OUTROS,TARIFAS,ARQUIVO_URL from contestacao where id='" + CStr(codigo) + "') , "
            sql = sql + "               (select sysdate,substr('" + autor + "',0,20),'" + tipo + "',(select nvl(max(codigo_LOG),0)+1 from CONTESTACAO_LOG) from dual))"

            Dim connection As New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            Return ex.Message
        End Try


    End Function

    Public Function GetContestacoesByFatura(ByVal pCodigo As Integer) As List(Of Contestacao)
        Dim result As Boolean = False
        Dim connection As New OleDbConnection(strConn)
        Dim listContestacao As New List(Of Contestacao)

        Try
            Dim strSQL As String = "select c.ID,"
            strSQL = strSQL & "c.DATA_CONT,c.COMPETENCIA,c.FATURA,nvl(c.CODIGO_CONTA,-1)CODIGO_CONTA,c.DOCUMENT,c.AUTOR,c.CODIGO_USUARIO,c.CODIGO_TIPO,c.CODIGO_FATURA "
            strSQL = strSQL & ",nvl(c.VALOR_COBRADO,0) as VALOR_COBRADO,nvl(c.VALOR_CONTESTADO,0) as VALOR_CONTESTADO,nvl(c.VALOR_DEVOLVIDO,0) as VALOR_DEVOLVIDO "
            strSQL = strSQL & ",c.STATUS,c.NUMERO_CHAMADO_OPER,c.PROTOCOLO,nvl(c.DATA_ABERTURA,'') as DATA_ABERTURA,nvl(c.DATA_CONCLUSAO,'') as DATA_CONCLUSAO "
            strSQL = strSQL & ",c.NUMERO_PROTOCOLO,c.TIPO_DEVOLUCAO,c.NUM_DEPOSITO,c.NUM_FATURA_CREDITO,nvl(c.FATURA_CREDITO_DATA,'') as FATURA_CREDITO_DATA,nvl(c.REFATURAMENTO_VALOR,0) as REFATURAMENTO_VALOR,c.OUTROS,c.TARIFAS,c.ARQUIVO_URL "
            strSQL = strSQL & ",nvl(f.DT_VENCIMENTO,'') as DT_VENCIMENTO,nvl(f.VALOR,0) as VALOR_FATURA,f.CODIGO_CLIENTE"
            strSQL = strSQL & ",o.CODIGO as CODIGO_OPERADORA,o.DESCRICAO as OPERADORA, nvl(cs.descricao,' ')status_desc "
            strSQL = strSQL & "from CONTESTACAO c, FATURAS F, OPERADORAS_TESTE O,CONTESTACAO_STATUS CS "
            strSQL = strSQL & "WHERE c.CODIGO_FATURA = f.CODIGO_FATURA "
            strSQL = strSQL & "AND  f.CODIGO_OPERADORA = o.CODIGO "
            strSQL = strSQL & "AND  C.STATUS = CS.CODIGO "
            strSQL = strSQL & "AND c.codigo_fatura = '" & pCodigo & "'"


            'If Not Me.chkTodas.Checked Then
            'mostra somente a última contestação
            strSQL += " and c.data_cont= "
            strSQL += " (select max(DATA_CONT) keep (dense_rank first order by c.DATA_CONT) "
            strSQL += " from contestacao where codigo_fatura=c.codigo_fatura "
            strSQL += " ) "

            



            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _Contestacao As New Contestacao(reader.Item("ID").ToString, reader.Item("DATA_CONT").ToString, reader.Item("COMPETENCIA").ToString, reader.Item("FATURA").ToString, reader.Item("CODIGO_CONTA").ToString, reader.Item("AUTOR").ToString, reader.Item("CODIGO_USUARIO").ToString, reader.Item("CODIGO_TIPO").ToString, reader.Item("CODIGO_FATURA").ToString, reader.Item("VALOR_COBRADO").ToString, reader.Item("VALOR_CONTESTADO").ToString, reader.Item("VALOR_DEVOLVIDO").ToString, reader.Item("STATUS").ToString, reader.Item("NUMERO_CHAMADO_OPER").ToString, reader.Item("PROTOCOLO").ToString, IIf(String.IsNullOrEmpty(reader.Item("DATA_ABERTURA").ToString), Nothing, reader.Item("DATA_ABERTURA").ToString), IIf(String.IsNullOrEmpty(reader.Item("DATA_CONCLUSAO").ToString), Nothing, reader.Item("DATA_CONCLUSAO").ToString), reader.Item("NUMERO_PROTOCOLO").ToString, reader.Item("TIPO_DEVOLUCAO").ToString, reader.Item("NUM_DEPOSITO").ToString, reader.Item("NUM_FATURA_CREDITO").ToString, IIf(String.IsNullOrEmpty(reader.Item("FATURA_CREDITO_DATA").ToString), Nothing, reader.Item("FATURA_CREDITO_DATA").ToString), reader.Item("REFATURAMENTO_VALOR").ToString, reader.Item("OUTROS").ToString, reader.Item("TARIFAS").ToString, reader.Item("ARQUIVO_URL").ToString, reader.Item("DT_VENCIMENTO").ToString, reader.Item("VALOR_FATURA").ToString, reader.Item("CODIGO_CLIENTE").ToString, reader.Item("CODIGO_OPERADORA").ToString, reader.Item("OPERADORA").ToString)
                    _Contestacao.Status_Desc = reader.Item("status_desc").ToString
                    listContestacao.Add(_Contestacao)
                End While
            End Using
            connection.Close()

        Catch ex As Exception
            connection.Close()

        End Try

        Return listContestacao

    End Function

    Public Function InsereContestacao(ByVal pFatura As Fatura, ByVal pValorContestado As Double, ByVal pAutor As String, ByVal pCodigoUsuario As Integer) As String

        Try
            Dim strSQL As String = "insert into CONTESTACAO (id,codigo_fatura,data_cont,competencia,fatura,codigo_usuario,codigo_tipo,valor_cobrado,valor_contestado) "
            strSQL = strSQL & " values ((select nvl(max(id),0)+1 from CONTESTACAO),'" & pFatura.ID & "',SYSDATE,'" & pFatura.DTVencimento & "','" & pFatura.Fatura & "','" & pCodigoUsuario & "','" & pFatura.CodigoTipo & "',to_number('" & Replace(pFatura.Valor.ToString, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''),to_number('" & Replace(pValorContestado.ToString, ".", ",") & "','9999999999D99','NLS_NUMERIC_CHARACTERS = '',.'''))"


            Dim connection As New OleDbConnection(strConn)
            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"

        Catch ex As Exception
            Return ex.Message

        End Try

    End Function
#End Region

    Public Function GetCSVByID(ByVal pCodigo As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _csv As String = ""
        Dim strSQL As String = ""
        strSQL = strSQL + "select o.arquivo_url "
        strSQL = strSQL + " from contestacao o"
        strSQL = strSQL + " where o.id='" & pCodigo & "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _csv = reader.Item("arquivo_url").ToString
            End While
        End Using

        Return _csv
    End Function

    Public Function getFaturaByContestacao(ByVal pcodigo As Integer) As Fatura

        Dim connection As New OleDbConnection(strConn)
        Dim _csv As String = ""
        Dim strSQL As String = ""
        Try
            strSQL = "SELECT  p1.codigo_fatura codigo,nvl(p1.descricao,'-')fatura, p1.CODIGO_OPERADORA,nvl(p3.nome_fantasia,'-')operadora, nvl(p2.TIPO,'-')tipo,nvl(p1.valor,0)valor,to_char(p1.dt_vencimento,'DD')dia_vencimento, to_char(p1.dt_vencimento,'dd/MM/YYYY')dt_vencimento,nvl(p1.codigo_tipo,0)codigo_tipo, p1.codigo_estado,nvl(e.descricao,'-')estado,nvl(p1.codigo_status,0)codigo_status,nvl(fs.status_desc,' ')status_desc FROM FATURAS p1, faturas_tipo p2, fornecedores p3, estados e, faturas_status fs where p1.codigo_status=fs.codigo_status(+) and (p1.codigo_tipo = p2.codigo_tipo) and p1.CODIGO_FORNECEDOR = p3.codigo and p1.codigo_estado=e.codigo_estado(+) "
            strSQL = strSQL + " and p1.codigo_fatura in (select codigo_fatura from CONTESTACAO where id='" & pcodigo & "') "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Dim _fatura As New Fatura
            Using connection
                While reader.Read
                    _fatura = New Fatura()
                    _fatura.DTVencimento = reader.Item("dt_vencimento").ToString
                    _fatura.Fatura = reader.Item("fatura").ToString
                    _fatura.Operadora = reader.Item("operadora").ToString
                    _fatura.Valor = reader.Item("valor").ToString

                End While
            End Using
            connection.Close()
            Return _fatura
        Catch ex As Exception
            Return Nothing

        Finally
            connection.Close()
        End Try
    End Function

    Public Function getContestacaoLinhas(ByVal pcodigo As Integer, ByVal pAprovado As String) As DataTable

        Dim connection As New OleDbConnection(strConn)
        Dim _csv As String = ""
        Dim strSQL As String = ""
        Try
            strSQL = "select t.codigo,t.linha,nvl(t.tipo_serv2,'-')servico,t.valor_faturado,nvl(t.valor_audit,0)valor_auditado,nvl(t.VALOR_DEVOLVIDO,0)VALOR_DEVOLVIDO,t.codigo_contestacao,decode(t.aprovada,'S','Aprovada','N','Pendente','R','Recusada','Pendente')aprovada from CONTESTACAO_LINHAS t "
            strSQL = strSQL + " where t.codigo_contestacao='" & pcodigo & "'"

            If Not String.IsNullOrEmpty(pAprovado) Then
                strSQL = strSQL + "and upper(t.aprovada) ='" & pAprovado.ToUpper & "'  "
            End If

            strSQL = strSQL + " order by t.linha, t.tipo_serv2 "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Dim _dt As New DataTable
            Using connection
                _dt.Load(reader)
            End Using
            connection.Close()
            Return _dt
        Catch ex As Exception
            Return Nothing

        Finally
            connection.Close()
        End Try
    End Function

    Public Function AtualizaContestacoesLinhas(ByVal pCodigoS As String, ByVal pAprovado As String, ByVal pAutor As String, ByVal pvalor As String) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update CONTESTACAO_LINHAS set "
            strSQL = strSQL + "  APROVADA='" + pAprovado + "'"
            strSQL = strSQL + ", VALOR_DEVOLVIDO='" + pvalor.Replace(",", ".") + "'"
            'strSQL = strSQL + ", VALOR_contestado=valor_faturado - nvl('" + pvalor.Replace(",", ".") + "',0)"
            strSQL = strSQL + ", AUTOR='" + pAutor + "'"
            strSQL = strSQL + " where CODIGO  IN (" + pCodigoS + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


            'faz o update na cdrs_celular
            strSQL = "update cdrs_celular c"
            strSQL += " set  "
            strSQL += "  c.aprovada='" + pAprovado + "' "
            'sql += " where c.codigo in( "
            strSQL += "  where c.codigo_contestacao in  (select distinct t.codigo_contestacao from CONTESTACAO_LINHAS t where t.codigo in(" & pCodigoS & "))"
            strSQL += " and c.tipo_serv2 in (select distinct t.tipo_serv2 from CONTESTACAO_LINHAS t where t.codigo in(" & pCodigoS & "))"
            cmd = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function AtualizaStatusContestacao(ByVal pstatus As String, ByVal pcodigo_contestacao As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update CONTESTACAO p1 set "
            strSQL = strSQL + "  p1.status='" + pstatus + "'"
            'strSQL = strSQL + ", p1.VALOR_contestado=(select nvl(sum(valor_contestado),0) from CONTESTACAO_LINHAS where codigo_contestacao='" & pcodigo_contestacao & "')"
            If pstatus = 2 Then
                strSQL = strSQL + ",p1.VALOR_devolvido=(select nvl(sum(VALOR_DEVOLVIDO),0) from CONTESTACAO_LINHAS where codigo_contestacao='" & pcodigo_contestacao & "' and nvl(aprovada,'N')='S')"
            End If
            strSQL = strSQL + " where p1.id='" & pcodigo_contestacao & "'"
            If pstatus = 2 Then
                strSQL = strSQL + "and not exists (select 0 from CONTESTACAO_LINHAS where codigo_contestacao='" & pcodigo_contestacao & "' and nvl(aprovada,'N')='N')"
            End If
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


    Public Function AtualizaValorPgtoFatura(ByVal pcodigo_contestacao As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update faturas p1 set "
            strSQL = strSQL + "  p1.valor_pago=p1.valor-(select nvl(sum(VALOR_DEVOLVIDO),0)valor_contestado from contestacao_linhas where codigo_contestacao='" & pcodigo_contestacao & "' and upper(aprovada)='S')"
            strSQL = strSQL + " where p1.codigo_fatura =(select codigo_fatura from contestacao where id='" & pcodigo_contestacao & "' and rownum<2)"
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


    Public Function AtualizaContestacoesLinhasByCodigoContestacao(ByVal pCodigo As Integer, ByVal pAprovado As String, ByVal pAutor As String) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update CONTESTACAO_LINHAS set "
            strSQL = strSQL + "  APROVADA='" + pAprovado + "'"
            'strSQL = strSQL + ", VALOR_AUDIT='" + PValorAudit.Replace(",", ".") + "'"
            'strSQL = strSQL + ", VALOR_contestado=valor_faturado - nvl('" + PValorAudit.Replace(",", ".") + "',0)"
            strSQL = strSQL + ", AUTOR='" + pAutor + "'"
            'If pAprovado = "N" Then
            '    strSQL = strSQL + ",p1.VALOR_devolvido=0"
            'End If
            strSQL = strSQL + " where CODIGO_CONTESTACAO='" & pCodigo & "'"
            If pAprovado = "S" Then
                strSQL = strSQL + " and nvl(aprovada,'N')<>'R'"
            End If
            If pAprovado = "N" Then
                strSQL = strSQL + " and not exists (select 0 from CONTESTACAO_LINHAS where codigo_contestacao='" & pCodigo & "' and nvl(aprovada,'N')='N')"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()

            'atualiza na cdrs_celular
            strSQL = "update cdrs_celular set "
            strSQL = strSQL + "  APROVADA='" + pAprovado + "'"
            strSQL = strSQL + ", AUTOR='" + pAutor + "'"
            If pAprovado = "S" Then
                strSQL = strSQL + " ,valor_devolvido=case when valor_devolvido is null then (valor_cdr-valor_audit) else valor_devolvido end"
            End If
            strSQL = strSQL + " where CODIGO_CONTESTACAO='" & pCodigo & "'"
            If pAprovado = "S" Then
                strSQL = strSQL + " and nvl(aprovada,'N')<>'R'"
            End If
            If pAprovado = "N" Then
                strSQL = strSQL + " and not exists (select 0 from CONTESTACAO_LINHAS where codigo_contestacao='" & pCodigo & "' and nvl(aprovada,'N')='N')"
            End If

            cmd = connection.CreateCommand
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


#Region "Retorno da Operadora"

    Public Function InsereContestacaoRetorno(ByVal dt As DataTable, ByVal codigo_contestacao As Integer, ByVal autor As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""

        For Each _row As DataRow In dt.Rows
            Try
                strSQL = "INSERT INTO CONTESTACAO_RETORNO (linha,CNL_ORIGEM,tipo_serv2,CLASSIF_AUDIT,DATA,NUM_CHAMADO,CNL_DESTINO,valor_faturado,controle,valor_audit,valor_contestado,valor_devolvido,codigo_contestacao,codigo,autor) "
                strSQL = strSQL + " values ("

                strSQL = strSQL + "'" & _row.Item("linha").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("CNL_ORIGEM").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("CLASSIF_OPERADORA").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("CLASSIF_AUDITORIA").ToString & "'"
                If IsDate(_row.Item("DATA")) Then
                    strSQL = strSQL + ", to_date('" & FormatDateTime(_row.Item("DATA").ToString, DateFormat.ShortDate) & "','DD/MM/YYYY HH24:MI:SS')"
                Else
                    strSQL = strSQL + ",'' "
                End If

                'strSQL = strSQL + ",'" & _row.Item("DURACAO").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("NUMERO_CHAMADO").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("CNL_DESTINO").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("VALOR_FATURADO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim.Replace(" ", "") & "'"
                strSQL = strSQL + ",'" & _row.Item("CONTROLE").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("VALOR_CONTRATUAL").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim.Replace(" ", "").Replace("-", "0") & "'"
                strSQL = strSQL + ",'" & _row.Item("VALOR_CONTESTADO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim.Replace(" ", "").Replace("-", "0") & "'"
                strSQL = strSQL + ",'" & _row.Item("VALOR_DEVOLVIDO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim.Replace(" ", "").Replace("-", "0") & "'"
                strSQL = strSQL + ",'" & codigo_contestacao & "'"
                strSQL = strSQL + ",(select nvl(max(codigo),0)+1 from CONTESTACAO_RETORNO)"
                strSQL = strSQL + ",'" & autor & "'"
                strSQL = strSQL + " ) "


                Dim cmd As OleDbCommand = connection.CreateCommand
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
                cmd.Dispose()

            Catch ex As Exception

                Dim erro As String = ""
                erro += ",'" & _row.Item("VALOR_FATURADO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim & "'"
                erro += ",'" & _row.Item("CONTROLE").ToString & "'"
                erro += ",'" & _row.Item("VALOR_CONTRATUAL").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim & "'"
                erro += ",'" & _row.Item("VALOR_CONTESTADO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim & "'"
                erro += ",'" & _row.Item("VALOR_DEVOLVIDO").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim & "'"

                connection.Close()
                Return False
            End Try
        Next



        Return True


    End Function

    Public Function DeleteContestacaoRetorno(ByVal codigo_contestacao As Integer) As Boolean

        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = " delete from CONTESTACAO_RETORNO "
            strSQL = strSQL + " where codigo_contestacao='" & codigo_contestacao & "'"

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

    Public Function UpdateContestacaoLinhasByRetorno(ByVal codigo_contestacao As Integer) As Boolean


        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = " update CONTESTACAO_LINHAS p1 "
            strSQL = strSQL + " set APROVADA='S'"
            strSQL = strSQL + " ,p1.valor_devolvido=(SELECT nvl(SUM(t.valor_devolvido),0) from CONTESTACAO_RETORNO t group by t.linha, t.tipo_serv2, t.codigo_contestacao having replace(replace(replace(replace(t.linha,'(',''),')',''),'-',''),' ','')=replace(replace(replace(replace(p1.linha,'(',''),')',''),'-',''),' ','') and t.tipo_serv2=p1.tipo_serv2 and t.codigo_contestacao=p1.codigo_contestacao )"
            strSQL = strSQL + " where p1.codigo_contestacao='" & codigo_contestacao & "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()


            'update na cdrs celular
            strSQL = " update cdrs_celular p1 "
            strSQL = strSQL + " set APROVADA='S'"
            strSQL = strSQL + " ,p1.valor_devolvido=(SELECT nvl(t.valor_devolvido, 0) from CONTESTACAO_RETORNO t where t.codigo_contestacao = p1.codigo_contestacao and to_number(replace(p1.controle,' ',''))=to_number(replace(t.controle,' ','')))"
            strSQL = strSQL + " where p1.codigo_contestacao='" & codigo_contestacao & "'"
            cmd = connection.CreateCommand
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


#End Region


    Public Function InsereCreditoFatura(ByVal dt As DataTable, ByVal codigo_contestacao As Integer, ByVal autor As String) As Boolean

        Dim connection As New OleDbConnection(strConn)

        For Each _row As DataRow In dt.Rows
            Try
                Dim strSQL As String = "INSERT INTO FATURAS_CREDITOS (codigo_contestacao,codigo_fatura,codigo_fatura_credito,servico,valor,codigo_cdrs,AUTOR, data) "
                strSQL = strSQL + " values ("

                strSQL = strSQL + "'" & _row.Item("codigo_contestacao").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("codigo_fatura").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("codigo_fatura_credito").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("servico").ToString & "'"
                strSQL = strSQL + ",'" & _row.Item("valor").ToString.Replace("R$", "").Replace(".", "").Replace(",", ".").Trim.Replace(" ", "") & "'"
                strSQL = strSQL + ",'" & _row.Item("codigo_cdrs").ToString & "'"
                strSQL = strSQL + ",'" & autor & "', sysdate"
                strSQL = strSQL + " ) "


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
        Next



        Return True



    End Function

    Public Function DeleteCreditoFatura(ByVal codigo_contestacao As Integer, ByVal autor As String) As Boolean

        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "DELETE FATURAS_CREDITOS WHERE CODIGO_CONTESTACAO='" & codigo_contestacao & "'"

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
