Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Contratos
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

    Public Function GetContractById(ByVal pcodigo As String) As AppContrato
        Dim connection As New OleDbConnection(strConn)
        Dim _registro As New AppContrato

        Dim strSQL As String = "select t.CODIGO"
        strSQL = strSQL + ", nvl(t.NUM_CONT_CLIENT, '') AS NUM_CONT_CLIENT"
        strSQL = strSQL + ", nvl(t.NUM_CONT_OP, '') AS NUM_CONT_OP"
        strSQL = strSQL + ", nvl(t.TIPO_CONTRATO, 0) AS TIPO_CONTRATO"
        strSQL = strSQL + ", nvl(t.DATA_CONTRATACAO, '') AS DATA_CONTRATACAO"
        strSQL = strSQL + ", nvl(t.NUMERO_OC, '') AS NUMERO_OC"
        strSQL = strSQL + ", nvl(t.VALOR_INSTALACAO, '') AS VALOR_INSTALACAO"
        strSQL = strSQL + ", nvl(t.VALOR_MENSAL, '') AS VALOR_MENSAL"
        strSQL = strSQL + ", nvl(t.PRAZO_INST, '') AS PRAZO_INST"
        strSQL = strSQL + ", nvl(t.DATA_ATIV,'') AS DATA_ATIV"
        strSQL = strSQL + ", nvl(t.DATA_DESAT, '') AS DATA_DESAT"
        strSQL = strSQL + ", nvl(t.MULTA_RECISAO, '') AS MULTA_RECISAO"
        strSQL = strSQL + ", nvl(t.NUMERO_FATURA, '') AS NUMERO_FATURA"
        strSQL = strSQL + ", nvl(t.END_ENVIO_FAT, '') AS END_ENVIO_FAT"
        strSQL = strSQL + ", nvl(t.DATA_PGTO_FAT, '') AS DATA_PGTO_FAT"
        strSQL = strSQL + ", nvl(t.RESPONSAVEL, '') AS RESPONSAVEL"
        strSQL = strSQL + ", nvl(t.PERIODO, '') AS PERIODO"
        strSQL = strSQL + " from CONTRATOS t where CODIGO='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                _registro.codigo = reader.Item("CODIGO").ToString
                _registro.num_cont_client = reader.Item("NUM_CONT_CLIENT").ToString
                _registro.num_cont_op = reader.Item("NUM_CONT_OP").ToString
                _registro.tipo_contrato = reader.Item("TIPO_CONTRATO").ToString
                _registro.data_contratacao = reader.Item("DATA_CONTRATACAO").ToString
                _registro.numero_oc = reader.Item("NUMERO_OC").ToString
                _registro.valor_instalacao = reader.Item("VALOR_INSTALACAO").ToString
                _registro.valor_mensal = reader.Item("VALOR_MENSAL").ToString
                _registro.prazo_inst = reader.Item("PRAZO_INST").ToString
                _registro.data_ativ = reader.Item("DATA_ATIV").ToString
                _registro.data_desat = reader.Item("DATA_DESAT").ToString
                _registro.multa_recisao = reader.Item("MULTA_RECISAO").ToString
                _registro.num_fatura = reader.Item("NUMERO_FATURA").ToString
                _registro.end_envio_fat = reader.Item("END_ENVIO_FAT").ToString
                _registro.data_pgto_fat = reader.Item("DATA_PGTO_FAT").ToString
                _registro.responsavel = reader.Item("RESPONSAVEL").ToString
                _registro.periodo = reader.Item("PERIODO").ToString

            End While
        End Using

        Return _registro
    End Function


    Public Function InsereContrato(ByVal contrato As AppContrato, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = "insert into Contratos(CODIGO,NUM_CONT_CLIENT,NUM_CONT_OP,TIPO_CONTRATO,DATA_CONTRATACAO"
            strSQL = strSQL + ", NUMERO_OC,VALOR_INSTALACAO, VALOR_MENSAL, PRAZO_INST, DATA_ATIV, DATA_DESAT "
            strSQL = strSQL + ", MULTA_RECISAO,NUMERO_FATURA, END_ENVIO_FAT, DATA_PGTO_FAT, RESPONSAVEL,PERIODO) "
            strSQL = strSQL + " values ((select nvl(max(CODIGO),0)+1 from Contratos)"
            strSQL = strSQL + ",'" + contrato.num_cont_client + "'"
            strSQL = strSQL + ",'" + contrato.num_cont_op + "'"
            strSQL = strSQL + ",'" + contrato.tipo_contrato + "'"
            strSQL = strSQL + ", to_date('" + contrato.data_contratacao + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + contrato.numero_oc + "'"
            strSQL = strSQL + ",'" + contrato.valor_instalacao + "'"
            strSQL = strSQL + ",'" + contrato.valor_mensal + "'"
            strSQL = strSQL + ", to_date('" + contrato.prazo_inst + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", to_date('" + contrato.data_ativ + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", to_date('" + contrato.data_desat + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + contrato.multa_recisao + "'"
            strSQL = strSQL + ",'" + contrato.num_fatura + "'"
            strSQL = strSQL + ",'" + contrato.end_envio_fat + "'"
            strSQL = strSQL + ", to_date('" + contrato.data_pgto_fat + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + contrato.responsavel + "'"
            strSQL = strSQL + ",'" + contrato.periodo + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("(select nvl(max(CODIGO),0) from Contratos)", "N", usuario)
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

    Public Function AtualizaContrato(ByVal contrato As AppContrato, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = String_log("'" & contrato.codigo & "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update Contratos set "
            strSQL = strSQL + " CODIGO='" + contrato.codigo + "',"
            strSQL = strSQL + " NUM_CONT_CLIENT='" + contrato.num_cont_client + "',"
            strSQL = strSQL + " NUM_CONT_OP='" + contrato.num_cont_op + "',"
            strSQL = strSQL + " NUMERO_OC='" + contrato.numero_oc + "',"
            strSQL = strSQL + " TIPO_CONTRATO='" + contrato.tipo_contrato + "',"
            strSQL = strSQL + " DATA_CONTRATACAO= to_date('" + contrato.data_contratacao + "','dd/mm/yyyy hh24:mi:ss'),"
            strSQL = strSQL + " VALOR_INSTALACAO='" + contrato.valor_instalacao + "',"
            strSQL = strSQL + " VALOR_MENSAL='" + contrato.valor_mensal + "',"
            strSQL = strSQL + " PRAZO_INST= to_date('" + contrato.prazo_inst + "','dd/mm/yyyy hh24:mi:ss'),"
            strSQL = strSQL + " DATA_ATIV= to_date('" + contrato.data_ativ + "','dd/mm/yyyy hh24:mi:ss'),"
            strSQL = strSQL + " DATA_DESAT= to_date('" + contrato.data_desat + "','dd/mm/yyyy hh24:mi:ss'),"
            strSQL = strSQL + " MULTA_RECISAO='" + contrato.multa_recisao + "',"
            strSQL = strSQL + " NUMERO_FATURA='" + contrato.num_fatura + "',"
            strSQL = strSQL + " END_ENVIO_FAT='" + contrato.end_envio_fat + "',"
            strSQL = strSQL + " DATA_PGTO_FAT= to_date('" + contrato.data_pgto_fat + "','dd/mm/yyyy hh24:mi:ss'),"
            strSQL = strSQL + " PERIODO='" + contrato.periodo + "',"
            strSQL = strSQL + " RESPONSAVEL='" + contrato.responsavel + "'"

            strSQL = strSQL + " where CODIGO = '" + contrato.codigo + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" & contrato.codigo & "'", "B", usuario)
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

    Public Function ExcluiContrato(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            Dim strSQL As String = String_log("'" & pcodigo & "'", "D", usuario)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()
            
            strSQL = "delete CONTRATOS "
            strSQL = strSQL + "where CODIGO = '" & pcodigo & "'"

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


    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into contratos_log (codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + "CODIGO,NUM_CONT_CLIENT,NUM_CONT_OP "
        sql = sql + " ,TIPO_CONTRATO,DATA_CONTRATACAO,NUMERO_OC,VALOR_INSTALACAO,VALOR_MENSAL,PRAZO_INST "
        sql = sql + " ,DATA_ATIV,DATA_DESAT,MULTA_RECISAO,NUMERO_FATURA,END_ENVIO_FAT,DATA_PGTO_FAT,RESPONSAVEL,PERIODO )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from contratos_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + "" + pcodigo + ","
            sql = sql + " (select NUM_CONT_CLIENT from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select NUM_CONT_OP from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select TIPO_CONTRATO from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select DATA_CONTRATACAO from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select NUMERO_OC from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select VALOR_INSTALACAO from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select VALOR_MENSAL from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select PRAZO_INST from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select DATA_ATIV from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select DATA_DESAT from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select MULTA_RECISAO from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select NUMERO_FATURA from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select END_ENVIO_FAT from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select DATA_PGTO_FAT from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select RESPONSAVEL from contratos where CODIGO=" + pcodigo + "),"
            sql = sql + " (select PERIODO from contratos where CODIGO=" + pcodigo + "))"
        End If
        Return sql

    End Function


End Class
