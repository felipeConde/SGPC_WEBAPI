Imports Microsoft.VisualBasic

Public Class AppAviso


    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As Integer, ByVal pfatura As String, ByVal pVencimento As String, ByVal pDataAviso As String, ByVal pProtocolo As String, ByVal pJustificativa As String, ByVal pAutor As String, ByVal pData As String, ByVal pCodigoFaturaControle As String)

        _codigo = pCodigo
        _fatura = pfatura
        _dataAviso = pDataAviso
        _protocolo = pProtocolo
        _justificativa = pJustificativa
        _autor = pAutor
        _data = pData
        _codigofaturaControle = pCodigoFaturaControle
        _vencimento = pVencimento

    End Sub


    Private _codigo As Integer
    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Private _fatura As String
    Public Property Fatura As String
        Get
            Return _fatura
        End Get
        Set(ByVal value As String)
            _fatura = value
        End Set
    End Property

    Private _vencimento As String
    Public Property Vencimento As String
        Get
            Return _vencimento
        End Get
        Set(ByVal value As String)
            _vencimento = value
        End Set
    End Property

    Private _dataAviso As String
    Public Property DataAviso As String
        Get
            Return _dataAviso
        End Get
        Set(ByVal value As String)
            _dataAviso = value
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

    Private _justificativa As String
    Public Property Justificativa As String
        Get
            Return _justificativa
        End Get
        Set(ByVal value As String)
            _justificativa = value
        End Set
    End Property

    Private _autor As String
    Public Property Autor As String
        Get
            Return _autor
        End Get
        Set(ByVal value As String)
            _autor = value
        End Set
    End Property

    Private _data As String
    Public Property Data As String
        Get
            Return _data
        End Get
        Set(ByVal value As String)
            _data = value
        End Set
    End Property

    Private _codigofaturaControle As Integer
    Public Property CodigoFaturaControle As Integer
        Get
            Return _codigofaturaControle
        End Get
        Set(ByVal value As Integer)
            _codigofaturaControle = value
        End Set
    End Property


End Class
