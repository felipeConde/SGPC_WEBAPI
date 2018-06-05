Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports System.Collections.Generic

Public Class DAO_BasicaCodigoDDI
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

    Public Function GetPaisDisponivel(ByVal cod_tarif_bas As String, ByVal codigooperadora As String, ByVal exibecodigo As Boolean) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim List As New List(Of AppGeneric)

        Dim Sql As String = "select p1.codigo_ddi codigo,nvl(p1.descricao,' ')descricao from codigo_ddi p1 where 1=1 and not exists( select 0 from codigoddi_basica p2 where p2.codigo_ddi=p1.codigo_ddi and p2.codigo_basico='" + cod_tarif_bas + "')"
        If codigooperadora <> "" Then
            Sql = Sql + " and not exists("
            Sql = Sql + " select"
            Sql = Sql + " p10.codigo_ddi,p10.codigo_basico,p11.codigo_operadora"
            Sql = Sql + " from codigoddi_basica p10,tarifas_basicas_praticadas p11"
            Sql = Sql + " where p10.codigo_basico=p11.codigo_basico"
            Sql = Sql + " and p11.codigo_operadora=" + codigooperadora
            'sql=sql+" and p10.codigo_basico="+codigobasica
            Sql = Sql + " and p1.codigo_ddi=p10.codigo_ddi)"
        End If
        If exibecodigo = True Then
            Sql = Sql + " order by p1.codigo_ddi"
        Else
            Sql = Sql + " order by p1.descricao"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = Sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString)
                List.Add(_registro)
            End While
        End Using

        Return List

    End Function

    Public Function GetPaisTarifaBasica(ByVal cod_tarif_bas As String, ByVal exibecodigo As Boolean) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim List As New List(Of AppGeneric)

        Dim Sql As String = "select p1.codigo_ddi codigo,nvl(p1.descricao,' ')descricao from codigo_ddi p1 where 1=1 and exists( select 0 from codigoddi_basica p2 where p2.codigo_ddi=p1.codigo_ddi and p2.codigo_basico='" + cod_tarif_bas + "')"
        If exibecodigo = True Then
            Sql = Sql + " order by p1.codigo_ddi"
        Else
            Sql = Sql + " order by p1.descricao"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = Sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("codigo").ToString, reader.Item("descricao").ToString)
                List.Add(_registro)
            End While
        End Using

        Return List

    End Function

    Public Function GetComboBasicas(ByVal codigo_operadora As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select codigo_basico codigo,nvl(descricao,' ')descricao from tarifas_basicas_praticadas where 1=1"
        If codigo_operadora <> "" Then
            strSQL = strSQL + " and codigo_operadora=" + Trim(codigo_operadora)
        End If

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

    Public Function GravaBasicaDDI(ByVal codigobasica As String, ByVal paises_tarifa As DataTable) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        Dim sql As String = ""

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            sql = "delete from codigoddi_basica where codigo_basico=" + codigobasica
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()


            For Each row As DataRow In paises_tarifa.Rows
                sql = "insert into codigoddi_basica(codigo_basico,codigo_ddi)values("
                sql = sql + "" + Trim(codigobasica) + "," + Trim(row.Item(0)) + ")"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next


            ''''LOG''''sql = String_log("", "N", Usuario)

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
End Class
