Imports Microsoft.VisualBasic

Public Class AppLoteLaudo

    Public Sub New()

    End Sub


    Private _num_lote As String
    Public Property Num_Lote As String
        Get
            Return _num_lote
        End Get
        Set(ByVal value As String)
            _num_lote = value
        End Set
    End Property

    Private _protocolo As String
    Public Property Protocolo As String
        Get
            Return _protocolo
        End Get
        Set(ByVal value As String)
            _protocolo = value
        End Set
    End Property

    Private _NOVO_VENCIMENTO As String
    Public Property NOVO_VENCIMENTO As String
        Get
            Return _NOVO_VENCIMENTO
        End Get
        Set(ByVal value As String)
            _NOVO_VENCIMENTO = value
        End Set
    End Property

    Private _CODIGO_JUSTIFICATIVA As Integer
    Public Property CODIGO_JUSTIFICATIVA As Integer
        Get
            Return _CODIGO_JUSTIFICATIVA
        End Get
        Set(ByVal value As Integer)
            _CODIGO_JUSTIFICATIVA = value
        End Set
    End Property

    Private _RESULTADO_DETALHADO As String
    Public Property RESULTADO_DETALHADO As String
        Get
            Return _RESULTADO_DETALHADO
        End Get
        Set(ByVal value As String)
            _RESULTADO_DETALHADO = value
        End Set
    End Property

    Private _OBS As String
    Public Property OBS As String
        Get
            Return _OBS
        End Get
        Set(ByVal value As String)
            _OBS = value
        End Set
    End Property

End Class
