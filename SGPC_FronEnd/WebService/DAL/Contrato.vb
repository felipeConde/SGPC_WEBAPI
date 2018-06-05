Imports Microsoft.VisualBasic

Public Class Contrato
    Private _num_contrato As String
    Private _codigo As Integer
    Private _empresa As String
    Private _codigo_fornecedor As Integer
    Private _objeto As String
    Private _multas As String
    Private _descontos As Double
    Private _periodicidade As String
    Private _data_assinatura As Date
    Private _data_inicio As Date
    Private _indice_reajuste As String
    Private _data_reajuste As Date
    Private _status As String
    Private _data_vencimento As Date
    Private _possui_anexo As String
    Private _possui_alerta As String
    Private _data_alerta As Date
    Private _valor_contrato As Double
    Private _assinatura As String
    Private _codigo_responsavel As Integer
    Private _obs As String
    Private _sla As String
    Private _anexos As String
    Private _copia_contrato As String

#Region "Construtor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCodigo As Integer, ByVal pNum_Contrato As String, ByVal pEmpresa As String, ByVal pCodigo_Fornecedor As Integer, ByVal pObjeto As String, ByVal pMultas As String, ByVal pDescontos As Double, ByVal pPeriodicidade As String, ByVal pData_Assinatura As Date, ByVal pData_inicio As Date, ByVal pIndice_Reajuste As String, ByVal pData_reajuste As Date, ByVal pStatus As String, ByVal pData_vencimento As Date, ByVal pPossui_anexo As String, ByVal pPossui_alerta As String, ByVal pData_alerta As Date, ByVal pValor_Contrato As Double, ByVal pAssinatura As String, ByVal pCodigo_responsavel As Integer, ByVal pObs As String, ByVal pSLA As String, ByVal pAnexos As String, ByVal pCopia_Contrato As String)

        Codigo = pCodigo
        Empresa = pEmpresa
        Codigo_Fornecedor = pCodigo_Fornecedor
        Objeto = pObjeto
        Multas = pMultas
        Descontos = pDescontos
        Periodicidade = pPeriodicidade
        Data_Assinatura = pData_Assinatura
        Data_Inicio = pData_inicio
        Indice_Reajuste = pIndice_Reajuste
        Data_Reajuste = pData_reajuste
        Status = pStatus
        Data_Vencimento = pData_vencimento
        Possui_Anexo = pPossui_anexo
        Possui_Alerta = pPossui_alerta
        Data_Alerta = pData_alerta
        Valor_Contrato = pValor_Contrato
        Assinatura = pAssinatura
        Codigo_Responsavel = pCodigo_responsavel
        Obs = pObs
        SLA = pSLA
        Anexos = pAnexos
        Copia_Contrato = pCopia_Contrato
        Num_Contrato = pNum_Contrato





    End Sub

#End Region


#Region "Propriedades"

    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Public Property Num_Contrato As String
        Get
            Return _num_contrato
        End Get
        Set(ByVal value As String)
            _num_contrato = value
        End Set
    End Property

    Public Property Empresa As String
        Get
            Return _empresa
        End Get
        Set(ByVal value As String)
            _empresa = value
        End Set
    End Property

    Public Property Codigo_Fornecedor As Integer
        Get
            Return _codigo_fornecedor

        End Get
        Set(ByVal value As Integer)
            _codigo_fornecedor = value
        End Set
    End Property

    Public Property Objeto As String
        Get
            Return _objeto
        End Get
        Set(ByVal value As String)
            _objeto = value
        End Set
    End Property

    Public Property Multas As String
        Get
            Return _multas
        End Get
        Set(ByVal value As String)
            _multas = value
        End Set
    End Property

    Public Property Descontos As Double
        Get
            Return _descontos
        End Get
        Set(ByVal value As Double)
            _descontos = value
        End Set
    End Property

    Public Property Periodicidade As String
        Get
            Return _periodicidade
        End Get
        Set(ByVal value As String)
            _periodicidade = value
        End Set
    End Property

    Public Property Data_Assinatura As Date
        Get
            Return _data_assinatura
        End Get
        Set(ByVal value As Date)
            _data_assinatura = value
        End Set
    End Property

    Public Property Data_Inicio As Date
        Get
            Return _data_inicio
        End Get
        Set(ByVal value As Date)
            _data_inicio = value
        End Set
    End Property

    Public Property Indice_Reajuste As String
        Get
            Return _indice_reajuste
        End Get
        Set(ByVal value As String)
            _indice_reajuste = value
        End Set
    End Property

    Public Property Data_Reajuste As Date
        Get
            Return _data_reajuste
        End Get
        Set(ByVal value As Date)
            _data_reajuste = value
        End Set
    End Property

    Public Property Status As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
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

    Public Property Possui_Anexo As String
        Get
            Return _possui_anexo
        End Get
        Set(ByVal value As String)
            _possui_anexo = value
        End Set
    End Property

    Public Property Possui_Alerta As String
        Get
            Return _possui_alerta
        End Get
        Set(ByVal value As String)
            _possui_alerta = value
        End Set
    End Property

    Public Property Data_Alerta As Date
        Get
            Return _data_alerta
        End Get
        Set(ByVal value As Date)
            _data_alerta = value
        End Set
    End Property

    Public Property Valor_Contrato As Double
        Get
            Return _valor_contrato
        End Get
        Set(ByVal value As Double)
            _valor_contrato = value
        End Set
    End Property

    Public Property Assinatura As String
        Get
            Return _assinatura
        End Get
        Set(ByVal value As String)
            _assinatura = value
        End Set
    End Property

    Public Property Codigo_Responsavel As Integer
        Get
            Return _codigo_responsavel

        End Get
        Set(ByVal value As Integer)
            _codigo_responsavel = value
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

    Public Property SLA As String
        Get
            Return _sla
        End Get
        Set(ByVal value As String)
            _sla = value
        End Set
    End Property

    Public Property Anexos As String
        Get
            Return _anexos
        End Get
        Set(ByVal value As String)
            _anexos = value
        End Set
    End Property

    Public Property Copia_Contrato As String
        Get
            Return _copia_contrato
        End Get
        Set(ByVal value As String)
            _copia_contrato = value
        End Set
    End Property

#End Region

End Class
