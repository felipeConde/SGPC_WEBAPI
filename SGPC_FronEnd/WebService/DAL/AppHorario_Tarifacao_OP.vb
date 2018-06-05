Imports Microsoft.VisualBasic

Public Class AppHorario_Tarifacao_OP
    Private _codigo_operadora As Integer
    Private _codigo_tipo_ligacao As Integer
    Private _codigo_horario As Integer
    Private _ativo As Char
    Private _descricao As String
    Private _codigo_ddi As Integer

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo_operadora As Integer, ByVal pcodigo_tipo_ligacao As Integer, ByVal pcodigo_horario As Integer, ByVal pativo As Char, ByVal pdescricao As String, ByVal pcodigo_ddi As Integer)
        _codigo_operadora = pcodigo_operadora
        _codigo_tipo_ligacao = pcodigo_tipo_ligacao
        _codigo_horario = pcodigo_horario
        _descricao = pdescricao
        _ativo = pativo
        _codigo_ddi = pcodigo_ddi
    End Sub

    Public Property Codigo_operadora As Integer
        Get
            Return _codigo_operadora
        End Get
        Set(ByVal value As Integer)
            _codigo_operadora = value
        End Set
    End Property

    Public Property Codigo_tipo_ligacao As Integer
        Get
            Return _codigo_tipo_ligacao
        End Get
        Set(ByVal value As Integer)
            _codigo_tipo_ligacao = value
        End Set
    End Property

    Public Property Codigo_horario As Integer
        Get
            Return _codigo_horario
        End Get
        Set(ByVal value As Integer)
            _codigo_horario = value
        End Set
    End Property

    Public Property Ativo As Char
        Get
            Return _ativo
        End Get
        Set(ByVal value As Char)
            _ativo = value
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

    Public Property Codigo_DDI As Integer
        Get
            Return _codigo_ddi
        End Get
        Set(ByVal value As Integer)
            _codigo_ddi = value
        End Set
    End Property

End Class
