Imports Microsoft.VisualBasic

Public Class AppCidades
    
Private _codigo_cidade As Integer
   Private _cidade As String
   Private _uf As String


   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo_cidade As Integer, ByVal pcidade As String, ByVal puf As String)
      _codigo_cidade = pcodigo_cidade
      _cidade = pcidade
      _uf = puf
   End Sub

   Public Property Codigo_cidade() As Integer
      Get
         Return _codigo_cidade
      End Get
      Set(ByVal value As Integer)
         _codigo_cidade = value
      End Set
   End Property

   Public Property cidade() As String
      Get
         Return _cidade
      End Get
      Set(ByVal value As String)
         _cidade = value
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

End Class
