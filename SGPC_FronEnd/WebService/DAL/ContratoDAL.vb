Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class ContratoDAL

    Private _strConn As String = ""
    'Private strConn As String = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=cl;"
    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Public Function Insert(ByVal pnumContrato As String, ByVal pEmpresa As String, ByVal pCodigo_Fornecedor As Integer, ByVal pObjeto As String, ByVal pMulta As String, ByVal pDesconto As Double, ByVal pPeriodicidade As String, ByVal pdata_Assinatura As Date, ByVal pInicio As Date, ByVal pReajuste As String, ByVal Pdata_reajuste As Date, ByVal pStatus As String, ByVal pVencimento As Date, ByVal pPossui_Anexo As String, ByVal pPossui_alerta As String, ByVal pAlerta As Date, ByVal pValor_Contrato As Double, ByVal pAssinatura As String, ByVal pCodigo_responsavel As Integer, ByVal pOBS As String, ByVal pSLA As String, ByVal pAnexos As String, ByVal pCopia_Contrato As String) As String

        Try
            Dim strSQL As String = "insert into contratos(CODIGO,NUM_CONTRATO,EMPRESA,CODIGO_FORNECEDOR,OBJETO,MULTAS,DESCONTOS,PERIODICIDADE,DATA_ASSINATURA,DATA_INICIO,INDICE_REAJUSTE,DATA_REAJUSTE,STATUS,DATA_VENCIMENTO,POSSUI_ANEXO,POSSUI_ALERTA,DATA_ALERTA,VALOR_CONTRATO,ASSINATURA,CODIGO_RESPONSAVEL,OBS,SLA,ANEXOS,COPIA_CONTRATO) "
            strSQL = strSQL & " values ((select nvl(max(codigo)+1,1) from contratos),'" & pnumContrato & "','" & pEmpresa & "','" & pCodigo_Fornecedor & "','" & pObjeto & "','" & pMulta & "','" & pDesconto & "','" & pPeriodicidade & "',to_date('" & pdata_Assinatura & "','DD/MM/YYYY'),to_date('" & pInicio & "','DD/MM/YYYY'),'" & pReajuste & "',to_date('" & Pdata_reajuste & "','DD/MM/YYYY'),'" & pStatus & "',to_date('" & pVencimento & "','DD/MM/YYYY'),'" & pPossui_Anexo & "','" & pPossui_alerta & "',to_date('" & pAlerta & "','DD/MM/YYYY'),'" & pValor_Contrato & "','" & pAssinatura & "','" & pCodigo_responsavel & "','" & pOBS & "','" & pSLA & "','" & pAnexos & "','" & pCopia_Contrato & "')"
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

    Public Function Update(ByVal pCodigo As Integer, ByVal pNum_contrato As String, ByVal pEmpresa As String, ByVal pCodigo_Fornecedor As Integer, ByVal pObjeto As String, ByVal pMulta As String, ByVal pDesconto As Double, ByVal pPeriodicidade As String, ByVal pdata_Assinatura As Date, ByVal pInicio As Date, ByVal pReajuste As String, ByVal Pdata_reajuste As Date, ByVal pStatus As String, ByVal pVencimento As Date, ByVal pPossui_Anexo As String, ByVal pPossui_alerta As String, ByVal pAlerta As Date, ByVal pValor_Contrato As Double, ByVal pAssinatura As String, ByVal pCodigo_responsavel As Integer, ByVal pOBS As String, ByVal pSLA As String, ByVal pAnexos As String, ByVal pCopia_Contrato As String) As String

        Try
            Dim strSQL As String = "update contratos set "
            strSQL = strSQL & "NUM_CONTRATO = '" & pNum_contrato & "',"
            strSQL = strSQL & "EMPRESA = '" & pEmpresa & "',"
            strSQL = strSQL & "CODIGO_FORNECEDOR = '" & pCodigo_Fornecedor & "',"
            strSQL = strSQL & "OBJETO = '" & pObjeto & "',"
            strSQL = strSQL & "MULTAS = '" & pMulta & "',"
            strSQL = strSQL & "DESCONTOS = '" & pDesconto & "',"
            strSQL = strSQL & "PERIODICIDADE = '" & pPeriodicidade & "',"
            strSQL = strSQL & "DATA_ASSINATURA = to_date('" & pdata_Assinatura & "','DD/MM/YYYY'),"
            strSQL = strSQL & "DATA_INICIO = to_date('" & pInicio & "','DD/MM/YYYY'),"
            strSQL = strSQL & "INDICE_REAJUSTE = '" & pReajuste & "',"
            strSQL = strSQL & "DATA_REAJUSTE = to_date('" & Pdata_reajuste & "','DD/MM/YYYY'),"
            strSQL = strSQL & "STATUS = '" & pStatus & "',"
            strSQL = strSQL & "DATA_VENCIMENTO = to_date('" & pVencimento & "','DD/MM/YYYY'),"
            strSQL = strSQL & "POSSUI_ANEXO = '" & pPossui_Anexo & "',"
            strSQL = strSQL & "POSSUI_ALERTA = '" & pPossui_alerta & "',"
            strSQL = strSQL & "DATA_ALERTA = to_date('" & pAlerta & "','DD/MM/YYYY'),"
            strSQL = strSQL & "VALOR_CONTRATO = '" & pValor_Contrato & "',"
            strSQL = strSQL & "ASSINATURA = '" & pAssinatura & "',"
            strSQL = strSQL & "CODIGO_RESPONSAVEL = '" & pCodigo_responsavel & "',"
            strSQL = strSQL & "OBS = '" & pOBS & "',"
            strSQL = strSQL & "SLA = '" & pSLA & "',"
            strSQL = strSQL & "ANEXOS = '" & pAnexos & "',"
            strSQL = strSQL & "COPIA_CONTRATO = '" & pCopia_Contrato & "' "
            strSQL = strSQL & " where CODIGO = '" & pCodigo & "'"

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

    Public Function Delete(ByVal pCodigo As Integer) As String

        Try
            Dim strSQL As String = "delete from contratos "
            strSQL = strSQL & "where CODIGO = '" & pCodigo & "'"

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

    Public Function GetContrato(ByVal pCodigo As Integer) As List(Of Contrato)
        Dim result As Boolean = False
        Dim connection As New OleDbConnection(strConn)
        Dim listContrato As New List(Of Contrato)

        Try
            Dim strSQL As String = "select CODIGO,"
            strSQL = strSQL & "EMPRESA,num_contrato,CODIGO_FORNECEDOR,OBJETO,MULTAS,DESCONTOS,PERIODICIDADE,DATA_ASSINATURA,DATA_INICIO,INDICE_REAJUSTE,DATA_REAJUSTE,STATUS,DATA_VENCIMENTO,POSSUI_ANEXO,POSSUI_ALERTA,DATA_ALERTA,VALOR_CONTRATO,ASSINATURA,CODIGO_RESPONSAVEL,OBS,SLA,ANEXOS,COPIA_CONTRATO "
            strSQL = strSQL & "from contratos "
            strSQL = strSQL & "where CODIGO = '" & pCodigo & "'"

            'Dim connection As New Data.OleDb.o
            Dim cmd As OleDbCommand = connection.CreateCommand
            Dim reader As OleDbDataReader
            cmd.CommandText = strSQL
            connection.Open()
            reader = cmd.ExecuteReader

            Using connection
                While reader.Read
                    Dim _Contrato As New Contrato(reader.Item("CODIGO").ToString, reader.Item("num_contrato").ToString, reader.Item("EMPRESA").ToString, reader.Item("CODIGO_FORNECEDOR").ToString, reader.Item("OBJETO").ToString, reader.Item("MULTAS").ToString, reader.Item("DESCONTOS").ToString, reader.Item("PERIODICIDADE").ToString, reader.Item("DATA_ASSINATURA").ToString, reader.Item("DATA_INICIO").ToString, reader.Item("INDICE_REAJUSTE").ToString, reader.Item("DATA_REAJUSTE").ToString, reader.Item("STATUS").ToString, reader.Item("DATA_VENCIMENTO").ToString, reader.Item("POSSUI_ANEXO").ToString, reader.Item("POSSUI_ALERTA").ToString, reader.Item("DATA_ALERTA").ToString, reader.Item("VALOR_CONTRATO").ToString, reader.Item("ASSINATURA").ToString, reader.Item("CODIGO_RESPONSAVEL").ToString, reader.Item("OBS").ToString, reader.Item("SLA").ToString, reader.Item("ANEXOS").ToString, reader.Item("COPIA_CONTRATO").ToString)
                    listContrato.Add(_Contrato)
                End While
            End Using
            connection.Close()

        Catch ex As Exception
            connection.Close()

        End Try

        Return listContrato
    End Function

End Class
