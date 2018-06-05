Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_TarifasDerivadas

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

    Public Function getComboTarifas() As List(Of AppGeneric)
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
        strSQL = strSQL + " and p2.tipo_tarifa in ('0')"
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


    Public Function InsereTarifaDerivada(ByVal TarifaDerivada As AppTarifasDerivadas, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into TARIFAS_DERIVADAS_PRATICADAS (CODIGO_DERIVADO,FATOR "
            sql = sql + " ,HORARIO_APLICACAO,DESCRICAO,FLAG_PROTECAO)  "
            sql = sql + " values ((select nvl(max(CODIGO_DERIVADO),0)+1 from TARIFAS_DERIVADAS_PRATICADAS), "
            sql = sql + "'" + Replace(TarifaDerivada.fator, ",", ".") + "',"
            sql = sql + "'" + TarifaDerivada.horario + "',"
            sql = sql + "'" + TarifaDerivada.descricao + "',"
            sql = sql + "'N')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(TarifaDerivada, "N", usuario)

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

    Public Function geraHorario(ByVal dia As String, ByVal hora As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim sql As String = ""
        Dim horario As String = ""

        sql = sql + " select to_char((to_date('" + dia + "/01/1995 " + hora + ":00','DD/MM/YYYY HH24:MI:SS')"
        sql = sql + "-to_date('01/01/1995 00:00:00','DD/MM/YYYY HH24:MI:SS'))"
        sql = sql + "*86400,'0000000000000000000')horario from dual"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                horario = reader.Item("horario").ToString
            End While
        End Using

        Return horario

    End Function


    Public Function AtualizaTarifasDerivadas(ByVal TarifaDerivada As AppTarifasDerivadas, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifasDerivadasById(TarifaDerivada.codigo).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update TARIFAS_DERIVADAS_PRATICADAS set "
            'sql = sql + " rota='" + TarifacaoRota.rota + "',"
            sql = sql + " FATOR='" + Replace(TarifaDerivada.fator, ",", ".") + "',"
            sql = sql + " HORARIO_APLICACAO='" + TarifaDerivada.horario + "',"
            sql = sql + " DESCRICAO='" + TarifaDerivada.descricao + "'"
            sql = sql + " where codigo_derivado = '" + TarifaDerivada.codigo + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(TarifaDerivada, "B", usuario)

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

    Public Function ExcluiTarifacaoRota(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifasDerivadasById(pcodigo).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete TARIFAS_DERIVADAS_PRATICADAS "
            sql = sql + "where codigo_derivado = '" + pcodigo + "'"

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

    Public Function GetTarifasDerivadasById(ByVal pcodigo As String) As List(Of AppTarifasDerivadas)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifasDerivadas)

        Dim strSQL As String = "select t.codigo_derivado"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.fator, 0) AS fator"
        strSQL = strSQL + ", nvl(t.HORARIO_APLICACAO, '') AS horario"
        strSQL = strSQL + " from TARIFAS_DERIVADAS_PRATICADAS t where codigo_derivado='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifasDerivadas

                _registro.codigo = reader.Item("codigo_derivado").ToString
                _registro.descricao = reader.Item("descricao").ToString
                _registro.fator = reader.Item("fator").ToString
                _registro.horario = reader.Item("horario").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function GetTarifasDerivadasbyBasica(ByVal codigobasica As String, ByVal livre As String) As List(Of AppTarifasDerivadas)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifasDerivadas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select"
        strSQL = strSQL + " p1.codigo_derivado codigo,"
        strSQL = strSQL + " decode(to_number(to_char(to_date('01/01/1995 00:00:00','DD/MM/YYYY HH24:MI:SS')+"
        strSQL = strSQL + " (p1.horario_aplicacao /86400),'DD'))-1,"
        strSQL = strSQL + " 0,'DOM',1,'SEG',2,'TER',3,'QUA',4,'QUI',5,'SEX',6,'SAB')"
        strSQL = strSQL + " ||' '||to_char(to_date('01/01/1995 00:00:00','DD/MM/YYYY HH24:MI:SS')+"
        strSQL = strSQL + " (p1.horario_aplicacao/86400),'HH24:MI') horario,"
        strSQL = strSQL + " p1.fator,"
        strSQL = strSQL + " nvl(p1.descricao,' ')descricao,"
        strSQL = strSQL + " p1.horario_aplicacao horario2"
        strSQL = strSQL + " from tarifas_derivadas_praticadas p1 where 1=1"
        If livre = "1" Then
            strSQL = strSQL + " and not exists("
            strSQL = strSQL + " select 0 from basica_derivada p2"
            strSQL = strSQL + " where p2.codigo_derivado=p1.codigo_derivado"
            strSQL = strSQL + " and p2.codigo_basico='" + codigobasica + "')"
        End If
        If livre = "0" Then
            strSQL = strSQL + " and exists("
            strSQL = strSQL + " select 0 from basica_derivada p2"
            strSQL = strSQL + " where p2.codigo_derivado=p1.codigo_derivado"
            strSQL = strSQL + " and p2.codigo_basico='" + codigobasica + "')"
        End If

        strSQL = strSQL + " order by horario2"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifasDerivadas

                _registro.codigo = reader.Item("codigo").ToString
                _registro.horario = reader.Item("horario").ToString
                _registro.descricao = reader.Item("descricao").ToString
                _registro.fator = reader.Item("fator").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal TarifaDerivada As AppTarifasDerivadas, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into TARIFAS_DERIVADAS_PRAT_LOG (codigo_log, usuario_log, data_log, tipo_log"
        sql = sql + " ,CODIGO_DERIVADO,FATOR,HORARIO_APLICACAO,FLAG_PROTECAO)  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from TARIFAS_DERIVADAS_PRAT_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        sql = sql + " '" & TarifaDerivada.codigo & "',"
        sql = sql + " '" & Replace(TarifaDerivada.fator, ",", ".") & "',"
        sql = sql + " '" & TarifaDerivada.horario & "',"
        sql = sql + " 'N')"
        Return sql

    End Function

    Public Function GravaBasicaDerivada(ByVal codigobasica As String, ByVal paises_tarifa As DataTable) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "delete from basica_derivada where codigo_basico=" + codigobasica
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()


            For Each row As DataRow In paises_tarifa.Rows
                sql = "insert into basica_derivada(codigo_basico,codigo_derivado)values("
                sql = sql + "" + Trim(codigobasica) + "," + Trim(row.Item(0)) + ")"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next


            ''''LOG''''sql = String_log("", "N", Usuario)

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

