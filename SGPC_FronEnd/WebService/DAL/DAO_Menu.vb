Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Configuration
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System
Imports System.Object

Public Class DAO_Menu

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

    Public Function InsertNewMenu(ByVal idioma As String, ByVal tipo_log As Char, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim _dao_commons As New DAO_Commons
        _dao_commons.strConn = strConn

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into menuleft  "
            strSQL = strSQL + "(ID,LABEL,URL,IDIOMA,ORDEM) "
            strSQL = strSQL + " values ('" & _dao_commons.GetMaximumCode("ID", "menuleft") & "'"
            strSQL = strSQL + " ,'Novo Menu'"
            strSQL = strSQL + " ,'#'"
            strSQL = strSQL + " ,'" + idioma + "'"
            strSQL = strSQL + " ,'" & _dao_commons.GetMaximumCode("ORDEM", "menuleft") & "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'strSQL = "insert into menuleft_log  "
            'strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,ID,LABEL,URL,IDIOMA,ORDEM) "
            'strSQL = strSQL + " values ('" & _dao_commons.GetMaximumCode("codigo_log", "menuleft_log") & "','" + usuario + "',"
            'strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + " ,'" + tipo_log + "'"
            'strSQL = strSQL + " ,(select nvl(max(ID),0) from menuleft)"
            'strSQL = strSQL + " ,'Novo Menu'"
            'strSQL = strSQL + " ,'#'"
            'strSQL = strSQL + " ,'" + idioma + "'"
            'strSQL = strSQL + " ,(select nvl(max(ORDEM),0) from menuleft)"
            'strSQL = strSQL + ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

            strSQL = "insert into relatorios  "
            strSQL = strSQL + "(CODIGO,nome,URL,IDIOMA,ORDEM,ID_MENU) "
            strSQL = strSQL + " values ('" & _dao_commons.GetMaximumCode("codigo", "relatorios") & "'"
            strSQL = strSQL + " ,'Novo Menu'"
            strSQL = strSQL + " ,'#'"
            strSQL = strSQL + " ,'" + idioma + "'"
            strSQL = strSQL + " ,'" & _dao_commons.GetMaximumCode("ORDEM", "relatorios") & "'"
            strSQL = strSQL + " ,'" & _dao_commons.GetMaximumCode("ID", "menuleft") & "'"
            strSQL = strSQL + ")"

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
    End Function

    Public Function UpdateMenu(ByVal list_cats As List(Of String), ByVal menu_id As String, ByVal label As String, ByVal url As String, ByVal idioma As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "  "
            strSQL = strSQL + "insert into menuleft_log  "
            strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,ID,LABEL,URL,IDIOMA,ORDEM) "
            strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from menuleft_log),'" + usuario + "',"
            strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,'A'"
            strSQL = strSQL + " ,'" + menu_id + "'"
            strSQL = strSQL + " ,(select LABEL from menuleft where Id='" + menu_id + "')"
            strSQL = strSQL + " ,'" + GetUrlMenus(menu_id) + "'"
            strSQL = strSQL + " ,'" + GetIdiomaMenus(menu_id) + "'"
            strSQL = strSQL + " ,'" + GetOrderMenus(menu_id) + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update menuleft set "
            strSQL = strSQL + " LABEL='" + label + "'"
            strSQL = strSQL + " ,url='" + url + "'"
            strSQL = strSQL + " ,idioma='" + idioma + "'"
            strSQL = strSQL + " where ID='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from menuleft_categorias "
            strSQL = strSQL + " where ID='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            For Each cat As String In list_cats

                strSQL = "insert into menuleft_categorias (ID, TIPO_USUARIO) "
                strSQL = strSQL + " values ('" + menu_id + "','" + cat + "')"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            Next

            strSQL = "update relatorios set "
            strSQL = strSQL + " nome='" & label & "'"
            strSQL = strSQL + " ,url='" & url & "'"
            strSQL = strSQL + " ,idioma='" & idioma & "'"
            strSQL = strSQL + " where ID_MENU='" & menu_id & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into menuleft_log  "
            strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,ID,LABEL,URL,IDIOMA,ORDEM) "
            strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from menuleft_log),'" + usuario + "',"
            strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + " ,'B'"
            strSQL = strSQL + " ,'" + menu_id + "'"
            strSQL = strSQL + " ,'" + label + "'"
            strSQL = strSQL + " ,'" + url + "'"
            strSQL = strSQL + " ,'" + idioma + "'"
            strSQL = strSQL + " ,'" + GetOrderMenus(menu_id) + "'"
            strSQL = strSQL + ")"

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

    End Function

    Public Function RemoveSelectedMenu(ByVal menu_id As String, ByVal menu_label As String, ByVal tipo_log As Char, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "delete from menuleft_categorias "
            strSQL = strSQL + " where ID='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from menuleft  "
            strSQL = strSQL + "where id='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete from relatorios  "
            strSQL = strSQL + "where ID_MENU='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'strSQL = "insert into menuleft_log  "
            'strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,ID,LABEL,URL,IDIOMA,ORDEM) "
            'strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from menuleft_log),'" + usuario + "',"
            'strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + " ,'" + tipo_log + "'"
            'strSQL = strSQL + " ,'" + menu_id + "'"
            'strSQL = strSQL + " ,'" + menu_label + "'"
            'strSQL = strSQL + " ,'" + GetUrlMenus(menu_id) + "'"
            'strSQL = strSQL + " ,'" + GetIdiomaMenus(menu_id) + "'"
            'strSQL = strSQL + " ,'" + GetOrderMenus(menu_id) + "'"
            'strSQL = strSQL + ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

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

    Public Function GetCatsMenus(ByVal menu_id As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of String)

        Dim strSQL As String = "select TIPO_USUARIO"
        strSQL = strSQL + " from menuleft_categorias where ID='" + menu_id + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("TIPO_USUARIO").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetMenus() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppGeneric)

        Dim strSQL As String = "select rel.CODIGO"
        strSQL = strSQL + " ,ml.LABEL, ml.ID"
        strSQL = strSQL + " from menuleft ml, relatorios rel where ml.IDIOMA='BR' and ml.id = rel.id_menu order by ml.ORDEM"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("ID").ToString, reader.Item("LABEL").ToString, reader.Item("CODIGO").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetReportList(Optional ByVal report_code As String = "") As List(Of AppRelatorio)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppRelatorio)

        Dim strSQL As String = "select * from relatorios"
        If report_code <> "" Then
            strSQL = strSQL + " where codigo='" + report_code + "'"
        End If
        strSQL = strSQL + "  order by ordem"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRelatorio()
                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Idioma = reader.Item("IDIOMA").ToString
                _registro.Nome = reader.Item("nome").ToString
                _registro.Url = reader.Item("URL").ToString
                _registro.Ordem = reader.Item("ORDEM").ToString
                _registro.Id_parent = reader.Item("ID_PARENT").ToString
                _registro.Id_menu = reader.Item("ID_MENU").ToString
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetReportListCats(ByVal report_code As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppGeneric)

        Dim strSQL As String = "select * from relatorios_categorias"
        strSQL = strSQL + " where codigo_relatorio='" + report_code + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo_relatorio").ToString, reader.Item("tipo_usuario").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetUrlMenus(ByVal menu_id As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select URl"
        strSQL = strSQL + " from menuleft where id='" + menu_id + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("URL").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function ChangeOrder(ByVal menu_id As String, ByVal order As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            Dim menu_order As String = GetOrderMenus(menu_id)

            If order = "Up" And menu_order = "1" Then
                Return True
            ElseIf order <> "Up" And IsMenuLast(menu_order, GetIdiomaMenus(menu_id)) Then
                Return True
            End If

            Dim menu_id_2 As String = GetIDMenuNext(menu_order, GetIdiomaMenus(menu_id), order)
            Dim menu_order_2 As String = GetOrderMenus(menu_id_2)

            Dim strSQL As String = "update menuleft set ORDEM = '" + menu_order_2 + "'  "
            strSQL = strSQL + " where ID='" + menu_id + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update relatorios set "
            strSQL = strSQL + " ORDEM='" & menu_order_2 & "'"
            strSQL = strSQL + " where ID_MENU='" & menu_id & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update menuleft set ORDEM = '" + menu_order + "'  "
            strSQL = strSQL + " where ID='" + menu_id_2 + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update relatorios set "
            strSQL = strSQL + " ORDEM='" & menu_order & "'"
            strSQL = strSQL + " where ID_MENU='" & menu_id_2 & "'"

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
    End Function

    Public Function ChangeOrderReport(ByVal report_selected As AppRelatorio, ByVal Order As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim report_next As New AppRelatorio
            report_next = GetReportNext(report_selected, Order)


            If Order = "Up" Then

                Dim strSQL As String = "update relatorios set ORDEM ='" & report_next.Ordem & "' "
                strSQL = strSQL + " where codigo='" & report_selected.Codigo & "'"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            Else

                Dim strSQL As String = "update relatorios set ORDEM ='" & report_selected.Ordem & "' "
                strSQL = strSQL + " where codigo='" & report_next.Codigo & "'"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

            If Order <> "Up" Then

                If E_SubMenu(report_next.Codigo) = True Then
                    OrdenaFilhos(report_next.Codigo)
                End If

                Dim strSQL As String = "update relatorios set ORDEM = '" & MaximumOrderForMe(report_next.Codigo) + 1 & "'  "
                strSQL = strSQL + " where codigo='" & report_selected.Codigo & "'"

                ResolveQuery(strSQL)

                If E_SubMenu(report_selected.Codigo) = True Then
                    OrdenaFilhos(report_selected.Codigo)
                End If

            Else

                If E_SubMenu(report_selected.Codigo) = True Then
                    OrdenaFilhos(report_selected.Codigo)
                End If

                Dim strSQL As String = "update relatorios set ORDEM = '" & MaximumOrderForMe(report_selected.Codigo) + 1 & "'  "
                strSQL = strSQL + " where codigo='" & report_next.Codigo & "'"

                ResolveQuery(strSQL)

                If E_SubMenu(report_next.Codigo) = True Then
                    OrdenaFilhos(report_next.Codigo)
                End If

            End If

            OrdenaFilhos(report_selected.Id_parent)

            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try
    End Function

    Public Function GetReportNext(ByVal report As AppRelatorio, ByVal order As String) As AppRelatorio
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""
        Dim _registro As New AppRelatorio()

        If order = "Up" Then
            strSQL = "select *  from (select CODIGO, IDIOMA, NOME, URL, ORDEM, ID_PARENT, ID_MENU, rank() over (order by ORDEM desc) as rnk from relatorios "
            strSQL = strSQL + " where ORDEM < '" + report.Ordem + "' and ID_PARENT='" + report.Id_parent + "'"
        Else
            strSQL = "select *  from (select CODIGO, IDIOMA, NOME, URL, ORDEM, ID_PARENT, ID_MENU, rank() over (order by ORDEM asc) as rnk from relatorios "
            strSQL = strSQL + " where ORDEM > '" + report.Ordem + "' and ID_PARENT='" + report.Id_parent + "'"
        End If
        strSQL = strSQL + " ) where rnk='1'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Idioma = reader.Item("IDIOMA").ToString
                _registro.Nome = reader.Item("nome").ToString
                _registro.Url = reader.Item("URL").ToString
                _registro.Ordem = reader.Item("ORDEM").ToString
                _registro.Id_parent = reader.Item("ID_PARENT").ToString
                _registro.Id_menu = reader.Item("ID_MENU").ToString
                Return _registro
            End While
        End Using

        Return _registro
    End Function

    Public Function GetIDMenuNext(ByVal menu_order As String, ByVal idioma As String, ByVal order As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""

        If order = "Up" Then
            strSQL = "select nvl(max(ID),0) as ID from menuleft "
            strSQL = strSQL + " where ORDEM < '" + menu_order + "'"
        Else
            strSQL = "select nvl(max(ID),0) as ID  from from menuleft "
            strSQL = strSQL + " where ORDEM > '" + menu_order + "'"
        End If
        strSQL = strSQL + " and IDIOMA ='" + idioma + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("ID").ToString
            End While
        End Using

        Return ""
    End Function


    Public Function GetOrderMenus(ByVal menu_id As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select ORDEM"
        strSQL = strSQL + " from menuleft where id='" + menu_id + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("ORDEM").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function GetOrderReport(ByVal report_id As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select ORDEM"
        strSQL = strSQL + " from relatorios where codigo='" + report_id + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("ORDEM").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function GetIdiomaMenus(ByVal menu_id As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select IDIOMA"
        strSQL = strSQL + " from menuleft where id='" + menu_id + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("IDIOMA").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function GetComboIdiomas() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As New OleDbCommand
        Dim reader As OleDbDataReader
        Dim listItem As New List(Of AppGeneric)

        Dim strSQL As String = "select ID,DESCRICAO"
        strSQL = strSQL + " from idiomas "

        cmd.Connection = connection
        cmd.CommandText = strSQL
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim registro As New AppGeneric(reader.Item("ID").ToString, reader.Item("DESCRICAO").ToString)
                listItem.Add(registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function IsMenuLast(ByVal menu_order As String, ByVal idioma As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = " select ID from menuleft where ORDEM > '" + menu_order + "' and idioma='" + idioma + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return False
            End While
        End Using

        Return True
    End Function

    Public Function InsertNewReport(ByVal idioma As String, ByVal id_parent As String, ByVal tipo_log As Char, ByVal usuario As String, ByVal type As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim _dao_commons As New DAO_Commons
        _dao_commons.strConn = strConn

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into relatorios  "
            strSQL = strSQL + "(CODIGO,nome,URL,IDIOMA,ORDEM,ID_PARENT) "
            strSQL = strSQL + " values ('" & _dao_commons.GetMaximumCode("codigo", "relatorios") & "'"
            If type = "submenu" Then
                strSQL = strSQL + " ,'Novo Sub-Menu'"
                strSQL = strSQL + " ,'#'"
            Else
                strSQL = strSQL + " ,'Novo Relatório'"
                strSQL = strSQL + " ,'xml/'"
            End If

            strSQL = strSQL + " ,'" + idioma + "'"
            strSQL = strSQL + " ,'" & MaximumOrderForMe(id_parent) + 1 & "'"
            strSQL = strSQL + " ,'" + id_parent + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'strSQL = "insert into relatorios_log  "
            'strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,CODIGO,nome_usuario,URL,IDIOMA,ORDEM,ID_PARENT) "
            'strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from relatorios_log),'" + usuario + "',"
            'strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + " ,'" + tipo_log + "'"
            'strSQL = strSQL + " ,(select nvl(max(CODIGO),0) from relatorios)"
            'If type = "submenu" Then
            '    strSQL = strSQL + " ,'Novo Sub-Menu'"
            '    strSQL = strSQL + " ,'#'"
            'Else
            '    strSQL = strSQL + " ,'Novo Relatório'"
            '    strSQL = strSQL + " ,'xml/'"
            'End If

            'strSQL = strSQL + " ,'" + idioma + "'"
            'strSQL = strSQL + " ,(select nvl(max(ORDEM),0) from relatorios where ID_PARENT='" + id_parent + "' )"
            'strSQL = strSQL + " ,'" + id_parent + "'"
            'strSQL = strSQL + ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

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

    Public Function RemoveSelectedSub(ByVal report_code As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim report As New AppRelatorio
            report = GetReportList(report_code).Item(0)

            Dim strSQL As String = "delete FROM RELATORIOS_USUARIOS  "
            strSQL = strSQL & " where codigo_relatorio='" & report_code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete FROM RELATORIOS_CATEGORIAS  "
            strSQL = strSQL & " where codigo_relatorio='" & report_code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete relatorios "
            strSQL = strSQL & " where codigo='" & report_code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            'strSQL = "insert into relatorios_log  "
            'strSQL = strSQL & "(codigo_log, usuario_log, data_log, tipo_log,CODIGO,nome_usuario,URL,IDIOMA,ORDEM,ID_PARENT) "
            'strSQL = strSQL & " values ((select nvl(max(codigo_log),0)+1 from relatorios_log),'" & usuario & "',"
            'strSQL = strSQL & " to_date('" & DateTime.Now.ToString & "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL & " ,'D'"
            'strSQL = strSQL & " ,'" & report.Codigo & "'"
            'strSQL = strSQL & " ,'" & report.nome_usuario & "'"
            'strSQL = strSQL & " ,'" & report.Url & "'"
            'strSQL = strSQL & " ,'" & report.Idioma & "'"
            'strSQL = strSQL & " ,'" & report.Ordem & "'"
            'strSQL = strSQL & " ,'" & report.Id_parent & "'"
            'strSQL = strSQL & ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

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

    Public Function GetIDParentFromMenu(ByVal id_menu As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + " from relatorios where ID_MENU='" + id_menu + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("CODIGO").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function ChangeParentReport(ByVal id_parent As String, ByVal codigo As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'Update 

            Dim strSQL As String = "update relatorios set id_parent ='" & id_parent & "', ordem = (select nvl(max(ORDEM),0) + 1 from relatorios where ID_PARENT='" + id_parent + "' or CODIGO = '" + id_parent + "' ) "
            strSQL = strSQL + " where codigo='" & codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

            OrdenaFilhos("1")

            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try
    End Function

    Public Function OrdenaFilhos(ByVal codigo_no As String) As String

        Dim ultimo_sobrinho_ordem As String = "0"

        Dim filhos As New List(Of String)
        Dim count As Integer = 0
        Dim count_sobrinho As Integer = 0

        filhos = GetFilhos(codigo_no)

        For Each filho_code As String In filhos
            If count = 0 Then
                Dim strSQL As String = "update relatorios set ordem = ('" & GetOrderReport(codigo_no) & "' + 1 )"
                strSQL = strSQL + " where codigo='" & filho_code & "'"

                ResolveQuery(strSQL)

                If E_SubMenu(filho_code) = True Then
                    ultimo_sobrinho_ordem = OrdenaFilhos(filho_code)
                End If

                count = count + 1
            Else
                If ultimo_sobrinho_ordem <> "0" Then
                    Dim strSQL As String = "update relatorios set ordem = ('" & ultimo_sobrinho_ordem & "' + '" & count_sobrinho + 1 & "')"
                    strSQL = strSQL + " where codigo='" & filho_code & "'"

                    ResolveQuery(strSQL)

                    If E_SubMenu(filho_code) = True Then
                        ultimo_sobrinho_ordem = OrdenaFilhos(filho_code)
                    End If

                    count = count + 1
                    count_sobrinho = count_sobrinho + 1
                Else
                    Dim strSQL As String = "update relatorios set ordem = ('" & GetOrderReport(codigo_no) & "' + '" & count + 1 & "' )"
                    strSQL = strSQL + " where codigo='" & filho_code & "'"

                    ResolveQuery(strSQL)

                    If E_SubMenu(filho_code) = True Then
                        ultimo_sobrinho_ordem = OrdenaFilhos(filho_code)
                    End If

                    count = count + 1
                End If

            End If
        Next

        If filhos.Count > 0 Then
            If Convert.ToInt32(ultimo_sobrinho_ordem) < (MaiorOrdemSobrinho(filhos.Item(filhos.Count - 1))) Then
                ultimo_sobrinho_ordem = (MaiorOrdemSobrinho(filhos.Item(filhos.Count - 1))).ToString
            End If
        End If

        Return ultimo_sobrinho_ordem

    End Function

    Public Sub ResolveQuery(ByVal strSQL As String)
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
        End Try

    End Sub

    Public Function MaximumOrderForMe(ByVal id_parent As String) As String
        Dim Ordem As String = GetOrderReport(id_parent)

        Dim filhos As New List(Of String)
        Dim count As Integer = 0

        filhos = GetFilhos(id_parent)

        For Each filho_code As String In filhos

            Ordem = GetOrderReport(filho_code)

            If E_SubMenu(filho_code) = True Then
                Ordem = MaximumOrderForMe(filho_code)
            End If

        Next

        Return Ordem
    End Function

    Public Function MaiorOrdemSobrinho(ByVal codigo As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select ORDEM"
        strSQL = strSQL + " from relatorios where CODIGO='" + codigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("ORDEM").ToString
            End While
        End Using

        Return False
    End Function


    Public Function E_SubMenu(ByVal codigo As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + " from relatorios where CODIGO='" + codigo + "' and url='#'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return True
            End While
        End Using

        Return False
    End Function

    Public Function GetFilhos(ByVal id_parent As String) As List(Of String)

        Dim connection As New OleDbConnection(strConn)
        Dim filhos As New List(Of String)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + " from relatorios where ID_PARENT='" + id_parent + "' order by ORDEM"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                filhos.Add(reader.Item("CODIGO").ToString)
            End While
        End Using

        Return filhos

    End Function

    Public Function UpdateReport(ByVal list_cats As List(Of String), ByVal report As AppRelatorio, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim report_old As New AppRelatorio
            report_old = GetReportList(report.Codigo).Item(0)

            'Dim strSQL As String = "insert into relatorios_log  "
            'strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,CODIGO,nome_usuario,URL,IDIOMA,ORDEM,ID_PARENT) "
            'strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from relatorios_log),'" + usuario + "',"
            'strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + " ,'A'"
            'strSQL = strSQL & " ,'" & report_old.Codigo & "'"
            'strSQL = strSQL & " ,'" & report_old.nome_usuario & "'"
            'strSQL = strSQL & " ,'" & report_old.Url & "'"
            'strSQL = strSQL & " ,'" & report_old.Idioma & "'"
            'strSQL = strSQL & " ,'" & report_old.Ordem & "'"
            'strSQL = strSQL & " ,'" & report_old.Id_parent & "'"
            'strSQL = strSQL & ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

            Dim strSQL As String = "update relatorios set "
            strSQL = strSQL + " nome ='" & report.Nome & "'"
            strSQL = strSQL + " ,url='" & report.Url & "'"
            strSQL = strSQL + " ,idioma='" & report.Idioma & "'"
            strSQL = strSQL + " where CODIGO='" & report.Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete  from relatorios_categorias "
            strSQL = strSQL + " where CODIGO_RELATORIO='" & report.Codigo & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            For Each cat As String In list_cats

                strSQL = "insert into relatorios_categorias (CODIGO_RELATORIO, TIPO_USUARIO) "
                strSQL = strSQL + " values ('" & report.Codigo & "','" & cat & "')"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            Next

            'strSQL = "insert into relatorios_log  "
            'strSQL = strSQL + "(codigo_log, usuario_log, data_log, tipo_log,CODIGO,nome_usuario,URL,IDIOMA,ORDEM,ID_PARENT) "
            'strSQL = strSQL + " values ((select nvl(max(codigo_log),0)+1 from relatorios_log),'" + usuario + "',"
            'strSQL = strSQL + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + " ,'B'"
            'strSQL = strSQL & " ,'" & report.Codigo & "'"
            'strSQL = strSQL & " ,'" & report.nome_usuario & "'"
            'strSQL = strSQL & " ,'" & report.Url & "'"
            'strSQL = strSQL & " ,'" & report.Idioma & "'"
            'strSQL = strSQL & " ,'" & report.Ordem & "'"
            'strSQL = strSQL & " ,'" & report.Id_parent & "'"
            'strSQL = strSQL & ")"

            'cmd.CommandText = strSQL
            'cmd.ExecuteNonQuery()

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

    Public Sub GetMenuData(ByRef menu As AppMenu)

        Dim connection As New OleDbConnection(strConn)
        Dim cmd As New OleDbCommand
        Dim reader As OleDbDataReader
        Dim strsql As String = ""
        strsql = strsql + " Select "
        strsql = strsql + " login_usuario,"
        strsql = strsql + " nome_usuario, "
        strsql = strsql + " senha_usuario,"
        strsql = strsql + " codigo "
        strsql = strsql + " From usuarios "
        strsql = strsql + " where codigo='" & UCase(menu.code_user) & "'"

        cmd.Connection = connection
        cmd.CommandText = strsql
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                menu.login_user = reader.Item("login_usuario").ToString
                menu.name_user = reader.Item("nome_usuario").ToString
                menu.passw_user = reader.Item("senha_usuario").ToString
            End While
        End Using

        Me.TabelaIdiomas(menu)

    End Sub

    Public Function Possuisenha_usuario(ByVal menu_data As AppMenu) As String

        'Possuiu acesso Web ?
        Dim connection As New OleDbConnection(strConn)

        Dim strsql As String = ""
        strsql = strsql + "select upper(acesso_web) from usuarios where upper(login_usuario_usuario)='" & UCase(menu_data.login_user) & "'"
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strsql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            If reader.Item("acesso_web").ToString = "N" Or reader.Item("acesso_web").ToString = "" Then
                Return "<script>top.location.href='msg.asp?msg=Acesso web bloqueado';</script>"
            End If
        Else
            Return "<script>top.location.href='/msg.asp?msg=Acesso negado.<BR><BR>Clique <a href=/index.asp target=_top>aqui</a> para tentar novamente.';</script>"
        End If

        Return ""

    End Function

    Public Function Verificasenha_usuario(ByVal menu_data As AppMenu) As String

        ' senha_usuario WEB EXPIROU?
        Dim connection As New OleDbConnection(strConn)

        Dim strsql As String = ""
        strsql = strsql + "select nvl(trunc(expiracao_senha_usuario_web) - trunc(sysdate),'0') as ex_senha_usuario from usuarios where upper(login_usuario_usuario)='" & UCase(menu_data.login_user) & "'"
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strsql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        If reader.Item("ex_senha_usuario") < 0 Or reader.Item("ex_senha_usuario") = 0 Then
            Return "<script>top.location.href='alterar_senha_usuario.asp';</script>"
        End If

        Return ""

    End Function

    Public Function TabelaIdiomas(ByRef menu_data As AppMenu) As String

        'TABELA DE IDIOMAS
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As New OleDbCommand
        Dim reader As OleDbDataReader


        Dim strsql As String = ""
        strsql = strsql + "select id, descricao, link_bandeira from idiomas where lcid='" + menu_data.lcid + "' order by ID"
        cmd.Connection = connection
        cmd.CommandText = strsql
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                menu_data.DescIdioma = reader.Item("descricao").ToString
                menu_data.IdIdioma = reader.Item("id").ToString
            End While
        End Using

        Return "<script>top.location.href='index.asp?page1=inicio&user=10';</script>"

    End Function

    Public Function MontaMenu(ByVal menu_data As AppMenu) As DataTable

        Dim tableMenu As New DataTable()
        Dim strsql As String = ""

        'strsql = "select distinct m.id, label, url, m.id, m.ordem "
        'strsql = strsql + " FROM menuleft m, menuleft_categorias mc, categoria_usuario cu "
        'strsql = strsql + " WHERE m.id=mc.id "
        'strsql = strsql + " AND cu.tipo_usuario=mc.tipo_usuario "
        'strsql = strsql + " AND cu.codigo_usuario='" & menu_data.code_user & "' "
        'strsql = strsql + " AND m.IDIOMA='" & menu_data.IdIdioma & "' "
        'strsql = strsql + " ORDER BY m.ordem asc "

        strsql = "select distinct m.id, label, url, m.id, m.ordem "
        strsql = strsql + " FROM menuleft m, menuleft_usuarios mc "
        strsql = strsql + " WHERE m.id=mc.id "
        strsql = strsql + " AND mc.codigo_usuario='" & menu_data.code_user & "' "
        strsql = strsql + " AND m.IDIOMA='" & menu_data.IdIdioma & "' "
        strsql = strsql + " ORDER BY m.ordem asc "

        tableMenu = myDataTable(strsql)

        If tableMenu.Rows.Count = 0 Then

            strsql = ""
            strsql = "select distinct m.id,label, url, m.id, m.ordem "
            strsql = strsql + " FROM menuleft m, menuleft_categorias mc, categoria_usuario cu "
            strsql = strsql + " WHERE m.id=mc.id "
            strsql = strsql + " AND cu.tipo_usuario=mc.tipo_usuario "
            strsql = strsql + " AND cu.codigo_usuario='" & menu_data.code_user & "' "
            strsql = strsql + " AND m.IDIOMA='" & menu_data.IdIdioma & "' "
            strsql = strsql + " ORDER BY m.ordem asc "

            tableMenu = myDataTable(strsql)
        End If

        Return tableMenu

    End Function

    Public Function MontaRelatorios(ByVal id_parent As String, ByVal usuario As String, ByVal possui_custom As Boolean)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = " select distinct p1.codigo as codigo, p1.nome as nome"
        strSQL = strSQL + " FROM relatorios p1"

        If possui_custom = True Then
            strSQL = strSQL + " , RELATORIOS_USUARIOS p3"
        Else
            strSQL = strSQL + " , RELATORIOS_CATEGORIAS p2"
        End If

        strSQL = strSQL + " where p1.id_parent = '" & id_parent & "'"

        If possui_custom = True Then
            strSQL = strSQL + "  and p3.codigo_usuario = '" & usuario & "' and p3.codigo_relatorio = p1.codigo"
        Else
            strSQL = strSQL + " and p1.codigo = p2.codigo_relatorio"
        End If

        strSQL = strSQL + " and exists (select p100.tipo_usuario"
        strSQL = strSQL + " from RELATORIOS_CATEGORIAS p100"
        strSQL = strSQL + " where p100.codigo_relatorio = p1.codigo"
        strSQL = strSQL + " and p100.tipo_usuario in"
        strSQL = strSQL + " (select p200.tipo_usuario"
        strSQL = strSQL + " from categoria_usuario p200"
        strSQL = strSQL + " where p200.codigo_usuario = '" & usuario & "')) order by nome"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("nome").ToString)
                list.Add(_registro)
            End While
        End Using

        connection.Close()

        Return list
    End Function

    Public Function PossuiCustomizacao(ByVal usuario As String) As Boolean

        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = " select * from RELATORIOS_USUARIOS p3"
        strSQL = strSQL + " where  p3.codigo_usuario = '" & usuario & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            Return True
        Else
            Return False
        End If

    End Function


    Public Function myDataTable(ByVal SQL As String) As DataTable
        Dim cn As OleDbConnection
        Dim dsTemp As DataSet
        Dim dsCmd As OleDbDataAdapter

        cn = New OleDbConnection(strConn)
        cn.Open()

        dsCmd = New OleDbDataAdapter(SQL, cn)
        dsTemp = New DataSet()
        dsCmd.Fill(dsTemp, "myQuery")
        cn.Close()
        Return dsTemp.Tables(0)
    End Function
End Class
