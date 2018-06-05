Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOSolicitacao
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

    Public Function InsereSolicitacao(ByVal psolicitacao As AppSolicitacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into SOLICITACOES(CODIGO"
            strSQL = strSQL + ",DATA_SOLICITACAO,DIAS_ADITIVO,MINUTOS_EXTRA,PROTOCOLO,ATENDENTE,DESCRICAO,PRAZO,OBSERVACAO,DATA_FECHAMENTO,REQUERENTE_USUARIO_CODIGO,ITEM_CODIGO,SERVICOS_CODIGO,SITUACAO_CODIGO,RELACIONAL_CODIGO) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from SOLICITACOES)"
            strSQL = strSQL + ",to_date('" + Left(psolicitacao.DataSolicitacao.ToString, 10) + "','dd/mm/yyyy')"
            strSQL = strSQL + ",'" + psolicitacao.DiasAditivo.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.MinutosExtras.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.Protocolo.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.Atendente.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.Descricao.ToString + "'"
            If (Not psolicitacao.Prazo = Nothing) Then
                strSQL = strSQL + ",to_date('" + Left(psolicitacao.Prazo.ToString, 10) + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            strSQL = strSQL + ",'" + psolicitacao.Observacao.ToString + "'"
            If (Not psolicitacao.DataFechamento = Nothing) Then
                strSQL = strSQL + ",to_date('" + Left(psolicitacao.DataFechamento.ToString, 10) + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            strSQL = strSQL + ",'" + psolicitacao.RequerenteCodigo.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.ItemCodigo.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.ServicoCodigo.ToString + "'"
            strSQL = strSQL + ",'" + psolicitacao.SituacaoCodigo.ToString + "'"
            If (Not psolicitacao.RelacionalCodigo = Nothing) Then
                strSQL = strSQL + ",'" + psolicitacao.RelacionalCodigo.ToString + "'"
            Else
                strSQL = strSQL + ",null"
            End If
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

    Public Function AtualizaSolicitacao(ByVal psolicitacao As AppSolicitacao) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update SOLICITACOES set "
            strSQL = strSQL + "PROTOCOLO='" + psolicitacao.Protocolo.ToString + "'"
            strSQL = strSQL + ",ATENDENTE='" + psolicitacao.Atendente.ToString + "'"
            strSQL = strSQL + ",DESCRICAO='" + psolicitacao.Descricao.ToString + "'"
            If (Not psolicitacao.Prazo = Nothing) Then
                strSQL = strSQL + ",PRAZO=to_date('" + Left(psolicitacao.Prazo.ToString, 10) + "','dd/mm/yyyy')"
            End If
            strSQL = strSQL + ",OBSERVACAO='" + psolicitacao.Observacao.ToString + "'"
            If (Not psolicitacao.DataFechamento = Nothing) Then
                strSQL = strSQL + ",DATA_FECHAMENTO=to_date('" + Left(psolicitacao.DataFechamento.ToString, 10) + "','dd/mm/yyyy')"
            End If
            strSQL = strSQL + ",SITUACAO_CODIGO='" + psolicitacao.SituacaoCodigo.ToString + "'"
            strSQL = strSQL + " where CODIGO = '" + psolicitacao.Codigo.ToString + "'"

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

    Public Function ExcluiSolicitacao(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete SOLICITACOES "
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

    Public Function GetSolicitacaoById(ByVal pcodigo As Integer) As List(Of AppSolicitacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSolicitacao As New List(Of AppSolicitacao)

        Dim strSQL As String = "select s.CODIGO"
        strSQL = strSQL + ",to_char(s.DATA_SOLICITACAO,'dd/mm/yyyy') as DATA_SOLICITACAO,s.DIAS_ADITIVO,s.MINUTOS_EXTRA,s.PROTOCOLO,s.ATENDENTE,s.DESCRICAO"
        strSQL = strSQL + ",to_char(s.PRAZO,'dd/mm/yyyy') as PRAZO,s.OBSERVACAO,to_char(s.DATA_FECHAMENTO,'dd/mm/yyyy') as DATA_FECHAMENTO"
        strSQL = strSQL + ",s.REQUERENTE_USUARIO_CODIGO,ru.NOME_USUARIO as REQUERENTE"
        strSQL = strSQL + ",s.ITEM_CODIGO,it.ITEM_SGPC as ITEM"
        strSQL = strSQL + ",s.SERVICOS_CODIGO,sv.SERVICO"
        strSQL = strSQL + ",s.SITUACAO_CODIGO,st.SITUACAO,st.DESCRICAO as SITUACAO_DESCRICAO"
        strSQL = strSQL + ",tp.CODIGO as CODIGO_TIPO,tp.SOLICITACAO as TIPO_SOLICITACAO"
        strSQL = strSQL + ",s.RELACIONAL_CODIGO"
        strSQL = strSQL + ",lh.NUM_LINHA,ot.DESCRICAO as OPERADORA,to_char(lh.VENC_CONTA,'dd/mm/yyyy') as VENC_CONTA"
        strSQL = strSQL + ",us.NOME_USUARIO as USUARIO,us.CIDADE as UNIDADE,us.CARGO_USUARIO as SETOR"
        strSQL = strSQL + ",pl.FIM_CICLO,gp.GESTAO_PERFIL as PERFIL"
        strSQL = strSQL + ",gr.NOME_GRUPO,rm.GRP_CODIGO"
        strSQL = strSQL + " from SOLICITACOES s "
        strSQL = strSQL + "inner join USUARIOS ru on s.REQUERENTE_USUARIO_CODIGO = ru.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_ITENS it on s.ITEM_CODIGO = it.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_SERVICOS sv on s.SERVICOS_CODIGO = sv.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_SITUACOES st on s.SITUACAO_CODIGO = st.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_TIPOS tp on sv.TIPO_SOLICITACAO_CODIGO = tp.CODIGO "
        strSQL = strSQL + "left join LINHAS lh on s.RELACIONAL_CODIGO = to_char(lh.CODIGO_LINHA) "
        strSQL = strSQL + "left join LINHAS_MOVEIS lm on lh.CODIGO_LINHA = lm.CODIGO_LINHA and 3 = s.ITEM_CODIGO "
        strSQL = strSQL + "left join OPERADORAS_TESTE ot on lm.CODIGO_OPERADORA = ot.CODIGO or lh.CODIGO_OPERADORA = ot.CODIGO "
        strSQL = strSQL + "left join RAMAIS rm on s.RELACIONAL_CODIGO = rm.NUMERO_A "
        strSQL = strSQL + "left join GRUPOS gr on rm.GRP_CODIGO = gr.CODIGO "
        strSQL = strSQL + "left join USUARIOS us on lm.CODIGO_USUARIO = us.CODIGO or rm.NUMERO_A = us.RML_NUMERO_A or lh.CODIGO_USUARIO = us.CODIGO "
        strSQL = strSQL + "left join PERFIL_LINHA pl on lh.CODIGO_LINHA = pl.CODIGO_LINHA "
        strSQL = strSQL + "left join GESTAO_PERFIL gp on pl.CODIGO_GESTAOPERFIL = gp.CODIGO "

        If pcodigo > 0 Then
            strSQL = strSQL + "where s.CODIGO = " + Convert.ToString(pcodigo)
        Else
            strSQL = strSQL + "order by s.DATA_SOLICITACAO,s.SITUACAO"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSolicitacao(reader.Item("CODIGO").ToString)
                _registro.Atendente = reader.Item("ATENDENTE").ToString
                _registro.DataFechamento = reader.Item("DATA_FECHAMENTO").ToString
                _registro.DataSolicitacao = reader.Item("DATA_SOLICITACAO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.DiasAditivo = reader.Item("DIAS_ADITIVO").ToString
                _registro.FimCiclo = reader.Item("FIM_CICLO").ToString
                _registro.ItemCodigo = reader.Item("ITEM_CODIGO").ToString
                _registro.ItemSGPC = reader.Item("ITEM").ToString
                _registro.MinutosExtras = reader.Item("MINUTOS_EXTRA").ToString
                _registro.Observacao = reader.Item("OBSERVACAO").ToString
                _registro.Operadora = reader.Item("OPERADORA").ToString
                _registro.Perfil = reader.Item("PERFIL").ToString
                _registro.Prazo = reader.Item("PRAZO").ToString
                _registro.Protocolo = reader.Item("PROTOCOLO").ToString
                _registro.Relacional = ""
                _registro.RelacionalCodigo = reader.Item("RELACIONAL_CODIGO").ToString
                _registro.Requerente = reader.Item("REQUERENTE").ToString
                _registro.RequerenteCodigo = reader.Item("REQUERENTE_USUARIO_CODIGO").ToString
                _registro.Servico = reader.Item("SERVICO").ToString
                _registro.ServicoCodigo = reader.Item("SERVICOS_CODIGO").ToString
                _registro.Setor = reader.Item("SETOR").ToString
                _registro.Situacao = reader.Item("SITUACAO").ToString
                _registro.SituacaoCodigo = reader.Item("SITUACAO_CODIGO").ToString
                _registro.SituacaoDescricao = reader.Item("SITUACAO_DESCRICAO").ToString
                _registro.Solicitacao = reader.Item("TIPO_SOLICITACAO").ToString
                _registro.Unidade = reader.Item("UNIDADE").ToString
                _registro.Usuario = reader.Item("USUARIO").ToString
                _registro.VencConta = reader.Item("VENC_CONTA").ToString
                _registro.Grupo = reader.Item("NOME_GRUPO").ToString
                _registro.CCusto = reader.Item("GRP_CODIGO").ToString
                listSolicitacao.Add(_registro)
            End While
        End Using

        Return listSolicitacao
    End Function

    Public Function GetComboServico(ByVal pitem As Boolean) As List(Of AppServicos)
        Dim connection As New OleDbConnection(strConn)
        Dim listServico As New List(Of AppServicos)

        Dim strSQL As String = "select 0 as CODIGO, '...' as SERVICO from dual union "
        strSQL = strSQL + "select CODIGO, SERVICO "
        strSQL = strSQL + "from SOLICITACOES_SERVICOS "
        If pitem Then
            strSQL = strSQL + "where CODIGO > 1 "
        End If
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

    Public Function GetComboSituacao() As List(Of AppSituacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSituacao As New List(Of AppSituacao)

        Dim strSQL As String = "select 0 as CODIGO, '...' as DESCRICAO, '.' as SITUACAO from dual union "
        strSQL = strSQL + "select CODIGO, DESCRICAO, SITUACAO"
        strSQL = strSQL + " from SOLICITACOES_SITUACOES "
        strSQL = strSQL + "order by SITUACAO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSituacao(reader.Item("CODIGO").ToString, reader.Item("SITUACAO").ToString, reader.Item("DESCRICAO").ToString)
                listSituacao.Add(_registro)
            End While
        End Using

        Return listSituacao
    End Function

    Public Function GetComboItem() As List(Of AppItens)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppItens)

        Dim strSQL As String = "select 0 as CODIGO, '...' as ITEM_SGPC from dual union "
        strSQL = strSQL + "select CODIGO, ITEM_SGPC "
        strSQL = strSQL + " from SOLICITACOES_ITENS "
        strSQL = strSQL + "order by ITEM_SGPC"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppItens(reader.Item("CODIGO").ToString, reader.Item("ITEM_SGPC").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetComboLinhas(ByVal plMovelFixo As Boolean) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinhas As New List(Of AppLinhas)

        Dim strSQL As String = "select 0 as CODIGO_LINHA, '...' as NUM_LINHA from dual union "
        strSQL = strSQL + "select CODIGO_LINHA, NUM_LINHA "
        strSQL = strSQL + "from LINHAS "
        strSQL = strSQL + "where (NUM_LINHA is not null or trim(NUM_LINHA) != '')"
        If plMovelFixo Then
            strSQL = strSQL + "and CODIGO_LINHA not in (select CODIGO_LINHA from LINHAS_MOVEIS) "
        End If
        strSQL = strSQL + "order by NUM_LINHA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString, reader.Item("NUM_LINHA").ToString)
                listLinhas.Add(_registro)
            End While
        End Using

        Return listLinhas
    End Function

    Public Function GetComboRequerente() As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listRequerente As New List(Of AppUsuarios)

        Dim strSQL As String = "select 0 as CODIGO, '...' as REQUERENTE from dual union "
        strSQL = strSQL + "select CODIGO, NOME_USUARIO as REQUERENTE "
        strSQL = strSQL + "from USUARIOS "
        strSQL = strSQL + "order by REQUERENTE"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("REQUERENTE").ToString)
                listRequerente.Add(_registro)
            End While
        End Using

        Return listRequerente
    End Function

    Public Function GetComboRamais() As List(Of AppRamais)
        Dim connection As New OleDbConnection(strConn)
        Dim listRamais As New List(Of AppRamais)

        Dim strSQL As String = "select '...' as NUMERO_A from dual union "
        strSQL = strSQL + "select NUMERO_A"
        strSQL = strSQL + " from RAMAIS "
        strSQL = strSQL + "order by NUMERO_A"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRamais(reader.Item("NUMERO_A").ToString)
                listRamais.Add(_registro)
            End While
        End Using

        Return listRamais
    End Function

    Public Function RelComboSituacao() As List(Of AppSituacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSituacao As New List(Of AppSituacao)

        Dim strSQL As String = "select 0 as CODIGO, 'TODOS' as DESCRICAO, '.' as SITUACAO from dual union "
        strSQL = strSQL + "select CODIGO, DESCRICAO, SITUACAO"
        strSQL = strSQL + " from SOLICITACOES_SITUACOES "
        strSQL = strSQL + "where CODIGO > 1 "
        strSQL = strSQL + "order by SITUACAO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSituacao(reader.Item("CODIGO").ToString, reader.Item("SITUACAO").ToString, reader.Item("DESCRICAO").ToString)
                listSituacao.Add(_registro)
            End While
        End Using

        Return listSituacao
    End Function

    Public Function RelComboItem() As List(Of AppItens)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppItens)

        Dim strSQL As String = "select 0 as CODIGO, 'TODOS' as ITEM_SGPC from dual union "
        strSQL = strSQL + "select CODIGO, ITEM_SGPC "
        strSQL = strSQL + " from SOLICITACOES_ITENS "
        strSQL = strSQL + "order by CODIGO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppItens(reader.Item("CODIGO").ToString, reader.Item("ITEM_SGPC").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function RelComboLinhas(ByVal plMovelFixo As Boolean) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinhas As New List(Of AppLinhas)

        Dim strSQL As String = "select 0 as CODIGO_LINHA"
        strSQL = strSQL + IIf(plMovelFixo, ", 'TODAS LINHAS MÓVEIS'", ", 'TODAS LINHAS FIXAS'") + " as NUM_LINHA"
        strSQL = strSQL + " from dual union "
        strSQL = strSQL + "select CODIGO_LINHA, NUM_LINHA"
        strSQL = strSQL + " from LINHAS "
        strSQL = strSQL + "where (NUM_LINHA is not null or trim(NUM_LINHA) != '') and DESATIVADA is null "
        If plMovelFixo Then
            strSQL = strSQL + "and CODIGO_LINHA in (select CODIGO_LINHA from LINHAS_MOVEIS) "
        Else
            strSQL = strSQL + "and CODIGO_LINHA not in (select CODIGO_LINHA from LINHAS_MOVEIS) "
        End If
        strSQL = strSQL + "order by CODIGO_LINHA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString, reader.Item("NUM_LINHA").ToString)
                listLinhas.Add(_registro)
            End While
        End Using

        Return listLinhas
    End Function

    Public Function RelComboRamais() As List(Of AppRamais)
        Dim connection As New OleDbConnection(strConn)
        Dim listRamais As New List(Of AppRamais)

        Dim strSQL As String = "select ' TODOS RAMAIS' as NUMERO_A from dual union "
        strSQL = strSQL + "select NUMERO_A"
        strSQL = strSQL + " from RAMAIS "
        strSQL = strSQL + "order by NUMERO_A"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRamais(reader.Item("NUMERO_A").ToString)
                listRamais.Add(_registro)
            End While
        End Using

        Return listRamais
    End Function

    Public Function RelComboServico() As List(Of AppServicos)
        Dim connection As New OleDbConnection(strConn)
        Dim listServico As New List(Of AppServicos)

        Dim strSQL As String = "select 0 as CODIGO, 'TODOS' as SERVICO from dual union "
        strSQL = strSQL + "select CODIGO, SERVICO "
        strSQL = strSQL + "from SOLICITACOES_SERVICOS "
        strSQL = strSQL + "where CODIGO > 1 "
        strSQL = strSQL + "order by CODIGO"

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

    Public Function RelComboRequerente() As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listRequerente As New List(Of AppUsuarios)

        Dim strSQL As String = "select 0 as CODIGO, 'TODOS' as REQUERENTE from dual union "
        strSQL = strSQL + "select u.CODIGO, u.NOME_USUARIO as REQUERENTE "
        strSQL = strSQL + "from USUARIOS u "
        strSQL = strSQL + "inner join SOLICITACOES s on u.CODIGO = s.REQUERENTE_USUARIO_CODIGO "
        strSQL = strSQL + "order by CODIGO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("REQUERENTE").ToString)
                listRequerente.Add(_registro)
            End While
        End Using

        Return listRequerente
    End Function

    Public Function RelSolicitacao(ByVal pparametro As String) As List(Of AppSolicitacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSolicitacao As New List(Of AppSolicitacao)
        Dim aparametro() As String = pparametro.Split(";")

        Dim vStatus As Integer = aparametro(0)
        Dim vTipoItem As Integer = aparametro(1)
        Dim vItem As Integer = IIf(aparametro(2) = "", 0, aparametro(2))
        Dim vServico As Integer = aparametro(3)
        Dim vRequerente As Integer = aparametro(4)
        Dim vAbertura As String = aparametro(5)
        Dim vFechamento As String = aparametro(6)
        Dim vFormatoData As String = aparametro(7)
        Dim vOrdena As String = aparametro(8)

        Dim strSQL As String = "select s.CODIGO"
        strSQL = strSQL + ", ss.DESCRICAO as Status, ru.NOME_USUARIO as Requerente, nvl(s.PROTOCOLO,' ') as Protocolo, to_char(s.DATA_SOLICITACAO,'dd/mm/yyyy') as AbertoEm, nvl(s.ATENDENTE,' ') as Atendente"
        strSQL = strSQL + ", (case it.CODIGO when 1 then 'SGPC' when 2 then rm.NUMERO_A when 3 then lh.NUM_LINHA when 4 then lh.NUM_LINHA else ' ' end) Item"
        strSQL = strSQL + ", it.ITEM_SGPC as Tipo, nvl(ot.DESCRICAO,' ') as Operadora, nvl(us.NOME_USUARIO,' ') as Usuario, nvl(us.CIDADE,' ') as Unidade, nvl(us.CARGO_USUARIO,' ') as Setor"
        strSQL = strSQL + ", sv.SERVICO, nvl(s.DESCRICAO,' ') as Descricao, nvl(to_char(s.DATA_FECHAMENTO,'dd/mm/yyyy'),' ') as FechadoEm"
        strSQL = strSQL + " from SOLICITACOES s "
        strSQL = strSQL + "inner join SOLICITACOES_SITUACOES ss on s.SITUACAO_CODIGO = ss.CODIGO "
        strSQL = strSQL + "inner join USUARIOS ru on s.REQUERENTE_USUARIO_CODIGO = ru.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_ITENS it on s.ITEM_CODIGO = it.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_SERVICOS sv on s.SERVICOS_CODIGO = sv.CODIGO "
        strSQL = strSQL + "inner join SOLICITACOES_TIPOS tp on sv.TIPO_SOLICITACAO_CODIGO = tp.CODIGO "
        strSQL = strSQL + "left join LINHAS lh on s.RELACIONAL_CODIGO = to_char(lh.CODIGO_LINHA) "
        strSQL = strSQL + "left join LINHAS_MOVEIS lm on lh.CODIGO_LINHA = lm.CODIGO_LINHA "
        strSQL = strSQL + "left join OPERADORAS_TESTE ot on lh.CODIGO_OPERADORA = ot.CODIGO "
        strSQL = strSQL + "left join RAMAIS rm on s.RELACIONAL_CODIGO = rm.NUMERO_A "
        strSQL = strSQL + "left join USUARIOS us on lm.CODIGO_USUARIO = us.CODIGO or rm.NUMERO_A = us.RML_NUMERO_A or lh.CODIGO_USUARIO = us.CODIGO "
        strSQL = strSQL + "where s.SERVICOS_CODIGO <> 1 and s.SITUACAO_CODIGO <> 1 "

        If vStatus > 0 Then
            strSQL = strSQL + "and s.SITUACAO_CODIGO = " + vStatus.ToString + " "
        End If
        If vTipoItem > 0 Then
            strSQL = strSQL + "and s.ITEM_CODIGO = " + vTipoItem.ToString + " "
            If vItem > 0 Then
                strSQL = strSQL + "and s.RELACIONAL_CODIGO = '" + vItem.ToString + "' "
            End If
        End If
        If vServico > 0 Then
            strSQL = strSQL + "and s.SERVICOS_CODIGO = " + vServico.ToString + " "
        End If
        If vRequerente > 0 Then
            strSQL = strSQL + "and s.REQUERENTE_USUARIO_CODIGO = " + vRequerente.ToString + " "
        End If

        If (Not String.IsNullOrEmpty(vAbertura)) Then
            Select Case vFormatoData
                Case "ano"
                    strSQL = strSQL + "and to_char(s.DATA_SOLICITACAO,'yyyy') = '" + vAbertura.ToString + "' "
                Case "mesano"
                    strSQL = strSQL + "and to_char(s.DATA_SOLICITACAO,'mm/yyyy') = '" + vAbertura.ToString + "' "
                Case Else
                    strSQL = strSQL + "and to_char(s.DATA_SOLICITACAO,'dd/mm/yyyy') = '" + vAbertura.ToString + "' "
            End Select
        End If
        If (Not String.IsNullOrEmpty(vFechamento)) Then
            Select Case vFormatoData
                Case "ano"
                    strSQL = strSQL + "and to_char(s.DATA_FECHAMENTO,'yyyy') = '" + vFechamento.ToString + "' "
                Case "mesano"
                    strSQL = strSQL + "and to_char(s.DATA_FECHAMENTO,'mm/yyyy') = '" + vFechamento.ToString + "' "
                Case Else
                    strSQL = strSQL + "and to_char(s.DATA_FECHAMENTO,'dd/mm/yyyy') = '" + vFechamento.ToString + "' "
            End Select
        End If

        strSQL = strSQL + "order by "

        strSQL = strSQL + "ss.DESCRICAO " + vOrdena + ",s.DATA_SOLICITACAO " + vOrdena + " "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppSolicitacao(reader.Item("CODIGO").ToString)
                _registro.SituacaoDescricao = reader.Item("Status").ToString
                _registro.Requerente = reader.Item("Requerente").ToString
                _registro.Protocolo = reader.Item("Protocolo").ToString
                _registro.DataSolicitacao = reader.Item("AbertoEm").ToString
                _registro.Atendente = reader.Item("Atendente").ToString
                _registro.Relacional = reader.Item("Item").ToString
                _registro.ItemSGPC = reader.Item("Tipo").ToString
                _registro.Operadora = reader.Item("Operadora").ToString
                _registro.Usuario = reader.Item("Usuario").ToString
                _registro.Unidade = reader.Item("Unidade").ToString
                _registro.Setor = reader.Item("Setor").ToString
                _registro.Servico = reader.Item("SERVICO").ToString
                _registro.Descricao = reader.Item("Descricao").ToString
                _registro.DataFechamento = reader.Item("FechadoEm").ToString

                listSolicitacao.Add(_registro)
            End While
        End Using

        Return listSolicitacao
    End Function

    Public Function RelComboOperadora() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadora As New List(Of AppOperadoras)

        Dim strSQL As String = "select -1 as CODIGO, 'TODAS' as OPERADORA from dual union "
        strSQL = strSQL + "select CODIGO, DESCRICAO as OPERADORA "
        strSQL = strSQL + "from OPERADORAS_TESTE "
        strSQL = strSQL + "order by CODIGO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadora.Add(_registro)
            End While
        End Using

        Return listOperadora
    End Function

    Public Function RelComboUnidade() As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listUnidade As New List(Of AppUsuarios)

        Dim strSQL As String = "select 0 as CODIGO_CIDADE, 'TODAS' as MUNICIPIO from dual union "
        strSQL = strSQL + "select distinct u.CODIGO_CIDADE, '('||c.UF||') '||c.MUNICIPIO as MUNICIPIO "
        strSQL = strSQL + "from USUARIOS u inner join CIDADES c on u.CODIGO_CIDADE = c.CODIGO_CIDADE "
        strSQL = strSQL + "order by CODIGO_CIDADE"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbAbort, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbAbort, vbNull, vbNull, vbNull, vbNull, vbNull, vbAbort, vbNull, reader.Item("CODIGO_CIDADE").ToString, reader.Item("MUNICIPIO").ToString, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull, vbNull)
                listUnidade.Add(_registro)
            End While
        End Using

        Return listUnidade
    End Function

    Public Function RelComboPerfil() As List(Of AppGestaoPerfil)
        Dim connection As New OleDbConnection(strConn)
        Dim listGestaoPerfil As New List(Of AppGestaoPerfil)

        Dim strSQL As String = "select 0 as CODIGO, 'TODOS' as GESTAO_PERFIL from dual union "
        strSQL = strSQL + "select CODIGO, GESTAO_PERFIL "
        strSQL = strSQL + "from GESTAO_PERFIL "
        strSQL = strSQL + "order by CODIGO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString)
                listGestaoPerfil.Add(_registro)
            End While
        End Using

        Return listGestaoPerfil
    End Function

    Public Function RelAditivo(ByVal ptipo As Integer, ByVal pfiltro As String) As List(Of AppSolicitacao)
        Dim connection As New OleDbConnection(strConn)
        Dim listSolicitacao As New List(Of AppSolicitacao)
        Dim aparametro() As String = pfiltro.Split(";")

        Dim strSQL As String = ""
        Select Case ptipo
            Case 1  'Controle
                Dim vOperadora As Integer = aparametro(0)
                Dim vUnidade As Integer = aparametro(1)
                Dim vPerfil As Integer = aparametro(2)
                Dim vPeriodo As String = aparametro(3)

                strSQL = strSQL + "select s.CODIGO"
                strSQL = strSQL + ", lh.NUM_LINHA as Linha, nvl(ot.DESCRICAO,' ') as Operadora"
                strSQL = strSQL + ", nvl(us.NOME_USUARIO,' ') as Usuario, nvl(us.CIDADE,' ') as Unidade, nvl(us.CARGO_USUARIO,' ') as Setor"
                strSQL = strSQL + ", gp.GESTAO_PERFIL as PerfilFixo, to_char(s.DATA_SOLICITACAO,'dd/mm/yyyy') as EfetivadoEm"
                strSQL = strSQL + ", (case when to_number(to_char(s.DATA_SOLICITACAO,'dd')) > to_number(to_char(pl.FIM_CICLO))"
                strSQL = strSQL + " then (pl.FIM_CICLO||'/'||to_char(add_months(s.DATA_SOLICITACAO,1),'mm/yyyy'))"
                strSQL = strSQL + " else (pl.FIM_CICLO||'/'||to_char(s.DATA_SOLICITACAO,'mm/yyyy'))"
                strSQL = strSQL + " end) FimCiclo"
                strSQL = strSQL + ", nvl(to_char(lh.VENC_CONTA,'dd/mm/yyyy'),' ') as Vencimento, s.DIAS_ADITIVO as Dias, s.MINUTOS_EXTRA as Extra"
                strSQL = strSQL + " from SOLICITACOES s "
                strSQL = strSQL + "inner join SOLICITACOES_ITENS it on s.ITEM_CODIGO = it.CODIGO "
                strSQL = strSQL + "inner join SOLICITACOES_SERVICOS sv on s.SERVICOS_CODIGO = sv.CODIGO "
                strSQL = strSQL + "inner join SOLICITACOES_SITUACOES ss on s.SITUACAO_CODIGO = ss.CODIGO "
                strSQL = strSQL + "inner join SOLICITACOES_TIPOS tp on sv.TIPO_SOLICITACAO_CODIGO = tp.CODIGO "
                strSQL = strSQL + "inner join LINHAS lh on s.RELACIONAL_CODIGO = to_char(lh.CODIGO_LINHA) "
                strSQL = strSQL + "inner join LINHAS_MOVEIS lm on lh.CODIGO_LINHA = lm.CODIGO_LINHA "
                strSQL = strSQL + "inner join OPERADORAS_TESTE ot on lm.CODIGO_OPERADORA = ot.CODIGO "
                strSQL = strSQL + "inner join USUARIOS us on lm.CODIGO_USUARIO = us.CODIGO "
                strSQL = strSQL + "inner join PERFIL_LINHA pl on lm.CODIGO_LINHA = pl.CODIGO_LINHA "
                strSQL = strSQL + "inner join GESTAO_PERFIL gp on pl.CODIGO_GESTAOPERFIL = gp.CODIGO "
                strSQL = strSQL + "where (s.SERVICOS_CODIGO = 1 And s.SITUACAO_CODIGO = 1) "

                If vOperadora > 0 Then
                    strSQL = strSQL + "and ot.CODIGO = " + vOperadora.ToString + " "
                End If
                If vUnidade > 0 Then
                    strSQL = strSQL + "and us.CODIGO_CIDADE = " + vUnidade.ToString + " "
                End If
                If vPerfil > 0 Then
                    strSQL = strSQL + "and gp.CODIGO = " + vPerfil.ToString + " "
                End If

                If (Not String.IsNullOrEmpty(vPeriodo)) Then
                    strSQL = strSQL + "and (to_char(s.DATA_SOLICITACAO,'mm/yyyy') = '" + vPeriodo.ToString + "' "
                    strSQL = strSQL + "or to_char(s.DATA_SOLICITACAO,'yyyy') = '" + vPeriodo.ToString + "') "
                End If

                strSQL = strSQL + "order by s.DATA_SOLICITACAO"

                Dim cmd As OleDbCommand = connection.CreateCommand
                cmd.CommandText = strSQL
                Dim reader As OleDbDataReader
                connection.Open()
                reader = cmd.ExecuteReader
                Using connection
                    While reader.Read
                        Dim _registro As New AppSolicitacao(reader.Item("CODIGO").ToString)
                        _registro.Relacional = reader.Item("Linha").ToString
                        _registro.Operadora = reader.Item("Operadora").ToString
                        _registro.Usuario = reader.Item("Usuario").ToString
                        _registro.Unidade = reader.Item("Unidade").ToString
                        _registro.Setor = reader.Item("Setor").ToString
                        _registro.Perfil = reader.Item("PerfilFixo").ToString
                        _registro.DataSolicitacao = reader.Item("EfetivadoEm").ToString
                        _registro.FimCiclo = reader.Item("FimCiclo").ToString
                        _registro.VencConta = reader.Item("Vencimento").ToString
                        _registro.DiasAditivo = reader.Item("Dias")
                        _registro.MinutosExtras = reader.Item("Extra")

                        listSolicitacao.Add(_registro)
                    End While
                End Using

            Case 2  'Painel
                Dim vInicio As String = aparametro(0)
                Dim vFim As String = aparametro(1)

                strSQL = strSQL + "select vl.UNIDADE, nvl(vl.CIDADE, '')CIDADE,"
                strSQL = strSQL + " nvl(vl.TOTALMINUTOS, 0)TOTALMINUTOS,"
                strSQL = strSQL + " nvl(vl.ADITIVO, 0)ADITIVO,"
                strSQL = strSQL + " nvl(vc.MESANO, '')MESANO,"
                strSQL = strSQL + " nvl(vc.TOTALCREDITO, 0)TOTALCREDITO,"
                strSQL = strSQL + " round(nvl(vc.PERCENTUAL, 0),2)PERCENTUAL,"
                strSQL = strSQL + " nvl((case when vc.PERCENTUAL > 10 then 'V' else 'F' end), '') MaiorDez"
                strSQL = strSQL + " from CONTROLE_ADITIVO_CREDITO_VW vc "
                strSQL = strSQL + "inner join LIMITE_ADITIVO_CREDITO_VW vl on vc.CIDADE = vl.CIDADE(+) "
                strSQL = strSQL + "where "
                If (vInicio <> vFim) Then
                    strSQL = strSQL + "to_date('01/'||vc.MESANO,'dd/MM/yyyy') between to_date(" + vInicio.ToString + ",'dd/MM/yyyy') and to_date(" + vFim.ToString + ",'dd/MM/yyyy')"
                Else
                    strSQL = strSQL + "substr(vc.MESANO,4,4) = '" + Right(vInicio.ToString, 4) + "'"
                End If
                strSQL = strSQL + " order by vl.CIDADE, to_date('01/'||vc.MESANO)"

                Dim cmd As OleDbCommand = connection.CreateCommand
                cmd.CommandText = strSQL
                Dim reader As OleDbDataReader
                connection.Open()
                reader = cmd.ExecuteReader
                Using connection
                    While reader.Read
                        Dim _registro As New AppSolicitacao()
                        _registro.Cidade = reader.Item("CIDADE").ToString
                        _registro.Unidade = reader.Item("UNIDADE").ToString
                        _registro.TotalMinutos = reader.Item("TOTALMINUTOS").ToString
                        _registro.Aditivo = reader.Item("ADITIVO").ToString
                        _registro.MesAno = reader.Item("MESANO").ToString
                        _registro.Extra = reader.Item("TOTALCREDITO").ToString
                        _registro.PercMes = reader.Item("PERCENTUAL").ToString
                        _registro.MaiorDez = reader.Item("MaiorDez").ToString

                        listSolicitacao.Add(_registro)
                    End While
                End Using

        End Select

        Return listSolicitacao
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
