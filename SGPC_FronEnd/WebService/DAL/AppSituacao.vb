Imports Microsoft.VisualBasic

Public Class AppSituacao

   Private _codigo As Integer
   Private _situacao As String
   Private _descricao As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal psituacao As String, ByVal pdescricao As String)
      _codigo = pcodigo
      _situacao = psituacao
      _descricao = pdescricao
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property Situacao() As String
      Get
         Return _situacao
      End Get
      Set(ByVal value As String)
         _situacao = value
      End Set
   End Property

   Public Property Descricao() As String
      Get
         Return _descricao
      End Get
      Set(ByVal value As String)
         _descricao = value
      End Set
   End Property

End Class
