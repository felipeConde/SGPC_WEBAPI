Imports Microsoft.VisualBasic

Public Class AppFaturasStatus

    Private _codigo_status As Integer
    Private _status_desc As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo_status As Integer, ByVal pStatus_Desc As String)
        _codigo_status = pCodigo_status
        _status_desc = pStatus_Desc
    End Sub

    Public Property Codigo_Status As Integer
        Get
            Return _codigo_status
        End Get
        Set(ByVal value As Integer)
            _codigo_status = value
        End Set
    End Property

    Public Property Status_Desc As String
        Get
            Return _status_desc
        End Get
        Set(ByVal value As String)
            _status_desc = value
        End Set
    End Property



End Class
