Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_Localidades

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

    Public Function GetLocalidades() As List(Of AppLocalidades)
        Dim connection As New OleDbConnection(strConn)
        Dim _list As New List(Of AppLocalidades)

        Dim strSQL As String = "select codigo,nvl(localidade,' ')localidade from localidades order by localidade "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                Dim _registro As New AppLocalidades(reader.Item("codigo").ToString, reader.Item("localidade").ToString)
                _list.Add(_registro)

            End While
        End Using

        Return _list
    End Function

    Public Function GetLocalidadeById(ByVal pcodigo As Integer) As List(Of AppLocalidades)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppLocalidades)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ", nvl(LOCALIDADE, '') AS LOCALIDADE"
        strSQL = strSQL + ", nvl(TELEFONE, '') AS TELEFONE"
        strSQL = strSQL + ", nvl(FAX, '') AS FAX"
        strSQL = strSQL + ", nvl(CANAL_VOZ, '') AS CANAL_VOZ"
        strSQL = strSQL + ", nvl(ENDERECO, '') AS ENDERECO"
        strSQL = strSQL + ", nvl(COMPLEMENTO, '') AS COMPLEMENTO"
        strSQL = strSQL + ", nvl(NUMERO, '') AS NUMERO"
        strSQL = strSQL + ", nvl(BAIRRO, '') AS BAIRRO"
        strSQL = strSQL + ", nvl(CIDADE, '') AS CIDADE"
        strSQL = strSQL + ", nvl(ESTADO, '') AS ESTADO"
        strSQL = strSQL + ", nvl(CEP, '') AS CEP"
        strSQL = strSQL + ", nvl(INS_ESTADUAL, '') AS INS_ESTADUAL"
        strSQL = strSQL + ", nvl(CNPJ, '') AS CNPJ"
        strSQL = strSQL + ", nvl(RESPONSAVEL, '') AS RESPONSAVEL"
        strSQL = strSQL + ", nvl(DATA_ABE, '') AS DATA_ABE"
        strSQL = strSQL + ", nvl(DATA_ENC, '') AS DATA_ENC"
        strSQL = strSQL + ", nvl(PARCEIRO, 0) AS PARCEIRO"
        strSQL = strSQL + ", nvl(INS_MUNIC, '') AS INS_MUNIC"
        strSQL = strSQL + ", nvl(CODIGO_CIDADE, 0) AS CODIGO_CIDADE"
        strSQL = strSQL + " FROM LOCALIDADES "
        strSQL = strSQL + " WHERE CODIGO='" + pcodigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLocalidades
                _registro.Codigo = reader.Item("CODIGO").ToString
                _registro.Localidade = reader.Item("LOCALIDADE").ToString
                _registro.Telefone = reader.Item("TELEFONE").ToString()
                _registro.Fax = reader.Item("FAX").ToString
                _registro.Canal_Voz = reader.Item("CANAL_VOZ").ToString
                _registro.Endereco = reader.Item("ENDERECO").ToString
                _registro.Complemento = reader.Item("COMPLEMENTO").ToString
                _registro.Numero = reader.Item("NUMERO").ToString
                _registro.Bairro = reader.Item("BAIRRO").ToString()
                _registro.Cidade = reader.Item("CIDADE").ToString()
                _registro.Estado = reader.Item("ESTADO").ToString()
                _registro.CEP = reader.Item("CEP").ToString()
                _registro.Ins_Estadual = reader.Item("INS_ESTADUAL").ToString()
                _registro.CNPJ = reader.Item("CNPJ").ToString()
                _registro.Responsavel = reader.Item("RESPONSAVEL").ToString()
                _registro.Data_Abe = reader.Item("DATA_ABE").ToString()
                _registro.Data_Enc = reader.Item("DATA_ENC").ToString()
                _registro.Parceiro = reader.Item("PARCEIRO").ToString()
                _registro.Ins_Municipal = reader.Item("INS_MUNIC").ToString()
                _registro.Codgo_Cidade = reader.Item("CODIGO_CIDADE").ToString()
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Function InsereLocalidade(ByVal _registro As AppLocalidades, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into LOCALIDADES(CODIGO"
            strSQL = strSQL + ", LOCALIDADE"
            strSQL = strSQL + ", TELEFONE"
            strSQL = strSQL + ", FAX"
            strSQL = strSQL + ", CANAL_VOZ"
            strSQL = strSQL + ", ENDERECO"
            strSQL = strSQL + ", COMPLEMENTO"
            strSQL = strSQL + ", NUMERO"
            strSQL = strSQL + ", BAIRRO"
            strSQL = strSQL + ", CIDADE"
            strSQL = strSQL + ", ESTADO"
            strSQL = strSQL + ", CEP"
            strSQL = strSQL + ", INS_ESTADUAL"
            strSQL = strSQL + ", CNPJ"
            strSQL = strSQL + ", RESPONSAVEL"
            strSQL = strSQL + ", DATA_ABE"
            strSQL = strSQL + ", DATA_ENC"
            strSQL = strSQL + ", PARCEIRO"
            strSQL = strSQL + ", INS_MUNIC"
            strSQL = strSQL + ", CODIGO_CIDADE"
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from LOCALIDADES)"
            strSQL = strSQL + ",'" + _registro.Localidade + "'"
            strSQL = strSQL + ",'" + _registro.Telefone + "'"
            strSQL = strSQL + ",'" + _registro.Fax + "'"
            strSQL = strSQL + ",'" + _registro.Canal_Voz + "'"
            strSQL = strSQL + ",'" + _registro.Endereco + "'"
            strSQL = strSQL + ",'" + _registro.Complemento + "'"
            strSQL = strSQL + ",'" + _registro.Numero + "'"
            strSQL = strSQL + ",'" + _registro.Bairro + "'"
            strSQL = strSQL + ",'" + _registro.Cidade + "'"
            strSQL = strSQL + ",'" + _registro.Estado + "'"
            strSQL = strSQL + ",'" + _registro.CEP + "'"
            strSQL = strSQL + ",'" + _registro.Ins_Estadual + "'"
            strSQL = strSQL + ",'" + _registro.CNPJ + "'"
            strSQL = strSQL + ",'" + _registro.Responsavel + "'"
            strSQL = strSQL + ",to_date('" + _registro.Data_Abe + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",to_date('" + _registro.Data_Enc + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + _registro.Parceiro + "'"
            strSQL = strSQL + ",'" + _registro.Ins_Municipal + "'"
            strSQL = strSQL + ",'" + _registro.Codgo_Cidade.ToString + "'"
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


        Return InsertLocalidadesLog(_registro, "N", log_string)
        Return True
    End Function

    Public Function AtualizaLocalidade(ByVal _registro As AppLocalidades, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim _registro_old As List(Of AppLocalidades) = GetLocalidadeById(_registro.Codigo)

        If InsertLocalidadesLog(_registro_old.Item(0), "A", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "update LOCALIDADES set "
            strSQL = strSQL + " LOCALIDADE='" + _registro.Localidade + "'"
            strSQL = strSQL + ", TELEFONE='" + _registro.Telefone + "'"
            strSQL = strSQL + ", FAX='" + _registro.Fax + "'"
            strSQL = strSQL + ", CANAL_VOZ='" + _registro.Canal_Voz + "'"
            strSQL = strSQL + ", ENDERECO='" + _registro.Endereco + "'"
            strSQL = strSQL + ", COMPLEMENTO='" + _registro.Complemento + "'"
            strSQL = strSQL + ", NUMERO='" + _registro.Numero + "'"
            strSQL = strSQL + ", BAIRRO='" + _registro.Bairro + "'"
            strSQL = strSQL + ", CIDADE='" + _registro.Cidade + "'"
            strSQL = strSQL + ", ESTADO='" + _registro.Estado + "'"
            strSQL = strSQL + ", CEP= '" + _registro.CEP + "'"
            strSQL = strSQL + ", RESPONSAVEL= '" + _registro.Responsavel + "'"
            strSQL = strSQL + ", INS_ESTADUAL= '" + _registro.Ins_Estadual + "'"
            strSQL = strSQL + ", CNPJ= '" + _registro.CNPJ + "'"
            strSQL = strSQL + ", DATA_ABE= to_date('" + _registro.Data_Abe + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", DATA_ENC= to_date('" + _registro.Data_Enc + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", PARCEIRO='" + _registro.Parceiro + "'"
            strSQL = strSQL + ", INS_MUNIC='" + _registro.Ins_Municipal + "'"
            strSQL = strSQL + ", CODIGO_CIDADE='" + _registro.Codgo_Cidade.ToString + "'"
            strSQL = strSQL + " where CODIGO  = '" + _registro.Codigo.ToString() + "'"

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

        If InsertLocalidadesLog(_registro, "B", log_string) = False Then
            Return False
        End If

        Return True

    End Function

    Public Function InsertLocalidadesLog(ByVal _registro As AppLocalidades, ByVal insert As Char, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into LOCALIDADES_LOG(CODIGO_LOG, TIPO_LOG, CODIGO"
            strSQL = strSQL + ", USUARIO "
            strSQL = strSQL + ", DATA_LOG "
            strSQL = strSQL + ", LOCALIDADE "
            strSQL = strSQL + ", TELEFONE "
            strSQL = strSQL + ", FAX "
            strSQL = strSQL + ", CANAL_VOZ "
            strSQL = strSQL + ", ENDERECO "
            strSQL = strSQL + ", COMPLEMENTO "
            strSQL = strSQL + ", NUMERO "
            strSQL = strSQL + ", BAIRRO "
            strSQL = strSQL + ", CIDADE "
            strSQL = strSQL + ", ESTADO "
            strSQL = strSQL + ", CEP "
            strSQL = strSQL + ", INS_ESTADUAL "
            strSQL = strSQL + ", CNPJ "
            strSQL = strSQL + ", RESPONSAVEL "
            strSQL = strSQL + ", DATA_ABE "
            strSQL = strSQL + ", DATA_ENC "
            strSQL = strSQL + ", PARCEIRO "
            strSQL = strSQL + ", INS_MUNIC "
            strSQL = strSQL + ", CODIGO_CIDADE "
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO_LOG),0)+1 from LOCALIDADES_LOG)"
            'Tipo_log
            strSQL = strSQL + ",'" + insert + "'"
            strSQL = strSQL + ",'" + _registro.Codigo.ToString() + "'"
            strSQL = strSQL + ",'" + log_string.Item(1).ToString + "'"
            strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + _registro.Localidade + "'"
            strSQL = strSQL + ",'" + _registro.Telefone + "'"
            strSQL = strSQL + ",'" + _registro.Fax + "'"
            strSQL = strSQL + ",'" + _registro.Canal_Voz + "'"
            strSQL = strSQL + ",'" + _registro.Endereco + "'"
            strSQL = strSQL + ",'" + _registro.Complemento + "'"
            strSQL = strSQL + ",'" + _registro.Numero + "'"
            strSQL = strSQL + ",'" + _registro.Bairro + "'"
            strSQL = strSQL + ",'" + _registro.Cidade + "'"
            strSQL = strSQL + ",'" + _registro.Estado + "'"
            strSQL = strSQL + ",'" + _registro.CEP + "'"
            strSQL = strSQL + ",'" + _registro.Ins_Estadual + "'"
            strSQL = strSQL + ",'" + _registro.CNPJ + "'"
            strSQL = strSQL + ",'" + _registro.Responsavel + "'"
            If _registro.Data_Abe = "" Then
                strSQL = strSQL + ",'" + _registro.Data_Abe + "'"
            Else
                strSQL = strSQL + ",'" + FormatDateTime(_registro.Data_Abe, DateFormat.ShortDate) + "'"

            End If
            If _registro.Data_Enc = "" Then
                strSQL = strSQL + ",'" + _registro.Data_Enc + "'"
            Else
                strSQL = strSQL + ",'" + FormatDateTime(_registro.Data_Enc, DateFormat.ShortDate) + "'"

            End If
            strSQL = strSQL + ",'" + _registro.Parceiro + "'"
            strSQL = strSQL + ",'" + _registro.Ins_Municipal + "'"
            strSQL = strSQL + ",'" + _registro.Codgo_Cidade.ToString + "'"
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

    Public Function ExcluiLocalidade(ByVal pcodigo As Integer, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim _registro As List(Of AppLocalidades) = GetLocalidadeById(pcodigo)
        If InsertLocalidadesLog(_registro.Item(0), "D", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "delete LOCALIDADES "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

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

End Class
