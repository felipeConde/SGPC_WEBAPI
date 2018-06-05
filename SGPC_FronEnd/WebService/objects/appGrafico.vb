Public Class appGrafico

    Private _categorias As String

    Public Property Categorias As String
        Get
            Return _categorias
        End Get
        Set(value As String)
            _categorias = value
        End Set
    End Property

    Public Property Dados As String
        Get
            Return _dados
        End Get
        Set(value As String)
            _dados = value
        End Set
    End Property

    Private _dados As String

End Class
