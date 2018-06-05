Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic
'Imports ADOX

Public Class DAO_TarifaNumCham

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

    Public Function InsereTarifa(ByVal tarifa As AppTarifaNumCham, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into numb_valor_tipo (numerob,ttm "
            sql = sql + " ,valor_ttm,step,valor_step,COD_TIPO_LIGACAO)  "
            sql = sql + " values ( "
            sql = sql + "'" + tarifa.Numero + "',"
            sql = sql + "'" + tarifa.TTM + "',"
            sql = sql + "to_number('" + Replace(tarifa.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifa.Step_ + "',"
            sql = sql + "to_number('" + Replace(tarifa.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifa.Codigo_tipo_ligacao + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(tarifa, "N", usuario)

            cmd.CommandText = sql
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

    Public Function AtualizaTarifa(ByVal tarifa As AppTarifaNumCham, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifaNumChamById(tarifa.Numero).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update numb_valor_tipo set "
            sql = sql + " ttm='" + tarifa.TTM + "',"
            sql = sql + " valor_ttm= to_number('" + Replace(tarifa.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " step='" + tarifa.Step_ + "',"
            sql = sql + " valor_step= to_number('" + Replace(tarifa.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " COD_TIPO_LIGACAO='" + tarifa.Codigo_tipo_ligacao + "'"
            sql = sql + " where numerob = '" + tarifa.Numero.ToString + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(GetTarifaNumChamById(tarifa.Numero).Item(0), "B", usuario)

            cmd.CommandText = sql
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

    Public Function ExcluiTarifa(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifaNumChamById(pcodigo).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete numb_valor_tipo "
            sql = sql + "where numerob = " + pcodigo

            cmd.CommandText = sql
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


    Public Function GetTarifaNumChamById(ByVal pcodigo As Integer) As List(Of AppTarifaNumCham)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifaNumCham)

        Dim strSQL As String = "select NUMEROB,"
        strSQL = strSQL + " nvl(t.TTM, '') AS TTM"
        strSQL = strSQL + ", nvl(t.VALOR_TTM, 0) AS VALOR_TTM"
        strSQL = strSQL + ", nvl(t.STEP, '') AS STEP"
        strSQL = strSQL + ", nvl(t.VALOR_STEP, 0) AS VALOR_STEP"
        strSQL = strSQL + ", nvl(t.COD_TIPO_LIGACAO, 0) AS COD_TIPO_LIGACAO"
        strSQL = strSQL + " from numb_valor_tipo t where numerob='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifaNumCham

                _registro.Numero = reader.Item("NUMEROB").ToString
                _registro.TTM = reader.Item("TTM").ToString
                _registro.TTM_Value = reader.Item("VALOR_TTM").ToString
                _registro.Step_ = reader.Item("STEP").ToString
                _registro.Step_value = reader.Item("VALOR_STEP").ToString
                _registro.Codigo_tipo_ligacao = reader.Item("COD_TIPO_LIGACAO").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal registro As AppTarifaNumCham, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into NUMB_VALOR_TIPO_LOG (codigo_log, usuario_log, data_log, tipo_log, numerob,ttm "
        sql = sql + " ,valor_ttm,step,valor_step,COD_TIPO_LIGACAO) "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from NUMB_VALOR_TIPO_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        sql = sql + " '" & registro.Numero & "',"
        sql = sql + "'" + registro.TTM + "',"
        sql = sql + "to_number('" + Replace(registro.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
        sql = sql + "'" + registro.Step_ + "',"
        sql = sql + "to_number('" + Replace(registro.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
        sql = sql + "'" + registro.Codigo_tipo_ligacao + "' )"
        Return sql

    End Function

End Class
