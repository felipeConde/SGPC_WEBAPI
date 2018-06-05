Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOWans


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


    Public Function InsereWan(ByVal Wan As AppWans, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into WanS(CODIGO_Wan"
            strSQL = strSQL + ",  nome,wan_matriz, MASCARA, RANGE, concentrador, wan_remota, END_IP_CLIENTE,END_IP_OPERADORA,IP_INICIAL,IP_FINAL,GATEWAY,DNS_1,DNS_2,DOMINIO,REDE ) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO_Wan),0)+1 from WanS)"
            strSQL = strSQL + ",'" + Wan.Nome_Wan + "'"
            strSQL = strSQL + ",'" + Wan.wan_matriz + "'"
            strSQL = strSQL + ",'" + Wan.Mascara + "'"
            strSQL = strSQL + ",'" + Wan.Range + "'"
            strSQL = strSQL + ",'" + Wan.concentrador + "'"
            strSQL = strSQL + ",'" + Wan.wan_remota + "'"
            strSQL = strSQL + ",'" + Wan.End_ip_cliente + "'"
            strSQL = strSQL + ",'" + Wan.End_ip_operadora + "'"
            strSQL = strSQL + ",'" + Wan.Ip_Final + "'"
            strSQL = strSQL + ",'" + Wan.Ip_Inicial + "'"
            strSQL = strSQL + ",'" + Wan.Gateway + "'"
            strSQL = strSQL + ",'" + Wan.DNS_1 + "'"
            strSQL = strSQL + ",'" + Wan.DNS_2 + "'"
            strSQL = strSQL + ",'" + Wan.Dominio + "'"
            strSQL = strSQL + ",'" + Wan.Rede + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("(select nvl(max(CODIGO_WAN),0) from WANS)", "N", usuario)
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

        Return True

    End Function

    Public Function AtualizaWan(ByVal pWans As AppWans, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = String_log("'" & pWans.Codigo_Wan & "'", "A", Usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update WanS set "
            strSQL = strSQL + "nome='" + pWans.Nome_Wan + "',"
            strSQL = strSQL + "concentrador='" + pWans.concentrador + "',"
            strSQL = strSQL + "wan_matriz='" + pWans.wan_matriz + "',"
            strSQL = strSQL + "wan_remota='" + pWans.wan_remota + "',"
            strSQL = strSQL + "MASCARA='" + pWans.Mascara + "',"
            strSQL = strSQL + "END_IP_CLIENTE='" + pWans.End_ip_cliente + "',"
            strSQL = strSQL + "END_IP_OPERADORA='" + pWans.End_ip_operadora + "',"
            strSQL = strSQL + "IP_INICIAL='" + pWans.Ip_Inicial + "',"
            strSQL = strSQL + "IP_FINAL='" + pWans.Ip_Final + "',"
            strSQL = strSQL + "GATEWAY='" + pWans.Gateway + "',"
            strSQL = strSQL + "DNS_1='" + pWans.DNS_1 + "',"
            strSQL = strSQL + "DNS_2='" + pWans.DNS_2 + "',"
            strSQL = strSQL + "DOMINIO='" + pWans.Dominio + "',"
            strSQL = strSQL + "REDE='" + pWans.Rede + "',"
            strSQL = strSQL + "RANGE='" + pWans.Range + "'"

            strSQL = strSQL + " where CODIGO_Wan = '" + pWans.Codigo_Wan + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" & pWans.Codigo_Wan & "'", "B", Usuario)
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

        Return True

    End Function

    Public Function ExcluiWan(ByVal pcodigo As Integer, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = String_log("'" & pcodigo & "'", "D", Usuario)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete WanS "
            strSQL = strSQL + "where CODIGO_Wan = " + Convert.ToString(pcodigo)

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

        Return True
    End Function

    Public Function GetWanById(ByVal pcodigo As Integer) As List(Of AppWans)
        Dim connection As New OleDbConnection(strConn)
        Dim listLink As New List(Of AppWans)

        Dim strSQL As String = "select CODIGO_Wan"
        strSQL = strSQL + ", nvl(nome, '') AS Nome"
        strSQL = strSQL + ", nvl(concentrador, '') AS concentrador"
        strSQL = strSQL + ", nvl(wan_matriz, '') AS wan_matriz"
        strSQL = strSQL + ", nvl(wan_remota, '') AS wan_remota"
        strSQL = strSQL + ", nvl(MASCARA, '') AS MASCARA"
        strSQL = strSQL + ", nvl(RANGE, '') AS RANGE"
        strSQL = strSQL + ", nvl(END_IP_CLIENTE, '') AS END_IP_CLIENTE"
        strSQL = strSQL + ", nvl(END_IP_OPERADORA, '') AS END_IP_OPERADORA"
        strSQL = strSQL + ", nvl(IP_INICIAL, '') AS IP_INICIAL"
        strSQL = strSQL + ", nvl(IP_FINAL, '') AS IP_FINAL"
        strSQL = strSQL + ", nvl(GATEWAY, '') AS GATEWAY"
        strSQL = strSQL + ", nvl(DNS_1, '') AS DNS_1"
        strSQL = strSQL + ", nvl(DNS_2, '') AS DNS_2"
        strSQL = strSQL + ", nvl(DOMINIO, '') AS DOMINIO"
        strSQL = strSQL + ", nvl(REDE, '') AS REDE"
        strSQL = strSQL + " FROM WanS "
        strSQL = strSQL + " WHERE CODIGO_Wan ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppWans(reader.Item("CODIGO_Wan").ToString, reader.Item("Nome").ToString, reader.Item("concentrador").ToString, reader.Item("wan_matriz").ToString, reader.Item("wan_remota").ToString, reader.Item("MASCARA").ToString, reader.Item("RANGE").ToString)

                _registro.End_ip_cliente = reader.Item("END_IP_CLIENTE").ToString
                _registro.End_ip_operadora = reader.Item("END_IP_OPERADORA").ToString
                _registro.DNS_1 = reader.Item("DNS_1").ToString
                _registro.DNS_2 = reader.Item("DNS_2").ToString
                _registro.Ip_Final = reader.Item("IP_FINAL").ToString
                _registro.Ip_Inicial = reader.Item("IP_INICIAL").ToString
                _registro.Dominio = reader.Item("DOMINIO").ToString
                _registro.Gateway = reader.Item("GATEWAY").ToString
                _registro.Rede = reader.Item("REDE").ToString
                listLink.Add(_registro)
            End While
        End Using

        Return listLink
    End Function

    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into wans_log(codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + "CODIGO_WAN,RANGE,CONCENTRADOR "
        sql = sql + ",IP_INICIAL,IP_FINAL,GATEWAY,DNS_1,DNS_2,DOMINIO,NOME "
        sql = sql + " ,WAN_MATRIZ,WAN_REMOTA,MASCARA,END_IP_OPERADORA,REDE,END_IP_CLIENTE )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from wans_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + "" + pcodigo + ","
            sql = sql + " (select RANGE from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select CONCENTRADOR from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select IP_INICIAL from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select IP_FINAL from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select GATEWAY from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select DNS_1 from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select DNS_2 from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select DOMINIO from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select NOME from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select WAN_MATRIZ from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select WAN_REMOTA from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select MASCARA from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select END_IP_OPERADORA from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select REDE from WANS where CODIGO_WAN=" + pcodigo + "),"
            sql = sql + " (select END_IP_CLIENTE from WANS where CODIGO_WAN=" + pcodigo + "))"
        End If
        Return sql
    End Function

End Class
