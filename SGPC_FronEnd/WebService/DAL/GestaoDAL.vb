Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.IO
Imports System
Imports System.Collections.Generic

Public Class GestaoDAL
    Private _strConn As String
    Public Property StrConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Private tabelCDRS As String = "cdrs_celular"

    ''' <summary>
    ''' Este método atualiza o código das tarifas das chamadas
    ''' </summary>
    ''' <param name="pCodOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateClassificacao(ByVal pCodOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim strSQL As String = "update " & tabelCDRS & " set tarif_codigo=get_tarif_cod(classificacell(trim(tipo_serv),trim(tipo_serv2)," & pCodOperadora & ")), codigo_tipo_ligacao =classificacell(trim(tipo_serv),trim(tipo_serv2)," & pCodOperadora & ") where codigo_conta= " & pCodigoConta & " and cdr_codigo='3'"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateClassificacao]: " & ex.Message
        End Try
    End Function
    ''' <summary>
    ''' Este método atualiza o código das tarifas dos serviços
    ''' </summary>
    ''' <param name="pCodOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateClassificacaoServicos(ByVal pCodOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim strSQL As String = "update " & tabelCDRS & " set tarif_codigo=get_tarif_cod_servico(trim(tipo_serv2),trim(tipo_serv)," & pCodOperadora & ")where codigo_conta= " & pCodigoConta & " and cdr_codigo='4'"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função: UpdateClassificacaoServicos-Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateClassificacaoServicos]: " & ex.Message
        End Try
    End Function

    Public Function UpdateAudit(ByVal pCodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pTipo As Integer) As String
        Try
            Dim strSQL As String = ""
            If pTipo = 1 Then
                strSQL = "update cdrs_celular set valor_audit=gestaoAudit(rml_numero_a,case when trim(tipo_serv2)='' then tipo_serv else tipo_serv2 end,'" & pCodigoOperadora & "',valor_cdr,'1') where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='4' or cdr_codigo='5' ) "
            Else
                strSQL = "update cdrs_celular set valor_ok=gestaoAudit(rml_numero_a,case when trim(tipo_serv2)='' then tipo_serv else tipo_serv2 end,'" & pCodigoOperadora & "',valor_cdr,'2') where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='4' or cdr_codigo='5' ) "
            End If

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateAudit]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Este médoto verifica se as chamadas VC2 estão sendo cobradas devidamente
    ''' </summary>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GestaoVerificaVC2(ByVal pCodigoConta As Integer, ByVal pcodigoOperadora As Integer) As String
        Try
            Dim strSQL As String = ""
            strSQL = "select rml_numero_a,numero_b,codigo,nvl(categoria,'0')categoria, tipo_serv2 from cdrs_celular where codigo_conta=" & pCodigoConta & " and cdr_codigo=3"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim numero_a As String = reader.Item("rml_numero_a").ToString
                    Dim numero_b As String = IIf(reader.Item("numero_b").ToString.Substring(0, 1) = "0" And reader.Item("numero_b").ToString.Substring(0, 4) <> "0800", reader.Item("numero_b").ToString.Remove(0, 1), reader.Item("numero_b").ToString.Trim)
                    Dim codigo As String = reader.Item("codigo").ToString
                    Dim categoria As String = reader.Item("categoria").ToString
                    Dim tipo_serv2 As String = reader.Item("tipo_serv2").ToString
                    If (Mid(numero_a, 1, 2) = Mid(numero_b, 1, 2)) And tipo_serv2.ToUpper <> "V2R" And (tipo_serv2.ToUpper.Trim = "VALOR DE COMUNICAÇÃO 2" Or categoria.ToUpper.Trim Like "V2*" Or tipo_serv2.ToUpper.Trim = "CHAMADAS DE LONGA DISTÂNCIA DENTRO DO ESTADO" Or tipo_serv2.ToUpper.Trim = "VC2" Or tipo_serv2.ToUpper.Trim = "V2F" Or tipo_serv2.ToUpper.Trim = "V2M" Or tipo_serv2.ToUpper.Trim = "V2T") And (categoria.ToUpper.Trim <> "V2R") Then

                        Dim strSQLUpdate As String = ""
                        strSQLUpdate = "update cdrs_celular set obs='VC2 classificado indevidamente', valor_audit=tarifaGestao((select NOME_CONFIGURACAO from tarifacao where (upper(NOME_CONFIGURACAO) like 'CHAMADAS LOCAIS%' or upper(NOME_CONFIGURACAO) like 'VC LOCAL%' or upper(NOME_CONFIGURACAO) like 'VC2%' or upper(NOME_CONFIGURACAO) like 'V2T%' or upper(NOME_CONFIGURACAO) like 'V2M%' or upper(NOME_CONFIGURACAO) like 'V2F%')  and OPER_CODIGO_OPERADORA='" & pcodigoOperadora & "' and rownum<=1),duracao,'" & pcodigoOperadora & "',valor_cdr,rml_numero_a,numero_b,categoria,'1') where codigo='" & codigo & "'"
                        Dim cmd2 As OleDbCommand = connection.CreateCommand
                        cmd2.CommandText = strSQLUpdate
                        cmd2.ExecuteNonQuery()
                        cmd2.Dispose()
                    End If
                End While
            End Using

            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[GestaoVerificaVC2]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Este médoto verifica se as chamadas VC3 estão sendo cobradas devidamente
    ''' </summary>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GestaoVerificaVC3(ByVal pCodigoConta As Integer, ByVal pcodigoOperadora As Integer) As String


        Try
            Dim strSQL As String = ""
            strSQL = "select rml_numero_a,numero_b,codigo,nvl(categoria,'0')categoria, tipo_serv2 from cdrs_celular where codigo_conta=" & pCodigoConta & " and cdr_codigo=3 "

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim numero_a As String = reader.Item("rml_numero_a").ToString
                    Dim numero_b As String
                    Dim categoria As String = reader.Item("categoria").ToString
                    If Len(reader.Item("numero_b")) >= 8 Then
                        numero_b = IIf(reader.Item("numero_b").ToString.Substring(0, 1) = "0" And reader.Item("numero_b").ToString.Substring(0, 4) <> "0800", reader.Item("numero_b").ToString.Remove(0, 1), reader.Item("numero_b").ToString.Trim)
                    Else
                        numero_b = reader.Item("numero_b")
                    End If
                    Dim strSQLUpdate As String = ""
                    Dim codigo As String = reader.Item("codigo").ToString
                    Dim tipo_serv2 As String = reader.Item("tipo_serv2").ToString

                    If (Mid(numero_a, 1, 2) = Mid(numero_b, 1, 2)) And tipo_serv2.ToUpper <> "V3R" And (tipo_serv2.ToUpper = "VALOR DE COMUNICAÇÃO 3" Or categoria.ToUpper.Trim Like "V3*" Or tipo_serv2.ToUpper.Trim = "CHAMADAS DE LONGA DISTÂNCIA FORA DO ESTADO" Or tipo_serv2.ToUpper.Trim = "VC3" Or tipo_serv2.ToUpper.Trim = "V3T" Or tipo_serv2.ToUpper.Trim = "V3M" Or tipo_serv2.ToUpper.Trim = "V3F") And (categoria.ToUpper.Trim <> "V3R") Then
                        'é um VC1 e aplica a tarifa de VC
                        strSQLUpdate = "update cdrs_celular set obs='VC3 classificado indevidamente', valor_audit=tarifaGestao((select NOME_CONFIGURACAO from tarifacao where (upper(NOME_CONFIGURACAO) like 'CHAMADAS LOCAIS%' or upper(NOME_CONFIGURACAO) like 'VC LOCAL%' or upper(NOME_CONFIGURACAO) like 'VC3%' or upper(NOME_CONFIGURACAO) like 'V3T%' or upper(NOME_CONFIGURACAO) like 'V3M%' or upper(NOME_CONFIGURACAO) like 'V3F%')  and OPER_CODIGO_OPERADORA='" & pcodigoOperadora & "' and rownum<=1),duracao,'" & pcodigoOperadora & "',valor_cdr,rml_numero_a,numero_b,categoria,'1') where codigo='" & codigo & "'"
                        Dim cmd2 As OleDbCommand = connection.CreateCommand
                        cmd2.CommandText = strSQLUpdate
                        cmd2.ExecuteNonQuery()
                        cmd2.Dispose()
                    Else
                        If (Mid(numero_a, 1, 1) = Mid(numero_b, 1, 1)) And (tipo_serv2.ToUpper = "VALOR DE COMUNICAÇÃO 3" Or categoria.ToUpper.Trim Like "V3*" Or tipo_serv2.ToUpper.Trim = "CHAMADAS DE LONGA DISTÂNCIA FORA DO ESTADO") And (categoria.ToUpper.Trim <> "V3R") Then
                            strSQLUpdate = "update cdrs_celular set obs='VC3 classificado indevidamente' where codigo='" & codigo & "'"
                            Dim cmd2 As OleDbCommand = connection.CreateCommand
                            cmd2.CommandText = strSQLUpdate
                            cmd2.ExecuteNonQuery()
                            cmd2.Dispose()
                        End If
                    End If

                End While
            End Using

            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[GestaoVerificaVC3]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Método que Tarifa as ligações da CDRS_CELULAR
    ''' </summary>
    ''' <param name="pCodigoOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <param name="pTipo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TarifaGestao(ByVal pCodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pTipo As Integer) As String


        Try
            Dim strSQL As String = ""
            If pTipo = 1 Then
                strSQL = "update cdrs_celular set valor_audit=tarifaGestao(tipo_serv2,duracao,'" & pCodigoOperadora & "',valor_cdr,replace(rml_numero_a,' ',''),replace(numero_b,' ',''),categoria,'1') where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='3') "
            Else
                strSQL = "update cdrs_celular set valor_ok=tarifaGestao(tipo_serv2,duracao,'" & pCodigoOperadora & "',valor_cdr,replace(rml_numero_a,' ',''),replace(numero_b,' ',''),categoria,'2') where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='3')  "
            End If

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função TarifaGestao ", Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[TarifaGestao]: " & ex.Message
        End Try
    End Function


    ''' <summary>
    ''' Este método tarifas as ligações de DDI
    ''' </summary>
    ''' <param name="pCodigoOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <param name="pTipo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TarifaGestaoDDI(ByVal pCodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pTipo As Integer) As String


        Try
            Dim strSQL As String = ""
            If pTipo = 1 Then
                strSQL = "update cdrs_celular set valor_audit=tarifaGestaoDDI(tipo_serv2,duracao,'" & pCodigoOperadora & "',valor_cdr,rml_numero_a,numero_b,'1',COD_PAIS_CHAMADO,CLASSE_SERVICO) where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='3') and COD_PAIS_CHAMADO is not null and COD_PAIS_CHAMADO<>'55' "
            Else
                strSQL = "update cdrs_celular set valor_ok=tarifaGestaoDDI(tipo_serv2,duracao,'" & pCodigoOperadora & "',valor_cdr,rml_numero_a,numero_b,'2',COD_PAIS_CHAMADO,CLASSE_SERVICO) where codigo_conta='" & pCodigoConta & "' and (cdr_codigo='3')  and COD_PAIS_CHAMADO is not null and COD_PAIS_CHAMADO<>'55' "
            End If

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função TarifaGestaoDDI ", Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[TarifaGestaoDDI]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Este método verifica se cobraram ligações dentro do intragrupo
    ''' </summary>
    ''' <param name="pCodigoConta"></param>
    ''' <param name="pCodigoOperadora"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessaIntragrupo(ByVal pCodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Dim connection As New OleDbConnection(StrConn)
        Dim strSQL As String = ""
        Dim vc1 As String = ""
        Dim vc2 As String = ""
        Dim vc3 As String = ""

        'pegamos o tipo ligacao de intragrupo da operadora
        strSQL = "SELECT NVL(VC1,'N')VC1,NVL(VC2,'N')VC2,NVL(VC3,'N')VC3 FROM INTRAGRUPO P1 where P1.CODIGO_OPERADORA = '" & pCodigoOperadora & "'"
        Dim cmd3 As OleDbCommand = connection.CreateCommand
        Dim reader3 As OleDbDataReader
        cmd3.CommandText = strSQL
        connection.Open()
        reader3 = cmd3.ExecuteReader

        If Not reader3.HasRows Then
            connection.Close()
            cmd3.Dispose()
            Return "OK"
        End If

        Using connection
            While reader3.Read
                vc1 = reader3.Item("VC1").ToString.ToUpper
                vc2 = reader3.Item("VC2").ToString.ToUpper
                vc3 = reader3.Item("VC3").ToString.ToUpper
            End While
        End Using
        connection.Close()
        cmd3.Dispose()

        Try
            strSQL = "GESTAOAUDITINTRAGRUPO('" & pCodigoConta & "','" & pCodigoOperadora & "','" & vc1 & "','" & vc2 & "','" & vc3 & "')"

            Dim franquia As Integer = 0
            connection = New OleDbConnection(StrConn)
            connection.Open()
            Dim cmd4 As OleDbCommand = connection.CreateCommand
            cmd4.CommandType = CommandType.StoredProcedure
            cmd4.CommandText = strSQL
            cmd4.ExecuteNonQuery()
            cmd4.Dispose()
            connection.Close()
            Return "OK"
        Catch ex As Exception
            Return ex.Message
        Finally
            connection.Close()
        End Try

    End Function


    ''' <summary>
    ''' Este método pega o os minutos da franquia de cada celular e verifica se está sendo cobrado corretamente
    ''' </summary>
    ''' <param name="pCodigoOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessaAuditFranquias(ByVal pCodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String

        Dim listCdr As New List(Of cdr)
        Dim listCdr2 As New List(Of cdr)
        Dim listCdrAtualizar As New List(Of cdr)
        Dim compartilhado As String = ""
        Dim tipo_fraquia As String = ""
        Dim connection As New OleDbConnection(StrConn)

        'vamos pegar o tipo de franquia (VAlor/Minuto - Todas as linhas/Individual/Compartilhado)
        Dim strSQL As String = "Select case when minutos>0 then 'M' else 'V' end tipo, nvl(compartilhado,'T')compartilhado from vas where franquia='S' and codigo_operadora='" & pCodigoOperadora & "'"
        Dim cmd3 As OleDbCommand = connection.CreateCommand
        Dim reader3 As OleDbDataReader
        cmd3.CommandText = strSQL
        connection.Open()
        reader3 = cmd3.ExecuteReader

        If Not reader3.HasRows Then
            connection.Close()
            cmd3.Dispose()
            Return "OK"
        End If

        Using connection
            While reader3.Read
                tipo_fraquia = reader3.Item("tipo").ToString.ToUpper
                compartilhado = reader3.Item("compartilhado").ToString.ToUpper
            End While
        End Using
        connection.Close()
        cmd3.Dispose()

        If tipo_fraquia = "M" Then

            Try


                'verifica se é para aplicar a franquia em todas as linhas da fatura ou verificar uma a uma
                If compartilhado = "T" Or compartilhado = "C" Then
                    'listCdrAtualizar = ProcessaAuditFranquiaAtualizarTodasLinhas(listCdr, pCodigoOperadora)
                    strSQL = "GESTAOAUDITFRAQUIA_MINUTO('" & pCodigoConta & "','" & pCodigoOperadora & "')"

                    Dim franquia As Integer = 0
                    connection = New OleDbConnection(StrConn)
                    connection.Open()
                    Dim cmd4 As OleDbCommand = connection.CreateCommand
                    cmd4.CommandType = CommandType.StoredProcedure
                    cmd4.CommandText = strSQL
                    cmd4.ExecuteNonQuery()
                    cmd4.Dispose()
                    connection.Close()

                    'faz o update do valor_ok na tabela cdrs
                    If FranquiaValorTemTipoLigacao(pCodigoOperadora, "C") Then
                        strSQL = "update cdrs_celular set valor_ok='3', obs='VALOR COBRADO DENTRO DA FRANQUIA DE MINUTOS', valor_franquia=valor_audit, valor_audit=0 where valor_cdr>0 and SALDO_FRANQUIA > 0  and codigo_tipo_ligacao in(select p4.codigo_tipo_ligacao from vas_tarifas p4 where p4.codigo_vas in(select p5.codigo_vas from vas p5 where p5.franquia='S' and p5.codigo_operadora='" & pCodigoOperadora & "' and p5.compartilhado='C')) and codigo_conta ='" & pCodigoConta & "'"
                    Else
                        strSQL = "update cdrs_celular set valor_ok='3', obs='VALOR COBRADO DENTRO DA FRANQUIA DE MINUTOS', valor_franquia=valor_audit, valor_audit=0 where valor_cdr>0 and SALDO_FRANQUIA > 0   and codigo_conta ='" & pCodigoConta & "'"
                    End If

                    connection = New OleDbConnection(StrConn)
                    connection.Open()
                    Dim cmd5 As OleDbCommand = connection.CreateCommand
                    cmd5.CommandText = strSQL
                    cmd5.ExecuteNonQuery()
                    cmd5.Dispose()
                    connection.Close()
                    Return "ok"


                Else
                    strSQL = "Select codigo,duracao,rml_numero_a,valor_cdr,codigo_tipo_ligacao,nvl(valor_audit,'0')valor_audit,nvl(valor_ok,'0')valor_ok,nvl(tarif_codigo,'0')tarif_codigo from cdrs_celular where codigo_conta='" & pCodigoConta & "' and cdr_codigo='3' order by rml_numero_a,tarif_codigo,data_inicio,duracao"

                    Dim cmd As OleDbCommand = connection.CreateCommand
                    Dim reader As OleDbDataReader
                    cmd.CommandText = strSQL
                    connection.Open()
                    reader = cmd.ExecuteReader

                    Using connection
                        While reader.Read
                            Dim _cdr As New cdr
                            _cdr.CdrCodigo = reader.Item("codigo").ToString
                            _cdr.Duracao = reader.Item("duracao").ToString
                            _cdr.ValorCDR = reader.Item("valor_cdr").ToString
                            _cdr.ValorAudit = reader.Item("valor_audit").ToString
                            _cdr.ValorOk = reader.Item("valor_ok").ToString
                            _cdr.RmlNumeroA = reader.Item("rml_numero_a").ToString
                            _cdr.Tarif_Codigo = reader.Item("tarif_Codigo").ToString
                            _cdr.Tipo_ligacao = reader.Item("codigo_tipo_ligacao").ToString
                            listCdr.Add(_cdr)
                        End While
                    End Using
                    connection.Close()
                    cmd.Dispose()
                    listCdrAtualizar = ProcessaAuditFranquiaAtualizar(listCdr, pCodigoOperadora)
                    UpdateAuditFranquias(listCdrAtualizar)
                End If


                Return "ok"
            Catch ex As Exception
                'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
                'GeraArquivoLog(myLog)
                Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            End Try

        Else
            'franquia de valor
            If compartilhado = "C" Then
                Try
                    strSQL = "GESTAOAUDITFRAQUIA_VALOR('" & pCodigoConta & "','" & pCodigoOperadora & "')"

                    Dim franquia As Integer = 0
                    connection = New OleDbConnection(StrConn)
                    connection.Open()
                    Dim cmd4 As OleDbCommand = connection.CreateCommand
                    cmd4.CommandType = CommandType.StoredProcedure
                    cmd4.CommandText = strSQL
                    cmd4.ExecuteNonQuery()
                    cmd4.Dispose()
                    connection.Close()


                    'faz o update do valor_ok na tabela cdrs
                    If FranquiaValorTemTipoLigacao(pCodigoOperadora, "C") Then
                        strSQL = "update cdrs_celular set valor_ok='3', obs='VALOR COBRADO DENTRO DA FRANQUIA DE VALOR', valor_franquia=valor_audit, valor_audit=0 where valor_cdr>0 and SALDO_FRANQUIA > 0  and codigo_tipo_ligacao in(select p4.codigo_tipo_ligacao from vas_tarifas p4 where p4.codigo_vas in(select p5.codigo_vas from vas p5 where p5.franquia='S' and p5.codigo_operadora='" & pCodigoOperadora & "' and p5.compartilhado='C' and p5.valor is not null and p5.minutos is null)) and codigo_conta ='" & pCodigoConta & "'"
                    Else
                        strSQL = "update cdrs_celular set valor_ok='3', obs='VALOR COBRADO DENTRO DA FRANQUIA DE VALOR', valor_franquia=valor_audit, valor_audit=0 where valor_cdr>0 and SALDO_FRANQUIA > 0   and codigo_conta ='" & pCodigoConta & "'"
                    End If

                    connection = New OleDbConnection(StrConn)
                    connection.Open()
                    Dim cmd5 As OleDbCommand = connection.CreateCommand
                    cmd5.CommandText = strSQL
                    cmd5.ExecuteNonQuery()
                    cmd5.Dispose()
                    connection.Close()
                    Return "ok"
                Catch ex As Exception
                    Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
                End Try
            End If

        End If
        Return ""
    End Function

    Private Function FranquiaValorTemTipoLigacao(ByVal pCodigoOperadora As Integer, ByVal pCompartilhado As String) As Boolean
        Dim connection As New OleDbConnection(StrConn)
        Dim strSQL As String = ""
        Dim _count As Integer = 0
        strSQL = " select COUNT(*) from vas p1 where p1.franquia='S' and p1.codigo_operadora='" & pCodigoOperadora & "'"
        strSQL = strSQL + " and p1.compartilhado='" & pCompartilhado & "'"
        'strSQL = strSQL + " and p1.valor is not null"
        'strSQL = strSQL + " and p1.minutos is null"
        strSQL = strSQL + "   and EXISTS (select 0 from vas_tarifas p4 where p4.codigo_vas=P1.CODIGO_VAS)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim reader As OleDbDataReader
        cmd.CommandText = strSQL
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _count = reader.Item(0).ToString
            End While
        End Using
        connection.Close()
        cmd.Dispose()

        If _count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Public Function ProcessaAuditFranquiaAtualizar(ByVal listCdr As List(Of cdr), ByVal pCodigoOperadora As Integer) As List(Of cdr)
        Dim listCdr2 As List(Of cdr)
        listCdr2 = listCdr
        Dim ultimoNumero As String = ""
        Dim totalDuracao As Double = 0
        Dim listCdrAtualizar As New List(Of cdr)
        For Each myCDR As cdr In listCdr

            Dim strSQL As String = "select p1.minutos minutos from vas p1, linhas p2, linhas_vas p3"
            strSQL &= " where p1.franquia='S'"
            strSQL &= " and p1.codigo_operadora='" & pCodigoOperadora & "'"
            strSQL &= "and p3.codigo_linha=p2.codigo_linha"
            strSQL &= " and p3.codigo_vas = p1.codigo_vas"
            strSQL &= " and replace(replace(REPLACE(p2.num_linha,')',''),'(',''),'-','') = replace(replace(REPLACE('" & myCDR.RmlNumeroA & "',')',''),'(',''),'-','')"
            'strSQL &= " and p1.tarif_codigo='" & myCDR.Tarif_Codigo & "'"
            strSQL &= " and p1.codigo_vas in(select codigo_vas from vas_tarifas where CODIGO_TIPO_LIGACAO ='" & myCDR.Tipo_ligacao & "' )"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            Dim franquia As Integer = 0
            'connection = New OleDbConnection(strConn)
            connection.Open()
            Dim reader2 As OleDbDataReader
            reader2 = cmd2.ExecuteReader

            'tenta pegar o valor da franquia da linha
            If reader2.HasRows Then
                While reader2.Read
                    franquia = reader2.Item("minutos")
                End While
            End If

            connection.Close()

            'se a linha possui minutos de franquia continua o processo
            If franquia > 0 And ultimoNumero = myCDR.RmlNumeroA Then
                'ultimoNumero = myCDR.RmlNumeroA
                totalDuracao += myCDR.Duracao

                If totalDuracao < franquia And myCDR.ValorCDR > 0 Then
                    'cobrou a ligação dentro da franquia
                    listCdrAtualizar.Add(myCDR)

                End If
            Else
                totalDuracao = 0
            End If
            ultimoNumero = myCDR.RmlNumeroA
        Next
        Return listCdrAtualizar
    End Function

    Private Function ProcessaAuditFranquiaAtualizarTodasLinhas(ByVal listCdr As List(Of cdr), ByVal pCodigoOperadora As Integer) As List(Of cdr)
        Dim listCdr2 As List(Of cdr)
        listCdr2 = listCdr
        Dim listCdrAtualizar As New List(Of cdr)
        Dim ultimoNumero As String = ""
        Dim totalDuracao As Double = 0
        For Each myCDR As cdr In listCdr

            Dim strSQL As String = "select p1.minutos minutos from vas p1"
            strSQL &= " where p1.franquia='S'"
            strSQL &= " and p1.codigo_operadora='" & pCodigoOperadora & "'"
            'strSQL &= " and p1.tarif_codigo='" & myCDR.Tarif_Codigo & "'"
            strSQL &= " and p1.codigo_vas in(select codigo_vas from vas_tarifas where CODIGO_TIPO_LIGACAO ='" & myCDR.Tipo_ligacao & "' )"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            Dim franquia As Integer = 0
            Dim tarifa As Integer = -1
            If ultimoNumero = "" Then
                ultimoNumero = myCDR.RmlNumeroA
            End If
            'connection = New OleDbConnection(strConn)
            connection.Open()
            Dim reader2 As OleDbDataReader
            reader2 = cmd2.ExecuteReader

            'tenta pegar o valor da franquia da linha

            While reader2.Read
                franquia = reader2.Item("minutos")

            End While

            connection.Close()

            'se a linha possui minutos de franquia continua o processo
            If franquia > 0 And ultimoNumero = myCDR.RmlNumeroA Then
                'ultimoNumero = myCDR.RmlNumeroA
                totalDuracao += myCDR.Duracao

                If totalDuracao < franquia And myCDR.ValorCDR > 0 Then
                    'cobrou a ligação dentro da franquia
                    listCdrAtualizar.Add(myCDR)

                End If
            Else
                totalDuracao = 0
            End If
            ultimoNumero = myCDR.RmlNumeroA
        Next
        Return listCdrAtualizar
    End Function

    Public Function UpdateAuditFranquias(ByVal listCDR As List(Of cdr)) As String


        For Each myCdr As cdr In listCDR
            Try
                Dim strSQL As String = ""
                strSQL = "update cdrs_celular set valor_audit='0', valor_ok='3' where codigo='" & myCdr.CdrCodigo & "'"

                Dim connection As New OleDbConnection(StrConn)
                Dim cmd As OleDbCommand = connection.CreateCommand
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
                cmd.Dispose()
                Return "ok"
            Catch ex As Exception
                'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
                '(myLog)
                Return "Erro Função:[UpdateAuditFranquias]: " & ex.Message
            End Try
        Next
        Return True
    End Function

    ''' <summary>
    ''' Este método zera a coluna auditoria nas chama q possuem valor zero e são do tipo 3 (chamadas)
    ''' </summary>
    ''' <param name="pcodigoOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <param name="pListarifas"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateChamadasZeradas(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pListarifas As String) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set valor_audit=0"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND valor_cdr=0 and cdr_codigo='3'"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:UpdateChamadasZeradas Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateChamadasZeradas]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Atualiza o codigo usuário e grp_codigo na tabela cdrs_celular
    ''' </summary>
    ''' <param name="pCodConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDebito(ByVal pCodConta As Integer) As String
        Dim total As Integer = -1
        Try
            Dim strSQL As String = "update " & tabelCDRS & " set grp_codigo=debtgroup(rml_numero_a), codigo_usuario=debtuser(rml_numero_a) where codigo_conta='" & pCodConta & "'"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            total = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateDebito]: " & ex.Message
        End Try
    End Function


    ''' <summary>
    ''' traz a lista de tarefas se não houver alguma em execução
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getTarefas() As List(Of Tarefa)

        Dim listTarefas As New List(Of Tarefa)

        Try
            Dim strSQL As String = "select codigo,data, status, cod_tarefa, nvl(descricao,'')descricao,nvl(autor,'')autor,obs from gestao_agendamentos_tarefas where nvl(status,0)=0 and not exists (select 0 from gestao_agendamentos_tarefas where nvl(status,0)=1)"
            'Dim strSQL As String = "select codigo,data, status, cod_tarefa, nvl(descricao,'')descricao,nvl(autor,'')autor,obs from gestao_agendamentos_tarefas where nvl(status,0)=0 "
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _tarefa As New Tarefa
                    _tarefa.Codigo = reader.Item("codigo").ToString
                    _tarefa.Data = reader.Item("data").ToString
                    _tarefa.Status = reader.Item("status").ToString
                    _tarefa.Codtarefa = reader.Item("cod_tarefa").ToString
                    _tarefa.Descricao = reader.Item("Descricao").ToString
                    _tarefa.Autor = reader.Item("autor").ToString
                    _tarefa.OBS = reader.Item("OBS").ToString
                    '_tarefa.InicioTarefa = reader.Item("inicio_tarefa")
                    listTarefas.Add(_tarefa)
                End While
            End Using
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            ' Return Nothing
        End Try
        Return listTarefas
    End Function


    Public Function getTarefasFaturas(ByVal pCodigotarefa As Integer) As List(Of Fatura)

        Dim listFaturas As New List(Of Fatura)

        Try
            Dim strSQL As String = "select distinct p1.codigo_fatura, p2.descricao, p2.CODIGO_OPERADORA, p2.CODIGO_TIPO, p4.codigo_conta, to_char(p2.dt_vencimento,'DD/MM/YYYY')vencimento from gestao_tarefas_faturas p1, faturas p2,gestao_agendamentos_tarefas p3, faturas_arquivos p4 where p1.codigo_fatura=p2.codigo_fatura and nvl(p3.status,0)=0 and p2.codigo_fatura=p4.codigo_fatura and p1.codigo_tarefa='" & pCodigotarefa & "'"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _fatura As New Fatura
                    _fatura.ID = reader.Item("codigo_fatura").ToString
                    _fatura.Fatura = reader.Item("descricao").ToString
                    _fatura.CodigoConta = reader.Item("codigo_conta").ToString
                    _fatura.CodigoOperadora = reader.Item("CODIGO_OPERADORA").ToString
                    _fatura.CodigoTipo = reader.Item("CODIGO_TIPO").ToString
                    _fatura.DTVencimento = reader.Item("vencimento").ToString
                    listFaturas.Add(_fatura)
                End While
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return listFaturas
    End Function


    'LOG da Carga
    Public Function InsereLOG(ByVal pfatura As Fatura, ByVal ptarefa As Tarefa, ByVal pTexto As String, ByVal erro As String) As String
        Try
            Dim strSQL As String = "INSERT into  gestao_agendamentos_log (CODIGO,data,descricao,autor,erro,codigo_tarefa,codigo_fatura,desc_fatura,codigo_operadora,vencimento) values ((SELECT NVL(MAX(CODIGO),0)+1 FROM gestao_agendamentos_log),sysdate, '" & pTexto & "','" & ptarefa.Autor & "','" & erro & "','" & ptarefa.Codigo & "','" & pfatura.ID & "','" & pfatura.Fatura & "','" & pfatura.CodigoOperadora & "',to_date('" & IIf(pfatura.DTVencimento.Day < 10, "0" & pfatura.DTVencimento.Day, pfatura.DTVencimento.Day) & "/" & IIf(pfatura.DTVencimento.Month < 10, "0" & pfatura.DTVencimento.Month, pfatura.DTVencimento.Month) & "/" & pfatura.DTVencimento.Year & "','DD/MM/YYYY')) "
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[InsereLOG]: " & ex.Message
        End Try
    End Function

    ''' <summary>
    ''' Atualiza o status do agendamento
    ''' </summary>
    ''' <param name="ptarefa"></param>
    ''' <param name="Status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AtualizaStatusAgendamento(ByVal ptarefa As Tarefa, ByVal Status As String, ByVal inicio As Boolean) As String
        Try
            Dim strSQL As String = "update gestao_agendamentos_tarefas set status='" & Status & "' "
            If inicio Then
                strSQL = strSQL + " ,inicio_tarefa=sysdate "
            Else
                strSQL = strSQL + " ,fim_tarefa=sysdate "
            End If
            strSQL = strSQL + " where codigo= '" & ptarefa.Codigo & "'"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[AtualizaStatusAgendamento]: " & ex.Message
        End Try
    End Function


    Public Function GetFaturasByOperadoraVencimento(ByVal pCodigoOperadora As Integer, ByVal pVencimento As String, ByVal pTipo As String) As List(Of Fatura)

        Dim listFaturas As New List(Of Fatura)

        Try
            Dim strSQL As String = "select p2.codigo_fatura, p2.descricao, p2.CODIGO_OPERADORA, p2.CODIGO_TIPO, to_char(p2.dt_vencimento,'DD/MM/YYYY')vencimento from  faturas p2 "

            If pCodigoOperadora <> -1 Then
                strSQL = strSQL + "where p2.codigo_operadora='" & pCodigoOperadora & "'"
            End If

            If Not String.IsNullOrEmpty(pVencimento) Then
                strSQL = strSQL + " and to_char(p2.dt_vencimento,'MM/YYYY')= '" & pVencimento & "'"
            End If

            If Not String.IsNullOrEmpty(pTipo) Then
                strSQL = strSQL + " and p2.CODIGO_TIPO= '" & pTipo & "'"
            End If

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _fatura As New Fatura
                    _fatura.ID = reader.Item("codigo_fatura").ToString
                    _fatura.Fatura = reader.Item("descricao").ToString & "(" & reader.Item("codigo_fatura").ToString & ")"
                    listFaturas.Add(_fatura)
                End While
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return listFaturas
    End Function

    ''' <summary>
    ''' Insere o agendamento na tabela
    ''' </summary>
    ''' <param name="ptarefa"></param>
    ''' <param name="Status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsereAgendamento(ByVal ptarefa As Tarefa, ByVal Status As String) As String
        Dim codigo_agendatento As Integer = GetMaxCodigoAgendamento()
        Try
            Dim strSQL As String = "insert into gestao_agendamentos_tarefas (codigo,data,descricao,autor,status,cod_tarefa) values ('" & codigo_agendatento & "',sysdate,'" & ptarefa.Descricao & "','" & ptarefa.Autor & "','" & Status & "','" & ptarefa.Codtarefa & "') "

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            'insere as faturas
            For Each _fatura As Fatura In ptarefa.Faturas

                'antes de inserir verifica se já não está agendada
                If VerificaFaturaJaAgendada(ptarefa.Codtarefa, _fatura.ID) = False Then
                    strSQL = "insert into gestao_tarefas_faturas (codigo_tarefa,codigo_fatura,ativo) values ('" & codigo_agendatento & "','" & _fatura.ID & "','S') "
                    Dim cmd2 As OleDbCommand = connection.CreateCommand
                    cmd2.CommandText = strSQL
                    connection.Open()
                    registrosAfetados = cmd2.ExecuteNonQuery()
                    connection.Close()
                    cmd2.Dispose()
                End If
            Next


            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[InsereAgendamento]: " & ex.Message
        End Try
    End Function


    Public Function VerificaFaturaJaAgendada(ByVal pTipo As String, ByVal pCodigoFatura As Integer) As Boolean

        Dim _result As Boolean = False

        Try
            Dim strSQL As String = "select 0 from gestao_agendamentos_tarefas p1, gestao_tarefas_faturas p2 "
            strSQL = strSQL + " where p1.codigo=p2.codigo_tarefa and p1.cod_tarefa='" & pTipo & "' and p2.codigo_fatura='" & pCodigoFatura & "' and p1.status=0 "

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                If reader.HasRows Then
                    _result = True
                End If
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return _result
    End Function



    Public Function GetMaxCodigoAgendamento() As Integer
        Dim result As Integer = 0
        Try
            Dim strSQL As String = "select nvl(max(codigo),0)+1 from gestao_agendamentos_tarefas"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    result = reader.Item(0).ToString
                End While
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
            Return -1
        End Try
        Return result
    End Function


    Public Function AtualizaRelatorios() As String

        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            strSQL = "DBMS_MVIEW.REFRESH('CDRS_CELULAR_ANALITICO_MV','C')"
            Dim AtualizaView As String = ConfigurationManager.AppSettings("AtualizaView")
            If AtualizaView = "3" Then
                strSQL = "atualiza_relatorios"
            End If

            Dim connection As New OleDbConnection(StrConn)
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


    Public Function getValorFranquia(ByVal pCodigoOperadora As Integer, ByVal pTipo As String, ByVal pCompartilhado As String) As Double

        'Dim listFaturas As New List(Of Fatura)
        Dim valor As Double = 0
        Try
            Dim strSQL As String = "select "

            If pTipo.ToUpper = "VALOR" Then
                strSQL = strSQL + " sum(P1.VALOR)valor  "
            Else
                strSQL = strSQL + " sum(P1.MINUTOS)valor  "
            End If

            strSQL = strSQL + " from vas p1"
            strSQL = strSQL + " where p1.franquia='S' "
            strSQL = strSQL + " and p1.codigo_operadora='" & pCodigoOperadora & "'"
            If pCompartilhado.ToUpper = "C" Then
                strSQL = strSQL + " and p1.compartilhado='C'"
            Else
                strSQL = strSQL + " and p1.compartilhado='I'"
            End If
            If pTipo.ToUpper = "VALOR" Then
                strSQL = strSQL + " and p1.valor is not null"
                strSQL = strSQL + " and p1.minutos is null"
            Else
                strSQL = strSQL + " and p1.valor is  null"
                strSQL = strSQL + " and p1.minutos is not null"

            End If


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    valor = reader.Item("valor").ToString
                End While
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return valor
    End Function

    Public Function updateSaldoFranquiaVAlor(ByVal pCodigoOperadora As Integer, ByVal pcodigoConta As String) As Integer

        'Dim listFaturas As New List(Of Fatura)
        Dim result As Integer = 0
        Try
            Dim strSQL As String = "UPDATE CDRS_CELULAR P2 SET P2.SALDO_FRANQUIA=(select first_value(p3.SALDO_FRANQUIA) OVER (order by data_inicio desc rows 1 preceding) from CDRS_CELULAR p3 where p3.data_inicio>p2.data_inicio and p2.codigo_conta='" & pcodigoConta & "' )-(select nvl(sum(p1.valor_audit),0)      from cdrs_celular p1 "
            strSQL = strSQL + " where p1.codigo_conta='" & pcodigoConta & "'"
            strSQL = strSQL + " and codigo<=P2.CODIGO and p1.codigo_tipo_ligacao in(select p4.codigo_tipo_ligacao from vas_tarifas p4 where p4.codigo_vas in(select p5.codigo_vas from vas p5 "
            strSQL = strSQL + " where p5.franquia='S'"
            strSQL = strSQL + " and p5.codigo_operadora='" & pCodigoOperadora & "'"
            strSQL = strSQL + " and p5.compartilhado='C'"
            strSQL = strSQL + " and p5.valor is not null"
            strSQL = strSQL + " and p5.minutos is null))) "
            strSQL = strSQL + " where p2.codigo_conta='" & pcodigoConta & "'"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            connection.Open()
            cmd2.ExecuteNonQuery()
            connection.Close()
            cmd2.Dispose()
            result = 1

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return result
    End Function

    Public Function AplicaImpostoValor(ByVal pcodigoConta As String) As Integer

        'Dim listFaturas As New List(Of Fatura)
        Dim result As Integer = 0
        Try
            Dim strSQL As String = "UPDATE CDRS_CELULAR P2 SET p2.valor_audit=AplicaImposto(p2.rml_numero_a,p2.valor_audit)"
            strSQL = strSQL + " where p2.codigo_conta='" & pcodigoConta & "' and p2.cdr_codigo=3"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            connection.Open()
            cmd2.ExecuteNonQuery()
            connection.Close()
            cmd2.Dispose()
            result = 1

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            'Return Nothing
        End Try
        Return result
    End Function

    Public Function ZeraAuditoria(ByVal pCodigoConta As Integer) As String


        Try
            Dim strSQL As String = ""

            strSQL = "update cdrs_celular set valor_audit=0,valor_ok=null,intragrupo=null,obs='' where codigo_conta='" & pCodigoConta & "'"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função TarifaGestaoDDI ", Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[ZeraAuditoria]: " & ex.Message
        End Try
    End Function

    Public Function FaturaLOG(ByVal _fatura As Fatura, ByVal _tarefa As Tarefa) As String


        Try
            Dim strSQL As String = ""

            strSQL = "select faturalog('" & _tarefa.Autor & "','D','" & _fatura.ID & "') from dual"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função TarifaGestaoDDI ", Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[FaturaLOG]: " & ex.Message
        End Try
    End Function


    Public Function ApagarFatura(ByVal pCodigoFatura As Integer) As String
        Try
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim registrosAfetados As Integer
            Dim strSQL As String = "delete from cdrs_celular where codigo_conta in (select codigo_conta from faturas_arquivos where codigo_fatura='" & pCodigoFatura & "')"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            'cmd.Dispose()

            'apaga da cdrs_resumo
            strSQL = "delete from cdrs_celular_resumo where codigo_conta in (select codigo_conta from faturas_arquivos where codigo_fatura='" & pCodigoFatura & "')"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()

            'apaga da rateio_faturas
            strSQL = "delete from rateio_faturas where codigo_fatura='" & pCodigoFatura & "'"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()

            'apaga da rateio_faturas
            strSQL = "delete from gestao_tarefas_faturas where codigo_fatura='" & pCodigoFatura & "'"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()

            'strSQL = "delete from gestao_agendamentos_log where codigo_fatura='" & pCodigoFatura & "'"
            'cmd.CommandText = strSQL
            'connection.Open()
            'registrosAfetados = cmd.ExecuteNonQuery()
            'connection.Close()


            'apaga da fatura_arquivos
            strSQL = "delete from faturas_arquivos where codigo_fatura='" & pCodigoFatura & "'"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()

            'apaga da faturas
            strSQL = "delete from faturas where codigo_fatura ='" & pCodigoFatura & "'"
            cmd.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()


            cmd.Dispose()
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função: UpdateClassificacaoServicos-Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[ApagarFatura]: " & ex.Message
        End Try
    End Function


    Public Function AtualizaDebito(ByVal pCodigoFatura As Integer) As String


        Try
            Dim strSQL As String = ""

            strSQL = "update cdrs_celular set grp_codigo=debtgroup(rml_numero_a), codigo_usuario=debtuser(rml_numero_a) where codigo_conta in (select codigo_conta from faturas_arquivos where codigo_fatura ='" & pCodigoFatura & "')"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função TarifaGestaoDDI ", Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[AtualizaDebito]: " & ex.Message
        End Try
    End Function


    Public Function getAgendamentos() As List(Of AppAgendamentosTipos)

        Dim _list As New List(Of AppAgendamentosTipos)

        Try
            Dim strSQL As String = "SELECT CODIGO, DESCRICAO FROM GESTAO_AGENDAMENTOS_TIPOS order by codigo"
            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _registro As New AppAgendamentosTipos(reader.Item("codigo").ToString, reader.Item("DESCRICAO").ToString)
                    _list.Add(_registro)
                End While
            End Using
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            'Dim myLog = New Log("Erro: " & ex.Message & " - Função ProcessaAuditFranquias ", Date.Now)
            'GeraArquivoLog(myLog)
            'Return "Erro Função:[ProcessaAuditFranquias]: " & ex.Message
            ' Return Nothing
        End Try
        Return _list
    End Function

    Public Function DeleteAgendamentosExecução() As String
        Dim connection As New OleDbConnection(StrConn)
        Try
            Dim strSQL As String = "delete from gestao_agendamentos_log p1 where p1.codigo_tarefa in( select t.codigo from gestao_agendamentos_tarefas t where t.status<2) "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim registrosAfetados As Integer
            registrosAfetados = cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            strSQL = "delete from gestao_tarefas_faturas p1 where p1.codigo_tarefa in( select t.codigo from gestao_agendamentos_tarefas t where t.status<2)"
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd2.ExecuteNonQuery()
            connection.Close()
            cmd2.Dispose()

            strSQL = "delete from gestao_agendamentos_tarefas t where t.status<2"
            Dim cmd3 As OleDbCommand = connection.CreateCommand
            cmd3.CommandText = strSQL
            connection.Open()
            registrosAfetados = cmd3.ExecuteNonQuery()
            connection.Close()
            cmd3.Dispose()



            Return "ok"
        Catch ex As Exception
            connection.Close()
            'Dim myLog = New Log("Função: UpdateClassificacaoServicos-Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[UpdateAgendamentosExecução]: " & ex.Message
        End Try
    End Function




End Class
