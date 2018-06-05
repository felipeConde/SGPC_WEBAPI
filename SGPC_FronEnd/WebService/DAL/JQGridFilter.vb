Imports Microsoft.VisualBasic


Public Class JQGridFilter
    Public groupOp As String
    Public rules As New JQGridFilterRules
End Class

Public Class JQGridFilterRules
    Public field As String
    Public op As String
    Public data As String
End Class
