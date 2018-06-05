Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Requisicao

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


    Public Function Insert(ByVal registro As AppRequisicao, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into REQUISICOES(CODIGO, DATA"
            strSQL = strSQL + ",  DESCRICAO,AUTOR) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from REQUISICOES)"
            strSQL = strSQL + " , to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + registro.Descricao + "'"
            strSQL = strSQL + ",'" + usuario + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("(select nvl(max(CODIGO),0) from REQUISICOES)", "N", usuario)
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

    Public Function Update(ByVal registro As AppRequisicao, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = String_log("'" & registro.Codigo & "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update REQUISICOES set "
            strSQL = strSQL + " DESCRICAO='" + registro.Descricao + "',"
            strSQL = strSQL + " AUTOR='" + registro.Autor + "',"
            strSQL = strSQL + " AUTORIZADOR='" + registro.Autorizador + "',"
            strSQL = strSQL + " APROVADA='" + registro.Aprovada + "',"
            strSQL = strSQL + " OPERADOR='" + registro.Operador + "',"
            strSQL = strSQL + " CONCLUIDA='" + registro.Concluida + "'"

            strSQL = strSQL + " where CODIGO = '" + registro.Codigo + "' "

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

    Public Function Excluir(ByVal pcodigo As Integer, ByVal usuario As String) As Boolean
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

            strSQL = "delete REQUISICOES "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

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

    Public Function GetRequisicaoByCodigo(ByVal pcodigo As Integer) As List(Of AppRequisicao)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppRequisicao)

        Dim strSQL As String = "select codigo"
        strSQL = strSQL + ", nvl(DESCRICAO, '') AS DESCRICAO"
        strSQL = strSQL + ", nvl(AUTOR, '') AS AUTOR"
        strSQL = strSQL + ", nvl(AUTORIZADOR, '') AS AUTORIZADOR"
        strSQL = strSQL + ", nvl(APROVADA, '') AS APROVADA"
        strSQL = strSQL + ", nvl(OPERADOR, '') AS OPERADOR"
        strSQL = strSQL + ", nvl(CONCLUIDA, '') AS CONCLUIDA"
        strSQL = strSQL + " FROM REQUISICOES "
        strSQL = strSQL + " WHERE codigo ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRequisicao()

                _registro.Codigo = reader.Item("codigo").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.Autor = reader.Item("AUTOR").ToString
                _registro.Autorizador = reader.Item("AUTORIZADOR").ToString
                _registro.Aprovada = reader.Item("APROVADA").ToString
                _registro.Operador = reader.Item("OPERADOR").ToString
                _registro.Concluida = reader.Item("CONCLUIDA").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into REQUISICOES_log(codigo_log,  "
        sql = sql + " usuario_log, data_log, tipo_log, "
        sql = sql + " CODIGO,DESCRICAO,AUTOR "
        sql = sql + " ,AUTORIZADOR,APROVADA,OPERADOR,CONCLUIDA )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from REQUISICOES_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + "" + pcodigo + ","
            sql = sql + " (select DESCRICAO from REQUISICOES where CODIGO=" + pcodigo + "),"
            sql = sql + " (select AUTOR from REQUISICOES where CODIGO=" + pcodigo + "),"
            sql = sql + " (select AUTORIZADOR from REQUISICOES where CODIGO=" + pcodigo + "),"
            sql = sql + " (select APROVADA from REQUISICOES where CODIGO=" + pcodigo + "),"
            sql = sql + " (select OPERADOR from REQUISICOES where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CONCLUIDA from REQUISICOES where CODIGO=" + pcodigo + "))"
        End If
        Return sql
    End Function


End Class
