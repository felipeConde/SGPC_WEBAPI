Imports Microsoft.VisualBasic

Public Class AppFranquiaCobranca

    Public Sub New()

    End Sub



    Private _codigo_franquia As Integer
    Public Property codigo_franquia As Integer
        Get
            Return _codigo_franquia
        End Get
        Set(value As Integer)
            _codigo_franquia = value
        End Set
    End Property

    Private _servico As String
    Public Property servico As String
        Get
            Return _servico
        End Get
        Set(value As String)
            _servico = value
        End Set
    End Property

    Private _qtd As Integer
    Public Property qtd As Integer
        Get
            Return _qtd
        End Get
        Set(value As Integer)
            _qtd = value
        End Set
    End Property

    Private _valor_faturado As Double
    Public Property valor_faturado As Double
        Get
            Return _valor_faturado
        End Get
        Set(value As Double)
            _valor_faturado = value
        End Set
    End Property

    Private _valor_correto As Double
    Public Property valor_correto As Double
        Get
            Return _valor_correto
        End Get
        Set(value As Double)
            _valor_correto = value
        End Set
    End Property

    Private _valor_contratado As Double
    Public Property valor_contratado As Double
        Get
            Return _valor_contratado
        End Get
        Set(value As Double)
            _valor_contratado = value
        End Set
    End Property


End Class
