Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Grupos
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


    Public Function GetGrupos() As List(Of AppGrupo)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppGrupo)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_grupo as descricao "
        strSQL = strSQL + " from grupos o"
        strSQL = strSQL + " order by nome_grupo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGrupo(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetGruposById(ByVal codigo As String) As List(Of AppGrupo)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppGrupo)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_grupo as descricao, nvl(o.tarifavel, '') tarifavel, nvl(o.ativo,'') ativo, nvl(o.id_grupo_parent,'') id_grupo_parent  "
        strSQL = strSQL + " from grupos o"
        strSQL = strSQL + " where o.CODIGO='" + codigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGrupo(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString, reader.Item("tarifavel").ToString, reader.Item("ativo").ToString, reader.Item("id_grupo_parent").ToString)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetGruposByUsuario(ByVal pCodigoUsuario As Integer) As List(Of AppGrupo)
        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppGrupo)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_grupo as descricao "
        strSQL = strSQL + " from grupos o, usuarios u"
        strSQL = strSQL + " where o.codigo=u.grp_codigo and u.codigo='" & pCodigoUsuario & "'"
        strSQL = strSQL + " order by nome_grupo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGrupo(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                _lista.Add(_registro)
            End While
        End Using

        Return _lista
    End Function

    Public Function GetGruposByLinha(ByVal pCodigo As Integer) As List(Of AppGrupo)
        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppGrupo)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_grupo as descricao, nvl(gi.rateio,0) as rateio "
        strSQL = strSQL + " from grupos o, grupos_item gi"
        strSQL = strSQL + " where o.codigo=gi.grupo and gi.item='" & pCodigo & "'"
        strSQL = strSQL + " order by nome_grupo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGrupo(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString, reader.Item("rateio").ToString)
                _lista.Add(_registro)
            End While
        End Using

        Return _lista
    End Function

    Public Function GetGruposByLink(ByVal pCodigo As Integer) As List(Of AppGrupo)
        Dim connection As New OleDbConnection(strConn)
        Dim _lista As New List(Of AppGrupo)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_grupo as descricao, nvl(cc.rateio,0) as rateio "
        strSQL = strSQL + " from grupos o, ccusto_links cc"
        strSQL = strSQL + " where o.codigo=cc.CODIGO_CCUSTO and cc.codigo_link='" & pCodigo & "'"
        strSQL = strSQL + " order by nome_grupo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGrupo(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString, reader.Item("rateio").ToString)
                _lista.Add(_registro)
            End While
        End Using

        Return _lista
    End Function

    Public Function InsereGrupos(ByVal grupo As AppGrupo, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into GRUPOS(CODIGO"
        strSQL = strSQL + ", NOME_GRUPO, TARIFAVEL, ATIVO, EMP_CODIGO, TP_GRPO_CODIGO "
        If grupo.Responsavel <> "" Then
            strSQL = strSQL + ", IDE_GRUPO_PARENT) "
        Else
            strSQL = strSQL + " ) "
        End If
        strSQL = strSQL + "values('" + grupo.Codigo + "'"
        strSQL = strSQL + ",'" + grupo.Grupo + "'"
        strSQL = strSQL + ",'" + grupo.Tarifavel + "'"
        strSQL = strSQL + ",'" + grupo.Ativo + "'"
        strSQL = strSQL + ",'2'"
        strSQL = strSQL + ",'1'"
        If grupo.Responsavel <> "" Then
            strSQL = strSQL + ",'" + grupo.Responsavel.ToString + "'"
        End If
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        InsertGruposLog(grupo, "N", user)

        Return True
    End Function

    Public Function AtualizaGrupos(ByVal grupo As AppGrupo, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        InsertGruposLog(GetGruposById(grupo.Codigo).Item(0), "A", user)

        Dim strSQL As String = "update GRUPOS set "
        strSQL = strSQL + "NOME_GRUPO='" + grupo.Grupo + "',"
        strSQL = strSQL + "ATIVO='" + grupo.Ativo + "',"
        strSQL = strSQL + "TARIFAVEL='" + grupo.Tarifavel + "',"
        strSQL = strSQL + "ID_GRUPO_PARENT='" + grupo.Responsavel.ToString + "'"

        strSQL = strSQL + " where CODIGO = '" + grupo.Codigo.ToString + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        InsertGruposLog(grupo, "B", user)

        Return True

    End Function

    Public Function ExcluiGrupo(ByVal pcodigo As String, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        InsertGruposLog(GetGruposById(pcodigo).Item(0), "D", user)

        Try
            Dim strSQL As String = "delete GRUPOS "
            strSQL = strSQL + "where CODIGO = '" + pcodigo + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function InsertGruposLog(ByVal _registro As AppGrupo, ByVal insert As Char, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into GRUPOS_LOG(CODIGO_LOG, CODIGO_GRUPO, TIPO_LOG, DATA_LOG"
        strSQL = strSQL + ", USUARIO "
        strSQL = strSQL + ", NOME_GRUPO, TARIFAVEL, ATIVO, EMP_CODIGO, TP_GRPO_CODIGO, ID_GRUPO_PARENT) "

        strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from GRUPOS_LOG)"
        'Tipo_log
        strSQL = strSQL + ",'" + _registro.Codigo.ToString() + "'"
        strSQL = strSQL + ",'" + insert + "'"
        strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
        strSQL = strSQL + ",'" + user + "'"
        strSQL = strSQL + ",'" + _registro.Grupo + "'"
        strSQL = strSQL + ",'" + _registro.Tarifavel + "'"
        strSQL = strSQL + ",'" + _registro.Ativo + "'"
        strSQL = strSQL + ",'2'"
        strSQL = strSQL + ",'1'"
        strSQL = strSQL + ",'" + _registro.Responsavel + "'"
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        Return True
    End Function


End Class
