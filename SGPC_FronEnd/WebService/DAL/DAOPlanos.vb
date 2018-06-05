Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOPlanos

    Private _strConn As String = ""

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property


    Public Function InserePlano(ByVal _registro As AppPlano, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into OPERADORAS_PLANOS(CODIGO_PLANO"
        strSQL = strSQL + ", PLANO, INICIO_VALIDADE, FIM_VALIDADE,CONTRATO,CODIGO_OPERADORA,TRAFEGO )"

        strSQL = strSQL + " values((select nvl(max(t.codigo_plano),0)+1 from OPERADORAS_PLANOS t)"
        strSQL = strSQL + ",'" + _registro.Plano.ToString + "'"
        strSQL = strSQL + "," + IIf(String.IsNullOrEmpty(_registro.InicioValidade.ToString), "''", "to_date('" & _registro.InicioValidade.ToString & "','DD/MM/YYYY')") + " "
        strSQL = strSQL + "," + IIf(String.IsNullOrEmpty(_registro.FimValidade.ToString), "''", "to_date('" & _registro.FimValidade.ToString & "','DD/MM/YYYY')") + " "
        strSQL = strSQL + ",'" + _registro.Contrato.ToString + "'"
        strSQL = strSQL + ",'" + _registro.CodigoOperadora.ToString + "'"
        strSQL = strSQL + ",'" + _registro.Trafego.ToString.ToString.Replace(".", "").Replace(",", ".") + "'"
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        Return True
    End Function


    Public Function GetPlanoByCodigo(ByVal pCodigo As Integer) As AppPlano
        Dim connection As New OleDbConnection(strConn)
        Dim _registro As AppPlano

        Dim strSQL As String = "select CODIGO_PLANO,PLANO, to_char(INICIO_VALIDADE,'DD/MM/YYYY')INICIO_VALIDADE, to_char(FIM_VALIDADE,'DD/MM/YYYY')FIM_VALIDADE,CONTRATO,CODIGO_OPERADORA,TRAFEGO "
        strSQL = strSQL + " from OPERADORAS_PLANOS where CODIGO_PLANO='" & pCodigo & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro = New AppPlano(reader.Item("CODIGO_PLANO").ToString, reader.Item("plano").ToString, reader.Item("CODIGO_OPERADORA").ToString)
                _registro.Contrato = reader.Item("CONTRATO").ToString
                _registro.InicioValidade = reader.Item("INICIO_VALIDADE").ToString
                _registro.FimValidade = reader.Item("FIM_VALIDADE").ToString
                _registro.Contrato = reader.Item("CONTRATO").ToString
                _registro.Trafego = reader.Item("TRAFEGO").ToString
            End While
        End Using

        Return _registro
    End Function

    Public Function Atualiza(ByVal registro As AppPlano, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "update OPERADORAS_PLANOS set "
        strSQL = strSQL + "PLANO='" + registro.Plano + "',"
        If Not String.IsNullOrEmpty(registro.InicioValidade.ToString) Then
            strSQL = strSQL + "INICIO_VALIDADE=to_date('" + registro.InicioValidade.ToString + "','DD/MM/YYYY'), "
        Else
            strSQL = strSQL + "INICIO_VALIDADE='',"
        End If
        If Not String.IsNullOrEmpty(registro.FimValidade.ToString) Then
            strSQL = strSQL + "FIM_VALIDADE=to_date('" + registro.FimValidade.ToString + "','DD/MM/YYYY'), "
        Else
            strSQL = strSQL + "FIM_VALIDADE='', "
        End If
        strSQL = strSQL + "CONTRATO='" + registro.Contrato.ToString + "', "
        strSQL = strSQL + "CODIGO_OPERADORA='" + registro.CodigoOperadora.ToString + "', "
        strSQL = strSQL + "TRAFEGO='" + registro.Trafego.ToString.Replace(".", "").Replace(",", ".") + "'  "

        strSQL = strSQL + " where CODIGO_PLANO = '" + registro.CodigoPlano.ToString + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        Return True

    End Function

    Public Function ExcluirPlano(ByVal pcodigo As String, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try

            Dim strSQL As String = "delete OPERADORAS_PLANOS "
            strSQL = strSQL + "where CODIGO_PLANO = '" + pcodigo + "'"

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

End Class
