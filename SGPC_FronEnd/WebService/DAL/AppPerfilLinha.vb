Imports Microsoft.VisualBasic

Public Class AppPerfilLinha

   Private _codigo_gestao_perfil As Integer
   Private _codigo_linha As Integer
   Private _fim_ciclo As String

   Private _gestao_perfil As String
   Private _num_linha As String
   Private _codigo_operadora As Integer
   Private _operadora As String
   Private _usuario As String

   Private _codigo As Integer  'tabela GESTAO_PERFIL

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo_gestao_perfil As Integer, ByVal pgestao_perfil As String)
      _codigo_gestao_perfil = pcodigo_gestao_perfil
      _gestao_perfil = pgestao_perfil
   End Sub

   Public Sub New(ByVal pcodigo_gestao_perfil As Integer, ByVal pcodigo_linha As Integer)
      _codigo_gestao_perfil = pcodigo_gestao_perfil
      _codigo_linha = pcodigo_linha
   End Sub

   Public Sub New(ByVal pcodigo_gestao_perfil As Integer, ByVal pcodigo_linha As Integer, ByVal pfim_ciclo As String)
      _codigo_gestao_perfil = pcodigo_gestao_perfil
      _codigo_linha = pcodigo_linha
      _fim_ciclo = pfim_ciclo
   End Sub

   Public Sub New(ByVal pcodigo_gestao_perfil As Integer, ByVal pcodigo_linha As Integer, ByVal pfim_ciclo As String, ByVal pgestao_perfil As String, ByVal pnum_linha As String, ByVal pcodigo_operadora As Integer, ByVal poperadora As String, ByVal pusuario As String)
      _codigo_gestao_perfil = pcodigo_gestao_perfil
      _codigo_linha = pcodigo_linha
      _fim_ciclo = pfim_ciclo

      _gestao_perfil = pgestao_perfil
      _num_linha = pnum_linha
      _codigo_operadora = pcodigo_operadora
      _operadora = poperadora
      _usuario = pusuario
   End Sub

   Public Property Codigo_GestaoPerfil() As Integer
      Get
         Return _codigo_gestao_perfil
      End Get
      Set(ByVal value As Integer)
         _codigo_gestao_perfil = value
      End Set
   End Property

   Public Property Codigo_Linha() As Integer
      Get
         Return _codigo_linha
      End Get
      Set(ByVal value As Integer)
         _codigo_linha = value
      End Set
   End Property

   Public Property Fim_Ciclo() As String
      Get
         Return _fim_ciclo
      End Get
      Set(ByVal value As String)
         _fim_ciclo = value
      End Set
   End Property

   Public Property Gestao_Perfil() As String
      Get
         Return _gestao_perfil
      End Get
      Set(ByVal value As String)
      End Set
   End Property

   Public Property Num_Linha() As String
      Get
         Return _num_linha
      End Get
      Set(ByVal value As String)
      End Set
   End Property

   Public Property Codigo_Operadora() As Integer
      Get
         Return _codigo_operadora
      End Get
      Set(ByVal value As Integer)
      End Set
   End Property

   Public Property Operadora() As String
      Get
         Return _operadora
      End Get
      Set(ByVal value As String)
      End Set
   End Property

   Public Property Usuario() As String
      Get
         Return _usuario
      End Get
      Set(ByVal value As String)
      End Set
   End Property

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
      End Set
   End Property

End Class
