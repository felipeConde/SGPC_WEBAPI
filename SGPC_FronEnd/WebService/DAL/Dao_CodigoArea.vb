Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class Dao_CodigoArea
    
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


    Public Function InsereCodigoArea(ByVal CodigoArea As AppCodigoArea, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into codigo_area (codigo,operadora "
            sql = sql + " ,codigo_tipo_ligacao,descricao)  "
            sql = sql + " values ( '" + CodigoArea.codigo + "', "
            sql = sql + "'" + CodigoArea.operadora + "',"
            sql = sql + "'" + CodigoArea.tipo_ligacao + "',"
            sql = sql + "'" + CodigoArea.descricao + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(CodigoArea, "N", usuario)

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

    Public Function AtualizaCodigoArea(ByVal CodigoArea As AppCodigoArea, ByVal pcodigo_operadora As String, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetCodigoAreaById(CodigoArea.codigo, pcodigo_operadora).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update codigo_area set "
            'sql = sql + " codigo='" + Tarifacaocodigo.codigo + "',"
            sql = sql + " operadora='" + CodigoArea.operadora + "',"
            sql = sql + " codigo_tipo_ligacao='" + CodigoArea.tipo_ligacao + "',"
            sql = sql + " descricao='" + CodigoArea.descricao + "'"
            sql = sql + " where codigo = '" + CodigoArea.codigo + "' and operadora='" + pcodigo_operadora + "'"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(CodigoArea, "B", usuario)

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

    Public Function ExcluiCodigoArea(ByVal pcodigo As String, ByVal pcodigo_operadora As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetCodigoAreaById(pcodigo, pcodigo_operadora).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete codigo_area "
            sql = sql + "where codigo='" + pcodigo + "' and operadora='" + pcodigo_operadora + "'"

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

    Public Function GetCodigoAreaById(ByVal pcodigo As String, ByVal pcodigo_operadora As String) As List(Of AppCodigoArea)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppCodigoArea)

        Dim strSQL As String = "select t.codigo"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.operadora, 0) AS operadora"
        strSQL = strSQL + ", nvl(t.codigo_tipo_ligacao, 0) AS tipo_ligacao"
        strSQL = strSQL + " from codigo_area t where codigo='" + pcodigo.ToString + "' and operadora='" + pcodigo_operadora + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppCodigoArea

                _registro.codigo = reader.Item("codigo").ToString
                _registro.descricao = reader.Item("descricao").ToString
                _registro.operadora = reader.Item("operadora").ToString
                _registro.tipo_ligacao = reader.Item("tipo_ligacao").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal CodigoArea As AppCodigoArea, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into codigo_area_log (codigo_log, usuario_log, data_log, tipo_log"
        sql = sql + " ,codigo,operadora,codigo_tipo_ligacao,descricao)  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from codigo_area_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        sql = sql + " '" & CodigoArea.codigo & "',"
        sql = sql + " '" & CodigoArea.operadora & "',"
        sql = sql + " '" & CodigoArea.tipo_ligacao & "',"
        sql = sql + " '" & CodigoArea.descricao & "')"
        Return sql

    End Function
End Class


