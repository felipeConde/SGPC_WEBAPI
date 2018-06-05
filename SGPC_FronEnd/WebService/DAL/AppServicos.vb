Imports Microsoft.VisualBasic

Public Class AppServicos

   Private _codigo As Integer
   Private _servico As String
   Private _tipo_solicitacao_codigo As Integer
   Private _solicitacao As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pservico As String)
      _codigo = pcodigo
      _servico = pservico
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pservico As String, ByVal ptipo_solicitacao As Integer)
      _codigo = pcodigo
      _servico = pservico
      _tipo_solicitacao_codigo = ptipo_solicitacao
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property Servico() As String
      Get
         Return _servico
      End Get
      Set(ByVal value As String)
         _servico = value
      End Set
   End Property

   Public Property TipoSolicitacao() As Integer
      Get
         Return _tipo_solicitacao_codigo
      End Get
      Set(ByVal value As Integer)
         _tipo_solicitacao_codigo = value
      End Set
   End Property

   Public Property Solicitacao() As String
      Get
         Return _solicitacao
      End Get
      Set(ByVal value As String)
      End Set
   End Property

End Class
