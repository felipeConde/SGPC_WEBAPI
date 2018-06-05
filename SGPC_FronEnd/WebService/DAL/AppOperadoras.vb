Imports Microsoft.VisualBasic

Public Class AppOperadoras

   Private _codigo As Integer
   Private _descricao As String
   Private _default_op As String
   Private _multimedicao As String
   Private _nome_fantasia As String
   Private _dt_vencimento As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Sub New(ByVal pdt_vencimento As String)
      _dt_vencimento = pdt_vencimento
   End Sub
   Public Sub New(ByVal pcodigo As Integer, ByVal pnome_fantasia As String)
      _codigo = pcodigo
      _nome_fantasia = pnome_fantasia
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pdescricao As String, ByVal pdefault_op As String, ByVal pmultimedicao As String)
      _codigo = pcodigo
      _descricao = pdescricao
      _default_op = pdefault_op
      _multimedicao = pmultimedicao
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
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

   Public Property Default_Op() As String
      Get
         Return _default_op
      End Get
      Set(ByVal value As String)
         _default_op = value
      End Set
   End Property

   Public Property MultiMedicao() As String
      Get
         Return _multimedicao
      End Get
      Set(ByVal value As String)
         _multimedicao = value
      End Set
   End Property

   Public Property Nome_Fantasia() As String
      Get
         Return _nome_fantasia
      End Get
      Set(ByVal value As String)
         _nome_fantasia = value
      End Set
   End Property

   Public Property Dt_Vencimento() As String
      Get
         Return _dt_vencimento
      End Get
      Set(ByVal value As String)
         _dt_vencimento = value
      End Set
   End Property

End Class
