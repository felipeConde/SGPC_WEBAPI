Imports Microsoft.VisualBasic
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAO_SAPFolhaRegistro

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


    Public Function InsereFR(FR As AppSAPFR) As String
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim sql As String = ""

        Try
            _dao_commons.strConn = strConn
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            'pegamos o codigo de num externo
            Dim num_externo As String = _dao_commons.myDataTable("select nvl(max(t.lblne_lblne1),0)+1 num_externo from SAP_FR t ").Rows(0).Item(0).ToString

            sql = " insert into SAP_FR ( "
            sql += "LBLNE_LBLNE1" 'Nº externo da folha de registro de serviços
            sql += ",SBNAMAG_SBNAMAG" 'Responsável interno
            sql += ",DLORT_DLORT" 'Local da prestação do serviço
            sql += ",LZVON_LZVON" 'Período
            sql += ",LZBIS_LZBIS" 'Fim do período
            sql += ",TXZ01_TXZ01_ESSR" 'Texto breve da folha de registro de serviços
            sql += ",EBELN_EBELN" 'Nº do documento de compras
            sql += ",EBELP_EBELP " 'Nº item do documento de compra
            sql += ",PWWE_MC_PWWE" 'Nota para a qualidade do serviço
            sql += " ,PWFR_MC_PWFR" 'Nota para o cumprimento de prazos
            sql += " ,XBLNR_XBLNR_SRV1" 'Nº documento de referência
            sql += " ,BKTXT_BKTXT_SRV" 'Texto de cabeçalho de documento
            sql += " ,KNTTP_KNTTP" 'Categoria de classificação contábil
            sql += " ,DATA_CRIACAO" 'data de criação para controle
            sql += " )"
            sql += " values ("
            sql += "'" & num_externo.ToString & "'"  'Nº externo da folha de registro de serviços
            sql += ",'" & FR.SBNAMAG_SBNAMAG & "'" 'Responsável interno
            sql += ",'" & FR.DLORT_DLORT & "'" 'Local da prestação do serviço
            sql += ",'" & FR.LZVON_LZVON & "'"  'Período
            sql += ",'" & FR.LZBIS_LZBIS & "'" 'Fim do período
            sql += ",'" & FR.TXZ01_TXZ01_ESSR & "'" 'Texto breve da folha de registro de serviços
            sql += ",'" & FR.EBELN_EBELN & "'"  'Nº do documento de compras
            sql += ",'" & FR.EBELP_EBELP & "'" 'Nº item do documento de compra
            sql += ",'" & FR.PWWE_MC_PWWE & "'" 'Nota para a qualidade do serviço
            sql += ",'" & FR.PWFR_MC_PWFR & "'"  'Nota para o cumprimento de prazos
            sql += ",'" & FR.XBLNR_XBLNR_SRV1 & "'" 'Nº documento de referência
            sql += ",'" & FR.BKTXT_BKTXT_SRV & "'" 'Texto de cabeçalho de documento
            sql += ",'" & FR.KNTTP_KNTTP & "'" 'Categoria de classificação contábil
            sql += ", sysdate"
            sql += ")"

            cmd.CommandText = sql
            cmd.ExecuteNonQuery()


            For Each item As AppSAPFR_Item In FR.ListItem

                'insere os itens
                sql = " insert into SAP_FR_ITENS ( "
                sql += "LBLNE_LBLNE1" 'Nº externo da folha de registro de serviços
                sql += ",SAKTO_SAKNR" 'Nº conta do Razão
                sql += ",KOSTL_KOSTL" 'Centro de custo
                sql += ",AUFNR_AUFNR" 'Nº ordem
                sql += ",PS_PSP_PNR_PS_PSP_PNR" 'Elemento do plano da estrutura do projeto (elemento PEP)
                sql += ",EXTROW_EXTROW" 'Nº da linha
                sql += ",MENGE_MENGEV" 'Qtd.com símbolo +/-
                sql += " )"
                sql += " values ("
                sql += "'" & num_externo.ToString & "'"  'Nº externo da folha de registro de serviços
                sql += ",'" & item.SAKTO_SAKNR & "'" 'Nº conta do Razão
                sql += ",'" & item.KOSTL_KOSTL & "'" 'Centro de custo
                sql += ",'" & item.AUFNR_AUFNR & "'"  'Nº ordem
                sql += ",'" & item.POSID_PS_POSID & "'" 'Elemento do plano da estrutura do projeto (elemento PEP)
                sql += ",'" & item.EXTROW_EXTROW & "'" 'Nº da linha        
                sql += ",'" & item.MENGE_MENGEV.ToString.Replace(".", "").Replace(",", ".") & "'" 'Qtd.com símbolo +/-  
                sql += ")"

                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            Next

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return num_externo.ToString

        Catch e As Exception
            _dao_commons.EscreveLog("Erro na InsereFR: " & e.Message)
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return -1
        End Try



    End Function



End Class
