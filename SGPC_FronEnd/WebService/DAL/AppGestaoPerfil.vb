Imports Microsoft.VisualBasic

Public Class AppGestaoPerfil

    Private _codigo As Integer
    Private _tipo_franquia As String
    Private _gestao_perfil As String
    Private _minutos As Double
    Private _ddd As String  '(N)ão / (S)im
    Private _sms As String  '(N)ão / (S)im
    Private _acobrar As String  '(N)ão / (S)im
    Private _pct_dados As String  '(N)ão / (S)im
    Private _min_ddd As Double
    Private _min_local As Double
    Private _vlr_ddd As Double
    Private _vlr_pct_LD As Double
    Private _vlr_local As Double
    Private _vlr_sms As Double
    Private _vlr_assinatura As Double
    Private _vlr_g_web As Double
    Private _vlr_t_zero As Double
    Private _vlr_pct_black As Double
    Private _vlr_lim_total As Double
    Private _vlr_roaming As Double

    Private _min_roaming As Double
    Private _qtd_sms As Double
    Private _mb_dados As Double

   Public Sub New()
   End Sub

   Public Sub New(ByVal pgestao_perfil As String)
      _gestao_perfil = pgestao_perfil
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pgestao_perfil As String)
      _codigo = pcodigo
      _gestao_perfil = pgestao_perfil
   End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pgestao_perfil As String, ByVal pminutos As Double, ByVal pddd As String, ByVal psms As String, ByVal pacobrar As String, ByVal ppct_dados As String, ByVal pmin_ddd As Double, ByVal pmin_local As Double, ByVal pvlr_ddd As Double, ByVal pvlr_pct_LD As Double, ByVal pvlr_local As Double, ByVal pvlr_sms As Double, ByVal pvlr_assinatura As Double, ByVal pvlr_g_web As Double, ByVal pvlr_t_zero As Double, ByVal pvlr_pct_black As Double, ByVal pvlr_lim_total As Double)
        _codigo = pcodigo
        _gestao_perfil = pgestao_perfil
        _minutos = pminutos
        _ddd = pddd
        _sms = psms
        _acobrar = pacobrar
        _pct_dados = ppct_dados
        _min_ddd = pmin_ddd
        _min_local = pmin_local
        _vlr_ddd = pvlr_ddd
        _vlr_pct_LD = pvlr_pct_LD
        _vlr_local = pvlr_local
        _vlr_sms = pvlr_sms
        _vlr_assinatura = pvlr_assinatura
        _vlr_g_web = pvlr_g_web
        _vlr_t_zero = pvlr_t_zero
        _vlr_pct_black = pvlr_pct_black
        _vlr_lim_total = pvlr_lim_total
    End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property Gestao_Perfil() As String
      Get
         Return _gestao_perfil
      End Get
      Set(ByVal value As String)
         _gestao_perfil = value
      End Set
   End Property

   Public Property Minutos() As Double
      Get
         Return _minutos
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _minutos = value
      End Set
   End Property

   Public Property DDD() As String
      Get
         Return _ddd
      End Get
      Set(ByVal value As String)
         _ddd = value
      End Set
   End Property

   Public Property SMS() As String
      Get
         Return _sms
      End Get
      Set(ByVal value As String)
         _sms = value
      End Set
   End Property

   Public Property ACobrar() As String
      Get
         Return _acobrar
      End Get
      Set(ByVal value As String)
         _acobrar = value
      End Set
   End Property

   Public Property Pct_Dados() As String
      Get
         Return _pct_dados
      End Get
      Set(ByVal value As String)
         _pct_dados = value
      End Set
   End Property

   Public Property Min_DDD() As Double
      Get
         Return _min_ddd
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _min_ddd = value
      End Set
   End Property

   Public Property Min_Local() As Double
      Get
         Return _min_local
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _min_local = value
      End Set
   End Property

   Public Property Vlr_DDD() As Double
      Get
         Return _vlr_ddd
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_ddd = value
      End Set
   End Property

    Public Property Vlr_pct_LD() As Double
        Get
            Return _vlr_pct_LD
        End Get
        Set(ByVal value As Double)
            If Double.IsNaN(value) Then
                value = 0
            End If
            _vlr_pct_LD = value
        End Set
    End Property

   Public Property Vlr_Local() As Double
      Get
         Return _vlr_local
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_local = value
      End Set
   End Property

   Public Property Vlr_SMS() As Double
      Get
         Return _vlr_sms
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_sms = value
      End Set
   End Property

   Public Property Vlr_Assinatura() As Double
      Get
         Return _vlr_assinatura
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_assinatura = value
      End Set
   End Property

   Public Property Vlr_G_Web() As Double
      Get
         Return _vlr_g_web
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_g_web = value
      End Set
   End Property

   Public Property Vlr_T_Zero() As Double
      Get
         Return _vlr_t_zero
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_t_zero = value
      End Set
   End Property

   Public Property Vlr_Pct_Black() As Double
      Get
         Return _vlr_pct_black
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_pct_black = value
      End Set
   End Property

   Public Property Vlr_Lim_Total() As Double
      Get
         Return _vlr_lim_total
      End Get
      Set(ByVal value As Double)
         If Double.IsNaN(value) Then
            value = 0
         End If
         _vlr_lim_total = value
      End Set
    End Property

    Public Property Min_roaming() As Double
        Get
            Return _min_roaming
        End Get
        Set(value As Double)
            _min_roaming = value
        End Set
    End Property

    Public Property Qtd_sms() As Double
        Get
            Return _qtd_sms
        End Get
        Set(value As Double)
            _qtd_sms = value
        End Set
    End Property

    Public Property Mb_dados() As Double
        Get
            Return _mb_dados
        End Get
        Set(value As Double)
            _mb_dados = value
        End Set
    End Property

    Public Property Vlr_Roaming() As Double
        Get
            Return _vlr_roaming
        End Get
        Set(value As Double)
            _vlr_roaming = value
        End Set
    End Property

    Public Property Tipo_Franquia() As String
        Get
            Return _tipo_franquia
        End Get
        Set(value As String)
            _tipo_franquia = value
        End Set
    End Property


End Class
