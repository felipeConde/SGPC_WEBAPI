Imports Microsoft.VisualBasic

Public Class AppRelatorio
    Private _codigo As Integer
    Private _idioma As String
    Private _nome As String
    Private _url As String
    Private _ordem As String
    Private _id_parent As String
    Private _id_menu As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo As Integer)
        _codigo = pcodigo
    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pnome As String)
        _codigo = pcodigo
        _nome = pnome
    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pidioma As String, ByVal pnome As String, ByVal purl As String, ByVal pordem As String, ByVal pid_parent As String, ByVal pid_menu As String)
        _codigo = pcodigo
        _idioma = pidioma
        _nome = pnome
        _url = purl
        _ordem = pordem
        _id_parent = pid_parent
        _id_menu = pid_menu
    End Sub

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Id_menu() As String
        Get
            Return _id_menu
        End Get
        Set(ByVal value As String)
            _id_menu = value
        End Set
    End Property

    Public Property Id_parent() As String
        Get
            Return _id_parent
        End Get
        Set(ByVal value As String)
            _id_parent = value
        End Set
    End Property

    Public Property Ordem() As String
        Get
            Return _ordem
        End Get
        Set(ByVal value As String)
            _ordem = value
        End Set
    End Property


    Public Property Url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property Idioma() As String
        Get
            Return _idioma
        End Get
        Set(ByVal value As String)
            _idioma = value
        End Set
    End Property

End Class
