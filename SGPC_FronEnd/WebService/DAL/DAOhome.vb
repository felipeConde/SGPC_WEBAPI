Imports System.Data


Public Class DAOhome
    Private _DAO_commons As New DAO_COMMONS
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Dim session As HttpSessionState = HttpContext.Current.Session

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property


    Public GraficoData As String

    Public GraficoData2 As String

    Public GraficoData3 As String

    Public GraficoData4 As String

    Public GraficoData5 As String

    Public dataValues As String

    'link de dadoss
    Public GraficoData6 As String

    Public GraficoLabel As String
    Private ExibeMovel As Boolean = True
    Private ExibeFixo As Boolean = True
    Private Exibe0800 As Boolean
    Private Exibe3003 As Boolean
    Private exibeDados As Boolean
    Private ExibeServico As Boolean





End Class
