Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_HorarioTarifacao
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
    Public Function GetDDiCombo() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select codigo_ddi codigo,descricao nome_grupo from codigo_ddi order by descricao"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric

                _registro.Codigo = reader.Item("codigo").ToString
                _registro.Descricao = reader.Item("nome_grupo").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function GetMaximumCode_Horario_Tarif_OP() As String
        Dim connection As New OleDbConnection(strConn)

        Dim sql As String = "select nvl(max(codigo_horario),1) as codigo from horarios_tarifacao_op_teste"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("codigo").ToString
            End While
        End Using

        Return ""

    End Function

    Public Function GetMaximumCodePlusONE_TarifasTeste() As String
        Dim connection As New OleDbConnection(strConn)

        Dim sql As String = "select nvl(max(codigo),0)+1 as codigo from tarifas_teste"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("codigo").ToString
            End While
        End Using

        Return ""

    End Function

    Public Function InsereHorarioTarifacao(ByVal horario_tarifacao As AppHorario_Tarifacao_OP, ByVal grade_horarios As DataTable, ByVal codigo_ddi As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim reader As OleDbDataReader
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim codigo_horario As String = (GetMaximumCode_Horario_Tarif_OP() + 1).ToString

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into horarios_tarifacao_op_teste(codigo_operadora,codigo_tipo_ligacao,"
            If codigo_ddi <> 0 Then
                sql = sql + " codigo_ddi,"
            End If
            sql = sql + " codigo_horario,ativo,descricao) values( '" + horario_tarifacao.Codigo_operadora.ToString + "',"
            sql = sql + " '" + horario_tarifacao.Codigo_tipo_ligacao.ToString + "', "
            If codigo_ddi <> 0 Then
                sql = sql + " '" + codigo_ddi + "',"
            End If
            sql = sql + " '" + codigo_horario + "',"
            sql = sql + " '" + horario_tarifacao.Ativo + "', '" + horario_tarifacao.Descricao + "' )"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Dim connection2 As New OleDbConnection(strConn)
        Dim strSQL As String = String_log(codigo_horario, "N", usuario)
        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = strSQL
        connection2.Open()
        reader = cmd2.ExecuteReader
        connection2.Close()

        If ApagaDetalhesHorarioTarifacao(horario_tarifacao, codigo_ddi) = False Then
            Return False
        End If

        For Each row As DataRow In grade_horarios.Rows
            If InsereGradeHorarios(row, codigo_horario) = False Then
                Return False
            End If
        Next

        If InsereHorarioZero(codigo_horario) = False Then
            Return False
        End If

        Return True

    End Function

    Public Function ApagaDetalhesHorarioTarifacao(ByVal horario_tarifacao As AppHorario_Tarifacao_OP, ByVal codigo_ddi As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim connection2 As New OleDbConnection(strConn)
        Dim connection3 As New OleDbConnection(strConn)
        Dim connection4 As New OleDbConnection(strConn)
        Dim reader As OleDbDataReader
        Dim strSQL As String = ""
        Dim codigo_horario As String = ""
        Dim list_cod_tarif As New List(Of String)

        strSQL = "select codigo_horario from horarios_tarifacao_op_teste "
        strSQL = strSQL + " where codigo_operadora='" + horario_tarifacao.Codigo_operadora.ToString + "'"
        strSQL = strSQL + " and codigo_tipo_ligacao='" + horario_tarifacao.Codigo_tipo_ligacao.ToString + "'"
        If codigo_ddi <> 0 Then
            strSQL = strSQL + " and codigo_ddi='" + codigo_ddi + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                codigo_horario = reader.Item("codigo_horario").ToString
            End While
        End Using

        connection.Close()

        strSQL = "select codigo_tarifa from horarios_tarifacao_teste "
        strSQL = strSQL + " where codigo='" + codigo_horario + "'"

        Dim cmd4 As OleDbCommand = connection4.CreateCommand
        cmd4.CommandText = strSQL
        connection4.Open()
        reader = cmd4.ExecuteReader
        Using connection4
            While reader.Read
                list_cod_tarif.Add(reader.Item("codigo_tarifa").ToString)
            End While
        End Using

        connection4.Close()

        For Each codigo_tarifa As String In list_cod_tarif
            strSQL = "delete from tarifas_teste where codigo='" + codigo_tarifa + "'"
            Dim cmd2 As OleDbCommand = connection2.CreateCommand
            cmd2.CommandText = strSQL
            connection2.Open()
            reader = cmd2.ExecuteReader
            connection2.Close()
        Next


        strSQL = "delete from horarios_tarifacao_teste where codigo=" + codigo_horario + ""
        Dim cmd3 As OleDbCommand = connection3.CreateCommand
        cmd3.CommandText = strSQL
        connection3.Open()
        reader = cmd3.ExecuteReader

        connection3.Close()

        Return True
    End Function

    Public Function InsereGradeHorarios(ByVal row As DataRow, ByVal codigo_horario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim data As String = ""
        Dim dia As String = ""
        Dim tipo_tarifa As String = ""
        Dim horario_final As String = ""

        If row.Item(1).ToString.Replace(" ", "") = "00:00" Then
            Return True
        Else
            Dim horario As String() = row.Item(1).ToString.Split(New Char() {" "c})
            If horario(0) = "DOM" Then
                horario(0) = "0"
            ElseIf horario(0) = "SEG" Then
                horario(0) = "1"
            ElseIf horario(0) = "TER" Then
                horario(0) = "2"
            ElseIf horario(0) = "QUA" Then
                horario(0) = "3"
            ElseIf horario(0) = "QUI" Then
                horario(0) = "4"
            ElseIf horario(0) = "SEX" Then
                horario(0) = "5"
            ElseIf horario(0) = "SAB" Then
                horario(0) = "6"
            End If

            data = (1 + FormatCurrency(horario(0))) / 100
            dia = Mid(data, 3, 2)

            horario_final = geraHorario(dia, horario(1)).Replace(" ", "")
        End If

        If row.Item(7).ToString = "N" Then
            tipo_tarifa = "4"
        ElseIf row.Item(7).ToString = "D" Then
            tipo_tarifa = "8"
        ElseIf row.Item(7).ToString = "R" Then
            tipo_tarifa = "2"
        Else
            tipo_tarifa = "1"
        End If

        Dim max_tarif_codeplusone As String = GetMaximumCodePlusONE_TarifasTeste()

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into horarios_tarifacao_teste(horario,codigo,codigo_tarifa,tipo_tarifa)values("
            sql = sql + "'" + horario_final + "',"
            sql = sql + "'" + codigo_horario.ToString + "',"
            sql = sql + "'" + max_tarif_codeplusone + "',"
            sql = sql + "'" + tipo_tarifa + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "insert into tarifas_teste(codigo,ttm,valor_ttm,step,valor_step)values("
            sql = sql + "'" + max_tarif_codeplusone + "',"
            sql = sql + "'" + row.Item(3).ToString + "',"
            sql = sql + "'" + row.Item(4).ToString.Replace(",", ".") + "',"
            sql = sql + "'" + row.Item(5).ToString + "',"
            sql = sql + "'" + row.Item(6).ToString.Replace(",", ".") + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Return True
    End Function

    Public Function ReturnGradeHorarioByID(ByVal pcodigo As String) As List(Of AppHorarioTarifa)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppHorarioTarifa)

        Dim strSQL As String = " select * from horarios_tarifacao_teste ht, tarifas_teste tt"
        strSQL = strSQL + " where ht.codigo_tarifa = tt.codigo"
        strSQL = strSQL + " and ht.codigo = '" + pcodigo + "'"
        strSQL = strSQL + " and ht.horario <> 604800"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppHorarioTarifa()

                _registro.Horario = reader.Item("horario").ToString
                _registro.TTM = reader.Item("ttm").ToString
                _registro.TTM_Value = reader.Item("valor_ttm").ToString
                _registro.Step_ = reader.Item("step").ToString
                _registro.Step_value = reader.Item("valor_step").ToString
                _registro.Tipo_tarifa = reader.Item("tipo_tarifa").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function InsereHorarioZero(ByVal codigo_horario As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Dim connection2 As New OleDbConnection(strConn)
        Dim connection3 As New OleDbConnection(strConn)
        Dim connection4 As New OleDbConnection(strConn)
        Dim reader As OleDbDataReader
        Dim sql As String = ""
        Dim list As New List(Of String)
        Dim max_tarif_codeplusone As String = GetMaximumCodePlusONE_TarifasTeste()

        sql = "insert into horarios_tarifacao_teste(horario,codigo,codigo_tarifa,tipo_tarifa)values("
        sql = sql + "604800,"
        sql = sql + "" + Trim(codigo_horario) + ","
        sql = sql + "'" + max_tarif_codeplusone + "',"
        sql = sql + "'4')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        connection.Open()
        reader = cmd.ExecuteReader
        connection.Close()

        sql = "insert into tarifas_teste(codigo,ttm,valor_ttm,step,valor_step)values("
        sql = sql + "'" + max_tarif_codeplusone + "',"
        sql = sql + "0,"
        sql = sql + "0,"
        sql = sql + "0,"
        sql = sql + "0)"

        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = sql
        connection2.Open()
        reader = cmd2.ExecuteReader
        connection2.Close()

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

    Public Function AtualizaHorarioTarifacao(ByVal horario_tarifacao As AppHorario_Tarifacao_OP, ByVal grade_horarios As DataTable, ByVal codigo_ddi As String, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim reader As OleDbDataReader
        Dim sql As String = ""

        Dim connection2 As New OleDbConnection(strConn)
        Dim strSQL As String = String_log(horario_tarifacao.Codigo_horario, "A", usuario)
        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = strSQL
        connection2.Open()
        reader = cmd2.ExecuteReader
        connection2.Close()

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "update horarios_tarifacao_op_teste set codigo_operadora='" + horario_tarifacao.Codigo_operadora.ToString + "',"
            sql = sql + " codigo_tipo_ligacao='" + horario_tarifacao.Codigo_tipo_ligacao.ToString + "',"
            If horario_tarifacao.Codigo_DDI = 0 Then
                sql = sql + " codigo_ddi='',"
            Else
                sql = sql + " codigo_ddi='" + horario_tarifacao.Codigo_DDI.ToString + "',"
            End If
            sql = sql + " ativo='" + horario_tarifacao.Ativo + "',"
            sql = sql + " descricao='" + horario_tarifacao.Descricao + "'"
            sql = sql + " where codigo_horario='" + horario_tarifacao.Codigo_horario.ToString + "'"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Dim connection3 As New OleDbConnection(strConn)
        strSQL = String_log(horario_tarifacao.Codigo_horario, "B", usuario)
        Dim cmd3 As OleDbCommand = connection3.CreateCommand
        cmd3.CommandText = strSQL
        connection3.Open()
        reader = cmd3.ExecuteReader
        connection3.Close()

        If ApagaDetalhesHorarioTarifacao(horario_tarifacao, codigo_ddi) = False Then
            Return False
        End If

        For Each row As DataRow In grade_horarios.Rows
            If InsereGradeHorarios(row, horario_tarifacao.Codigo_horario) = False Then
                Return False
            End If
        Next

        If InsereHorarioZero(horario_tarifacao.Codigo_horario) = False Then
            Return False
        End If

        Return True

    End Function

    Public Function ExcluiHorarioTarifacao(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim tarifa_codes As New List(Of String)
        Dim reader As OleDbDataReader


        tarifa_codes = GetListTarifCodes(pcodigo)

        Dim connection2 As New OleDbConnection(strConn)
        Dim strSQL As String = String_log(pcodigo, "D", usuario)
        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = strSQL
        connection2.Open()
        reader = cmd2.ExecuteReader
        connection2.Close()

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'sql = String_log(pcodigo, "D", usuario)

            For Each tarifa_code As String In tarifa_codes
                sql = "delete from tarifas_teste where codigo='" + tarifa_code + "'"
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            sql = "delete from horarios_tarifacao_teste where codigo='" + pcodigo + "'"
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "delete from horarios_tarifacao_op_teste where codigo_horario='" + pcodigo + "'"
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

    Public Function GetListTarifCodes(ByVal pcodigo As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select codigo_tarifa from horarios_tarifacao_teste where codigo='" + pcodigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                list.Add(reader.Item("codigo_tarifa").ToString)
            End While
        End Using

        Return list
    End Function

    Public Function GetHorarioTarifOPByID(ByVal pcodigo As Integer) As List(Of AppHorario_Tarifacao_OP)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppHorario_Tarifacao_OP)

        Dim strSQL As String = "select CODIGO_HORARIO,"
        strSQL = strSQL + " nvl(CODIGO_OPERADORA, 0)CODIGO_OPERADORA, "
        strSQL = strSQL + " nvl(CODIGO_TIPO_LIGACAO, 0)CODIGO_TIPO_LIGACAO, "
        strSQL = strSQL + " nvl(ATIVO, '')ATIVO, "
        strSQL = strSQL + " nvl(DESCRICAO, '')DESCRICAO, "
        strSQL = strSQL + " nvl(CODIGO_DDI, 0)CODIGO_DDI "
        strSQL = strSQL + " from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppHorario_Tarifacao_OP

                _registro.Codigo_horario = reader.Item("CODIGO_HORARIO").ToString
                _registro.Codigo_operadora = reader.Item("CODIGO_OPERADORA").ToString
                _registro.Codigo_tipo_ligacao = reader.Item("CODIGO_TIPO_LIGACAO").ToString
                _registro.Ativo = reader.Item("ATIVO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.Codigo_DDI = reader.Item("CODIGO_DDI").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function RetornaComboHoraDiaValues(ByVal horario As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim horario_string As String = ""
        Dim strSQL As String = ""

        strSQL = " select distinct decode(to_number(to_char(to_date('01/01/1995 00:00:00',"
        strSQL = strSQL + " 'DD/MM/YYYY HH24:MI:SS')+(ht.horario / 86400), 'DD')) - 1,"
        strSQL = strSQL + " 0, 'DOM',1, 'SEG',2, 'TER',3, 'QUA',4, 'QUI',5, 'SEX',6, 'SAB') || ' ' ||"
        strSQL = strSQL + " to_char(to_date('01/01/1995 00:00:00', 'DD/MM/YYYY HH24:MI:SS') +"
        strSQL = strSQL + "       (ht.horario / 86400),'HH24:MI') horario2"
        strSQL = strSQL + " from horarios_tarifacao_teste ht "
        strSQL = strSQL + " where ht.horario = '" + horario + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                horario_string = reader.Item("horario2").ToString
            End While
        End Using
        Return horario_string
    End Function


    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into horarios_tarifacao_op_log (codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + "CODIGO_HORARIO,CODIGO_OPERADORA,CODIGO_TIPO_LIGACAO "
        sql = sql + " ,ATIVO,DESCRICAO,CODIGO_DDI )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from horarios_tarifacao_op_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + " '" + pcodigo + "', "
            sql = sql + " (select CODIGO_OPERADORA from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo + "'),"
            sql = sql + " (select CODIGO_TIPO_LIGACAO from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo + "'),"
            sql = sql + " (select ATIVO from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo + "'),"
            sql = sql + " (select DESCRICAO from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo + "'),"
            sql = sql + " (select CODIGO_DDI from horarios_tarifacao_op_teste where CODIGO_HORARIO='" + pcodigo + "'))"
        End If
        Return sql

    End Function
End Class
