Imports System.Net
Imports System.Web.Http
Imports System.Web.HttpContext
Imports System.Web.SessionState

Namespace WebService
    Public Class HomeController
        Inherits ApiController
        Implements System.Web.SessionState.IRequiresSessionState


        Dim _dao As New DAOfaturas
        Dim _daoHome As New DAOhome
        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()



        ' GET: api/Home
        Public Function GetValues() As IEnumerable(Of String)
            'Dim totalFaturado As String = _dao.GetValorFaturado
            'Dim totalPago As String = _dao.GetValorPago
            'Dim totalContestado As String = _dao.GetValorContestado
            'Dim economiaPercet As Integer = (totalPago / totalFaturado) * 100
            'Dim contestadoPercet As Integer = (totalContestado / totalFaturado) * 100

            Return New String() {""}

            'Return New String() {FormatCurrency(totalFaturado), FormatCurrency(totalPago), economiaPercet, FormatCurrency(totalContestado), contestadoPercet}
        End Function

        ' GET: api/Home/5
        Public Function GetValue(ByVal id As Integer) As String
            Return "value"
        End Function

        ' POST: api/Home
        'Public Function PostValue(<FromBody()> usuario As appUsuario) As IEnumerable(Of String)
        Public Function PostValue(<FromBody()> usuario As appUsuario) As Object

            'Dim session As HttpSessionState = HttpContext.Current.Session
            ''session("codigousuario") = usuario.codigo
            ''Dim codigo As String = usuario.codigo
            'Dim codigo As String = session("codigousuario")

            'Dim Carrega_Grafico = _daoHome.Carrega_Grafico("", "", "", "")
            'Dim categorias As IEnumerable(Of String) = _daoHome.GraficoLabel.Replace(" ", "").Replace("'", "").Split(",")
            'Dim values As String = _daoHome.dataValues
            'Dim _grafico As New appGrafico
            '_grafico.Categorias = serializer.Serialize(categorias)
            '_grafico.Dados = values
            'Return _grafico

            Return Nothing

        End Function

        ' PUT: api/Home/5
        Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        End Sub

        ' DELETE: api/Home/5
        Public Sub DeleteValue(ByVal id As Integer)

        End Sub
    End Class
End Namespace