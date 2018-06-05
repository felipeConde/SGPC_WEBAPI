Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.IO
Imports System

Public Class GestaoDALFixo
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
    ''' Este método verifica se a ligação é para um fixo
    ''' </summary>
    ''' <param name="pCodigoConta"></param>
    ''' <param name="pcodigoOperadora"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ClassificaLigacaoFixo(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            strSQL = "update cdrs_celular set codigo_tipo_Ligacao =CLASSIFICAGESTAOFIXO(replace(rml_numero_a,' ',''),replace(numero_b,' ',''),origem,destino,(select no_id from usuarios_tarifacao where substr(cod_area,1,1) = substr(rml_numero_a,1,1) and upper(no_id) like 'GESTAO%' and rownum<=1)) where codigo_conta=" & pCodigoConta & ""

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:ClassificaLigacaoFixo Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[ClassificaLigacaoFixo]: " & ex.Message
        End Try
    End Function

    Public Function TarifaLigacaoFixoV3(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""

            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set "
            strSQL &= " valor_audit=billingAudit.avaliar_chamada(numero_b,route,1,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and UPPER(no_id) like 'GESTAO%' and rownum<=1), "
            strSQL &= " codigo_tipo_Ligacao, "
            strSQL &= " data_inicio,(data_fim-data_inicio)*24*3600,'" & pcodigoOperadora & "'),"
            'strSQL &= " tarif_codigo_audit=(select codigo_Tarif from tipos_ligacao_teste"
            strSQL &= " tarif_codigo_audit=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and UPPER(no_id) like 'GESTAO%' and rownum<=1)),"
            strSQL &= " tarif_codigo=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and UPPER(no_id) like 'GESTAO%' and rownum<=1))"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND CDR_CODIGO='3'"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:TarifaLigacaoFixoV2 Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[TarifaLigacaoFixoV3]: " & ex.Message
        End Try
    End Function




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
    ''' Este método zera as chamadas que estão dentro da fraquina de minutos
    ''' </summary>
    ''' <param name="pcodigoOperadora"></param>
    ''' <param name="pCodigoConta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessaAuditFranquiasFixo(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set "
            strSQL &= " valor_audit=GESTAOAUDITFRANQUIAFIXO(rml_numero_a,data_inicio,codigo_conta,valor_audit,tarif_codigo_audit)"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND CDR_CODIGO='3' "
            strSQL &= " and exists (SELECT 0 FROM PLANOS_VAS WHERE rownum<2 and CODIGO_PLANO=(SELECT CODIGO_PLANO FROM LINHAS p1 WHERE rownum<2 and replace(replace(REPLACE(p1.num_linha,')',''),'(',''),'-','')=cdrs_celular.rml_numero_a))"


            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:ProcessaAuditFranquiasFixo Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[ProcessaAuditFranquiasFixo]: " & ex.Message
        End Try
    End Function

    Public Function Classifica_0800(ByVal pCodigoConta As Integer, ByVal pOperadora As String, ByVal pTipoTarifacao0800 As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular p1"
            strSQL &= " set "
            strSQL &= " p1.CODIGO_TIPO_LIGACAO=CLASSIFICAGESTAOFIXO0800(p1.rml_numero_a,p1.numero_b,p1.origem,p1.destino,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1),'" & pOperadora & "','" & pTipoTarifacao0800 & "')"
            'strSQL &= " TARIF_CODIGO=get_tarif_cod('" & pCDR.Tipo_ligacao & "') "
            strSQL &= " where codigo_conta='" & pCodigoConta & "' "

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:Classifica_0800 Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[Classifica_0800]: " & ex.Message

        End Try
    End Function


    Public Function TarifaLigacaoFixoV30800(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set "
            'strSQL &= " valor_audit=billingAudit.avaliar_chamada('0'||numero_b,route,0,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and no_id like 'GESTAO%' and rownum<=1), "
            'strSQL &= " valor_audit=billingAudit.avaliar_chamada('0'||numero_b,route,0,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and no_id like 'GESTAO%' and rownum<=1), "
            strSQL &= " valor_audit=billingAudit.avaliar_chamada(numero_b,route,1,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1), "
            strSQL &= " codigo_tipo_Ligacao, "
            strSQL &= " data_inicio,(data_fim-data_inicio)*24*3600,'" & pcodigoOperadora & "'),"
            'strSQL &= " tarif_codigo_audit=(select codigo_Tarif from tipos_ligacao_teste"
            strSQL &= " tarif_codigo=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1)),"
            strSQL &= " tarif_codigo_audit=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1))"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND CDR_CODIGO='3'"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            'Return rowsAffect
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:TarifaLigacaoFixoV30800 Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            'Return -1
            Return "Erro Função:[TarifaLigacaoFixoV30800]: " & ex.Message
        End Try
    End Function

    Public Function AtualizaValorAuditServicos(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pListarifas As String) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set valor_audit=0"
            'strSQL &= " valor_ok=0"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND (tarif_codigo is null or tarif_codigo<1)"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            'Return rowsAffect
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:AtualizaValorAuditServicos Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            'Return -1
            Return "Erro Função:[AtualizaValorAuditServicos]: " & ex.Message
        End Try
    End Function

    Public Function Classifica_3003(ByVal pCodigoConta As Integer, ByVal pOperadora As String, ByVal pTipoTarifacao As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular p1"
            strSQL &= " set "
            strSQL &= " p1.CODIGO_TIPO_LIGACAO=CLASSIFICAGESTAOFIXO4004(p1.rml_numero_a,p1.numero_b,p1.origem,p1.destino,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1),'" & pOperadora & "','" & pTipoTarifacao & "')"
            'strSQL &= " TARIF_CODIGO=get_tarif_cod('" & pCDR.Tipo_ligacao & "') "
            strSQL &= " where codigo_conta='" & pCodigoConta & "' "

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:Classifica_0800 Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            Return "Erro Função:[Classifica_3003]: " & ex.Message

        End Try
    End Function
    Public Function TarifaLigacaoFixo3003(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""
            'strSQL = "update cdrs_celular set categoria_audit =CLASSIFICAGESTAOFIXO(rml_numero_a,numero_b,origem,destino) where codigo_conta=" & pCodigoConta & ""
            strSQL = ""

            strSQL &= "update cdrs_celular"
            strSQL &= " set "
            'strSQL &= " valor_audit=billingAudit.avaliar_chamada('0'||numero_b,route,0,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and no_id like 'GESTAO%' and rownum<=1), "
            'strSQL &= " valor_audit=billingAudit.avaliar_chamada('0'||numero_b,route,0,(select no_id from usuarios_tarifacao where substr(cod_area,1,1)=substr(rml_numero_a,1,1) and no_id like 'GESTAO%' and rownum<=1), "
            strSQL &= " valor_audit=billingAudit.avaliar_chamada(numero_b,route,1,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1), "
            strSQL &= " codigo_tipo_Ligacao, "
            strSQL &= " data_inicio,(data_fim-data_inicio)*24*3600,'" & pcodigoOperadora & "'),"
            'strSQL &= " tarif_codigo_audit=(select codigo_Tarif from tipos_ligacao_teste"
            strSQL &= " tarif_codigo=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1)),"
            strSQL &= " tarif_codigo_audit=get_tarif_cod_Gestao(codigo_tipo_Ligacao,(select no_id from usuarios_tarifacao where UPPER(no_id) like 'GESTAO%' and rownum<=1))"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND CDR_CODIGO='3'"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()

            'Return rowsAffect
            Return "ok"
        Catch ex As Exception
            'Dim myLog = New Log("Função:TarifaLigacaoFixoV30800 Erro: " & ex.Message, Date.Now)
            'GeraArquivoLog(myLog)
            'Return -1
            Return "Erro Função:[TarifaLigacaoFixoV30800]: " & ex.Message
        End Try
    End Function


    Public Function AtualizaLigacoesCSP(ByVal pcodigoOperadora As Integer, ByVal pCodigoConta As Integer, ByVal pListarifas As String) As String
        Try
            Dim rowsAffect As Integer = -1
            Dim strSQL As String = ""

            strSQL = ""
            strSQL &= "update cdrs_celular"
            strSQL &= " set valor_audit=0, tarif_codigo=0,codigo_tipo_ligacao=-1"
            strSQL &= " where codigo_conta='" & pCodigoConta & "' AND (tarif_codigo is null or tarif_codigo<1)"

            Dim connection As New OleDbConnection(StrConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            rowsAffect = cmd.ExecuteNonQuery
            connection.Close()
            cmd.Dispose()
            Return "ok"
        Catch ex As Exception

            Return "Erro Função:[AtualizaLigacoesCSP]: " & ex.Message
        End Try
    End Function


End Class
