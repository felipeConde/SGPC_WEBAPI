Imports Microsoft.VisualBasic

Public Class AppTipoSolicitacao

   Private _codigo As Integer
   Private _solicitacao As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal psolicitacao As String)
      _codigo = pcodigo
      _solicitacao = psolicitacao
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property Solicitacao() As String
      Get
         Return _solicitacao
      End Get
      Set(ByVal value As String)
         _solicitacao = value
      End Set
   End Property

End Class
