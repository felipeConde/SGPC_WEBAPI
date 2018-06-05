Imports Microsoft.VisualBasic

Public Class Fatura

    Public Sub New()

    End Sub

    Public Sub New(ByVal pFatura As String, ByVal pDtReferencia As Date)
        _fatura = pFatura
        _dtReferencia = pDtReferencia
    End Sub
    Public Sub New(ByVal pFatura As String, ByVal pDtReferencia As Date, ByVal pIntevalo As Integer, ByVal pCodigoOperadora As Integer, ByVal pOperadora As String)
        _fatura = pFatura
        _dtReferencia = pDtReferencia
        _intervaloMes = pIntevalo
        _codigoOperadora = pCodigoOperadora
        _operadora = pOperadora
    End Sub
    Public Sub New(ByVal pId As Integer, ByVal pFatura As String, ByVal pDtReferencia As Date, ByVal pIntevalo As Integer, ByVal pCodigoOperadora As Integer, ByVal pOperadora As String, ByVal pCodigoTipo As Integer)
        _id = pId
        _fatura = pFatura
        _dtReferencia = pDtReferencia
        _intervaloMes = pIntevalo
        _codigoOperadora = pCodigoOperadora
        _operadora = pOperadora
        _codigoTipo = pCodigoTipo
    End Sub

    Public Sub New(ByVal pId As Integer, ByVal pFatura As String, ByVal pDtReferencia As Date, ByVal pIntevalo As Integer, ByVal pCodigoOperadora As Integer, ByVal pOperadora As String, ByVal pCodigoTipo As Integer, ByVal pEstado As Estado, ByVal pIdentContaUnica As String, ByVal pDebitoAutomatico As String, ByVal pDiaVencimento As String, ByVal pValor As Double, ByVal pDataFim As Date, ByVal pfebraban As String, ByVal pCNPJ As String, ByVal pNomeCliente As String)
        _id = pId
        _fatura = pFatura
        _dtReferencia = pDtReferencia
        _intervaloMes = pIntevalo
        _codigoOperadora = pCodigoOperadora
        _operadora = pOperadora
        _codigoTipo = pCodigoTipo
        _identContaUnica = pIdentContaUnica
        _estado = pEstado
        DebitoAutomatico = pDebitoAutomatico
        _diaVencimento = pDiaVencimento
        _valor = pValor
        _data_fim = pDataFim
        _febraban = pfebraban
        _CNPJ = pCNPJ
        _NomeCliente = pNomeCliente
    End Sub

    Public Sub New(ByVal pId As Integer, ByVal pFatura As String, ByVal pDtReferencia As Date, ByVal pIntevalo As Integer, ByVal pCodigoOperadora As Integer, ByVal pOperadora As String, ByVal pCodigoTipo As Integer, ByVal pEstado As Estado, ByVal pIdentContaUnica As String, ByVal pDebitoAutomatico As String, ByVal pDiaVencimento As String, ByVal pValor As Double, ByVal pDataFim As Date, ByVal pfebraban As String, ByVal pCNPJ As String, ByVal pNomeCliente As String, ByVal pCodigoFornecedor As Integer, ByVal pArquivo As String, ByVal pDtVencimento As Date, ByVal pCodigoStatus As Integer, ByVal pCodigo_servico As Integer, ByVal PNotaFiscal As String, ByVal pValorPago As Double, ByVal pOp As String, ByVal pDataPgto As Date, ByVal pjustificativa As String)
        _id = pId
        _fatura = pFatura
        _dtReferencia = pDtReferencia
        _intervaloMes = pIntevalo
        _codigoOperadora = pCodigoOperadora
        _operadora = pOperadora
        _codigoTipo = pCodigoTipo
        _identContaUnica = pIdentContaUnica
        _estado = pEstado
        DebitoAutomatico = pDebitoAutomatico
        _diaVencimento = pDiaVencimento
        _valor = pValor
        _data_fim = pDataFim
        _febraban = pfebraban
        _CNPJ = pCNPJ
        _NomeCliente = pNomeCliente
        _codigo_fornecedor = pCodigoFornecedor
        _arquivo = pArquivo
        _dtVencimento = pDtVencimento
        _codigo_status = pCodigoStatus
        _codigo_servico = pCodigo_servico
        _notaFiscal = PNotaFiscal
        _valor_pago = pValorPago
        _op = pOp
        _data_pgto = pDataPgto
        _justificativa = pjustificativa
    End Sub



    Private _id As Integer
    Private _fatura As String
    Private _dtReferencia As String
    Private _intervaloMes As Integer
    Private _codigoOperadora As Integer
    Private _operadora As String
    Private _codigoTipo As Integer
    Private _identContaUnica As String
    Private _estado As Estado
    Private _debitoAutomatico As String
    Private _diaVencimento As Integer
    Private _carregada As String '1-Carregada 2-Não carregada 3-Não cadastrada
    Private _valor As Double
    Private _data_fim As Date
    Private _febraban As String
    Private _CNPJ As String
    Private _NomeCliente As String
    Private _valorCarregado As Double
    Private _notaFiscal As String
    Private _codigoConta As Integer
    Private _dtVencimento As Date
    Private _tipo As String
    Private _justificativa As String
    Private _ciclo_ini As String
    Private _ciclo_fim As String

    Public Property Ciclo_ini() As String
        Get
            Return _ciclo_ini
        End Get
        Set(ByVal value As String)
            _ciclo_ini = value
        End Set
    End Property
    Public Property Ciclo_fim() As String
        Get
            Return _ciclo_fim
        End Get
        Set(ByVal value As String)
            _ciclo_fim = value
        End Set
    End Property


    Public Property Fatura() As String
        Get
            Return _fatura
        End Get
        Set(ByVal value As String)
            _fatura = value
        End Set
    End Property

    Public Property DtReferencia() As Date
        Get
            Return _dtReferencia
        End Get
        Set(ByVal value As Date)
            _dtReferencia = value
        End Set
    End Property

    Public Property IntevaloMes() As Integer
        Get
            Return _intervaloMes
        End Get
        Set(ByVal value As Integer)
            _intervaloMes = value
        End Set
    End Property

    Public Property CodigoOperadora() As Integer
        Get
            Return _codigoOperadora
        End Get
        Set(ByVal value As Integer)
            _codigoOperadora = value
        End Set
    End Property

    Public Property Operadora() As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property CodigoTipo() As Integer
        Get
            Return _codigoTipo
        End Get
        Set(ByVal value As Integer)
            _codigoTipo = value
        End Set
    End Property

    Public Property IndentContaUnica() As String
        Get
            Return _identContaUnica
        End Get
        Set(ByVal value As String)
            _identContaUnica = value
        End Set
    End Property

    Public Property Estado() As Estado
        Get
            Return _estado
        End Get
        Set(ByVal value As Estado)
            _estado = value
        End Set
    End Property

    Public Property DebitoAutomatico() As String
        Get
            Return _debitoAutomatico
        End Get
        Set(ByVal value As String)
            _debitoAutomatico = value

        End Set
    End Property

    Public Property DiaVencimento() As Integer
        Get
            Return _diaVencimento
        End Get
        Set(ByVal value As Integer)
            _diaVencimento = value
        End Set
    End Property

    Public Property Carregada() As String
        Get
            Return _carregada
        End Get
        Set(ByVal value As String)
            _carregada = value
        End Set
    End Property

    Public Property Valor() As Double
        Get
            Return _valor
        End Get
        Set(ByVal value As Double)
            _valor = value
        End Set
    End Property

    Public Property DataFim As Date
        Get
            Return _data_fim
        End Get
        Set(ByVal value As Date)
            _data_fim = value
        End Set
    End Property

    Public Property Febraban As String
        Get
            Return _febraban
        End Get
        Set(ByVal value As String)
            _febraban = value
        End Set
    End Property

    Public Property CNPJ As String
        Get
            Return _CNPJ
        End Get
        Set(ByVal value As String)
            _CNPJ = value
        End Set
    End Property

    Public Property NomeCliente As String
        Get
            Return _NomeCliente
        End Get
        Set(ByVal value As String)
            _NomeCliente = value
        End Set
    End Property

    Public Property Valorcarregado As Double
        Get
            Return _valorCarregado
        End Get
        Set(ByVal value As Double)
            _valorCarregado = value
        End Set
    End Property

    Public Property NotaFiscal As String
        Get
            Return _notaFiscal
        End Get
        Set(ByVal value As String)

            If String.IsNullOrEmpty(value) Then
                value = "-"
            End If
            _notaFiscal = value

        End Set
    End Property

    Public Property CodigoConta As Integer
        Get
            Return _codigoConta
        End Get
        Set(ByVal value As Integer)
            _codigoConta = value
        End Set
    End Property


    Public Property DTVencimento As Date
        Get
            Return _dtVencimento
        End Get
        Set(ByVal value As Date)
            _dtVencimento = value
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

    'novos campos em 22/05/2012
    Private _codigo_status As Integer
    Public Property Codigo_Status As Integer
        Get
            Return _codigo_status
        End Get
        Set(ByVal value As Integer)
            _codigo_status = value
        End Set
    End Property

    Private _valor_pago As Double
    Public Property Valor_Pago As Double
        Get
            Return _valor_pago
        End Get
        Set(ByVal value As Double)
            _valor_pago = value
        End Set
    End Property

    Private _data_pgto As Date
    Public Property Data_pgto As Date
        Get
            Return _data_pgto
        End Get
        Set(ByVal value As Date)
            _data_pgto = value
        End Set
    End Property

    Private _codigo_servico As Integer
    Public Property Codigo_Servico As Integer
        Get
            Return _codigo_servico
        End Get
        Set(ByVal value As Integer)
            _codigo_servico = value
        End Set
    End Property

    Private _op As String
    Public Property Op As String
        Get
            Return _op
        End Get
        Set(ByVal value As String)
            _op = value
        End Set
    End Property

    Private _dt_criacao As Date
    Public Property Dt_Criacao As Date
        Get
            Return _dt_criacao
        End Get
        Set(ByVal value As Date)
            _dt_criacao = value
        End Set
    End Property

    Private _codigo_fornecedor As Integer
    Public Property Codigo_Fornecedor As Integer
        Get
            Return _codigo_fornecedor
        End Get
        Set(ByVal value As Integer)
            _codigo_fornecedor = value
        End Set
    End Property

    Private _codigo_plano As Integer
    Public Property Codigo_Plano As Integer
        Get
            Return _codigo_plano
        End Get
        Set(ByVal value As Integer)
            _codigo_plano = value
        End Set
    End Property

    Private _ativa As String
    Public Property Ativa As String
        Get
            Return _ativa
        End Get
        Set(ByVal value As String)
            _ativa = value
        End Set
    End Property

    Private _periodica As String
    Public Property Periodica As String
        Get
            Return _periodica
        End Get
        Set(ByVal value As String)
            _periodica = value
        End Set
    End Property

    Private _arquivo As String
    Public Property Arquivo As String
        Get
            Return _arquivo
        End Get
        Set(ByVal value As String)
            _arquivo = value
        End Set
    End Property

    Private _Status_Desc As String
    Public Property Status_desc As String
        Get
            Return _Status_Desc
        End Get
        Set(ByVal value As String)
            _Status_Desc = value
        End Set
    End Property

    Private _valor_contestado As Double
    Public Property ValorContestado As Double
        Get
            Return _valor_contestado
        End Get
        Set(ByVal value As Double)
            _valor_contestado = value
        End Set
    End Property

    'novos campos 04/10/2012
    Private _dt_financeiro As Date
    Public Property DT_Financeiro As Date
        Get
            Return _dt_financeiro
        End Get
        Set(ByVal value As Date)
            _dt_financeiro = value
        End Set
    End Property

    Private _lote As String
    Public Property Lote As String
        Get
            Return _lote
        End Get
        Set(ByVal value As String)
            _lote = value
        End Set
    End Property

    Private _valor_provisionado As Double
    Public Property ValorProvisionado As Double
        Get
            Return _valor_provisionado
        End Get
        Set(ByVal value As Double)
            _valor_provisionado = value
        End Set
    End Property

    Private _servico_desc As String
    Public Property Servico_Desc As String
        Get
            Return _servico_desc
        End Get
        Set(ByVal value As String)
            _servico_desc = value
        End Set
    End Property

    Private _staus_Ativacao As String
    Public Property Staus_Ativacao As String
        Get
            Return _staus_Ativacao
        End Get
        Set(ByVal value As String)
            _staus_Ativacao = value
        End Set
    End Property


    Private _status_agendamento As String
    Public Property Status_Agendamento() As String
        Get
            Return _status_agendamento
        End Get
        Set(ByVal value As String)
            _status_agendamento = value
        End Set
    End Property

    Private _obs As String
    Public Property OBS() As String
        Get
            Return _obs
        End Get
        Set(ByVal value As String)
            _obs = value
        End Set
    End Property

    Public Property Justificativa() As String
        Get
            Return _justificativa
        End Get
        Set(ByVal value As String)
            _justificativa = value
        End Set
    End Property

    Private _justificativa_pgto As Integer
    Public Property Justificativa_PGTO() As Integer
        Get
            Return _justificativa_pgto
        End Get
        Set(ByVal value As Integer)
            _justificativa_pgto = value
        End Set
    End Property

    Private _data_encaminhamento As Integer
    Public Property Data_Encaminhamento() As Integer
        Get
            Return _data_encaminhamento
        End Get
        Set(ByVal value As Integer)
            _data_encaminhamento = value
        End Set
    End Property

    Private _valor_correto As Double
    Public Property ValorCorreto As Double
        Get
            Return _valor_correto
        End Get
        Set(ByVal value As Double)
            _valor_correto = value
        End Set
    End Property


    Private _statusContestacao As String
    Public Property StatusContestacao As String
        Get
            Return _statusContestacao
        End Get
        Set(value As String)
            _statusContestacao = value
        End Set
    End Property

    Private _valorContestadoAprovado As Double
    Public Property ValorContestadoAprovado As Double
        Get
            Return _valorContestadoAprovado
        End Get
        Set(ByVal value As Double)
            _valorContestadoAprovado = value
        End Set
    End Property


    Private _dt_novo_vencimento As Date
    Public Property DT_Novo_Vencimento As Date
        Get
            Return _dt_novo_vencimento
        End Get
        Set(ByVal value As Date)
            _dt_novo_vencimento = value
        End Set
    End Property

End Class
