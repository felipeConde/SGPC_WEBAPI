Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic


Public Class DAO_ModeloCelular
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


    Public Function GetModelo(ByVal pcodigo As String) As List(Of AppModeloCelular)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppModeloCelular)

        Dim strSQL As String = ""
        strSQL = strSQL + "select * "
        strSQL = strSQL + " from APARELHOS_MODELOS "
        strSQL = strSQL + " where COD_MODELO='" & pcodigo & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppModeloCelular(reader.Item("COD_MODELO").ToString, reader.Item("MODELO").ToString, reader.Item("COD_TIPO").ToString, reader.Item("COD_MARCA").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function InsertModelo(ByVal _data As AppModeloCelular, ByVal date_log As String, ByVal user As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = " insert into APARELHOS_MODELOS(COD_MODELO,MODELO,COD_TIPO,COD_MARCA)"
            strSQL = strSQL + " values ((select nvl(max(COD_MODELO),0)+1 from APARELHOS_MODELOS), '" & _data.modelo & "',"
            strSQL = strSQL + " '" & _data.cod_tipo & "','" & _data.cod_marca & "')"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = " insert into APARELHOS_MODELOS_LOG( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, COD_MODELO,MODELO,COD_TIPO,COD_MARCA)"
            strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from APARELHOS_MODELOS_LOG),'N',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + " (select nvl(max(COD_MODELO),0)+1 from APARELHOS_MODELOS) , '" & _data.modelo & "','" & _data.cod_tipo & "','" & _data.cod_marca & "')"

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

    Public Function UpdateModelo(ByVal _data As AppModeloCelular, ByVal date_log As String, ByVal user As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim _data_old As New AppModeloCelular
            _data_old = GetModelo(_data.cod_modelo).Item(0)

            Dim strSQL As String = " insert into APARELHOS_MODELOS_LOG( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, COD_MODELO,MODELO,COD_TIPO,COD_MARCA)"
            strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from APARELHOS_MODELOS_LOG),'A',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + " (select nvl(max(COD_MODELO),0)+1 from APARELHOS_MODELOS) , '" & _data_old.modelo & "','" & _data_old.cod_tipo & "','" & _data_old.cod_marca & "')"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************

            strSQL = " update APARELHOS_MODELOS set MODELO='" & _data.modelo & "',COD_TIPO='" & _data.cod_tipo & "',COD_MARCA='" & _data.cod_marca & "' where COD_MODELO='" & _data.cod_modelo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************

            strSQL = " insert into APARELHOS_MODELOS_LOG( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, COD_MODELO,MODELO,COD_TIPO,COD_MARCA)"
            strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from APARELHOS_MODELOS_LOG),'B',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + " (select nvl(max(COD_MODELO),0)+1 from APARELHOS_MODELOS) , '" & _data.modelo & "','" & _data.cod_tipo & "','" & _data.cod_marca & "')"

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

    Public Function RemoveModelo(ByVal _code As String, ByVal date_log As String, ByVal user As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim _data_old As New AppModeloCelular
            _data_old = GetModelo(_code).Item(0)

            Dim strSQL As String = " insert into APARELHOS_MODELOS_LOG( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, COD_MODELO,MODELO,COD_TIPO,COD_MARCA)"
            strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from APARELHOS_MODELOS_LOG),'D',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + " (select nvl(max(COD_MODELO),0)+1 from APARELHOS_MODELOS) , '" & _data_old.modelo & "','" & _data_old.cod_tipo & "','" & _data_old.cod_marca & "')"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '**********************************************************************************************

            strSQL = " delete APARELHOS_MODELOS where COD_MODELO='" & _code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************

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
