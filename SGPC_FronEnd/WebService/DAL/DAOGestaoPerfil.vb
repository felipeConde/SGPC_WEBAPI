Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAOGestaoPerfil
    'Private strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    'Private _strConn As String = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=server;"
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

    Public Function InsereGestaoPerfil(ByVal pgestao_perfil As AppGestaoPerfil, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = "insert into GESTAO_PERFIL(CODIGO"
            strSQL = strSQL + ", GESTAO_PERFIL, MINUTOS, DDD, SMS, ACOBRAR, PCT_DADOS, MIN_DDD, MIN_LOCAL, VLR_DDD, VLR_PCT_LD, VLR_LOCAL, VLR_SMS, VLR_ASSINATURA, VLR_G_WEB, VLR_T_ZERO, VLR_PCT_BLACK, VLR_LIM_TOTAL,MIN_ROAMING,QTD_SMS,MB_DADOS,TIPO_FRANQUIA,VLR_ROAMING) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from GESTAO_PERFIL)"
            strSQL = strSQL + ",'" + pgestao_perfil.Gestao_Perfil + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Minutos.ToString, ",", ".") + "'"
            strSQL = strSQL + ",'" + pgestao_perfil.DDD + "','" + pgestao_perfil.SMS + "','" + pgestao_perfil.ACobrar + "','" + pgestao_perfil.Pct_Dados + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Min_DDD, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Min_Local, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_DDD, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_pct_LD, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_Local, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_SMS, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_Assinatura, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_G_Web, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_T_Zero, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_Pct_Black, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(pgestao_perfil.Vlr_Lim_Total, ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(Replace(pgestao_perfil.Min_roaming, ".", ""), ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(Replace(pgestao_perfil.Qtd_sms, ".", ""), ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(Replace(pgestao_perfil.Mb_dados, ".", ""), ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(Replace(pgestao_perfil.Tipo_Franquia, ".", ""), ",", ".") + "'"
            strSQL = strSQL + ",'" + Replace(Replace(pgestao_perfil.Vlr_Roaming, ".", ""), ",", ".") + "'"

            strSQL += ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("(select nvl(max(CODIGO),0) from GESTAO_PERFIL)", "N", usuario)
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
    End Function

    Public Function AtualizaGestaoPerfil(ByVal pgestao_perfil As AppGestaoPerfil, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            strSQL = String_log("'" & pgestao_perfil.Codigo.ToString & "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update GESTAO_PERFIL set "
            strSQL = strSQL + "GESTAO_PERFIL='" + pgestao_perfil.Gestao_Perfil + "'"
            strSQL = strSQL + ", MINUTOS='" + Replace(pgestao_perfil.Minutos.ToString, ",", ".") + "'"
            strSQL = strSQL + ", DDD='" + pgestao_perfil.DDD + "'"
            strSQL = strSQL + ", SMS='" + pgestao_perfil.SMS + "'"
            strSQL = strSQL + ", ACOBRAR='" + pgestao_perfil.ACobrar + "'"
            strSQL = strSQL + ", PCT_DADOS='" + pgestao_perfil.Pct_Dados + "'"
            strSQL = strSQL + ", MIN_DDD='" + Replace(pgestao_perfil.Min_DDD.ToString, ",", ".") + "'"
            strSQL = strSQL + ", MIN_LOCAL='" + Replace(pgestao_perfil.Min_Local.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_DDD='" + Replace(pgestao_perfil.Vlr_DDD.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_PCT_LD='" + Replace(pgestao_perfil.Vlr_pct_LD.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_LOCAL='" + Replace(pgestao_perfil.Vlr_Local.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_SMS='" + Replace(pgestao_perfil.Vlr_SMS.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_ASSINATURA='" + Replace(pgestao_perfil.Vlr_Assinatura.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_G_WEB='" + Replace(pgestao_perfil.Vlr_G_Web.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_T_ZERO='" + Replace(pgestao_perfil.Vlr_T_Zero.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_PCT_BLACK='" + Replace(pgestao_perfil.Vlr_Pct_Black.ToString, ",", ".") + "'"
            strSQL = strSQL + ", VLR_LIM_TOTAL='" + Replace(pgestao_perfil.Vlr_Lim_Total.ToString, ",", ".") + "'"
            strSQL = strSQL + ", MIN_ROAMING='" + Replace(pgestao_perfil.Min_roaming.ToString, ",", ".") + "'"
            strSQL = strSQL + ", QTD_SMS='" + Convert.ToInt32(pgestao_perfil.Qtd_sms).ToString + "'"
            strSQL = strSQL + ", MB_DADOS='" + Convert.ToInt32(pgestao_perfil.Mb_dados).ToString + "'"
            If pgestao_perfil.Tipo_Franquia <> Nothing Then
                strSQL = strSQL + ", TIPO_FRANQUIA='" + Replace(pgestao_perfil.Tipo_Franquia.ToString, ",", ".") + "'"
            End If
            strSQL = strSQL + ", VLR_ROAMING='" + Replace(pgestao_perfil.Vlr_Roaming.ToString, ",", ".") + "'"
            strSQL = strSQL + " where CODIGO = '" + Replace(pgestao_perfil.Codigo.ToString, ",", ".") + "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" & pgestao_perfil.Codigo.ToString & "'", "B", usuario)
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

    End Function

    Public Function ExcluiGestaoPerfil(ByVal pcodigo As Integer, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction


            Dim strSQL As String = String_log("'" & pcodigo & "'", "D", Usuario)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete GESTAO_PERFIL "
            strSQL = strSQL + "where CODIGO = '" & pcodigo & "'"

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
    End Function

    Public Function GetGestaoPerfilById(ByVal pcodigo As Integer) As List(Of AppGestaoPerfil)
        Dim connection As New OleDbConnection(strConn)
        Dim listGestaoPerfil As New List(Of AppGestaoPerfil)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ", GESTAO_PERFIL, MINUTOS"
        strSQL = strSQL + ", case when DDD='N' then 'Não' else 'Sim' end DDD"
        strSQL = strSQL + ", case when SMS='N' then 'Não' else 'Sim' end SMS"
        strSQL = strSQL + ", case when ACOBRAR='N' then 'Não' else 'Sim' end ACOBRAR"
        strSQL = strSQL + ", case when PCT_DADOS='N' then 'Não' else 'Sim' end PCT_DADOS"
        strSQL = strSQL + ", MIN_DDD, MIN_LOCAL, VLR_DDD, VLR_LOCAL, VLR_SMS, VLR_ASSINATURA, VLR_G_WEB, VLR_T_ZERO, VLR_PCT_BLACK, nvl(VLR_PCT_LD, 0) VLR_PCT_LD, VLR_LIM_TOTAL,nvl(MIN_ROAMING,0)MIN_ROAMING,nvl(QTD_SMS,0)QTD_SMS,nvl(MB_DADOS,0)MB_DADOS,nvl(VLR_ROAMING,0)VLR_ROAMING, nvl(TIPO_FRANQUIA,'')TIPO_FRANQUIA "
        strSQL = strSQL + "from GESTAO_PERFIL "
        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)
        ElseIf pcodigo = 0 Then
            strSQL = strSQL + "order by CODIGO"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString, reader.Item("MINUTOS").ToString, reader.Item("DDD").ToString, reader.Item("SMS").ToString, reader.Item("ACOBRAR").ToString, reader.Item("PCT_DADOS").ToString, reader.Item("MIN_DDD").ToString, reader.Item("MIN_LOCAL").ToString, reader.Item("VLR_DDD").ToString, reader.Item("VLR_PCT_LD").ToString, reader.Item("VLR_LOCAL").ToString, reader.Item("VLR_SMS").ToString, reader.Item("VLR_ASSINATURA").ToString, reader.Item("VLR_G_WEB").ToString, reader.Item("VLR_T_ZERO").ToString, reader.Item("VLR_PCT_BLACK").ToString, reader.Item("VLR_LIM_TOTAL").ToString)
                _registro.Min_roaming = reader.Item("MIN_ROAMING").ToString
                _registro.Qtd_sms = reader.Item("QTD_SMS").ToString
                _registro.Mb_dados = reader.Item("MB_DADOS").ToString
                _registro.Vlr_Roaming = reader.Item("VLR_ROAMING").ToString
                _registro.Tipo_Franquia = reader.Item("TIPO_FRANQUIA").ToString


                listGestaoPerfil.Add(_registro)
            End While
        End Using

        Return listGestaoPerfil
    End Function

    Public Function GetGestaoPerfil(ByVal pgestao_perfil As String) As AppGestaoPerfil
        Dim _registro As AppGestaoPerfil = Nothing
        Try
            Dim strSQL As String = "select CODIGO"
            strSQL = strSQL + ", GESTAO_PERFIL, MINUTOS, DDD, SMS, ACOBRAR, PCT_DADOS, MIN_DDD, MIN_LOCAL, VLR_DDD, VLR_PCT_LD, VLR_LOCAL, VLR_SMS, VLR_ASSINATURA, VLR_G_WEB, VLR_T_ZERO, VLR_PCT_BLACK, VLR_LIM_TOTAL "
            strSQL = strSQL + "from GESTAO_PERFIL "
            strSQL = strSQL + "where GESTAO_PERFIL = '" + pgestao_perfil + "'"

            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    _registro = New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString, reader.Item("MINUTOS").ToString, reader.Item("DDD").ToString, reader.Item("SMS").ToString, reader.Item("ACOBRAR").ToString, reader.Item("PCT_DADOS").ToString, reader.Item("MIN_DDD").ToString, reader.Item("MIN_LOCAL").ToString, reader.Item("VLR_DDD").ToString, reader.Item("VLR_PCT_LD").ToString, reader.Item("VLR_LOCAL").ToString, reader.Item("VLR_SMS").ToString, reader.Item("VLR_ASSINATURA").ToString, reader.Item("VLR_G_WEB").ToString, reader.Item("VLR_T_ZERO").ToString, reader.Item("VLR_PCT_BLACK").ToString, reader.Item("VLR_LIM_TOTAL").ToString)
                End While
            End Using
            connection.Close()
            cmd.Dispose()
        Catch ex As Exception

        End Try
        Return _registro
    End Function

    Public Function GetComboGestaoPerfil_livres() As List(Of AppGestaoPerfil)
        Dim connection As New OleDbConnection(strConn)
        Dim listGestaoPerfil As New List(Of AppGestaoPerfil)

        Dim strSQL As String = "select 0 as CODIGO, '...' as GESTAO_PERFIL from dual union "
        strSQL = strSQL + "select CODIGO, GESTAO_PERFIL "
        strSQL = strSQL + "from GESTAO_PERFIL "
        strSQL = strSQL + "where GESTAO_PERFIL.codigo not in ( select p.codigo_gestaoperfil from perfil_linha p) "
        strSQL = strSQL + "order by GESTAO_PERFIL"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString)
                listGestaoPerfil.Add(_registro)
            End While
        End Using

        Return listGestaoPerfil
    End Function

    Public Function GetComboGestaoPerfil_all() As List(Of AppGestaoPerfil)
        Dim connection As New OleDbConnection(strConn)
        Dim listGestaoPerfil As New List(Of AppGestaoPerfil)

        Dim strSQL As String = "select 0 as CODIGO, '...' as GESTAO_PERFIL from dual union "
        strSQL = strSQL + "select CODIGO, GESTAO_PERFIL "
        strSQL = strSQL + "from GESTAO_PERFIL "
        strSQL = strSQL + "order by GESTAO_PERFIL"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString)
                listGestaoPerfil.Add(_registro)
            End While
        End Using

        Return listGestaoPerfil
    End Function

    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into GESTAO_PERFIL_LOG (codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + " CODIGO, GESTAO_PERFIL, MINUTOS, DDD, SMS, ACOBRAR, PCT_DADOS, MIN_DDD, MIN_LOCAL,"
        sql = sql + " VLR_DDD, VLR_PCT_LD, VLR_LOCAL, VLR_SMS, VLR_ASSINATURA, VLR_G_WEB, VLR_T_ZERO, VLR_PCT_BLACK, "
        sql = sql + " VLR_LIM_TOTAL,MIN_ROAMING,QTD_SMS,MB_DADOS,TIPO_FRANQUIA,VLR_ROAMING) "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from GESTAO_PERFIL_LOG),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        sql = sql + "" + pcodigo + ","
        sql = sql + " (select GESTAO_PERFIL from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select MINUTOS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select DDD from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select SMS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select ACOBRAR from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select PCT_DADOS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select MIN_DDD from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select MIN_LOCAL from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_DDD from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_PCT_LD from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_LOCAL from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_SMS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_ASSINATURA from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_G_WEB from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_T_ZERO from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_PCT_BLACK from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_LIM_TOTAL from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select MIN_ROAMING from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select QTD_SMS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select MB_DADOS from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select TIPO_FRANQUIA from gestao_perfil where CODIGO=" + pcodigo + "),"
        sql = sql + " (select VLR_ROAMING from gestao_perfil where CODIGO=" + pcodigo + "))"
        Return sql

    End Function

End Class
