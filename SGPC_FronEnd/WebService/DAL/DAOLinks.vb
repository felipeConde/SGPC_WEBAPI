Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System

Public Class DAOLinks

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

    Public Function ComboStatus() As List(Of String)
        Dim status As New List(Of String)

        status.Add("Ativado")
        status.Add("Desativado")
        status.Add("Agendamento")
        status.Add("Cancelado")

        Return status
    End Function

    Public Function ComboCCusto() As List(Of CCusto)
        Dim connection As New OleDbConnection(strConn)
        Dim CCustoList As New List(Of CCusto)

        Dim strSQL As String = "select CODIGO, NOME_GRUPO "
        strSQL = strSQL + "from GRUPOS"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New CCusto(reader.Item("CODIGO").ToString, reader.Item("NOME_GRUPO").ToString)
                CCustoList.Add(_registro)
            End While
        End Using

        Return CCustoList
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

    Public Function ComboMarcasRouters() As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select COD_MARCA, MARCA "
        strSQL = strSQL + "from ROUTERS_MARCAS "
        strSQL = strSQL + "order by MARCA"

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

    Public Function ComboModelosRouters(ByVal marca As String) As List(Of AppGeneric)
        Dim connection As New OleDbConnection(strConn)
        Dim list As New List(Of AppGeneric)

        Dim strSQL As String = "select COD_MODELO, MODELO "
        strSQL = strSQL + "from ROUTERS_MODELOS "
        strSQL = strSQL + "where COD_MARCA='" + marca + "' "
        strSQL = strSQL + "order by MODELO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppGeneric(reader.Item("COD_MODELO").ToString, reader.Item("MODELO").ToString)
                list.Add(_registro)
            End While
        End Using

        Return list

    End Function

    Public Function ComboRouters() As List(Of AppRouters)
        Dim connection As New OleDbConnection(strConn)
        Dim listRouter As New List(Of AppRouters)

        Dim strSQL As String = "select CODIGO_ROUTER AS CODIGO"
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

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRouters(reader.Item("CODIGO").ToString, reader.Item("NOME").ToString, reader.Item("MODELO").ToString, reader.Item("VERSAO").ToString, reader.Item("RELEASE").ToString, reader.Item("BOOTROM").ToString, reader.Item("ATIVO_FIXO").ToString, reader.Item("CANAL_VOZ").ToString, reader.Item("IPPABX").ToString, reader.Item("MARCA").ToString)
                listRouter.Add(_registro)
            End While
        End Using

        Return listRouter
    End Function

    Public Function CarregaRouters(ByVal router As String) As List(Of AppRouters)
        Dim connection As New OleDbConnection(strConn)
        Dim listRouter As New List(Of AppRouters)

        Dim strSQL As String = "select CODIGO_ROUTER AS CODIGO"
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
        strSQL = strSQL + " WHERE CODIGO_ROUTER = '" + router.ToString() + "' "


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppRouters(reader.Item("CODIGO").ToString, reader.Item("NOME").ToString, reader.Item("MODELO").ToString, reader.Item("VERSAO").ToString, reader.Item("RELEASE").ToString, reader.Item("BOOTROM").ToString, reader.Item("ATIVO_FIXO").ToString, reader.Item("CANAL_VOZ").ToString, reader.Item("IPPABX").ToString, reader.Item("MARCA").ToString)
                listRouter.Add(_registro)
            End While
        End Using

        Return listRouter
    End Function

    Public Function ComboOperadoras() As List(Of AppOperadoras)
        Dim connection As New OleDbConnection(strConn)
        Dim listOperadoras As New List(Of AppOperadoras)

        Dim strSQL As String = "select CODIGO, nvl(NOME_FANTASIA,'') AS NOME_FANTASIA from FORNECEDORES order by NOME_FANTASIA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppOperadoras(reader.Item("CODIGO").ToString, reader.Item("NOME_FANTASIA").ToString)
                listOperadoras.Add(_registro)
            End While
        End Using

        Return listOperadoras
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

    Public Function GetLocalidade(ByVal codigo As Integer) As List(Of AppLocalidades)
        Dim connection As New OleDbConnection(strConn)
        Dim localidades As New List(Of AppLocalidades)

        Dim strSQL As String = "select CODIGO, LOCALIDADE "
        strSQL = strSQL + "from LOCALIDADES "
        strSQL = strSQL + "WHERE CODIGO='" + codigo.ToString + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppLocalidades(reader.Item("CODIGO").ToString, reader.Item("LOCALIDADE").ToString)
                localidades.Add(_registro)
            End While
        End Using

        Return localidades
    End Function


    Public Function InsereLink(ByVal link As AppLinks, ByVal log_string As List(Of String), ByVal ccustos_rateio_list As List(Of AppGrupo)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into LINKS(CODIGO_LINK"
            If link.Codigo_Fornecedor <> 0 Then
                strSQL = strSQL + ", CODIGO_FORNECEDOR "
            End If
            strSQL = strSQL + ", LOCALIDADE "
            strSQL = strSQL + ", COD_LOCAL "
            strSQL = strSQL + ", COD_TIPO_LINK "
            strSQL = strSQL + ", DESIG_CIR "
            strSQL = strSQL + ", DESIG_PAG "
            strSQL = strSQL + ", ANT_PORTA "
            strSQL = strSQL + ", ANT_CIR "
            strSQL = strSQL + ", ATUAL_PORTA "
            strSQL = strSQL + ", ATUAL_CIR "
            strSQL = strSQL + ", CODIGO_CIDADE "
            strSQL = strSQL + ", UF "
            strSQL = strSQL + ", CEP "
            strSQL = strSQL + ", TELEFONE "
            strSQL = strSQL + ", CONTATO "
            strSQL = strSQL + ", REGIAO "
            strSQL = strSQL + ", DLCI_MAT "
            strSQL = strSQL + ", DLCI_REM "
            strSQL = strSQL + ", LMI_REM "
            strSQL = strSQL + ", DATA_ATI "
            strSQL = strSQL + ", DATA_DES "
            strSQL = strSQL + ", STATUS "
            strSQL = strSQL + ", SALVA "
            strSQL = strSQL + ", WAN "
            strSQL = strSQL + ", OBS "
            strSQL = strSQL + ", CODIGO_ROUTER "
            strSQL = strSQL + ", CODIGO_WAN "
            strSQL = strSQL + ", CODIGO_LAN "
            strSQL = strSQL + ", ENDERECO_B "
            strSQL = strSQL + ", VALOR_ATIV "
            strSQL = strSQL + ", VALOR_LINK "
            strSQL = strSQL + ", COD_CLIENTE_CONTA "
            strSQL = strSQL + ", NUM_FATURA "
            strSQL = strSQL + ", NUM_CONTRATO_TVG "
            strSQL = strSQL + ", NUM_CONTRATO_OP "
            strSQL = strSQL + ", DATA_IPCA "
            strSQL = strSQL + ", VALOR_MENSAL "
            strSQL = strSQL + ", SPONSOR "
            strSQL = strSQL + ", ENDERECO_COB "
            strSQL = strSQL + ", NUMERO_OC"
            strSQL = strSQL + ", UF_B"
            strSQL = strSQL + ", CODIGO_CIDADE_B"
            strSQL = strSQL + ", NUM_CONTRATO_OP_AS"
            strSQL = strSQL + ", CONTATO_B"
            strSQL = strSQL + ", TEL_CONT_A"
            strSQL = strSQL + ", EMAIL_CONT_A"
            strSQL = strSQL + ", TEL_CONT_B"
            strSQL = strSQL + ", EMAIL_CONT_B"
            strSQL = strSQL + ", CODIGO_SUCURSAL"
            strSQL = strSQL + ", CONTA_CONTABIL"
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO_LINK),0)+1 from LINKS)"
            If link.Codigo_Fornecedor <> 0 Then
                strSQL = strSQL + ",'" + link.Codigo_Fornecedor.ToString() + "'"
            End If
            strSQL = strSQL + ",'" + link.Localidade + "'"
            strSQL = strSQL + ",'" + link.Cod_local.ToString + "'"
            strSQL = strSQL + ",'" + link.Tipo_Link + "'"
            strSQL = strSQL + ",'" + link.Desig_Cir + "'"
            strSQL = strSQL + ",'" + link.Desig_Pag + "'"
            'strSQL = strSQL + ",'" + link.Codigo_CCusto + "'"
            strSQL = strSQL + ",'" + link.Ant_Porta + "'"
            strSQL = strSQL + ",'" + link.Ant_Cir + "'"
            strSQL = strSQL + ",'" + link.Atual_Porta + "'"
            strSQL = strSQL + ",'" + link.Atual_Cir + "'"
            strSQL = strSQL + ",'" + link.Codigo_Cidade.ToString() + "'"
            strSQL = strSQL + ",'" + link.Uf + "'"
            strSQL = strSQL + ",'" + link.Cep + "'"
            strSQL = strSQL + ",'" + link.Telefone + "'"
            strSQL = strSQL + ",'" + link.Contato + "'"
            strSQL = strSQL + ",'" + link.Regiao + "'"
            strSQL = strSQL + ",'" + link.Dlci_Mat + "'"
            strSQL = strSQL + ",'" + link.Dlci_Rem + "'"
            strSQL = strSQL + ",'" + link.Lmi_Rem + "'"
            strSQL = strSQL + ",to_date('" + link.Data_Ati + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",to_date('" + link.Data_Des + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + link.Status.ToString() + "'"
            strSQL = strSQL + ",'" + link.Salva + "'"
            strSQL = strSQL + ",'" + link.Protocolo + "'"
            strSQL = strSQL + ",'" + link.OBS_ + "'"
            If link.Codigo_Router <> 0 Then
                strSQL = strSQL + ",'" + link.Codigo_Router.ToString() + "'"
            Else
                strSQL = strSQL + ",''"
            End If
            If link.Codigo_Wan <> 0 Then
                strSQL = strSQL + ",'" + link.Codigo_Wan.ToString() + "'"
            Else
                strSQL = strSQL + ",''"
            End If
            If link.Codigo_Lan <> 0 Then
                strSQL = strSQL + ",'" + link.Codigo_Lan.ToString() + "'"
            Else
                strSQL = strSQL + ",''"
            End If
            strSQL = strSQL + ",'" + link.ENDERECO_B_ + "'"
            strSQL = strSQL + ",'" + link.Valor_Ativ + "'"
            strSQL = strSQL + ",'" + link.Valor_Link + "'"
            strSQL = strSQL + ",'" + link.Cod_cliente_conta + "'"
            strSQL = strSQL + ",'" + link.NumeroFatura + "'"
            strSQL = strSQL + ",'" + link.NumeroContratoTVG + "'"
            strSQL = strSQL + ",'" + link.NumeroContratoOP + "'"
            strSQL = strSQL + ",to_date('" + link.DataIPCA + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + link.ValorMensal + "'"
            strSQL = strSQL + ",'" + link.Sponsor + "'"
            strSQL = strSQL + ",'" + link.Endereco_cob + "'"
            strSQL = strSQL + ",'" + link.NumeroOC + "'"
            strSQL = strSQL + ",'" + link.UF_B + "'"
            strSQL = strSQL + ",'" + link.Codigo_Cidade_B + "'"
            strSQL = strSQL + ",'" + link.Num_Contrato_Op_As + "'"
            strSQL = strSQL + ",'" + link.Contato_B + "'"
            strSQL = strSQL + ",'" + link.Tel_cont_a + "'"
            strSQL = strSQL + ",'" + link.Email_cont_a + "'"
            strSQL = strSQL + ",'" + link.Tel_cont_b + "'"
            strSQL = strSQL + ",'" + link.Email_Cont_B + "'"
            strSQL = strSQL + ",'" + link.Sucursal + "'"
            strSQL = strSQL + ",'" + link.Conta_cont + "'"
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

        If ccustos_rateio_list.Count > 0 Then
            InsertCCustos("(select nvl(max(CODIGO_LINK),0) from LINKS)", ccustos_rateio_list)
        End If

        Return InsertLinklLog(link, "N", log_string)

    End Function

    Public Function InsertLinksArquivos(ByVal codigo_link As String, ByVal File_Name As String, ByVal Bytes() As Byte) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = " Insert into LINKS_FILES "
            strSQL = strSQL + " (CODIGO_LINK,FILE_NAME,BYTES)"
            If codigo_link = "" Then
                strSQL = strSQL + " values ((select nvl(max(CODIGO_LINK),0) from LINKS),:sName,:sFile)"
            Else
                strSQL = strSQL + " values ('" + codigo_link + "',:sName,:sFile)"
            End If

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            cmd.Parameters.Add(":sName", OleDbType.VarChar).Value = File_Name
            cmd.Parameters.Add(":sFile", OleDbType.LongVarBinary).Value = Bytes
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

    Public Function DeleteFiles(ByVal pcodigo As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete links_Files "
            strSQL = strSQL + "where codigo_link = " + pcodigo

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

    Public Function GetFiles(ByVal codigo As Integer, ByRef File_Names As List(Of String), ByRef _bytes As List(Of Byte())) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select FILE_NAME, BYTES "
        strSQL = strSQL + " from LINKS_FILES "
        strSQL = strSQL + " WHERE CODIGO_LINK='" + codigo.ToString + "'"

        If File_Names.Count Then
            For Each name As String In File_Names
                strSQL = strSQL + " and FILE_NAME <> '" + name + "'"
            Next
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                File_Names.Add(reader.Item("FILE_NAME").ToString())
                _bytes.Add(reader.Item("BYTES"))
                Return True
            End While
        End Using

        Return False
    End Function

    Public Function GetFileParam(ByVal codigo As Integer, ByVal File_Name As String) As String
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = "select BYTES "
        strSQL = strSQL + " from LINKS_FILES "
        strSQL = strSQL + " WHERE CODIGO_LINK='" + codigo.ToString + "'"
        strSQL = strSQL + " AND FILE_NAME='" + File_Name + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Return (reader.Item("BYTES"))
            End While
        End Using
        Return ("")
    End Function

    Public Function AtualizaLink(ByVal link As AppLinks, ByVal log_string As List(Of String), ByVal ccustos_rateio_list As List(Of AppGrupo)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim link_old As List(Of AppLinks) = GetLinkById(link.Codigo_Link)

        If InsertLinklLog(link_old.Item(0), "A", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "update LINKS set "
            If link.Codigo_Fornecedor <> 0 Then
                strSQL = strSQL + "CODIGO_FORNECEDOR='" + link.Codigo_Fornecedor.ToString() + "' ,"
            End If
            strSQL = strSQL + " LOCALIDADE='" + link.Localidade + "'"
            strSQL = strSQL + ", COD_LOCAL='" + link.Cod_local.ToString + "'"
            strSQL = strSQL + ", COD_TIPO_LINK='" + link.Tipo_Link + "'"
            strSQL = strSQL + ", DESIG_CIR='" + link.Desig_Cir + "'"
            strSQL = strSQL + ", DESIG_PAG='" + link.Desig_Pag + "'"
            strSQL = strSQL + ", ANT_PORTA='" + link.Ant_Porta + "'"
            strSQL = strSQL + ", ANT_CIR='" + link.Ant_Cir + "'"
            strSQL = strSQL + ", ATUAL_PORTA='" + link.Atual_Porta + "'"
            strSQL = strSQL + ", ATUAL_CIR='" + link.Atual_Cir + "'"
            strSQL = strSQL + ", CODIGO_CIDADE='" + link.Codigo_Cidade.ToString() + "'"
            strSQL = strSQL + ", UF='" + link.Uf + "'"
            strSQL = strSQL + ", CEP='" + link.Cep + "'"
            strSQL = strSQL + ", TELEFONE='" + link.Telefone + "'"
            strSQL = strSQL + ", CONTATO='" + link.Contato + "'"
            strSQL = strSQL + ", REGIAO='" + link.Regiao + "'"
            strSQL = strSQL + ", DLCI_MAT= '" + link.Dlci_Mat + "'"
            strSQL = strSQL + ", DLCI_REM= '" + link.Dlci_Rem + "'"
            strSQL = strSQL + ", LMI_REM= '" + link.Lmi_Rem + "'"
            strSQL = strSQL + ", DATA_ATI= to_date('" + link.Data_Ati + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", DATA_DES= to_date('" + link.Data_Des + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", STATUS='" + link.Status.ToString() + "'"
            strSQL = strSQL + ", SALVA='" + link.Salva + "'"
            strSQL = strSQL + ", WAN='" + link.Protocolo + "'"
            strSQL = strSQL + ", OBS='" + link.OBS_ + "'"
            If link.Codigo_Router <> 0 Then
                strSQL = strSQL + ", CODIGO_ROUTER='" + link.Codigo_Router.ToString() + "'"
            Else
                strSQL = strSQL + ", CODIGO_ROUTER=''"
            End If
            If link.Codigo_Lan <> 0 Then
                strSQL = strSQL + ", CODIGO_WAN='" + link.Codigo_Wan.ToString() + "'"
            Else
                strSQL = strSQL + ", CODIGO_WAN=''"
            End If
            If link.Codigo_Lan <> 0 Then
                strSQL = strSQL + ", CODIGO_LAN='" + link.Codigo_Lan.ToString() + "'"
            Else
                strSQL = strSQL + ", CODIGO_LAN=''"
            End If
            strSQL = strSQL + ", ENDERECO_B='" + link.ENDERECO_B_ + "'"
            strSQL = strSQL + ", VALOR_ATIV='" + link.Valor_Ativ + "'"
            strSQL = strSQL + ", VALOR_LINK='" + link.Valor_Link + "'"
            strSQL = strSQL + ", COD_CLIENTE_CONTA='" + link.Cod_cliente_conta + "'"
            strSQL = strSQL + ", NUM_FATURA='" + link.NumeroFatura + "'"
            strSQL = strSQL + ", NUM_CONTRATO_TVG='" + link.NumeroContratoTVG + "'"
            strSQL = strSQL + ", NUM_CONTRATO_OP='" + link.NumeroContratoOP + "'"
            strSQL = strSQL + ", DATA_IPCA= to_date('" + link.DataIPCA + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", VALOR_MENSAL='" + link.ValorMensal + "'"
            strSQL = strSQL + ", SPONSOR='" + link.Sponsor + "'"
            strSQL = strSQL + ", ENDERECO_COB='" + link.Endereco_cob + "'"
            strSQL = strSQL + ", NUMERO_OC='" + link.NumeroOC + "'"
            strSQL = strSQL + ", UF_B='" + link.UF_B + "'"
            strSQL = strSQL + ", CODIGO_CIDADE_B='" + link.Codigo_Cidade_B + "'"
            strSQL = strSQL + ", NUM_CONTRATO_OP_AS='" + link.Num_Contrato_Op_As + "'"
            strSQL = strSQL + ", CONTATO_B='" + link.Contato_B + "'"
            strSQL = strSQL + ", TEL_CONT_A='" + link.Tel_cont_a + "'"
            strSQL = strSQL + ", EMAIL_CONT_A='" + link.Email_cont_a + "'"
            strSQL = strSQL + ", TEL_CONT_B='" + link.Tel_cont_b + "'"
            strSQL = strSQL + ", EMAIL_CONT_B='" + link.Email_Cont_B + "'"
            strSQL = strSQL + ", CODIGO_SUCURSAL='" + link.Sucursal + "'"
            strSQL = strSQL + ", CONTA_CONTABIL='" + link.Conta_cont + "'"
            strSQL = strSQL + " where CODIGO_LINK  = '" + link.Codigo_Link.ToString() + "' "


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


        InsertCCustos("'" & link.Codigo_Link & "'", ccustos_rateio_list)


        If InsertLinklLog(link, "B", log_string) = False Then
            Return False
        End If

        Return True

    End Function

    Public Function ExcluiLink(ByVal pcodigo As Integer, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim link As List(Of AppLinks) = GetLinkById(pcodigo)
        If InsertLinklLog(link.Item(0), "D", log_string) = False Then
            Return False
        End If

        Try
            Dim strSQL As String = "delete LINKS "
            strSQL = strSQL + "where CODIGO_LINK = " + Convert.ToString(pcodigo)

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

    Public Function ExcluiOldRouter(ByVal pcodigo As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "delete ROUTERS "
            strSQL = strSQL + "where codigo_router = " + pcodigo

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

    Public Function GetCcustosLink(ByVal pcodigo_link As Integer) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim ccusto_list As New List(Of String)
        Dim strSQL As String = "select codigo_ccusto "
        strSQL = strSQL + "from  ccusto_links "
        strSQL = strSQL + "where codigo_link= " + Convert.ToString(pcodigo_link)

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("codigo_ccusto").ToString)
                ccusto_list.Add(_registro)
            End While
        End Using

        Return ccusto_list

    End Function

    Public Function GetLinkById(ByVal pcodigo As Integer) As List(Of AppLinks)
        Dim connection As New OleDbConnection(strConn)
        Dim listLink As New List(Of AppLinks)

        Dim strSQL As String = "select CODIGO_LINK"
        strSQL = strSQL + ", nvl(CODIGO_FORNECEDOR, 0) AS CODIGO_FORNECEDOR"
        strSQL = strSQL + ", nvl(LOCALIDADE, '') AS LOCALIDADE"
        strSQL = strSQL + ", nvl(COD_LOCAL, 0) AS COD_LOCAL"
        strSQL = strSQL + ", nvl(COD_TIPO_LINK, '') AS TIPO_LINK"
        strSQL = strSQL + ", nvl(DESIG_CIR, '') AS DESIG_CIR"
        strSQL = strSQL + ", nvl(DESIG_PAG, '') AS DESIG_PAG"
        strSQL = strSQL + ", nvl(CODIGO_CCUSTO, '') AS CODIGO_CCUSTO"
        strSQL = strSQL + ", nvl(ANT_PORTA, '') AS ANT_PORTA"
        strSQL = strSQL + ", nvl(ANT_CIR, '') AS ANT_CIR"
        strSQL = strSQL + ", nvl(ATUAL_PORTA, '') AS ATUAL_PORTA"
        strSQL = strSQL + ", nvl(ATUAL_CIR, '') AS ATUAL_CIR"
        strSQL = strSQL + ", nvl(CODIGO_CIDADE, 0) AS CODIGO_CIDADE"
        strSQL = strSQL + ", nvl(UF, 0) AS UF"
        strSQL = strSQL + ", nvl(CEP, '') AS CEP"
        strSQL = strSQL + ", nvl(TELEFONE, '') AS TELEFONE"
        strSQL = strSQL + ", nvl(CONTATO, '') AS CONTATO"
        strSQL = strSQL + ", nvl(REGIAO, '') AS REGIAO"
        strSQL = strSQL + ", nvl(DLCI_MAT, '') AS DLCI_MAT"
        strSQL = strSQL + ", nvl(DLCI_REM, '') AS DLCI_REM"
        strSQL = strSQL + ", nvl(LMI_REM, '') AS LMI_REM"
        strSQL = strSQL + ", nvl(DATA_ATI, '') AS DATA_ATI"
        strSQL = strSQL + ", nvl(DATA_DES, '') AS DATA_DES"
        strSQL = strSQL + ", nvl(STATUS, 0) AS STATUS"
        strSQL = strSQL + ", nvl(SALVA, '') AS SALVA"
        strSQL = strSQL + ", nvl(OBS, '') AS OBS_"
        strSQL = strSQL + ", nvl(CODIGO_ROUTER, '0') AS CODIGO_ROUTER"
        strSQL = strSQL + ", nvl(CODIGO_WAN, '0') AS CODIGO_WAN"
        strSQL = strSQL + ", nvl(CODIGO_LAN, '0') AS CODIGO_LAN"
        strSQL = strSQL + ", nvl(WAN, '') AS PROTOCOLO"
        strSQL = strSQL + ", nvl(ENDERECO_B, '') AS ENDERECO_B"
        strSQL = strSQL + ", nvl(VALOR_ATIV, '') AS VALOR_ATIV"
        strSQL = strSQL + ", nvl(VALOR_LINK, '') AS VALOR_LINK"
        strSQL = strSQL + ", nvl(COD_CLIENTE_CONTA, '') AS COD_CLIENTE_CONTA"
        strSQL = strSQL + ", nvl(NUM_FATURA, '') AS NUM_FATURA"
        strSQL = strSQL + ", nvl(NUM_CONTRATO_TVG, '') AS NUM_CONTRATO_TVG"
        strSQL = strSQL + ", nvl(NUM_CONTRATO_OP, '') AS NUM_CONTRATO_OP"
        strSQL = strSQL + ", nvl(DATA_IPCA, '') AS DATA_IPCA"
        strSQL = strSQL + ", nvl(VALOR_MENSAL, '') AS VALOR_MENSAL"
        strSQL = strSQL + ", nvl(SPONSOR, '') AS SPONSOR"
        strSQL = strSQL + ", nvl(ENDERECO_COB, '') AS ENDERECO_COB"
        strSQL = strSQL + ", nvl(NUMERO_OC, '') AS NUMERO_OC"
        strSQL = strSQL + ", nvl(CODIGO_CIDADE_B, 0) AS CODIGO_CIDADE_B"
        strSQL = strSQL + ", nvl(UF_B, 0) AS UF_B"
        strSQL = strSQL + ", nvl(NUM_CONTRATO_OP_AS, '') AS NUM_CONTRATO_OP_AS"
        strSQL = strSQL + ", nvl(CONTATO_B, '') AS CONTATO_B"
        strSQL = strSQL + ", nvl(TEL_CONT_A, '') AS TEL_CONT_A"
        strSQL = strSQL + ", nvl(EMAIL_CONT_A, '') AS EMAIL_CONT_A"
        strSQL = strSQL + ", nvl(TEL_CONT_B, '') AS TEL_CONT_B"
        strSQL = strSQL + ", nvl(EMAIL_CONT_B, '') AS EMAIL_CONT_B"
        strSQL = strSQL + ", nvl(CODIGO_SUCURSAL, '') AS CODIGO_SUCURSAL"
        strSQL = strSQL + ", nvl(CONTA_CONTABIL, '') AS CONTA_CONTABIL"
        strSQL = strSQL + " FROM LINKS "

        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO_LINK = " + Convert.ToString(pcodigo) + ""
        ElseIf pcodigo = 0 Then
            strSQL = strSQL + "order by CODIGO_LINK"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppLinks(reader.Item("CODIGO_LINK").ToString, reader.Item("COD_LOCAL").ToString, reader.Item("CODIGO_FORNECEDOR").ToString, reader.Item("LOCALIDADE").ToString, reader.Item("TIPO_LINK").ToString, reader.Item("DESIG_CIR").ToString, reader.Item("DESIG_PAG").ToString, reader.Item("CODIGO_CCUSTO").ToString, reader.Item("ANT_PORTA").ToString, reader.Item("ANT_CIR").ToString, reader.Item("ATUAL_PORTA").ToString, reader.Item("ATUAL_CIR").ToString, reader.Item("CODIGO_CIDADE").ToString, reader.Item("UF").ToString, reader.Item("CEP").ToString, reader.Item("TELEFONE").ToString, reader.Item("CONTATO").ToString, reader.Item("REGIAO").ToString, reader.Item("DLCI_MAT").ToString, reader.Item("DLCI_REM").ToString, reader.Item("LMI_REM").ToString, reader.Item("DATA_ATI").ToString, reader.Item("DATA_DES").ToString, reader.Item("STATUS").ToString, reader.Item("SALVA").ToString, reader.Item("OBS_").ToString, reader.Item("CODIGO_ROUTER").ToString, reader.Item("CODIGO_LAN").ToString, reader.Item("CODIGO_WAN").ToString, reader.Item("PROTOCOLO").ToString, reader.Item("ENDERECO_B").ToString, reader.Item("VALOR_ATIV").ToString, reader.Item("VALOR_LINK").ToString, reader.Item("COD_CLIENTE_CONTA").ToString, reader.Item("UF_B").ToString, reader.Item("CODIGO_CIDADE_B").ToString, reader.Item("NUM_CONTRATO_OP_AS").ToString, reader.Item("CONTATO_B").ToString)
                _registro.NumeroFatura = reader.Item("NUM_FATURA").ToString
                _registro.NumeroContratoTVG = reader.Item("NUM_CONTRATO_TVG").ToString
                _registro.NumeroContratoOP = reader.Item("NUM_CONTRATO_OP").ToString
                _registro.DataIPCA = reader.Item("DATA_IPCA").ToString
                _registro.ValorMensal = reader.Item("VALOR_MENSAL").ToString
                _registro.Sponsor = reader.Item("SPONSOR").ToString
                _registro.Endereco_cob = reader.Item("ENDERECO_COB").ToString
                _registro.NumeroOC = reader.Item("NUMERO_OC").ToString
                _registro.Tel_cont_a = reader.Item("TEL_CONT_A").ToString
                _registro.Email_cont_a = reader.Item("EMAIL_CONT_A").ToString
                _registro.Tel_cont_b = reader.Item("TEL_CONT_B").ToString
                _registro.Email_Cont_B = reader.Item("EMAIL_CONT_B").ToString
                _registro.Conta_cont = reader.Item("CONTA_CONTABIL").ToString
                listLink.Add(_registro)
            End While
        End Using

        Return listLink
    End Function

    Public Function InsertLinklLog(ByVal link As AppLinks, ByVal insert As Char, ByVal log_string As List(Of String)) As Boolean
        Dim connection As New OleDbConnection(strConn)
        link.Codigo_CCusto = ""

        For Each item As String In Me.GetCcustosLink(link.Codigo_Link)
            link.Codigo_CCusto = link.Codigo_CCusto + " " + item
        Next

        Try
            Dim strSQL As String = "insert into LINKS_LOG(CODIGO_LOG, TIPO_LOG, CODIGO_LINK"
            strSQL = strSQL + ", FORNECEDOR"
            strSQL = strSQL + ", LOCALIDADE_B "
            strSQL = strSQL + ", TIPO_LINK "
            strSQL = strSQL + ", DESIG_CIR "
            strSQL = strSQL + ", DESIG_PAG "
            strSQL = strSQL + ", CODIGO_CCUSTO "
            strSQL = strSQL + ", ANT_PORTA "
            strSQL = strSQL + ", ANT_CIR "
            strSQL = strSQL + ", ATUAL_PORTA "
            strSQL = strSQL + ", ATUAL_CIR "
            strSQL = strSQL + ", CIDADE "
            strSQL = strSQL + ", UF "
            strSQL = strSQL + ", CEP "
            strSQL = strSQL + ", TELEFONE "
            strSQL = strSQL + ", CONTATO "
            strSQL = strSQL + ", REGIAO "
            strSQL = strSQL + ", DLCI_MAT "
            strSQL = strSQL + ", DLCI_REM "
            strSQL = strSQL + ", LMI_REM "
            strSQL = strSQL + ", DATA_ATI "
            strSQL = strSQL + ", DATA_DES "
            strSQL = strSQL + ", STATUS "
            strSQL = strSQL + ", SALVA "
            strSQL = strSQL + ", OBS "
            strSQL = strSQL + ", ROUTER "
            strSQL = strSQL + ", WAN "
            strSQL = strSQL + ", LAN "
            strSQL = strSQL + ", ENDERECO_B "
            strSQL = strSQL + ", VALOR_ATIV "
            strSQL = strSQL + ", VALOR_LINK "
            strSQL = strSQL + ", COD_CLIENTE_CONTA "
            strSQL = strSQL + ", USUARIO "
            strSQL = strSQL + ", DATA_LOG "
            strSQL = strSQL + ", NUM_FATURA "
            strSQL = strSQL + ", NUM_CONTRATO_TVG "
            strSQL = strSQL + ", NUM_CONTRATO_OP "
            strSQL = strSQL + ", DATA_IPCA "
            strSQL = strSQL + ", VALOR_MENSAL "
            strSQL = strSQL + ", SPONSOR "
            strSQL = strSQL + ", ENDERECO_COB "
            strSQL = strSQL + ", NUMERO_OC "
            strSQL = strSQL + ", UF_B"
            strSQL = strSQL + ", CODIGO_CIDADE_B"
            strSQL = strSQL + ", NUM_CONTRATO_OP_AS"
            strSQL = strSQL + ", CONTATO_B"
            strSQL = strSQL + ", TEL_CONT_A"
            strSQL = strSQL + ", EMAIL_CONT_A"
            strSQL = strSQL + ", TEL_CONT_B"
            strSQL = strSQL + ", EMAIL_CONT_B"
            strSQL = strSQL + ", CODIGO_SUCURSAL"
            strSQL = strSQL + ", CONTA_CONTABIL"
            strSQL = strSQL + ")"
            strSQL = strSQL + "values ((select nvl(max(CODIGO_LOG),0)+1 from LINKS_LOG)"

            'TIPO

            strSQL = strSQL + ",'" + insert + "'"
            strSQL = strSQL + ",'" + link.Codigo_Link.ToString() + "'"
            strSQL = strSQL + ",'" + link.Codigo_Fornecedor.ToString() + "'"
            strSQL = strSQL + ",'" + link.Localidade + "'"
            strSQL = strSQL + ",(select TIPO from LINHAS_TIPO where CODIGO_TIPO='" + link.Tipo_Link + "')"
            strSQL = strSQL + ",'" + link.Desig_Cir + "'"
            strSQL = strSQL + ",'" + link.Desig_Pag + "'"
            strSQL = strSQL + ",'" + link.Codigo_CCusto + "'"
            strSQL = strSQL + ",'" + link.Ant_Porta + "'"
            strSQL = strSQL + ",'" + link.Ant_Cir + "'"
            strSQL = strSQL + ",'" + link.Atual_Porta + "'"
            strSQL = strSQL + ",'" + link.Atual_Cir + "'"
            strSQL = strSQL + ",'" + log_string.Item(0).ToString + "'" 'Nome da cidade
            strSQL = strSQL + ",'" + link.Uf + "'"
            strSQL = strSQL + ",'" + link.Cep + "'"
            strSQL = strSQL + ",'" + link.Telefone + "'"
            strSQL = strSQL + ",'" + link.Contato + "'"
            strSQL = strSQL + ",'" + link.Regiao + "'"
            strSQL = strSQL + ",'" + link.Dlci_Mat + "'"
            strSQL = strSQL + ",'" + link.Dlci_Rem + "'"
            strSQL = strSQL + ",'" + link.Lmi_Rem + "'"
            If link.Data_Ati = "" Then
                strSQL = strSQL + ",'" + link.Data_Ati + "'"
            Else
                strSQL = strSQL + ",'" + FormatDateTime(link.Data_Ati, DateFormat.ShortDate) + "'"

            End If
            If link.Data_Des = "" Then
                strSQL = strSQL + ",'" + link.Data_Des + "'"
            Else
                strSQL = strSQL + ",'" + FormatDateTime(link.Data_Des, DateFormat.ShortDate) + "'"

            End If
            strSQL = strSQL + ",'" + log_string.Item(1).ToString + "'" 'Descrição do Status
            strSQL = strSQL + ",'" + link.Salva + "'"
            strSQL = strSQL + ",'" + link.OBS_ + "'"
            strSQL = strSQL + ",'" + link.Codigo_Router.ToString() + "'"
            strSQL = strSQL + ",'" + link.Codigo_Wan.ToString() + "'"
            strSQL = strSQL + ",'" + link.Codigo_Lan.ToString() + "'"
            strSQL = strSQL + ",'" + link.ENDERECO_B_ + "'"
            strSQL = strSQL + ",'" + link.Valor_Ativ + "'"
            strSQL = strSQL + ",'" + link.Valor_Link + "'"
            strSQL = strSQL + ",'" + link.Cod_cliente_conta + "'"
            strSQL = strSQL + ",'" + log_string.Item(2).ToString + "'"
            strSQL = strSQL + ", to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ", '" + link.NumeroFatura + "'"
            strSQL = strSQL + ", '" + link.NumeroContratoTVG + "'"
            strSQL = strSQL + ", '" + link.NumeroContratoOP + "'"

            If link.DataIPCA = "" Then
                strSQL = strSQL + ",'" + link.DataIPCA + "'"
            Else
                strSQL = strSQL + ",'" + FormatDateTime(link.DataIPCA, DateFormat.ShortDate) + "'"
            End If

            strSQL = strSQL + ", '" + link.ValorMensal + "'"
            strSQL = strSQL + ", '" + link.Sponsor + "'"
            strSQL = strSQL + ", '" + link.Endereco_cob + "'"
            strSQL = strSQL + ", '" + link.NumeroOC + "'"
            strSQL = strSQL + ",'" + link.UF_B + "'"
            strSQL = strSQL + ",'" + link.Codigo_Cidade_B + "'"
            strSQL = strSQL + ",'" + link.Num_Contrato_Op_As + "'"
            strSQL = strSQL + ",'" + link.Contato_B + "'"
            strSQL = strSQL + ",'" + link.Tel_cont_a + "'"
            strSQL = strSQL + ",'" + link.Email_cont_a + " '"
            strSQL = strSQL + ",'" + link.Tel_cont_b + "'"
            strSQL = strSQL + ",'" + link.Email_Cont_B + "'"
            strSQL = strSQL + ",'" + link.Sucursal + "'"
            strSQL = strSQL + ",'" + link.Conta_cont + "'"
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

    Public Sub InsertCCustos(ByVal pcodigo_link As String, ByVal ccustos_rateio_list As List(Of AppGrupo))

        Dim strSQL As String = "delete from CCUSTO_LINKS where codigo_link =" + pcodigo_link + ""
        ResolveQuery(strSQL)

        If ccustos_rateio_list.Count > 0 Then
            For Each item As AppGrupo In ccustos_rateio_list
                strSQL = "insert into CCUSTO_LINKS(codigo_link,codigo_ccusto,rateio) "
                strSQL = strSQL + "values (" + pcodigo_link + ",'" + item.Codigo + "','" + item.Rateio.ToString.Replace(",", ".") + "')"

                ResolveQuery(strSQL)
            Next
        End If

    End Sub

    Public Function ResolveQuery(ByVal query As String) As Boolean

        Try
            Dim connection As New OleDbConnection(strConn)
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = query
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
End Class
