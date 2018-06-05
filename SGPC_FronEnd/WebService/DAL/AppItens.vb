Imports Microsoft.VisualBasic

Public Class AppItens

   Private _codigo As Integer
   Private _item_sgpc As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pitem_sgpc As String)
      _codigo = pcodigo
      _item_sgpc = pitem_sgpc
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property ItemSgpc() As String
      Get
         Return _item_sgpc
      End Get
      Set(ByVal value As String)
         _item_sgpc = value
      End Set
   End Property

   Public Property Item_Sgpc() As String
      Get
         Return _item_sgpc
      End Get
      Set(ByVal value As String)
      End Set
   End Property

End Class
