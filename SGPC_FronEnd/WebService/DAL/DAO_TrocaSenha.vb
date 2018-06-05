Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Collections.Generic


Public Class DAO_TrocaSenha

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


    Public Function GerarSenha(ByVal alfanumerica As Boolean, ByVal tamanho As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select p1.senha senha from "
        If alfanumerica Then
            strSQL = strSQL + " (select upper(to_char(DBMS_RANDOM.STRING('a', " & tamanho & ")))senha from dual)p1 "
        Else
            Dim tamanhonumerico As String = ""
            Dim i As Integer = 0
            For i = 0 To tamanho - 1
                tamanhonumerico += "9"
            Next

            strSQL = strSQL + " (SELECT to_char((1+ABS(MOD(dbms_random.random," & tamanhonumerico & ")))) senha from dual )p1 "
        End If
        strSQL = strSQL + " where senha not in (select senha_usuario from usuarios where senha_usuario is not null) "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("senha").ToString
            End While
        End Using


        'se veio nulo gera novamente
        If String.IsNullOrEmpty(_result) Then
            _result = GerarSenha(alfanumerica, tamanho)
        End If

        Return _result

    End Function

    Public Function PegaParametros(ByVal categoria As String, ByVal parametro As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = " "
        strSQL = strSQL + " select t.valor_parametro from PARAMETROS_SGPC t "
        strSQL = strSQL + " where upper(t.categoria)='" & categoria.ToUpper.Trim & "' and upper(t.nome_parametro)='" & parametro.ToUpper.Trim & "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _result = reader.Item("valor_parametro").ToString
            End While
        End Using

        Return _result

    End Function

    Public Function UpdateSenha(ByVal codigo_usuario As Integer, ByRef senha_usuario As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try

            Dim strSQL As String = "update usuarios set"
            strSQL = strSQL & " senha_usuario='" + senha_usuario + "'"
            strSQL = strSQL & " WHERE CODIGO='" + codigo_usuario.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()



            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function UpdateRamal(ByRef ramal As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try

            Dim strSQL As String = "update ramais set"
            strSQL = strSQL & " POSSUI_BLOQUEIO='S', BLOQUEAVEL='S' "
            strSQL = strSQL & " WHERE NUMERO_A='" + ramal.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function InsereLog(ByVal codigo_usuario As Integer, ByVal autor As String, ByVal senha_usuario As String, ByVal senha_anterior As String, ByVal EMAIL_USUARIO As String, ByVal EMAIL_ADMINISTRADOR As String, ByVal USUARIO As String, ByVal RAMAL As String, ByVal SOLICITANTE As String, ByVal CODIGO_AUTOR As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try

            Dim strSQL As String = "insert into TROCA_SENHA_LOG (ID,TIPO,DATA,AUTOR,SENHA_ANTERIOR,SENHA_NOVA,EMAIL_USUARIO,EMAIL_ADMINISTRADOR,USUARIO,RAMAL,SOLICITANTE,CODIGO_AUTOR)"
            strSQL = strSQL & " values ((select nvl(max(id),0)+1 from TROCA_SENHA_LOG),'N',sysdate"
            strSQL = strSQL & ",'" + autor + "'"
            strSQL = strSQL & ",'" + senha_anterior + "'"
            strSQL = strSQL & ",'" + senha_usuario + "'"
            strSQL = strSQL & ",'" + EMAIL_USUARIO + "'"
            strSQL = strSQL & ",'" + EMAIL_ADMINISTRADOR + "'"
            strSQL = strSQL & ",'" + USUARIO + "'"
            strSQL = strSQL & ",'" + RAMAL + "'"
            strSQL = strSQL & ",'" + SOLICITANTE + "'"
            strSQL = strSQL & ",'" + CODIGO_AUTOR + "'"
            strSQL = strSQL & " ) "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function


End Class
