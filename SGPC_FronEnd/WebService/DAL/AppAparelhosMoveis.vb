Imports Microsoft.VisualBasic

Public Class AppAparelhosMoveis

    Private _codigo As Integer

    Public Property Codigo() As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Private _chamado As String

    Public Property Chamado() As String
        Get
            Return _chamado
        End Get
        Set(ByVal value As String)
            _chamado = value
        End Set
    End Property

    Private _classificacao As String

    Public Property Classificacao() As String
        Get
            Return _classificacao
        End Get
        Set(ByVal value As String)
            _classificacao = value
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

    Private _plano As String

    Public Property Plano() As String
        Get
            Return _plano
        End Get
        Set(ByVal value As String)
            _plano = value
        End Set
    End Property

    Private _pin1 As String

    Public Property Pin1() As String
        Get
            Return _pin1
        End Get
        Set(ByVal value As String)
            _pin1 = value
        End Set
    End Property

    Private _pin2 As String

    Public Property Pin2() As String
        Get
            Return _pin2
        End Get
        Set(ByVal value As String)
            _pin2 = value
        End Set
    End Property

    Private _puk1 As String

    Public Property Puk1() As String
        Get
            Return _puk1
        End Get
        Set(ByVal value As String)
            _puk1 = value
        End Set
    End Property

    Private _puk2 As String

    Public Property Puk2() As String
        Get
            Return _puk2
        End Get
        Set(ByVal value As String)
            _puk2 = value
        End Set
    End Property

    Private _ip As String

    Public Property Ip() As String
        Get
            Return _ip
        End Get
        Set(ByVal value As String)
            _ip = value
        End Set
    End Property

    Private _telefone As String

    Public Property Telefone() As String
        Get
            Return _telefone
        End Get
        Set(ByVal value As String)
            _telefone = value
        End Set
    End Property

    Private _fleet As String

    Public Property Fleet() As String
        Get
            Return _fleet
        End Get
        Set(ByVal value As String)
            _fleet = value
        End Set
    End Property

    Private _simcard As String

    Public Property Simcard() As String
        Get
            Return _simcard
        End Get
        Set(ByVal value As String)
            _simcard = value
        End Set
    End Property

    Private _simcard_value As String

    Public Property Simcard_value() As String
        Get
            Return _simcard_value
        End Get
        Set(ByVal value As String)
            _simcard_value = value
        End Set
    End Property

    Private _ativacao As String

    Public Property Ativacao() As String
        Get
            Return _ativacao
        End Get
        Set(ByVal value As String)
            _ativacao = value
        End Set
    End Property

    Private _desativacao As String

    Public Property Desativacao() As String
        Get
            Return _desativacao
        End Get
        Set(ByVal value As String)
            _desativacao = value
        End Set
    End Property

    Private _status As String

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _facilidades As String

    Public Property Facilidades() As String
        Get
            Return _facilidades
        End Get
        Set(ByVal value As String)
            _facilidades = value
        End Set
    End Property

    Private _projetos As String

    Public Property Projetos() As String
        Get
            Return _projetos
        End Get
        Set(ByVal value As String)
            _projetos = value
        End Set
    End Property

    Private _codigo_cliente As String

    Public Property Codigo_cliente() As String
        Get
            Return _codigo_cliente
        End Get
        Set(ByVal value As String)
            _codigo_cliente = value
        End Set
    End Property

    Private _intragrupo As String

    Public Property Intragrupo() As String
        Get
            Return _intragrupo
        End Get
        Set(ByVal value As String)
            _intragrupo = value
        End Set
    End Property

    Private _tecnologia As String

    Public Property Tecnologia() As String
        Get
            Return _tecnologia
        End Get
        Set(ByVal value As String)
            _tecnologia = value
        End Set
    End Property

    Private _marca As String

    Public Property Marca() As String
        Get
            Return _marca
        End Get
        Set(ByVal value As String)
            _marca = value
        End Set
    End Property

    Private _modelo As String

    Public Property Modelo() As String
        Get
            Return _modelo
        End Get
        Set(ByVal value As String)
            _modelo = value
        End Set
    End Property

    Private _identificacao As String

    Public Property Identificacao() As String
        Get
            Return _identificacao
        End Get
        Set(ByVal value As String)
            _identificacao = value
        End Set
    End Property

    Private _codigo_aparelho As String

    Public Property Codigo_aparelho() As String
        Get
            Return _codigo_aparelho
        End Get
        Set(ByVal value As String)
            _codigo_aparelho = value
        End Set
    End Property

    Private _valor_aparelho As String

    Public Property Valor_aparelho() As String
        Get
            Return _valor_aparelho
        End Get
        Set(ByVal value As String)
            _valor_aparelho = value
        End Set
    End Property

    Private _nota_fiscal As String

    Public Property Nota_fiscal() As String
        Get
            Return _nota_fiscal
        End Get
        Set(ByVal value As String)
            _nota_fiscal = value
        End Set
    End Property

    Private _natureza As String

    Public Property Natureza() As String
        Get
            Return _natureza
        End Get
        Set(ByVal value As String)
            _natureza = value
        End Set
    End Property

    Private _venc_garantia As String

    Public Property Venc_garantia() As String
        Get
            Return _venc_garantia
        End Get
        Set(ByVal value As String)
            _venc_garantia = value
        End Set
    End Property

    Private _venc_comodato As String

    Public Property Venc_comodato() As String
        Get
            Return _venc_comodato
        End Get
        Set(ByVal value As String)
            _venc_comodato = value
        End Set
    End Property

    Private _estoque As String

    Public Property Estoque() As String
        Get
            Return _estoque
        End Get
        Set(ByVal value As String)
            _estoque = value
        End Set
    End Property

    Private _sucata As String

    Public Property Sucata() As String
        Get
            Return _sucata
        End Get
        Set(ByVal value As String)
            _sucata = value
        End Set
    End Property

    Private _backup As String

    Public Property Backup() As String
        Get
            Return _backup
        End Get
        Set(ByVal value As String)
            _backup = value
        End Set
    End Property

    Private _perdido As String

    Public Property Perdido() As String
        Get
            Return _perdido
        End Get
        Set(ByVal value As String)
            _perdido = value
        End Set
    End Property

    Private _prop_estoque As String

    Public Property Prop_estoque() As String
        Get
            Return _prop_estoque
        End Get
        Set(ByVal value As String)
            _prop_estoque = value
        End Set
    End Property

    Private _ordem_serv As String

    Public Property Ordem_serv() As String
        Get
            Return _ordem_serv
        End Get
        Set(ByVal value As String)
            _ordem_serv = value
        End Set
    End Property

    Private _emissao As String

    Public Property Emissao() As String
        Get
            Return _emissao
        End Get
        Set(ByVal value As String)
            _emissao = value
        End Set
    End Property

    Private _chamada_retirada As String

    Public Property Chamada_retirada() As String
        Get
            Return _chamada_retirada
        End Get
        Set(ByVal value As String)
            _chamada_retirada = value
        End Set
    End Property

    Private _data_retirada As String

    Public Property Data_retirada() As String
        Get
            Return _data_retirada
        End Get
        Set(ByVal value As String)
            _data_retirada = value
        End Set
    End Property

    Private _usuario As String

    Public Property Usuario() As String
        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
        End Set
    End Property

    Private _term_resp As String

    Public Property Term_resp() As String
        Get
            Return _term_resp
        End Get
        Set(ByVal value As String)
            _term_resp = value
        End Set
    End Property

    Private _limite_uso As String

    Public Property Limite_uso() As String
        Get
            Return _limite_uso
        End Get
        Set(ByVal value As String)
            _limite_uso = value
        End Set
    End Property

    Private _centro_custo As String

    Public Property Centro_custo() As String
        Get
            Return _centro_custo
        End Get
        Set(ByVal value As String)
            _centro_custo = value
        End Set
    End Property

    Private _obs As String

    Public Property Obs() As String
        Get
            Return _obs
        End Get
        Set(ByVal value As String)
            _obs = value
        End Set
    End Property

    Private _pin_aparelho As String

    Public Property Pin_Aparelho() As String
        Get
            Return _pin_aparelho
        End Get
        Set(ByVal value As String)
            _pin_aparelho = value
        End Set
    End Property

    Private _protocolo_cancel As String

    Public Property Protocolo_cancel() As String
        Get
            Return _protocolo_cancel
        End Get
        Set(ByVal value As String)
            _protocolo_cancel = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo As String)
        _codigo = pcodigo
    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal ppin_aparelho As String, ByVal pchamado As String, ByVal poperadora As String, ByVal pplano As String, ByVal ppin1 As String, ByVal ppin2 As String, ByVal ppuk1 As String, ByVal ppuk2 As String, ByVal pip As String, ByVal ptelefone As String, ByVal pfleet As String, ByVal psimcard As String, ByVal psimcard_value As String, ByVal pativacao As String, ByVal pdesativacao As String, ByVal pstatus As String, ByVal pfacilidades As String, ByVal pprojetos As String, ByVal pcodigo_cliente As String, ByVal pintragrupo As String, ByVal ptecnologia As String, ByVal pmarca As String, ByVal pmodelo As String, ByVal pidentificacao As String, ByVal pcodigo_aparelho As String, ByVal pvalor_aparelho As String, ByVal pnota_fiscal As String, ByVal pnatureza As String, ByVal pvenc_garantia As String, ByVal pvenc_comodato As String, ByVal pestoque As String, ByVal psucata As String, ByVal pbackup As String, ByVal pperdido As String, ByVal pprop_estoque As String, ByVal pordem_serv As String, ByVal pemissao As String, ByVal pchamada_retirada As String, ByVal pdata_retirada As String, ByVal pusuario As String, ByVal pterm_resp As String, ByVal plimite_uso As String, ByVal pcentro_custo As String, ByVal pobs As String)

        _codigo = pcodigo
        _chamado = pchamado
        _operadora = poperadora
        _plano = pplano
        _pin1 = ppin1
        _pin2 = ppin2
        _puk1 = ppuk1
        _puk2 = ppuk2
        _ip = pip
        _telefone = ptelefone
        _fleet = pfleet
        _simcard = psimcard
        _simcard_value = psimcard_value
        _ativacao = pativacao
        _desativacao = pdesativacao
        _status = pstatus
        _facilidades = pfacilidades
        _projetos = pprojetos
        _codigo_cliente = pcodigo_cliente
        _intragrupo = pintragrupo
        _tecnologia = ptecnologia
        _marca = pmarca
        _modelo = pmodelo
        _identificacao = pidentificacao
        _codigo_aparelho = pcodigo_aparelho
        _pin_aparelho = ppin_aparelho
        _valor_aparelho = pvalor_aparelho
        _nota_fiscal = pnota_fiscal
        _natureza = pnatureza
        _venc_garantia = pvenc_garantia
        _venc_comodato = pvenc_comodato
        _estoque = pestoque
        _sucata = psucata
        _backup = pbackup
        _perdido = pperdido
        _prop_estoque = pprop_estoque
        _ordem_serv = pordem_serv
        _emissao = pemissao
        _chamada_retirada = pchamada_retirada
        _data_retirada = pdata_retirada
        _usuario = pusuario
        _term_resp = pterm_resp
        _limite_uso = plimite_uso
        _centro_custo = pcentro_custo
        _obs = pobs

    End Sub

    Private _contrato As String

    Public Property Contrato() As String
        Get
            Return _contrato
        End Get
        Set(ByVal value As String)
            _contrato = value
        End Set
    End Property

    Private _serial_number As String

    Public Property Serial_Number() As String
        Get
            Return _serial_number
        End Get
        Set(ByVal value As String)
            _serial_number = value
        End Set
    End Property

    Private _sucursal As String

    Public Property Sucursal() As String
        Get
            Return _sucursal
        End Get
        Set(ByVal value As String)
            _sucursal = value
        End Set
    End Property

    Private _conta_cont As String

    Public Property Conta_cont() As String
        Get
            Return _conta_cont
        End Get
        Set(ByVal value As String)
            _conta_cont = value
        End Set
    End Property
End Class


