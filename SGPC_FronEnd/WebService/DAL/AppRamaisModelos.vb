Imports Microsoft.VisualBasic

Public Class AppRamaisModelos

   Private _codigo_modelo As Integer
   Private _modelo As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo_modelo As Integer, ByVal pmodelo As String)
      _codigo_modelo = pcodigo_modelo
      _modelo = pmodelo
   End Sub

   Public Property Codigo_Modelo() As Integer
      Get
         Return _codigo_modelo
      End Get
      Set(ByVal value As Integer)
         _codigo_modelo = value
      End Set
   End Property

   Public Property Modelo() As String
      Get
         Return _modelo
      End Get
      Set(ByVal value As String)
         _modelo = value
      End Set
   End Property

End Class
