Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports System.Web.UI
Imports System.Drawing
Imports SGPC_FronEnd


Partial Class home
    Inherits System.Web.UI.Page
    Public _dao_commons As New DAO_COMMONS

    Public Property GraficoData() As String
        Get
            Return ViewState("graficoData")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData") = value
        End Set
    End Property
    Public Property GraficoData2() As String
        Get
            Return ViewState("graficoData2")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData2") = value
        End Set
    End Property
    Public Property GraficoData3() As String
        Get
            Return ViewState("graficoData3")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData3") = value
        End Set
    End Property
    Public Property GraficoData4() As String
        Get
            Return ViewState("graficoData4")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData4") = value
        End Set
    End Property
    Public Property GraficoData5() As String
        Get
            Return ViewState("graficoData5")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData5") = value
        End Set
    End Property
    'link de dados
    Public Property GraficoData6() As String
        Get
            Return ViewState("graficoData6")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData6") = value
        End Set
    End Property
    Public Property GraficoLabel() As String
        Get
            Return ViewState("graficoLabel")
        End Get
        Set(ByVal value As String)
            ViewState("graficoLabel") = value
        End Set
    End Property

    Public strGrafico As String = ""
    Public strSQL As String = ""
    Public Meta As Double
    Public ExibeMovel As Boolean = True
    Public ExibeFixo As Boolean = False
    Public Exibe0800 As Boolean = False
    Public Exibe3003 As Boolean = False
    Public ExibeServico As Boolean = False
    Public exibeDados As Boolean = False
    Public _excluirServico As String = ""




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim grupo As String = ""
        Dim area As String = ""
        Dim area_interna As String = ""
        Dim hierarquia As String = ""
        Dim tipoValor As String = 1

        'Response.Cache.SetCacheability(HttpCacheability.Public)
        'Response.Cache.SetMaxAge(New TimeSpan(24, 0, 0))
        GraficoData = ""
        GraficoData2 = ""
        GraficoLabel = ""

        ' Response.Write("entrou")

        'Exit Sub

        If Not Page.IsPostBack Then

            _dao_commons.strConn = Session("conexao")
            'MontaGraficoTotalOperadora()

            'pega o C.Custo
            grupo = Request.QueryString("grupo")
            area = Request.QueryString("area")
            area_interna = Request.QueryString("area_interna")
            hierarquia = Request.QueryString("hierarquia")
            'tipoValor = Request.QueryString("tipoValor")
            tipoValor = Session("tipoValor")

            'If Not DALCGestor.AcessoAdmin() Or grupo <> "" Then
            '    'exibe só o valor faturado
            '    Me.lstRbTipoValor.Items.RemoveAt(1)
            '    Me.lstRbTipoValor.SelectedValue = 1
            '    Session("tipoValor") = 1
            '    tipoValor = Session("tipoValor")
            '    Me.lstRbTipoValor.Visible = False
            'End If


            ''If (tipoValor = 2 Or tipoValor = "") And DALCGestor.AcessoAdmin() Then
            'If (tipoValor = 2) And DALCGestor.AcessoAdmin() Then
            '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "DEVIDO", area, area_interna)
            '    'Carrega_Grafico(grupo, hierarquia, "Exibe-Ramal", "DEVIDO")
            '    Me.lstRbTipoValor.SelectedValue = 2
            'Else
            '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna)
            '    Me.lstRbTipoValor.SelectedValue = 1
            'End If

            Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "DEVIDO", area, area_interna)


        End If
        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "MontaGraficoGeral();", True)

    End Sub

    Sub MontaGraficoTotalOperadora()
        Dim dt As DataTable = _dao_commons.myDataTable(getQueryTotalOperadora())
        Dim dtTipos As DataTable = _dao_commons.myDataTable(getQueryTotalCustoOperadora())

    End Sub

    Protected Sub Carrega_Grafico(ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "")
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim strTipovalor As String = "sum(nvl(p1.valor_cdr,0))gasto"
        If tipoValor.ToUpper = "DEVIDO" Then
            'strTipovalor = "sum(p1.valor_cdr)-nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) gasto"
            'strTipovalor = "sum(p1.valor_cdr) -(((select nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) from dual ))) gasto"
            strTipovalor = "sum(p1.valor_cdr)-nvl(p4.valor_contestado,0) gasto"

        End If

        'monta query
        strSQL = "select  tipo, sum(gasto)gasto, data, codigo_tipo from" & vbNewLine
        strSQL += "(select 'MÓVEL' tipo, " & strTipovalor & " ,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=1 " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then

            If hierarquia = 1 Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            Else
                strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
            End If

        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If

        'strSQL += " union all " & vbNewLine
        'strSQL += "select 'fixo' tipo, sum(p1.valor_cdr)gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo=2 " & vbNewLine
        'strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(_excluirServico) Then
        '    strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
        If Cliente = "Exibe-Ramal" Then
            strSQL += " union all " & vbNewLine
            strSQL += " select 'ramal' tipo, sum(p1.valor_cdr)gasto ,to_char(p1.data_inicio, 'MM/YYYY')data,2 codigo_tipo "
            strSQL += " from cdrs p1, grupos g "
            strSQL += " and g.codigo(+) = p1.grp_codigo "
            strSQL += " where to_date(to_char(p1.data_inicio, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) "
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p1.data_inicio, 'MM/YYYY') "
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If

        Else
            strSQL += " union all " & vbNewLine
            strSQL += "select 'FIXO' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
            strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
            strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
            strSQL += " and p3.codigo_tipo=2 " & vbNewLine
            strSQL += " and g.codigo(+) = p1.grp_codigo "
            strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
            strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
            'filtra o c.custo
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If

            If Not String.IsNullOrEmpty(_excluirServico) Then
                strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If
        End If

        strSQL += " union all " & vbNewLine
        strSQL += "select '0800' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=4 " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If

        strSQL += " union all " & vbNewLine
        strSQL += "select 'Número Único' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=6 " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If
        'LINK de Dados - ENtra como Serviço

        strSQL += " union all " & vbNewLine
        strSQL += "select 'LINK DE DADOS' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo in(5) " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If

        'SERVIÇOS
        strSQL += " union all " & vbNewLine
        strSQL += "select 'SERVIÇOS' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo in(3) " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If


        'faturas manuais
        If DALCGestor.AcessoAdmin() Then
            strSQL += " union all " & vbNewLine
            strSQL += " select UPPER(ft.tipo) tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
            strSQL += " from faturas p1, fornecedores p2,  faturas_tipo ft " & vbNewLine
            strSQL += " where p1.codigo_tipo=ft.codigo_tipo and not exists(select 0 from faturas_arquivos where codigo_fatura=p1.codigo_fatura) " & vbNewLine
            strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
            strSQL += " and to_date(to_char(p1.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12)"
            strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo,ft.tipo " & vbNewLine
        End If
        strSQL += " ) group by tipo, data, codigo_tipo " & vbNewLine
        strSQL += " order by to_date(data, 'MM/YYYY') asc,codigo_tipo " & vbNewLine

        'Response.Write(strSQL)
        'Response.End()

        Me.SqlDataSourceResumoGeral.SelectCommand = strSQL
        Me.SqlDataSourceResumoGeral.ConnectionString = Session("conexao")
        Me.SqlDataSourceResumoGeral.DataBind()
        Meta = DALCGestor.GetMetaByCcusto("")
    End Sub

    Protected Sub Carrega_Grafico_old(ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String)
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim strTipovalor As String = "sum(p1.valor_cdr)gasto"
        If tipoValor.ToUpper = "DEVIDO" Then
            'strTipovalor = "sum(p1.valor_cdr)-nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) gasto"
            strTipovalor = "sum(p1.valor_cdr) -(((select nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) from dual ))) gasto"

        End If


        'monta query
        strSQL = "select * from" & vbNewLine
        strSQL += "(select 'movel' tipo, " & strTipovalor & " ,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=1 " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then

            If hierarquia = 1 Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            Else
                strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
            End If

        End If

        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine


        'strSQL += " union all " & vbNewLine
        'strSQL += "select 'fixo' tipo, sum(p1.valor_cdr)gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo=2 " & vbNewLine
        'strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(_excluirServico) Then
        '    strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
        If Cliente = "Exibe-Ramal" Then
            strSQL += " union all " & vbNewLine
            strSQL += " select 'ramal' tipo, sum(p1.valor_cdr)gasto ,to_char(p1.data_inicio, 'MM/YYYY')data,2 codigo_tipo "
            strSQL += " from cdrs p1 "
            strSQL += " where to_date(to_char(p1.data_inicio, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) "
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p1.data_inicio, 'MM/YYYY') "


        Else
            strSQL += " union all " & vbNewLine
            strSQL += "select 'fixo' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
            strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
            strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
            strSQL += " and p3.codigo_tipo=2 " & vbNewLine
            strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
            'filtra o c.custo
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(_excluirServico) Then
                strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
            'strSQL += " where rownum<=13 " & vbNewLine
        End If

        strSQL += " union all " & vbNewLine
        strSQL += "select '0800' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=4 " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine

        strSQL += " union all " & vbNewLine
        strSQL += "select 'Número Único' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=6 " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine

        'se for administrador geral pega as faturas de serviços
        If DALCGestor.AcessoAdmin() Then
            strSQL += " union all " & vbNewLine
            strSQL += " select 'SERVIÇOS' tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
            strSQL += " from faturas p1, fornecedores p2 " & vbNewLine
            strSQL += " where p1.codigo_tipo='3' " & vbNewLine
            strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
            strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo " & vbNewLine
        End If

        'LINK de Dados - ENtra como Serviço

        strSQL += " union all " & vbNewLine
        strSQL += "select 'SERVIÇOS' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4 " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo=5 " & vbNewLine
        strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine

        'faturas manuais
        If DALCGestor.AcessoAdmin() Then
            strSQL += " union all " & vbNewLine
            strSQL += " select UPPER(ft.tipo) tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
            strSQL += " from faturas p1, fornecedores p2,  faturas_tipo ft " & vbNewLine
            strSQL += " where p1.codigo_tipo=ft.codigo_tipo and not exists(select 0 from faturas_arquivos where codigo_fatura=p1.codigo_fatura) " & vbNewLine
            strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
            strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo,ft.tipo " & vbNewLine
        End If

        strSQL += " ) " & vbNewLine
        strSQL += " order by to_date(data, 'MM/YYYY') asc,codigo_tipo " & vbNewLine

        'Response.Write(strSQL)
        'Response.End()

        Me.SqlDataSourceResumoGeral.SelectCommand = strSQL
        Me.SqlDataSourceResumoGeral.ConnectionString = Fabrica.conexao.ToString
        Me.SqlDataSourceResumoGeral.DataBind()
        Meta = DALCGestor.GetMetaByCcusto("")
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "MontaGrafico()", True)
        'strGrafico = RetornaGrafico()
        'Exit Sub
        InverteGridView(Me.gvResumoMensal)

    End Sub


    Protected Sub gvResumoMensal_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResumoMensal.Sorted
        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "MontaGrafico()", False)
        'Response.Write("teste")
    End Sub

    Public Function RetornaGrafico() As String

        Dim strGrafico As String = "<script>" & vbNewLine
        strGrafico += "var api = new jGCharts.Api(); " & vbNewLine
        strGrafico += "jQuery('<img>') " & vbNewLine
        strGrafico += ".attr('src', api.make({ " & vbNewLine
        strGrafico += "//data : [[500.7,300],[400,300],[250,300]], //MANDATORY " & vbNewLine
        strGrafico += "data : [" & GraficoData & "], //MANDATORY " & vbNewLine
        strGrafico += "type:   'bvh'," & vbNewLine
        strGrafico += "size:   '800x300'," & vbNewLine
        strGrafico += "//axis_labels : ['01-03','02-03','03-03']," & vbNewLine
        strGrafico += "axis_labels : [" & GraficoLabel & "]," & vbNewLine
        strGrafico += "legend: ['Gasto','Duração(HS)','Chamadas']," & vbNewLine
        strGrafico += "bar_width : 15, //default 20 " & vbNewLine
        strGrafico += "})) " & vbNewLine
        strGrafico += ".appendTo('#graficoMensal');" & vbNewLine
        strGrafico += "</script>" & vbNewLine

        Return strGrafico
    End Function



    Public Sub InverteGridView(ByVal mygrid As GridView)

        Dim dt As New DataTable
        Dim i As Integer = 1

        'totais
        'Dim total(12) As Double


        'cria as colunas de acordo com as linhas
        dt.Columns.Add(" ")

        For Each linha As GridViewRow In mygrid.Rows
            Try
                Dim nomeColuna As String = linha.Cells(0).Text

                dt.Columns.Add(nomeColuna)
                dt.Columns(nomeColuna).DefaultValue = "R$ 0,00"


            Catch ex As Exception
            End Try

        Next

        Dim myRow As DataRow = dt.NewRow
        Dim myRowFixo As DataRow = dt.NewRow
        Dim myRowServicos As DataRow = dt.NewRow
        Dim myRow0800 As DataRow = dt.NewRow
        Dim myRow4004 As DataRow = dt.NewRow
        Dim myRowDADOS As DataRow = dt.NewRow
        Dim myRowTotais As DataRow = dt.NewRow
        'coloca os valores das colunas
        myRow.Item(0) = "Linhas Móveis"
        myRowFixo.Item(0) = "Linhas Fixas"
        myRowServicos.Item(0) = "Serviços"
        myRow0800.Item(0) = "Linhas 0800"
        myRow4004.Item(0) = "Número Único"
        myRowDADOS.Item(0) = "Link de Dados"
        myRowTotais.Item(0) = "Total"
        For Each linha As GridViewRow In mygrid.Rows
            Dim valor As String = linha.Cells(1).Text
            If linha.Cells(2).Text = 1 Then
                'movel
                myRow.Item(linha.Cells(0).Text) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    ExibeMovel = True
                End If
            ElseIf linha.Cells(2).Text = 2 Then
                'fixo
                myRowFixo.Item(linha.Cells(0).Text) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    ExibeFixo = True
                End If
            ElseIf linha.Cells(2).Text = 4 Then
                '0800
                myRow0800.Item(linha.Cells(0).Text) = valor
                ' myRow.Item(linha.Cells(0).Text) = 0
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    Exibe0800 = True
                End If
            ElseIf linha.Cells(2).Text = 6 Then
                '0800
                myRow4004.Item(linha.Cells(0).Text) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    Exibe3003 = True
                End If
                'ElseIf linha.Cells(2).Text = 3 Then
                '    'servicos
                '    myRowServicos.Item(linha.Cells(0).Text) = valor

                '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                '        ExibeServico = True

                '    End If
                'ElseIf linha.Cells(2).Text = 5 Then
                '    'servicos
                '    myRowServicos.Item(linha.Cells(0).Text) = valor

                '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                '        ExibeServico = True

                '    End If
            ElseIf linha.Cells(2).Text = 5 Then
                'LINK de DADOS
                myRowDADOS.Item(linha.Cells(0).Text) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    exibeDados = True
                End If
            Else
                'serviços
                myRowServicos.Item(linha.Cells(0).Text) = valor

                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    ExibeServico = True

                End If
            End If

            Try
                'coloca os totais
                myRowTotais.Item(linha.Cells(0).Text) = Convert.ToDecimal(myRowTotais.Item(linha.Cells(0).Text).ToString.Trim.Replace("R$ ", "")) + valor
                myRowTotais.Item(linha.Cells(0).Text) = FormatCurrency(myRowTotais.Item(linha.Cells(0).Text))

            Catch ex As Exception

            End Try

            i += 1
        Next

        dt.Rows.Add(myRow)
        dt.Rows.Add(myRowFixo)
        dt.Rows.Add(myRow0800)
        dt.Rows.Add(myRow4004)
        dt.Rows.Add(myRowServicos)
        dt.Rows.Add(myRowDADOS)
        dt.Rows.Add(myRowTotais)
        Me.gvResumoGeral.DataSource = dt
        Me.gvResumoGeral.DataBind()
        Me.gvResumoGeral.Rows(6).Style.Add("font-weight", "bold")
        'Me.gvResumoGeral.Rows(6).Style.Add("background-color", "#D8D9DE")
        Me.gvResumoMensal.Visible = False

        'coloca a legenda dos graficos

        For i = 1 To dt.Columns.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dt.Columns(i).ColumnName & "'"
            GraficoLabel += ","

            'coloca os valores
            'movel
            GraficoData += dt.Rows(0).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","

            'fixo
            If ExibeFixo Then
                GraficoData2 += dt.Rows(1).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(1).Visible = False
                gvResumoGeral.Rows(5).Visible = False
            End If

            '0800
            If Exibe0800 Then
                GraficoData3 += dt.Rows(2).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(2).Visible = False
            End If

            '4004
            If Exibe3003 Then
                GraficoData4 += dt.Rows(3).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(3).Visible = False
            End If

            'serviços
            If ExibeServico Then
                GraficoData5 += dt.Rows(4).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(4).Visible = False
            End If

            'link de dados
            If exibeDados Then
                GraficoData6 += dt.Rows(5).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(5).Visible = False
            End If

            'coloca o link para o RIT
            'dt.Columns(i).ColumnName = Context.Server.HtmlDecode("<a href='#'>" + dt.Columns(i).ColumnName.ToString + "</a>")

        Next

        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

        If GraficoData2 <> "" Then
            If GraficoData2.Substring(GraficoData2.Length - 1, 1) = "," Then
                GraficoData2 = GraficoData2.Substring(0, GraficoData2.Length - 1)
            End If
        End If


        If GraficoData3 <> "" Then
            If GraficoData3.Substring(GraficoData3.Length - 1, 1) = "," Then
                GraficoData3 = GraficoData3.Substring(0, GraficoData3.Length - 1)
            End If
        End If
        If GraficoData4 <> "" Then
            If GraficoData4.Substring(GraficoData4.Length - 1, 1) = "," Then
                GraficoData4 = GraficoData4.Substring(0, GraficoData4.Length - 1)
            End If
        End If

        If GraficoData5 <> "" Then
            If GraficoData5.Substring(GraficoData5.Length - 1, 1) = "," Then
                GraficoData5 = GraficoData5.Substring(0, GraficoData5.Length - 1)
            End If
        End If

        If GraficoData6 <> "" Then
            If GraficoData6.Substring(GraficoData6.Length - 1, 1) = "," Then
                GraficoData6 = GraficoData6.Substring(0, GraficoData6.Length - 1)
            End If
        End If


        'Me.gvResumoGeral.DataSource = dt
        'Me.gvResumoGeral.DataBind()
    End Sub


    Protected Sub gvResumoGeral_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResumoGeral.RowDataBound
        For Each cell As TableCell In e.Row.Cells
            If e.Row.RowType = DataControlRowType.Header Then
                If Not String.IsNullOrEmpty(cell.Text.Trim) Then
                    'cell.Text = Server.HtmlDecode("<a href='Rit.aspx?competencia='" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & " '>" & cell.Text & "</a>")
                    cell.Text = Server.HtmlDecode("<a href=""RIT.aspx?competencia=" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & "&grupo=" & Request.QueryString("grupo") & """>" & cell.Text & "</a>")
                End If
            End If

            If e.Row.Cells.GetCellIndex(cell) > 0 Then
                cell.HorizontalAlign = HorizontalAlign.Right
            End If
            Dim nomeServico As String = ""
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim codigoTipo As String = ""
                If Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS MÓVEIS" Then
                    codigoTipo = 1
                    nomeServico = " - Linhas Móveis"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS FIXAS" Then
                    codigoTipo = 2
                    nomeServico = " - Linhas Fixas"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS 0800" Then
                    codigoTipo = 4
                    nomeServico = " - Linhas 0800"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "NÚMERO ÚNICO" Then
                    codigoTipo = 6
                    nomeServico = " - Número Único"
                End If

                e.Row.Cells(0).Text = "<b><a href='graficoOperServico.aspx?codigoTipo=" & codigoTipo & "&nomeServico=" & nomeServico & "'>" & e.Row.Cells(0).Text & "</a></b>"
            End If

        Next
    End Sub

    'Protected Sub lstRbTipoValor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstRbTipoValor.SelectedIndexChanged
    '    Session("tipoValor") = Me.lstRbTipoValor.SelectedValue
    '    Response.Redirect("gestor.aspx?grupo=" & Request.QueryString("grupo") & "&hierarquia=" & Request.QueryString("hierarquia") & "&tipoValor=" & Me.lstRbTipoValor.SelectedValue)
    'End Sub

    Function getQueryTotalOperadora() As String

        Dim sql As String = ""
        sql += " "
        sql += " select p4.codigo || '-' || p4.descricao operadora, sum(p1.valor_cdr)gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p3.codigo_operadora=p4.codigo "
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-3),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'), p4.codigo || '-' || p4.descricao "
        sql += " order  by to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')"

        Return sql
    End Function

    Function getQueryTotalCustoOperadora() As String

        Dim sql As String = ""
        sql += " "
        sql += " select distinct p4.codigo || '-' || p4.descricao operadora  "
        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p3.codigo_operadora=p4.codigo "
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-3),'MM/YYYY'),'MM/YYYY') "
        sql += " group by  p4.codigo || '-' || p4.descricao "
        'sql += " group by fs.status_desc"

        Return sql
    End Function


End Class