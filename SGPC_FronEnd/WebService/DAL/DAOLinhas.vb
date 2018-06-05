Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOLinhas
    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    'Private _strConn As String = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=server;"
    Private _strConn As String = ""
    Private _dao_commons As New DAO_Commons

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

    Public Function GetFiltroLinhasPerfil(ByVal pSemCom As Boolean, ByVal pCodigoOperadora As Integer, ByVal Classificacao As String, ByVal Codigo_cliente As String, ByVal numero As String, Optional cod_ccusto As String = "", Optional cod_perfil As String = "") As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinhas As New List(Of AppLinhas)

        Dim strSQL As String = "select "
        strSQL = strSQL + "nvl(l.CODIGO_LINHA, 0) as CODIGO_LINHA, nvl(l.NUM_LINHA, '') as NUM_LINHA"
        strSQL = strSQL + ", nvl(m.CODIGO_APARELHO, 0) as CODIGO_APARELHO"
        strSQL = strSQL + ", nvl(o.CODIGO,0) as CODIGO_OPERADORA, nvl(o.DESCRICAO,'') as OPERADORA"
        strSQL = strSQL + ", nvl(u.CODIGO,0) as CODIGO_USUARIO, nvl(u.NOME_USUARIO, '') as NOME_USUARIO"
        strSQL = strSQL + " from LINHAS l, LINHAS_MOVEIS m, USUARIOS u, OPERADORAS_TESTE o "
        If cod_ccusto <> "" Then
            strSQL = strSQL + ", GRUPOS_ITEM gi "
        End If
        If cod_perfil <> "" And cod_perfil <> "0" Then
            strSQL = strSQL + ", PERFIL_LINHA pl "
        End If

        If (pCodigoOperadora < 0) Then
            strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA "
        Else
            'strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and " + Convert.ToString(pCodigoOperadora) + " = m.CODIGO_OPERADORA "
            strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and  l.CODIGO_FORNECEDOR IN(SELECT codigo from FORNECEDORES where CODIGO_OPERADORA= " + Convert.ToString(pCodigoOperadora) + ") "
        End If
        If (Classificacao <> "0" And Classificacao <> "") Then
            'strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and " + Convert.ToString(pCodigoOperadora) + " = m.CODIGO_OPERADORA "
            strSQL = strSQL + " and l.codigo_tipo = '" + Classificacao + "'"
        End If
        If Codigo_cliente <> "" Then
            strSQL = strSQL + " and l.codigo_cliente like '" + Codigo_cliente + "'"
        End If
        If numero <> "" Then
            strSQL = strSQL + " and replace(replace(replace(replace(nvl(l.NUM_LINHA,' '),'(',''),')',''),'-',''), ' ','') like replace(replace(replace(replace(nvl('" + numero + "',' '),'(',''),')',''),'-',''), ' ','')  "
        End If

        strSQL = strSQL + " and  l.CODIGO_OPERADORA = o.CODIGO(+) "
        strSQL = strSQL + " and m.CODIGO_USUARIO = u.CODIGO(+) "
        strSQL = strSQL + " and (l.NUM_LINHA is not null or trim(l.NUM_LINHA) != '') "
        strSQL = strSQL + " and l.STATUS ='1' "
        If cod_ccusto <> "" Then
            strSQL = strSQL + " and l.CODIGO_LINHA = gi.ITEM and gi.modalidade = '4' "
            strSQL = strSQL + " and gi.grupo = '" & cod_ccusto & "' "
        End If

        If cod_perfil <> "" And cod_perfil <> "0" Then
            strSQL = strSQL + " and l.CODIGO_LINHA = pl.codigo_linha "
            strSQL = strSQL + " and pl.codigo_gestaoperfil = '" & cod_perfil & "' "
        Else
            If (pSemCom) Then
                'linhas sem perfil (TRUE)
                strSQL = strSQL + "and l.CODIGO_LINHA not in (select CODIGO_LINHA from PERFIL_LINHA) "
            Else
                'linhas com perfil (FALSE)
                strSQL = strSQL + "and l.CODIGO_LINHA in (select CODIGO_LINHA from PERFIL_LINHA) "
            End If
        End If

        
        strSQL = strSQL + " and l.NUM_LINHA <> '()-'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%0000-00000'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%000)%'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%(00%'"

        strSQL = strSQL + "order by l.NUM_LINHA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA"), reader.Item("NUM_LINHA").ToString, reader.Item("CODIGO_APARELHO"), reader.Item("CODIGO_OPERADORA"), reader.Item("OPERADORA").ToString, reader.Item("CODIGO_USUARIO"), reader.Item("NOME_USUARIO").ToString)
                listLinhas.Add(_registro)
            End While
        End Using

        Return listLinhas
    End Function

    Public Function GetLinhaById_Solicitacao(ByVal pcodigo As Integer) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinha As New List(Of AppLinhas)

        Dim strSQL As String = "select l.CODIGO_LINHA"
        strSQL = strSQL + ",l.NUM_LINHA,nvl(m.CODIGO_APARELHO,0) as CODIGO_APARELHO,nvl(o.CODIGO, 0) as CODIGO_OPERADORA,o.DESCRICAO as OPERADORA"
        strSQL = strSQL + ",nvl(u.CODIGO,0) as CODIGO_USUARIO,u.NOME_USUARIO,u.CIDADE as UNIDADE,u.CARGO_USUARIO as SETOR"
        strSQL = strSQL + ",gp.GESTAO_PERFIL as PERFIL,nvl(pl.FIM_CICLO,0)FIM_CICLO,to_char(nvl(l.VENC_CONTA,'')) as VENC_CONTA,nvl(gp.MINUTOS, 0)MINUTOS, nvl(l.PROTOCOLO_CANCEL, '') as PROTOCOLO_CANCEL  "
        strSQL = strSQL + "from LINHAS l "
        strSQL = strSQL + "inner join LINHAS_MOVEIS m on l.CODIGO_LINHA = m.CODIGO_LINHA "
        strSQL = strSQL + "inner join OPERADORAS_TESTE o on m.CODIGO_OPERADORA = o.CODIGO "
        strSQL = strSQL + "inner join USUARIOS u on m.CODIGO_USUARIO = u.CODIGO "
        strSQL = strSQL + "inner join PERFIL_LINHA pl on l.CODIGO_LINHA = pl.CODIGO_LINHA "
        strSQL = strSQL + "inner join GESTAO_PERFIL gp on pl.CODIGO_GESTAOPERFIL = gp.CODIGO "
        strSQL = strSQL + "where "
        strSQL = strSQL + "(l.NUM_LINHA is not null or trim(l.NUM_LINHA) != '') "
        strSQL = strSQL + "and l.CODIGO_LINHA = " + Convert.ToString(pcodigo)

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString, reader.Item("NUM_LINHA").ToString, reader.Item("CODIGO_APARELHO").ToString, reader.Item("CODIGO_OPERADORA").ToString, reader.Item("OPERADORA").ToString, reader.Item("CODIGO_USUARIO").ToString, reader.Item("NOME_USUARIO").ToString, reader.Item("UNIDADE").ToString(), reader.Item("SETOR").ToString(), reader.Item("PERFIL").ToString(), reader.Item("FIM_CICLO").ToString(), reader.Item("VENC_CONTA").ToString, reader.Item("MINUTOS").ToString(), reader.Item("PROTOCOLO_CANCEL").ToString())
                listLinha.Add(_registro)
            End While
        End Using

        Return listLinha
    End Function

    Public Function GetLinhaById_Fixo(ByVal pcodigo As Integer) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinha As New List(Of AppLinhas)

        Dim strSQL As String = "select l.CODIGO_LINHA"
        strSQL = strSQL + ",l.NUM_LINHA,nvl(o.CODIGO,0) as CODIGO_OPERADORA,o.DESCRICAO as OPERADORA"
        strSQL = strSQL + ",nvl(u.CODIGO,0) as CODIGO_USUARIO,u.NOME_USUARIO,u.CIDADE as UNIDADE,u.CARGO_USUARIO as SETOR"
        strSQL = strSQL + " from LINHAS l "
        strSQL = strSQL + "left join OPERADORAS_TESTE o on l.CODIGO_OPERADORA = o.CODIGO "
        strSQL = strSQL + "left join USUARIOS u on l.CODIGO_USUARIO = u.CODIGO "
        strSQL = strSQL + "where "
        strSQL = strSQL + "(l.NUM_LINHA is not null or trim(l.NUM_LINHA) != '') "
        strSQL = strSQL + "and l.DESATIVADA is null "
        strSQL = strSQL + "and l.CODIGO_LINHA not in (select CODIGO_LINHA from LINHAS_MOVEIS) "
        strSQL = strSQL + "and l.CODIGO_LINHA = " + Convert.ToString(pcodigo)

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString)
                listLinha.Add(_registro)
                listLinha.Item(0).Num_Linha = reader.Item("NUM_LINHA").ToString
                listLinha.Item(0).Codigo_Operadora = reader.Item("CODIGO_OPERADORA").ToString
                listLinha.Item(0).Operadora = reader.Item("OPERADORA").ToString
                listLinha.Item(0).Codigo_Usuario = reader.Item("CODIGO_USUARIO").ToString
                listLinha.Item(0).Nome_Usuario = reader.Item("NOME_USUARIO").ToString
                listLinha.Item(0).Unidade = reader.Item("UNIDADE").ToString()
                listLinha.Item(0).Setor = reader.Item("SETOR").ToString()
            End While
        End Using

        Return listLinha
    End Function


    Public Function GetLinhaFixaByID(ByVal pcodigo As Integer) As AppLinhas
        Dim connection As New OleDbConnection(strConn)


        Dim strSQL As String = "select circuito,codigo_linha,nvl(status,0)status,to_char(ativacao,'DD/MM/YYYY')ativacao,to_char(desativada,'DD/MM/YYYY')desativada,digital,contrato,num_linha,to_char(venc_contr,'DD/MM/YYYY')venc_contr,oem,to_char(venc_conta,'DD/MM/YYYY')venc_conta,conta,internet,nvl(transferencia,'N')transferencia,nvl(fax,'N')fax,codigo_plano,codigo_fornecedor,nvl(codigo_localidade,-1)codigo_localidade,range1,range2,endereco,protocolo,obs,chave_pabx,nvl(codigo_tipo,'0')codigo_tipo,nvl(pontaB,'')pontaB,nvl(codigo_usuario,0)codigo_usuario,codigo_uc,NVL(codigo_cliente,'')codigo_cliente,NVL(protocolo_cancel,'')protocolo_cancel,NVL(CONTA_CONTABIL,'')CONTA_CONTABIL "
        strSQL = strSQL + " from LINHAS l "
        strSQL = strSQL + "where "
        strSQL = strSQL + " l.CODIGO_LINHA = " + Convert.ToString(pcodigo)

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Dim _registro As New AppLinhas
        Using connection
            While reader.Read
                _registro.Codigo_Linha = reader.Item("CODIGO_LINHA").ToString
                _registro.Num_Linha = reader.Item("NUM_LINHA").ToString
                _registro.Codigo_Usuario = reader.Item("CODIGO_USUARIO").ToString
                _registro.Status = reader.Item("STATUS").ToString
                _registro.Ativacao = reader.Item("ATIVACAO").ToString
                _registro.Desativada = reader.Item("DESATIVADA").ToString
                _registro.Digital = reader.Item("DIGITAL").ToString
                _registro.Contrato = reader.Item("CONTRATO").ToString
                _registro.VencContrato = reader.Item("VENC_CONTR").ToString
                _registro.Oem = reader.Item("OEM").ToString
                _registro.VencConta = reader.Item("VENC_CONTA").ToString
                _registro.Conta = reader.Item("CONTA").ToString
                _registro.Internet = reader.Item("INTERNET").ToString
                _registro.Transferencia = reader.Item("TRANSFERENCIA").ToString
                _registro.Fax = reader.Item("FAX").ToString
                _registro.CodigoPlano = reader.Item("CODIGO_PLANO").ToString
                _registro.CodigoFornecedor = reader.Item("CODIGO_FORNECEDOR").ToString
                _registro.CodigoLocalidade = reader.Item("CODIGO_LOCALIDADE").ToString
                _registro.Range1 = reader.Item("range1").ToString
                _registro.Range2 = reader.Item("range2").ToString
                _registro.Endereco = reader.Item("Endereco").ToString
                _registro.Protocolo = reader.Item("Protocolo").ToString
                _registro.OBS = reader.Item("OBS").ToString
                _registro.ChavePabx = reader.Item("Chave_Pabx").ToString
                _registro.CodigoTipo = reader.Item("Codigo_Tipo").ToString
                _registro.PontaB = reader.Item("pontaB").ToString
                _registro.Circuito = reader.Item("circuito").ToString
                _registro.CodigoCliente = reader.Item("codigo_cliente").ToString
                _registro.Local = reader.Item("OEM").ToString
                _registro.Protocolo_Cancel = reader.Item("PROTOCOLO_CANCEL").ToString
                _registro.Conta_cont = reader.Item("CONTA_CONTABIL").ToString
            End While
        End Using
        Return _registro
    End Function


    Public Function GetLinhasTipo() As List(Of AppLinhasTipos)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppLinhasTipos)

        Dim strSQL As String = "select t.codigo_tipo, nvl(t.tipo,'-')tipo from LINHAS_TIPO t "
        strSQL = strSQL + " where upper(t.classificacao)='F' "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                Dim _registro As New AppLinhasTipos(reader.Item("codigo_tipo").ToString, reader.Item("tipo").ToString)
                _list.Add(_registro)

            End While
        End Using

        Return _list
    End Function

    Public Function GetLinhasStatus() As List(Of AppStatusLinha)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppStatusLinha)

        Dim strSQL As String = "select t.codigo_tipo, nvl(t.tipo,' ')descricao from STATUS t "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                Dim _registro As New AppStatusLinha(reader.Item("codigo_tipo").ToString, reader.Item("descricao").ToString)
                _list.Add(_registro)

            End While
        End Using

        Return _list
    End Function

    Public Function InsereLinha(ByVal _registro As AppLinhas) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim cod_linha As Integer = getMaxCodigoLinha()
        Try
            Dim strSQL As String = "insert into LINHAS(CODIGO_LINHA"
            strSQL = strSQL + ", STATUS "
            strSQL = strSQL + ", ATIVACAO "
            strSQL = strSQL + ", DESATIVADA "
            strSQL = strSQL + ", DIGITAL "
            strSQL = strSQL + ", CONTRATO "
            strSQL = strSQL + ", NUM_LINHA "
            strSQL = strSQL + ", VENC_CONTR "
            strSQL = strSQL + ", OEM "
            strSQL = strSQL + ", VENC_CONTA "
            strSQL = strSQL + ", CONTA "
            strSQL = strSQL + ", INTERNET "
            strSQL = strSQL + ", TRANSFERENCIA "
            strSQL = strSQL + ", FAX "
            strSQL = strSQL + ", CODIGO_PLANO "
            strSQL = strSQL + ", CODIGO_FORNECEDOR "
            strSQL = strSQL + ", CODIGO_LOCALIDADE "
            strSQL = strSQL + ", RANGE1 "
            strSQL = strSQL + ", RANGE2 "
            strSQL = strSQL + ", circuito "
            strSQL = strSQL + ", ENDERECO "
            strSQL = strSQL + ", PROTOCOLO "
            strSQL = strSQL + ", OBS "
            strSQL = strSQL + ", CHAVE_PABX "
            strSQL = strSQL + ", CODIGO_TIPO "
            strSQL = strSQL + ", PONTAB "
            strSQL = strSQL + ", CODIGO_USUARIO "
            strSQL = strSQL + ", CODIGO_CLIENTE "
            strSQL = strSQL + ", PROTOCOLO_CANCEL "
            strSQL = strSQL + ", CONTA_CONTABIL "
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ('" & cod_linha & "'"
            strSQL = strSQL + ",'" + _registro.Status.ToString + "'"
            If Not String.IsNullOrEmpty(_registro.Ativacao) Then
                strSQL = strSQL + ",to_date('" + _registro.Ativacao + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            If Not String.IsNullOrEmpty(_registro.Desativada.ToString) Then
                strSQL = strSQL + ",to_date('" + _registro.Desativada + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            strSQL = strSQL + ",'" + _registro.Digital + "'"
            strSQL = strSQL + ",'" + _registro.Contrato + "'"
            strSQL = strSQL + ",'" + _registro.Num_Linha + "'"
            If Not String.IsNullOrEmpty(_registro.VencContrato) Then
                strSQL = strSQL + ",to_date('" + _registro.VencContrato + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            strSQL = strSQL + ",'" + _registro.Local + "'"
            If Not String.IsNullOrEmpty(_registro.VencConta) Then
                strSQL = strSQL + ",to_date('" + _registro.VencConta + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",null"
            End If
            strSQL = strSQL + ",'" + _registro.Conta + "'"
            strSQL = strSQL + ",'" + _registro.Internet + "'"
            strSQL = strSQL + ",'" + _registro.Transferencia + "'"
            strSQL = strSQL + ",'" + _registro.Fax + "'"
            strSQL = strSQL + ",'" + _registro.CodigoPlano.ToString + "'"
            strSQL = strSQL + ",'" + _registro.CodigoFornecedor.ToString + "'"
            strSQL = strSQL + ",'" + _registro.CodigoLocalidade.ToString + "'"
            strSQL = strSQL + ",'" + _registro.Range1.ToString + "'"
            strSQL = strSQL + ",'" + _registro.Range2.ToString + "'"
            strSQL = strSQL + ",'" + _registro.Circuito + "'"
            strSQL = strSQL + ",'" + _registro.Endereco + "'"
            strSQL = strSQL + ",'" + _registro.Protocolo + "'"
            strSQL = strSQL + ",'" + _registro.OBS + "'"
            strSQL = strSQL + ",'" + _registro.ChavePabx + "'"
            strSQL = strSQL + ",'" + _registro.CodigoTipo.ToString + "'"
            strSQL = strSQL + ",'" + _registro.PontaB + "'"
            strSQL = strSQL + ",'" + _registro.Codigo_Usuario.ToString + "'"
            strSQL = strSQL + ",'" + _registro.CodigoCliente.ToString + "'"
            strSQL = strSQL + ",'" + _registro.Protocolo_Cancel.ToString + "'"
            strSQL = strSQL + ",'" + _registro.Conta_cont.ToString + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            'Insere os grupos
            DeleteGrupoLinha(cod_linha)
            For Each _item As AppGrupo In _registro.Grupos
                InsereGrupoLinha(_item.Codigo, cod_linha, _item.Rateio)
            Next

            If _registro.Oem <> "" Then
                InsereChamado(_registro.Oem, cod_linha)
            End If

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
        Return True
    End Function

    Public Function InsereChamado(ByVal pchamado As String, ByVal pCodigoLinha As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = " INSERT INTO CHAMADOS_ITEMS(OEM, CODIGO_ITEM, codigo_tipo) values('" & pchamado & "','" & pCodigoLinha & "','1')"
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

    Public Function VerificaLinhaJaCadastrada(ByVal pLinha As String, ByVal pCodigoLinha As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim _result As Boolean = False
        Dim strSQL As String = "select 0 from linhas l where l.num_linha='" & pLinha.Trim & "' "

        If pCodigoLinha > 0 Then

            strSQL = strSQL + " and l.codigo_linha<>'" & pCodigoLinha & "'"

        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            If reader.HasRows Then
                _result = True
            End If
        End Using
        Return _result
    End Function


    Public Function getMaxCodigoLinha() As Integer
        Dim connection As New OleDbConnection(strConn)
        Dim _result As Integer = 1
        Dim strSQL As String = "select nvl(max(CODIGO_LINHA),0)+1 from linhas"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item(0).ToString
            End While
        End Using
        Return _result
    End Function


    Public Function InsereGrupoLinha(ByVal _grupo As String, ByVal pCodigoLinha As Integer, ByVal pRateio As Double) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into grupos_item (grupo, modalidade, item, rateio) values ('" & _grupo & "', '4', '" & pCodigoLinha & "', '" & pRateio.ToString.Replace(".", "").Replace(",", ".") & "')"

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

    Public Function DeleteGrupoLinha(ByVal pCodigoLinha As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete from grupos_item  where item='" & pCodigoLinha & "'"

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


    Public Function GetNomeUsuarioLinha(ByVal pCodigo As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim nomeUsuario As String = ""
        Dim strSQL As String = "select nome_usuario from usuarios where codigo='" & pCodigo & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                nomeUsuario = reader.Item(0).ToString
            End While
        End Using
        Return nomeUsuario
    End Function


    Public Function AtualizaLinha(ByVal _registro As AppLinhas) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update LINHAS set "
            strSQL = strSQL + " STATUS= '" + _registro.Status.ToString + "'"
            If Not String.IsNullOrEmpty(_registro.Ativacao) Then
                strSQL = strSQL + ",ATIVACAO=to_date('" + _registro.Ativacao + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",ATIVACAO=''"
            End If
            If Not String.IsNullOrEmpty(_registro.Desativada.ToString) Then
                strSQL = strSQL + ",DESATIVADA=to_date('" + _registro.Desativada + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",DESATIVADA=''"
            End If
            strSQL = strSQL + ",DIGITAL='" + _registro.Digital + "'"
            strSQL = strSQL + ",CONTRATO='" + _registro.Contrato + "'"
            strSQL = strSQL + ",NUM_LINHA='" + _registro.Num_Linha + "'"
            If Not String.IsNullOrEmpty(_registro.VencContrato) Then
                strSQL = strSQL + ",VENC_CONTR=to_date('" + _registro.VencContrato + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",VENC_CONTR=''"
            End If
            strSQL = strSQL + ",OEM='" + _registro.Local + "'"
            If Not String.IsNullOrEmpty(_registro.VencConta) Then
                strSQL = strSQL + ",VENC_CONTA=to_date('" + _registro.VencConta + "','dd/mm/yyyy')"
            Else
                strSQL = strSQL + ",VENC_CONTA=''"
            End If
            strSQL = strSQL + ",CONTA='" + _registro.ContratoEmpresa + "'"
            strSQL = strSQL + ",INTERNET='" + _registro.Internet + "'"
            strSQL = strSQL + ",TRANSFERENCIA='" + _registro.Transferencia + "'"
            strSQL = strSQL + ",FAX='" + _registro.Fax + "'"
            strSQL = strSQL + ",CODIGO_PLANO='" + _registro.CodigoPlano.ToString + "'"
            strSQL = strSQL + ",CODIGO_FORNECEDOR='" + _registro.CodigoFornecedor.ToString + "'"
            strSQL = strSQL + ",CODIGO_LOCALIDADE='" + _registro.CodigoLocalidade.ToString + "'"
            strSQL = strSQL + ",RANGE1='" + _registro.Range1.ToString + "'"
            strSQL = strSQL + ",RANGE2='" + _registro.Range2.ToString + "'"
            strSQL = strSQL + ",circuito='" + _registro.Circuito + "'"
            strSQL = strSQL + ",ENDERECO='" + _registro.Endereco + "'"
            strSQL = strSQL + ",PROTOCOLO='" + _registro.Protocolo + "'"
            strSQL = strSQL + ",OBS='" + _registro.OBS + "'"
            strSQL = strSQL + ",CHAVE_PABX='" + _registro.ChavePabx + "'"
            strSQL = strSQL + ",CODIGO_TIPO='" + _registro.CodigoTipo.ToString + "'"
            strSQL = strSQL + ",PONTAB='" + _registro.PontaB + "'"
            strSQL = strSQL + ",CODIGO_USUARIO='" + _registro.Codigo_Usuario.ToString + "'"
            strSQL = strSQL + ",CODIGO_CLIENTE='" + _registro.CodigoCliente.ToString + "'"
            strSQL = strSQL + ",PROTOCOLO_CANCEL='" + _registro.Protocolo_Cancel.ToString + "'"
            strSQL = strSQL + ",CONTA_CONTABIL='" + _registro.Conta_cont.ToString + "'"
            strSQL = strSQL + " where codigo_linha='" & _registro.Codigo_Linha & "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            'Insere os grupos
            DeleteGrupoLinha(_registro.Codigo_Linha)
            For Each _item As AppGrupo In _registro.Grupos
                InsereGrupoLinha(_item.Codigo, _registro.Codigo_Linha, _item.Rateio)
            Next

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
        Return True
    End Function

    Public Function DeleteLinha(ByVal pCodigoLinha As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            DeleteGrupoLinha(pCodigoLinha)

            Dim strSQL As String = "delete from linhas where codigo_linha='" & pCodigoLinha & "'"

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

    Public Sub logalinha(ByVal pcodigoLinha As Integer, ByVal pTipo As String, ByVal pAutor As String)
        Dim connection As New OleDbConnection(strConn)

        _dao_commons.strConn = strConn
        Dim chamado As List(Of AppGeneric) = _dao_commons.GetGenericList("", "p1.OEM", "nvl(p1.abertura, '')", " chamados p1, chamados_items p2 ", "", " and p1.oem = p2.oem and p2.codigo_item='" & pcodigoLinha & "' and p1.tipo_item ='1' order by descricao desc")

        Try
            Dim strSQL As String = ""
            strSQL = "insert into linhas_log (circuito,CODIGO_LINHA,STATUS,ATIVACAO,DESATIVADA,DIGITAL,CONTRATO,NUM_LINHA,VENC_CONTR,OEM,VENC_CONTA,CONTA,INTERNET,TRANSFERENCIA,FAX,CODIGO_PLANO,CODIGO_FORNECEDOR,CODIGO_LOCALIDADE,range1,range2,ENDERECO,PROTOCOLO,OBS,CHAVE_PABX,protocolo_cancel,CONTA_CONTABIL,codigo_grupo, tipo,codigo,data,autor) "
            strSQL = strSQL + " (select * from (select circuito,CODIGO_LINHA,STATUS,ATIVACAO,DESATIVADA,DIGITAL,CONTRATO,NUM_LINHA,VENC_CONTR,'" & IIf(chamado.Count > 0, chamado.Item(0).Codigo, "") & "' as oem,VENC_CONTA,CONTA,INTERNET,TRANSFERENCIA,FAX,CODIGO_PLANO,CODIGO_FORNECEDOR,CODIGO_LOCALIDADE,range1,range2,ENDERECO,PROTOCOLO,OBS,CHAVE_PABX,protocolo_cancel,CONTA_CONTABIL,gi.grupo from linhas l, grupos_item gi where gi.item=l.codigo_linha and l.codigo_linha='" + CStr(pcodigoLinha) + "' and rownum<2) , "
            strSQL = strSQL + "               (select '" + pTipo + "',(select nvl(max(codigo),0)+1 from linhas_log),sysdate,substr('" + pAutor + "',0,20) from dual))"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception
            connection.Close()
        End Try

    End Sub

    Public Function getNumLinhaByCodigo(ByVal pCodigo As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _linha As String = ""
        Dim strSQL As String = "select num_linha from linhas where codigo_linha='" & pCodigo & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _linha = reader.Item(0).ToString
            End While
        End Using
        Return _linha
    End Function

End Class
