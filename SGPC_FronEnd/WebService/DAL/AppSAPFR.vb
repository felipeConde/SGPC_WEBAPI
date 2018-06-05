Imports Microsoft.VisualBasic

Public Class AppSAPFR

    Public Sub New()

    End Sub


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

    'Responsável interno
    Private _SBNAMAG_SBNAMAG As String
    Public Property SBNAMAG_SBNAMAG As String
        Get
            Return _SBNAMAG_SBNAMAG
        End Get
        Set(value As String)
            _SBNAMAG_SBNAMAG = value
        End Set
    End Property

    'Local da prestação do serviço
    Private _DLORT_DLORT As String
    Public Property DLORT_DLORT As String
        Get
            Return _DLORT_DLORT
        End Get
        Set(value As String)
            _DLORT_DLORT = value
        End Set
    End Property

    'Período
    Private _LZVON_LZVON As String
    Public Property LZVON_LZVON As String
        Get
            Return _LZVON_LZVON
        End Get
        Set(value As String)
            _LZVON_LZVON = value
        End Set
    End Property

    'Fim do período
    Private _LZBIS_LZBIS As String
    Public Property LZBIS_LZBIS As String
        Get
            Return _LZBIS_LZBIS
        End Get
        Set(value As String)
            _LZBIS_LZBIS = value
        End Set
    End Property

    'Texto breve da folha de registro de serviços
    Private _TXZ01_TXZ01_ESSR As String
    Public Property TXZ01_TXZ01_ESSR As String
        Get
            Return _TXZ01_TXZ01_ESSR
        End Get
        Set(value As String)
            _TXZ01_TXZ01_ESSR = value
        End Set
    End Property

    'Nº do documento de compras
    Private _EBELN_EBELN As String
    Public Property EBELN_EBELN As String
        Get
            Return _EBELN_EBELN
        End Get
        Set(value As String)
            _EBELN_EBELN = value
        End Set
    End Property

    'Nº item do documento de compra
    Private _EBELP_EBELP As Double
    Public Property EBELP_EBELP As Double
        Get
            Return _EBELP_EBELP
        End Get
        Set(value As Double)
            _EBELP_EBELP = value
        End Set
    End Property

    'Nota para a qualidade do serviço
    Private _PWWE_MC_PWWE As Double
    Public Property PWWE_MC_PWWE As Double
        Get
            Return _PWWE_MC_PWWE
        End Get
        Set(value As Double)
            _PWWE_MC_PWWE = value
        End Set
    End Property

    'Nota para o cumprimento de prazos
    Private _PWFR_MC_PWFR As Double
    Public Property PWFR_MC_PWFR As Double
        Get
            Return _PWFR_MC_PWFR
        End Get
        Set(value As Double)
            _PWFR_MC_PWFR = value
        End Set
    End Property

    'Nº documento de referência
    Private _XBLNR_XBLNR_SRV1 As String
    Public Property XBLNR_XBLNR_SRV1 As String
        Get
            Return _XBLNR_XBLNR_SRV1
        End Get
        Set(value As String)
            _XBLNR_XBLNR_SRV1 = value
        End Set
    End Property

    'Texto de cabeçalho de documento
    Private _BKTXT_BKTXT_SRV As String
    Public Property BKTXT_BKTXT_SRV As String
        Get
            Return _BKTXT_BKTXT_SRV
        End Get
        Set(value As String)
            _BKTXT_BKTXT_SRV = value
        End Set
    End Property

    'Categoria de classificação contábil
    Private _KNTTP_KNTTP As String
    Public Property KNTTP_KNTTP As String
        Get
            Return _KNTTP_KNTTP
        End Get
        Set(value As String)
            _KNTTP_KNTTP = value
        End Set
    End Property

    'lista de itens
    Private _lisItem As New List(Of AppSAPFR_Item)
    Public Property ListItem As List(Of AppSAPFR_Item)
        Get
            Return _lisItem
        End Get
        Set(value As List(Of AppSAPFR_Item))
            _lisItem = value
        End Set
    End Property




End Class
