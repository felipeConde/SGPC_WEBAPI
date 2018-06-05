Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_CategoriaUsuario

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

    Public Function InsereCategoriaUsuario(ByVal cat_user As AppCategoriaUsuario, ByVal user As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'Inserção do registro

            strSQL = "insert into categoria_usuario(codigo, "
            strSQL = strSQL + " codigo_usuario, tipo_usuario, codigo_grupo, periodicidade, dia_email ) "
            strSQL = strSQL + " values ((select nvl(max(codigo),0)+1 from categoria_usuario)"
            strSQL = strSQL + " ,'" + cat_user.Codigo_usuario + "'"
            strSQL = strSQL + " ,'" + cat_user.Tipo_usuario + "'"
            strSQL = strSQL + " ,'" + cat_user.Codigo_Grupo + "'"
            strSQL = strSQL + " ,'" + cat_user.Peridiocidade + "'"
            strSQL = strSQL + " ,'" + cat_user.Dia_email + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'Inserção do Log

            strSQL = "insert into categoria_usuario_log(codigo, "
            strSQL = strSQL + " tipo_log,"
            strSQL = strSQL + " data_log,"
            strSQL = strSQL + " codigo_usuario,"
            strSQL = strSQL + " categoria_tipo,"
            strSQL = strSQL + " codigo_grupo,"
            strSQL = strSQL + " periodicidade,"
            strSQL = strSQL + " dia_email, autor )"
            strSQL = strSQL + " values((select nvl(max(codigo),0)+1 from categoria_usuario_log)"
            strSQL = strSQL + " ,'N'"
            strSQL = strSQL + " ,to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,'" & cat_user.Codigo_usuario & "'"
            strSQL = strSQL + " ,'" & cat_user.Tipo_usuario & "'"
            strSQL = strSQL + " ,'" & cat_user.Codigo_Grupo & "'"
            strSQL = strSQL + " ,'" & cat_user.Peridiocidade & "'"
            strSQL = strSQL + " ,'" & cat_user.Dia_email & "'"
            strSQL = strSQL + " ,'" + user + "'"
            strSQL = strSQL + ")"

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

    Public Function ExcluiCategoria(ByVal pcodigo As String, ByVal user As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'Inserção do log

            Dim strSQL As String = "insert into categoria_usuario_log(codigo, "
            strSQL = strSQL + " tipo_log,"
            strSQL = strSQL + " data_log,"
            strSQL = strSQL + " codigo_usuario,"
            strSQL = strSQL + " categoria_tipo,"
            strSQL = strSQL + " codigo_grupo,"
            strSQL = strSQL + " periodicidade,"
            strSQL = strSQL + " dia_email, autor )"
            strSQL = strSQL + " values((select nvl(max(codigo),0)+1 from categoria_usuario_log)"
            strSQL = strSQL + " ,'D'"
            strSQL = strSQL + " ,to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,(select codigo_usuario from categoria_usuario where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select tipo_usuario from categoria_usuario where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select codigo_grupo from categoria_usuario where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select periodicidade from categoria_usuario where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,(select dia_email from categoria_usuario where codigo='" + pcodigo + "')"
            strSQL = strSQL + " ,'" + user + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'Exclusão do registro

            strSQL = "delete categoria_usuario "
            strSQL = strSQL + "where CODIGO = '" + pcodigo + "'"

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

    Public Function GetCategoriaTypeByCod(ByVal pcodigo As String) As String
        Dim connection As New OleDbConnection(strConn)

        'InsertGruposLog(GetGruposById(pcodigo).Item(0), "D", user)
        Dim _registro As String = ""

        Try
            Dim strSQL As String = "select tipo_usuario "
            strSQL = strSQL + " from categoria_usuario "
            strSQL = strSQL + " where CODIGO = '" + pcodigo + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    _registro = reader.Item("tipo_usuario").ToString
                End While
            End Using

            Return _registro

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function VerificaMesmaCat(ByVal pcodigo As String, ByVal categoria As String, ByVal ccusto As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim _registro As New List(Of String)
        'InsertGruposLog(GetGruposById(pcodigo).Item(0), "D", user)

        Dim strSQL As String = "select tipo_usuario "
        strSQL = strSQL + " from categoria_usuario "
        strSQL = strSQL + " where codigo_usuario = '" + pcodigo + "' and codigo_grupo='" & ccusto & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro.Add(reader.Item("tipo_usuario").ToString)
            End While
        End Using

        For Each item As String In _registro
            If categoria = item Then
                Return True
            End If
        Next

        Return False
    End Function

End Class
