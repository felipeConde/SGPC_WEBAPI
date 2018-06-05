Imports Microsoft.VisualBasic
Imports System.Data

Public Class AppIni
    Private Shared _dao As New DAO_Commons

    Public Sub New()

    End Sub


    Public Shared Sub inicialize()
        GetGoogleCode()
    End Sub

    Private Shared Sub GetGoogleCode()
        _dao.strConn = System.Web.HttpContext.Current.Session("conexao").ToString


        Dim googleKey As String = ""
        Dim strScript As String = ""
        Dim dt As DataTable = _dao.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='GOOGLEKEY'")
        If dt.Rows.Count > 0 Then
            googleKey = dt.Rows(0).Item(0).ToString

            strScript += " <script>" & vbNewLine
            'strScript += " //google analytics" & vbNewLine
            strScript += " (function (i, s, o, g, r, a, m) {" & vbNewLine
            strScript += "i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {" & vbNewLine
            strScript += "(i[r].q = i[r].q || []).push(arguments)" & vbNewLine
            strScript += "}, i[r].l = 1 * new Date(); a = s.createElement(o)," & vbNewLine
            strScript += " m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)" & vbNewLine
            strScript += "})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');" & vbNewLine

            strScript += "ga('create', '" & googleKey & "', 'clconsult.com.br');" & vbNewLine
            strScript += "ga('send', 'pageview');" & vbNewLine

            strScript += "</script>" & vbNewLine

            System.Web.HttpContext.Current.Response.Write(strScript)
            'System.Web.HttpContext.Current..ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScriptGoogle", Script, False)
            'System.Web.UI.Page.

        End If
        'Return strScript
    End Sub

#Region "Parametros"

    'parametro utilizado no RIT para colocar o grafico classificado pelo Tipo da Linha
    Public Shared GraficoTipoaparelho As Boolean = False
    Public Shared EscodeBotaoTipoaparelho As Boolean = False

    Public Shared Sulamerica_Param As Boolean = False

    'parametros do relatório análise de consumo

    Public Shared Aes_Param As Boolean = False
    Public Shared GloboRJ_Parm As Boolean = False

    Public Shared exibe_franquia As Boolean = True
    Public Shared exibe_auditado As Boolean = True
    Public Shared exibe_rateio As Boolean = True

    '******************** CADASTRO DE CELULAR *******************************************

    Public Shared CCusto_Editable As Boolean = True
    Public Shared Vonpar_Param As Boolean = False
    Public Shared Ageradora_Param As Boolean = True

    '******************** EXTRATO DE FATURAS *******************************************
    Public Shared ExibeSoTotalExtrato As Boolean = False


#End Region



End Class
