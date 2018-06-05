Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Avisos

    Private _strConn As String = ""

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Private _msg As String
    Public Property MSG As String
        Get
            Return _msg
        End Get
        Set(ByVal value As String)
            _msg = value
        End Set
    End Property

    Public Function RetornaConexao() As String
        Return strConn
    End Function


    Public Function InsereAviso(ByVal registro As AppAviso) As Boolean

        Try
            Dim connection As New OleDbConnection(strConn)

            Dim strSQL As String = "insert into faturas_avisos(CODIGO"
            strSQL = strSQL + ", CODIGO_FATURAS_CONTROLE, FATURA, VENCIMENTO, DATA_AVISO,PROTOCOLO,JUSTIFICATIVA,AUTOR,DATA "
            strSQL = strSQL + " ) "
            strSQL = strSQL + " values((select nvl(max(codigo),0)+1 from faturas_avisos)"
            strSQL = strSQL + ",'" + registro.CodigoFaturaControle.ToString + "'"
            strSQL = strSQL + ",'" + registro.Fatura.ToString + "'"
            strSQL = strSQL + ",to_date('" + registro.Vencimento.ToString + "','DD/MM/YYYY')"
            strSQL = strSQL + ",to_date('" + registro.DataAviso.ToString + "','DD/MM/YYYY')"
            strSQL = strSQL + ",'" + registro.Protocolo.ToString + "'"
            strSQL = strSQL + ",'" + registro.Justificativa.ToString + "'"
            strSQL = strSQL + ",'" + registro.Autor + "'"
            strSQL = strSQL + ",sysdate"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            MSG = ex.Message
            Return False

        End Try

       

    End Function


    Public Function TemAviso(ByVal registro As AppAviso, ByVal Operadora As String) As String

        Try
            Dim connection As New OleDbConnection(strConn)
            Dim result As String = ""
            Dim strSQL As String = " select t.justificativa from FATURAS_AVISOS t, faturas_controle p2 "
            strSQL = strSQL + " where t.codigo_faturas_controle=p2.codigo_faturas_controle and t.fatura='" + registro.Fatura.ToString + "' "
            strSQL = strSQL + " and to_char(t.vencimento,'dd/MM/yyyy')='" + registro.Vencimento.ToString + "' "
            strSQL = strSQL + " and p2.codigo_operadora='" + Operadora + "' "


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            Dim _reader As OleDbDataReader = cmd.ExecuteReader()

            Using connection

                While _reader.Read
                    result = _reader.Item(0).ToString
                End While

            End Using


            connection.Close()
            cmd.Dispose()
            Return result
        Catch ex As Exception
            MSG = ex.Message
            Return False

        End Try



    End Function

End Class
