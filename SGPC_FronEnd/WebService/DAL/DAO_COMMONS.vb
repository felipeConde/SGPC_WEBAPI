Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Collections.Generic

Public Class DAO_Commons
    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Private tipo_ordem As String = "TAMANHO"

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

    Public Function UpdateParticularesCelular(ByVal codigo As String, ByVal autorlog As String, ByVal particular As String, ByVal data As String) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim sql As String = ""
            sql = "update CDRS_CELULAR set particular='" + particular + "',data_apontamento=sysdate,autor_apontamento='" + autorlog + "' where data_inicio=to_date('" + data + "','DD/MM/YYYY HH24:MI:SS')  and codigo='" + codigo + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function CompararObjetos(ByVal _Objeto1 As Object, ByVal _Objeto2 As Object) As Boolean

        Dim _TipoObjeto1 As String = ""
        Dim _TipoObjeto2 As String = ""

        If Not _Objeto1 Is Nothing Then
            _TipoObjeto1 = _Objeto1.GetType.ToString
        End If

        If Not _Objeto2 Is Nothing Then
            _TipoObjeto2 = _Objeto2.GetType.ToString
        End If

        Dim _Resultado As Boolean = True

        If _TipoObjeto1 = _TipoObjeto2 Then
            Dim Propiedades() As PropertyInfo = _Objeto1.GetType.GetProperties
            Dim Propiedad As PropertyInfo
            Dim _Valor1 As Object
            Dim _Valor2 As Object
            For Each Propiedad In Propiedades
                _Valor1 = Propiedad.GetValue(_Objeto1, Nothing)
                _Valor2 = Propiedad.GetValue(_Objeto2, Nothing)
                If _Valor1 <> _Valor2 Then
                    _Resultado = False
                    Exit For
                End If
            Next
        Else
            _Resultado = False
        End If

        Return _Resultado

    End Function

    Public Function GetMaximumCode(ByVal codefield As String, ByVal table As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim max As String = ""
        Dim strSQL As String = "select nvl(max(" + codefield + "),0)+1 as MAXIMUM from " + table + ""

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                max = reader.Item("MAXIMUM")
            End While
        End Using

        Return max

    End Function

    Public Function Get_CellPhoneType(ByVal cellnumber As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim tipo_aparelho As String = ""
        Dim strSQL As String = " select decode(nvl(cod_tipo,'0'), '1', 'CELULAR', '2', 'RADIO','3', 'MODEM','4','SMARTPHONE','5','BLACKBERRY','6','GATEWAY', 'OUTROS')Tipo "
        strSQL = strSQL + " from aparelhos_modelos p1, aparelhos_moveis p2, linhas_moveis p3, linhas p4"
        strSQL = strSQL + " where p1.cod_modelo =  p2.cod_modelo"
        strSQL = strSQL + " and p2.codigo_aparelho =  p3.codigo_aparelho"
        strSQL = strSQL + " and p3.codigo_linha =  p4.codigo_linha "
        strSQL = strSQL + " and replace(replace(replace(REPLACE(p4.NUM_LINHA(+),')',''),'(',''),'-',''),' ','') ='" + cellnumber + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                tipo_aparelho = reader.Item("Tipo")
            End While
        End Using

        Return tipo_aparelho

    End Function

    Public Sub GetRateiosLinks(ByVal codigo As String, ByRef list_ccusto As List(Of String), ByRef list_rateios As List(Of String))
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select CODIGO_CCUSTO,  nvl(RATEIO,0) as RATEIO"
        strSQL = strSQL + " FROM CCUSTO_LINKS where CODIGO_LINK='" + codigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                list_ccusto.Add(reader.Item("CODIGO_CCUSTO").ToString)
                list_rateios.Add(reader.Item("RATEIO").ToString)
            End While
        End Using

    End Sub

    Public Function GetTiposCategoriaUser() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim List As New List(Of AppGeneric)

        Dim strSQL As String = "select codigo, descricao  from tipo_categoria where ativo='S' order by codigo "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, (reader.Item("codigo").ToString & " - " & reader.Item("descricao").ToString))
                List.Add(_registro)
            End While
        End Using

        Return List

    End Function

    Public Function SalvaRateios(ByVal codigo_link As String, ByRef ccusto As String, ByRef rateio As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try

            Dim strSQL As String = "update CCUSTO_LINKS set"
            strSQL = strSQL + " RATEIO='" + rateio.Replace(",", ".").ToString + "'"
            strSQL = strSQL + " WHERE CODIGO_CCUSTO='" + ccusto + "'"
            strSQL = strSQL + " AND CODIGO_LINK='" + codigo_link + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function SearchField(ByVal text As String, ByVal table_field As String, ByVal table_code_field As String, ByVal table_name As String, ByVal op_field As String, ByVal op_value As String, Optional ByVal str_query As String = "") As List(Of AppPesquisa)
        Dim connection As New OleDbConnection(strConn)
        Dim Pesquisa As New List(Of AppPesquisa)
        Dim aux As Integer = 0

        Dim strSQL As String = "select " + table_code_field + "," + table_field + " as DESCRICAO "
        strSQL = strSQL + "FROM " + table_name
        If text <> "" Then
            strSQL = strSQL + " WHERE (upper(" + table_field + ") LIKE '%" + text.ToUpper + "%' OR upper(" + table_code_field + ") LIKE '%" + text.ToUpper + "%') "
            If op_field <> "" Then
                strSQL = strSQL + " AND " + op_field + " = '" + op_value + "'"
            End If
            If table_name.ToUpper = "GRUPOS" Then
                strSQL = strSQL + " and ATIVO='S' "
            End If
        Else
            If op_field <> "" Then
                strSQL = strSQL + " WHERE " + op_field + " = '" + op_value + "'"

                If table_name.ToUpper = "GRUPOS" Then
                    strSQL = strSQL + " and ATIVO='S' "
                End If
            Else
                If table_name.ToUpper = "GRUPOS" Then
                    strSQL = strSQL + " where ATIVO='S' "
                End If

            End If
        End If

            If str_query <> "" Then
                strSQL = strSQL + str_query
            End If

            If table_name.ToUpper = "GRUPOS" Then

                If tipo_ordem.ToUpper = "TAMANHO" Then
                    strSQL = strSQL + "    order by  LENGTH(" + table_code_field + ")," + table_code_field + ", " + table_field + " "
                Else
                    strSQL = strSQL + "    order by  " + table_code_field + ", " + table_field + " "

                End If
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _registro As New AppPesquisa(reader.Item("" + table_code_field + ""), reader.Item("DESCRICAO").ToString)
                    Pesquisa.Add(_registro)
                End While
            End Using

            Return Pesquisa

    End Function

    Public Function SearchCellPhoneHierarchybyUser(ByVal text As String, ByVal user_code As String, ByVal session_user As String) As List(Of AppPesquisa)
        Dim connection As New OleDbConnection(strConn)
        Dim Pesquisa As New List(Of AppPesquisa)
        Dim aux As Integer = 0

        Dim strSQL As String = " select l.NUM_LINHA as CODIGO, (u.NOME_USUARIO || ' ' || l.NUM_LINHA || ' - ' || decode(nvl(amo.cod_tipo,'0'),'0', 'SEM APARELHO', '1', 'CELULAR', '2', 'RADIO','3', 'MODEM','4','SMARTPHONE','5','BLACKBERRY','6','GATEWAY', 'OUTROS')) as NUM_LINHA "
        strSQL = strSQL + " from linhas l, linhas_moveis lm,usuarios u,aparelhos_moveis ap,aparelhos_marcas  ama,aparelhos_modelos amo "
        strSQL = strSQL + " where l.CODIGO_LINHA = lm.CODIGO_LINHA "
        strSQL = strSQL + " and lm.CODIGO_USUARIO = u.CODIGO "
        strSQL = strSQL + " and lm.codigo_aparelho = ap.codigo_aparelho(+) "
        strSQL = strSQL + " and ama.cod_marca(+) = amo.cod_marca "
        strSQL = strSQL + " and amo.cod_modelo(+) = ap.cod_modelo "
        strSQL = strSQL + " and l.NUM_LINHA is not null "
        If user_code <> "" Then
            strSQL = strSQL + " and   lm.CODIGO_USUARIO ='" + user_code + "' "
        End If
        If text <> "" Then
            strSQL = strSQL + " and UPPER(replace(replace(replace(REPLACE((u.NOME_USUARIO || ' ' || l.NUM_LINHA || ' - ' || decode(nvl(amo.cod_tipo,'0'),'0', 'SEM APARELHO', '1', 'CELULAR', '2', 'RADIO','3', 'MODEM','4','SMARTPHONE','5','BLACKBERRY','6','GATEWAY', 'OUTROS')),')',''),'(',''),'-',''),' ','')) LIKE UPPER(replace(replace(replace(REPLACE('%" + text + "%',')',''),'(',''),'-',''),' ','')) "
        End If
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(session_user) & vbNewLine
            strSQL = strSQL + "     and to_char(u.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppPesquisa(reader.Item("CODIGO"), reader.Item("NUM_LINHA").ToString)
                Pesquisa.Add(_registro)
            End While
        End Using

        Return Pesquisa

    End Function

    Public Function SearchPhoneHierarchybyUser(ByVal text As String, ByVal user_code As String, ByVal session_user As String) As List(Of AppPesquisa)
        Dim connection As New OleDbConnection(strConn)
        Dim Pesquisa As New List(Of AppPesquisa)
        Dim aux As Integer = 0

        Dim strSQL As String = " select l.NUM_LINHA as CODIGO_LINHA,  (u.NOME_USUARIO || ' ' || l.NUM_LINHA) as NUM_LINHA   from linhas l, usuarios u "
        strSQL = strSQL + " where l.CODIGO_USUARIO=u.CODIGO "
        If user_code <> "" Then
            strSQL = strSQL + " and   l.CODIGO_USUARIO ='" + user_code + "' "
        End If
        If text <> "" Then
            strSQL = strSQL + " and UPPER(replace(replace(replace(REPLACE((u.NOME_USUARIO || ' ' || l.NUM_LINHA),')',''),'(',''),'-',''),' ','')) LIKE replace(replace(replace(REPLACE('%" + text + "%',')',''),'(',''),'-',''),' ','') "
        End If
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(session_user) & vbNewLine
            strSQL = strSQL + "     and to_char(u.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        strSQL = strSQL + "  and not exists (select lm.CODIGO_LINHA from linhas_moveis lm where l.CODIGO_LINHA = lm.CODIGO_LINHA) "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppPesquisa(reader.Item("CODIGO_LINHA"), reader.Item("NUM_LINHA").ToString)
                Pesquisa.Add(_registro)
            End While
        End Using

        Return Pesquisa

    End Function

    Public Function SearchFieldHierarchy(ByVal text As String, ByVal table_field As String, ByVal table_code_field As String, ByVal table_name As String, ByVal code_field_2 As String, ByVal value_code_field_2 As String, ByVal cod_user As String, Optional ByVal str_query As String = "") As List(Of AppPesquisa)
        Dim connection As New OleDbConnection(strConn)
        Dim Pesquisa As New List(Of AppPesquisa)
        Dim aux As Integer = 0

        Dim strSQL As String = "select distinct p1." + table_code_field + ""

        If table_name.ToUpper = "USUARIOS" Then
            strSQL = strSQL + " , (p1.matricula || ' - ' || p1." + table_field + " ) as descricao "
            strSQL = strSQL + " FROM " + table_name + " p1"
            strSQL = strSQL + " WHERE (upper(" + table_field + ") LIKE '%" + text.ToUpper + "%' OR upper(" + table_code_field + ") LIKE '%" + text.ToUpper + "%'  OR matricula LIKE '%" + text.ToUpper + "%')"
        Else
            strSQL = strSQL + " , p1." + table_field + " as descricao "
            strSQL = strSQL + " FROM " + table_name + " p1"
            strSQL = strSQL + " WHERE (upper(" + table_field + ") LIKE '%" + text.ToUpper + "%' OR upper(" + table_code_field + ") LIKE '%" + text.ToUpper + "%')"
        End If

  
        If code_field_2 <> "" Then
            strSQL = strSQL + " AND " + code_field_2 + "='" + value_code_field_2 + "' "
        End If
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(cod_user) & vbNewLine
            'strSQL = strSQL + "     and cat.tipo_usuario in('D','G','GE','DI','SU')" & vbNewLine
            If table_name.ToUpper = "GRUPOS" Then
                strSQL = strSQL + "     and to_char(p1.CODIGO) like cat.codigo_grupo||'%' ) and p1.ATIVO='S'" & vbNewLine
            ElseIf table_name.ToUpper = "REL_GERENCIAL" Then
                strSQL = strSQL + "     and to_char(p1.GRUPO) like cat.codigo_grupo||'%' ) " & vbNewLine
            Else
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
            End If

        End If

        If table_name.ToUpper = "GRUPOS" Then
            strSQL = strSQL + "     and   upper(nvl(p1.ATIVO,'N'))='S' "
            'ElseIf table_name.ToUpper = "RAMAIS" Then
            '    strSQL = strSQL + " and not exists (select 0 from usuarios where rml_numero_a=p1.NUMERO_A) "
        End If
        If str_query <> "" Then
            strSQL = strSQL + " " + str_query
        End If

        If table_name.ToUpper = "GRUPOS" Then
            If tipo_ordem.ToUpper = "TAMANHO" Then
                strSQL = strSQL + "    order by  LENGTH(p1." + table_code_field + "),p1." + table_code_field + ", p1." + table_field + " "
            Else
                strSQL = strSQL + "    order by  p1." + table_code_field + ", p1." + table_field + " "
            End If
        End If


        'HttpContext.Current.Response.Write(strSQL)
        'HttpContext.Current.Response.End()


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL.Replace("&#39;", " ' ")
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppPesquisa(reader.Item("" + table_code_field + ""), reader.Item("descricao").ToString)
                Pesquisa.Add(_registro)
            End While
        End Using

        Return Pesquisa

    End Function

    Public Function GetLogs(ByVal table_itens_codes As List(Of String), ByVal Id As String, ByVal table As String) As DataTable
        Dim connection As New OleDbConnection(strConn)
        Dim aux As Integer = 0

        Dim strSQL As String = "select * "
        strSQL = strSQL + "FROM " + table

        For Each item As String In table_itens_codes
            If aux = 0 Then
                strSQL = strSQL + " WHERE " + Id + " = '" + item + "'"
            Else
                strSQL = strSQL + " OR " + Id + " = '" + item + "'"
            End If
            aux = aux + 1
        Next

        Return myDataTable(strSQL)

    End Function

    Public Function GetAll(ByVal table As String) As DataTable
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select * "
        strSQL = strSQL + "FROM " + table

        Return myDataTable(strSQL)

    End Function

    Public Function myDataTable(ByVal SQL As String) As DataTable
        Dim cn As OleDbConnection
        Dim dsTemp As DataSet
        Dim dsCmd As OleDbDataAdapter

        cn = New OleDbConnection(strConn)
        cn.Open()

        dsCmd = New OleDbDataAdapter(SQL, cn)
        dsTemp = New DataSet()
        dsCmd.Fill(dsTemp, "myQuery")
        cn.Close()
        Return dsTemp.Tables(0)
    End Function

    Public Sub CSVFromReader(ByVal SQL As String, ByVal file_name As String)
        Dim cn As New OleDbConnection(strConn)
        Dim comma2 As New OleDbCommand(SQL, cn)
        Dim builder As New System.Text.StringBuilder

        'HttpContext.Current.Response.Write(strConn)
        'HttpContext.Current.Response.End()
        cn.Open()
        Using cn
            Dim dr2 As OleDbDataReader = comma2.ExecuteReader()
            Dim i As Integer
            'Dim dt As New DataTable()
            'dt.Load(dr2)


            If dr2.HasRows Then

                'pega o nome das colunas
                For i = 0 To dr2.FieldCount - 1
                    'builder.Append(dr2.GetName(i)).Append(";")
                    HttpContext.Current.Response.Write(dr2.GetName(i) & ";")
                Next

            End If
            HttpContext.Current.Response.Write(vbNewLine)

            While dr2.Read
                For i = 0 To dr2.FieldCount - 1
                    'builder.Append(dr2.Item(i)).Append(";")
                    HttpContext.Current.Response.Write(dr2.Item(i).ToString & ";")
                Next
                HttpContext.Current.Response.Write(vbNewLine)
            End While
            dr2.Close()
        End Using
        comma2.Dispose()
        HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + file_name + ".csv")
        HttpContext.Current.Response.End()
    End Sub


    Public Sub CSVFromReader2(ByVal SQL As String, ByVal file_name As String)
        Dim cn As New OleDbConnection(strConn)
        Dim comma2 As New OleDbCommand(SQL, cn)
        Dim builder As New System.Text.StringBuilder

        'HttpContext.Current.Response.Write(strConn)
        'HttpContext.Current.Response.End()
        cn.Open()
        Using cn
            Dim dr2 As OleDbDataReader = comma2.ExecuteReader()
            Dim i As Integer
            'Dim dt As New DataTable()
            'dt.Load(dr2)


            If dr2.HasRows Then

                'pega o nome das colunas
                For i = 0 To dr2.FieldCount - 1
                    'builder.Append(dr2.GetName(i)).Append(";")
                    HttpContext.Current.Response.Write(dr2.GetName(i) & ";")
                Next

            End If
            HttpContext.Current.Response.Write(vbNewLine)

            While dr2.Read
                For i = 0 To dr2.FieldCount - 1
                    'builder.Append(dr2.Item(i)).Append(";")
                    HttpContext.Current.Response.Write(dr2.Item(i).ToString & ";")
                Next
                HttpContext.Current.Response.Write(vbNewLine)
            End While
            dr2.Close()
        End Using
        comma2.Dispose()
        'HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + file_name + ".csv")
        'HttpContext.Current.Response.End()
    End Sub

    Public Function CSVFromReader_strs(ByVal SQL As String) As String

        'primeiro a gente conta o numero de linhas. Se for muito grande gera na tela
        ' Dim dt As DataTable = myDataTable("select count(*) from (" & SQL & ")")
        Try
            Dim cn As New OleDbConnection(strConn)
            Dim comma2 As New OleDbCommand(SQL, cn)
            Dim builder As New System.Text.StringBuilder
            'Dim builder As String = ""


            cn.Open()
            Using cn
                Dim dr2 As OleDbDataReader = comma2.ExecuteReader()
                Dim i As Integer
                'Dim dt As New DataTable()
                'dt.Load(dr2)




                If dr2.HasRows Then

                    'pega o nome das colunas
                    For i = 0 To dr2.FieldCount - 1
                        builder.Append(dr2.GetName(i)).Append(";")
                        ' builder += dr2.GetName(i).ToString + ";"
                    Next

                End If
                builder.Append(vbNewLine)
                'builder += vbNewLine

                While dr2.Read
                    For i = 0 To dr2.FieldCount - 1

                        If InStr(dr2.GetName(i).ToString.ToUpper, "VALOR") Then
                            builder.Append(FormatCurrency(dr2.Item(i))).Append(";")
                            'builder += FormatCurrency(dr2.Item(i)).ToString + ";"
                        Else
                            builder.Append(dr2.Item(i)).Append(";")
                            'builder += dr2.Item(i).ToString + ";"
                        End If


                    Next
                    builder.Append(vbNewLine)
                    'builder += vbNewLine
                End While
                dr2.Close()
            End Using
            comma2.Dispose()

            If builder.ToString.EndsWith(";") Then
                builder = builder.Remove(0, builder.Length - 1)
            End If
            Return builder.ToString
        Catch ex As Exception
            CSVFromReader2(SQL, "Contestacao_" & Date.Now.Ticks)
        End Try





    End Function

    Public Function GetGRPNameCode(ByVal pcodigo As Integer) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of String)

        Dim strSQL As String = "select NOME_GRUPO, CODIGO"
        strSQL = strSQL + " from GRUPOS "
        strSQL = strSQL + "where CODIGO in(select GRP_CODIGO from usuarios where CODIGO='" + Convert.ToString(pcodigo) + "')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                listItem.Add(reader.Item("NOME_GRUPO").ToString)
                listItem.Add(reader.Item("CODIGO").ToString)
            End While
        End Using

        Return listItem
    End Function

    Public Function GetCollumnsNamesOfaTable(ByVal table_name As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim listItem As New List(Of AppGeneric)

        Dim strSQL As String = "Select COLUMN_NAME from user_tab_columns where table_name=upper('" + table_name + "')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                listItem.Add(New AppGeneric(reader.Item("COLUMN_NAME").ToString, reader.Item("COLUMN_NAME").ToString))
            End While
        End Using

        Return listItem
    End Function

    Public Function GetComboLinhasTipo(ByVal tipo As Char) As List(Of AppComboComponetes)
        Dim connection As New OleDbConnection(strConn)
        Dim ComboComponetes As New List(Of AppComboComponetes)

        Dim strSQL As String = "select CODIGO_TIPO, Upper(TIPO) as TIPO "
        strSQL = strSQL + "from LINHAS_TIPO "
        strSQL = strSQL + "WHERE CLASSIFICACAO='" + tipo + "' order by TIPO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppComboComponetes(reader.Item("CODIGO_TIPO").ToString, reader.Item("TIPO").ToString)
                ComboComponetes.Add(_registro)
            End While
        End Using

        Return ComboComponetes
    End Function

    Public Function GetComboLocalidades() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim ComboComponetes As New List(Of AppGeneric)

        Dim strSQL As String = "select codigo, localidade as descricao from localidades"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("DESCRICAO").ToString)
                ComboComponetes.Add(_registro)
            End While
        End Using

        Return ComboComponetes
    End Function

    Public Function GetUF_and_citycode_ByLocalidades(ByVal pcodigo As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim ComboComponetes As New List(Of AppGeneric)

        Dim strSQL As String = "select codigo_cidade as codigo, ESTADO as descricao from localidades where codigo='" + pcodigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("DESCRICAO").ToString)
                ComboComponetes.Add(_registro)
            End While
        End Using

        Return ComboComponetes
    End Function

    Public Function ReturnTipoLigacaoByOP(ByVal operadora As String) As List(Of AppTipoLigacao)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTipoLigacao)

        Dim strSQL As String = " select p1.codigo,p1.descricao, "
        strSQL = strSQL + " decode(nvl(p1.TIPO_CHAMADA, 0),  0, 'Todas', 1, "
        strSQL = strSQL + " 'Ligações da mesma operadora', 2, 'Ligações para fixo', 3, "
        strSQL = strSQL + " 'Ligações para outra operadora') TIPO_CHAMADA,"
        strSQL = strSQL + " nvl(p3.tipo, ' ') tipo, p2.nome_configuracao tarifa "
        strSQL = strSQL + " from tipos_ligacao_teste p1, tarifacao p2, faturas_tipo p3 "
        strSQL = strSQL + " where p1.CODIGO_TARIF = p2.codigo "
        strSQL = strSQL + " and p2.TIPO_TARIFA = p3.codigo_tipo "
        strSQL = strSQL + " and p2.OPER_CODIGO_OPERADORA = '" + operadora + "' "
        strSQL = strSQL + " order by descricao "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppTipoLigacao(reader.Item("codigo").ToString, reader.Item("descricao").ToString, reader.Item("TIPO_CHAMADA").ToString, reader.Item("tipo").ToString, reader.Item("tarifa").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function ReturnGradeHorario(ByVal tipoligacao As String, ByVal operadora As String) As List(Of AppHorarioTarifa)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppHorarioTarifa)

        Dim strSQL As String = " select '' campo,"
        strSQL = strSQL + " decode(to_number(to_char(to_date('01/01/1995 00:00:00',"
        strSQL = strSQL + " 'DD/MM/YYYY HH24:MI:SS') + (p1.horario / 86400),"
        strSQL = strSQL + " 'DD')) - 1, 0, 'DOM', 1, 'SEG', 2, 'TER', 3, "
        strSQL = strSQL + " 'QUA', 4, 'QUI', 5,'SEX', 6, 'SAB') || ' ' || "
        strSQL = strSQL + " to_char(to_date('01/01/1995 00:00:00', 'DD/MM/YYYY HH24:MI:SS') +"
        strSQL = strSQL + " (p1.horario / 86400), 'HH24:MI') horario2,"
        strSQL = strSQL + " p2.ttm, rtrim(ltrim(to_char(p2.valor_ttm, '999.999999'))) valor_ttm,"
        strSQL = strSQL + " p2.step, rtrim(ltrim(to_char(p2.valor_step, '999.999999'))) valor_step,"
        strSQL = strSQL + " decode(p1.tipo_tarifa, 1, 'S', 2, 'R', 4, 'N', 8, 'D') tipo_tarifa"
        strSQL = strSQL + " from tarifas_teste p2, horarios_tarifacao_teste p1"
        strSQL = strSQL + " where(p1.codigo_tarifa = p2.codigo)"
        strSQL = strSQL + " and p1.horario <> 604800"
        strSQL = strSQL + " and p1.codigo =( select codigo_horario from horarios_tarifacao_op_teste"
        strSQL = strSQL + " where codigo_operadora='" & (operadora) & "'"
        strSQL = strSQL + " and codigo_tipo_ligacao='" & (tipoligacao) & "'"
        strSQL = strSQL + " and rownum <2) order by p1.horario"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppHorarioTarifa()

                _registro.Horario = reader.Item("horario2").ToString
                _registro.TTM = reader.Item("ttm").ToString
                _registro.TTM_Value = reader.Item("valor_ttm").ToString
                _registro.Step_ = reader.Item("step").ToString
                _registro.Step_value = reader.Item("valor_step").ToString
                _registro.Tipo_tarifa = reader.Item("tipo_tarifa").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function ComboTarifacaobyOP(ByVal operadora As String, ByVal tipo_tarifa As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = " select distinct p1.codigo, p1.nome_configuracao, "
        strSQL = strSQL + " p1.OPER_CODIGO_OPERADORA, p2.descricao operadora "
        strSQL = strSQL + " from tarifacao p1, operadoras_teste p2 "
        strSQL = strSQL + " where p1.OPER_CODIGO_OPERADORA = p2.codigo(+) "
        strSQL = strSQL + " and tipo_tarifa in ('" + tipo_tarifa + "') and oper_codigo_operadora = '" + operadora + "' "
        strSQL = strSQL + " and exists (select * from operadoras_teste p1 where codigo = OPER_CODIGO_OPERADORA) "
        strSQL = strSQL + " order by OPER_CODIGO_OPERADORA, nome_configuracao "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("nome_configuracao").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function ComboCidades(ByVal Uf As String) As List(Of AppCidades)
        Dim connection As New OleDbConnection(strConn)
        Dim listCidades As New List(Of AppCidades)

        Dim strSQL As String = "select MUNICIPIO, CODIGO_CIDADE, UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "where UF='" + Uf + "' "
        strSQL = strSQL + "order by MUNICIPIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppCidades(reader.Item("CODIGO_CIDADE").ToString, reader.Item("MUNICIPIO").ToString, reader.Item("UF").ToString)
                listCidades.Add(_registro)
            End While
        End Using

        Return listCidades
    End Function

    Public Function RetornaCidade(ByVal city_code As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim listCidades As New List(Of AppCidades)

        Dim strSQL As String = "select MUNICIPIO  "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "where CODIGO_CIDADE ='" + city_code + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Return reader.Item("MUNICIPIO").ToString
            End While
        End Using

        Return ""
    End Function

    Public Function ComboUfs(Optional ByVal cod_cidade As String = "") As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listUfs As New List(Of String)

        Dim strSQL As String = "select DISTINCT UF "
        strSQL = strSQL + " from CIDADES "
        If cod_cidade <> "" Then
            strSQL = strSQL + " WHERE CODIGO_CIDADE='" + cod_cidade.ToString + "'"
        End If
        strSQL = strSQL + " order by UF"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("UF").ToString)
                listUfs.Add(_registro)
            End While
        End Using

        Return listUfs
    End Function

    Public Function Is_Commom_User(ByVal code_user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select * from categoria_usuario where codigo_usuario=" + code_user

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                If reader.Item("CODIGO").ToString Then
                    Return False
                End If
            End While
        End Using

        Return True
    End Function

    Public Function Is_Administrator(ByVal code_user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select codigo_usuario from categoria_usuario cat"
        strSQL = strSQL + " where cat.codigo_usuario= '" + code_user + "'"
        strSQL = strSQL + " and cat.tipo_usuario in('A')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list.Add(reader.Item("codigo_usuario").ToString)
            End While
        End Using

        If list.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Return_UserLevel(ByVal code_user As String) As Integer

        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select area from grupos p1"
        strSQL = strSQL + " where exists(" & vbNewLine
        strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
        strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(code_user) & vbNewLine
        strSQL = strSQL + "     and cat.tipo_usuario in('D','G') and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list.Add(reader.Item("area").ToString)
            End While
        End Using

        Return list.Count

    End Function

    Public Function Is_AdministratorGA(ByVal code_user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select codigo_usuario from categoria_usuario cat"
        strSQL = strSQL + " where cat.codigo_usuario= '" + code_user + "'"
        strSQL = strSQL + " and cat.tipo_usuario in('A','GA')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list.Add(reader.Item("codigo_usuario").ToString)
            End While
        End Using

        If list.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Get_Phone_List_User(ByVal code_user As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim list_phone As New List(Of String)

        Dim strSQL As String = "select p1.num_linha from linhas p1, linhas_moveis p2 where p1.codigo_linha=p2.codigo_linha and trim(p2.codigo_usuario)='" + code_user + "' and p1.num_linha is not null"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list_phone.Add(reader.Item("num_linha").ToString)
            End While
        End Using

        Return list_phone
    End Function

    Public Function Get_Phone_List_UserNoCELL(ByVal code_user As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim list_phone As New List(Of String)

        Dim strSQL As String = " select p1.num_linha from linhas p1 "
        strSQL = strSQL + " where not exists (select lm.CODIGO_LINHA from linhas_moveis lm where p1.CODIGO_LINHA = lm.CODIGO_LINHA)"
        strSQL = strSQL + " and trim(p1.codigo_usuario)='" + code_user + "' and p1.num_linha is not null"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list_phone.Add(reader.Item("num_linha").ToString)
            End While
        End Using

        Return list_phone
    End Function

    Public Function SortGenericListbyDescription(ByVal x As AppGeneric, ByVal y As AppGeneric) As Integer
        Return x.Descricao.CompareTo(y.Descricao)
    End Function

    Public Function GetGenericList(ByVal codigo As String, ByVal campo_codigo As String, ByVal campo As String, ByVal tabela As String, Optional ByVal valor_campo As String = "", Optional ByVal end_query As String = "") As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select distinct " + campo_codigo + " as codigo , " + campo + " as descricao"
        strSQL = strSQL + " FROM " + tabela + " WHERE 1=1 "

        If codigo <> "" Then
            strSQL = strSQL + " AND " + campo_codigo + "='" + codigo + "' "
        End If

        If valor_campo <> "" Then
                strSQL = strSQL + " AND " + campo + "='" + valor_campo + "' "
        End If

        If end_query <> "" Then
            strSQL = strSQL + end_query
        End If

        If campo = "to_char(DATA, 'DD/MM/YYYY') AS DATA" Then
            campo = "DATA"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString)
                list.Add(_registro)
            End While
        End Using

        connection.Close()

        Return list
    End Function

    Public Function TemAcessoWEB(ByVal code_user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select u.codigo from usuarios u"
        strSQL = strSQL + " where u.codigo= '" + code_user + "' and upper(u.acesso_web)='S'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                list.Add(reader.Item("codigo").ToString)
            End While
        End Using

        If list.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function BloqueiaAcessoWeb(ByVal username_login As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select nvl(trunc(bloqueio_web) - trunc(sysdate),'9') as result from usuarios where upper(login_usuario)='" & UCase(username_login) & "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                If reader.Item("result").ToString <= 0 Then
                    Return True
                Else
                    Return False
                End If
            End While
        End Using

    End Function

    Public Function ExpirouSenha(ByVal username_login As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of String)

        Dim strSQL As String = "select nvl(trunc(expiracao_senha_web) - trunc(sysdate),'0') as result from usuarios where upper(login_usuario)='" & UCase(username_login) & "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                If reader.Item("result").ToString <= 0 Then
                    Return True
                Else
                    Return False
                End If
            End While
        End Using
    End Function

    Function SetDataTable_To_CSV(ByVal dtable As DataTable, ByVal sep_char As String) As String
        Dim aux_string As String = ""

        Try

            Dim _sep As String = ""
            Dim builder As New System.Text.StringBuilder
            For Each col As DataColumn In dtable.Columns
                builder.Append(_sep).Append(col.ColumnName)
                _sep = sep_char
            Next
            aux_string = aux_string + builder.ToString() & vbNewLine

            For Each row As DataRow In dtable.Rows
                _sep = ""
                builder = New System.Text.StringBuilder

                For Each col As DataColumn In dtable.Columns
                    builder.Append(_sep).Append(row(col.ColumnName))
                    _sep = sep_char
                Next
                aux_string = aux_string + builder.ToString() & vbNewLine

            Next
        Catch ex As Exception

        Finally
        End Try

        Return aux_string
    End Function
    Public Shared Function ConvertToDataTable(Of T)(ByVal list As IList(Of T)) As DataTable
        Dim table As New DataTable("myQuery")
        Dim fields() As FieldInfo = GetType(T).GetFields(BindingFlags.Public Or BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.NonPublic)
        For Each field As FieldInfo In fields
            'table.Columns.Add(field.Name, field.FieldType)
            table.Columns.Add(field.Name)
        Next
        For Each item As T In list
            Dim row As DataRow = table.NewRow()
            For Each field As FieldInfo In fields
                row(field.Name) = field.GetValue(item)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function Is_Reader_HasRows(ByVal query As String, ByVal conexao As String) As Boolean
        Dim connection As New OleDbConnection(conexao)

        Dim strSQL As String = query

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            If reader.HasRows Then
                Return True
            End If
        End Using

        Return False
    End Function

    Public Function Get_Ramais_List_User(ByVal code_user As String, ByVal pBuscaPorNome As Boolean) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of String)

        Dim strSQL As String = "select p1.numero_a ramal  from ramais p1 where p1.numero_a in "
        If pBuscaPorNome Then
            strSQL = strSQL + " (select rml_numero_a from usuarios where upper(nome_usuario) in (select upper(nome_usuario) from usuarios where codigo='" + code_user + "')) "
        Else
            strSQL = strSQL + " (select rml_numero_a from usuarios where codigo='" + code_user + "') "
        End If


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _list.Add(reader.Item("ramal").ToString)
            End While
        End Using

        Return _list
    End Function

    Public Function GenericInsert(ByVal _data As AppGeneric, ByVal date_log As String, ByVal user As String, ByVal code_field As String, ByVal description_field As String, ByVal Table As String, Optional ByVal Table_log As String = "", Optional ByVal extra_fields As List(Of AppGeneric) = Nothing) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        If extra_fields Is Nothing Then
            extra_fields = New List(Of AppGeneric)
        End If

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction



            Dim strSQL As String = " insert into " & Table & "(" & code_field & "," & description_field & ""
            For Each item As AppGeneric In extra_fields
                strSQL = strSQL + " ," & item.Codigo & ""
            Next
            strSQL = strSQL + ")"
            If _data.Codigo <> "" Then
                strSQL = strSQL + " values ('" & _data.Codigo & "' , '" & _data.Descricao & "'"
            Else
                strSQL = strSQL + " values ((select nvl(max(" & code_field & "),0)+1 from " & Table & ") , '" & _data.Descricao & "'"
            End If
            For Each item As AppGeneric In extra_fields
                If item.Codigo = "DATA" Then
                    strSQL = strSQL + " ,to_date('" & item.Descricao & "','DD/MM/YYYY')"
                Else
                    strSQL = strSQL + " ,'" & item.Descricao & "'"
                End If
            Next
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If Table_log <> "" Then 'Tabela log diferente de vazia = Insere na tabela Log

                strSQL = " insert into " & Table_log & "( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, " & code_field & "," & description_field & ""
                For Each item As AppGeneric In extra_fields
                    strSQL = strSQL + " ," & item.Codigo & ""
                Next
                strSQL = strSQL + ")"
                strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from " & Table_log & "),'N',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "',(select max(" & code_field & ") from " & Table & "),'" & _data.Descricao & "'"
                For Each item As AppGeneric In extra_fields
                    If item.Codigo = "DATA" Then
                        strSQL = strSQL + " ,to_date('" & item.Descricao & "','DD/MM/YYYY')"
                    Else
                        strSQL = strSQL + " ,'" & item.Descricao & "'"
                    End If
                Next
                strSQL = strSQL + ")"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try


        Return True
    End Function

    Public Function GenericUpdate(ByVal _data As AppGeneric, ByVal date_log As String, ByVal user As String, ByVal code_field As String, ByVal description_field As String, ByVal Table As String, Optional ByVal Table_log As String = "", Optional ByVal extra_fields As List(Of AppGeneric) = Nothing, Optional ByVal old_code As String = "") As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        If extra_fields Is Nothing Then
            extra_fields = New List(Of AppGeneric)
        End If

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            If Table_log <> "" Then 'Tabela log diferente de vazia = Insere na tabela Log

                strSQL = " insert into " & Table_log & "( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG,"
                strSQL = strSQL + " " & code_field & "," & description_field & ""
                For Each item As AppGeneric In extra_fields
                    strSQL = strSQL + " ," & item.Codigo & ""
                Next
                strSQL = strSQL + ")"
                strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from " & Table_log & "),'A',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "'"
                strSQL = strSQL + " ,'" & IIf(old_code <> "", old_code, _data.Codigo) & "',(select " & description_field & " from " & Table & " where " & code_field & "='" & _data.Codigo & "')"
                For Each item As AppGeneric In extra_fields
                    strSQL = strSQL + " ,(select " & item.Codigo & " from " & Table & " where " & code_field & "='" & _data.Codigo & "')"
                Next
                strSQL = strSQL + ")"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            '*************************************************************************************
            If _data.Codigo = "" Then
                _data.Codigo = "(select nvl(max(" & code_field & "),0) from " & Table & ")"
            End If

            strSQL = " update " & Table & " set " & code_field & "=" & _data.Codigo & ", " & description_field & "='" & _data.Descricao & "' "

            For Each item As AppGeneric In extra_fields
                If item.Codigo = "DATA" Then
                    strSQL = strSQL + " ," & item.Codigo & "= to_date('" & item.Descricao & "','DD/MM/YYYY')"
                Else
                    strSQL = strSQL + " ," & item.Codigo & "='" & item.Descricao & "'"

                End If
            Next

            If _data.Codigo = "" Then
                strSQL = strSQL + " where " & code_field & "=(select nvl(max(" & code_field & "),0) from " & Table & ")"
            Else
                strSQL = strSQL + " where " & code_field & "='" & IIf(old_code <> "", old_code, _data.Codigo) & "'"
            End If

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************
            If Table_log <> "" Then

                strSQL = " insert into " & Table_log & "( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG, "
                strSQL = strSQL + " " & code_field & "," & description_field & ""
                For Each item As AppGeneric In extra_fields
                    strSQL = strSQL + " ," & item.Codigo & ""
                Next
                strSQL = strSQL + ")"
                strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from " & Table_log & "),'B',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "'"
                strSQL = strSQL + " ,'" & _data.Codigo & "','" & _data.Descricao & "'"
                For Each item As AppGeneric In extra_fields
                    If item.Codigo = "DATA" Then
                        strSQL = strSQL + " ,to_date('" & item.Descricao & "','DD/MM/YYYY')"
                    Else
                        strSQL = strSQL + " ,'" & item.Descricao & "'"
                    End If
                Next
                strSQL = strSQL + ")"

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()
            End If

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception


            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try


        Return True

    End Function

    Public Function GenericRemove(ByVal _code As String, ByVal date_log As String, ByVal user As String, ByVal code_field As String, ByVal description_field As String, ByVal Table As String, Optional ByVal Table_log As String = "", Optional ByVal extra_fields As List(Of AppGeneric) = Nothing) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        If extra_fields Is Nothing Then
            extra_fields = New List(Of AppGeneric)
        End If

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            Dim strSQL As String = " insert into " & Table_log & "( CODIGO_LOG, TIPO_LOG, DATA_LOG, USUARIO_LOG,"
            strSQL = strSQL + " " & code_field & "," & description_field & ""
            For Each item As AppGeneric In extra_fields
                strSQL = strSQL + " ," & item.Codigo & ""
            Next
            strSQL = strSQL + ")"
            strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from " & Table_log & "),'D',to_date('" & date_log & "','dd/mm/yyyy hh24:mi:ss'),'" & user & "'"
            strSQL = strSQL + " ,'" & _code & "',(select " & description_field & " from " & Table & " where " & code_field & "='" & _code & "')"
            For Each item As AppGeneric In extra_fields
                strSQL = strSQL + " ,(select " & item.Codigo & " from " & Table & " where " & code_field & "='" & _code & "')"
            Next
            strSQL = strSQL + ")"

            If Table_log <> "" Then

                cmd.CommandText = strSQL
                cmd.ExecuteNonQuery()

            End If

            '**********************************************************************************************

            strSQL = " delete " & Table & " where " & code_field & "='" & _code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            '*************************************************************************************

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try


        Return True

    End Function

    '******************************************** FUNÇÕES DE ARQUIVOS ************************************************

    Public Function InsertFiles(ByVal pcodigo As String, ByVal campo_codigo As String, ByVal tabela As String, ByVal File_Name As String, ByVal Bytes() As Byte, ByVal ByteNameField As String, ByVal FileNamefield As String, Optional data As String = "") As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = " Insert into " & tabela & " "
            strSQL = strSQL + " (" & campo_codigo & "," & FileNamefield & "," & ByteNameField & IIf(data <> "", ",DATA", "") & ")"
            If pcodigo = "" Then
                strSQL = strSQL + " values ((select nvl(max(" & campo_codigo & "),0)+1 from " & tabela & "),:sName,:sFile " & IIf(data <> "", ",to_date('" + data + "','dd/mm/yyyy hh24:mi:ss')", "") & ")"
            Else
                strSQL = strSQL + " values (" + pcodigo + ",:sName,:sFile " & IIf(data <> "", ",to_date('" + data + "','dd/mm/yyyy hh24:mi:ss')", "") & ")"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            cmd.Parameters.Add(":sName", OleDbType.VarChar).Value = File_Name
            cmd.Parameters.Add(":sFile", OleDbType.LongVarBinary).Value = Bytes
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function


    Public Function UpdateFiles(ByVal pcodigo As String, ByVal campo_codigo As String, ByVal tabela As String, ByVal File_Name As String, ByVal Bytes() As Byte, ByVal ByteNameField As String, ByVal FileNamefield As String, Optional data As String = "") As Boolean
        Dim connection As New OleDbConnection(strConn)

        If pcodigo = "" Then
            pcodigo = (Convert.ToInt16(GetMaximumCode("codigo_termo", "TERMOS_RESPONSABILIDADE")) - 1).ToString
        End If

        Try
            Dim strSQL As String = " Update " & tabela & ""
            strSQL = strSQL + " set " & FileNamefield & "= :sName, " & ByteNameField & " = :sFile"
            strSQL = strSQL + " where " & campo_codigo & "=" & pcodigo

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            cmd.Parameters.Add(":sName", OleDbType.VarChar).Value = File_Name
            cmd.Parameters.Add(":sFile", OleDbType.LongVarBinary).Value = Bytes
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function


    Public Function UpdateFileField(ByVal pcodigo As String, ByVal campo_codigo As String, ByVal tabela As String, ByVal Bytes() As Byte, ByVal ByteNameField As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = " Update " & tabela & ""
            strSQL = strSQL + " set " & ByteNameField & "=:sFile"
            strSQL = strSQL + " where " & campo_codigo & "='" & pcodigo & "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            cmd.Parameters.Add(":sFile", OleDbType.LongVarBinary).Value = Bytes
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function DeleteFiles(ByVal pcodigo As String, ByVal pcodigo_campo As String, ByVal tabela As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete " & tabela & " "
            strSQL = strSQL + "where " & pcodigo_campo & " = " + pcodigo

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetFiles(ByVal pcodigo As String, ByVal pcodigo_campo As String, ByVal tabela As String, ByRef File_Names As List(Of String), ByRef _bytes As List(Of Byte()), Optional data As Boolean = False, Optional ByRef data_aux As List(Of String) = Nothing) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select FILE_NAME, BYTES " & IIf(data = True, ", DATA", "")
        strSQL = strSQL + " from " & tabela & " "
        strSQL = strSQL + " WHERE " & pcodigo_campo & "='" + pcodigo + "'"

        If File_Names.Count Then
            For Each name As String In File_Names
                strSQL = strSQL + " and FILE_NAME <> '" + name + "'"
            Next
        End If

        strSQL = strSQL + IIf(data = True, " order by data desc", "")

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                File_Names.Add(reader.Item("FILE_NAME").ToString())
                If data = True Then
                    data_aux.Add(reader.Item("DATA").ToString())
                End If
                _bytes.Add(reader.Item("BYTES"))
                Return True
            End While
        End Using

        Return False
    End Function

    Public Function GetBytesByField(ByVal pcodigo As String, ByVal pcodigo_campo As String, ByVal tabela As String, ByVal field As String, ByRef _bytes As List(Of Byte())) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select " & field
        strSQL = strSQL + " from " & tabela & " "
        strSQL = strSQL + " WHERE " & pcodigo_campo & "='" + pcodigo + "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _bytes.Add(reader.Item(field))
                Return True
            End While
        End Using

        Return False
    End Function

    Public Function GetFileParam(ByVal codigo As Integer, ByVal File_Name As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select BYTES "
        strSQL = strSQL + " from LINKS_FILES "
        strSQL = strSQL + " WHERE CODIGO_LINK='" + codigo.ToString + "'"
        strSQL = strSQL + " AND FILE_NAME='" + File_Name + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Return (reader.Item("BYTES"))
            End While
        End Using
        Return ("")
    End Function

    Public Function GetGoogleCode() As String
        Dim googleKey As String = ""
        Dim strScript As String = ""
        Dim dt As DataTable = myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='GOOGLEKEY'")
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

            'System.Web.HttpContext.Current.Response.Write(strScript)

        End If
        Return strScript
    End Function
   
    Public Sub EscreveLog(ByVal pMSG As String)
        Try


            Dim log As IO.StreamWriter
            'Dim caminhoLog As String = Application.StartupPath & ConfigurationManager.AppSettings("nomeArquivo").ToString
            Dim caminhoLog As String = AppDomain.CurrentDomain.BaseDirectory + "logGlobal.txt"
            If Not IO.File.Exists(caminhoLog) Then
                log = IO.File.CreateText(caminhoLog)
                log.WriteLine(Date.Now + "-" + pMSG)
            Else

                log = New IO.StreamWriter(caminhoLog, True, System.Text.Encoding.UTF8)
                log.WriteLine(Date.Now + "-" + pMSG)

            End If
            log.Close()
            log.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Function ConvertDataTable_ToHtmlFile(ByVal targetTable As DataTable) As String
        Dim myHtmlFile As String = ""


        If (targetTable Is Nothing) Then
            Throw New System.ArgumentNullException("targetTable")
        Else
            'Continue.
        End If

        'Get a worker object.
        Dim myBuilder As System.Text.StringBuilder = New System.Text.StringBuilder()

        'Open tags and write the top portion.
        myBuilder.Append("<html xmlns='http://www.w3.org/1999/xhtml'>")
        myBuilder.Append("<head>")
        myBuilder.Append("<title>")
        myBuilder.Append("Page-")
        myBuilder.Append(Guid.NewGuid().ToString())
        myBuilder.Append("</title>")
        myBuilder.Append("</head>")
        myBuilder.Append("<body>")
        myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ")
        myBuilder.Append("style='border: solid 1px Silver; font-size: x-small;font-family: verdana;'>")

        'Add the headings row.

        myBuilder.Append("<tr align='left' valign='top' style='background-color: rgb(206, 206, 206);'>")

        For Each myColumn As DataColumn In targetTable.Columns
            myBuilder.Append("<td align='left' valign='top'><b>")
            myBuilder.Append(myColumn.ColumnName)
            myBuilder.Append("</b></td>")
        Next myColumn

        myBuilder.Append("</tr>")

        'Add the data rows.

        Dim count As Integer = 1
        For Each myRow As DataRow In targetTable.Rows

            If count Mod 2 > 0 Then
                myBuilder.Append("<tr align='left' valign='top'>")
            Else
                myBuilder.Append("<tr align='left' valign='top' style='background-color: rgb(206, 206, 206);'>")
            End If

            For Each myColumn As DataColumn In targetTable.Columns
                myBuilder.Append("<td align='left' valign='top'>")
                myBuilder.Append(myRow(myColumn.ColumnName).ToString())
                myBuilder.Append("</td>")
            Next myColumn

            myBuilder.Append("</tr>")

            count = count + 1

        Next myRow


        'Close tags.
        myBuilder.Append("</table>")
        myBuilder.Append("</body>")
        myBuilder.Append("</html>")


        'Get the string for return.
        myHtmlFile = myBuilder.ToString()


        Return myHtmlFile
    End Function

    Public Function InsereAgendamentos(ByVal Codigo_fatura As String, ByVal usuario As String, ByVal titulo As String, ByVal tipo As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction



            'INSERE NO AGENDAMENTO
            strSQL = "insert into gestao_agendamentos_tarefas (codigo,data,descricao,autor,status,cod_tarefa) values ((select nvl(max(codigo),0)+1 from gestao_agendamentos_tarefas),sysdate,'" & titulo & "','" & usuario & "','0','" & tipo & "') "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into gestao_tarefas_faturas (codigo_tarefa,codigo_fatura,ativo) values ((select nvl(max(codigo),0) from gestao_agendamentos_tarefas),'" & Codigo_fatura & "','S') "
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        Return True

    End Function

    Public Function ConvertGridtoDT(dtg As GridView) As DataTable
        Dim dt As New DataTable()
        Dim aux As Integer = 0

        ' add the columns to the datatable            
        If dtg.HeaderRow IsNot Nothing Then

            For i As Integer = 0 To dtg.HeaderRow.Cells.Count - 1
                dt.Columns.Add(IIf(dtg.HeaderRow.Cells(i).Text = "&nbsp;", aux.ToString, dtg.HeaderRow.Cells(i).Text))
                aux = aux + 1
            Next

        End If

        '  add each of the data rows to the table
        For Each row As GridViewRow In dtg.Rows
            Dim dr As DataRow
            dr = dt.NewRow()

            For i As Integer = 0 To row.Cells.Count - 1
                dr(i) = row.Cells(i).Text.Replace(" ", "")
            Next
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function


    Public Function ReturnGridFilters(ByVal campo_codigo As String, ByVal campo As String, ByVal tabela As String, Optional end_query As String = "") As String
        Dim connection As New OleDbConnection(strConn)
        Dim list As String = ""

        Dim strSQL As String = "select distinct " + campo_codigo + " as codigo , " + campo + " as descricao"
        strSQL = strSQL + " FROM " + tabela + " WHERE 1=1 "

        If end_query <> "" Then
            strSQL = strSQL + end_query
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                list = list & ";" & reader.Item("descricao").ToString & ":" & reader.Item("descricao").ToString
            End While
        End Using

        connection.Close()

        Return list
    End Function

End Class
