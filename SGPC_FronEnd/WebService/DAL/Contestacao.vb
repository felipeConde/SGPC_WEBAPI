Imports Microsoft.VisualBasic

Public Class Contestacao
   Private _id As Integer
   Private _data_conte As Date
   Private _competencia As Date
   Private _fatura As String
   Private _codigo_conta As Integer
   'Private _document As Image  'BLOB
   Private _autor As String
   Private _codigo_usuario As Integer
   Private _codigo_tipo As Integer
   Private _codigo_fatura As Integer
   Private _valor_cobrado As Double
   Private _valor_contestado As Double
   Private _valor_devolvido As Double
   Private _status As Integer
   Private _numero_chamado_oper As String
   Private _protocolo As String
   Private _data_abertura As Date
   Private _data_conclusao As Date
   Private _numero_protocolo As String
   Private _tipo_devolucao As String
   Private _num_deposito As String
   Private _num_fatura_credito As String
   Private _fatura_credito_data As Date
   Private _refaturamento_valor As Double
   Private _outros As String
   Private _tarifas As String
   Private _arquivo_url As String

   Private _data_vencimento As Date
   Private _valor_fatura As Double
   Private _codigo_cliente As String
   Private _codigo_operadora As Integer
    Private _operadora As String
    Private _status_desc As String


#Region "Propiedades"
	Public Property ID As Integer
		Get
			Return _id
		End Get
		Set(ByVal value As Integer)
			_id = value
		End Set
	End Property

    Public Property Data_Conte As Date
        Get
            Return _data_conte
        End Get
        Set(ByVal value As Date)
            _data_conte = value
        End Set
    End Property

    Public Property Competencia As Date
        Get
            Return _competencia
        End Get
        Set(ByVal value As Date)
            _competencia = value
        End Set
    End Property

    Public Property Fatura As String
        Get
            Return _fatura
        End Get
        Set(ByVal value As String)
            _fatura = value
        End Set
    End Property

    Public Property Codigo_Conta As Integer
        Get
            Return _codigo_conta
        End Get
        Set(ByVal value As Integer)
            _codigo_conta = value
        End Set
    End Property

    'Public Property Document As Image
    '    Get
    '        Return _document
    '    End Get
    '    Set(ByVal value As Image)
    '        _document = value
    '    End Set
    'End Property

    Public Property Autor As String
        Get
            Return _autor
        End Get
        Set(ByVal value As String)
            _autor = value
        End Set
    End Property

    Public Property Codigo_Usuario As Integer
        Get
            Return _codigo_usuario
        End Get
        Set(ByVal value As Integer)
            _codigo_usuario = value
        End Set
    End Property

    Public Property Codigo_Tipo As Integer
        Get
            Return _codigo_tipo
        End Get
        Set(ByVal value As Integer)
            _codigo_tipo = value
        End Set
    End Property

    Public Property Codigo_Fatura As Integer
        Get
            Return _codigo_fatura
        End Get
        Set(ByVal value As Integer)
            _codigo_fatura = value
        End Set
    End Property

    Public Property Valor_Cobrado As Double
        Get
            Return _valor_cobrado
        End Get
        Set(ByVal value As Double)
            _valor_cobrado = value
        End Set
    End Property

    Public Property Valor_Contestado As Double
        Get
            Return _valor_contestado
        End Get
        Set(ByVal value As Double)
            _valor_contestado = value
        End Set
    End Property

    Public Property Valor_Devolvido As Double
        Get
            Return _valor_devolvido
        End Get
        Set(ByVal value As Double)
            _valor_devolvido = value
        End Set
    End Property

    Public Property Status As Integer
        Get
            Return _status
        End Get
        Set(ByVal value As Integer)
            _status = value
        End Set
    End Property

    Public Property Numero_Chamado_Oper As String
        Get
            Return _numero_chamado_oper
        End Get
        Set(ByVal value As String)
            _numero_chamado_oper = value
        End Set
    End Property

    Public Property Protocolo As String
        Get
            Return _protocolo
        End Get
        Set(ByVal value As String)
            _protocolo = value
        End Set
    End Property

    Public Property Data_Abertura As Date
        Get
            Return _data_abertura
        End Get
        Set(ByVal value As Date)
            _data_abertura = value
        End Set
    End Property

    Public Property Data_Conclusao As Date
        Get
            Return _data_conclusao
        End Get
        Set(ByVal value As Date)
            _data_conclusao = value
        End Set
    End Property

    Public Property Numero_Protocolo As String
        Get
            Return _numero_protocolo
        End Get
        Set(ByVal value As String)
            _numero_protocolo = value
        End Set
    End Property

    Public Property Tipo_Devolucao As String
        Get
            Return _tipo_devolucao
        End Get
        Set(ByVal value As String)
            _tipo_devolucao = value
        End Set
    End Property

    Public Property Num_Deposito As String
        Get
            Return _num_deposito
        End Get
        Set(ByVal value As String)
            _num_deposito = value
        End Set
    End Property

    Public Property Num_Fatura_Credito As String
        Get
            Return _num_fatura_credito
        End Get
        Set(ByVal value As String)
            _num_fatura_credito = value
        End Set
    End Property

    Public Property Fatura_Credito_Data As Date
        Get
            Return _fatura_credito_data
        End Get
        Set(ByVal value As Date)
            _fatura_credito_data = value
        End Set
    End Property

    Public Property Refaturamento_Valor As Double
        Get
            Return _refaturamento_valor
        End Get
        Set(ByVal value As Double)
            _refaturamento_valor = value
        End Set
    End Property

    Public Property Outros As String
        Get
            Return _outros
        End Get
        Set(ByVal value As String)
            _outros = value
        End Set
    End Property

    Public Property Tarifas As String
        Get
            Return _tarifas
        End Get
        Set(ByVal value As String)
            _tarifas = value
        End Set
    End Property

    Public Property Arquivo_Url As String
        Get
            Return _arquivo_url
        End Get
        Set(ByVal value As String)
            _arquivo_url = value
        End Set
    End Property


   Public Property Data_Vencimento As Date
      Get
         Return _data_vencimento
      End Get
      Set(ByVal value As Date)
         _data_vencimento = value
      End Set
   End Property

    Public Property Valor_Fatura As Double
        Get
            Return _valor_fatura
        End Get
        Set(ByVal value As Double)
            _valor_fatura = value
        End Set
    End Property

   Public Property Codigo_Cliente As String
      Get
         Return _codigo_cliente
      End Get
      Set(ByVal value As String)
         _codigo_cliente = value
      End Set
   End Property

   Public Property Codigo_Operadora As Integer
      Get
         Return _codigo_operadora
      End Get
      Set(ByVal value As Integer)
         _codigo_operadora = value
      End Set
   End Property

    Public Property Operadora As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

    Public Property Status_Desc As String
        Get
            Return _status_desc
        End Get
        Set(ByVal value As String)
            _status_desc = value
        End Set
    End Property

