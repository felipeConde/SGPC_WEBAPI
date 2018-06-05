Imports Microsoft.VisualBasic

Public Class AppFornecedor

    Private _codigo As Integer
    Private _nome_fantasia As String
    Private _contato_comercial As String
    Private _contato_tecnico As String
    Private _email_comercial As String
    Private _email_tecnico As String
    Private _telefone_tecnico As String
    Private _telefone_comercial As String
    Private _razao_social As String
    Private _cnpj As String
    Private _ins_estadual As String
    Private _endereco As String
    Private _complemento As String
    Private _numero As String
    Private _cep As String
    Private _cod_tipo_fornecedor As String
    Private _datai As String
    Private _datar As String
    Private _bairro As String
    Private _banco As String
    Private _agencia As String
    Private _conta As String
    Private _ins_munic As String
    Private _fax As String
    Private _toll_free As String
    Private _codigo_cidade As String
    Private _codigo_operadora As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As Integer, ByVal pNomeFantasia As String)

        _codigo = pCodigo
        _nome_fantasia = pNomeFantasia

    End Sub

    Public Sub New(ByVal pCodigo As Integer, ByVal pNomeFantasia As String, ByVal p_contato_comercial As String, ByVal p_contato_tecnico As String, ByVal p_email_comercial As String, ByVal p_email_tecnico As String, ByVal p_telefone_tecnico As String, ByVal p_telefone_comercial As String, ByVal p_razao_social As String, ByVal p_cnpj As String, ByVal p_ins_estadual As String, ByVal p_endereco As String, ByVal p_complemento As String, ByVal p_numero As String, ByVal p_cep As String, ByVal p_cod_tipo_fornecedor As String, ByVal p_datai As String, ByVal p_datar As String, ByVal p_bairro As String, ByVal p_banco As String, ByVal p_agencia As String, ByVal p_conta As String, ByVal p_ins_munic As String, ByVal p_fax As String, ByVal p_toll_free As String, ByVal p_codigo_cidade As String, ByVal p_codigo_operadora As String)

        _codigo = pCodigo
        _nome_fantasia = pNomeFantasia
        _contato_comercial = p_contato_comercial
        _contato_tecnico = p_contato_tecnico
        _email_comercial = p_email_comercial
        _email_tecnico = p_email_tecnico
        _telefone_tecnico = p_telefone_tecnico
        _telefone_comercial = p_telefone_comercial
        _razao_social = p_razao_social
        _cnpj = p_cnpj
        _ins_estadual = p_ins_estadual
        _endereco = p_endereco
        _complemento = p_complemento
        _numero = p_numero
        _cep = p_cep
        _cod_tipo_fornecedor = p_cod_tipo_fornecedor
        _datai = p_datai
        _datar = p_datar
        _bairro = p_bairro
        _banco = p_banco
        _agencia = p_agencia
        _conta = p_conta
        _ins_munic = p_ins_munic
        _fax = p_fax
        _toll_free = p_toll_free
        _codigo_cidade = p_codigo_cidade
        _codigo_operadora = p_codigo_operadora

    End Sub

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property NomeFantasia As String
        Get
            Return _nome_fantasia
        End Get
        Set(ByVal value As String)
            _nome_fantasia = value
        End Set
    End Property

    Public Property ContatoComercial As String
        Get
            Return _contato_comercial
        End Get
        Set(ByVal value As String)
            _contato_comercial = value
        End Set
    End Property

    Public Property ContatoTecnico As String
        Get
            Return _contato_tecnico
        End Get
        Set(ByVal value As String)
            _contato_tecnico = value
        End Set
    End Property

    Public Property EmailComercial As String
        Get
            Return _email_comercial
        End Get
        Set(ByVal value As String)
            _email_comercial = value
        End Set
    End Property

    Public Property EmailTecnico As String
        Get
            Return _email_tecnico
        End Get
        Set(ByVal value As String)
            _email_tecnico = value
        End Set
    End Property

    Public Property TelefoneTecnico As String
        Get
            Return _telefone_tecnico
        End Get
        Set(ByVal value As String)
            _telefone_tecnico = value
        End Set
    End Property

    Public Property TelefoneComercial As String
        Get
            Return _telefone_comercial
        End Get
        Set(ByVal value As String)
            _telefone_comercial = value
        End Set
    End Property

    Public Property RazaoSocial As String
        Get
            Return _razao_social
        End Get
        Set(ByVal value As String)
            _razao_social = value
        End Set
    End Property

    Public Property CNPJ As String
        Get
            Return _cnpj
        End Get
        Set(ByVal value As String)
            _cnpj = value
        End Set
    End Property

    Public Property InsEstadual As String
        Get
            Return _ins_estadual
        End Get
        Set(ByVal value As String)
            _ins_estadual = value
        End Set
    End Property

    Public Property Endereco As String
        Get
            Return _endereco
        End Get
        Set(ByVal value As String)
            _endereco = value
        End Set
    End Property

    Public Property Complemento As String
        Get
            Return _complemento
        End Get
        Set(ByVal value As String)
            _complemento = value
        End Set
    End Property

    Public Property Numero As String
        Get
            Return _numero
        End Get
        Set(ByVal value As String)
            _numero = value
        End Set
    End Property

    Public Property CEP As String
        Get
            Return _cep
        End Get
        Set(ByVal value As String)
            _cep = value
        End Set
    End Property

    Public Property CodTipoFornecedor As String
        Get
            Return _cod_tipo_fornecedor
        End Get
        Set(ByVal value As String)
            _cod_tipo_fornecedor = value
        End Set
    End Property

    Public Property DataI As String
        Get
            Return _datai
        End Get
        Set(ByVal value As String)
            _datai = value
        End Set
    End Property

    Public Property DataR As String
        Get
            Return _datar
        End Get
        Set(ByVal value As String)
            _datar = value
        End Set
    End Property

    Public Property Bairro As String
        Get
            Return _bairro
        End Get
        Set(ByVal value As String)
            _bairro = value
        End Set
    End Property

    Public Property Banco As String
        Get
            Return _banco
        End Get
        Set(ByVal value As String)
            _banco = value
        End Set
    End Property

    Public Property Agencia As String
        Get
            Return _agencia
        End Get
        Set(ByVal value As String)
            _agencia = value
        End Set
    End Property

    Public Property Conta As String
        Get
            Return _conta
        End Get
        Set(ByVal value As String)
            _conta = value
        End Set
    End Property

    Public Property InsMunic As String
        Get
            Return _ins_munic
        End Get
        Set(ByVal value As String)
            _ins_munic = value
        End Set
    End Property

    Public Property Fax As String
        Get
            Return _fax
        End Get
        Set(ByVal value As String)
            _fax = value
        End Set
    End Property

    Public Property TollFree As String
        Get
            Return _toll_free
        End Get
        Set(ByVal value As String)
            _toll_free = value
        End Set
    End Property

    Public Property CodigoCidade As String
        Get
            Return _codigo_cidade
        End Get
        Set(ByVal value As String)
            _codigo_cidade = value
        End Set
    End Property

    Public Property CodigoOperadora As String
        Get
            Return _codigo_operadora
        End Get
        Set(ByVal value As String)
            _codigo_operadora = value
        End Set
    End Property



End Class
