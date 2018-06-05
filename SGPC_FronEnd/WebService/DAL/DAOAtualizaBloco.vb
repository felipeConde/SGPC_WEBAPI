Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOAtualizaBloco

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

    Public Function GetLinhas(ByVal pOperadora As Integer, ByVal pCCusto As String, ByVal pMoveis As Boolean, ByVal pFixas As Boolean) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim _listRegistros As New List(Of AppLinhas)

        Dim strSQL As String = "select p1.codigo_linha, p1.num_linha"
        strSQL = strSQL + " from linhas p1 where 1=1 "
        If pOperadora > 0 Then
            strSQL = strSQL + "and p1.codigo_fornecedor in(select codigo from fornecedores where codigo_operadora ='" + Convert.ToString(pOperadora) + "')"
        End If
        If (pMoveis = True And pFixas = False) Then
            strSQL = strSQL + " and p1.codigo_linha in (select codigo_linha from linhas_moveis)"
        End If
        If (pMoveis = False And pFixas = True) Then
            strSQL = strSQL + " and p1.codigo_linha not in (select codigo_linha from linhas_moveis)"
        End If

        If Not String.IsNullOrEmpty(pCCusto) Then
            strSQL = strSQL + " and codigo_linha in(select item from grupos_item where upper(grupo)='" & pCCusto & "')"
        End If


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("codigo_linha").ToString, reader.Item("num_linha").ToString)
                _listRegistros.Add(_registro)
            End While
        End Using

        Return _listRegistros
    End Function


    Public Function AtualizaCCusto(ByVal pitem As String, ByVal pccusto As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try
            Dim strSQL As String = "update grupos_item set grupo='" & pccusto & "' where item ='" & pitem & "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()

            strSQL = "update usuarios set grp_codigo='" & pccusto & "' where codigo in(select codigo_usuario from linhas_moveis where codigo_linha ='" & pitem & "')"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
    End Function

    Public Function AtualizaOBS(ByVal pitem As String, ByVal pobs As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = ""

            If VerificaMovelouFixo(pitem) = True Then
                strSQL = "update linhas_moveis "
            Else
                strSQL = "update linhas "
            End If

            strSQL = strSQL + " set obs='" & pobs & "' where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaChamado(ByVal pitem As String, ByVal pchamado As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        If VerificaMovelouFixo(pitem) = False Then
            Return False
        End If

        Try

            Dim strSQL As String = " insert into chamados_items(OEM, codigo_item, codigo_tipo) values('" & pchamado & "','" & pitem & "','1')"

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

    Public Function AtualizaClassificacao(ByVal pitem As String, ByVal pclassific As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set CODIGO_TIPO='" & pclassific & "' where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaProtocolo(ByVal pitem As String, ByVal pprotocolo As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set protocolo_cancel='" & pprotocolo & "' where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaDtDES(ByVal pitem As String, ByVal dt As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set desativada= to_DATE('" & dt & "','DD/MM/YYYY') where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaDtATV(ByVal pitem As String, ByVal dt As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set ATIVACAO= to_DATE('" & dt & "','DD/MM/YYYY') where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaStatus(ByVal pitem As String, ByVal status As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set status='" & status & "' where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaUsuarioFixos(ByVal pitem As String, ByVal user_code As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas set codigo_usuario='" & user_code & "' where codigo_linha ='" & pitem & "'"

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

    Public Function AtualizaUsuarioMoveis(ByVal pitem As String, ByVal user_code As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        'For Each _item As String In pList
        '    strCodigoLinhas = strCodigoLinhas & "," & _item
        'Next

        'strCodigoLinhas = strCodigoLinhas.Substring(1)

        Try

            Dim strSQL As String = " update linhas_moveis set codigo_usuario='" & user_code & "' where codigo_linha ='" & pitem & "'"

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

    Public Function VerificaMovelouFixo(ByVal pcodigo_linha As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim strCodigoLinhas As String = ""

        Dim strSQL As String = " select codigo_linha from linhas_moveis where codigo_linha ='" & pcodigo_linha & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return True
            End While
        End Using

        Return False

    End Function

    Public Function logaAparelho(ByVal codigo As Integer, ByVal tipo As String, ByVal autorlog As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim sql As String = ""
        Dim sqlCliente As String = ""

        _dao_commons.strConn = strConn

        Dim chamado As List(Of AppGeneric) = _dao_commons.GetGenericList("", "p1.OEM", "nvl(p1.abertura, '')", " chamados p1, chamados_items p2 ", "", " and p1.oem = p2.oem and p2.codigo_item='" & codigo & "' and p1.tipo_item ='1' order by descricao desc")
        Dim num_chamado As String = ""
        If chamado.Count > 0 Then
            num_chamado = chamado.Item(0).Codigo
        End If

        sqlCliente = "(select cliente from codigos_cliente where codigo_cliente="
        sqlCliente = sqlCliente & "(select codigo_cliente from linhas_moveis where codigo_linha='" & codigo & "'))"

        If VerificaMovelouFixo(codigo) = True Then
        
        sql = "insert into linhas_moveis_log (codigo_linha,valor_unit,venc_garan,sim_card,desc_acess,pim,puc,pin2,puk2,pin_aparelho,hexa,status,ativacao,desativado,termo_resp,oem,contrato,nota_fiscal, "
        sql = sql + " venc_conta,cod_conta,num_tel,codigo_plano,codigo_localidade,codigo_grupo,codigo_usuario,codigo_fornecedor,imei,ip,cod_modelo,natu_operacao,servicos, "
        sql = sql + " mensalidade,fleet,codigo_cliente,obs,limite_uso,estoque,backup,sucata,propriedade_estoque,ordem_servico,chamado_retirada,data_retirada,emissao,tipo,codigo,data,autor) "
        sql = sql + " (select * from ("
        sql = sql + " Select l.CODIGO_LINHA, a.VALOR, a.GARANTIA,sim.NUMERO,' '  desc_acess,sim.PIN,sim.PUK,sim.pin2,sim.puk2,a.pin_aparelho, a.HEXA,nvl(trim(l.STATUS),'1') status,l.ATIVACAO,"
            sql = sql + " l.DESATIVADA,t.NUMERO termo, '" & num_chamado & "' as oem ,l.CONTRATO,nvl(lm.NOTA_FISCAL,'') nota,l.VENC_CONTA,l.CONTA,l.NUM_LINHA,l.CODIGO_PLANO,l.CODIGO_LOCALIDADE,"
        sql = sql + " g.CODIGO codigo_grupo,u.CODIGO codigo_usuario,l.CODIGO_FORNECEDOR,a.IMEI,lm.IP,a.COD_MODELO,lm.NATUREZA,' ' servicos,lm.MENSALIDADE"
        sql = sql + " , lm.FLEET," & sqlCliente & ",lm.obs,nvl(l.limite_uso,'')limite_uso,nvl(a.estoque,'N')estoque,nvl(a.backup,'N')backup,nvl(a.sucata,'N')sucata,nvl(a.propriedade_estoque,'')propriedade_estoque,nvl(a.ordem_servico,'')ordem_servico,nvl(a.chamado_retirada,'')chamado_retirada,a.data_retirada,a.emissao "
        'sql=sql+" , lm.FLEET,lm.CODIGO_CLIENTE,lm.obs "
        sql = sql + " from aparelhos_moveis a, linhas l, linhas_moveis lm, sim_cards sim, localidades lo, usuarios u, aparelhos_marcas ma,"
        sql = sql + " aparelhos_modelos mo, fornecedores f, grupos g, grupos_item gi,  termos_responsabilidade t "
        sql = sql + " where"
        sql = sql + " l.CODIGO_LINHA = lm.CODIGO_LINHA and lm.CODIGO_APARELHO = a.CODIGO_APARELHO(+) "
        sql = sql + " and lm.CODIGO_SIM = sim.CODIGO_SIM(+) and l.CODIGO_LOCALIDADE=lo.CODIGO(+) and"
        sql = sql + " a.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = ma.COD_MARCA(+)  and lm.codigo_termo=t.codigo_termo(+) "
        sql = sql + " and l.CODIGO_FORNECEDOR = f.CODIGO(+) and l.codigo_linha=gi.item(+) and nvl(gi.modalidade,'4') ='4' and gi.grupo = g.codigo(+) "
        'sql=sql+" and nvl(trim(l.STATUS),'1') = to_CHAR(s.CODIGO_STATUS) "
        sql = sql + " and lm.codigo_usuario = u.codigo(+)  "
        sql = sql + " and l.codigo_linha='" & codigo & "' and rownum < 2 "
        'sql=sql+" ) "
        'sql=sql+" select l.CODIGO_LINHA, ma.MARCA, mo.MODELO, DECODE(mo.COD_TIPO,'CELULAR','RADIO','OUTROS'), u.NOME_USUARIO,"
        'strSQL = strSQL +"g.NOME_GRUPO,s.DESCRICAO,lo.LOCALIDADE,l.NUM_LINHA,f.NOME_FANTASIA,a.IMEI,nvl(lm.OBS,' ')"

        sql = sql + " ) , "
        sql = sql + "               (select '" & tipo & "' tipo,(select nvl(max(codigo),0)+1 from linhas_moveis_log)codigo_log,sysdate data_atual,'" & autorlog & "' autor from dual))"

        Else
            sql = "insert into linhas_log (circuito,CODIGO_LINHA,STATUS,ATIVACAO,DESATIVADA,DIGITAL,CONTRATO,NUM_LINHA,VENC_CONTR,OEM,VENC_CONTA,CONTA,INTERNET,TRANSFERENCIA,FAX,CODIGO_PLANO,CODIGO_FORNECEDOR,CODIGO_LOCALIDADE,range1,range2,ENDERECO,PROTOCOLO,OBS,CHAVE_PABX,protocolo_cancel,tipo,codigo,data,autor) "
            sql = sql + " (select * from (select circuito,CODIGO_LINHA,STATUS,ATIVACAO,DESATIVADA,DIGITAL,CONTRATO,NUM_LINHA,VENC_CONTR,OEM,VENC_CONTA,CONTA,INTERNET,TRANSFERENCIA,FAX,CODIGO_PLANO,CODIGO_FORNECEDOR,CODIGO_LOCALIDADE,range1,range2,ENDERECO,PROTOCOLO,OBS,CHAVE_PABX,protocolo_cancel from linhas where codigo_linha='" + CStr(codigo) + "') , "
            sql = sql + "               (select '" + tipo + "',(select nvl(max(codigo),0)+1 from linhas_log),sysdate,substr('" + autorlog + "',0,20) from dual))"

        End If

        Try

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
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

End Class
