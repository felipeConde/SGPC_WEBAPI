Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_AppMobile

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

    Public Function InsertNotification(ByVal registro As AppMobileNotification, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into mobile_notification(codigo"
            strSQL = strSQL + ",  codigo_linha,codigo_usuario, usercustomdata, mensagem, autor, autor_CODIGO, data_cadastro,enviada)"
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from mobile_notification)"
            strSQL = strSQL + ",'" + registro.CodigoLinha.ToString + "'"
            strSQL = strSQL + ",'" + registro.CodigoUsuario.ToString + "'"
            strSQL = strSQL + ",'" + registro.Usercustomdata.ToString + "'"
            strSQL = strSQL + ",'" + registro.Mensagem.ToString + "'"
            strSQL = strSQL + ",'" + usuario + "'"
            strSQL = strSQL + ",'" + registro.AutorCODIGO.ToString + "'"
            strSQL = strSQL + ",sysdate"
            strSQL = strSQL + ",'N'"
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

        Return True

    End Function

    Public Function GetFiltroLinhas(ByVal pSemCom As Boolean, ByVal pCodigoOperadora As Integer, ByVal Classificacao As String, ByVal Codigo_cliente As String, ByVal numero As String, Optional cod_ccusto As String = "", Optional cod_perfil As String = "") As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinhas As New List(Of AppLinhas)

        Dim strSQL As String = "select "
        strSQL = strSQL + "nvl(l.CODIGO_LINHA, 0) as CODIGO_LINHA, nvl(l.NUM_LINHA, '') as NUM_LINHA"
        strSQL = strSQL + ", nvl(m.CODIGO_APARELHO, 0) as CODIGO_APARELHO"
        strSQL = strSQL + ", nvl(o.CODIGO,0) as CODIGO_OPERADORA, nvl(o.DESCRICAO,'') as OPERADORA"
        strSQL = strSQL + ", nvl(u.CODIGO,0) as CODIGO_USUARIO, nvl(u.NOME_USUARIO, '') as NOME_USUARIO"
        strSQL = strSQL + " from LINHAS l, LINHAS_MOVEIS m, USUARIOS u, OPERADORAS_TESTE o "
        If cod_ccusto <> "" Then
            strSQL = strSQL + ", GRUPOS_ITEM gi "
        End If
        If cod_perfil <> "" And cod_perfil <> "0" Then
            strSQL = strSQL + ", PERFIL_LINHA pl "
        End If

        If (pCodigoOperadora < 0) Then
            strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA "
        Else
            'strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and " + Convert.ToString(pCodigoOperadora) + " = m.CODIGO_OPERADORA "
            strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and  l.CODIGO_FORNECEDOR IN(SELECT codigo from FORNECEDORES where CODIGO_OPERADORA= " + Convert.ToString(pCodigoOperadora) + ") "
        End If
        If (Classificacao <> "0" And Classificacao <> "") Then
            'strSQL = strSQL + "where l.CODIGO_LINHA = m.CODIGO_LINHA and " + Convert.ToString(pCodigoOperadora) + " = m.CODIGO_OPERADORA "
            strSQL = strSQL + " and l.codigo_tipo = '" + Classificacao + "'"
        End If
        If Codigo_cliente <> "" Then
            strSQL = strSQL + " and l.codigo_cliente like '" + Codigo_cliente + "'"
        End If
        If numero <> "" Then
            strSQL = strSQL + " and replace(replace(replace(replace(nvl(l.NUM_LINHA,' '),'(',''),')',''),'-',''), ' ','') like replace(replace(replace(replace(nvl('" + numero + "%',' '),'(',''),')',''),'-',''), ' ','')  "
        End If
        strSQL = strSQL + " and  m.CODIGO_OPERADORA = o.CODIGO(+) "
        strSQL = strSQL + " and m.CODIGO_USUARIO = u.CODIGO(+) "
        strSQL = strSQL + " and (l.NUM_LINHA is not null or trim(l.NUM_LINHA) != '') "
        strSQL = strSQL + " and l.STATUS ='1' "
        If cod_ccusto <> "" Then
            strSQL = strSQL + " and l.CODIGO_LINHA = gi.ITEM and gi.modalidade = '4' "
            strSQL = strSQL + " and gi.grupo = '" & cod_ccusto & "' "
        End If

        If cod_perfil <> "" And cod_perfil <> "0" Then
            strSQL = strSQL + " and l.CODIGO_LINHA = pl.codigo_linha "
            strSQL = strSQL + " and pl.codigo_gestaoperfil = '" & cod_perfil & "' "
        Else
            'If (pSemCom) Then
            '    'linhas sem perfil (TRUE)
            '    strSQL = strSQL + "and l.CODIGO_LINHA not in (select CODIGO_LINHA from PERFIL_LINHA) "
            'Else
            '    'linhas com perfil (FALSE)
            '    strSQL = strSQL + "and l.CODIGO_LINHA in (select CODIGO_LINHA from PERFIL_LINHA) "
            'End If
        End If


        strSQL = strSQL + " and l.NUM_LINHA <> '()-'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%0000-00000'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%000)%'"
        strSQL = strSQL + " and l.NUM_LINHA not like '%(00%'"

        strSQL = strSQL + "order by l.NUM_LINHA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA"), reader.Item("NUM_LINHA").ToString, reader.Item("CODIGO_APARELHO"), reader.Item("CODIGO_OPERADORA"), reader.Item("OPERADORA").ToString, reader.Item("CODIGO_USUARIO"), reader.Item("NOME_USUARIO").ToString)
                listLinhas.Add(_registro)
            End While
        End Using

        Return listLinhas
    End Function

End Class
