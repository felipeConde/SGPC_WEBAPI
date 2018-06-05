Imports Microsoft.VisualBasic

Public Class AppSAPFR_Linha


    Private _ClasseContab As New List(Of AppSAPFR_Item)
    Public Property Itens As List(Of AppSAPFR_Item)
        Get
            Return _ClasseContab
        End Get
        Set(value As List(Of AppSAPFR_Item))
            _ClasseContab = value
        End Set
    End Property



End Class
