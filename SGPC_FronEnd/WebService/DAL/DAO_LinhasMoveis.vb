Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_LinhasMoveis
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

    Public Function GetLinhaMovelById(ByVal pcodigo As Integer) As List(Of AppLinhasMoveis)
        Dim connection As New OleDbConnection(strConn)
        Dim listMovel As New List(Of AppLinhasMoveis)

        Dim strSQL As String = "select CODIGO_LINHA "
        strSQL = strSQL + " FROM APARELHOS_MOVEIS "
        strSQL = strSQL + "where CODIGO_APARELHO = " + Convert.ToString(pcodigo) + ""


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhasMoveis(reader.Item("CODIGO_LINHA").ToString)
                listMovel.Add(_registro)
            End While
        End Using

        Return listMovel
    End Function

    Public Function GetLinhaCodeByNumber(ByVal number As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO_LINHA "
        strSQL = strSQL + " from linhas l "
        strSQL = strSQL + "where replace(replace(replace(replace(l.num_linha, '(', ''), ')', ''), '-', ''),' ','') ="
        strSQL = strSQL + " replace(replace(replace(replace('" & number & "', '(', ''), ')', ''), '-', ''),' ','')"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("CODIGO_LINHA").ToString
            End While
        End Using

        Return ""
    End Function

    Public Sub GetStatusMoveis(ByRef status_code As List(Of String), ByRef listStatus As List(Of String))
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO_STATUS, DESCRICAO"
        strSQL = strSQL + " from STATUS_LINHAS"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("CODIGO_STATUS").ToString)
                Dim _registro2 As New String(reader.Item("DESCRICAO").ToString)
                status_code.Add(_registro)
                listStatus.Add(_registro2)
            End While
        End Using

        Return
    End Sub

    Public Sub GetStatusFixos(ByRef status_code As List(Of String), ByRef listStatus As List(Of String))
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO_TIPO, TIPO"
        strSQL = strSQL + " from STATUS"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("CODIGO_TIPO").ToString)
                Dim _registro2 As New String(reader.Item("TIPO").ToString)
                status_code.Add(_registro)
                listStatus.Add(_registro2)
            End While
        End Using

        Return
    End Sub

    Public Function GetComboModelos(ByVal cod_marca As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim marcas As New List(Of AppGeneric)

        Dim strSQL As String = "select COD_MODELO, MODELO as DESCRICAO"
        strSQL = strSQL + " from APARELHOS_MODELOS"
        strSQL = strSQL + " where COD_MARCA='" + cod_marca + "' order by descricao"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("COD_MODELO").ToString, reader.Item("DESCRICAO").ToString)
                marcas.Add(_registro)
            End While
        End Using

        Return marcas
    End Function

    Public Function GetMovelById(ByVal pcodigo As Integer) As List(Of AppAparelhosMoveis)
        Dim connection As New OleDbConnection(strConn)
        Dim listMovel As New List(Of AppAparelhosMoveis)

        Dim strSQL As String = "select li.CODIGO_LINHA, nvl(ap.CODIGO_APARELHO, '') CODIGO_APARELHO"
        strSQL = strSQL + ", nvl(ap.VALOR, '') AS VALOR"
        strSQL = strSQL + ", nvl(ap.NATUREZA, '') AS NATUREZA"
        strSQL = strSQL + ", nvl(ap.IMEI, '') AS IMEI"
        strSQL = strSQL + ", nvl(ap.HEXA, '') AS HEXA"
        'strSQL = strSQL + ", nvl(ap.GARANTIA, '') AS GARANTIA"
        strSQL = strSQL + ", nvl(ap.COD_MODELO, '') AS COD_MODELO"
        strSQL = strSQL + ", nvl(aa.COD_MARCA, '') AS COD_MARCA"
        strSQL = strSQL + ", nvl(ap.NOTA_FISCAL, '') AS NOTA_FISCAL"
        strSQL = strSQL + ", nvl(ap.PIN_APARELHO, '') AS PIN_APARELHO"
        strSQL = strSQL + ", nvl(ap.ESTOQUE, 'N') AS ESTOQUE"
        strSQL = strSQL + ", nvl(ap.BACKUP, 'N') AS BACKUP"
        strSQL = strSQL + ", nvl(ap.SUCATA, 'N') AS SUCATA"
        strSQL = strSQL + ", nvl(ap.PROPRIEDADE_ESTOQUE, '') AS PROPRIEDADE"
        strSQL = strSQL + ", nvl(ap.ORDEM_SERVICO, '') AS ORDEM_SERVICO"
        strSQL = strSQL + ", nvl(ap.CHAMADO_RETIRADA, '') AS CHAMADO"
        strSQL = strSQL + ", to_char(ap.DATA_RETIRADA, 'DD/MM/YYYY') AS DATA_RET"
        strSQL = strSQL + ", to_char(ap.EMISSAO, 'DD/MM/YYYY') AS EMISSAO"
        strSQL = strSQL + ", nvl(ap.PERDIDO, 'N') AS PERDIDO"
        strSQL = strSQL + ", to_char(ap.GARANTIA, 'DD/MM/YYYY') AS GARANTIA"
        strSQL = strSQL + ", to_char(lm.FIM_COMODATO, 'DD/MM/YYYY') AS FIM_COMODATO"
        strSQL = strSQL + ", nvl(li.OEM, '') AS OEM"
        strSQL = strSQL + ", nvl(li.CODIGO_TIPO, 0) AS CLASSIFICACAO"
        strSQL = strSQL + ", nvl(li.LIMITE_USO, '') AS LIMITE_USO"
        strSQL = strSQL + ", nvl(lm.CODIGO_CLIENTE, '') AS CODIGO_CLIENTE"
        strSQL = strSQL + ", nvl(lm.CODIGO_TECNOLOGIA, '') AS CODIGO_TECNOLOGIA"
        strSQL = strSQL + ", to_char(li.ATIVACAO, 'DD/MM/YYYY') AS ATIVACAO"
        strSQL = strSQL + ", to_char(li.DESATIVADA, 'DD/MM/YYYY') AS DESATIVACAO"
        strSQL = strSQL + ", nvl(li.NUM_LINHA, '') AS TELEFONE"
        strSQL = strSQL + ", nvl(li.STATUS, '') AS STATUS"
        strSQL = strSQL + ", nvl(li.CODIGO_FORNECEDOR, 0) AS CODIGO_FORNECEDOR"
        strSQL = strSQL + ", nvl(li.CODIGO_PLANO, 0) AS CODIGO_PLANO"
        strSQL = strSQL + ", nvl(li.INTRAGRUPO, 'N') AS INTRAGRUPO"
        strSQL = strSQL + ", nvl(li.CODIGO_LOCALIDADE, '') AS CODIGO_LOCALIDADE"
        strSQL = strSQL + ", nvl(lm.IP, '') AS IP"
        strSQL = strSQL + ", nvl(lm.OBS, '') AS OBS"
        strSQL = strSQL + ", nvl(lm.FLEET, '') AS FLEET"
        strSQL = strSQL + ", nvl(lm.CODIGO_USUARIO, 0) CODIGO_USUARIO"
        strSQL = strSQL + ", nvl(lm.CODIGO_TERMO, 0) CODIGO_TERMO"
        strSQL = strSQL + ", nvl(ap.IMEI,' ') IMEI"
        strSQL = strSQL + ", nvl(s.PIN,' ') PIN1"
        strSQL = strSQL + ", nvl(s.PIN2,' ') PIN2"
        strSQL = strSQL + ", nvl(s.PUK,' ') PUK1"
        strSQL = strSQL + ", nvl(s.PUK2,' ') PUK2"
        strSQL = strSQL + ", nvl(s.numero,' ') SIMCARD"
        strSQL = strSQL + ", nvl(s.valor, 0) VALOR_SIMCARD"
        strSQL = strSQL + ", nvl(Li.PROTOCOLO_CANCEL, '') PROTOCOLO_CANCEL"
        strSQL = strSQL + ", nvl(Li.CONTRATO, '') CONTRATO"
        strSQL = strSQL + ", nvl(ap.SERIAL_NUMBER, '') SERIAL_NUMBER"
        strSQL = strSQL + ", nvl(li.CONTA_CONTABIL, '') CONTA_CONTABIL"

        strSQL = strSQL + " FROM APARELHOS_MOVEIS ap, LINHAS li, LINHAS_MOVEIS lm, sim_cards s, APARELHOS_MODELOS mo, APARELHOS_MARCAS aa "
        strSQL = strSQL + " where li.CODIGO_LINHA = " + Convert.ToString(pcodigo) + ""
        strSQL = strSQL + " and li.codigo_linha = lm.codigo_linha "
        strSQL = strSQL + " and ap.codigo_aparelho(+) = lm.codigo_aparelho "
        strSQL = strSQL + " and lm.CODIGO_SIM = s.CODIGO_SIM(+)"
        strSQL = strSQL + " and ap.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = aa.COD_MARCA(+) "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppAparelhosMoveis

                _registro.Codigo = reader.Item("CODIGO_LINHA").ToString
                _registro.Codigo_aparelho = reader.Item("CODIGO_APARELHO").ToString
                _registro.Valor_aparelho = reader.Item("VALOR").ToString
                _registro.Natureza = reader.Item("NATUREZA").ToString
                _registro.Identificacao = reader.Item("IMEI").ToString
                '_registro.Identificacao = reader.Item("HEXA").ToString
                _registro.Pin_Aparelho = reader.Item("PIN_APARELHO").ToString()
                _registro.Venc_garantia = reader.Item("GARANTIA").ToString
                _registro.Modelo = reader.Item("COD_MODELO").ToString
                _registro.Marca = reader.Item("COD_MARCA").ToString
                _registro.Estoque = reader.Item("ESTOQUE").ToString()
                _registro.Backup = reader.Item("BACKUP").ToString()
                _registro.Sucata = reader.Item("SUCATA").ToString()
                _registro.Prop_estoque = reader.Item("PROPRIEDADE").ToString()
                _registro.Ordem_serv = reader.Item("ORDEM_SERVICO").ToString()
                _registro.Chamado = reader.Item("CHAMADO").ToString()
                _registro.Data_retirada = reader.Item("DATA_RET").ToString()
                _registro.Emissao = reader.Item("EMISSAO").ToString()
                _registro.Perdido = reader.Item("PERDIDO").ToString()
                _registro.Chamado = reader.Item("OEM").ToString()
                _registro.Telefone = reader.Item("TELEFONE").ToString()
                _registro.Ip = reader.Item("IP").ToString()
                _registro.Fleet = reader.Item("FLEET").ToString()
                _registro.Identificacao = reader.Item("IMEI").ToString()
                _registro.Simcard = reader.Item("SIMCARD").ToString()
                _registro.Simcard_value = reader.Item("VALOR_SIMCARD").ToString()
                _registro.Nota_fiscal = reader.Item("NOTA_FISCAL").ToString()
                _registro.Desativacao = reader.Item("DESATIVACAO").ToString()
                _registro.Ativacao = reader.Item("ATIVACAO").ToString()
                _registro.Codigo_cliente = reader.Item("CODIGO_CLIENTE").ToString()
                _registro.Tecnologia = reader.Item("CODIGO_TECNOLOGIA").ToString()
                _registro.Venc_comodato = reader.Item("FIM_COMODATO").ToString()
                _registro.Venc_garantia = reader.Item("GARANTIA").ToString()
                _registro.Limite_uso = reader.Item("LIMITE_USO").ToString()
                _registro.Obs = reader.Item("OBS").ToString()
                _registro.Pin1 = reader.Item("PIN1").ToString()
                _registro.Pin2 = reader.Item("PIN2").ToString()
                _registro.Puk1 = reader.Item("PUK1").ToString()
                _registro.Puk2 = reader.Item("PUK2").ToString()
                _registro.Usuario = reader.Item("CODIGO_USUARIO").ToString()
                _registro.Term_resp = reader.Item("CODIGO_TERMO").ToString()
                _registro.Operadora = reader.Item("CODIGO_FORNECEDOR").ToString()
                _registro.Plano = reader.Item("CODIGO_PLANO").ToString()
                _registro.Intragrupo = reader.Item("INTRAGRUPO").ToString()
                _registro.Status = reader.Item("STATUS").ToString()
                _registro.Classificacao = reader.Item("CLASSIFICACAO").ToString()
                _registro.Protocolo_cancel = reader.Item("PROTOCOLO_CANCEL").ToString()
                _registro.Contrato = reader.Item("CONTRATO").ToString()
                _registro.Serial_Number = reader.Item("SERIAL_NUMBER").ToString()
                _registro.Sucursal = reader.Item("CODIGO_LOCALIDADE").ToString()
                _registro.Conta_cont = reader.Item("CONTA_CONTABIL").ToString()

                listMovel.Add(_registro)
            End While
        End Using

        Return listMovel
    End Function

    Public Function GetClassificacao() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select CODIGO_TIPO, TIPO from LINHAS_TIPO where CLASSIFICACAO='M'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO_TIPO").ToString, reader.Item("TIPO").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function InsereLinhaMovel(ByVal _linha_movel As AppAparelhosMoveis, ByVal codigoOperadora As String, ByVal list_facilidades As String(), ByVal list_projetos As String(), ByVal list_ccusto As String(), ByVal autorlog As String, ByRef msg As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""
        Dim codigo_linha As String = ""
        Dim codigoSim As String = ""
        Dim codigoAparelho As String = ""

        Try

            If _dao_commons.Is_Reader_HasRows("SELECT * FROM LINHAS where num_linha='" & _linha_movel.Telefone & "' and num_linha <> '()-'", strConn) = True Then
                msg = "Já existe uma outra linha com o mesmo número!"

                Return False
            End If

            If _linha_movel.Simcard <> "" Then
                If _dao_commons.Is_Reader_HasRows("SELECT codigo_sim FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' ", strConn) = True Then
                    'existe simcard associado a telefone celular
                    msg = "Número de simcard já cadastrado para outro telefone!"

                    Return False
                End If
            End If
            If _linha_movel.Marca <> 0 Then
                If _dao_commons.Is_Reader_HasRows("SELECT * FROM aparelhos_moveis where imei='" + _linha_movel.Identificacao + "'", strConn) = True Then
                    msg = "O IMEI já está cadastrado!"

                    Return False
                End If
            End If

            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            'Dim reader As OleDbDataReader
            codigo_linha = Get_Num_linha()


            strSQL = "INSERT INTO linhas(codigo_linha,status,ativacao,desativada,digital,num_linha,oem,internet,codigo_plano,codigo_fornecedor,"
            strSQL = strSQL + " range1,range2,circuito,codigo_operadora,codigo_tipo, limite_uso,intragrupo,protocolo_cancel, CODIGO_LOCALIDADE,CONTA_CONTABIL) "
            strSQL = strSQL + " VALUES('" + codigo_linha + "','" + _linha_movel.Status + "',"
            strSQL = strSQL + " to_date('" + _linha_movel.Ativacao + "','DD/MM/YYYY'), to_date('" + _linha_movel.Desativacao + "','DD/MM/YYYY'), 'M',"
            strSQL = strSQL + " '" + _linha_movel.Telefone + "','" + _linha_movel.Chamado + "', 'S','" + IIf(_linha_movel.Plano = 0, "", _linha_movel.Plano) + "',"
            strSQL = strSQL + " '" + _linha_movel.Operadora + "',NULL,NULL,NULL,"
            strSQL = strSQL + " '" + codigoOperadora + "','" + _linha_movel.Classificacao + "' ,'" + Replace(_linha_movel.Limite_uso, ",", ".") + "','" + _linha_movel.Intragrupo + "','" + _linha_movel.Protocolo_cancel + "','" + _linha_movel.Sucursal + "','" + _linha_movel.Conta_cont + "')"

            'msg = "Erro ao executar query: " + strSQL + ""

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "INSERT INTO linhas_moveis(CODIGO_LINHA,CODIGO_OPERADORA,IP,FLEET,OBS, "
            strSQL = strSQL + " CODIGO_CLIENTE,CODIGO_USUARIO,CODIGO_TERMO,CODIGO_TECNOLOGIA,FIM_COMODATO)"
            strSQL = strSQL + " VALUES('" + codigo_linha + "','" + codigoOperadora + "','" + _linha_movel.Ip + "','" + _linha_movel.Fleet + "',"
            strSQL = strSQL + " '" + _linha_movel.Obs + "','" + _linha_movel.Codigo_cliente + "','" + IIf(_linha_movel.Usuario = "0", "", _linha_movel.Usuario) + "','" + IIf(_linha_movel.Term_resp = "0", "", _linha_movel.Term_resp) + "',"
            strSQL = strSQL + " '" + _linha_movel.Tecnologia + "',to_date('" + _linha_movel.Venc_comodato + "','DD/MM/YYYY'))"

            'msg = "Erro ao executar query: " + strSQL + ""

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If _linha_movel.Simcard <> "" Then

                'deve inserir somente se não existir o simcard
                If _dao_commons.Is_Reader_HasRows("SELECT codigo_sim FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' ", strConn) = True Then
                    'existe esse simcard não associado a telefone celular
                    strSQL = "update SIM_CARDS set puk='" & _linha_movel.Puk1 & "',pin='" & _linha_movel.Pin1 & "',puk2='" & _linha_movel.Puk2 & "',"
                    strSQL = strSQL & " pin2='" & _linha_movel.Pin2 & "',valor='" & _linha_movel.Simcard_value.Replace(",", ".").Replace(" ", ",") & "',numero='" & _linha_movel.Simcard & "' where codigo_sim='" & Get_cod_SimCard(_linha_movel.Simcard) & "'"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()
                Else

                    If _linha_movel.Simcard_value = "" Then
                        _linha_movel.Simcard_value = 0
                    End If

                    strSQL = "INSERT INTO SIM_CARDS(codigo_sim,puk,pin,pin2,puk2,valor,numero) VALUES((SELECT NVL(MAX(codigo_sim),0)+1 FROM sim_cards),"
                    strSQL = strSQL & " '" & _linha_movel.Puk1 & "','" & _linha_movel.Pin1 & "','" & _linha_movel.Pin2 & "',"
                    strSQL = strSQL & " '" & _linha_movel.Puk2 & "','" & Replace(CStr(CDbl(_linha_movel.Simcard_value)), ",", ".") & "','" & _linha_movel.Simcard & "')"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()
                End If

                strSQL = "UPDATE linhas_moveis set codigo_sim = (SELECT codigo_sim FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "')  where codigo_linha='" + CStr(codigo_linha) + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            If _linha_movel.Marca <> 0 Then

                codigoAparelho = Get_cod_Aparelho()

                If _linha_movel.Valor_aparelho = "" Then
                    _linha_movel.Valor_aparelho = 0
                End If
                strSQL = "INSERT INTO APARELHOS_MOVEIS(codigo_aparelho,pin_aparelho,valor,natureza,imei,cod_modelo,garantia,emissao,data_Retirada,Estoque,Backup,Sucata,PROPRIEDADE_ESTOQUE,ordem_servico,chamado_retirada, Perdido,NOTA_FISCAL,SERIAL_NUMBER) VALUES('" + CStr(codigoAparelho) + "','" + _linha_movel.Pin_Aparelho + "','" + Replace(CStr(CDbl(_linha_movel.Valor_aparelho)), ",", ".") + "','" + _linha_movel.Natureza + "','" + _linha_movel.Identificacao + "','" + IIf(_linha_movel.Modelo = 0, "", _linha_movel.Modelo) + "'"
                strSQL = strSQL + ",to_DATE('" + _linha_movel.Venc_garantia + "','DD/MM/YYYY'),to_DATE('" + _linha_movel.Emissao + "','DD/MM/YYYY'),to_DATE('" + _linha_movel.Data_retirada + "','DD/MM/YYYY'),'" + CStr(_linha_movel.Estoque) + "','" + CStr(_linha_movel.Backup) + "','" + CStr(_linha_movel.Sucata) + "','" + CStr(_linha_movel.Prop_estoque) + "','" + CStr(_linha_movel.Ordem_serv) + "','" + CStr(_linha_movel.Chamada_retirada) + "','" + CStr(_linha_movel.Perdido) + "','" + CStr(_linha_movel.Nota_fiscal) + "','" + CStr(_linha_movel.Serial_Number) + "')"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

                strSQL = "UPDATE linhas_moveis set codigo_aparelho = '" + CStr(codigoAparelho) + "'  where codigo_linha='" + CStr(codigo_linha) + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            If _linha_movel.Chamado <> "" Then

                strSQL = " INSERT INTO CHAMADOS_ITEMS(OEM, CODIGO_ITEM, codigo_tipo) values('" & _linha_movel.Chamado & "','" & codigo_linha & "','1')"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            strSQL = "delete from linhas_vas where codigo_linha='" + codigo_linha.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from VAS_LINHAS where linha='" + _linha_movel.Telefone.ToString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + " '"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_facilidades.Length > 0 Then
                For Each item As String In list_facilidades
                    If item <> "" Then
                        strSQL = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values('" + item.ToString + "', (select codigo_operadora from vas where codigo_vas = '" + item.ToString + "'),'" + codigo_linha.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                        strSQL = "insert into VAS_LINHAS(codigo_vas,linha) values('" + item.ToString + "', '" + _linha_movel.Telefone.ToString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + " ')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                    End If
                Next
            End If

            strSQL = "delete from grupos_item where item='" + codigo_linha.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_ccusto.Length > 0 Then
                For Each item As String In list_ccusto
                    If item <> "" Then
                        strSQL = "insert into grupos_item(grupo,modalidade,item) values('" + item.ToString + "', '4','" + codigo_linha.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            strSQL = "delete from linhas_projetos where codigo_linha='" + codigo_linha.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()
            If list_projetos.Length > 0 Then
                For Each item As String In list_projetos
                    If item <> "" Then
                        strSQL = "insert into linhas_projetos(codigo_projeto,codigo_operadora,codigo_linha) values('" + item.ToString + "',(select codigo_operadora from linhas_moveis where codigo_linha='" + codigo_linha.ToString + "'),'" + codigo_linha + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            strSQL = Gerar_Log_Moveis(codigo_linha, "N", autorlog)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            _dao_commons.EscreveLog("Erro na Insert Linhas Móveis: " & e.Message)
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            connection.Close()
            connection.Dispose()
            Return False
        End Try
    End Function

    Public Function VerificaIntegridadeCadastro(ByVal codigo_linha As String, ByVal num_linha As String, ByVal simcard As String, ByVal imei As String, ByVal codigo_aparelho As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim msg As String

        If _dao_commons.Is_Reader_HasRows("SELECT num_linha FROM LINHAS where num_linha='" & num_linha & "' and codigo_linha <> '" & codigo_linha & "' and Replace(Replace(Replace(num_linha,'(',''),')',''),'-','') <> ''", strConn) = True Then
            msg = "Aviso: Base de dados inconsistente - Existe uma outra linha com o mesmo número!"

            Return msg
        End If

        If simcard <> "" Then
            If _dao_commons.Is_Reader_HasRows("SELECT * FROM SIM_CARDS where numero='" + simcard + "' and codigo_sim in (select codigo_sim from linhas_moveis where codigo_linha <> '" + codigo_linha + "' )", strConn) = True Then
                'existe simcard associado a telefone celular
                msg = "Aviso: Base de dados inconsistente - Número de simcard está duplicado!"

                Return msg
            End If
        End If
        If imei <> "" Then
            If _dao_commons.Is_Reader_HasRows("SELECT * FROM aparelhos_moveis where imei='" + imei + "' and codigo_aparelho <>'" + CStr(codigo_aparelho) + "'", strConn) = True Then
                msg = "Aviso: Base de dados inconsistente - O IMEI cadastrado pertence a mais de um aparelho!"

                Return msg
            End If
        End If

        Return "ok"

    End Function

    Public Function AlteraAparelho(ByVal _linha_movel As AppAparelhosMoveis, ByVal codigoOperadora As String, ByVal list_facilidades As String(), ByVal list_projetos As String(), ByVal list_ccusto As String(), ByVal autorlog As String, ByRef msg As String, ByVal alterlog As Boolean) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""
        Dim codigoSim As String = ""
        Dim codigoAparelho As String = ""

        Try

            If _dao_commons.Is_Reader_HasRows("SELECT num_linha FROM LINHAS where num_linha='" & _linha_movel.Telefone & "' and num_linha <> '()-' and codigo_linha <> '" & _linha_movel.Codigo.ToString & "'", strConn) = True Then
                msg = "Já existe uma outra linha com o mesmo número!"

                Return False
            End If

            If _linha_movel.Simcard <> "" Then
                If _dao_commons.Is_Reader_HasRows("SELECT * FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' and codigo_sim in (select codigo_sim from linhas_moveis where codigo_linha <> '" + _linha_movel.Codigo.ToString + "' )", strConn) = True Then
                    'existe simcard associado a telefone celular
                    msg = "Número de simcard já cadastrado para outro telefone!"

                    Return False
                End If
            End If
            If _linha_movel.Marca <> 0 Then
                If _dao_commons.Is_Reader_HasRows("SELECT * FROM aparelhos_moveis where imei='" + _linha_movel.Identificacao + "' and codigo_aparelho <>'" + CStr(_linha_movel.Codigo_aparelho) + "'", strConn) = True Then
                    msg = "O IMEI inserido pertence a outro aparelho!"

                    Return False
                End If
            End If

            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            'Dim reader As OleDbDataReader

            If alterlog = True Then
                strSQL = Gerar_Log_Moveis(_linha_movel.Codigo.ToString, "A", autorlog)
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If

            strSQL = " update linhas set status = '" + _linha_movel.Status + "', "
            strSQL = strSQL + " ativacao = to_date('" + _linha_movel.Ativacao + "','DD/MM/YYYY'), "
            strSQL = strSQL + " desativada = to_date('" + _linha_movel.Desativacao + "','DD/MM/YYYY'), "
            strSQL = strSQL + " digital='M', num_linha='" + _linha_movel.Telefone + "',oem='" + _linha_movel.Chamado + "', "
            strSQL = strSQL + " internet='S', codigo_plano='" + IIf(_linha_movel.Plano = 0, "", _linha_movel.Plano) + "',  "
            strSQL = strSQL + " codigo_fornecedor='" + _linha_movel.Operadora + "', "
            strSQL = strSQL + " range1=NULL, range2=NULL, circuito=NULL, codigo_operadora='" + codigoOperadora + "', "
            strSQL = strSQL + " codigo_tipo='" + _linha_movel.Classificacao + "', "
            strSQL = strSQL + " limite_uso='" + Replace(_linha_movel.Limite_uso, ",", ".") + "', "
            strSQL = strSQL + " CODIGO_LOCALIDADE='" + _linha_movel.Sucursal + "', "
            strSQL = strSQL + " CONTA_CONTABIL='" + _linha_movel.Conta_cont + "', "
            strSQL = strSQL + " intragrupo='" + _linha_movel.Intragrupo + "',protocolo_cancel='" + _linha_movel.Protocolo_cancel + "' where codigo_linha='" + _linha_movel.Codigo.ToString + "' "

            'msg = "Erro ao executar query: " + strSQL + ""

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "UPDATE linhas_moveis set IP='" + _linha_movel.Ip + "',FLEET='" + _linha_movel.Fleet + "',OBS='" + _linha_movel.Obs + "',CODIGO_CLIENTE='" + _linha_movel.Codigo_cliente + "' "
            strSQL = strSQL + ",codigo_tecnologia='" + _linha_movel.Tecnologia + "',codigo_usuario='" + IIf(_linha_movel.Usuario = 0, "", _linha_movel.Usuario) + "', codigo_termo='" + IIf(_linha_movel.Term_resp = "0", "", _linha_movel.Term_resp) + "', "
            strSQL = strSQL + " fim_comodato=to_date('" + _linha_movel.Venc_comodato + "','DD/MM/YYYY') where codigo_linha ='" + _linha_movel.Codigo.ToString + "'"

            'msg = "Erro ao executar query: " + strSQL + ""

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If _linha_movel.Simcard <> "" Then

                If _dao_commons.Is_Reader_HasRows("SELECT * FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' and codigo_sim in (select codigo_sim from linhas_moveis where codigo_linha <> '" + _linha_movel.Codigo.ToString + "' )", strConn) = True Then
                    'existe simcard associado a telefone celular
                    msg = "Número de simcard já cadastrado para outro telefone!"
                    transaction.Rollback()
                    transaction.Dispose()
                    transaction = Nothing
                    Return False

                Else 'deve inserir somente se não existir o simcard
                    If _dao_commons.Is_Reader_HasRows("SELECT codigo_sim FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' ", strConn) = True Then
                        'existe esse simcard não associado a telefone celular
                        strSQL = "update SIM_CARDS set puk='" + CStr(_linha_movel.Puk1) + "',pin='" + CStr(_linha_movel.Pin1) + "',puk2='" + CStr(_linha_movel.Puk2) + "',"
                        strSQL = strSQL + " pin2='" + CStr(_linha_movel.Pin2) + "',valor='" + Replace(CStr(CDbl(_linha_movel.Simcard_value)), ",", ".") + "',numero='" + _linha_movel.Simcard + "' where codigo_sim='" + CStr(Get_cod_SimCard(_linha_movel.Simcard)) + "'"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    Else

                        If _linha_movel.Simcard_value = "" Then
                            _linha_movel.Simcard_value = 0
                        End If

                        strSQL = "INSERT INTO SIM_CARDS(codigo_sim,puk,pin,pin2,puk2,valor,numero) VALUES((SELECT NVL(MAX(codigo_sim),0)+1 FROM sim_cards),"
                        strSQL = strSQL + " '" + CStr(_linha_movel.Puk1) + "','" + CStr(_linha_movel.Pin1) + "','" + CStr(_linha_movel.Pin2) + "',"
                        strSQL = strSQL + " '" + CStr(_linha_movel.Puk2) + "','" + Replace(CStr(CDbl(_linha_movel.Simcard_value)), ",", ".") + "','" + _linha_movel.Simcard + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If

                End If

                strSQL = "UPDATE linhas_moveis set codigo_sim = (SELECT codigo_sim FROM SIM_CARDS where numero='" + _linha_movel.Simcard + "' and rownum<2)  where codigo_linha='" + CStr(_linha_movel.Codigo.ToString) + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            Else
                strSQL = "UPDATE linhas_moveis set codigo_sim = ''  where codigo_linha='" + CStr(_linha_movel.Codigo.ToString) + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            If _linha_movel.Marca <> "0" Then

                If _dao_commons.Is_Reader_HasRows("SELECT * FROM aparelhos_moveis where imei='" + _linha_movel.Identificacao + "' and codigo_aparelho <>'" + CStr(_linha_movel.Codigo_aparelho) + "'", strConn) = True Then
                    msg = "O IMEI inserido pertence a outro aparelho!"
                    transaction.Rollback()
                    transaction.Dispose()
                    transaction = Nothing
                    Return False
                End If

                If _linha_movel.Codigo_aparelho = "" Then

                    codigoAparelho = Get_cod_Aparelho()

                    If _linha_movel.Valor_aparelho = "" Then
                        _linha_movel.Valor_aparelho = 0
                    End If
                    strSQL = "INSERT INTO APARELHOS_MOVEIS(codigo_aparelho, pin_aparelho,valor,natureza,imei,cod_modelo,garantia,emissao,data_Retirada,Estoque,Backup,Sucata,PROPRIEDADE_ESTOQUE,ordem_servico,chamado_retirada, Perdido,NOTA_FISCAL,SERIAL_NUMBER) VALUES('" + CStr(codigoAparelho) + "','" + _linha_movel.Pin_Aparelho + "','" + Replace(CStr(CDbl(_linha_movel.Valor_aparelho)), ",", ".") + "','" + _linha_movel.Natureza + "','" + _linha_movel.Identificacao + "','" + IIf(_linha_movel.Modelo = 0, "", _linha_movel.Modelo) + "'"
                    strSQL = strSQL + ",to_DATE('" + _linha_movel.Venc_garantia + "','DD/MM/YYYY'),to_DATE('" + _linha_movel.Emissao + "','DD/MM/YYYY'),to_DATE('" + _linha_movel.Data_retirada + "','DD/MM/YYYY'),'" + CStr(_linha_movel.Estoque) + "','" + CStr(_linha_movel.Backup) + "','" + CStr(_linha_movel.Sucata) + "','" + CStr(_linha_movel.Prop_estoque) + "','" + CStr(_linha_movel.Ordem_serv) + "','" + CStr(_linha_movel.Chamada_retirada) + "','" + CStr(_linha_movel.Perdido) + "','" + CStr(_linha_movel.Nota_fiscal) + "','" + CStr(_linha_movel.Serial_Number) + "')"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()

                    strSQL = "UPDATE linhas_moveis set codigo_aparelho = '" + CStr(codigoAparelho) + "'  where codigo_linha='" + _linha_movel.Codigo.ToString + "'"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()
                Else
                    'codigoAparelho = Get_cod_Aparelho()

                    If _linha_movel.Valor_aparelho = "" Then
                        _linha_movel.Valor_aparelho = "0"
                    End If

                    strSQL = "update aparelhos_moveis set valor='" + Replace(_linha_movel.Valor_aparelho, ",", ".") + "',natureza='" + _linha_movel.Natureza + "',imei='" + _linha_movel.Identificacao + "',cod_modelo='" + IIf(_linha_movel.Modelo = 0, "", _linha_movel.Modelo) + "'"
                    strSQL = strSQL + " ,garantia=to_DATE('" + _linha_movel.Venc_garantia + "','DD/MM/YYYY'),emissao=to_date('" + _linha_movel.Emissao + "','DD/MM/YYYY')"
                    strSQL = strSQL + " ,pin_aparelho='" + _linha_movel.Pin_Aparelho + "'"
                    strSQL = strSQL + " ,SERIAL_NUMBER='" + _linha_movel.Serial_Number + "'"
                    strSQL = strSQL + " ,data_Retirada=to_date('" + _linha_movel.Data_retirada + "','DD/MM/YYYY'),Estoque='" + CStr(_linha_movel.Estoque) + "',Backup='" + CStr(_linha_movel.Backup) + "'"
                    strSQL = strSQL + " ,Sucata='" + CStr(_linha_movel.Sucata) + "',Perdido='" + CStr(_linha_movel.Perdido) + "',propriedade_estoque='" + CStr(_linha_movel.Prop_estoque) + "'"
                    strSQL = strSQL + ",ordem_servico='" + CStr(_linha_movel.Ordem_serv) + "',chamado_retirada='" + CStr(_linha_movel.Chamada_retirada) + "', NOTA_FISCAL='" + _linha_movel.Nota_fiscal + "' where codigo_aparelho='" + CStr(_linha_movel.Codigo_aparelho) + "'"

                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()

                    strSQL = "UPDATE linhas_moveis set codigo_aparelho = '" + _linha_movel.Codigo_aparelho + "'  where codigo_linha='" + _linha_movel.Codigo.ToString + "'"
                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()

                End If
            Else

                strSQL = "UPDATE linhas_moveis set codigo_aparelho = ''  where codigo_linha='" + _linha_movel.Codigo.ToString + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

                'strSQL = "delete from aparelhos_moveis where codigo_aparelho='" + _linha_movel.Codigo_aparelho + "'"
                'cmd.CommandText = strSQL
                'cmd.ExecuteNonQuery()

            End If

            strSQL = "delete from linhas_vas where codigo_linha='" + _linha_movel.Codigo.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from VAS_LINHAS where linha='" + _linha_movel.Telefone.ToString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + " '"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_facilidades.Length > 0 Then
                For Each item As String In list_facilidades
                    If item <> "" Then
                        strSQL = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values('" + item.ToString + "', (select codigo_operadora from vas where codigo_vas = '" + item.ToString + "'),'" + _linha_movel.Codigo.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                        strSQL = "insert into VAS_LINHAS(codigo_vas,linha) values('" + item.ToString + "', '" + _linha_movel.Telefone.ToString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + " ')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            strSQL = "delete from grupos_item where item='" + _linha_movel.Codigo.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_ccusto.Length > 0 Then
                For Each item As String In list_ccusto
                    If item <> "" Then
                        strSQL = "insert into grupos_item(grupo,modalidade,item) values('" + item.ToString + "', '4','" + _linha_movel.Codigo.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            strSQL = "delete from linhas_projetos where codigo_linha='" + _linha_movel.Codigo.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()
            If list_projetos.Length > 0 Then
                For Each item As String In list_projetos
                    If item <> "" Then
                        strSQL = "insert into linhas_projetos(codigo_projeto,codigo_operadora,codigo_linha) values('" + item.ToString + "',(select codigo_operadora from linhas_moveis where codigo_linha='" + _linha_movel.Codigo.ToString + "'),'" + _linha_movel.Codigo.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If alterlog = True Then
                strSQL = Gerar_Log_Moveis(_linha_movel.Codigo.ToString, "B", autorlog)
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If

            strSQL = " delete sim_cards sc"
            strSQL = strSQL + " where NOT EXISTS (select lm.codigo_sim "
            strSQL = strSQL + " from linhas_moveis lm "
            strSQL = strSQL + "  where lm.codigo_sim = sc.codigo_sim)"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = " delete aparelhos_moveis ap "
            strSQL = strSQL + " where not exists (select lm.codigo_aparelho "
            strSQL = strSQL + " from linhas_moveis lm where lm.codigo_aparelho = ap.codigo_aparelho) "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            _dao_commons.EscreveLog("Erro na Update Linhas Móveis: " & e.Message)
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try
    End Function

    Public Function DeletarAparelho(ByVal _linha_movel As AppAparelhosMoveis, ByVal autorlog As String, ByRef msg As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""
        Dim codigoSim As String = ""
        Dim codigoAparelho As String = ""

        'logaAparelho(codigo, "D", "")

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            'Dim reader As OleDbDataReader

            strSQL = Gerar_Log_Moveis(_linha_movel.Codigo.ToString, "D", autorlog)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM linhas_projetos where codigo_linha='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM linhas_vas where codigo_linha='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM grupos_item where item='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM linhas_moveis where codigo_linha='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM manutencoes where codigo_aparelho = '" + _linha_movel.Codigo_aparelho.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from aparelhos_moveis where codigo_aparelho = '" + _linha_movel.Codigo_aparelho.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE FROM linhas where codigo_linha='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "DELETE from CHAMADOS_ITEMS t where t.codigo_tipo='1' and  t.codigo_item='" + _linha_movel.Codigo.ToString + "' "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If _linha_movel.Simcard <> "" Then
                strSQL = "delete from sim_cards where codigo_sim = '" + Get_cod_SimCard(_linha_movel.Simcard.ToString) + "'"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If

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

    Public Function Get_Num_linha() As String
        Dim connection As New OleDbConnection(strConn)
        Dim codigo_linha As String = ""

        Dim strSQL As String = "select (nvl(max(CODIGO_LINHA),0)+1)as CODIGO_LINHA from LINHAS"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                codigo_linha = reader.Item("CODIGO_LINHA").ToString
            End While
        End Using

        Return codigo_linha
    End Function

    Public Function Get_cod_SimCard(ByVal SimCard As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim codigo_sim As String = ""

        Dim strSQL As String = "SELECT codigo_sim FROM SIM_CARDS where numero='" + SimCard + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                codigo_sim = reader.Item("codigo_sim").ToString
            End While
        End Using

        Return codigo_sim
    End Function

    Public Function Get_cod_Aparelho() As String
        Dim connection As New OleDbConnection(strConn)
        Dim codigo_aparelho As String = ""

        Dim strSQL As String = "SELECT (nvl(MAX(codigo_aparelho),0)+1) as codigo_aparelho FROM aparelhos_moveis"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                codigo_aparelho = reader.Item("codigo_aparelho").ToString
            End While
        End Using

        Return codigo_aparelho
    End Function

    Public Function Gerar_Log_Moveis(ByVal codigo As String, ByVal tipo As Char, ByVal autorlog As String) As String

        _dao_commons.strConn = strConn
        Dim strSQL As String = ""
        Dim chamado As List(Of AppGeneric) = _dao_commons.GetGenericList("", "p1.OEM", "nvl(p1.abertura, '')", " chamados p1, chamados_items p2 ", "", " and p1.oem = p2.oem and p2.codigo_item='" & codigo & "' and p1.tipo_item ='1' order by descricao desc")
        Dim num_chamado As String = ""
        If chamado.Count > 0 Then
            num_chamado = chamado.Item(0).Codigo
        End If

        strSQL = "insert into linhas_moveis_log (codigo_linha,valor_unit,venc_garan,sim_card,desc_acess,pim,puc,pin2,puk2,pin_aparelho,hexa,status,ativacao,desativado,termo_resp,oem,contrato,nota_fiscal, "
        strSQL = strSQL + " venc_conta,cod_conta,num_tel,codigo_plano,codigo_localidade,codigo_grupo,codigo_usuario,codigo_fornecedor,imei,ip,cod_modelo,natu_operacao,servicos, "
        strSQL = strSQL + " mensalidade,fleet,codigo_cliente,obs,limite_uso,estoque,backup,perdido,sucata,propriedade_estoque,ordem_servico,chamado_retirada,data_retirada,emissao,protocolo_cancel,SERIAL_NUMBER,CONTA_CONTABIL,tipo,codigo,data,autor) "
        strSQL = strSQL + " (select * from ("
        strSQL = strSQL + " Select l.CODIGO_LINHA, a.VALOR, a.GARANTIA,sim.NUMERO,' '  desc_acess,sim.PIN,sim.PUK,sim.pin2,sim.puk2,a.pin_aparelho, a.HEXA,nvl(trim(l.STATUS),'1') status,l.ATIVACAO,"
        strSQL = strSQL + " l.DESATIVADA,t.NUMERO termo, '" & num_chamado & "' as oem ,l.CONTRATO,nvl(a.NOTA_FISCAL,'') nota,l.VENC_CONTA,l.CONTA,l.NUM_LINHA,l.CODIGO_PLANO,l.CODIGO_LOCALIDADE,"
        strSQL = strSQL + " g.CODIGO codigo_grupo,u.CODIGO codigo_usuario,l.CODIGO_FORNECEDOR,a.IMEI,lm.IP,a.COD_MODELO,a.NATUREZA,' ' servicos,lm.MENSALIDADE"
        strSQL = strSQL + " , lm.FLEET,(select codigo_cliente from linhas_moveis where codigo_linha='" & codigo & "'),lm.obs,nvl(l.limite_uso,'')limite_uso,"
        strSQL = strSQL + " nvl(a.estoque,'N')estoque,nvl(a.backup,'N')backup,nvl(a.perdido,'N')perdido,nvl(a.sucata,'N')sucata,"
        strSQL = strSQL + " nvl(a.propriedade_estoque,'')propriedade_estoque,nvl(a.ordem_servico,'')ordem_servico,nvl(a.chamado_retirada,'')chamado_retirada,a.data_retirada,a.emissao,l.protocolo_cancel, a.SERIAL_NUMBER, l.CONTA_CONTABIL "
        'strSQL=strSQL+" , lm.FLEET,lm.CODIGO_CLIENTE,lm.obs "
        strSQL = strSQL + " from aparelhos_moveis a, linhas l, linhas_moveis lm, sim_cards sim, localidades lo, usuarios u, aparelhos_marcas ma,"
        strSQL = strSQL + " aparelhos_modelos mo, fornecedores f, grupos g, grupos_item gi,  termos_responsabilidade t "
        strSQL = strSQL + " where"
        strSQL = strSQL + " l.CODIGO_LINHA = lm.CODIGO_LINHA and lm.CODIGO_APARELHO = a.CODIGO_APARELHO(+) "
        strSQL = strSQL + " and lm.CODIGO_SIM = sim.CODIGO_SIM(+) and l.CODIGO_LOCALIDADE=lo.CODIGO(+) and"
        strSQL = strSQL + " a.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = ma.COD_MARCA(+)  and lm.codigo_termo=t.codigo_termo(+) "
        strSQL = strSQL + " and l.CODIGO_FORNECEDOR = f.CODIGO(+) and l.codigo_linha=gi.item(+) and nvl(gi.modalidade,'4') ='4' and gi.grupo = g.codigo(+) "
        'strSQL=strSQL+" and nvl(trim(l.STATUS),'1') = to_CHAR(s.CODIGO_STATUS) "
        strSQL = strSQL + " and lm.codigo_usuario = u.codigo(+)  "
        strSQL = strSQL + " and l.codigo_linha='" + codigo + "' and rownum < 2 "
        'strSQL=strSQL+" ) "
        'strSQL=strSQL+" select l.CODIGO_LINHA, ma.MARCA, mo.MODELO, DECODE(mo.COD_TIPO,'CELULAR','RADIO','OUTROS'), u.NOME_USUARIO,"
        'strstrSQL = strstrSQL +"g.NOME_GRUPO,s.DESCRICAO,lo.LOCALIDADE,l.NUM_LINHA,f.NOME_FANTASIA,a.IMEI,nvl(lm.OBS,' ')"

        strSQL = strSQL + " ) , "
        strSQL = strSQL + "(select '" & tipo & "' tipo,(select nvl(max(codigo),0)+1 from linhas_moveis_log) codigo_log,sysdate data_atual,'" & autorlog & "' autor from dual))"


        Return strSQL
    End Function

    Public Function Gravar_Log_Moveis(ByVal codigo As String, ByVal tipo As Char, ByVal autorlog As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            _dao_commons.strConn = strConn
            Dim chamado As List(Of AppGeneric) = _dao_commons.GetGenericList("", "p1.OEM", "nvl(p1.abertura, '')", " chamados p1, chamados_items p2 ", "", " and p1.oem = p2.oem and p2.codigo_item='" & codigo & "' and p1.tipo_item ='1' order by descricao desc")

            strSQL = "insert into linhas_moveis_log (codigo_linha,valor_unit,venc_garan,sim_card,desc_acess,pim,puc,pin2,puk2,pin_aparelho,hexa,status,ativacao,desativado,termo_resp,oem,contrato,nota_fiscal, "
            strSQL = strSQL + " venc_conta,cod_conta,num_tel,codigo_plano,codigo_localidade,codigo_grupo,codigo_usuario,codigo_fornecedor,imei,ip,cod_modelo,natu_operacao,servicos, "
            strSQL = strSQL + " mensalidade,fleet,codigo_cliente,obs,limite_uso,estoque,backup,perdido,sucata,propriedade_estoque,ordem_servico,chamado_retirada,data_retirada,emissao,protocolo_cancel,SERIAL_NUMBER,CONTA_CONTABIL,tipo,codigo,data,autor) "
            strSQL = strSQL + " (select * from ("
            strSQL = strSQL + " Select l.CODIGO_LINHA, a.VALOR, a.GARANTIA,sim.NUMERO,' '  desc_acess,sim.PIN,sim.PUK,sim.pin2,sim.puk2,a.pin_aparelho, a.HEXA,nvl(trim(l.STATUS),'1') status,l.ATIVACAO,"
            strSQL = strSQL + " l.DESATIVADA,t.NUMERO termo, '" & IIf(chamado.Count > 0, chamado.Item(0).Codigo, "") & "' as oem ,l.CONTRATO,nvl(a.NOTA_FISCAL,'') nota,l.VENC_CONTA,l.CONTA,l.NUM_LINHA,l.CODIGO_PLANO,l.CODIGO_LOCALIDADE,"
            strSQL = strSQL + " g.CODIGO codigo_grupo,u.CODIGO codigo_usuario,l.CODIGO_FORNECEDOR,a.IMEI,lm.IP,a.COD_MODELO,a.NATUREZA,' ' servicos,lm.MENSALIDADE"
            strSQL = strSQL + " , lm.FLEET,(select codigo_cliente from linhas_moveis where codigo_linha='" & codigo & "'),lm.obs,nvl(l.limite_uso,'')limite_uso,"
            strSQL = strSQL + " nvl(a.estoque,'N')estoque,nvl(a.backup,'N')backup,nvl(a.perdido,'N')perdido,nvl(a.sucata,'N')sucata,"
            strSQL = strSQL + " nvl(a.propriedade_estoque,'')propriedade_estoque,nvl(a.ordem_servico,'')ordem_servico,nvl(a.chamado_retirada,'')chamado_retirada,a.data_retirada,a.emissao,l.protocolo_cancel, a.SERIAL_NUMBER, l.CONTA_CONTABIL "
            'strSQL=strSQL+" , lm.FLEET,lm.CODIGO_CLIENTE,lm.obs "
            strSQL = strSQL + " from aparelhos_moveis a, linhas l, linhas_moveis lm, sim_cards sim, localidades lo, usuarios u, aparelhos_marcas ma,"
            strSQL = strSQL + " aparelhos_modelos mo, fornecedores f, grupos g, grupos_item gi,  termos_responsabilidade t "
            strSQL = strSQL + " where"
            strSQL = strSQL + " l.CODIGO_LINHA = lm.CODIGO_LINHA and lm.CODIGO_APARELHO = a.CODIGO_APARELHO(+) "
            strSQL = strSQL + " and lm.CODIGO_SIM = sim.CODIGO_SIM(+) and l.CODIGO_LOCALIDADE=lo.CODIGO(+) and"
            strSQL = strSQL + " a.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = ma.COD_MARCA(+)  and lm.codigo_termo=t.codigo_termo(+) "
            strSQL = strSQL + " and l.CODIGO_FORNECEDOR = f.CODIGO(+) and l.codigo_linha=gi.item(+) and nvl(gi.modalidade,'4') ='4' and gi.grupo = g.codigo(+) "
            'strSQL=strSQL+" and nvl(trim(l.STATUS),'1') = to_CHAR(s.CODIGO_STATUS) "
            strSQL = strSQL + " and lm.codigo_usuario = u.codigo(+)  "
            strSQL = strSQL + " and l.codigo_linha='" + codigo + "' and rownum < 2 "
            'strSQL=strSQL+" ) "
            'strSQL=strSQL+" select l.CODIGO_LINHA, ma.MARCA, mo.MODELO, DECODE(mo.COD_TIPO,'CELULAR','RADIO','OUTROS'), u.NOME_USUARIO,"
            'strstrSQL = strstrSQL +"g.NOME_GRUPO,s.DESCRICAO,lo.LOCALIDADE,l.NUM_LINHA,f.NOME_FANTASIA,a.IMEI,nvl(lm.OBS,' ')"

            strSQL = strSQL + " ) , "
            strSQL = strSQL + "(select '" & tipo & "' tipo,(select nvl(max(codigo),0)+1 from linhas_moveis_log) codigo_log,sysdate data_atual,'" & autorlog & "' autor from dual))"

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


    Public Function ApagaAparelhos(ByVal codigo_aparelho As String, codigo_linha As String, ByVal codigo_aparelho2 As String, ByRef linha2 As List(Of AppGeneric)) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        linha2 = _dao_commons.GetGenericList("", "codigo_linha", "codigo_linha", "linhas_moveis", "", " and codigo_aparelho='" & codigo_aparelho2 & "'")

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            _dao_commons.strConn = strConn

            strSQL = " update linhas_moveis set "
            strSQL = strSQL + " codigo_aparelho =''   "
            strSQL = strSQL + " where codigo_linha ='" & linha2.Item(0).Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = " update linhas_moveis set "
            strSQL = strSQL + " codigo_aparelho =''   "
            strSQL = strSQL + " where codigo_linha ='" & codigo_linha & "'"

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

    Public Function Trocar_de_aparelho(ByVal codigo_aparelho As String, codigo_linha As String, ByVal codigo_aparelho2 As String, ByVal linha2 As List(Of AppGeneric)) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""
        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            _dao_commons.strConn = strConn

            strSQL = " update linhas_moveis set "
            strSQL = strSQL + " codigo_aparelho ='" & codigo_aparelho & "'"
            strSQL = strSQL + " where codigo_linha ='" & linha2.Item(0).Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = " update linhas_moveis set "
            strSQL = strSQL + " codigo_aparelho ='" & codigo_aparelho2 & "'"
            strSQL = strSQL + " where codigo_linha ='" & codigo_linha & "'"

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


    Public Function CriaLogTroca(ByVal codigo_linha1 As String, ByVal codigo_linha2 As String, ByVal tipo As Char, ByVal autorlog As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = Gerar_Log_Moveis(codigo_linha1.ToString, tipo, autorlog)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = Gerar_Log_Moveis(codigo_linha2.ToString, tipo, autorlog)
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


    Public Function AtualizaFornecedorPlano(ByVal codigo_linha1 As String, cod_operadora As String, cod_fornecedor As String, cod_plano As String, autorlog As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            _dao_commons.strConn = strConn
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'LOG A
            strSQL = Gerar_Log_Moveis(codigo_linha1.ToString, "A", autorlog)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update linhas set codigo_operadora='" & cod_operadora & "', codigo_fornecedor='" & cod_fornecedor & "' , codigo_plano='" & cod_plano & "'  where codigo_linha='" & codigo_linha1 & "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'atualiza as facilidades

            Dim Telefone As String = _dao_commons.myDataTable("select t.num_linha from LINHAS t where t.codigo_linha='" & codigo_linha1 & "'").Rows(0).Item(0).ToString.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")

            'vamos pegar as facilidades do plano
            Dim list_facilidades As String()
            Dim string_aux As String = ""
            Dim dt As Data.DataTable = _dao_commons.myDataTable("select codigo_vas from PLANOS_VAS t where t.codigo_plano='" & cod_plano & "'")
            For Each _row As Data.DataRow In dt.Rows
                string_aux = string_aux + " " + _row.Item(0).ToString
            Next

            list_facilidades = string_aux.Split(" ")


            strSQL = "delete from linhas_vas where codigo_linha='" + codigo_linha1.ToString + "'"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_facilidades.Length > 0 Then
                strSQL = "delete from VAS_LINHAS where linha='" + Telefone + " '"
                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If


            If list_facilidades.Length > 0 Then
                For Each item As String In list_facilidades
                    If item <> "" Then
                        strSQL = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values('" + item.ToString + "', (select codigo_operadora from vas where codigo_vas = '" + item.ToString + "'),'" + codigo_linha1.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()

                        strSQL = "insert into VAS_LINHAS(codigo_vas,linha) values('" + item.ToString + "', '" + Telefone + " ')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            'LOG B

            strSQL = Gerar_Log_Moveis(codigo_linha1.ToString, "B", autorlog)
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

End Class

