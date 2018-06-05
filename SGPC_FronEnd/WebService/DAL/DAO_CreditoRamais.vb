Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_CreditoRamais
    Private _strConn As String = ""
    Private _dao_commons As New DAO_Commons

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

    Public Function VerificarMeta(ByVal ramal As String, ByVal creditomovimentado As String, ByVal cod_usuario As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Dim resultado As Object

        Dim sql As String = ""
        sql = " select (valor_limite-(meta+ca+cr+ to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.'''))) as valor_limite from vRamaisMetas "
        sql = sql + " where (select grp_codigo from ramais where numero_a = '" + Trim(ramal) + "') "
        sql = sql + " like ramal_prefixo order by length(ramal_prefixo) desc"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            If reader.HasRows = True Then
                While reader.Read
                    resultado = reader.Item("valor_limite")
                End While
            Else
                resultado = True
            End If
        End Using

        If TravaMetaSite() Then
            If VerificaAdminGeral(cod_usuario) = True And IIf(resultado = True, True, IIf(resultado > 0, True, False)) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If


    End Function

    Public Function TravaMetaSite() As Boolean
        Dim connection As New OleDbConnection(strConn)


        Dim sql As String = ""
        sql = "select option_value from opcoes where option_name='travaMetaSites'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                If reader.Item("option_value").ToString = 1 Then
                    Return True
                Else
                    Return False
                End If
            End While
        End Using

        Return False
    End Function

    Public Function VerificaAdminGeral(ByVal cod_usuario As String) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Dim sql As String = ""
        sql = "select * from categoria_usuario where tipo_usuario in ('A','AL') and codigo_usuario=" & cod_usuario

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                If reader.Item("codigo").ToString Then
                    Return True
                Else
                    Return False
                End If
            End While
        End Using

        Return False
    End Function

    Public Function TransferirCredito(ByVal tipo As String, ByVal creditomovimentado As String, ByVal ramal As String, ByVal Usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            'Dim reader As OleDbDataReader

            strSQL = "update ramais set saldo_atual= saldo_atual + "
            If tipo = "debito" Then
                strSQL = strSQL & " - "
            End If
            strSQL = strSQL & " to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.''') where numero_a='" & ramal & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into lista_cred(NUMERO_A,CREDITO,DATA_CRED,CRED_DEB,MENSAL,GRP_CODIGO,USUARIO) "
            strSQL = strSQL + " select"
            strSQL = strSQL + "   p1.numero_a,"
            If tipo = "credito" Then
                strSQL = strSQL + "   to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.''') credito,"
            Else
                strSQL = strSQL + "  - to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.''') debito,"
            End If
            strSQL = strSQL + "   sysdate data_cred,"
            If tipo = "credito" Then
                strSQL = strSQL + "   'CA' cred_deb,"
            Else
                strSQL = strSQL + "   'CR' cred_deb,"
            End If
            strSQL = strSQL + "   'N' mensal,"
            strSQL = strSQL + "   (select grp_codigo from ramais where numero_a = '" + ramal + "') grp_codigo,"
            strSQL = strSQL + "   '" + Usuario + "' usuario"
            strSQL = strSQL + "   from ramais p1 where p1.numero_a='" + ramal + "'"

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

    Public Function AlteraMensal(ByVal creditomovimentado As String, ByVal ramal As String, ByVal Usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim strSQL As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction
            'Dim reader As OleDbDataReader

            strSQL = "update ramais set credito_mensal= "
            strSQL = strSQL & " to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.''') where numero_a='" & ramal & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into lista_cred(NUMERO_A,CREDITO,DATA_CRED,CRED_DEB,MENSAL,GRP_CODIGO,USUARIO) "
            strSQL = strSQL + " select"
            strSQL = strSQL + "   p1.numero_a,"
            strSQL = strSQL + "   to_number('" + Replace(creditomovimentado, ".", ",") + "','9999999999D99999999','NLS_NUMERIC_CHARACTERS = '',.''') debito,"
            strSQL = strSQL + "   sysdate data_cred,"
            strSQL = strSQL + "   'CM' cred_deb,"
            strSQL = strSQL + "   'N' mensal,"
            strSQL = strSQL + "   (select grp_codigo from ramais where numero_a = '" + ramal + "') grp_codigo,"
            strSQL = strSQL + "   '" + Usuario + "' usuario"
            strSQL = strSQL + "   from ramais p1 where p1.numero_a='" + ramal + "'"

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

    Public Sub AtualizaSaldo(ByVal dataFatura As String, ByVal datafim As String, ByVal prefixo As String, ByVal pRota As String)

        Dim strSQL As String = ""

        strSQL = "select " & _
                 "   p1.numero_a ramal," & _
                 "   nvl(p1.saldo_atual,0) saldo," & _
                 "   nvl(p1.credito_mensal,0) meta," & _
                 "   nvl(p2.gasto,0)gasto," & _
                 "   nvl(p3.adicionado,0)adicionado" & _
                 " from " & _
                 "   ramais p1,"
        strSQL = strSQL + "   (select p10.rml_numero_a ramal,sum(p10.valor_cdr)gasto from cdrs p10" & _
                          "    where p10.data_inicio>=to_date('" + dataFatura + "','DD/MM/YYYY HH24:MI:SS')" & _
                          "    and   p10.data_inicio<=to_date('" + datafim + "','DD/MM/YYYY HH24:MI:SS')" & _
                          "    and exists(select 0 from ramais p20 where p10.rml_numero_a=p20.numero_a) "

        If Not String.IsNullOrEmpty(pRota.Trim) Then
            strSQL = strSQL + " and   p10.route not in (" + pRota + ")"
        End If

        strSQL = strSQL + "    group by p10.rml_numero_a)p2,"

        strSQL = strSQL + "   (select p10.numero_a ramal,sum(p10.credito)adicionado from lista_cred p10" & _
                          "    where p10.data_cred>=to_date('" + dataFatura + "','DD/MM/YYYY HH24:MI:SS')" & _
                          "      and p10.data_cred<=to_date('" + datafim + "','DD/MM/YYYY HH24:MI:SS')" & _
                          "    and p10.cred_deb<>'CM'" & _
                          "    group by p10.numero_a)p3"

        strSQL = strSQL + " where p1.numero_a=p2.ramal(+)  and   p1.numero_a=p3.ramal(+)"

        If Not prefixo.Trim = "" Then
            strSQL = strSQL + " and   p1.numero_a like '" + prefixo + "%'"
        End If

        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Dim saldoRamal As Double = 0.0
        Dim strRamal As String

        While reader.Read
            'saldoRamal = reader.GetValue(2) + reader.GetValue(4) - reader.GetValue(3)
            saldoRamal = reader.Item("meta") + reader.Item("adicionado") - reader.Item("gasto")
            strRamal = reader.GetValue(0)
            strSQL = "update ramais set saldo_atual='" & Str(saldoRamal) & "'" & _
                    "Where numero_a='" + strRamal + "'"
            Dim cmd2 As OleDbCommand = connection.CreateCommand
            cmd2.CommandText = strSQL
            'cmd.Parameters.AddWithValue("@cod_operadora", CInt(pOperadora))
            cmd2.ExecuteNonQuery()
            cmd2.Dispose()
            cmd2 = Nothing
        End While
        connection.Close()
        cmd.Dispose()
    End Sub

    Public Function GetdiaFatura() As String

        Dim diaFatura As String = 1
        Dim strSQL As String = "SELECT to_char(DATA_FATURA,'DD')DATA_FATURA from EMPRESAS where rownum<2"


        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                'listUsuarios.Add(New usuario(reader.Item(0), reader.Item(1), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5), reader.Item(6), reader.Item(7)))
                diaFatura = reader.Item(0).ToString
            End While
        End Using
        connection.Close()
        Return diaFatura
    End Function


End Class
