Imports Microsoft.VisualBasic

Public Class AppTarifaRetro
    Private _codigo As Integer
    Private _TTM_value As String
    Private _STEP As String
    Private _STEP_value As String
    Private _codigo_tipo_ligacao As String
    Private _data_ini As String
    Private _data_fim As String
    Private _sem_imposto As String

    Public Sub New()

    End Sub

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
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


    Public Property Codigo_tipo_ligacao As String
        Get
            Return _codigo_tipo_ligacao
        End Get
        Set(ByVal value As String)
            _codigo_tipo_ligacao = value
        End Set
    End Property

    Public Property Data_ini As String
        Get
            Return _data_ini
        End Get
        Set(ByVal value As String)
            _data_ini = value
        End Set
    End Property

    Public Property Data_fim As String
        Get
            Return _data_fim
        End Get
        Set(ByVal value As String)
            _data_fim = value
        End Set
    End Property

    Public Property Sem_imposto As String
        Get
            Return _sem_imposto
        End Get
        Set(ByVal value As String)
            _sem_imposto = value
        End Set
    End Property



End Class
