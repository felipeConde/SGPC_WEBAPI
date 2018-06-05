Imports Microsoft.VisualBasic

Public Class AppMenu
    
    Private _login_user As String
    Private _code_user As String
    Private _passw_user As String
    Private _name_user As String
    Private _lcid As String
    Private _DescIdioma As String
    Private _IdIdioma As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal p_code_user As String)
        _code_user = p_code_user
    End Sub

    Public Sub New(ByVal p_login_user As String, ByVal p_code_user As String)
        _login_user = p_login_user
        _code_user = p_code_user
    End Sub

    Public Property login_user() As String
        Get
            Return _login_user
        End Get
        Set(ByVal value As String)
            _login_user = value
        End Set
    End Property

    Public Property code_user() As String
        Get
            Return _code_user
        End Get
        Set(ByVal value As String)
            _code_user = value
        End Set
    End Property

    Public Property passw_user() As String
        Get
            Return _passw_user
        End Get
        Set(ByVal value As String)
            _passw_user = value
        End Set
    End Property

    Public Property name_user() As String
        Get
            Return _name_user
        End Get
        Set(ByVal value As String)
            _name_user = value
        End Set
    End Property

    Public Property lcid() As String
        Get
            Return _lcid
        End Get
        Set(ByVal value As String)
            _lcid = value
        End Set
    End Property

    Public Property DescIdioma() As String
        Get
            Return _DescIdioma
        End Get
        Set(ByVal value As String)
            _DescIdioma = value
        End Set
    End Property

    Public Property IdIdioma() As String
        Get
            Return _IdIdioma
        End Get
        Set(ByVal value As String)
            _IdIdioma = value
        End Set
    End Property

End Class

