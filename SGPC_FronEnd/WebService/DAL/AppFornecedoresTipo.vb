Imports Microsoft.VisualBasic

Public Class AppFornecedoresTipo

    Private _codigo As Integer
    Private _tipo_fornecedor As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal ptipo_fornecedor As String)

        _codigo = pcodigo
        _tipo_fornecedor = ptipo_fornecedor

    End Sub

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Tipo_Fornecedor As String
        Get
            Return _tipo_fornecedor
        End Get
        Set(ByVal value As String)
            _tipo_fornecedor = value
        End Set
    End Property
End Class
