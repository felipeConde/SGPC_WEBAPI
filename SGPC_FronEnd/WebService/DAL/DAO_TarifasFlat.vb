Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_TarifasFlat

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

        strSQL = " select distinct p1.codigo,p1.nome_configuracao as descricao,p1.OPER_CODIGO_OPERADORA, p2.descricao as operadora from tarifacao p1, operadoras_teste p2 where p1.OPER_CODIGO_OPERADORA=p2.codigo(+) "
        strSQL = strSQL + "and  tipo_tarifa <1 order by descricao"

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


    Public Function InsereTarifaFlat(ByVal tarifaFlat As AppTarifaFlat, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into tipos_ligacao_teste (codigo,descricao,ttm "
            sql = sql + " ,valor_ttm,step,valor_step,tipo_chamada,CODIGO_OPERADORA_DESTINO,GESTAO_DDI,codigo_tarif)  "
            sql = sql + " values ( (select nvl(max(codigo),0)+1 from tipos_ligacao_teste), "
            sql = sql + "'" + tarifaFlat.Descricao + "',"
            sql = sql + "'" + tarifaFlat.TTM + "',"
            sql = sql + "to_number('" + Replace(tarifaFlat.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifaFlat.Step_ + "',"
            sql = sql + "to_number('" + Replace(tarifaFlat.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'0',"
            sql = sql + "'" + tarifaFlat.Operadora + "',"
            sql = sql + "'N',"
            sql = sql + "'" + tarifaFlat.Codigo_tarifa + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log("", "N", usuario)

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

    Public Function AtualizaTarifaFlat(ByVal tarifaFlat As AppTarifaFlat, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(tarifaFlat.Codigo, "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update tipos_ligacao_teste set "
            sql = sql + " descricao='" + tarifaFlat.Descricao + "',"
            sql = sql + " ttm='" + tarifaFlat.TTM + "',"
            sql = sql + " valor_ttm= to_number('" + Replace(tarifaFlat.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " step='" + tarifaFlat.Step_ + "',"
            sql = sql + " valor_step= to_number('" + Replace(tarifaFlat.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " tipo_chamada='0',"
            sql = sql + " CODIGO_OPERADORA_DESTINO='" + tarifaFlat.Operadora + "',"
            sql = sql + " GESTAO_DDI='N',"
            sql = sql + " codigo_tarif='" + tarifaFlat.Codigo_tarifa + "'"
            sql = sql + " where CODIGO = '" + tarifaFlat.Codigo.ToString + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(tarifaFlat.Codigo, "B", usuario)

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

    Public Function ExcluiTarifaFlat(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(pcodigo, "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete tipos_ligacao_teste "
            sql = sql + "where codigo = " + pcodigo

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

    Public Function ReturnTotalNumeroA(ByVal pcodigo As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim total As String = ""

        Try
            Dim strSQL As String = " select count(*) as total from("
            strSQL = strSQL + " select distinct rml_numero_a "
            strSQL = strSQL + " from(cdrs_celular_analitico_mv) "
            strSQL = strSQL + " where codigo_conta in "
            strSQL = strSQL + " (select codigo_conta from faturas_arquivos where codigo_fatura = '" + pcodigo + "')) "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    total = reader.Item("total").ToString
                End While
            End Using

            Return total

        Catch ex As Exception
            Return total
        End Try
    End Function

    Public Function GetTarifaFlatById(ByVal pcodigo As Integer) As List(Of AppTarifaFlat)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifaFlat)

        Dim strSQL As String = "select t.CODIGO"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.TTM, '') AS TTM"
        strSQL = strSQL + ", nvl(t.VALOR_TTM, 0) AS VALOR_TTM"
        strSQL = strSQL + ", nvl(t.STEP, '') AS STEP"
        strSQL = strSQL + ", nvl(t.VALOR_STEP, 0) AS VALOR_STEP"
        strSQL = strSQL + ", nvl(t.CODIGO_OPERADORA_DESTINO, 0) AS CODIGO_OPERADORA_DESTINO"
        strSQL = strSQL + ", nvl(t.CODIGO_TARIF, 0) AS CODIGO_TARIF"
        strSQL = strSQL + " from tipos_ligacao_teste t where codigo='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifaFlat

                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.TTM = reader.Item("TTM").ToString
                _registro.TTM_Value = reader.Item("VALOR_TTM").ToString
                _registro.Step_ = reader.Item("STEP").ToString
                _registro.Step_value = reader.Item("VALOR_STEP").ToString
                _registro.Operadora = reader.Item("CODIGO_OPERADORA_DESTINO").ToString
                _registro.Codigo_tarifa = reader.Item("CODIGO_TARIF").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into tipos_ligacao_log (codigo_log, usuario_log, data_log, tipo_log, codigo_tipo_ligacao, descricao,ttm "
        sql = sql + " ,valor_ttm,step,valor_step,tipo_chamada,CODIGO_OPERADORA_DESTINO,GESTAO_DDI,codigo_tarifa)  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from tipos_ligacao_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + " '" + pcodigo + "', "
            sql = sql + " (select descricao from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select TTM from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select VALOR_TTM from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select STEP from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select VALOR_STEP from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " '0',"
            sql = sql + " (select CODIGO_OPERADORA_DESTINO from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + "'N',"
            sql = sql + " (select CODIGO_TARIF from tipos_ligacao_teste where codigo='" + pcodigo + "'))"
        Else
            sql = sql + " (select nvl(max(codigo),0) from tipos_ligacao_teste),"
            sql = sql + " (select descricao from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select TTM from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select VALOR_TTM from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select STEP from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select VALOR_STEP from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " '0',"
            sql = sql + " (select CODIGO_OPERADORA_DESTINO from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + "'N',"
            sql = sql + " (select CODIGO_TARIF from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)))"
        End If
        Return sql

    End Function
End Class
