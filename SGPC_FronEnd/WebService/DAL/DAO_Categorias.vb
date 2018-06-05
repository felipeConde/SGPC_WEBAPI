Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Categorias

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


    Public Function Insert(ByVal registro As AppCategoria, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into TIPO_CATEGORIA(CODIGO "
            strSQL = strSQL + ",DESCRICAO,ATIVO, FLAG_CCUSTO ) "
            strSQL = strSQL + "values ('" + registro.Codigo + "'"
            strSQL = strSQL + ",'" + registro.Descricao + "'"
            strSQL = strSQL + ",'" + registro.Ativo + "'"
            strSQL = strSQL + ",'" + registro.FlagCCusto + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" + registro.Codigo + "'", "N", usuario)
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

    Public Function Update(ByVal registro As AppCategoria, ByVal old_code As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = String_log("'" & old_code & "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update TIPO_CATEGORIA set "
            strSQL = strSQL + " CODIGO='" + registro.Codigo + "',"
            strSQL = strSQL + " DESCRICAO='" + registro.Descricao + "',"
            strSQL = strSQL + " ATIVO='" + registro.Ativo + "',"
            strSQL = strSQL + " FLAG_CCUSTO='" + registro.FlagCCusto + "'"

            strSQL = strSQL + " where CODIGO = '" + old_code + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" & registro.Codigo & "'", "B", usuario)
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

    Public Function Excluir(ByVal pcodigo As String, ByVal usuario As String) As Boolean
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

            strSQL = "delete TIPO_CATEGORIA "
            strSQL = strSQL + "where CODIGO = '" + Convert.ToString(pcodigo) + "'"

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

    Public Function GetByCodigo(ByVal pcodigo As String) As List(Of AppCategoria)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppCategoria)

        Dim strSQL As String = "select CODIGO "
        strSQL = strSQL + ", nvl(DESCRICAO, '') AS DESCRICAO"
        strSQL = strSQL + ", nvl(ATIVO, '') AS ATIVO"
        strSQL = strSQL + ", nvl(FLAG_CCUSTO, '') AS FLAG_CCUSTO"
        strSQL = strSQL + " FROM TIPO_CATEGORIA "
        strSQL = strSQL + " WHERE codigo ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppCategoria()

                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.Ativo = reader.Item("ATIVO").ToString
                _registro.FlagCCusto = reader.Item("FLAG_CCUSTO").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into TIPO_CATEGORIA_LOG(codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + " CODIGO, DESCRICAO,ATIVO,FLAG_CCUSTO )"
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from TIPO_CATEGORIA_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + "" + pcodigo + ","
            sql = sql + " (select DESCRICAO from TIPO_CATEGORIA where CODIGO=" + pcodigo + "),"
            sql = sql + " (select ATIVO from TIPO_CATEGORIA where CODIGO=" + pcodigo + "),"
            sql = sql + " (select FLAG_CCUSTO from TIPO_CATEGORIA where CODIGO=" + pcodigo + "))"
        End If
        Return sql
    End Function

End Class
