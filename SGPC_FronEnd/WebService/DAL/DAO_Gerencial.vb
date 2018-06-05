Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Gerencial
    Private _strConn As String = ""
    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Public Function RetornaConexao() As String
        Return strConn
    End Function


    Public Function getGerencial(ByVal pCodigoUsuario As Integer, ByVal grupo As String, Optional ByVal pCodigoGerente As Integer = -1) As DataTable
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = ""
        'strSQL = strSQL + "select to_char(p1.vencimento,'MM/YYYY')vencimento, nvl(round(sum(p1.valor_original),2),0)valor_faturado,nvl(round(sum(p1.valor),2),0)valor_pago,nvl(round(sum(p1.valor_contestado), 2),0) valor_contestado, nvl(round((sum(p1.valor_original)-sum(p1.valor)),2),0)economia "
        strSQL = strSQL + "select to_char(p1.vencimento,'MM/YYYY')vencimento, nvl(round(sum(p1.valor_original),2),0)valor_faturado,nvl(round(sum(p1.valor_original) - SUM(valor_contestado_aprovado),2),0)valor_pago,nvl(round(sum(p1.valor_contestado), 2),0) valor_contestado, nvl(round((sum(p1.valor_original)-(nvl(round(sum(p1.valor_original) - SUM(valor_contestado_aprovado),2),sum(p1.valor_original)))),2),0)economia "

        strSQL = strSQL + " from rel_gerencial p1 where 1=1 "
        If pCodigoGerente > 0 Then
            strSQL = strSQL + " and p1.codigo_gerente='" & pCodigoGerente & "'"
        End If
        strSQL += " and vencimento>= TRUNC(add_months(SysDate,-13),'MONTH') "
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(pCodigoUsuario) & vbNewLine
            strSQL = strSQL + "     and p100.tipo_usuario in('DI','GE','SU')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        Else
            strSQL = strSQL + " and upper(operadora)<>'PABX' "
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            Dim list_grp_codigo As String() = grupo.Split(",")
            Dim i As Integer = 0
            strSQL += " and ("
            For i = 0 To list_grp_codigo.Length - 2
                strSQL += " UPPER(p1.grupo) like '" & list_grp_codigo(i).ToUpper & "%'" & vbNewLine
                If i < list_grp_codigo.Length - 2 Then
                    strSQL += " or "
                End If
            Next
            strSQL += " )" & vbNewLine
        End If
        strSQL = strSQL + " group by to_char(p1.vencimento,'MM/YYYY') order by to_date(to_char(p1.vencimento, 'MM/YYYY'),'MM/YYYY')"

        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()

        Dim cmd As New OleDbCommand(strSQL, connection)
        cmd.CommandText = strSQL
        'Dim reader As OleDbDataReader
        connection.Open()
        'reader = cmd.ExecuteReader
        Dim _dt As DataTable = New DataTable
        Using connection
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            adapter.Fill(_dt)
        End Using

        Return _dt

        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()

    End Function

    Public Function getGerencialServicosV2(ByVal pCodigoUsuario As Integer, ByVal grupo As String, ByVal pdata As String, Optional ByVal pCodigoGerente As Integer = -1) As DataTable
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = ""
        strSQL = strSQL + "select  upper(replace(p1.servico,' ',''))servico,to_char(p1.vencimento,'MM/YYYY')vencimento,nvl(sum(p1.valor_original),0)valor_faturado,nvl(sum(p1.valor),0)valor_pago, nvl((sum(p1.valor_original)-sum(p1.valor)),0)economia "
        strSQL = strSQL + " from rel_gerencial p1 "
        strSQL = strSQL + " where 1=1  "
        If pCodigoGerente > 0 Then
            strSQL = strSQL + " and p1.codigo_gerente='" & pCodigoGerente & "'"
        End If
        strSQL += " and vencimento>= TRUNC(add_months(SysDate,-13),'MONTH') "

        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(pCodigoUsuario) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('DI','GE','SU')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            Dim list_grp_codigo As String() = grupo.Split(",")
            Dim i As Integer = 0
            strSQL += " and ("
            For i = 0 To list_grp_codigo.Length - 2
                strSQL += " UPPER(p1.grupo) like '" & list_grp_codigo(i).ToUpper & "%'" & vbNewLine
                If i < list_grp_codigo.Length - 2 Then
                    strSQL += " or "
                End If
            Next
            strSQL += " )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(pdata) Then
            strSQL += " and to_char(vencimento,'MM/YYYY')= '" & pdata & "'" & vbNewLine
        End If
        strSQL = strSQL + " group by  upper(replace(p1.servico,' ','')),to_char(p1.vencimento,'MM/YYYY') order by  to_date(to_char(p1.vencimento, 'MM/YYYY'),'MM/YYYY'),upper(replace(p1.servico,' ',''))"

        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()

        Dim cmd As New OleDbCommand(strSQL, connection)
        cmd.CommandText = strSQL
        'Dim reader As OleDbDataReader
        connection.Open()
        'reader = cmd.ExecuteReader
        Dim _dt As DataTable = New DataTable
        Using connection
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            adapter.Fill(_dt)
        End Using
        Return _dt
    End Function

    Public Function getGerencialServicos(ByVal pCodigoUsuario As Integer, ByVal grupo As String, ByVal pdata As String, Optional ByVal pCodigoGerente As Integer = -1) As DataTable
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = ""
        strSQL = strSQL + "select  upper(replace(p1.servico,' ',''))servico,nvl(sum(p1.valor_original),0)valor_faturado,nvl(sum(p1.valor),0)valor_pago, nvl((sum(p1.valor_original)-sum(p1.valor)),0)economia "
        strSQL = strSQL + " from rel_gerencial p1 "
        strSQL = strSQL + " where 1=1  "
        If pCodigoGerente > 0 Then
            strSQL = strSQL + " and p1.codigo_gerente='" & pCodigoGerente & "'"
        End If
        strSQL += " and vencimento>= TRUNC(add_months(SysDate,-13),'MONTH') "

        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(pCodigoUsuario) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('DI','GE','SU')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            Dim list_grp_codigo As String() = grupo.Split(",")
            Dim i As Integer = 0
            strSQL += " and ("
            For i = 0 To list_grp_codigo.Length - 2
                strSQL += " UPPER(p1.grupo) like '" & list_grp_codigo(i).ToUpper & "%'" & vbNewLine
                If i < list_grp_codigo.Length - 2 Then
                    strSQL += " or "
                End If
            Next
            strSQL += " )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(pdata) Then
            strSQL += " and to_char(vencimento,'MM/YYYY')= '" & pdata & "'" & vbNewLine
        End If
        strSQL = strSQL + " group by  upper(replace(p1.servico,' ','')) order by  upper(replace(p1.servico,' ',''))"

        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()

        Dim cmd As New OleDbCommand(strSQL, connection)
        cmd.CommandText = strSQL
        'Dim reader As OleDbDataReader
        connection.Open()
        'reader = cmd.ExecuteReader
        Dim _dt As DataTable = New DataTable
        Using connection
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            adapter.Fill(_dt)
        End Using
        Return _dt
    End Function

    Public Function getGerencialServicosOperadora(ByVal pCodigoUsuario As Integer, ByVal grupo As String, ByVal pdata As String, Optional ByVal pCodigoGerente As Integer = -1) As DataTable
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = ""
        strSQL = strSQL + "select  upper(replace(p1.servico,' ',''))servico,to_char(p1.vencimento,'MM/YYYY')vencimento,upper(replace(p1.operadora,' ',''))operadora,nvl(sum(p1.valor_original),0)valor_faturado,nvl(sum(p1.valor),0)valor_pago, nvl((sum(p1.valor_original)-sum(p1.valor)),0)economia "
        strSQL = strSQL + " from rel_gerencial p1 where 1=1 "
        If pCodigoGerente > 0 Then
            strSQL = strSQL + " and p1.codigo_gerente='" & pCodigoGerente & "'"
        End If
        strSQL += " and vencimento>= TRUNC(add_months(SysDate,-13),'MONTH') "
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(pCodigoUsuario) & vbNewLine
            'strSQL = strSQL + "     and p100.tipo_usuario in('DI','GE','SU')" & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            Dim list_grp_codigo As String() = grupo.Split(",")
            Dim i As Integer = 0
            strSQL += " and ("
            For i = 0 To list_grp_codigo.Length - 2
                strSQL += " UPPER(p1.grupo) like '" & list_grp_codigo(i).ToUpper & "%'" & vbNewLine
                If i < list_grp_codigo.Length - 2 Then
                    strSQL += " or "
                End If
            Next
            strSQL += " )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(pdata) Then
            strSQL += " and to_char(vencimento,'MM/YYYY')= '" & pdata & "'" & vbNewLine
        End If
        strSQL = strSQL + " group by upper(replace(p1.servico,' ','')),to_char(p1.vencimento,'MM/YYYY'),upper(replace(p1.operadora,' ','')) order by upper(replace(p1.servico,' ','')),upper(replace(p1.operadora,' ',''))"

        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()

        Dim cmd As New OleDbCommand(strSQL, connection)
        cmd.CommandText = strSQL
        'Dim reader As OleDbDataReader
        connection.Open()
        'reader = cmd.ExecuteReader
        Dim _dt As DataTable = New DataTable
        Using connection
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            adapter.Fill(_dt)
        End Using
        Return _dt
    End Function

    Public Function GetGerentes(ByVal pTipo As String, Optional ByVal pCodigoPai As Integer = -1) As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listUsuarios As New List(Of AppUsuarios)

        Dim strSQL As String = "select distinct p1.NOME_USUARIO, p1.CODIGO "
        strSQL = strSQL + "from USUARIOS p1, rel_gerencial p2 where  "
        If pTipo.ToString.ToUpper = "DI" Then
            strSQL = strSQL + " p1.codigo=p2.codigo_diretor "
        ElseIf pTipo.ToString.ToUpper = "SU" Then
            strSQL = strSQL + " p1.codigo=p2.codigo_superintendente "
        Else
            strSQL = strSQL + " p1.codigo=p2.codigo_gerente "
        End If

        'strSQL = strSQL + " and p1.codigo in(select distinct t.codigo_usuario from CATEGORIA_USUARIO t where upper(t.tipo_usuario) ='" & pTipo.ToString.ToUpper & "') "
        strSQL = strSQL + " and exists (select 0 from CATEGORIA_USUARIO t where upper(t.tipo_usuario) ='" & pTipo.ToString.ToUpper & "' and t.codigo_usuario=p1.codigo) "
        If pCodigoPai > 0 Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(pCodigoPai) & vbNewLine
            strSQL = strSQL + "     and cat.tipo_usuario in('DI','GE','SU')" & vbNewLine
            'strSQL = strSQL + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
            strSQL = strSQL + "     and to_char(p2.grupo)=cat.codigo_grupo) " & vbNewLine
        End If
        strSQL = strSQL + "order by NOME_USUARIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("NOME_USUARIO").ToString)
                listUsuarios.Add(_registro)
            End While
        End Using

        Return listUsuarios
    End Function


    Public Function GetDiretorById(ByVal pTipo As String, Optional ByVal pCodigo As Integer = -1) As String
        Dim connection As New OleDbConnection(strConn)
        Dim _result As String = ""

        Dim strSQL As String = "select distinct p1.NOME_USUARIO, p1.CODIGO "
        strSQL = strSQL + "from USUARIOS p1, rel_gerencial p2 where  "
        If pTipo.ToString.ToUpper = "DI" Then
            strSQL = strSQL + " p1.codigo=p2.codigo_diretor "
        ElseIf pTipo.ToString.ToUpper = "SU" Then
            strSQL = strSQL + " p1.codigo=p2.codigo_superintendente "
        Else
            strSQL = strSQL + " p1.codigo=p2.codigo_gerente "
        End If

        'strSQL = strSQL + " and p1.codigo in(select distinct t.codigo_usuario from CATEGORIA_USUARIO t where upper(t.tipo_usuario) ='" & pTipo.ToString.ToUpper & "') "
        strSQL = strSQL + " and exists (select 0 from CATEGORIA_USUARIO t where upper(t.tipo_usuario) ='" & pTipo.ToString.ToUpper & "' and t.codigo_usuario=p1.codigo) "
        If pCodigo > 0 Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and p1.codigo='" & pCodigo & "'" & vbNewLine

        End If
        strSQL = strSQL + "order by NOME_USUARIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _result = reader.Item("NOME_USUARIO").ToString
            End While
        End Using

        Return _result
    End Function


End Class
