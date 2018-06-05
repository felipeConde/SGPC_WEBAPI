Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_TarifacaoRota

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

    Public Function getComboTarifas() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""
        Dim list As New List(Of AppGeneric)

        strSQL = " "
        strSQL = strSQL + " select distinct p1.codigo,"
        strSQL = strSQL + " p1.descricao,"
        strSQL = strSQL + " nvl(p1.complemento, ' ') complemento,"
        strSQL = strSQL + " p2.OPER_CODIGO_OPERADORA"
        strSQL = strSQL + " from tipos_ligacao_teste p1, tarifacao p2"
        strSQL = strSQL + " where(p1.CODIGO_TARIF = p2.codigo)"
        strSQL = strSQL + " and p2.tipo_tarifa in ('0')"
        strSQL = strSQL + " order by descricao"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list

    End Function


    Public Function InsereTarifacaoRota(ByVal TarifacaoRota As AppTarifacaoRota, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into rota_horario_teste (rota,operadora "
            sql = sql + " ,tipo_ligacao,descricao)  "
            sql = sql + " values ( '" + TarifacaoRota.rota + "', "
            sql = sql + "'" + TarifacaoRota.operadora + "',"
            sql = sql + "'" + TarifacaoRota.tipo_ligacao + "',"
            sql = sql + "'" + TarifacaoRota.descricao + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(TarifacaoRota, "N", usuario)

            cmd.CommandText = sql
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

    Public Function AtualizaTarifacaoRota(ByVal TarifacaoRota As AppTarifacaoRota, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifacaoRotaById(TarifacaoRota.rota).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update rota_horario_teste set "
            'sql = sql + " rota='" + TarifacaoRota.rota + "',"
            sql = sql + " operadora='" + TarifacaoRota.operadora + "',"
            sql = sql + " tipo_ligacao='" + TarifacaoRota.tipo_ligacao + "',"
            sql = sql + " descricao='" + TarifacaoRota.descricao + "'"
            sql = sql + " where rota = '" + TarifacaoRota.rota + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(TarifacaoRota, "B", usuario)

            cmd.CommandText = sql
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

    Public Function ExcluiTarifacaoRota(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifacaoRotaById(pcodigo).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete rota_horario_teste "
            sql = sql + "where rota = '" + pcodigo + "'"

            cmd.CommandText = sql
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

    Public Function GetTarifacaoRotaById(ByVal prota As String) As List(Of AppTarifacaoRota)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifacaoRota)

        Dim strSQL As String = "select t.rota"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.operadora, 0) AS operadora"
        strSQL = strSQL + ", nvl(t.tipo_ligacao, 0) AS tipo_ligacao"
        strSQL = strSQL + " from rota_horario_teste t where rota='" + prota.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifacaoRota

                _registro.rota = reader.Item("rota").ToString
                _registro.descricao = reader.Item("descricao").ToString
                _registro.operadora = reader.Item("operadora").ToString
                _registro.tipo_ligacao = reader.Item("tipo_ligacao").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal TarifacaoRota As AppTarifacaoRota, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into rota_horario_log (codigo_log, usuario_log, data_log, tipo_log"
        sql = sql + " ,rota,operadora,tipo_ligacao,descricao)  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from rota_horario_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        sql = sql + " '" & TarifacaoRota.rota & "',"
        sql = sql + " '" & TarifacaoRota.operadora & "',"
        sql = sql + " '" & TarifacaoRota.tipo_ligacao & "',"
        sql = sql + " '" & TarifacaoRota.descricao & "')"
        Return sql

    End Function
End Class

