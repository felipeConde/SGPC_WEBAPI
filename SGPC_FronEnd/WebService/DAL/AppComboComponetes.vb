Imports Microsoft.VisualBasic

Public Class AppComboComponetes
    Private _codigo As Integer
    Private _descricao As String

    Public Sub New(ByVal pcodigo As Integer, ByVal pdescricao As String)
        _codigo = pcodigo
        _descricao = pdescricao
    End Sub

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Descricao() As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property
End Class
