Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System

Public Class FacilidadesDAL
    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString2").ConnectionString

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

    Public Function RetornaConexao() As String
        Return strConn
    End Function


    Public Function InsereFacilidade(ByVal pCodigoVas As Integer, ByVal pCodigoOperadora As Integer, ByVal pCodigoLinha As Integer) As String
        Try
            Dim strSQL As String = "insert into linhas_vas(codigo_vas,codigo_operadora,codigo_linha) values ('" & pCodigoVas & "','" & pCodigoOperadora & "','" & pCodigoLinha & "')"
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

    Public Function ExcluiFacilidade(ByVal pCodigoVas As Integer, ByVal pCodigoOperadora As Integer, ByVal pCodigoLinha As Integer) As Boolean
        Try
            Dim strSQL As String = "delete from linhas_vas where codigo_vas='" & pCodigoVas & "' and codigo_operadora='" & pCodigoOperadora & "' and codigo_linha='" & pCodigoLinha & "'"
            Dim connection As New OleDbConnection(strConn)
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

    Public Function FacilidadeJaCadastrada(ByVal pCodigoVas As Integer, ByVal pCodigoOperadora As Integer, ByVal pCodigoLinha As Integer) As Boolean
        Dim result As Boolean = False
        Try
            Dim strSQL As String = "select 0 from linhas_vas where codigo_vas='" & pCodigoVas & "' and codigo_operadora='" & pCodigoOperadora & "' and codigo_linha='" & pCodigoLinha & "'"
            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                If reader.HasRows Then
                    result = True
                End If
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return result
    End Function

End Class
