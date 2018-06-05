Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_TarifasMoveis
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

    Public Function Get_Categoria_TarifMovel() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppGeneric)

        Dim strSQL As String = "select distinct p1.codigo as codigo, '[' || p2.descricao || '] ' ||  p1.nome_configuracao as descricao  "
        strSQL = strSQL + " from tarifacao p1, operadoras_teste p2 "
        strSQL = strSQL + " where p1.OPER_CODIGO_OPERADORA=p2.codigo(+) and tipo_tarifa in (1) order by descricao "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _list.Add(New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString))
            End While
        End Using

        Return _list
    End Function

    Public Function Get_Areas_Disponiveis() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppGeneric)

        Dim strSQL As String = "select P1.codigo, (P1.codigo || ' - ' || UPPER(P1.descricao)) descricao  "
        strSQL = strSQL + " from codigo_area P1  WHERE operadora = 0 ORDER BY P1.CODIGO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _list.Add(New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString))
            End While
        End Using

        Return _list
    End Function

    Public Function Get_CodigosDDI(Optional ByVal codigo As String = "", Optional ByVal areas As String = "", Optional codigo_tipo_ligacao As String = "") As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppGeneric)

        Dim strSQL As String = " select p1.codigo_ddi codigo,(P1.codigo_ddi || ' - ' || nvl(p1.descricao,' '))descricao from  "
        'strSQL = strSQL + " codigo_ddi p1 where 1=1 and not exists( select 0 from "
        strSQL = strSQL + " codigo_ddi p1 where 1=1  "

        'strSQL = strSQL + " gestao_ddi p10 where p10.CODIGO_DDI=p1.codigo_ddi "
        'If codigo <> "" Then
        '    strSQL = strSQL + " and p10.codigo_operadora=(select nvl(OPER_CODIGO_OPERADORA,-1)OPER_CODIGO_OPERADORA from tarifacao p1 where p1.codigo='" + codigo + "') "
        '    'strSQL = strSQL + " and  "
        'End If
        'strSQL = strSQL + " and p10.TIPO_NUMERO_B='0' "
        'If areas <> "" Then
        '    strSQL = strSQL + " and p10.codigo_area in (" & areas & ")  "
        'End If
        'strSQL = strSQL + " ) "

        If codigo_tipo_ligacao <> "" Then
            strSQL += " union select p1.codigo_ddi codigo,(P1.codigo_ddi || ' - ' || nvl(p1.descricao,' '))descricao from  "
            strSQL = strSQL + " codigo_ddi p1,GESTAO_DDI t where p1.codigo_ddi=t.codigo_ddi and t.codigo_tipo_ligacao='" & codigo_tipo_ligacao & "'"
            'strSQL = strSQL + " order by descricao"
        End If

        strSQL += " order by descricao"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                _list.Add(New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString))
            End While
        End Using

        Return _list
    End Function

    Public Function InsereTarifaMovel(ByVal tarifaMovel As AppTarifasMoveis, ByVal usuario As String, ByVal list_paises As List(Of String), ByVal list_areas As List(Of String)) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim _dao_Commons As New DAO_Commons
        _dao_Commons.strConn = strConn

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim operadora As String = _dao_Commons.myDataTable("select t.oper_codigo_operadora from TARIFACAO t where t.codigo='" & tarifaMovel.Codigo_tarifa & "'").Rows(0).Item(0)

            

            sql = "insert into tipos_ligacao_teste (codigo,descricao,ttm "
            sql = sql + " ,valor_ttm,step,valor_step,tipo_chamada,CODIGO_OPERADORA_DESTINO,GESTAO_DDI,COMPLEMENTO,codigo_tarif)  "
            sql = sql + " values ( (select nvl(max(codigo),0)+1 from tipos_ligacao_teste), "
            sql = sql + "'" + tarifaMovel.Descricao + "',"
            sql = sql + "'" + tarifaMovel.TTM + "',"
            sql = sql + "to_number('" + Replace(tarifaMovel.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifaMovel.Step_ + "',"
            sql = sql + "to_number('" + Replace(tarifaMovel.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifaMovel.TipoChamada + "',"
            sql = sql + "'" + tarifaMovel.Operadora + "',"
            sql = sql + "'" + tarifaMovel.GestaoDDI + "',"
            sql = sql + "'" + tarifaMovel.Complemento + "',"
            sql = sql + "'" + tarifaMovel.Codigo_tarifa + "')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log("", "N", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            'DDI
            If list_areas.Count > 0 And list_paises.Count > 0 Then

                'cmd.CommandText = sql
                'cmd.ExecuteNonQuery()

                For Each area As String In list_areas
                    For Each paises As String In list_paises
                        'sql = "select p2.descricao from  gestao_ddi p1, tipos_ligacao_teste p2 where p1.CODIGO_TIPO_LIGACAO=p2.codigo and p1.CODIGO_AREA='" + CStr(area) + "' and p1.CODIGO_DDI='" + CStr(paises) + "' and p1.CODIGO_OPERADORA='" + CStr(tarifaMovel.Operadora) + "' and p1.TIPO_NUMERO_B='" + CStr(tarifaMovel.TipoChamada) + "'"
                        'delete se ja existe antes de incluir
                        sql = "delete from  gestao_ddi p1 where p1.CODIGO_AREA='" + CStr(area) + "' and p1.CODIGO_DDI='" + CStr(paises) + "' and p1.CODIGO_OPERADORA='" + CStr(operadora) + "' and p1.TIPO_NUMERO_B='" + CStr(tarifaMovel.TipoChamada) + "'"
                        cmd.CommandText = sql
                        cmd.ExecuteNonQuery()

                        'insere
                        sql = "insert into gestao_ddi(CODIGO_TIPO_LIGACAO,CODIGO_AREA,CODIGO_DDI,CODIGO_OPERADORA,TIPO_NUMERO_B) values((select nvl(max(codigo),0) from tipos_ligacao_teste),'" + CStr(area) + "','" + CStr(paises) + "','" + CStr(operadora) + "','" + CStr(tarifaMovel.TipoChamada) + "')"
                        cmd.CommandText = sql
                        cmd.ExecuteNonQuery()

                    Next
                Next

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

    Public Function AtualizaTarifaMovel(ByVal tarifaMovel As AppTarifasMoveis, ByVal usuario As String, ByVal list_paises As List(Of String), ByVal list_areas As List(Of String)) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim _dao_Commons As New DAO_Commons
        _dao_Commons.strConn = strConn

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            Dim operadora As String = _dao_Commons.myDataTable("select t.oper_codigo_operadora from TARIFACAO t where t.codigo='" & tarifaMovel.Codigo_tarifa & "'").Rows(0).Item(0)

            sql = String_log(tarifaMovel.Codigo, "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update tipos_ligacao_teste set "
            sql = sql + " descricao='" + tarifaMovel.Descricao + "',"
            sql = sql + " ttm='" + tarifaMovel.TTM + "',"
            sql = sql + " valor_ttm= to_number('" + Replace(tarifaMovel.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " step='" + tarifaMovel.Step_ + "',"
            sql = sql + " valor_step= to_number('" + Replace(tarifaMovel.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " tipo_chamada='" + tarifaMovel.TipoChamada + "',"
            sql = sql + " CODIGO_OPERADORA_DESTINO='" + tarifaMovel.Operadora + "',"
            sql = sql + " GESTAO_DDI='" + tarifaMovel.GestaoDDI + "',"
            sql = sql + " COMPLEMENTO='" + tarifaMovel.Complemento + "',"
            sql = sql + " codigo_tarif='" + tarifaMovel.Codigo_tarifa + "'"
            sql = sql + " where CODIGO = '" + tarifaMovel.Codigo.ToString + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(tarifaMovel.Codigo, "B", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            'DDI
            If list_areas.Count > 0 And list_paises.Count > 0 Then

                'cmd.CommandText = sql
                'cmd.ExecuteNonQuery()
                sql = "delete from  gestao_ddi p1 where p1.codigo_tipo_ligacao='" + CStr(tarifaMovel.Codigo.ToString) + "'"
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

                For Each area As String In list_areas
                    For Each paises As String In list_paises
                        'sql = "select p2.descricao from  gestao_ddi p1, tipos_ligacao_teste p2 where p1.CODIGO_TIPO_LIGACAO=p2.codigo and p1.CODIGO_AREA='" + CStr(area) + "' and p1.CODIGO_DDI='" + CStr(paises) + "' and p1.CODIGO_OPERADORA='" + CStr(tarifaMovel.Operadora) + "' and p1.TIPO_NUMERO_B='" + CStr(tarifaMovel.TipoChamada) + "'"
                        'delete se ja existe antes de incluir
                       

                        'insere
                        sql = "insert into gestao_ddi(CODIGO_TIPO_LIGACAO,CODIGO_AREA,CODIGO_DDI,CODIGO_OPERADORA,TIPO_NUMERO_B) values('" + CStr(tarifaMovel.Codigo.ToString) + "','" + CStr(area) + "','" + CStr(paises) + "','" + CStr(operadora) + "','" + CStr(tarifaMovel.TipoChamada) + "')"
                        cmd.CommandText = sql
                        cmd.ExecuteNonQuery()

                    Next
                Next

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

    Public Function ExcluiTarifaMovel(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(pcodigo, "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete tipos_ligacao_teste "
            sql = sql + "where codigo = " + pcodigo

            cmd.CommandText = sql
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

    End Function


    Public Function GetTarifaMovel(ByVal pcodigo As Integer) As List(Of AppTarifasMoveis)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifasMoveis)

        Dim strSQL As String = "select t.CODIGO"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.TTM, '') AS TTM"
        strSQL = strSQL + ", nvl(t.VALOR_TTM, 0) AS VALOR_TTM"
        strSQL = strSQL + ", nvl(t.STEP, '') AS STEP"
        strSQL = strSQL + ", nvl(t.VALOR_STEP, 0) AS VALOR_STEP"
        strSQL = strSQL + ", nvl(t.CODIGO_OPERADORA_DESTINO, 0) AS CODIGO_OPERADORA_DESTINO"
        strSQL = strSQL + ", nvl(t.CODIGO_TARIF, 0) AS CODIGO_TARIF"
        strSQL = strSQL + ", nvl(t.COMPLEMENTO, '') AS COMPLEMENTO"
        strSQL = strSQL + ", nvl(t.TIPO_CHAMADA, 0) AS TIPO_CHAMADA"
        strSQL = strSQL + ", nvl(t.GESTAO_DDI, 'N') AS GESTAO_DDI"
        strSQL = strSQL + " from tipos_ligacao_teste t where codigo='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifasMoveis

                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.TTM = reader.Item("TTM").ToString
                _registro.TTM_Value = reader.Item("VALOR_TTM").ToString
                _registro.Step_ = reader.Item("STEP").ToString
                _registro.Step_value = reader.Item("VALOR_STEP").ToString
                _registro.Operadora = reader.Item("CODIGO_OPERADORA_DESTINO").ToString
                _registro.Codigo_tarifa = reader.Item("CODIGO_TARIF").ToString
                _registro.Complemento = reader.Item("COMPLEMENTO").ToString
                _registro.TipoChamada = reader.Item("TIPO_CHAMADA").ToString
                _registro.GestaoDDI = reader.Item("GESTAO_DDI").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into tipos_ligacao_log (codigo_log, usuario_log, data_log, tipo_log, codigo_tipo_ligacao, descricao,ttm "
        sql = sql + " ,valor_ttm,step,valor_step,tipo_chamada,COMPLEMENTO,CODIGO_OPERADORA_DESTINO,GESTAO_DDI,codigo_tarifa)  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from tipos_ligacao_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + " '" + pcodigo + "', "
            sql = sql + " (select descricao from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select TTM from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select VALOR_TTM from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select STEP from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select VALOR_STEP from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select TIPO_CHAMADA from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select COMPLEMENTO from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select CODIGO_OPERADORA_DESTINO from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select GESTAO_DDI from tipos_ligacao_teste where codigo='" + pcodigo + "'),"
            sql = sql + " (select CODIGO_TARIF from tipos_ligacao_teste where codigo='" + pcodigo + "'))"
        Else
            sql = sql + " (select nvl(max(codigo),0) from tipos_ligacao_teste),"
            sql = sql + " (select descricao from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select TTM from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select VALOR_TTM from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select STEP from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select VALOR_STEP from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select TIPO_CHAMADA from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select COMPLEMENTO from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select CODIGO_OPERADORA_DESTINO from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select GESTAO_DDI from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)),"
            sql = sql + " (select CODIGO_TARIF from tipos_ligacao_teste where codigo=(select nvl(max(codigo),0) from tipos_ligacao_teste)))"
        End If
        Return sql

    End Function
End Class