#End Region

#Region "Construtor"
   Public Sub New(ByVal pID As Integer, ByVal pData_Conte As Date, ByVal pCompetencia As String, ByVal pFatura As String, ByVal pCodigo_Conta As Integer, ByVal pAutor As String, ByVal pCodigo_Usuario As Integer, ByVal pCodigo_Tipo As Integer, ByVal pCodigo_Fatura As Integer, ByVal pValor_Cobrado As Double, ByVal pValor_Contestado As Double, ByVal pValor_Devolvido As Double, ByVal pStatus As Integer, ByVal pNumero_Chamado_Oper As String, ByVal pProtocolo As String, ByVal pData_Abertura As Date, ByVal pData_Conclusao As Date, ByVal pNumero_Protocolo As String, ByVal pTipo_Devolucao As String, ByVal pNum_Deposito As String, ByVal pNum_Fatura_Credito As String, ByVal pFatura_Credito_Data As Date, ByVal pRefaturamento_Valor As Double, ByVal pOutros As String, ByVal pTarifas As String, ByVal pArquivo_Url As String, ByVal pData_Vencimento As Date, ByVal pValor_Fatura As Double, ByVal pCodigo_Cliente As String, ByVal pCodigo_Operadora As Integer, ByVal pOperadora As String)

      ID = pID
      Data_Conte = pData_Conte
      Competencia = pCompetencia
      Fatura = pFatura
      Codigo_Conta = pCodigo_Conta
      'Document = pDocument 
      Autor = pAutor
      Codigo_Usuario = pCodigo_Usuario
      Codigo_Tipo = pCodigo_Tipo
      Codigo_Fatura = pCodigo_Fatura
      Valor_Cobrado = pValor_Cobrado
      Valor_Contestado = pValor_Contestado
      Valor_Devolvido = pValor_Devolvido
      Status = pStatus
      Numero_Chamado_Oper = pNumero_Chamado_Oper
      Protocolo = pProtocolo
      Data_Abertura = pData_Abertura
      Data_Conclusao = pData_Conclusao
      Numero_Protocolo = pNumero_Protocolo
      Tipo_Devolucao = pTipo_Devolucao
      Num_Deposito = pNum_Deposito
      Num_Fatura_Credito = pNum_Fatura_Credito
      Fatura_Credito_Data = pFatura_Credito_Data
      Refaturamento_Valor = pRefaturamento_Valor
      Outros = pOutros
      Tarifas = pTarifas
      Arquivo_Url = pArquivo_Url

      Data_Vencimento = pData_Vencimento
      Valor_Fatura = pValor_Fatura
      Codigo_Cliente = pCodigo_Cliente
      Codigo_Operadora = pCodigo_Operadora
      Operadora = pOperadora

   End Sub

#End Region

End Class
