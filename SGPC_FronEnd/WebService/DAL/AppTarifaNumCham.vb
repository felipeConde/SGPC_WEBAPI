Imports Microsoft.VisualBasic

Public Class AppTarifaNumCham
    Private _numero As String
    Private _TTM As String
    Private _TTM_value As String
    Private _STEP As String
    Private _STEP_value As String
    Private _codigo_tipo_ligacao As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pnumero As String, ByVal pTTM As String, ByVal pTTM_value As String, ByVal pSTEP As String, ByVal pSTEP_value As String, ByVal pcodigo_tipo_ligacao As String)
        _numero = pnumero
        _TTM = pTTM
        _TTM_value = pTTM_value
        _STEP = pSTEP
        _STEP_value = pSTEP_value
        _codigo_tipo_ligacao = pcodigo_tipo_ligacao
    End Sub

    Public Property Numero As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
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

    Public Property Codigo_tipo_ligacao As String
        Get
            Return _codigo_tipo_ligacao
        End Get
        Set(ByVal value As String)
            _codigo_tipo_ligacao = value
        End Set
    End Property


End Class
