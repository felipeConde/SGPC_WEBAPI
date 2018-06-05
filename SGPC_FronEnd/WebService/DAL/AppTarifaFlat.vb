Imports Microsoft.VisualBasic

Public Class AppTarifaFlat
    Private _codigo As Integer
    Private _descricao As String
    Private _TTM As String
    Private _TTM_value As String
    Private _STEP As String
    Private _STEP_value As String
    Private _Operadora As String
    Private _codigo_tarifa As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pdescricao As String, ByVal pTTM As String, ByVal pTTM_value As String, ByVal pSTEP As String, ByVal pSTEP_value As String, ByVal pOperadora As String, ByVal pcodigo_tarifa As String)
        _codigo = pcodigo
        _descricao = pdescricao
        _TTM = pTTM
        _TTM_value = pTTM_value
        _STEP = pSTEP
        _STEP_value = pSTEP_value
        _Operadora = pOperadora
        _codigo_tarifa = pcodigo_tarifa
    End Sub

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Public Property TTM As String
        Get
            Return _TTM
        End Get
        Set(ByVal value As String)
            _TTM = value
        End Set
    End Property

    Public Property TTM_Value As String
        Get
            Return _TTM_value
        End Get
        Set(ByVal value As String)
            _TTM_value = value
        End Set
    End Property

    Public Property Step_ As String
        Get
            Return _STEP
        End Get
        Set(ByVal value As String)
            _STEP = value
        End Set
    End Property

    Public Property Step_value As String
        Get
            Return _STEP_value
        End Get
        Set(ByVal value As String)
            _STEP_value = value
        End Set
    End Property

    Public Property Operadora As String
        Get
            Return _Operadora
        End Get
        Set(ByVal value As String)
            _Operadora = value
        End Set
    End Property

    Public Property Codigo_tarifa As String
        Get
            Return _codigo_tarifa
        End Get
        Set(ByVal value As String)
            _codigo_tarifa = value
        End Set
    End Property

End Class
