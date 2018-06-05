Imports Microsoft.VisualBasic

Public Class AppContrato

    Private _codigo As String
    Private _num_cont_client As String
    Private _num_cont_op As String
    Private _tipo_contrato As String
    Private _data_contratacao As String
    Private _numero_oc As String
    Private _valor_instalacao As String
    Private _valor_mensal As String
    Private _prazo_inst As String
    Private _data_ativ As String
    Private _data_desat As String
    Private _multa_recisao As String
    Private _num_fatura As String
    Private _end_envio_fat As String
    Private _data_pgto_fat As String
    Private _responsavel As String
    Private _periodo As String

    Public Sub New()

    End Sub

    Public Property periodo As String
        Get
            Return _periodo
        End Get
        Set(ByVal value As String)
            _periodo = value
        End Set
    End Property

    Public Property responsavel As String
        Get
            Return _responsavel
        End Get
        Set(ByVal value As String)
            _responsavel = value
        End Set
    End Property

    Public Property data_pgto_fat As String
        Get
            Return _data_pgto_fat
        End Get
        Set(ByVal value As String)
            _data_pgto_fat = value
        End Set
    End Property

    Public Property end_envio_fat As String
        Get
            Return _end_envio_fat
        End Get
        Set(ByVal value As String)
            _end_envio_fat = value
        End Set
    End Property

    Public Property num_fatura As String
        Get
            Return _num_fatura
        End Get
        Set(ByVal value As String)
            _num_fatura = value
        End Set
    End Property


    Public Property multa_recisao As String
        Get
            Return _multa_recisao
        End Get
        Set(ByVal value As String)
            _multa_recisao = value
        End Set
    End Property

    Public Property data_desat As String
        Get
            Return _data_desat
        End Get
        Set(ByVal value As String)
            _data_desat = value
        End Set
    End Property

    Public Property data_ativ As String
        Get
            Return _data_ativ
        End Get
        Set(ByVal value As String)
            _data_ativ = value
        End Set
    End Property

    Public Property prazo_inst As String
        Get
            Return _prazo_inst
        End Get
        Set(ByVal value As String)
            _prazo_inst = value
        End Set
    End Property

    Public Property valor_mensal As String
        Get
            Return _valor_mensal
        End Get
        Set(ByVal value As String)
            _valor_mensal = value
        End Set
    End Property

    Public Property valor_instalacao As String
        Get
            Return _valor_instalacao
        End Get
        Set(ByVal value As String)
            _valor_instalacao = value
        End Set
    End Property


    Public Property numero_oc As String
        Get
            Return _numero_oc
        End Get
        Set(ByVal value As String)
            _numero_oc = value
        End Set
    End Property

    Public Property data_contratacao As String
        Get
            Return _data_contratacao
        End Get
        Set(ByVal value As String)
            _data_contratacao = value
        End Set
    End Property

    Public Property codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property num_cont_client As String
        Get
            Return _num_cont_client
        End Get
        Set(ByVal value As String)
            _num_cont_client = value
        End Set
    End Property

    Public Property num_cont_op As String
        Get
            Return _num_cont_op
        End Get
        Set(ByVal value As String)
            _num_cont_op = value
        End Set
    End Property

    Public Property tipo_contrato As String
        Get
            Return _tipo_contrato
        End Get
        Set(ByVal value As String)
            _tipo_contrato = value
        End Set
    End Property

End Class
