Imports Microsoft.VisualBasic

Public Class AppLinhasTipos

    Private _codigo As Integer
    Private _tipo As String



    'Construtor
    Public Sub New(ByVal pCodigo As Integer, ByVal pTipo As String)
        _codigo = pCodigo
        _tipo = pTipo
    End Sub


    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
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

End Class
