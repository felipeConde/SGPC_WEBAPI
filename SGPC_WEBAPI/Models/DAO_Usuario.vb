Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Usuario

    Dim _commons As New DAO_Commons
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

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


    Public Function Login(pLogin As String, senha As String) As UsuarioModels

        Dim sql As String = ""
        sql += " select p1.codigo, p1.nome_usuario "
        sql += " from usuarios p1 "
        sql += " select p1.codigo, p1.nome_usuario "
        sql += " where upper(p1.login_usuario)='" & pLogin.ToUpper & "' and upper(password.decrypt(p1.senha_web))='" & senha.ToUpper & "' "

        Dim dt As DataTable = _commons.myDataTable(sql)
        If dt.Rows.Count > 0 Then

            Dim _usuario As New UsuarioModels()
            _usuario.CodigoUsuario = dt.Rows(0).Item("codigo")
            _usuario.NomeUsuario = dt.Rows(0).Item("nome_usuario")

            Return _usuario

        Else
            Return Nothing
        End If

    End Function









End Class
