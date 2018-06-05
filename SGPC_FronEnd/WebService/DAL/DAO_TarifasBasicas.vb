Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic
'Imports ADOX

Public Class DAO_TarifasBasicas
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

    Public Function getComboTarifas() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim strSQL As String = ""
        Dim list As New List(Of AppGeneric)

        strSQL = " "
        strSQL = strSQL + " select distinct p1.codigo,"
        strSQL = strSQL + " p1.descricao,"
        strSQL = strSQL + " nvl(p1.complemento, ' ') complemento,"
        strSQL = strSQL + " p2.OPER_CODIGO_OPERADORA"
        strSQL = strSQL + " from tipos_ligacao_teste p1, tarifacao p2"
        strSQL = strSQL + " where(p1.CODIGO_TARIF = p2.codigo)"
        strSQL = strSQL + " and p2.tipo_tarifa in ('0')"
        strSQL = strSQL + " order by descricao"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list

    End Function

    Public Function InsereTarifaBasica(ByVal tarifaBasica As AppTarifaBasica, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "insert into TARIFAS_BASICAS_PRATICADAS (codigo_basico,descricao,ttm "
            sql = sql + " ,valor_ttm,step,valor_step,CODIGO_OPERADORA,CODIGO_TIPO_LIGACAO,FLAG_PROTECAO)  "
            sql = sql + " values ( (select nvl(max(codigo_basico),0)+1 from TARIFAS_BASICAS_PRATICADAS), "
            sql = sql + "'" + tarifaBasica.Descricao + "',"
            sql = sql + "'" + tarifaBasica.TTM + "',"
            sql = sql + "to_number('" + Replace(tarifaBasica.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifaBasica.Step_ + "',"
            sql = sql + "to_number('" + Replace(tarifaBasica.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + "'" + tarifaBasica.Codigo_operadora + "',"
            sql = sql + "'" + tarifaBasica.Codigo_tipo_ligacao + "',"
            sql = sql + "'N')"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(tarifaBasica, "N", usuario)

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

        Return True

    End Function

    Public Function AtualizaTarifaBasica(ByVal tarifa As AppTarifaBasica, ByVal usuario As String) As Boolean

        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifaBasicaById(tarifa.Codigo).Item(0), "A", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " update TARIFAS_BASICAS_PRATICADAS set "
            sql = sql + " descricao='" + tarifa.Descricao + "',"
            sql = sql + " ttm='" + tarifa.TTM + "',"
            sql = sql + " valor_ttm= to_number('" + Replace(tarifa.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " step='" + tarifa.Step_ + "',"
            sql = sql + " valor_step= to_number('" + Replace(tarifa.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
            sql = sql + " CODIGO_OPERADORA='" + tarifa.Codigo_operadora + "',"
            'sql = sql + " FLAG_PROTECAO='N',"
            sql = sql + " CODIGO_TIPO_LIGACAO='" + tarifa.Codigo_tipo_ligacao + "'"
            sql = sql + " where CODIGO_BASICO = '" + tarifa.Codigo.ToString + "' "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = String_log(GetTarifaBasicaById(tarifa.Codigo).Item(0), "B", usuario)

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

        Return True

    End Function

    Public Function ExcluiTarifaBasica(ByVal pcodigo As String, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = String_log(GetTarifaBasicaById(pcodigo).Item(0), "D", usuario)

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = " delete TARIFAS_BASICAS_PRATICADAS "
            sql = sql + "where codigo_basico = " + pcodigo

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

    Public Function ReturnTotalNumeroA(ByVal pcodigo As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim total As String = ""

        Try
            Dim strSQL As String = " select count(*) as total from("
            strSQL = strSQL + " select distinct rml_numero_a "
            strSQL = strSQL + " from(cdrs_celular_analitico_mv) "
            strSQL = strSQL + " where codigo_conta in "
            strSQL = strSQL + " (select codigo_conta from faturas_arquivos where codigo_fatura = '" + pcodigo + "')) "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    total = reader.Item("total").ToString
                End While
            End Using

            Return total

        Catch ex As Exception
            Return total
        End Try
    End Function

    Public Function GetTarifaBasicaById(ByVal pcodigo As Integer) As List(Of AppTarifaBasica)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppTarifaBasica)

        Dim strSQL As String = "select t.CODIGO_BASICO"
        strSQL = strSQL + ", nvl(t.descricao, '') AS descricao"
        strSQL = strSQL + ", nvl(t.TTM, '') AS TTM"
        strSQL = strSQL + ", nvl(t.VALOR_TTM, 0) AS VALOR_TTM"
        strSQL = strSQL + ", nvl(t.STEP, '') AS STEP"
        strSQL = strSQL + ", nvl(t.VALOR_STEP, 0) AS VALOR_STEP"
        strSQL = strSQL + ", nvl(t.CODIGO_OPERADORA, 0) AS CODIGO_OPERADORA"
        strSQL = strSQL + ", nvl(t.CODIGO_TIPO_LIGACAO, 0) AS CODIGO_TIPO_LIGACAO"
        strSQL = strSQL + " from TARIFAS_BASICAS_PRATICADAS t where codigo_basico='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppTarifaBasica

                _registro.Codigo = reader.Item("CODIGO_BASICO").ToString
                _registro.Descricao = reader.Item("DESCRICAO").ToString
                _registro.TTM = reader.Item("TTM").ToString
                _registro.TTM_Value = reader.Item("VALOR_TTM").ToString
                _registro.Step_ = reader.Item("STEP").ToString
                _registro.Step_value = reader.Item("VALOR_STEP").ToString
                _registro.Codigo_operadora = reader.Item("CODIGO_OPERADORA").ToString
                _registro.Codigo_tipo_ligacao = reader.Item("CODIGO_TIPO_LIGACAO").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function


    Public Function String_log(ByVal registro As AppTarifaBasica, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into TARIFAS_BASICAS_LOG (codigo_log, usuario_log, data_log, tipo_log, codigo_basico,descricao,ttm "
        sql = sql + " ,valor_ttm,step,valor_step,CODIGO_OPERADORA,CODIGO_TIPO_LIGACAO,FLAG_PROTECAO) "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from TARIFAS_BASICAS_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + " '" & tipo_log & "',"
        sql = sql + " '" & registro.Codigo & "',"
        sql = sql + "'" + registro.Descricao + "',"
        sql = sql + "'" + registro.TTM + "',"
        sql = sql + "to_number('" + Replace(registro.TTM_Value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
        sql = sql + "'" + registro.Step_ + "',"
        sql = sql + "to_number('" + Replace(registro.Step_value, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
        sql = sql + "'" + registro.Codigo_operadora + "',"
        sql = sql + "'" + registro.Codigo_tipo_ligacao + "',"
        sql = sql + "'N')"
        Return sql

    End Function

    Public Function AtualizaNovosDDI(ByRef msg As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""
        Dim list_horarios_tarifacao_op As New List(Of AppHorario_Tarifacao_OP)

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'sql = ""
            'sql = sql + " select codigo_horario codigo from horarios_tarifacao_op_teste"
            'sql = sql + " where not codigo_ddi is null"

            'cmd.CommandText = sql
            'cmd.ExecuteNonQuery()

            'sql = "delete from tarifas_teste p10"
            'sql = sql + " where p10.ttm=-999 and p10.step=-999 and p10.valor_ttm=-999 and p10.valor_step=-999"

            'cmd.CommandText = sql
            'cmd.ExecuteNonQuery()

            sql = " delete from tarifas_teste p1"
            sql = sql + " where p1.codigo in ("
            sql = sql + " select tt.codigo from tarifas_teste tt,"
            sql = sql + " horarios_tarifacao_op_teste hop, "
            sql = sql + " horarios_tarifacao_teste h "
            sql = sql + "  where h.codigo_tarifa=tt.codigo and "
            sql = sql + " hop.codigo_horario=h.codigo and "
            sql = sql + " codigo_tipo_ligacao=3)"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = ""
            sql = sql + " delete from horarios_tarifacao_teste"
            sql = sql + "   where codigo in"
            sql = sql + "  (select codigo_horario"
            sql = sql + "  from horarios_tarifacao_op_teste"
            sql = sql + "  where codigo_tipo_ligacao=3) "

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            sql = "delete from horarios_tarifacao_op_teste where codigo_tipo_ligacao=3"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()

        Catch e As Exception
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        msg = msg + " todas as antigas foram apagadas<br>"

        'Inicia execução do procedimento de insert nas tabelas horario_tarifacao_op_teste, horario_tarifacao_op, tarifas_teste

        list_horarios_tarifacao_op = GetHorario_Tarifacao_OP()

        For Each item As AppHorario_Tarifacao_OP In list_horarios_tarifacao_op

            msg = msg + " operadora: " + item.Codigo_operadora.ToString + " codigo ddi: " + item.Codigo_DDI.ToString + " inicio<br>"

            'Traz os códigos correntes para inserções fora do loop pois o incremento precisa ser manual.(select só retorna valor+1 após o commit de um insert) 
            Dim Codigo_horario As Integer = Convert.ToInt16(GetMaximumCode_Horario_Tarif_OP()) + 1

            'Inserts de horario_tarifacao_op

            'If Verifica_integridade_OP(item.Codigo_operadora, item.Codigo_tipo_ligacao, item.Codigo_DDI) = False Then

            Try

                sql = " insert into horarios_tarifacao_op_teste"
                sql = sql + "(codigo_operadora,codigo_tipo_ligacao,codigo_horario,ativo,descricao,codigo_ddi) "
                sql = sql + " values ('" + item.Codigo_operadora.ToString + "',"
                sql = sql + " '" + item.Codigo_tipo_ligacao.ToString + "',"
                sql = sql + " '" + Codigo_horario.ToString + "',"
                sql = sql + " '" + item.Ativo.ToString + "',"
                sql = sql + " '" + item.Descricao.ToString + "',"
                sql = sql + " '" + item.Codigo_DDI.ToString + "')"

                item.Codigo_horario = Codigo_horario

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                cmd.Dispose()
                connection.Close()
                msg = msg + " ***<br>"
                msg = msg + " *** Dois DDIs iguais: " + item.Codigo_operadora.ToString + " para mesma Op: " + item.Codigo_DDI.ToString + "<br>"
                msg = msg + " ***<br>"
                Return False
            End Try

            Dim list_horario_tarif As New List(Of AppHorario_Tarifacao)
            Dim list_tarifas As New List(Of AppTarifas)

            'Chama a função que popula objetos de horario_tarifacao e tarifas

            GeraHorario_Tarifacao_e_Tarifas(item.Codigo_horario, item.Codigo_tipo_ligacao, item.Codigo_operadora, item.Codigo_DDI, list_tarifas, list_horario_tarif)

            'Inserts de horario tarifacao

            For Each horario_tarif As AppHorario_Tarifacao In list_horario_tarif

                Try
                    sql = " insert into horarios_tarifacao_teste"
                    sql = sql + "(horario,codigo,codigo_tarifa,tipo_tarifa) "
                    sql = sql + " values ('" + horario_tarif.Horario.ToString + "',"
                    sql = sql + " '" + horario_tarif.Codigo.ToString + "',"
                    sql = sql + " '" + horario_tarif.Codigo_tarifa.ToString + "',"
                    sql = sql + " '" + horario_tarif.Tipo_tarifa.ToString + "')"

                    cmd.CommandText = sql
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    cmd.Dispose()
                    connection.Close()
                    Return False
                End Try

            Next

            'Inserts de tarifas
            For Each tarifas As AppTarifas In list_tarifas

                Dim ttm_value As String = ""
                Dim step_value As String = ""

                If tarifas.TTM_Value.Length > 7 Then
                    ttm_value = tarifas.TTM_Value.Substring(0, 6)
                    step_value = tarifas.Step_value.Substring(0, 6)
                Else
                    ttm_value = tarifas.TTM_Value
                    step_value = tarifas.Step_value
                End If

                Try
                    sql = " insert into tarifas_teste"
                    sql = sql + "(codigo,ttm,valor_ttm,step,valor_step) "
                    sql = sql + " values ('" & tarifas.Codigo.ToString & "',"
                    sql = sql + " '" + tarifas.TTM.ToString & "',"
                    sql = sql + " to_number('" & Replace(ttm_value, ".", ",")
                    sql = sql + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''),"
                    sql = sql + " '" + tarifas.Step_.ToString & "',"
                    sql = sql + " to_number('" & Replace(step_value, ".", ",")
                    sql = sql + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''))"

                    cmd.CommandText = sql
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    cmd.Dispose()
                    connection.Close()
                    Return False
                End Try

            Next

            '+ Inserts de horario_tarifacao e tarifas

            Dim codigo_tarifa As String = Convert.ToInt16(GetMaximumCode_Tarifas()) + 1
            Try
                sql = " insert into horarios_tarifacao_teste"
                sql = sql + "(horario,codigo,codigo_tarifa,tipo_tarifa) "
                sql = sql + " values ('604800',"
                sql = sql + " '" & item.Codigo_horario.ToString & "',"
                sql = sql + " '" & codigo_tarifa.ToString & "',"
                sql = sql + " '4')"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                cmd.Dispose()
                connection.Close()
                Return False
            End Try

            Try

                sql = " insert into tarifas_teste"
                sql = sql + "(codigo,ttm,valor_ttm,step,valor_step) "
                sql = sql + " values ('" + codigo_tarifa.ToString + "',"
                sql = sql + " '0',"
                sql = sql + " '0',"
                sql = sql + " '0',"
                sql = sql + " '0')"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                cmd.Dispose()
                connection.Close()
                Return False
            End Try

            msg = msg + " codigo ddi: " + item.Codigo_DDI.ToString + " " + item.Descricao.ToString + " atualizada<br>"
        Next
        cmd.Dispose()
        connection.Close()
        msg = msg + " fim<br>"
        Return True
        'fim do insert 

    End Function

    Public Function GetHorario_Tarifacao_OP() As List(Of AppHorario_Tarifacao_OP)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppHorario_Tarifacao_OP)

        Dim sql As String = " select "
        sql = sql + "   p4.codigo codigo_operadora,"
        sql = sql + "   p5.codigo codigo_tipo_ligacao,"
        sql = sql + "   0 codigo_horario,"
        sql = sql + "   'S' ativo,"
        sql = sql + "   p3.descricao||'('||p1.descricao||')'descricao,"
        sql = sql + "   p1.codigo_ddi"
        sql = sql + " from "
        sql = sql + " codigo_ddi p1,"
        sql = sql + " codigoddi_basica p2,"
        sql = sql + " tarifas_basicas_praticadas p3,"
        sql = sql + " operadoras_teste p4,"
        sql = sql + " tipos_ligacao_teste p5"
        sql = sql + " where p1.codigo_ddi=p2.codigo_ddi"
        sql = sql + " and p3.codigo_basico=p2.codigo_basico"
        sql = sql + " and p3.codigo_operadora=p4.codigo"
        sql = sql + " and p3.codigo_tipo_ligacao=p5.codigo"
        sql = sql + " order by codigo_operadora,codigo_ddi"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppHorario_Tarifacao_OP

                _registro.Codigo_operadora = reader.Item("codigo_operadora").ToString
                _registro.Codigo_tipo_ligacao = reader.Item("codigo_tipo_ligacao").ToString

                _registro.Ativo = reader.Item("ativo").ToString
                _registro.Codigo_horario = reader.Item("codigo_horario").ToString
                _registro.Descricao = reader.Item("descricao").ToString
                _registro.Codigo_DDI = reader.Item("codigo_ddi").ToString

                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function Verifica_integridade_OP(ByVal codigo_operadora As String, ByVal codigo_tipo_ligacao As String, ByVal codigo_ddi As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim sql As String = "select codigo_operadora from horarios_tarifacao_op_teste"
        sql = sql + " where codigo_operadora='" + codigo_operadora + "'"
        sql = sql + " and codigo_tipo_ligacao='" + codigo_tipo_ligacao + "'"
        sql = sql + " and codigo_ddi='" + codigo_ddi + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return True
            End While
        End Using

        Return False

    End Function

    Public Sub GeraHorario_Tarifacao_e_Tarifas(ByVal codigo_horario As Integer, ByVal codigo_tipo_ligacao As String, ByVal codigo_operadora As String, ByVal codigo_ddi As String, ByRef list_tarifas As List(Of AppTarifas), ByRef list_hor_tar As List(Of AppHorario_Tarifacao))
        Dim connection As New OleDbConnection(strConn)
        Dim codigo_tarifa As Integer = Convert.ToInt16(GetMaximumCode_Tarifas() + 1)

        Dim sql As String = " select  "
        sql = sql + "   p4.codigo_ddi,"
        sql = sql + "   p1.horario_aplicacao horario,"
        sql = sql + "   p2.ttm,"
        sql = sql + "   p2.valor_ttm*p1.fator valor_ttm,"
        sql = sql + "   p2.step,"
        sql = sql + "   p2.valor_step*p1.fator valor_step"
        sql = sql + " from"
        sql = sql + " tarifas_derivadas_praticadas p1,"
        sql = sql + " tarifas_basicas_praticadas p2,"
        sql = sql + " basica_derivada p3,"
        sql = sql + " codigoddi_basica p4"
        sql = sql + " where p1.codigo_derivado=p3.codigo_derivado"
        sql = sql + " and p2.codigo_basico=p3.codigo_basico"
        sql = sql + " and p2.codigo_basico=p4.codigo_basico"
        sql = sql + " and p4.codigo_ddi='" + codigo_ddi + "'"
        sql = sql + " and p2.codigo_tipo_ligacao='" + codigo_tipo_ligacao + "'"
        sql = sql + " and p2.codigo_operadora='" + codigo_operadora + "'"
        sql = sql + " order by codigo_ddi,horario"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro_horario As New AppHorario_Tarifacao
                Dim _registro_tarifa As New AppTarifas

                'Popula lista de horarios tarifacao com objetos

                _registro_horario.Horario = reader.Item("horario").ToString
                _registro_horario.Codigo = codigo_horario.ToString
                _registro_horario.Codigo_tarifa = codigo_tarifa.ToString
                _registro_horario.Tipo_tarifa = "4"

                list_hor_tar.Add(_registro_horario)

                'Popula lista de tarifas com objetos

                _registro_tarifa.Codigo = codigo_tarifa.ToString
                _registro_tarifa.TTM = reader.Item("ttm").ToString
                _registro_tarifa.TTM_Value = reader.Item("valor_ttm").ToString
                _registro_tarifa.Step_ = reader.Item("step").ToString
                _registro_tarifa.Step_value = reader.Item("valor_step").ToString

                list_tarifas.Add(_registro_tarifa)

                codigo_tarifa = codigo_tarifa + 1

            End While
        End Using

    End Sub

    Public Function GetMaximumCode_Horario_Tarif_OP() As String
        Dim connection As New OleDbConnection(strConn)

        Dim sql As String = "select nvl(max(codigo_horario),1) as codigo from horarios_tarifacao_op_teste"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("codigo").ToString
            End While
        End Using

        Return ""

    End Function

    Public Function GetMaximumCode_Tarifas() As String
        Dim connection As New OleDbConnection(strConn)

        Dim sql As String = "select nvl(max(CODIGO_TARIFA),1) as CODIGO_TARIFA from HORARIOS_TARIFACAO_TESTE"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return reader.Item("CODIGO_TARIFA").ToString
            End While
        End Using

        Return ""

    End Function

End Class
