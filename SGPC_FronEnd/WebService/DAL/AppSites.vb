Imports Microsoft.VisualBasic

Public Class AppSites

   Private _codigo_site As Integer
   Private _site As String
   Private _codigo_localidade As Integer
   Private _sigla As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo_site As Integer, ByVal psite As String, ByVal psigla As String)
      _codigo_site = pcodigo_site
      _site = psite
      _sigla = psigla
   End Sub

   Public Sub New(ByVal pcodigo_site As Integer, ByVal psite As String, ByVal psigla As String, ByVal pcodigo_localidade As Integer)
      _codigo_site = pcodigo_site
      _site = psite
      _sigla = psigla
      _codigo_localidade = pcodigo_localidade
   End Sub

   Public Property Codigo_Site() As Integer
      Get
         Return _codigo_site
      End Get
      Set(ByVal value As Integer)
         _codigo_site = value
      End Set
   End Property

   Public Property Site() As String
      Get
         Return _site
      End Get
      Set(ByVal value As String)
         _site = value
      End Set
   End Property

   Public Property Sigla() As String
      Get
         Return _sigla
      End Get
      Set(ByVal value As String)
         _sigla = value
      End Set
   End Property

   Public Property Codigo_Localidade() As Integer
      Get
         Return _codigo_localidade
      End Get
      Set(ByVal value As Integer)
         _codigo_localidade = value
      End Set
   End Property

End Class
