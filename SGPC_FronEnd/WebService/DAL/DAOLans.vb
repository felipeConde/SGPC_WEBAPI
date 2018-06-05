Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAOLans

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


    Public Function InsereLan(ByVal Lan As AppLans) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into LANS(CODIGO_LAN"
            strSQL = strSQL + ",  NOME,IP, MASCARA, RANGE, REDE, BROADCAST, RANGE_2) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO_LAN),0)+1 from LanS)"
            strSQL = strSQL + ",'" + Lan.Nome_Lan + "'"
            strSQL = strSQL + ",'" + Lan.Ip + "'"
            strSQL = strSQL + ",'" + Lan.Mascara + "'"
            strSQL = strSQL + ",'" + Lan.Range + "'"
            strSQL = strSQL + ",'" + Lan.Rede + "'"
            strSQL = strSQL + ",'" + Lan.Broadcast + "'"
            strSQL = strSQL + ",'" + Lan.Range_2 + "'"
            strSQL = strSQL + ")"

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

    Public Function AtualizaLan(ByVal pLans As AppLans) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update LanS set "
            strSQL = strSQL + "REDE='" + pLans.Rede + "',"
            strSQL = strSQL + "NOME='" + pLans.Nome_Lan + "',"
            strSQL = strSQL + "IP='" + pLans.Ip + "',"
            strSQL = strSQL + "BROADCAST='" + pLans.Broadcast + "',"
            strSQL = strSQL + "MASCARA='" + pLans.Mascara + "',"
            strSQL = strSQL + "RANGE='" + pLans.Range + "',"
            strSQL = strSQL + "RANGE_2='" + pLans.Range_2 + "'"

            strSQL = strSQL + " where CODIGO_Lan = '" + pLans.Codigo_Lan + "' "

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

    Public Function ExcluiLan(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete LANS "
            strSQL = strSQL + "where CODIGO_LAN = " + Convert.ToString(pcodigo)

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

    Public Function GetLanById(ByVal pcodigo As Integer) As List(Of AppLans)
        Dim connection As New OleDbConnection(strConn)
        Dim listLink As New List(Of AppLans)

        Dim strSQL As String = "select CODIGO_LAN"
        strSQL = strSQL + ", nvl(NOME, '') AS NOME_LAN"
        strSQL = strSQL + ", nvl(REDE, '') AS REDE"
        strSQL = strSQL + ", nvl(IP, '') AS IP"
        strSQL = strSQL + ", nvl(BROADCAST, '') AS BROADCAST"
        strSQL = strSQL + ", nvl(MASCARA, '') AS MASCARA"
        strSQL = strSQL + ", nvl(RANGE, '') AS RANGE"
        strSQL = strSQL + ", nvl(RANGE_2, '') AS RANGE_2"
        strSQL = strSQL + " FROM LANS "
        strSQL = strSQL + " WHERE CODIGO_Lan ='" + pcodigo.ToString() + "' "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLans(reader.Item("CODIGO_LAN").ToString, reader.Item("NOME_LAN").ToString, reader.Item("REDE").ToString, reader.Item("IP").ToString, reader.Item("BROADCAST").ToString, reader.Item("MASCARA").ToString, reader.Item("RANGE").ToString, reader.Item("RANGE_2").ToString)
                listLink.Add(_registro)
            End While
        End Using

        Return listLink
    End Function

End Class
