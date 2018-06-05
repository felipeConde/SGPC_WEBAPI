Imports Microsoft.VisualBasic

Public Class AppFacilidades

    Public Sub New()
    End Sub

    Private _codigo As String

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Private _nome As String

    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Private _operadora As String

    Public Property Operadora() As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

    Private _funcionalidade As String

    Public Property Funcionalidade() As String
        Get
            Return _funcionalidade
        End Get
        Set(ByVal value As String)
            _funcionalidade = value
        End Set
    End Property

    Private _valor As String

    Public Property Valor() As String
        Get
            Return _valor
        End Get
        Set(ByVal value As String)
            _valor = value
        End Set
    End Property

    Private _franquia As String

    Public Property Franquia() As String
        Get
            Return _franquia
        End Get
        Set(ByVal value As String)
            _franquia = value
        End Set
    End Property

    Private _minutos As String

    Public Property Minutos() As String
        Get
            Return _minutos
        End Get
        Set(ByVal value As String)
            _minutos = value
        End Set
    End Property

    Private _tarifa As String

    Public Property Tarifa() As String
        Get
            Return _tarifa
        End Get
        Set(ByVal value As String)
            _tarifa = value
        End Set
    End Property

    Private _medida As String

    Public Property Medida() As String
        Get
            Return _medida
        End Get
        Set(ByVal value As String)
            _medida = value
        End Set
    End Property

    Private _fator As String

    Public Property Fator() As String
        Get
            Return _fator
        End Get
        Set(ByVal value As String)
            _fator = value
        End Set
    End Property

    Private _dt_ativ As String

    Public Property Dt_ativ() As String
        Get
            Return _dt_ativ
        End Get
        Set(ByVal value As String)
            _dt_ativ = value
        End Set
    End Property

    Private _dt_des As String

    Public Property Dt_des() As String
        Get
            Return _dt_des
        End Get
        Set(ByVal value As String)
            _dt_des = value
        End Set
    End Property

    Private _compartilhado As String

    Public Property Compartilhado() As String
        Get
            Return _compartilhado
        End Get
        Set(ByVal value As String)
            _compartilhado = value
        End Set
    End Property

    Private _franquia_tipo As Integer
    Public Property FranquiaTipo As Integer
        Get
            Return _franquia_tipo
        End Get
        Set(ByVal value As Integer)
            _franquia_tipo = value
        End Set
    End Property

    Private _MaximoMinimo As Integer
    Public Property MaximoMinimo As Integer
        Get
            Return _MaximoMinimo
        End Get
        Set(ByVal value As Integer)
            _MaximoMinimo = value
        End Set
    End Property

    Private _Ocorrencias As String
    Public Property Ocorrencias As String
        Get
            Return _Ocorrencias
        End Get
        Set(ByVal value As String)
            _Ocorrencias = value
        End Set
    End Property

    Private _codigoPlano As String
    Public Property CodigoPlano As String
        Get
            Return _codigoPlano
        End Get
        Set(ByVal value As String)
            _codigoPlano = value
        End Set
    End Property


End Class
