Imports Microsoft.VisualBasic

Public Class CCusto

#Region "Construtor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pNomeGrupo As String)
        _codigo = pCodigo
        _nomeGrupo = pNomeGrupo

    End Sub

#End Region

#Region "Propriedades"

    Private _codigo As String
    Private _nomeGrupo As String


    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property NomeGrupo() As String
        Get
            Return _nomeGrupo
        End Get
        Set(ByVal value As String)
            _nomeGrupo = value
        End Set
    End Property





#End Region

End Class
