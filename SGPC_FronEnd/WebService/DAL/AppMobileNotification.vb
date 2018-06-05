Imports Microsoft.VisualBasic

Public Class AppMobileNotification

    Private _codigo As Integer
    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(value As Integer)
            _codigo = value
        End Set
    End Property

    Private _codigoLinha As Integer
    Public Property CodigoLinha As Integer
        Get
            Return _codigoLinha
        End Get
        Set(value As Integer)
            _codigoLinha = value
        End Set
    End Property


    Private _codigoUsuario As Integer
    Public Property CodigoUsuario As Integer
        Get
            Return _codigoUsuario
        End Get
        Set(value As Integer)
            _codigoUsuario = value
        End Set
    End Property

    Private _usercustomdata As String
    Public Property Usercustomdata As String
        Get
            Return _usercustomdata
        End Get
        Set(value As String)
            _usercustomdata = value
        End Set
    End Property

    Private _mensagem As String
    Public Property Mensagem As String
        Get
            Return _mensagem
        End Get
        Set(value As String)
            _mensagem = value
        End Set
    End Property

    Private _autor As String
    Public Property Autor As String
        Get
            Return _autor
        End Get
        Set(value As String)
            _autor = value
        End Set
    End Property

    Private _autorCODIGO As Integer
    Public Property AutorCODIGO As Integer
        Get
            Return _autorCODIGO
        End Get
        Set(value As Integer)
            _autorCODIGO = value
        End Set
    End Property

    Private _dataCadastro As Date
    Public Property DataCadastro As Date
        Get
            Return _dataCadastro
        End Get
        Set(value As Date)
            _dataCadastro = value
        End Set
    End Property

    Private _enviada As String
    Public Property Enviada As String
        Get
            Return _enviada
        End Get
        Set(value As String)
            _enviada = value
        End Set
    End Property

    Private _dataEnvio As Date
    Public Property DataEnvio As Date
        Get
            Return _dataEnvio
        End Get
        Set(value As Date)
            _dataEnvio = value
        End Set
    End Property

    Private _obs As Date
    Public Property Obs As Date
        Get
            Return _obs
        End Get
        Set(value As Date)
            _obs = value
        End Set
    End Property



End Class
