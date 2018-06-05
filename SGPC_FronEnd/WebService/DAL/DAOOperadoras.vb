Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAOOperadoras
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

    Public Function GetComboOperadoras(Optional pSomenteComGasto As String = "") As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = "select -1 as CODIGO, ' TODAS' as OPERADORA from dual union "
        strSQL = strSQL + "select o.CODIGO, o.DESCRICAO as OPERADORA "
        strSQL = strSQL + " from OPERADORAS_TESTE o"
        If pSomenteComGasto = "S" Then
            strSQL = strSQL + " where exists(select 0 from faturas f where f.codigo_operadora=o.codigo)"
        End If
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function



    Public Function GetComboOperadorasMoveis() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct o.CODIGO, o.DESCRICAO as OPERADORA "
        strSQL = strSQL + " from OPERADORAS_TESTE o, FATURAS f"
        strSQL = strSQL + " where o.codigo = f.CODIGO_OPERADORA and f.CODIGO_TIPO=1"
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetComboOperadorasFixo() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct o.CODIGO, o.DESCRICAO as OPERADORA "
        strSQL = strSQL + " from OPERADORAS_TESTE o, FATURAS f"
        strSQL = strSQL + " where o.codigo = f.CODIGO_OPERADORA and f.CODIGO_TIPO=2"
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetComboOperadorasByTipo(ByVal tipo As String) As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct o.CODIGO, o.DESCRICAO as OPERADORA "
        strSQL = strSQL + " from OPERADORAS_TESTE o, FATURAS f"
        strSQL = strSQL + " where o.codigo = f.CODIGO_OPERADORA and f.CODIGO_TIPO='" + tipo + "'"
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetFornecedoresOperadoras() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct CODIGO, NOME_FANTASIA as OPERADORA "
        strSQL = strSQL + " from fornecedores"
        strSQL = strSQL + " where COD_TIPO_FORNECEDOR = 1"
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetCodOperadorasByFornec(ByVal cod_fornec As String) As String
        Dim connection As New OleDbConnection(strConn)
        Dim cod_operadora As String = ""

        Dim strSQL As String = ""
        strSQL = strSQL + " select distinct CODIGO_OPERADORA"
        strSQL = strSQL + " from fornecedores"
        strSQL = strSQL + " where CODIGO = '" + cod_fornec + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("CODIGO_OPERADORA").ToString)
                cod_operadora = _registro
            End While
        End Using

        Return cod_operadora
    End Function

    Public Function ComboOperadorasMoveis() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct o.CODIGO, o.DESCRICAO as OPERADORA "
        strSQL = strSQL + " from OPERADORAS_TESTE o, FATURAS f"
        strSQL = strSQL + " where o.codigo = f.CODIGO_OPERADORA and f.CODIGO_TIPO=1"
        strSQL = strSQL + " order by OPERADORA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("OPERADORA").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetOpVencimentoMoveis(ByVal codigo As Integer) As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listVencimentos As New List(Of AppOperadoras)

        Dim strSQL As String = "select dt_ven from"
        strSQL = strSQL + " (select distinct to_char(dt_vencimento,'MM/YYYY') as dt_ven from faturas"
        strSQL = strSQL + " where " + codigo.ToString() + " = CODIGO_OPERADORA and CODIGO_TIPO = 1)"
        strSQL = strSQL + " order by to_date(dt_ven, 'mm/yyyy')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("DT_VEN").ToString)
                listVencimentos.Add(_registro)
            End While
        End Using

        Return listVencimentos
    End Function

    Public Sub ComboOperadorasPlanos(ByVal codigo As String, ByRef listPlanos As List(Of AppGeneric))
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select PLANO, CODIGO_PLANO"
        strSQL = strSQL + " from OPERADORAS_PLANOS"
        strSQL = strSQL + " where CODIGO_OPERADORA='" + codigo + "'"
        strSQL = strSQL + " order by PLANO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("CODIGO_PLANO").ToString, reader.Item("PLANO").ToString)
                listPlanos.Add(_registro)
            End While
        End Using

    End Sub


    Public Function ExcluiOperadora(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = " delete from operadoras_teste "
            strSQL = strSQL + " where CODIGO = " + Convert.ToString(pcodigo)

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

    Public Function GetOperadorasByCodigo(ByVal pCodigo As Integer) As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = "select *"
        strSQL = strSQL + " from OPERADORAS_TESTE where codigo='" & pCodigo & "'"
        strSQL = strSQL + " order by DESCRICAO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("DESCRICAO").ToString, vbNull, vbNull)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function InsereOperadora(ByVal _registro As AppOperadoras, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into OPERADORAS_TESTE(CODIGO"
        strSQL = strSQL + ", DESCRICAO, DEFAULT_OP )"

        strSQL = strSQL + " values('" + _registro.Codigo.ToString + "'"
        strSQL = strSQL + ",'" + _registro.Descricao.ToString + "'"
        strSQL = strSQL + ",'" + _registro.Default_Op.ToString + "'"
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        InsertRegistroLog(_registro, "N", user)

        Return True
    End Function


    Public Function InsertRegistroLog(ByVal _registro As AppOperadoras, ByVal insert As Char, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into OPERADORAS_LOG(CODIGO_LOG, CODIGO, TIPO_LOG, DATA_LOG"
        strSQL = strSQL + ", USUARIO_LOG "
        strSQL = strSQL + ", DESCRICAO, DEFAULT_OP) "

        strSQL = strSQL + " values ((select nvl(max(CODIGO_LOG),0)+1 from OPERADORAS_LOG)"
        'Tipo_log
        strSQL = strSQL + ",'" + _registro.Codigo.ToString() + "'"
        strSQL = strSQL + ",'" + insert + "'"
        strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
        strSQL = strSQL + ",'" + user + "'"
        strSQL = strSQL + ",'" + _registro.Descricao + "'"
        strSQL = strSQL + ",'" + _registro.Default_Op + "'"
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        Return True
    End Function


    Public Function Atualizaoperadora(ByVal registro As AppOperadoras, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        InsertRegistroLog(GetOperadorasByCodigo(registro.Codigo).Item(0), "A", user)

        Dim strSQL As String = "update OPERADORAS_TESTE set "
        strSQL = strSQL + "DESCRICAO='" + registro.Descricao + "',"
        strSQL = strSQL + "DEFAULT_OP='" + registro.Default_Op.ToUpper + "' "
       
        strSQL = strSQL + " where CODIGO = '" + registro.Codigo.ToString + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        InsertRegistroLog(registro, "B", user)

        Return True

    End Function

    Public Function ExcluirOperadora(ByVal pcodigo As String, ByVal user As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim registro As AppOperadoras = GetOperadorasByCodigo(pcodigo).Item(0)

            Dim strSQL As String = "delete OPERADORAS_TESTE "
            strSQL = strSQL + "where CODIGO = '" + pcodigo + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            InsertRegistroLog(registro, "D", user)

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

End Class
