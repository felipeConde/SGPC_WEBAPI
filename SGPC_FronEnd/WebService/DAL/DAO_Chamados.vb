Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Chamados

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


    Public Function Insert(ByVal registro As AppChamado, ByVal usuario As String, Optional codigo_item As String = "") As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into CHAMADOS(oem "
            strSQL = strSQL + " ,tipo_item, abertura"
            strSQL = strSQL + " ,fechamento,status,responsavel,prioridade,texto,tipo_chamado) "
            strSQL = strSQL + "values ('" + registro.OEM + "'"
            strSQL = strSQL + ",'" + registro.Tipo_item + "'"
            strSQL = strSQL + ",TO_DATE('" + registro.Abertura + "', 'DD/MM/YYYY')"
            strSQL = strSQL + ",TO_DATE('" + registro.Fechamento + "', 'DD/MM/YYYY')"
            strSQL = strSQL + ",'" + registro.Status + "'"
            strSQL = strSQL + ",'" + registro.Responsavel + "'"
            strSQL = strSQL + ",'" + registro.Prioridade + "'"
            strSQL = strSQL + ",'" + registro.Texto + "'"
            strSQL = strSQL + ",'" + registro.Tipo_chamado + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If codigo_item <> "" Then
                strSQL = "insert into CHAMADOS_ITEMS(oem ,codigo_item,codigo_tipo ) "
                strSQL = strSQL + " values ('" + registro.OEM + "'"
                strSQL = strSQL + ",'" + codigo_item + "','" & registro.Tipo_item & "')"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If

            strSQL = String_log("'" + registro.OEM + "'", "'" + codigo_item + "'", "'" + registro.Tipo_item + "'", "N", usuario)
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
            connection.Close()
            connection.Dispose()
            Return False
        End Try

        Return True

    End Function

    Public Function Update(ByVal registro As AppChamado, ByVal old_oem As String, ByVal old_item As String, ByVal old_tipo As String, ByVal usuario As String, ByVal codigo_item As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = String_log("'" + old_oem + "'", "'" + old_item + "'", "'" + old_tipo + "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update CHAMADOS set "
            strSQL = strSQL + " oem='" + registro.OEM + "'"
            strSQL = strSQL + " ,tipo_item='" + registro.Tipo_item + "'"
            strSQL = strSQL + " ,abertura= TO_DATE('" + registro.Abertura + "', 'DD/MM/YYYY')"
            strSQL = strSQL + " ,fechamento= TO_DATE('" + registro.Fechamento + "', 'DD/MM/YYYY')"
            strSQL = strSQL + " ,status='" + registro.Status + "'"
            strSQL = strSQL + " ,responsavel='" + registro.Responsavel + "'"
            strSQL = strSQL + " ,prioridade='" + registro.Prioridade + "'"
            strSQL = strSQL + " ,texto='" + registro.Texto + "'"
            strSQL = strSQL + " ,tipo_chamado='" + registro.Tipo_chamado + "'"

            strSQL = strSQL + " where oem = '" + old_oem + "' "
            strSQL = strSQL + " and tipo_item = '" + old_tipo + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update CHAMADOS_ITEMS set "
            strSQL = strSQL + " oem='" + registro.OEM + "'"
            strSQL = strSQL + " where oem = '" + old_oem + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" + registro.OEM + "'", "'" + codigo_item + "'", "'" + registro.Tipo_item + "'", "B", usuario)
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

    Public Function Excluir(ByVal registro As AppChamado, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = String_log("'" + registro.OEM + "'", "''", "'" + registro.Tipo_item + "'", "D", usuario)


            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete CHAMADOS_ITEMS "
            strSQL = strSQL + " where oem = '" + registro.OEM + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete CHAMADOS "
            strSQL = strSQL + " where oem = '" + registro.OEM + "' "
            strSQL = strSQL + " and tipo_item = '" + registro.Tipo_item + "' "

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


    Public Function GetByCodigoNoItem(ByVal oem As String, ByVal tipo As String) As List(Of AppChamado)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppChamado)

        Dim strSQL As String = "select p1.oem "
        strSQL = strSQL + " ,p1.abertura"
        strSQL = strSQL + " ,p1.fechamento"
        strSQL = strSQL + " ,p1.status"
        strSQL = strSQL + " ,p1.responsavel"
        strSQL = strSQL + " ,p1.prioridade"
        strSQL = strSQL + " ,p1.texto"
        strSQL = strSQL + " ,p1.tipo_item"
        strSQL = strSQL + " ,p1.tipo_chamado"

        strSQL = strSQL + " FROM CHAMADOS p1 "
        strSQL = strSQL + " where  p1.oem = '" + oem + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppChamado()

                _registro.OEM = reader.Item("oem").ToString
                _registro.Abertura = reader.Item("abertura").ToString
                _registro.Fechamento = reader.Item("fechamento").ToString
                _registro.Status = reader.Item("status").ToString
                _registro.Responsavel = reader.Item("responsavel").ToString
                _registro.Prioridade = reader.Item("prioridade").ToString
                _registro.Texto = reader.Item("texto").ToString
                _registro.Tipo_item = reader.Item("tipo_item").ToString
                _registro.Tipo_chamado = reader.Item("tipo_chamado").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function GetByCodigo(ByVal oem As String, ByVal item As String, ByVal tipo As String) As List(Of AppChamado)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppChamado)

        Dim strSQL As String = "select p1.oem "
        strSQL = strSQL + " ,p1.abertura"
        strSQL = strSQL + " ,p1.fechamento"
        strSQL = strSQL + " ,p1.status"
        strSQL = strSQL + " ,p1.responsavel"
        strSQL = strSQL + " ,p1.prioridade"
        strSQL = strSQL + " ,p1.texto"
        strSQL = strSQL + " ,p1.tipo_chamado"

        strSQL = strSQL + " FROM CHAMADOS p1 "
        If oem <> "" Then
            strSQL = strSQL + " ,CHAMADOS_ITEMS p2 "
        End If
        strSQL = strSQL + " where p1.oem = p2.oem "

        If oem <> "" Then
            strSQL = strSQL + " and  p1.oem = '" + oem + "' "
            strSQL = strSQL + " and  p2.codigo_item = '" + item + "' "
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppChamado()

                _registro.OEM = reader.Item("oem").ToString
                _registro.Abertura = reader.Item("abertura").ToString
                _registro.Fechamento = reader.Item("fechamento").ToString
                _registro.Status = reader.Item("status").ToString
                _registro.Responsavel = reader.Item("responsavel").ToString
                _registro.Prioridade = reader.Item("prioridade").ToString
                _registro.Texto = reader.Item("texto").ToString
                _registro.Tipo_chamado = reader.Item("tipo_chamado").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function String_log(ByVal poem As String, ByVal pitem As String, ByVal ptipo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into CHAMADOS_LOG(codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + " oem,codigo_item,tipo_item, abertura,fechamento,status,responsavel,prioridade,texto,tipo_chamado) "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from CHAMADOS_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If poem <> "" Then
            sql = sql + "" + poem + "," + pitem + "," + ptipo + ""
            sql = sql + "         ,(select abertura"
            sql = sql + "             from chamados"
            sql = sql + "            where oem = " + poem + ""
            sql = sql + "              and codigo_item = " + pitem + ""
            sql = sql + "              and tipo_item = " + ptipo + "),"
            sql = sql + "          (select fechamento"
            sql = sql + "             from chamados"
            sql = sql + "           where oem = " + poem + ""
            sql = sql + "               and codigo_item = " + pitem + ""
            sql = sql + "               and tipo_item = " + ptipo + "),"
            sql = sql + "           (select status"
            sql = sql + "            from chamados"
            sql = sql + "            where oem = " + poem + ""
            sql = sql + "               and codigo_item = " + pitem + ""
            sql = sql + "               and tipo_item = " + ptipo + "),"
            sql = sql + "           (select responsavel"
            sql = sql + "              from chamados"
            sql = sql + "             where oem = " + poem + ""
            sql = sql + "              and codigo_item = " + pitem + ""
            sql = sql + "                and tipo_item = " + ptipo + "),"
            sql = sql + "            (select prioridade"
            sql = sql + "              from chamados"
            sql = sql + "            where oem = " + poem + ""
            sql = sql + "               and codigo_item = " + pitem + ""
            sql = sql + "               and tipo_item = " + ptipo + "),"
            sql = sql + "           (select texto"
            sql = sql + "               from chamados"
            sql = sql + "             where oem = " + poem + ""
            sql = sql + "               and codigo_item = " + pitem + ""
            sql = sql + "              and tipo_item = " + ptipo + "),"
            sql = sql + "           (select tipo_chamado"
            sql = sql + "               from chamados"
            sql = sql + "             where oem = " + poem + ""
            sql = sql + "               and codigo_item = " + pitem + ""
            sql = sql + "              and tipo_item = " + ptipo + ")"
            sql = sql + "          "
            sql = sql + "          )"
        End If
        Return sql
    End Function

End Class
