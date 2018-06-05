Imports Microsoft.VisualBasic

Public Class AppChamado

    Public Sub New()

    End Sub

    Private _oem As String
    Public Property OEM As String
        Get
            Return _oem
        End Get
        Set(ByVal value As String)
            _oem = value
        End Set
    End Property

    Private _tipo_item As String
    Public Property Tipo_item As String
        Get
            Return _tipo_item
        End Get
        Set(ByVal value As String)
            _tipo_item = value
        End Set
    End Property

    Private _abertura As String
    Public Property Abertura As String
        Get
            Return _abertura
        End Get
        Set(ByVal value As String)
            _abertura = value
        End Set
    End Property

    Private _fechamento As String
    Public Property Fechamento As String
        Get
            Return _fechamento
        End Get
        Set(ByVal value As String)
            _fechamento = value
        End Set
    End Property

    Private _status As String
    Public Property Status As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _prioridade As String
    Public Property Prioridade As String
        Get
            Return _prioridade
        End Get
        Set(ByVal value As String)
            _prioridade = value
        End Set
    End Property

    Private _responsavel As String
    Public Property Responsavel As String
        Get
            Return _responsavel
        End Get
        Set(ByVal value As String)
            _responsavel = value
        End Set
    End Property

    Private _texto As String
    Public Property Texto As String
        Get
            Return _texto
        End Get
        Set(ByVal value As String)
            _texto = value
        End Set
    End Property

    Private _tipo_chamado As String
    Public Property Tipo_chamado As String
        Get
            Return _tipo_chamado
        End Get
        Set(ByVal value As String)
            _tipo_chamado = value
        End Set
    End Property

End Class