Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Ativos

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


    Public Function Insert(ByVal registro As AppAtivos, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = "insert into Ativos(CODIGO"
            strSQL = strSQL + ",  TIPO,CLIENT_NAME, MARCA, MODELO, SERIAL_NUMBER, CENTRO_DE_CUSTO, NUM_NOTA_FISC"
            strSQL = strSQL + ", TIPO_CONTRATO,NUMERO_CONTRATO,INICIO_CONTRATO,TERMINO_CONTRATO,STATUS_DISPOSITIVO"
            strSQL = strSQL + ",DATA_ULTIMO_STATUS,OBS,CODIGO_USUARIO,CODIGO_FORNECEDOR,CUSTO,GARANTIA,GARANTIA_ESTENDIDA, PATRIMONIO ) "
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from Ativos)"
            strSQL = strSQL + ",'" + registro.Tipo + "'"
            strSQL = strSQL + ",'" + registro.Client_name + "'"
            strSQL = strSQL + ",'" + registro.Marca + "'"
            strSQL = strSQL + ",'" + registro.Modelo + "'"
            strSQL = strSQL + ",'" + registro.Serial_Number + "'"
            strSQL = strSQL + ",'" + registro.Centro_Custo + "'"
            strSQL = strSQL + ",'" + registro.Num_nota_fisc + "'"
            strSQL = strSQL + ",'" + registro.Tipo_Contrato + "'"
            strSQL = strSQL + ",'" + registro.Numero_Contrato + "'"
            strSQL = strSQL + ",'" + registro.Inicio_Contrato + "'"
            strSQL = strSQL + ",'" + registro.Termino_contrato + "'"
            strSQL = strSQL + ",'" + registro.Status_dispositivo + "'"
            strSQL = strSQL + ",'" + registro.Data_ultimo_status + "'"
            strSQL = strSQL + ",'" + registro.Obs + "'"
            strSQL = strSQL + ",'" + registro.Codigo_usuario + "'"
            strSQL = strSQL + ",'" + registro.Codigo_fornecedor + "'"
            strSQL = strSQL + ",'" + Replace(registro.Custo.Replace(".", ""), ",", ".") + "'"
            strSQL = strSQL + ",'" + registro.Garantia + "'"
            strSQL = strSQL + ",'" + registro.Garantia_estendida + "'"
            strSQL = strSQL + ",'" + registro.Patrimonio + "'"
            strSQL = strSQL + ")"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("(select nvl(max(CODIGO),0) from ATIVOS)", "N", usuario)
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

    Public Function Update(ByVal registro As AppAtivos, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = String_log("'" & registro.Codigo & "'", "A", usuario)
            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "update Ativos set "
            strSQL = strSQL + "TIPO='" + registro.Tipo + "',"
            strSQL = strSQL + "CLIENT_NAME='" + registro.Client_name + "',"
            strSQL = strSQL + "MARCA='" + registro.Marca + "',"
            strSQL = strSQL + "MODELO='" + registro.Modelo + "',"
            strSQL = strSQL + "SERIAL_NUMBER='" + registro.Serial_Number + "',"
            strSQL = strSQL + "CENTRO_DE_CUSTO='" + registro.Centro_Custo + "',"
            strSQL = strSQL + "NUM_NOTA_FISC='" + registro.Num_nota_fisc + "',"
            strSQL = strSQL + "TIPO_CONTRATO='" + registro.Tipo_Contrato + "',"
            strSQL = strSQL + "NUMERO_CONTRATO='" + registro.Numero_Contrato + "',"
            strSQL = strSQL + "INICIO_CONTRATO='" + registro.Inicio_Contrato + "',"
            strSQL = strSQL + "TERMINO_CONTRATO='" + registro.Termino_contrato + "',"
            strSQL = strSQL + "STATUS_DISPOSITIVO='" + registro.Status_dispositivo + "',"
            strSQL = strSQL + "DATA_ULTIMO_STATUS='" + registro.Data_ultimo_status + "',"
            strSQL = strSQL + "CODIGO_USUARIO='" + registro.Codigo_usuario + "',"
            strSQL = strSQL + "CODIGO_FORNECEDOR='" + registro.Codigo_fornecedor + "',"
            strSQL = strSQL + "CUSTO='" + Replace(registro.Custo.Replace(".", ""), ",", ".") + "',"
            strSQL = strSQL + "GARANTIA='" + registro.Garantia + "',"
            strSQL = strSQL + "GARANTIA_ESTENDIDA='" + registro.Garantia_estendida + "',"
            strSQL = strSQL + "OBS='" + registro.Obs + "',"
            strSQL = strSQL + "PATRIMONIO='" + registro.Patrimonio + "'"

            strSQL = strSQL + " where CODIGO = '" + registro.Codigo + "' "

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = String_log("'" & registro.Codigo & "'", "B", usuario)
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

    Public Function Excluir(ByVal pcodigo As Integer, ByVal usuario As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand


        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = String_log("'" & pcodigo & "'", "D", usuario)

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "delete Ativos "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

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

    Public Function GetAtivoByCodigo(ByVal pcodigo As Integer) As List(Of AppAtivos)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppAtivos)

        Dim strSQL As String = "select codigo"
        strSQL = strSQL + ", nvl(TIPO, '') AS TIPO"
        strSQL = strSQL + ", nvl(CLIENT_NAME, '') AS CLIENT_NAME"
        strSQL = strSQL + ", nvl(MARCA, '') AS MARCA"
        strSQL = strSQL + ", nvl(MODELO, '') AS MODELO"
        strSQL = strSQL + ", nvl(SERIAL_NUMBER, '') AS SERIAL_NUMBER"
        strSQL = strSQL + ", nvl(CENTRO_DE_CUSTO, '') AS CENTRO_DE_CUSTO"
        strSQL = strSQL + ", nvl(NUM_NOTA_FISC, '') AS NUM_NOTA_FISC"
        strSQL = strSQL + ", nvl(TIPO_CONTRATO, '') AS TIPO_CONTRATO"
        strSQL = strSQL + ", nvl(NUMERO_CONTRATO, '') AS NUMERO_CONTRATO"
        strSQL = strSQL + ", nvl(INICIO_CONTRATO, '') AS INICIO_CONTRATO"
        strSQL = strSQL + ", nvl(TERMINO_CONTRATO, '') AS TERMINO_CONTRATO"
        strSQL = strSQL + ", nvl(STATUS_DISPOSITIVO, '') AS STATUS_DISPOSITIVO"
        strSQL = strSQL + ", nvl(DATA_ULTIMO_STATUS, '') AS DATA_ULTIMO_STATUS"
        strSQL = strSQL + ", nvl(OBS, '') AS OBS"
        strSQL = strSQL + ", nvl(CODIGO_USUARIO, '') AS CODIGO_USUARIO"
        strSQL = strSQL + ", nvl(CODIGO_FORNECEDOR, '') AS CODIGO_FORNECEDOR"
        strSQL = strSQL + ", nvl(CUSTO, '') AS CUSTO"
        strSQL = strSQL + ", nvl(GARANTIA, '') AS GARANTIA"
        strSQL = strSQL + ", nvl(GARANTIA_ESTENDIDA, '') AS GARANTIA_ESTENDIDA"
        strSQL = strSQL + ", nvl(PATRIMONIO, '') AS PATRIMONIO"
        strSQL = strSQL + " FROM ATIVOS "
        strSQL = strSQL + " WHERE codigo ='" + pcodigo.ToString() + "' "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppAtivos()

                _registro.Tipo = reader.Item("TIPO").ToString
                _registro.Client_name = reader.Item("CLIENT_NAME").ToString
                _registro.Marca = reader.Item("MARCA").ToString
                _registro.Modelo = reader.Item("MODELO").ToString
                _registro.Serial_Number = reader.Item("SERIAL_NUMBER").ToString
                _registro.Centro_Custo = reader.Item("CENTRO_DE_CUSTO").ToString
                _registro.Num_nota_fisc = reader.Item("NUM_NOTA_FISC").ToString
                _registro.Tipo_Contrato = reader.Item("TIPO_CONTRATO").ToString
                _registro.Numero_Contrato = reader.Item("NUMERO_CONTRATO").ToString
                _registro.Inicio_Contrato = reader.Item("INICIO_CONTRATO").ToString
                _registro.Termino_contrato = reader.Item("TERMINO_CONTRATO").ToString
                _registro.Status_dispositivo = reader.Item("STATUS_DISPOSITIVO").ToString
                _registro.Data_ultimo_status = reader.Item("DATA_ULTIMO_STATUS").ToString
                _registro.Obs = reader.Item("OBS").ToString
                _registro.Codigo_usuario = reader.Item("CODIGO_USUARIO").ToString
                _registro.Codigo_fornecedor = reader.Item("CODIGO_FORNECEDOR").ToString
                _registro.Custo = reader.Item("CUSTO").ToString
                _registro.Garantia = reader.Item("GARANTIA").ToString
                _registro.Garantia_estendida = reader.Item("GARANTIA_ESTENDIDA").ToString
                _registro.Patrimonio = reader.Item("PATRIMONIO").ToString
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function String_log(ByVal pcodigo As String, ByVal tipo_log As Char, ByVal usuario As String) As String
        Dim sql As String = ""

        sql = "insert into Ativos_log(codigo_log, usuario_log, data_log, tipo_log, "
        sql = sql + "CODIGO,TIPO,CLIENT_NAME "
        sql = sql + " ,MARCA,MODELO,SERIAL_NUMBER,CENTRO_DE_CUSTO,NUM_NOTA_FISC,TIPO_CONTRATO,NUMERO_CONTRATO "
        sql = sql + " ,INICIO_CONTRATO,TERMINO_CONTRATO,STATUS_DISPOSITIVO,DATA_ULTIMO_STATUS"
        sql = sql + " ,OBS,CODIGO_USUARIO,CODIGO_FORNECEDOR,CUSTO,GARANTIA,GARANTIA_ESTENDIDA, PATRIMONIO )  "
        sql = sql + " values ( (select nvl(max(codigo_log),0)+1 from Ativos_log),'" + usuario + "',"
        sql = sql + " to_date('" + DateTime.Now.ToString + "','dd/mm/yyyy hh24:mi:ss'),"
        sql = sql + "'" + tipo_log + "',"
        If pcodigo <> "" Then
            sql = sql + "" + pcodigo + ","
            sql = sql + " (select TIPO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CLIENT_NAME from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select MARCA from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select MODELO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select SERIAL_NUMBER from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CENTRO_DE_CUSTO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select NUM_NOTA_FISC from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select TIPO_CONTRATO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select NUMERO_CONTRATO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select INICIO_CONTRATO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select TERMINO_CONTRATO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select STATUS_DISPOSITIVO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select DATA_ULTIMO_STATUS from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select OBS from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CODIGO_USUARIO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CODIGO_FORNECEDOR from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select CUSTO from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select GARANTIA from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select GARANTIA_ESTENDIDA from ATIVOS where CODIGO=" + pcodigo + "),"
            sql = sql + " (select PATRIMONIO from ATIVOS where CODIGO=" + pcodigo + "))"
        End If
        Return sql
    End Function

End Class
