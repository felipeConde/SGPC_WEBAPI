Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System
Imports System.Collections.Generic


Public Class DAO_Fornecedores

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

    Public Function InsereFornecedor(ByVal _registro As AppFornecedor, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into FORNECEDORES(CODIGO"
            strSQL = strSQL + ", CONTATO_COMERCIAL "
            strSQL = strSQL + ", CONTATO_TECNICO "
            strSQL = strSQL + ", EMAIL_COMERCIAL "
            strSQL = strSQL + ", EMAIL_TECNICO "
            strSQL = strSQL + ", TELEFONE_TECNICO "
            strSQL = strSQL + ", TELEFONE_COMERCIAL "
            strSQL = strSQL + ", RAZAO_SOCIAL "
            strSQL = strSQL + ", NOME_FANTASIA "
            strSQL = strSQL + ", CNPJ "
            strSQL = strSQL + ", INS_ESTADUAL "
            strSQL = strSQL + ", ENDERECO "
            strSQL = strSQL + ", COMPLEMENTO "
            strSQL = strSQL + ", NUMERO "
            strSQL = strSQL + ", CEP "
            strSQL = strSQL + ", COD_TIPO_FORNECEDOR "
            strSQL = strSQL + ", DATAI "
            strSQL = strSQL + ", DATAR "
            strSQL = strSQL + ", BAIRRO "
            strSQL = strSQL + ", BANCO "
            strSQL = strSQL + ", AGENCIA "
            strSQL = strSQL + ", CONTA "
            strSQL = strSQL + ", INS_MUNIC "
            strSQL = strSQL + ", FAX "
            strSQL = strSQL + ", TOLL_FREE "
            strSQL = strSQL + ", CODIGO_CIDADE "
            strSQL = strSQL + ", CODIGO_OPERADORA "
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from FORNECEDORES)"
            strSQL = strSQL + ",'" + _registro.ContatoComercial + "'"
            strSQL = strSQL + ",'" + _registro.ContatoTecnico + "'"
            strSQL = strSQL + ",'" + _registro.EmailComercial + "'"
            strSQL = strSQL + ",'" + _registro.EmailTecnico + "'"
            strSQL = strSQL + ",'" + _registro.TelefoneTecnico + "'"
            strSQL = strSQL + ",'" + _registro.TelefoneComercial + "'"
            strSQL = strSQL + ",'" + _registro.RazaoSocial + "'"
            strSQL = strSQL + ",'" + _registro.NomeFantasia + "'"
            strSQL = strSQL + ",'" + _registro.CNPJ + "'"
            strSQL = strSQL + ",'" + _registro.InsEstadual + "'"
            strSQL = strSQL + ",'" + _registro.Endereco + "'"
            strSQL = strSQL + ",'" + _registro.Complemento + "'"
            strSQL = strSQL + ",'" + _registro.Numero + "'"
            strSQL = strSQL + ",'" + _registro.CEP + "'"
            strSQL = strSQL + ",'" + _registro.CodTipoFornecedor + "'"
            strSQL = strSQL + ",to_date('" + _registro.DataI + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",to_date('" + _registro.DataR + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + _registro.Bairro + "'"
            strSQL = strSQL + ",'" + _registro.Banco + "'"
            strSQL = strSQL + ",'" + _registro.Agencia + "'"
            strSQL = strSQL + ",'" + _registro.Conta + "'"
            strSQL = strSQL + ",'" + _registro.InsMunic + "'"
            strSQL = strSQL + ",'" + _registro.Fax + "'"
            strSQL = strSQL + ",'" + _registro.TollFree + "'"
            strSQL = strSQL + ",'" + _registro.CodigoCidade + "'"
            strSQL = strSQL + ",'" + _registro.CodigoOperadora + "'"

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

        If _registro.CodigoOperadora <> "" Then
            InsereForncTelef(_registro.CodigoOperadora)
        End If

        Return InsertFornecLog(_registro, "N", log_string)

        Return True

    End Function

    Public Function AtualizaFornecedor(ByVal _registro As AppFornecedor, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        If InsertFornecLog(_registro, "A", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "update FORNECEDORES set "
            strSQL = strSQL + "CONTATO_COMERCIAL='" + _registro.ContatoComercial.ToString() + "'"
            strSQL = strSQL + ", CONTATO_TECNICO='" + _registro.ContatoTecnico.ToString() + "'"
            strSQL = strSQL + ", EMAIL_COMERCIAL='" + _registro.EmailComercial.ToString() + "'"
            strSQL = strSQL + ", EMAIL_TECNICO='" + _registro.EmailTecnico.ToString() + "'"
            strSQL = strSQL + ", TELEFONE_TECNICO='" + _registro.TelefoneTecnico.ToString() + "'"
            strSQL = strSQL + ", TELEFONE_COMERCIAL='" + _registro.TelefoneComercial.ToString() + "'"
            strSQL = strSQL + ", RAZAO_SOCIAL='" + _registro.RazaoSocial.ToString() + "'"
            strSQL = strSQL + ", NOME_FANTASIA='" + _registro.NomeFantasia.ToString() + "'"
            strSQL = strSQL + ", CNPJ='" + _registro.CNPJ.ToString() + "'"
            strSQL = strSQL + ", INS_ESTADUAL='" + _registro.InsEstadual.ToString() + "'"
            strSQL = strSQL + ", ENDERECO='" + _registro.Endereco.ToString() + "'"
            strSQL = strSQL + ", COMPLEMENTO='" + _registro.Complemento.ToString() + "'"
            strSQL = strSQL + ", NUMERO='" + _registro.Numero.ToString() + "'"
            strSQL = strSQL + ", CEP='" + _registro.CEP.ToString() + "'"
            strSQL = strSQL + ", COD_TIPO_FORNECEDOR='" + _registro.CodTipoFornecedor.ToString() + "'"
            strSQL = strSQL + ", DATAI= to_date('" + _registro.DataI + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", DATAR= to_date('" + _registro.DataR + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", BAIRRO='" + _registro.Bairro.ToString() + "'"
            strSQL = strSQL + ", BANCO='" + _registro.Banco.ToString() + "'"
            strSQL = strSQL + ", AGENCIA='" + _registro.Agencia.ToString() + "'"
            strSQL = strSQL + ", CONTA='" + _registro.Conta.ToString() + "'"
            strSQL = strSQL + ", INS_MUNIC='" + _registro.InsMunic.ToString() + "'"
            strSQL = strSQL + ", FAX='" + _registro.Fax.ToString() + "'"
            strSQL = strSQL + ", TOLL_FREE='" + _registro.TollFree.ToString() + "'"
            strSQL = strSQL + ", CODIGO_CIDADE='" + _registro.CodigoCidade.ToString() + "'"
            strSQL = strSQL + ", CODIGO_OPERADORA='" + _registro.CodigoOperadora.ToString() + "'"

            strSQL = strSQL + " where CODIGO  = '" + _registro.Codigo.ToString() + "' "


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

        AtualizaForncTelef(_registro.Codigo, _registro.CodigoOperadora)

        Dim registro As List(Of AppFornecedor) = GetFornecedorById(_registro.Codigo)
        If InsertFornecLog(registro.Item(0), "B", log_string) = False Then
            Return False
        End If

        Return True

    End Function

    Public Function ExcluiFornecedor(ByVal pcodigo As Integer, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim registro As List(Of AppFornecedor) = GetFornecedorById(pcodigo)
        If InsertFornecLog(registro.Item(0), "D", log_string) = False Then
            Return False
        End If

        If ExcluiFornecedorOp(pcodigo) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "delete FORNECEDORES "
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function

    Public Function ExcluiFornecedorOp(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete FORNECEDORES_TELEFONIA "
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

    Public Function GetFornecedorById(ByVal pcodigo As Integer) As List(Of AppFornecedor)
        Dim connection As New OleDbConnection(strConn)
        Dim listForn As New List(Of AppFornecedor)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ", nvl(NOME_FANTASIA, '') as NOME_FANTASIA "
        strSQL = strSQL + ", nvl(CONTATO_COMERCIAL, '') AS CONTATO_COMERCIAL"
        strSQL = strSQL + ", nvl(CONTATO_TECNICO, '') AS CONTATO_TECNICO"
        strSQL = strSQL + ", nvl(EMAIL_COMERCIAL, '') AS EMAIL_COMERCIAL"
        strSQL = strSQL + ", nvl(EMAIL_TECNICO, '') AS EMAIL_TECNICO"
        strSQL = strSQL + ", nvl(TELEFONE_TECNICO, '') AS TELEFONE_TECNICO"
        strSQL = strSQL + ", nvl(TELEFONE_COMERCIAL, '') AS TELEFONE_COMERCIAL"
        strSQL = strSQL + ", nvl(RAZAO_SOCIAL, '') AS RAZAO_SOCIAL"
        strSQL = strSQL + ", nvl(CNPJ, '') AS CNPJ"
        strSQL = strSQL + ", nvl(INS_ESTADUAL, '') AS INS_ESTADUAL"
        strSQL = strSQL + ", nvl(ENDERECO, '') AS ENDERECO"
        strSQL = strSQL + ", nvl(COMPLEMENTO, '') AS COMPLEMENTO"
        strSQL = strSQL + ", nvl(NUMERO, '') AS NUMERO"
        strSQL = strSQL + ", nvl(CEP, '') AS CEP"
        strSQL = strSQL + ", nvl(COD_TIPO_FORNECEDOR, '') AS COD_TIPO_FORNECEDOR"
        strSQL = strSQL + ", nvl(DATAI, '') AS DATAI"
        strSQL = strSQL + ", nvl(DATAR, '') AS DATAR"
        strSQL = strSQL + ", nvl(BAIRRO, '') AS BAIRRO"
        strSQL = strSQL + ", nvl(BANCO, '') AS BANCO"
        strSQL = strSQL + ", nvl(AGENCIA, '') AS AGENCIA"
        strSQL = strSQL + ", nvl(CONTA, '') AS CONTA"
        strSQL = strSQL + ", nvl(INS_MUNIC, '') AS INS_MUNIC"
        strSQL = strSQL + ", nvl(FAX, '') AS FAX"
        strSQL = strSQL + ", nvl(TOLL_FREE, '') AS TOLL_FREE"
        strSQL = strSQL + ", nvl(CODIGO_CIDADE, '') AS CODIGO_CIDADE"
        strSQL = strSQL + ", nvl(CODIGO_OPERADORA, '') AS CODIGO_OPERADORA"
        strSQL = strSQL + " FROM FORNECEDORES "
        strSQL = strSQL + " WHERE CODIGO='" + pcodigo.ToString + "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppFornecedor(reader.Item("CODIGO").ToString, reader.Item("NOME_FANTASIA").ToString, reader.Item("CONTATO_COMERCIAL").ToString, reader.Item("CONTATO_TECNICO").ToString, reader.Item("EMAIL_COMERCIAL").ToString, reader.Item("EMAIL_TECNICO").ToString, reader.Item("TELEFONE_TECNICO").ToString, reader.Item("TELEFONE_COMERCIAL").ToString, reader.Item("RAZAO_SOCIAL").ToString, reader.Item("CNPJ").ToString, reader.Item("INS_ESTADUAL").ToString, reader.Item("ENDERECO").ToString, reader.Item("COMPLEMENTO").ToString, reader.Item("NUMERO").ToString, reader.Item("CEP").ToString, reader.Item("COD_TIPO_FORNECEDOR").ToString, reader.Item("DATAI").ToString, reader.Item("DATAR").ToString, reader.Item("BAIRRO").ToString, reader.Item("BANCO").ToString, reader.Item("AGENCIA").ToString, reader.Item("CONTA").ToString, reader.Item("INS_MUNIC").ToString, reader.Item("FAX").ToString, reader.Item("TOLL_FREE").ToString, reader.Item("CODIGO_CIDADE").ToString, reader.Item("CODIGO_OPERADORA").ToString)
                listForn.Add(_registro)
            End While
        End Using

        Return listForn
    End Function


    Public Function ComboUfs() As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listUfs As New List(Of String)

        Dim strSQL As String = "select DISTINCT UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "order by UF"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("UF").ToString)
                listUfs.Add(_registro)
            End While
        End Using

        Return listUfs
    End Function

    Public Function AssinalaUf(ByVal codigo_cidade As String) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listUfs As New List(Of String)

        Dim strSQL As String = "select UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "where CODIGO_CIDADE='" + codigo_cidade + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("UF").ToString)
                listUfs.Add(_registro)
            End While
        End Using

        Return listUfs
    End Function


    Public Function ComboCidades(ByVal Uf As String) As List(Of AppCidades)
        Dim connection As New OleDbConnection(strConn)
        Dim listCidades As New List(Of AppCidades)

        Dim strSQL As String = "select MUNICIPIO, CODIGO_CIDADE, UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "where UF='" + Uf + "' "
        strSQL = strSQL + "order by MUNICIPIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppCidades(reader.Item("CODIGO_CIDADE").ToString, reader.Item("MUNICIPIO").ToString, reader.Item("UF").ToString)
                listCidades.Add(_registro)
            End While
        End Using

        Return listCidades
    End Function

    Public Function ComboTipos() As List(Of AppFornecedoresTipo)
        Dim connection As New OleDbConnection(strConn)
        Dim tipos As New List(Of AppFornecedoresTipo)

        Dim strSQL As String = "select CODIGO, TIPO_FORNECEDOR "
        strSQL = strSQL + "from FORNECEDORES_TIPOS "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppFornecedoresTipo(reader.Item("CODIGO").ToString, reader.Item("TIPO_FORNECEDOR").ToString)
                tipos.Add(_registro)
            End While
        End Using

        Return tipos
    End Function

    Public Sub InsereForncTelef(ByVal codigo_op As String)
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into FORNECEDORES_TELEFONIA(CODIGO"
            strSQL = strSQL + ", CODIGO_OPERADORA "
            strSQL = strSQL + ") "
            strSQL = strSQL + "values("
            strSQL = strSQL + "(select max(CODIGO) from FORNECEDORES)"
            strSQL = strSQL + ",'" + codigo_op + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
        End Try

    End Sub

    Public Sub InsereForncTelef2(ByVal codigo As String, ByVal codigo_op As String)
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into FORNECEDORES_TELEFONIA(CODIGO"
            strSQL = strSQL + ", CODIGO_OPERADORA "
            strSQL = strSQL + ") "
            strSQL = strSQL + "values("
            strSQL = strSQL + "'" + codigo + "'"
            strSQL = strSQL + ",'" + codigo_op + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
        End Try

    End Sub


    Public Function GetFornedores() As List(Of AppFornecedor)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppFornecedor)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_fantasia as descricao "
        strSQL = strSQL + " from fornecedores o"
        strSQL = strSQL + " order by nome_fantasia"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppFornecedor(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
    End Function

    Public Function GetFornecedoresLivres(ByVal codigo As String) As List(Of AppFornecedor)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppFornecedor)

        Dim strSQL As String = ""
        strSQL = strSQL + "select o.CODIGO, o.nome_fantasia as descricao "
        strSQL = strSQL + " from fornecedores o, linhas l, links li"
        strSQL = strSQL + " where li.CODIGO_FORNECEDOR ='" + codigo + "' or l.CODIGO_FORNECEDOR ='" + codigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppFornecedor(reader.Item("CODIGO").ToString, reader.Item("descricao").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list
    End Function

    Public Sub AtualizaForncTelef(ByVal codigo As String, ByVal codigo_op As String)
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "update FORNECEDORES_TELEFONIA set "
            strSQL = strSQL + " CODIGO_OPERADORA ='" + codigo_op + "'"
            strSQL = strSQL + " where CODIGO='" + codigo.ToString() + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
        End Try

    End Sub

    Public Function GetComboOperadoras(ByVal codigo As String) As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = ""
        strSQL = strSQL + "select distinct o.CODIGO, o.DESCRICAO"
        strSQL = strSQL + " from OPERADORAS_TESTE o "
        strSQL = strSQL + " where o.CODIGO not in (select CODIGO_OPERADORA from FORNECEDORES_telefonia where CODIGO <> '" + codigo + "') "
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

    Public Function InsertFornecLog(ByVal _registro As AppFornecedor, ByVal insert As Char, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into FORNECEDORES_LOG(CODIGO"
            strSQL = strSQL + ", TIPO_LOG"
            strSQL = strSQL + ", CODIGO_FORNECEDOR"
            strSQL = strSQL + ", CONTATO_COMERCIAL"
            strSQL = strSQL + ", CONTATO_TECNICO"
            strSQL = strSQL + ", EMAIL_COMERCIAL"
            strSQL = strSQL + ", EMAIL_TECNICO"
            strSQL = strSQL + ", TELEFONE_TECNICO"
            strSQL = strSQL + ", TELEFONE_COMERCIAL"
            strSQL = strSQL + ", RAZAO_SOCIAL"
            strSQL = strSQL + ", NOME_FANTASIA"
            strSQL = strSQL + ", CNPJ"
            strSQL = strSQL + ", INS_ESTADUAL"
            strSQL = strSQL + ", ENDERECO"
            strSQL = strSQL + ", COMPLEMENTO"
            strSQL = strSQL + ", NUMERO"
            strSQL = strSQL + ", CEP"
            strSQL = strSQL + ", TIPO_FORNECEDOR"
            strSQL = strSQL + ", DATAI"
            strSQL = strSQL + ", DATAR"
            strSQL = strSQL + ", BAIRRO"
            strSQL = strSQL + ", BANCO"
            strSQL = strSQL + ", AGENCIA"
            strSQL = strSQL + ", CONTA"
            strSQL = strSQL + ", INS_MUNIC"
            strSQL = strSQL + ", FAX"
            strSQL = strSQL + ", TOLL_FREE"
            strSQL = strSQL + ", CIDADE"
            strSQL = strSQL + ", OPERADORA"
            strSQL = strSQL + ", USUARIO "
            strSQL = strSQL + ", DATA_LOG "
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO),0)+1 from FORNECEDORES_LOG)"

            'TIPO
            If insert = "A" Then
                Dim _registro_old As List(Of AppFornecedor) = GetFornecedorById(_registro.Codigo)
                strSQL = strSQL + ",'" + insert + "'"

                strSQL = strSQL + ",'" + _registro_old.Item(0).Codigo.ToString + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).ContatoComercial + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).ContatoTecnico + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).EmailComercial + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).EmailTecnico + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).TelefoneTecnico + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).TelefoneComercial + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).RazaoSocial + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).NomeFantasia + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).CNPJ + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).InsEstadual + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Endereco + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Complemento + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Numero + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).CEP + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).CodTipoFornecedor + "'"
                strSQL = strSQL + ",to_date('" + _registro_old.Item(0).DataI + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",to_date('" + _registro_old.Item(0).DataR + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Bairro + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Banco + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Agencia + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Conta + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).InsMunic + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).Fax + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).TollFree + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).CodigoCidade + "'"
                strSQL = strSQL + ",'" + _registro_old.Item(0).CodigoOperadora + "'"
                strSQL = strSQL + ",'" + log_string.Item(0).ToString + "'"
                strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"

                strSQL = strSQL + ")"

            Else
                strSQL = strSQL + ",'" + insert + "'"

                If insert = "N" Then
                    strSQL = strSQL + ",(select nvl(max(CODIGO),0) from FORNECEDORES)"
                Else
                    strSQL = strSQL + ",'" + _registro.Codigo.ToString + "'"
                End If
                strSQL = strSQL + ",'" + _registro.ContatoComercial + "'"
                strSQL = strSQL + ",'" + _registro.ContatoTecnico + "'"
                strSQL = strSQL + ",'" + _registro.EmailComercial + "'"
                strSQL = strSQL + ",'" + _registro.EmailTecnico + "'"
                strSQL = strSQL + ",'" + _registro.TelefoneTecnico + "'"
                strSQL = strSQL + ",'" + _registro.TelefoneComercial + "'"
                strSQL = strSQL + ",'" + _registro.RazaoSocial + "'"
                strSQL = strSQL + ",'" + _registro.NomeFantasia + "'"
                strSQL = strSQL + ",'" + _registro.CNPJ + "'"
                strSQL = strSQL + ",'" + _registro.InsEstadual + "'"
                strSQL = strSQL + ",'" + _registro.Endereco + "'"
                strSQL = strSQL + ",'" + _registro.Complemento + "'"
                strSQL = strSQL + ",'" + _registro.Numero + "'"
                strSQL = strSQL + ",'" + _registro.CEP + "'"
                strSQL = strSQL + ",'" + _registro.CodTipoFornecedor + "'"
                strSQL = strSQL + ",to_date('" + _registro.DataI + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",to_date('" + _registro.DataR + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",'" + _registro.Bairro + "'"
                strSQL = strSQL + ",'" + _registro.Banco + "'"
                strSQL = strSQL + ",'" + _registro.Agencia + "'"
                strSQL = strSQL + ",'" + _registro.Conta + "'"
                strSQL = strSQL + ",'" + _registro.InsMunic + "'"
                strSQL = strSQL + ",'" + _registro.Fax + "'"
                strSQL = strSQL + ",'" + _registro.TollFree + "'"
                strSQL = strSQL + ",'" + _registro.CodigoCidade + "'"
                strSQL = strSQL + ",'" + _registro.CodigoOperadora + "'"
                strSQL = strSQL + ",'" + log_string.Item(0).ToString + "'"
                strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"

                strSQL = strSQL + ")"
            End If


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


End Class
