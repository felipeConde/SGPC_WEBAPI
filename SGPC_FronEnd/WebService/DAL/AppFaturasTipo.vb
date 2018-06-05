Imports Microsoft.VisualBasic

Public Class AppFaturasTipo

    Private _codigo_tipo As Integer
    Private _tipo As String

    Public Property CodigoTipo As Integer
        Get
            Return _codigo_tipo
        End Get
        Set(ByVal value As Integer)
            _codigo_tipo = value
        End Set
    End Property

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As Integer, ByVal pTipo As String)
        _codigo_tipo = pCodigo
        _tipo = pTipo
    End Sub

End Class
