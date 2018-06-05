Imports System.Configuration
Public Class DAOUsuario

    Private _DAO_commons As New DAO_COMMONS
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property


    Public Function Login(username As String, password As String) As appUsuario

        Dim sql As String = "select t.codigo, t.login_usuario, t.email_usuario, t.nome_usuario from USUARIOS t"
        sql += " where upper(t.login_usuario)='" & username.ToUpper & "' and senha_web = password.encrypt(upper('" & UCase(password.ToUpper) & "')) "
        Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        If dt.Rows.Count > 0 Then

            Dim usuario As New appUsuario
            usuario.username = dt.Rows(0).Item("login_usuario")
            usuario.Email = dt.Rows(0).Item("email_usuario")
            usuario.codigo = dt.Rows(0).Item("codigo")
            usuario.nome_usuario = dt.Rows(0).Item("nome_usuario")
            Return usuario
        Else
            Return Nothing
        End If

    End Function

End Class
