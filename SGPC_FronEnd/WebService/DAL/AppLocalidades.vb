Imports Microsoft.VisualBasic

Public Class AppLocalidades

    Private _codigo As Integer
    Private _localidade As String
    Private _telefone As String
    Private _fax As String
    Private _canal_voz As String
    Private _endereco As String
    Private _complemento As String
    Private _numero As String
    Private _bairro As String
    Private _cidade As String
    Private _estado As String
    Private _cep As String
    Private _ins_estadual As String
    Private _cnpj As String
    Private _responsavel As String
    Private _data_abe As String
    Private _data_enc As String
    Private _parceiro As String
    Private _ins_munic As String
    Private _codigo_cidade As Integer

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal plocalidade As String)
        _codigo = pcodigo
        _localidade = plocalidade
    End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal plocalidade As String, ByVal ptelefone As String, ByVal pfax As String, ByVal pcanal_voz As String, ByVal pendereco As String, ByVal pcomplemento As String, ByVal pnumero As String, ByVal pbairro As String, ByVal pcidade As String, ByVal pestado As String, ByVal pcep As String, ByVal pins_estadual As String, ByVal pcnpj As String, ByVal presponsavel As String, ByVal pdata_abe As String, ByVal pdata_enc As String, ByVal pparceiro As String, ByVal pins_munic As String, ByVal pcodigo_cidade As Integer)
        _codigo = pcodigo
        _localidade = plocalidade
        _telefone = ptelefone
        _fax = pfax
        _canal_voz = pcanal_voz
        _endereco = pendereco
        _complemento = pcomplemento
        _numero = pnumero
        _bairro = pbairro
        _cidade = pcidade
        _estado = pestado
        _cep = pestado
        _ins_estadual = pins_estadual
        _cnpj = pcnpj
        _responsavel = presponsavel
        _data_abe = pdata_abe
        _data_enc = pdata_enc
        _parceiro = pparceiro
        _ins_munic = pins_munic
        _codigo_cidade = pcodigo_cidade
    End Sub

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Localidade() As String
        Get
            Return _localidade
        End Get
        Set(ByVal value As String)
            _localidade = value
        End Set
    End Property

    Public Property Telefone() As String
        Get
            Return _telefone
        End Get
        Set(ByVal value As String)
            _telefone = value
        End Set
    End Property

    Public Property Fax() As String
        Get
            Return _fax
        End Get
        Set(ByVal value As String)
            _fax = value
        End Set
    End Property

    Public Property Canal_Voz() As String
        Get
            Return _canal_voz
        End Get
        Set(ByVal value As String)
            _canal_voz = value
        End Set
    End Property

    Public Property Endereco() As String
        Get
            Return _endereco
        End Get
        Set(ByVal value As String)
            _endereco = value
        End Set
    End Property

    Public Property Complemento() As String
        Get
            Return _complemento
        End Get
        Set(ByVal value As String)
            _complemento = value
        End Set
    End Property

    Public Property Numero() As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
        End Set
    End Property

    Public Property Bairro() As String
        Get
            Return _bairro
        End Get
        Set(ByVal value As String)
            _bairro = value
        End Set
    End Property

    Public Property Cidade() As String
        Get
            Return _cidade
        End Get
        Set(ByVal value As String)
            _cidade = value
        End Set
    End Property

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property CEP() As String
        Get
            Return _cep
        End Get
        Set(ByVal value As String)
            _cep = value
        End Set
    End Property

    Public Property Ins_Estadual() As String
        Get
            Return _ins_estadual
        End Get
        Set(ByVal value As String)
            _ins_estadual = value
        End Set
    End Property

    Public Property CNPJ() As String
        Get
            Return _cnpj
        End Get
        Set(ByVal value As String)
            _cnpj = value
        End Set
    End Property

    Public Property Responsavel() As String
        Get
            Return _responsavel
        End Get
        Set(ByVal value As String)
            _responsavel = value
        End Set
    End Property

    Public Property Data_Abe() As String
        Get
            Return _data_abe
        End Get
        Set(ByVal value As String)
            _data_abe = value
        End Set
    End Property

    Public Property Data_Enc() As String
        Get
            Return _data_enc
        End Get
        Set(ByVal value As String)
            _data_enc = value
        End Set
    End Property

    Public Property Parceiro() As String
        Get
            Return _parceiro
        End Get
        Set(ByVal value As String)
            _parceiro = value
        End Set
    End Property

    Public Property Ins_Municipal() As String
        Get
            Return _ins_munic
        End Get
        Set(ByVal value As String)
            _ins_munic = value
        End Set
    End Property

    Public Property Codgo_Cidade() As Integer
        Get
            Return _codigo_cidade
        End Get
        Set(ByVal value As Integer)
            _codigo_cidade = value
        End Set
    End Property

End Class
