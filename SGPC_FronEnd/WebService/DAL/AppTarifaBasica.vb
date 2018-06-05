Imports Microsoft.VisualBasic

Public Class AppTarifaBasica
    Private _codigo As Integer
    Private _descricao As String
    Private _TTM As String
    Private _TTM_value As String
    Private _STEP As String
    Private _STEP_value As String
    Private _codigo_operadora As String
    Private _codigo_tipo_ligacao As String
    Private _flag_protecao As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pdescricao As String, ByVal pTTM As String, ByVal pTTM_value As String, ByVal pSTEP As String, ByVal pSTEP_value As String, ByVal pcodigo_operadora As String, ByVal pcodigo_tipo_ligacao As String, ByVal pflag_protecao As String)
        _codigo = pcodigo
        _descricao = pdescricao
        _TTM = pTTM
        _TTM_value = pTTM_value
        _STEP = pSTEP
        _STEP_value = pSTEP_value
        _codigo_operadora = pcodigo_operadora
        _codigo_tipo_ligacao = pcodigo_tipo_ligacao
        _flag_protecao = pflag_protecao
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

    Public Property Codigo_tipo_ligacao As String
        Get
            Return _codigo_tipo_ligacao
        End Get
        Set(ByVal value As String)
            _codigo_tipo_ligacao = value
        End Set
    End Property

    Public Property Codigo_operadora As String
        Get
            Return _codigo_operadora
        End Get
        Set(ByVal value As String)
            _codigo_operadora = value
        End Set
    End Property

    Public Property flag_protecao As String
        Get
            Return _flag_protecao
        End Get
        Set(ByVal value As String)
            _flag_protecao = value
        End Set
    End Property

End Class
