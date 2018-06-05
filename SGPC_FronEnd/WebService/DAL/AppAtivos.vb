Imports Microsoft.VisualBasic

Public Class AppAtivos

    Private _codigo As String
    Private _tipo As String
    Private _client_name As String
    Private _marca As String
    Private _modelo As String
    Private _serial_number As String
    Private _centro_custo As String
    Private _num_nota_fisc As String
    Private _tipo_contrato As String
    Private _numero_contrato As String
    Private _inicio_contrato As String
    Private _termino_contrato As String
    Private _status_dispositivo As String
    Private _data_ultimo_status As String
    Private _obs As String
    Private _codigo_usuario As String
    Private _codigo_fornecedor As String
    Private _custo As String
    Private _garantia As String
    Private _garantia_estendida As String
    Private _patrimonio As String


    Public Sub New()

    End Sub

    Public Property Patrimonio As String
        Get
            Return _patrimonio
        End Get
        Set(ByVal value As String)
            _patrimonio = value
        End Set
    End Property

    Public Property Garantia_estendida As String
        Get
            Return _garantia_estendida
        End Get
        Set(ByVal value As String)
            _garantia_estendida = value
        End Set
    End Property

    Public Property Garantia As String
        Get
            Return _garantia
        End Get
        Set(ByVal value As String)
            _garantia = value
        End Set
    End Property

    Public Property Custo As String
        Get
            Return _custo
        End Get
        Set(ByVal value As String)
            _custo = value
        End Set
    End Property

    Public Property Codigo_fornecedor As String
        Get
            Return _codigo_fornecedor
        End Get
        Set(ByVal value As String)
            _codigo_fornecedor = value
        End Set
    End Property

    Public Property Codigo_usuario As String
        Get
            Return _codigo_usuario
        End Get
        Set(ByVal value As String)
            _codigo_usuario = value
        End Set
    End Property

    Public Property Obs As String
        Get
            Return _obs
        End Get
        Set(ByVal value As String)
            _obs = value
        End Set
    End Property

    Public Property Data_ultimo_status As String
        Get
            Return _data_ultimo_status
        End Get
        Set(ByVal value As String)
            _data_ultimo_status = value
        End Set
    End Property

    Public Property Status_dispositivo As String
        Get
            Return _status_dispositivo
        End Get
        Set(ByVal value As String)
            _status_dispositivo = value
        End Set
    End Property

    Public Property Termino_contrato As String
        Get
            Return _termino_contrato
        End Get
        Set(ByVal value As String)
            _termino_contrato = value
        End Set
    End Property

    Public Property Inicio_Contrato As String
        Get
            Return _inicio_contrato
        End Get
        Set(ByVal value As String)
            _inicio_contrato = value
        End Set
    End Property

    Public Property Numero_Contrato As String
        Get
            Return _numero_contrato
        End Get
        Set(ByVal value As String)
            _numero_contrato = value
        End Set
    End Property

    Public Property Tipo_Contrato As String
        Get
            Return _tipo_contrato
        End Get
        Set(ByVal value As String)
            _tipo_contrato = value
        End Set
    End Property

    Public Property Num_nota_fisc As String
        Get
            Return _num_nota_fisc
        End Get
        Set(ByVal value As String)
            _num_nota_fisc = value
        End Set
    End Property

    Public Property Centro_Custo As String
        Get
            Return _centro_custo
        End Get
        Set(ByVal value As String)
            _centro_custo = value
        End Set
    End Property

    Public Property Serial_Number As String
        Get
            Return _serial_number
        End Get
        Set(ByVal value As String)
            _serial_number = value
        End Set
    End Property

    Public Property Modelo As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property

    Public Property Marca As String
        Get
            Return _marca
        End Get
        Set(ByVal value As String)
            _marca = value
        End Set
    End Property

    Public Property Client_name As String
        Get
            Return _client_name
        End Get
        Set(ByVal value As String)
            _client_name = value
        End Set
    End Property

    Public Property Tipo As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property


End Class
