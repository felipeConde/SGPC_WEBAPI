Imports Microsoft.VisualBasic

Public Class AppFaturaServico

    Private _codigo_servico As Integer
    Private _servico_desc As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo_servico As Integer, ByVal pservico_desc As String)
        _codigo_servico = pCodigo_servico
        _servico_desc = pservico_desc
    End Sub



Public Property Codigo_Servico As Integer
        Get
            Return _codigo_servico
        End Get
        Set(ByVal value As Integer)
            _codigo_servico = value
        End Set
    End Property

    Public Property Servico_Desc As String
        Get
            Return _servico_desc
        End Get
        Set(ByVal value As String)
            _servico_desc = value
        End Set
    End Property



End Class
