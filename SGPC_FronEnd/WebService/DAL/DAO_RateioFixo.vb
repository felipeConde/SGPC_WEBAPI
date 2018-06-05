Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_RateioFixo
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

    Public Function InserePorCadastro(ByVal services As String(), ByVal lines As String(), ByVal values As String(), ByVal codigo_fatura As String, ByVal inicio As String, ByVal fim As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""
        Dim commons As New DAO_Commons

        Dim Rateio_code As String = ""

        commons.strConn = strConn
        Rateio_code = commons.GetMaximumCode("codigo_rateio", "rateio_fixo")

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            strSQL = "insert into rateio_fixo"
            strSQL = strSQL + " (CODIGO_RATEIO, DATA_RATEIO, CODIGO_FATURA, CODIGO_TIPO, INICIO_FATURAMENTO, FIM_FATURAMENTO) "
            strSQL = strSQL + " values('" & Rateio_code & "',sysdate,'" & codigo_fatura & "'"
            strSQL = strSQL + " ,'1', TO_DATE('" & inicio & "', 'DD/MM/YYYY') ,TO_DATE('" & fim & "', 'DD/MM/YYYY'))"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            Dim count As Integer = 1

            For Each line As String In lines
                If line <> "" Then
                    strSQL = "insert into rateio_fixo_linhas"
                    strSQL = strSQL + " (CODIGO_RATEIO, LINHA, VALOR, TIPO_SERV2 ) "
                    strSQL = strSQL + " values('" & Rateio_code & "','" & line & "','" & values(count).Replace(".", "").Replace(",", ".") & "','" & services(count) & "')"

                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()

                    count = count + 1
                End If
            Next


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


    Public Function InserePorTarifacao(ByVal services As String(), ByVal ccustos As String(), ByVal lines As String(), ByVal values As String(), ByVal codigo_fatura As String, ByVal inicio As String, ByVal fim As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""
        Dim commons As New DAO_Commons

        Dim Rateio_code As String = ""

        commons.strConn = strConn
        Rateio_code = commons.GetMaximumCode("codigo_rateio", "rateio_fixo")

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            strSQL = "insert into rateio_fixo"
            strSQL = strSQL + " (CODIGO_RATEIO, DATA_RATEIO, CODIGO_FATURA, CODIGO_TIPO, INICIO_FATURAMENTO, FIM_FATURAMENTO) "
            strSQL = strSQL + " values('" & Rateio_code & "',sysdate,'" & codigo_fatura & "'"
            strSQL = strSQL + " ,'2', TO_DATE('" & inicio & "', 'DD/MM/YYYY') ,TO_DATE('" & fim & "', 'DD/MM/YYYY'))"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            Dim count As Integer = 1

            For Each line As String In lines
                If line <> "" Then
                    strSQL = "insert into rateio_fixo_linhas"
                    strSQL = strSQL + " (CODIGO_RATEIO, LINHA, VALOR, TIPO_SERV2 ) "
                    strSQL = strSQL + " values('" & Rateio_code & "','" & line & "','" & values(count).Replace(".", "").Replace(",", ".") & "','" & services(count) & "')"

                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()

                    count = count + 1
                End If
            Next

            For Each ccusto As String In ccustos
                If ccusto <> "" Then
                    strSQL = "insert into rateio_fixo_grupos"
                    strSQL = strSQL + " (CODIGO_RATEIO, DATA_RATEIO, CCUSTO ) "
                    strSQL = strSQL + " values('" & Rateio_code & "','" & DateTime.Now.Date & "','" & ccusto & "')"

                    cmd.CommandText = strSQL
                    cmd.ExecuteNonQuery()
                End If
            Next


            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception

            EscreveLog("Erro na aplicação: " & e.Message)


            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Return True

    End Function

    Private Sub EscreveLog(ByVal pMSG As String)
        Try


            Dim log As IO.StreamWriter
            'Dim caminhoLog As String = Application.StartupPath & ConfigurationManager.AppSettings("nomeArquivo").ToString
            Dim caminhoLog As String = AppDomain.CurrentDomain.BaseDirectory + "logGlobal.txt"
            If Not IO.File.Exists(caminhoLog) Then
                log = IO.File.CreateText(caminhoLog)
                log.WriteLine(Date.Now + "-" + pMSG)
            Else

                log = New IO.StreamWriter(caminhoLog, True, System.Text.Encoding.UTF8)
                log.WriteLine(Date.Now + "-" + pMSG)

            End If
            log.Close()
            log.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Function ExcluiRateio(ByVal pcodigo As Integer, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "" 'String_log("'" & pcodigo & "'", "D", usuario)

            strSQL = "delete Rateio_Fixo "
            strSQL = strSQL + "where CODIGO_RATEIO = " + Convert.ToString(pcodigo)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete rateio_fixo_linhas "
            strSQL = strSQL + "where CODIGO_RATEIO = " + Convert.ToString(pcodigo)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete rateio_fixo_grupos "
            strSQL = strSQL + "where CODIGO_RATEIO = " + Convert.ToString(pcodigo)

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

End Class
