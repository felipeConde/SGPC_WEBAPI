Imports Microsoft.VisualBasic

Public Class AppTarifasDerivadas
    
    Private _codigo As String
    Private _descricao As String
    Private _horario As String
    Private _fator As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String)
        _codigo = pcodigo
        _descricao = pdescricao
    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal pdescricao As String, ByVal phorario As String, ByVal pfator As String)
        _codigo = pcodigo
        _descricao = pdescricao
        _horario = phorario
        _fator = pfator
    End Sub

    Public Property codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Public Property horario As String
        Get
            Return _horario
        End Get
        Set(ByVal value As String)
            _horario = value
        End Set
    End Property

    Public Property fator As String
        Get
            Return _fator
        End Get
        Set(ByVal value As String)
            _fator = value
        End Set
    End Property

End Class

