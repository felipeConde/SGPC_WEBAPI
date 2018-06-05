Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAOItens

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

    Public Function InsereItem(ByVal pitem As AppItens) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into SOLICITACOES_ITENS(CODIGO"
            strSQL = strSQL + ",ITEM_SGPC) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from SOLICITACOES_ITENS)"
            strSQL = strSQL + ",'" + pitem.ItemSgpc.ToUpper + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
    End Function

    Public Function AtualizaItem(ByVal pitem As AppItens) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update SOLICITACOES_ITENS set "
            strSQL = strSQL + "ITEM_SGPC='" + pitem.ItemSgpc.ToUpper + "'"
            strSQL = strSQL + " where CODIGO = '" + pitem.Codigo.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try
    End Function

    Public Function ExcluiItem(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete SOLICITACOES_ITENS "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

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

    Public Function GetItemById(ByVal pcodigo As Integer) As List(Of AppItens)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppItens)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ",ITEM_SGPC"
        strSQL = strSQL + " from SOLICITACOES_ITENS "
        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)
        Else
            strSQL = strSQL + "order by ITEM_SGPC"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppItens(reader.Item("CODIGO").ToString, reader.Item("ITEM_SGPC").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetComboItem() As List(Of AppItens)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppItens)

        Dim strSQL As String = "select 0 as CODIGO, '...' as ITEM_SGPC from dual union "
        strSQL = strSQL + "select CODIGO, ITEM_SGPC "
        strSQL = strSQL + " from SOLICITACOES_ITENS "
        strSQL = strSQL + "order by ITEM_SGPC"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppItens(reader.Item("CODIGO").ToString, reader.Item("ITEM_SGPC").ToString)
                listItem.Add(_registro)
            End While
        End Using

        Return listItem
    End Function

    'Public Function GravaLog(ByVal pTipo As String, ByVal pAutor As String, ByVal pfatura As Fatura, ByVal pCodigo As String) As Boolean
    '    Dim connection As New OleDbConnection(strConn)
    '    Try
    '        Dim strSQL As String = "insert into faturas_controle_log select '" & pTipo & "', fatura,codigo_operadora, codigo_tipo,intervalo_mes,data_inicio,debito_automatico,dia_vencimento,codigo_estado,data_fim,febraban,codigo_fatura_controle where codigo_fatura_controle='" & pCodigo & "'"

    '        Dim cmd As OleDbCommand = connection.CreateCommand
    '        cmd.CommandText = strSQL
    '        connection.Open()
    '        cmd.ExecuteNonQuery()
    '        connection.Close()
    '        cmd.Dispose()
    '        Return True
    '    Catch ex As Exception
    '        connection.Close()
    '        Return False
    '    End Try
    'End Function

End Class
