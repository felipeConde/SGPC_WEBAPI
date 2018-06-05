Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAO_Centrais

    Private _strConn As String = ""

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Private _msg As String
    Public Property MSG As String
        Get
            Return _msg
        End Get
        Set(ByVal value As String)
            _msg = value
        End Set
    End Property

    Public Function RetornaConexao() As String
        Return strConn
    End Function

    Public Function GetCentrais(ByVal pExibeCodigo As Boolean) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = ""
        strSQL = strSQL + " select codigo,descricao from codigo_area order by codigo "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                If pExibeCodigo Then
                    Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("CODIGO").ToString)
                    list.Add(_registro)
                Else
                    Dim _registro As New AppGeneric(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                    list.Add(_registro)
                End If

            End While
        End Using

        Return list
    End Function


    Public Function GetTipoLigacao(Optional ByVal tipo_tarifa As Integer = 0) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = ""
        strSQL = strSQL + " select distinct p1.codigo,p1.descricao,nvl(p1.complemento,' ')complemento, p2.OPER_CODIGO_OPERADORA "
        strSQL = strSQL + " from tipos_ligacao_teste p1, tarifacao p2 "
        strSQL = strSQL + " where p1.CODIGO_TARIF=p2.codigo and p2.tipo_tarifa='" & tipo_tarifa & "'"
        strSQL = strSQL + " order by descricao "

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


    Public Function InsereCentral(ByVal registro As AppCentral) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "insert into centrais_teste(area_code"
        strSQL = strSQL + ", central, tipo_ddd, descricao, operadora "
        strSQL = strSQL + " ) "
        strSQL = strSQL + " values('" + registro.Area_Code.ToString + "'"
        strSQL = strSQL + ",'" + registro.Central.ToString + "'"
        strSQL = strSQL + ",'" + registro.Tipo.ToString + "'"
        strSQL = strSQL + ",'" + registro.Descricao.ToString + "'"
        strSQL = strSQL + ",'" + registro.Operadora.ToString + "'"
        strSQL = strSQL + ")"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        connection.Open()
        cmd.ExecuteNonQuery()
        connection.Close()
        cmd.Dispose()

        Return True

    End Function


    Public Function AtualizaCentral(ByVal registro As AppCentral, ByVal registroOld As AppCentral) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update centrais_teste set "
            strSQL = strSQL + "area_code='" + registro.Area_Code.ToString + "',"
            strSQL = strSQL + "central='" + registro.Central.ToString + "',"
            strSQL = strSQL + "tipo_ddd='" + registro.Tipo.ToString + "',"
            strSQL = strSQL + "descricao='" + registro.Descricao.ToString + "'"
            strSQL = strSQL + " ,operadora='" + registro.Operadora.ToString + "'"

            strSQL = strSQL + " where area_code = '" + registroOld.Area_Code.ToString + "' and  central='" + registroOld.Central.ToString + "' and tipo_ddd='" + registroOld.Tipo.ToString + "' and operadora='" + registroOld.Operadora.ToString + "' "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            MSG = "Erro: " & ex.Message
            Return False
        End Try
        

    End Function


    Public Function GetCentralById(ByVal codigoarea As String, ByVal central As String, ByVal operadora As String) As AppCentral
        Dim connection As New OleDbConnection(strConn)
        Dim registro As New AppCentral

        Dim strSQL As String = ""
        strSQL = strSQL + "select * from centrais_teste "
        strSQL = strSQL + " where area_code='" + codigoarea + "'"
        strSQL = strSQL + " and central='" + central + "'"
        strSQL = strSQL + " and operadora='" + operadora + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                registro = New AppCentral(reader.Item("area_code").ToString, reader.Item("central").ToString, reader.Item("tipo_ddd").ToString, reader.Item("descricao").ToString, reader.Item("operadora").ToString)
                'listOperadoras.Add(_registro)
            End While
        End Using

        Return registro
    End Function

    Public Function ExcluiCentral(ByVal registro As AppCentral) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = ""
            strSQL = "delete from centrais_teste"
            strSQL = strSQL + " where area_code='" + registro.Area_Code.ToString + "'"
            strSQL = strSQL + " and central='" + registro.Central.ToString + "'"
            strSQL = strSQL + " and operadora='" + registro.Operadora.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return True
        Catch ex As Exception
            MSG = "Erro: " & ex.Message
            Return False
        End Try



    End Function

End Class
