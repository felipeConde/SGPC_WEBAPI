Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic


Public Class DAO_CatTarifas
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


    Public Function GetCatTarifa(ByVal pcodigo As String) As List(Of AppCatTarifa)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppCatTarifa)

        Dim strSQL As String = ""
        strSQL = strSQL + "select * "
        strSQL = strSQL + " from TARIFACAO "
        strSQL = strSQL + " where codigo='" & pcodigo & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppCatTarifa(reader.Item("codigo").ToString, reader.Item("nome_configuracao").ToString, reader.Item("tipo_tarifa").ToString, reader.Item("OPER_CODIGO_OPERADORA").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function InsertCatTarifa(ByVal registro As AppCatTarifa, ByVal date_log As String, ByVal user As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into tarifacao (codigo,"
            strSQL = strSQL + " nome_configuracao,tipo_tarifa,tipo_chamada,OPER_CODIGO_OPERADORA,tp_tarif_codigo) "
            strSQL = strSQL + " values ("
            strSQL = strSQL + "(select nvl(max(codigo),0)+1 from tarifacao),"
            strSQL = strSQL + "'" + registro.Descricao + "',"
            strSQL = strSQL + "'" + registro.Codigo_tipo + "',"
            strSQL = strSQL + "'" + IIf(registro.Codigo_tipo = "1", "C", "F") + "',"
            strSQL = strSQL + "'" + registro.Operadora + "',"
            strSQL = strSQL + "'P')"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()


            strSQL = "insert into tarifacao_log (CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, codigo,"
            strSQL = strSQL + " nome_configuracao,tipo_tarifa,tipo_chamada,OPER_CODIGO_OPERADORA,tp_tarif_codigo) "
            strSQL = strSQL + " values ("
            strSQL = strSQL + "(select nvl(max(CODIGO_LOG),0)+1 from tarifacao_log),'N',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + "(select nvl(max(codigo),0)+1 from tarifacao),"
            strSQL = strSQL + "'" + registro.Descricao + "',"
            strSQL = strSQL + "'" + registro.Codigo_tipo + "',"
            strSQL = strSQL + "'" + IIf(registro.Codigo_tipo = "1", "C", "F") + "',"
            strSQL = strSQL + "'" + registro.Operadora + "',"
            strSQL = strSQL + "'P')"

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

    Public Function UpdateCatTarifa(ByVal registro As AppCatTarifa, ByVal date_log As String, ByVal user As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim _data_old As New AppCatTarifa
            _data_old = GetCatTarifa(registro.Codigo).Item(0)

            Dim strSQL As String = "insert into tarifacao_log (CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, codigo,"
            strSQL = strSQL + " nome_configuracao,tipo_tarifa,tipo_chamada,OPER_CODIGO_OPERADORA,tp_tarif_codigo) "
            strSQL = strSQL + " values ("
            strSQL = strSQL + "(select nvl(max(CODIGO_LOG),0)+1 from tarifacao_log),'A',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + "'" + _data_old.Codigo + "',"
            strSQL = strSQL + "'" + _data_old.Descricao + "',"
            strSQL = strSQL + "'" + _data_old.Codigo_tipo + "',"
            strSQL = strSQL + "'" + IIf(_data_old.Codigo_tipo = "1", "C", "F") + "',"
            strSQL = strSQL + "'" + _data_old.Operadora + "',"
            strSQL = strSQL + "'P')"

            '*************************************************************************************

            strSQL = " update tarifacao set nome_configuracao='" & registro.Descricao & "',tipo_tarifa='" & registro.Codigo_tipo & "'"
            strSQL = strSQL + " ,tipo_chamada='" & IIf(registro.Codigo_tipo = "1", "C", "F") & "',OPER_CODIGO_OPERADORA='" & registro.Operadora & "' "
            strSQL = strSQL + " where codigo='" & registro.Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************

            strSQL = "insert into tarifacao_log (CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, codigo,"
            strSQL = strSQL + " nome_configuracao,tipo_tarifa,tipo_chamada,OPER_CODIGO_OPERADORA,tp_tarif_codigo) "
            strSQL = strSQL + " values ("
            strSQL = strSQL + "(select nvl(max(CODIGO_LOG),0)+1 from tarifacao_log),'B',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + "'" + registro.Codigo + "',"
            strSQL = strSQL + "'" + registro.Descricao + "',"
            strSQL = strSQL + "'" + registro.Codigo_tipo + "',"
            strSQL = strSQL + "'" + IIf(registro.Codigo_tipo = "1", "C", "F") + "',"
            strSQL = strSQL + "'" + registro.Operadora + "',"
            strSQL = strSQL + "'P')"

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

    Public Function RemoveCatTarifa(ByVal _code As String, ByVal date_log As String, ByVal user As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim _data_old As New AppCatTarifa
            _data_old = GetCatTarifa(_code).Item(0)

            Dim strSQL As String = "insert into tarifacao_log (CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, codigo,"
            strSQL = strSQL + " nome_configuracao,tipo_tarifa,tipo_chamada,OPER_CODIGO_OPERADORA,tp_tarif_codigo) "
            strSQL = strSQL + " values ("
            strSQL = strSQL + "(select nvl(max(CODIGO_LOG),0)+1 from tarifacao_log),'D',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',"
            strSQL = strSQL + "'" + _data_old.Codigo + "',"
            strSQL = strSQL + "'" + _data_old.Descricao + "',"
            strSQL = strSQL + "'" + _data_old.Codigo_tipo + "',"
            strSQL = strSQL + "'" + IIf(_data_old.Codigo_tipo = "1", "C", "F") + "',"
            strSQL = strSQL + "'" + _data_old.Operadora + "',"
            strSQL = strSQL + "'P')"
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '**********************************************************************************************

            strSQL = " delete tarifacao where codigo='" & _code & "'"

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
