Imports Microsoft.VisualBasic

Public Class AppHorario_Tarifacao
    Private _codigo As Integer
    Private _horario As Integer
    Private _codigo_tarifa As Integer
    Private _tipo_tarifa As Integer

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal phorario As Integer, ByVal pcodigo_tarifa As Integer, ByVal ptipo_tarifa As Integer)
        _codigo = pcodigo
        _horario = phorario
        _tipo_tarifa = ptipo_tarifa
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

    Public Property Horario As Integer
        Get
            Return _horario
        End Get
        Set(ByVal value As Integer)
            _horario = value
        End Set
    End Property

    Public Property Codigo_tarifa As Integer
        Get
            Return _codigo_tarifa
        End Get
        Set(ByVal value As Integer)
            _codigo_tarifa = value
        End Set
    End Property

    Public Property Tipo_tarifa As Integer
        Get
            Return _tipo_tarifa
        End Get
        Set(ByVal value As Integer)
            _tipo_tarifa = value
        End Set
    End Property

End Class
