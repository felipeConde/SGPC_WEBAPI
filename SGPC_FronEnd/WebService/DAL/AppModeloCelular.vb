Imports Microsoft.VisualBasic

Public Class AppModeloCelular

    Private _cod_modelo As String
    Private _modelo As String
    Private _cod_tipo As String
    Private _cod_marca As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pcod_modelo As String, ByVal pmodelo As String)
        _cod_modelo = pcod_modelo
        _modelo = pmodelo
    End Sub

    Public Sub New(ByVal pcod_modelo As String, ByVal pmodelo As String, ByVal pcod_tipo As String, ByVal pcod_marca As String)
        _cod_modelo = pcod_modelo
        _modelo = pmodelo
        _cod_tipo = pcod_tipo
        _cod_marca = pcod_marca
    End Sub

    Public Property cod_modelo As String
        Get
            Return _cod_modelo
        End Get
        Set(ByVal value As String)
            _cod_modelo = value
        End Set
    End Property

    Public Property modelo As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property

    Public Property cod_tipo As String
        Get
            Return _cod_tipo
        End Get
        Set(ByVal value As String)
            _cod_tipo = value
        End Set
    End Property

    Public Property cod_marca As String
        Get
            Return _cod_marca
        End Get
        Set(ByVal value As String)
            _cod_marca = value
        End Set
    End Property

End Class
