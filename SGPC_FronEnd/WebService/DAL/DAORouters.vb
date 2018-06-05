Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic

Public Class DAORouters

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


    Public Function InsereRouter(ByVal router As AppRouters) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into ROUTERS(CODIGO_ROUTER"
            strSQL = strSQL + ", NOME, MODELO, VER_ATUAL, RELEASE, BOOTROM_ATUAL, ATIVO_FIXO, CANAL_VOZ, IPPABX, MARCA) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO_ROUTER),0)+1 from ROUTERS)"
            strSQL = strSQL + ",'" + router.Nome + "'"
            strSQL = strSQL + ",'" + router.Modelo + "'"
            strSQL = strSQL + ",'" + router.Versao + "'"
            strSQL = strSQL + ",'" + router.Release + "'"
            strSQL = strSQL + ",'" + router.BootRom + "'"
            strSQL = strSQL + ",'" + router.Ativo + "'"
            strSQL = strSQL + ",'" + router.Canal + "'"
            strSQL = strSQL + ",'" + router.IP_PABX + "'"
            strSQL = strSQL + ",'" + router.Marca + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function AtualizaRouter(ByVal prouters As AppRouters) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update ROUTERS set "
            strSQL = strSQL + "NOME='" + prouters.Nome + "',"
            strSQL = strSQL + "MODELO='" + prouters.Modelo + "',"
            strSQL = strSQL + "ATIVO_FIXO='" + prouters.Ativo + "',"
            strSQL = strSQL + "BOOTROM_ATUAL='" + prouters.BootRom + "',"
            strSQL = strSQL + "CANAL_VOZ='" + prouters.Canal + "',"
            strSQL = strSQL + "IPPABX='" + prouters.IP_PABX + "',"
            strSQL = strSQL + "RELEASE='" + prouters.Release + "',"
            strSQL = strSQL + "VER_ATUAL='" + prouters.Versao + "',"
            strSQL = strSQL + "MARCA='" + prouters.Marca + "'"

            strSQL = strSQL + " where CODIGO_ROUTER = '" + prouters.Codigo_Router + "' "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function ExcluiRouter(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete ROUTERS "
            strSQL = strSQL + "where CODIGO_ROUTER = " + Convert.ToString(pcodigo)

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

    Public Function GetRouterById(ByVal pcodigo As Integer) As List(Of AppRouters)
        Dim connection As New OleDbConnection(strConn)
        Dim listLink As New List(Of AppRouters)

        Dim strSQL As String = "select CODIGO_ROUTER"
        strSQL = strSQL + ", nvl(NOME, '') AS NOME"
        strSQL = strSQL + ", nvl(MODELO, '') AS MODELO"
        strSQL = strSQL + ", nvl(VER_ATUAL, '') AS VERSAO"
        strSQL = strSQL + ", nvl(RELEASE, '') AS RELEASE"
        strSQL = strSQL + ", nvl(BOOTROM_ATUAL, '') AS BOOTROM"
        strSQL = strSQL + ", nvl(ATIVO_FIXO, '') AS ATIVO_FIXO"
        strSQL = strSQL + ", nvl(CANAL_VOZ, '') AS CANAL_VOZ"
        strSQL = strSQL + ", nvl(IPPABX, '') AS IPPABX"
        strSQL = strSQL + ", nvl(MARCA, '') AS MARCA"
        strSQL = strSQL + " FROM ROUTERS "
        strSQL = strSQL + " WHERE CODIGO_ROUTER ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRouters(reader.Item("CODIGO_ROUTER").ToString, reader.Item("NOME").ToString, reader.Item("MODELO").ToString, reader.Item("VERSAO").ToString, reader.Item("RELEASE").ToString, reader.Item("BOOTROM").ToString, reader.Item("ATIVO_FIXO").ToString, reader.Item("CANAL_VOZ").ToString, reader.Item("IPPABX").ToString, reader.Item("MARCA").ToString)
                listLink.Add(_registro)
            End While
        End Using

        Return listLink
    End Function

    Public Function ExcluiMarca(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete ROUTERS_MARCAS "
            strSQL = strSQL + "where COD_MARCA = " + Convert.ToString(pcodigo)

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

    Public Function ExcluiModelo(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete ROUTERS_MODELOS "
            strSQL = strSQL + "where COD_MODELO = " + Convert.ToString(pcodigo)

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

    Public Function InsereMarca(ByVal obj As AppGeneric) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into ROUTERS_MARCAS(COD_MARCA"
            strSQL = strSQL + ", MARCA) "
            strSQL = strSQL + "values ((select nvl(max(COD_MARCA),0)+1 from ROUTERS_MARCAS)"
            strSQL = strSQL + ",'" + obj.Descricao + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function AtualizaMarca(ByVal obj As AppGeneric) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim strSQL As String = "update ROUTERS_MARCAS set "
            strSQL = strSQL + "MARCA='" + obj.Descricao + "'"
            strSQL = strSQL + " where COD_MARCA = '" + obj.Codigo + "' "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function InsereModelo(ByVal obj As AppGeneric, ByVal codigo_marca As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into ROUTERS_MODELOS(COD_MODELO"
            strSQL = strSQL + ", MODELO, COD_MARCA) "
            strSQL = strSQL + "values ((select nvl(max(COD_MODELO),0)+1 from ROUTERS_MODELOS)"
            strSQL = strSQL + ",'" + obj.Descricao + "'"
            strSQL = strSQL + ",'" + codigo_marca + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function AtualizaModelo(ByVal obj As AppGeneric, ByVal codigo_marca As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update ROUTERS_MODELOS set "
            strSQL = strSQL + " MODELO='" + obj.Descricao + "',"
            strSQL = strSQL + " COD_MARCA='" + codigo_marca + "'"
            strSQL = strSQL + " where COD_MODELO = '" + obj.Codigo + "' "

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True

    End Function

    Public Function GetMarcaRouterById(ByVal pcodigo As Integer) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select COD_MARCA, MARCA"
        strSQL = strSQL + " FROM ROUTERS_MARCAS "
        strSQL = strSQL + " WHERE COD_MARCA ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("COD_MARCA").ToString, reader.Item("MARCA").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function GetModeloRouterById(ByVal pcodigo As Integer, ByRef cod_marca As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select COD_MARCA, COD_MODELO, MODELO"
        strSQL = strSQL + " FROM ROUTERS_MODELOS "
        strSQL = strSQL + " WHERE COD_MODELO ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("COD_MODELO").ToString, reader.Item("MODELO").ToString)
                cod_marca = reader.Item("COD_MARCA").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

End Class
