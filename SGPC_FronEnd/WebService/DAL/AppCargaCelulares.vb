Imports Microsoft.VisualBasic

Public Class AppCargaCelulares

#Region "Construtor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As String, ByVal pNomeGrupo As String, ByVal pEmpCodigo As Integer, ByVal pTpCrpoCodigo As Integer)
        _codigo = pCodigo
        _nomeGrupo = pNomeGrupo
        _empCodigo = pEmpCodigo
        _tpGrpoCodigo = pTpCrpoCodigo
    End Sub

#End Region

#Region "Propriedades"

    Private _codigo As String
    Private _nomeGrupo As String
    Private _empCodigo As Integer
    Private _tpGrpoCodigo As Integer

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

    Public Property EmpCodigo() As Integer
        Get
            Return _empCodigo
        End Get
        Set(ByVal value As Integer)
            _empCodigo = value
        End Set
    End Property

    Public Property TpGrpoCodigo() As Integer
        Get
            Return _tpGrpoCodigo
        End Get
        Set(ByVal value As Integer)
            _tpGrpoCodigo = value
        End Set
    End Property

   


#End Region

End Class
