Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOPerfilLinha
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

    Public Function InserePerfilLinha(ByVal pperfillinha As AppPerfilLinha) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into PERFIL_LINHA("
            strSQL = strSQL + "CODIGO_GESTAOPERFIL,CODIGO_LINHA,FIM_CICLO) "
            strSQL = strSQL + "values ("
            strSQL = strSQL + "'" + Convert.ToString(pperfillinha.Codigo_GestaoPerfil) + "'"
            strSQL = strSQL + ",'" + Convert.ToString(pperfillinha.Codigo_Linha) + "'"
            strSQL = strSQL + ",'" + pperfillinha.Fim_Ciclo + "'"
            strSQL = strSQL + ")"

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

    Public Function AtualizaPerfilLinha(ByVal pperfillinha As AppPerfilLinha) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update PERFIL_LINHA set "
            strSQL = strSQL + "CODIGO_GESTAOPERFIL='" + Convert.ToString(pperfillinha.Codigo_GestaoPerfil) + "'"
            strSQL = strSQL + ", CODIGO_LINHA='" + Convert.ToString(pperfillinha.Codigo_Linha) + "'"
            strSQL = strSQL + ", FIM_CICLO='" + pperfillinha.Fim_Ciclo + "'"
            strSQL = strSQL + " where CODIGO_GESTAOPERFIL='" + Convert.ToString(pperfillinha.Codigo_GestaoPerfil) + "'"
            strSQL = strSQL + " and CODIGO_LINHA='" + Convert.ToString(pperfillinha.Codigo_Linha) + "'"

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

    Public Function delaltPerfilLinhas(ByVal pcdgestaoperfil As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete PERFIL_LINHA "
            strSQL = strSQL + " where CODIGO_GESTAOPERFIL='" + Convert.ToString(pcdgestaoperfil) + "'"

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

    Public Function ExcluiPerfilLinha(ByVal pcdlinha As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete PERFIL_LINHA "
            strSQL = strSQL + " where CODIGO_LINHA='" + Convert.ToString(pcdlinha) + "'"

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

    Public Function ExcluiPerfil(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete PERFIL_LINHA "
            strSQL = strSQL + " where CODIGO_GESTAOPERFIL='" + Convert.ToString(pcodigo) + "'"

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

    Public Function GetPerfilLinhaById(ByVal pcdgestaoperfil As Integer) As List(Of AppPerfilLinha)
        Dim connection As New OleDbConnection(strConn)
        Dim listPerfilLinha As New List(Of AppPerfilLinha)

        Dim strSQL As String = "select p.CODIGO_GESTAOPERFIL, nvl(p.CODIGO_LINHA,0) as codigo_linha,  nvl(p.FIM_CICLO,0) as FIM_CICLO"
        strSQL = strSQL + ", g.GESTAO_PERFIL, l.NUM_LINHA, nvl(o.CODIGO,0) as CODIGO_OPERADORA, o.DESCRICAO as OPERADORA, u.NOME_USUARIO as USUARIO"
        strSQL = strSQL + " from PERFIL_LINHA p,GESTAO_PERFIL g,LINHAS_MOVEIS m,LINHAS l,OPERADORAS_TESTE o,USUARIOS u"
        strSQL = strSQL + " where p.CODIGO_GESTAOPERFIL = '" & pcdgestaoperfil.ToString & "'"
        strSQL = strSQL + " and p.CODIGO_GESTAOPERFIL = g.CODIGO(+)"
        strSQL = strSQL + " and p.CODIGO_LINHA = m.CODIGO_LINHA(+)"
        strSQL = strSQL + " and m.CODIGO_LINHA = l.CODIGO_LINHA(+)"
        strSQL = strSQL + " and m.CODIGO_OPERADORA = l.CODIGO_OPERADORA(+)"
        strSQL = strSQL + " and m.CODIGO_OPERADORA = o.CODIGO(+)"
        strSQL = strSQL + " and m.CODIGO_USUARIO = u.CODIGO(+)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppPerfilLinha(reader.Item("CODIGO_GESTAOPERFIL").ToString, reader.Item("CODIGO_LINHA").ToString, reader.Item("FIM_CICLO").ToString, reader.Item("GESTAO_PERFIL").ToString, reader.Item("NUM_LINHA").ToString, reader.Item("CODIGO_OPERADORA").ToString, reader.Item("OPERADORA").ToString, reader.Item("USUARIO").ToString)
                listPerfilLinha.Add(_registro)
            End While
        End Using

        Return listPerfilLinha
    End Function

    Public Function GetComboFiltroPerfil() As List(Of AppGestaoPerfil)
        Dim connection As New OleDbConnection(strConn)
        Dim listFiltroPerfil As New List(Of AppGestaoPerfil)

        Dim strSQL As String = "select 0 as CODIGO_GESTAOPERFIL, '...' as GESTAO_PERFIL from dual union "
        strSQL = strSQL + "select CODIGO as CODIGO_GESTAOPERFIL, GESTAO_PERFIL "
        strSQL = strSQL + "from GESTAO_PERFIL "
        strSQL = strSQL + "order by CODIGO_GESTAOPERFIL"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                'Dim _registro As New AppPerfilLinha(reader.Item("CODIGO_GESTAOPERFIL").ToString, vbNull, vbNull, reader.Item("GESTAO_PERFIL").ToString, vbNull, vbNull, vbNull, vbNull)
                Dim _registro As New AppGestaoPerfil(reader.Item("CODIGO_GESTAOPERFIL").ToString, reader.Item("GESTAO_PERFIL").ToString)
                listFiltroPerfil.Add(_registro)
            End While
        End Using

        Return listFiltroPerfil
    End Function

    'Public Function GetGestaoPerfil(ByVal pgestao_perfil As String) As AppGestaoPerfil
    '   Dim _registro As AppGestaoPerfil = Nothing
    '   Try
    '      Dim strSQL As String = "select CODIGO"
    '      strSQL = strSQL + ", GESTAO_PERFIL, MINUTOS, DDD, SMS, ACOBRAR, PCT_DADOS, MIN_DDD, MIN_LOCAL, VLR_DDD, VLR_LOCAL, VLR_SMS, VLR_ASSINATURA, VLR_G_WEB, VLR_T_ZERO, VLR_PCT_BLACK, VLR_LIM_TOTAL "
    '      strSQL = strSQL + "from GESTAO_PERFIL "
    '      strSQL = strSQL + "where GESTAO_PERFIL = '" + pgestao_perfil + "'"

    '      Dim connection As New OleDbConnection(strConn)
    '      Dim cmd As OleDbCommand = connection.CreateCommand
    '      cmd.CommandText = strSQL
    '      Dim reader As OleDbDataReader
    '      connection.Open()
    '      reader = cmd.ExecuteReader
    '      Using connection
    '         While reader.Read
    '            _registro = New AppGestaoPerfil(reader.Item("CODIGO").ToString, reader.Item("GESTAO_PERFIL").ToString, reader.Item("MINUTOS").ToString, reader.Item("DDD").ToString, reader.Item("SMS").ToString, reader.Item("ACOBRAR").ToString, reader.Item("PCT_DADOS").ToString, reader.Item("MIN_DDD").ToString, reader.Item("MIN_LOCAL").ToString, reader.Item("VLR_DDD").ToString, reader.Item("VLR_LOCAL").ToString, reader.Item("VLR_SMS").ToString, reader.Item("VLR_ASSINATURA").ToString, reader.Item("VLR_G_WEB").ToString, reader.Item("VLR_T_ZERO").ToString, reader.Item("VLR_PCT_BLACK").ToString, reader.Item("VLR_LIM_TOTAL").ToString)
    '         End While
    '      End Using
    '      connection.Close()
    '      cmd.Dispose()
    '   Catch ex As Exception

    '   End Try
    '   Return _registro
    'End Function

    'Public Function GravaLog(ByVal pTipo As String, ByVal pAutor As String, ByVal pfatura As Fatura, ByVal pCodigo As String) As Boolean
    '    Dim connection As New OleDbConnection(strConn)
    '    Try
    '        Dim strSQL As String = "insert into faturas_controle_log select '" & pTipo & "', fatura,codigo_operadora, codigo_tipo,intervalo_mes,data_inicio,debito_automatico,dia_vencimento,codigo_estado,data_fim,febraban,codigo_fatura_controle where codigo_fatura_controle='" & pCodigo & "'"

    '        Dim cmd As OleDbCommand = connection.CreateCommand
    '        cmd.CommandText = strSQL
    '        connection.Open()
    '        cmd.ExecuteNonQuery()
    '        connection.Close()
    '        cmd.Dispose()
    '        Return True
    '    Catch ex As Exception
    '        connection.Close()
    '        Return False
    '    End Try
    'End Function

End Class
