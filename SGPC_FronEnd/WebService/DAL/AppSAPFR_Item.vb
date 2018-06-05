Imports Microsoft.VisualBasic

Public Class AppSAPFR_Item

    'Nº externo da folha de registro de serviços
    Private _LBLNE_LBLNE1 As String
    Public Property LBLNE_LBLNE1 As String
        Get
            Return _LBLNE_LBLNE1
        End Get
        Set(value As String)
            _LBLNE_LBLNE1 = value
        End Set
    End Property

    'Nº conta do Razão
    Private _SAKTO_SAKNR As String
    Public Property SAKTO_SAKNR As String
        Get
            Return _SAKTO_SAKNR
        End Get
        Set(value As String)
            _SAKTO_SAKNR = value
        End Set
    End Property

    'Centro de custo
    Private _KOSTL_KOSTL As String
    Public Property KOSTL_KOSTL As String
        Get
            Return _KOSTL_KOSTL
        End Get
        Set(value As String)
            _KOSTL_KOSTL = value
        End Set
    End Property

    'Nº ordem
    Private _AUFNR_AUFNR As String
    Public Property AUFNR_AUFNR As String
        Get
            Return _AUFNR_AUFNR
        End Get
        Set(value As String)
            _AUFNR_AUFNR = value
        End Set
    End Property

    'Elemento do plano da estrutura do projeto (elemento PEP)
    Private _PS_PSP_PNR_PS_PSP_PNR As String
    Public Property POSID_PS_POSID As String
        Get
            Return _PS_PSP_PNR_PS_PSP_PNR
        End Get
        Set(value As String)
            _PS_PSP_PNR_PS_PSP_PNR = value
        End Set
    End Property

    'Nº da linha
    Private _EXTROW_EXTROW As Integer
    Public Property EXTROW_EXTROW As Integer
        Get
            Return _EXTROW_EXTROW
        End Get
        Set(value As Integer)
            _EXTROW_EXTROW = value
        End Set
    End Property


    'Qtd.com símbolo +/-

    Private _MENGE_MENGEV As Double
    Public Property MENGE_MENGEV As Double
        Get
            Return _MENGE_MENGEV
        End Get
        Set(value As Double)
            _MENGE_MENGEV = value
        End Set
    End Property

End Class
