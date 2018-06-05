Imports Microsoft.VisualBasic

Public Class AppLinks

    Private _codigo_link As Integer
    Private _codigo_fornecedor As Integer
    Private _localidade As String
    Private _cod_local As Integer
    Private _tipo_link As String
    Private _desig_cir As String
    Private _desig_pag As String
    Private _codigo_ccusto As String
    Private _ant_porta As String
    Private _ant_cir As String
    Private _atual_porta As String
    Private _atual_cir As String
    Private _codigo_cidade As Integer
    Private _uf As String
    Private _cep As String
    Private _telefone As String
    Private _contato As String
    Private _regiao As String
    Private _dlci_mat As String
    Private _dlci_rem As String
    Private _lmi_rem As String
    Private _data_ati As String
    Private _data_des As String
    Private _status As String
    Private _salva As String
    Private _obs_ As String
    Private _protocolo As String
    Private _codigo_router As Integer
    Private _codigo_lan As Integer
    Private _codigo_wan As Integer
    Private _endereco_b As String
    Private _valor_ativ As String
    Private _valor_link As String
    Private _cod_cliente_conta As String
    Private _num_fatura As String
    Private _num_contrato_tvg As String
    Private _num_contrato_op As String
    Private _num_operadora As String
    Private _numero_oc As String
    Private _data_IPCA As String
    Private _valor_mensal As String
    Private _sponsor As String
    Private _endereco_cob As String
    Private _uf_b As String
    Private _codigo_cidade_B As String
    Private _num_contrato_op_as As String
    Private _contato_b As String
    Private _tel_cont_a As String
    Private _email_cont_a As String
    Private _tel_cont_b As String
    Private _email_cont_b As String
    Private _sucursal As String
    Private _conta_cont As String
    Private _oem As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo_link As Integer, ByVal pcod_local As Integer, ByVal pcodigo_fornecedor As Integer, ByVal plocalidade As String, ByVal ptipo_link As String, ByVal pdesig_cir As String, ByVal pdesig_pag As String, ByVal pcodigo_ccusto As String, ByVal pant_porta As String, ByVal pant_cir As String, ByVal patual_porta As String, ByVal patual_cir As String, ByVal pcodigo_cidade As Integer, ByVal puf As String, ByVal pcep As String, ByVal ptelefone As String, ByVal pcontato As String, ByVal pregiao As String, ByVal pdlci_mat As String, ByVal pdlci_rem As String, ByVal plmi_rem As String, ByVal pdata_ati As String, ByVal pdata_des As String, ByVal pstatus As Integer, ByVal psalva As String, ByVal pobs_ As String, ByVal pcodigo_router As Integer, ByVal pcodigo_lan As Integer, ByVal pcodigo_wan As Integer, ByVal pprotocolo As String, ByVal pendereco_b As String, ByVal pvalor_ativ As String, ByVal pvalor_link As String, ByVal pcod_cliente_conta As String, ByVal puf_b As String, ByVal pcodigo_cidade_B As String, ByVal pnum_contrato_op_as As String, ByVal pcontato_b As String)

        _codigo_link = pcodigo_link
        _codigo_fornecedor = pcodigo_fornecedor
        _localidade = plocalidade
        _cod_local = pcod_local
        _tipo_link = ptipo_link
        _desig_cir = pdesig_cir
        _desig_pag = pdesig_pag
        _codigo_ccusto = pcodigo_ccusto
        _ant_porta = pant_porta
        _ant_cir = pant_cir
        _atual_porta = patual_porta
        _atual_cir = patual_cir
        _codigo_cidade = pcodigo_cidade
        _uf = puf
        _cep = pcep
        _telefone = ptelefone
        _contato = pcontato
        _regiao = pregiao
        _dlci_mat = pdlci_mat
        _dlci_rem = pdlci_rem
        _lmi_rem = plmi_rem
        _data_ati = pdata_ati
        _data_des = pdata_des
        _status = pstatus
        _salva = psalva
        _obs_ = pobs_
        _protocolo = pprotocolo
        _endereco_b = pendereco_b
        _cod_cliente_conta = pcod_cliente_conta

        _valor_ativ = pvalor_ativ
        _valor_link = pvalor_link

        _codigo_router = pcodigo_router
        _codigo_lan = pcodigo_lan
        _codigo_wan = pcodigo_wan

        _uf_b = puf_b
        _codigo_cidade_B = pcodigo_cidade_B
        _num_contrato_op_as = pnum_contrato_op_as
        _contato_b = pcontato_b

    End Sub

    Public Property Codigo_Link() As Integer
        Get
            Return _codigo_link
        End Get
        Set(ByVal value As Integer)
            _codigo_link = value
        End Set
    End Property

    Public Property Cod_local() As Integer
        Get
            Return _cod_local
        End Get
        Set(ByVal value As Integer)
            _cod_local = value
        End Set
    End Property

    Public Property Codigo_Router() As Integer
        Get
            Return _codigo_router
        End Get
        Set(ByVal value As Integer)
            _codigo_router = value
        End Set
    End Property

    Public Property Codigo_Lan() As Integer
        Get
            Return _codigo_lan
        End Get
        Set(ByVal value As Integer)
            _codigo_lan = value
        End Set
    End Property

    Public Property Codigo_Wan() As Integer
        Get
            Return _codigo_wan
        End Get
        Set(ByVal value As Integer)
            _codigo_wan = value
        End Set
    End Property


    Public Property Codigo_Fornecedor() As Integer
        Get
            Return _codigo_fornecedor
        End Get
        Set(ByVal value As Integer)
            _codigo_fornecedor = value
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

    Public Property Protocolo() As String
        Get
            Return _protocolo
        End Get
        Set(ByVal value As String)
            _protocolo = value
        End Set
    End Property

    Public Property Tipo_Link() As String
        Get
            Return _tipo_link
        End Get
        Set(ByVal value As String)
            _tipo_link = value
        End Set
    End Property


    Public Property Desig_Cir() As String
        Get
            Return _desig_cir
        End Get
        Set(ByVal value As String)
            _desig_cir = value
        End Set
    End Property


    Public Property Desig_Pag() As String
        Get
            Return _desig_pag
        End Get
        Set(ByVal value As String)
            _desig_pag = value
        End Set
    End Property


    Public Property Codigo_CCusto() As String
        Get
            Return _codigo_ccusto
        End Get
        Set(ByVal value As String)
            _codigo_ccusto = value
        End Set
    End Property


    Public Property Ant_Porta() As String
        Get
            Return _ant_porta
        End Get
        Set(ByVal value As String)
            _ant_porta = value
        End Set
    End Property


    Public Property Ant_Cir() As String
        Get
            Return _ant_cir
        End Get
        Set(ByVal value As String)
            _ant_cir = value
        End Set
    End Property


    Public Property Atual_Porta() As String
        Get
            Return _atual_porta
        End Get
        Set(ByVal value As String)
            _atual_porta = value
        End Set
    End Property


    Public Property Atual_Cir() As String
        Get
            Return _atual_cir
        End Get
        Set(ByVal value As String)
            _atual_cir = value
        End Set
    End Property


    Public Property Codigo_Cidade() As Integer
        Get
            Return _codigo_cidade
        End Get
        Set(ByVal value As Integer)
            _codigo_cidade = value
        End Set
    End Property


    Public Property Cep() As String
        Get
            Return _cep
        End Get
        Set(ByVal value As String)
            _cep = value
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


    Public Property Contato() As String
        Get
            Return _contato
        End Get
        Set(ByVal value As String)
            _contato = value
        End Set
    End Property


    Public Property Regiao() As String
        Get
            Return _regiao
        End Get
        Set(ByVal value As String)
            _regiao = value
        End Set
    End Property


    Public Property Dlci_Rem() As String
        Get
            Return _dlci_rem
        End Get
        Set(ByVal value As String)
            _dlci_rem = value
        End Set
    End Property


    Public Property Dlci_Mat() As String
        Get
            Return _dlci_mat
        End Get
        Set(ByVal value As String)
            _dlci_mat = value
        End Set
    End Property


    Public Property Lmi_Rem() As String
        Get
            Return _lmi_rem
        End Get
        Set(ByVal value As String)
            _lmi_rem = value
        End Set
    End Property


    Public Property Data_Ati() As String
        Get
            Return _data_ati
        End Get
        Set(ByVal value As String)
            _data_ati = value
        End Set
    End Property


    Public Property Data_Des() As String
        Get
            Return _data_des
        End Get
        Set(ByVal value As String)
            _data_des = value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return _status
        End Get
        Set(ByVal value As Integer)
            _status = value
        End Set
    End Property

    Public Property Uf() As String
        Get
            Return _uf
        End Get
        Set(ByVal value As String)
            _uf = value
        End Set
    End Property

    Public Property Salva() As String
        Get
            Return _salva
        End Get
        Set(ByVal value As String)
            _salva = value
        End Set
    End Property


    Public Property OBS_() As String
        Get
            Return _obs_
        End Get
        Set(ByVal value As String)
            _obs_ = value
        End Set
    End Property

    Public Property ENDERECO_B_() As String
        Get
            Return _endereco_b
        End Get
        Set(ByVal value As String)
            _endereco_b = value
        End Set
    End Property

    Public Property Valor_Ativ() As String
        Get
            Return _valor_ativ
        End Get
        Set(ByVal value As String)
            _valor_ativ = value
        End Set
    End Property

    Public Property Valor_Link() As String
        Get
            Return _valor_link
        End Get
        Set(ByVal value As String)
            _valor_link = value
        End Set
    End Property

    Public Property Cod_cliente_conta() As String
        Get
            Return _cod_cliente_conta
        End Get
        Set(ByVal value As String)
            _cod_cliente_conta = value
        End Set
    End Property

    Public Property NumeroFatura() As String
        Get
            Return _num_fatura
        End Get
        Set(ByVal value As String)
            _num_fatura = value
        End Set
    End Property

    Public Property NumeroContratoTVG() As String
        Get
            Return _num_contrato_tvg
        End Get
        Set(ByVal value As String)
            _num_contrato_tvg = value
        End Set
    End Property

    Public Property NumeroContratoOP() As String
        Get
            Return _num_contrato_op
        End Get
        Set(ByVal value As String)
            _num_contrato_op = value
        End Set
    End Property

    Public Property NumeroOC() As String
        Get
            Return _numero_oc
        End Get
        Set(ByVal value As String)
            _numero_oc = value
        End Set
    End Property

    Public Property DataIPCA() As String
        Get
            Return _data_IPCA
        End Get
        Set(ByVal value As String)
            _data_IPCA = value
        End Set
    End Property

    Public Property ValorMensal() As String
        Get
            Return _valor_mensal
        End Get
        Set(ByVal value As String)
            _valor_mensal = value
        End Set
    End Property

    Public Property Sponsor() As String
        Get
            Return _sponsor
        End Get
        Set(ByVal value As String)
            _sponsor = value
        End Set
    End Property

    Public Property Endereco_cob() As String
        Get
            Return _endereco_cob
        End Get
        Set(ByVal value As String)
            _endereco_cob = value
        End Set
    End Property

    Public Property UF_B() As String
        Get
            Return _uf_b
        End Get
        Set(ByVal value As String)
            _uf_b = value
        End Set
    End Property

    Public Property Codigo_Cidade_B() As String
        Get
            Return _codigo_cidade_B
        End Get
        Set(ByVal value As String)
            _codigo_cidade_B = value
        End Set
    End Property

    Public Property Num_Contrato_Op_As() As String
        Get
            Return _num_contrato_op_as
        End Get
        Set(ByVal value As String)
            _num_contrato_op_as = value
        End Set
    End Property

    Public Property Contato_B() As String
        Get
            Return _contato_b
        End Get
        Set(ByVal value As String)
            _contato_b = value
        End Set
    End Property

    Public Property Tel_cont_a() As String
        Get
            Return _tel_cont_a
        End Get
        Set(ByVal value As String)
            _tel_cont_a = value
        End Set
    End Property

    Public Property Email_cont_a() As String
        Get
            Return _email_cont_a
        End Get
        Set(ByVal value As String)
            _email_cont_a = value
        End Set
    End Property

    Public Property Tel_cont_b() As String
        Get
            Return _tel_cont_b
        End Get
        Set(ByVal value As String)
            _tel_cont_b = value
        End Set
    End Property

    Public Property Email_Cont_B() As String
        Get
            Return _email_cont_b
        End Get
        Set(ByVal value As String)
            _email_cont_b = value
        End Set
    End Property

    Public Property Sucursal() As String
        Get
            Return _sucursal
        End Get
        Set(ByVal value As String)
            _sucursal = value
        End Set
    End Property

    Public Property Conta_cont() As String
        Get
            Return _conta_cont
        End Get
        Set(ByVal value As String)
            _conta_cont = value
        End Set
    End Property

    Public Property Oem() As String
        Get
            Return _oem
        End Get
        Set(ByVal value As String)
            _oem = value
        End Set
    End Property

End Class