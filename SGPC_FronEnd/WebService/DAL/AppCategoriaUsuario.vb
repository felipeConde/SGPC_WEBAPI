Imports Microsoft.VisualBasic

Public Class AppCategoriaUsuario
    
    Private _codigo As Integer
    Private _tipo_usuario As String
    Private _codigo_usuario As String
    Private _codigo_grupo As String
    Private _periodicidade As String
    Private _dia_email As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal ptipo_usuario As String, ByVal pcodigo_grupo As String, ByVal pcodigo_usuario As String, ByVal pperiodicidade As String, ByVal pdia_email As String)
        _codigo = pcodigo
        _tipo_usuario = ptipo_usuario
        _codigo_usuario = pcodigo_usuario
        _codigo_grupo = pcodigo_grupo
        _periodicidade = pperiodicidade
        _dia_email = pdia_email

    End Sub

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Tipo_usuario() As String
        Get
            Return _tipo_usuario
        End Get
        Set(ByVal value As String)
            _tipo_usuario = value
        End Set
    End Property

    Public Property Codigo_usuario() As String
        Get
            Return _codigo_usuario
        End Get
        Set(ByVal value As String)
            _codigo_usuario = value
        End Set
    End Property

    Public Property Codigo_Grupo() As String
        Get
            Return _codigo_grupo
        End Get
        Set(ByVal value As String)
            _codigo_grupo = value
        End Set
    End Property

    Public Property Peridiocidade() As String
        Get
            Return _periodicidade
        End Get
        Set(ByVal value As String)
            _periodicidade = value
        End Set
    End Property

    Public Property Dia_email() As String
        Get
            Return _dia_email
        End Get
        Set(ByVal value As String)
            _dia_email = value
        End Set
    End Property

End Class

