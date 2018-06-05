Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic
'Imports ADOX

Public Class DAO_TarifasRetro
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

    Public Function getComboTarifas(ByVal tipo_tarifa As String, ByVal operadora As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""
        Dim list As New List(Of AppGeneric)

        strSQL = " "
        strSQL = strSQL + " select distinct p1.codigo,"
        strSQL = strSQL + " p1.descricao,"
        strSQL = strSQL + " nvl(p1.complemento, ' ') complemento,"
        strSQL = strSQL + " p2.OPER_CODIGO_OPERADORA"
        strSQL = strSQL + " from tipos_ligacao_teste p1, tarifacao p2"
        strSQL = strSQL + " where(p1.CODIGO_TARIF = p2.codigo)"
        strSQL = strSQL + " and p2.tipo_tarifa in ('" & tipo_tarifa.ToString & "')"
        strSQL = strSQL + " and p2.OPER_CODIGO_OPERADORA in ('" & operadora.ToString & "')"
        strSQL = strSQL + " order by descricao"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list

    End Function

    Public Function InsereTarifaRetro(ByVal tarifa As AppTarifaRetro, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into GESTAO_TARIFAS_RETRO (id, "
            sql = sql + " valor_ttm,step,CODIGO_TIPO_LIGACAO,DATA_INICIO,DATA_FIM,SEM_IMPOSTO)  "
            sql = sql + " values ( (select nvl(max(ID),0)+1 from GESTAO_TARIFAS_RETRO), "
            sql = sql + " to_number('" + Replace(tarifa.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifa.Step_ + "',"
            sql = sql + "'" + tarifa.Codigo_tipo_ligacao + "',"
            sql = sql + " to_date('" + tarifa.Data_ini + "','DD/MM/YYYY'), "
            sql = sql + " to_date('" + tarifa.Data_fim + "','DD/MM/YYYY'), "
            sql = sql + "'" + tarifa.Sem_imposto + "')"

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

    Public Function AtualizaTarifaRetro(ByVal tarifa As AppTarifaRetro, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifaById(tarifa.Codigo).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update GESTAO_TARIFAS_RETRO set "
            sql = sql + " valor_ttm= to_number('" + Replace(tarifa.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " step='" + tarifa.Step_ + "',"
            sql = sql + " SEM_IMPOSTO='" + tarifa.Sem_imposto + "',"
            sql = sql + " DATA_INICIO= to_date('" + tarifa.Data_ini + "','DD/MM/YYYY'), "
            sql = sql + " DATA_FIM=  to_date('" + tarifa.Data_fim + "','DD/MM/YYYY'), "
            sql = sql + " CODIGO_TIPO_LIGACAO='" + tarifa.Codigo_tipo_ligacao + "'"
            sql = sql + " where ID = '" + tarifa.Codigo.ToString + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(GetTarifaById(tarifa.Codigo).Item(0), "B", usuario)

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

            sql = String_log(GetTarifaById(pcodigo).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete GESTAO_TARIFAS_RETRO "
            sql = sql + "where ID = " + pcodigo

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

    Public Function GetTarifaById(ByVal pcodigo As Integer) As List(Of AppTarifaRetro)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifaRetro)

        Dim strSQL As String = "select t.ID"
        strSQL = strSQL + ", nvl(t.VALOR_TTM, 0) AS VALOR_TTM"
        strSQL = strSQL + ", nvl(t.STEP, '') AS STEP"
        strSQL = strSQL + ", nvl(t.DATA_INICIO, '') AS DATA_INICIO"
        strSQL = strSQL + ", nvl(t.DATA_FIM, '') AS DATA_FIM"
        strSQL = strSQL + ", nvl(t.SEM_IMPOSTO, '') AS SEM_IMPOSTO"
        strSQL = strSQL + ", nvl(t.CODIGO_TIPO_LIGACAO, 0) AS CODIGO_TIPO_LIGACAO"
        strSQL = strSQL + " from GESTAO_TARIFAS_RETRO t where ID='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifaRetro

                _registro.Codigo = reader.Item("ID").ToString
                _registro.TTM_Value = reader.Item("VALOR_TTM").ToString
                _registro.Step_ = reader.Item("STEP").ToString
                _registro.Data_ini = reader.Item("DATA_INICIO").ToString
                _registro.Data_fim = reader.Item("DATA_FIM").ToString
                _registro.Sem_imposto = reader.Item("SEM_IMPOSTO").ToString
                _registro.Codigo_tipo_ligacao = reader.Item("CODIGO_TIPO_LIGACAO").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal registro As AppTarifaRetro, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into GESTAO_TARIFAS_RETRO_LOG (codigo_log, usuario_log, data_log, tipo_log, ID"
        sql = sql + " , valor_ttm, step, DATA_INICIO, DATA_FIM, SEM_IMPOSTO, CODIGO_TIPO_LIGACAO) "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from GESTAO_TARIFAS_RETRO_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        If tipo_log = "N" Then
            sql = sql + " (select nvl(max(ID),0) from GESTAO_TARIFAS_RETRO),"
        Else
            sql = sql + " '" & registro.Codigo & "',"
        End If
        sql = sql + "to_number('" + Replace(registro.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
        sql = sql + "'" + registro.Step_ + "',"
        sql = sql + " to_date('" + registro.Data_ini.Replace("00:00:00", "") + "','DD/MM/YYYY'), "
        sql = sql + " to_date('" + registro.Data_fim.Replace("00:00:00", "") + "','DD/MM/YYYY'), "
        sql = sql + "'" + registro.Sem_imposto + "',"
        sql = sql + "'" + registro.Codigo_tipo_ligacao + "')"
        Return sql

    End Function

End Class
