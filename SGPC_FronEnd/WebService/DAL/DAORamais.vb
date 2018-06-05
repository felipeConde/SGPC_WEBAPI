Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System

Public Class DAORamais

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

    Public Function GetRamaisById(ByVal pnumero_a As String, ByRef listRamal As List(Of AppRamais)) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "select r.NUMERO_A"
            strSQL = strSQL + ",r.TARIFAVEL,r.TRANSFERIVEL,r.NUMERO_C,r.COMPARTILHA_CREDITO,r.GRUPO_ACDG"
            strSQL = strSQL + ",r.ATIVO,r.CREDITO_MENSAL,r.SALDO_ATUAL,r.LIMITE_COMPARTILHAMENTO,r.GRP_CODIGO,r.DDI"
            strSQL = strSQL + ",r.DDD,r.LOCAL,r.CELULAR,r.POSSUI_BLOQUEIO,r.A_COBRAR,r.DDG"
            strSQL = strSQL + ",r.EM_USO,r.CREDITO_EM_USO,r.BLOQUEIO_EMAIL,r.CUSTO_RAMAL,r.IDSUBGRUPO,r.CATEGORIA"
            strSQL = strSQL + ",r.BLOQUEAVEL,r.INTERNAL,r.OEM, to_char(r.ATIVACAO,'dd/mm/yyyy') as ATIVACAO, to_char(r.DESATIVADO,'dd/mm/yyyy') as DESATIVADO"
            strSQL = strSQL + ",nvl(r.CODIGO_MODELO,0) as CODIGO_MODELO,nvl(r.CODIGO_LINHA,0) as CODIGO_LINHA,nvl(r.CODIGO_LOCALIDADE,0) as CODIGO_LOCALIDADE"
            strSQL = strSQL + ",r.TN,nvl(r.CODIGO_SITE,0) as CODIGO_SITE,r.REDE,r.CT,r.STATION,r.TIPO_RAMAL"
            strSQL = strSQL + ",gr.NOME_GRUPO,us.NOME_USUARIO,us.CIDADE as UNIDADE,us.CARGO_USUARIO as SETOR"
            strSQL = strSQL + " from RAMAIS r "
            strSQL = strSQL + "inner join GRUPOS gr on r.GRP_CODIGO = gr.CODIGO "
            strSQL = strSQL + "left join USUARIOS us on r.NUMERO_A = us.RML_NUMERO_A "
            strSQL = strSQL + "where r.NUMERO_A = '" + pnumero_a + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _registro As New AppRamais(reader.Item("NUMERO_A").ToString, reader.Item("TARIFAVEL").ToString, reader.Item("TRANSFERIVEL").ToString, reader.Item("NUMERO_C").ToString, reader.Item("COMPARTILHA_CREDITO").ToString, reader.Item("GRUPO_ACDG").ToString, reader.Item("ATIVO").ToString, reader.Item("CREDITO_MENSAL").ToString, reader.Item("SALDO_ATUAL").ToString, reader.Item("LIMITE_COMPARTILHAMENTO").ToString, reader.Item("GRP_CODIGO").ToString, reader.Item("DDI").ToString, reader.Item("DDD").ToString, reader.Item("LOCAL").ToString, reader.Item("CELULAR").ToString, reader.Item("POSSUI_BLOQUEIO").ToString, reader.Item("A_COBRAR").ToString, reader.Item("DDG").ToString, reader.Item("EM_USO").ToString, reader.Item("CREDITO_EM_USO").ToString, reader.Item("BLOQUEIO_EMAIL").ToString, reader.Item("CUSTO_RAMAL").ToString, reader.Item("IDSUBGRUPO").ToString, reader.Item("CATEGORIA").ToString, reader.Item("BLOQUEAVEL").ToString, reader.Item("INTERNAL").ToString, reader.Item("OEM").ToString, reader.Item("ATIVACAO").ToString, reader.Item("DESATIVADO").ToString, reader.Item("CODIGO_MODELO").ToString, reader.Item("CODIGO_LINHA").ToString, reader.Item("CODIGO_LOCALIDADE").ToString, reader.Item("TN").ToString, reader.Item("CODIGO_SITE").ToString, reader.Item("REDE").ToString, reader.Item("CT").ToString, reader.Item("STATION").ToString, reader.Item("TIPO_RAMAL").ToString, reader.Item("NOME_GRUPO").ToString, reader.Item("NOME_USUARIO").ToString, reader.Item("UNIDADE").ToString, reader.Item("SETOR").ToString)
                    listRamal.Add(_registro)
                End While
            End Using
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Function GetRamaisLivres(ByVal pnumero_a As String) As List(Of AppRamais)
        Dim connection As New OleDbConnection(strConn)
        Dim listRamal As New List(Of AppRamais)
        'Inserir pnumero = "" parar obter a lista de ramais livres....inserir numero parar verificar se o ramal está livre

        Try
            'Dim strSQL As String = "select NUMERO_A from ramais where numero_a not in "
            'strSQL = strSQL + "(select rml_numero_a from usuarios where rml_numero_a is not null) "
            Dim strSQL As String = "select r.NUMERO_A from ramais r where not exists "
            strSQL = strSQL + "(select 0 from usuarios where rml_numero_a=r.NUMERO_A) "
            If pnumero_a <> "" Then
                strSQL = strSQL + "and numero_a ='" + pnumero_a + "'"
            End If
            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _registro As New AppRamais(reader.Item("NUMERO_A").ToString)
                    listRamal.Add(_registro)
                End While
            End Using
        Catch ex As Exception

        End Try

        Return listRamal
    End Function


    Public Function ComboRamaisModelos() As List(Of AppRamaisModelos)
        Dim connection As New OleDbConnection(strConn)
        Dim listModelos As New List(Of AppRamaisModelos)

        Dim strSQL As String = "select CODIGO_MODELO, MODELO "
        strSQL = strSQL + "from RAMAIS_MODELOS "
        strSQL = strSQL + "order by modelo"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppRamaisModelos(reader.Item("CODIGO_MODELO").ToString, reader.Item("MODELO").ToString)
                listModelos.Add(_registro)
            End While
        End Using

        Return listModelos
    End Function

    Public Function ComboLocalidades() As List(Of AppLocalidades)
        Dim connection As New OleDbConnection(strConn)
        Dim listLocalidades As New List(Of AppLocalidades)

        Dim strSQL As String = "select 0 as CODIGO, '...' as LOCALIDADE from dual union "
        strSQL = strSQL + "select CODIGO, LOCALIDADE "
        strSQL = strSQL + "from LOCALIDADES "
        strSQL = strSQL + "order by LOCALIDADE"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppLocalidades(reader.Item("CODIGO").ToString, reader.Item("LOCALIDADE").ToString)
                listLocalidades.Add(_registro)
            End While
        End Using

        Return listLocalidades
    End Function

    Public Function ComboPredio() As List(Of AppSites)
        Dim connection As New OleDbConnection(strConn)
        Dim listPredio As New List(Of AppSites)

        Dim strSQL As String = "select 0 as CODIGO_SITE, 'SEM REGISTRO' as SITE, '...' as SIGLA from dual union "
        strSQL = strSQL + "select CODIGO_SITE, SITE, nvl(SIGLA,'XXX') as SIGLA "
        strSQL = strSQL + "from SITES "

        strSQL = strSQL + "order by SITE"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppSites(reader.Item("CODIGO_SITE").ToString, reader.Item("SITE").ToString, reader.Item("SIGLA").ToString)
                listPredio.Add(_registro)
            End While
        End Using

        Return listPredio
    End Function

    Public Function ComboLinha(ByVal Localidade_Codigo As String) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinha As New List(Of AppLinhas)

        Dim strSQL As String = "select 0 as CODIGO_LINHA, '...' as NUM_LINHA from dual union "
        strSQL = strSQL + "select CODIGO_LINHA, NUM_LINHA "
        strSQL = strSQL + "from LINHAS "

        If Localidade_Codigo Then

            strSQL = strSQL + "where LINHAS.CODIGO_LOCALIDADE = " + Localidade_Codigo + ""

        End If

        strSQL = strSQL + "order by NUM_LINHA"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString, reader.Item("NUM_LINHA").ToString)
                listLinha.Add(_registro)
            End While
        End Using

        Return listLinha
    End Function

    Public Function InsertRamal(ByVal ramal As AppRamais, ByVal autorlog As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "insert into RAMAIS(NUMERO_A, TARIFAVEL, TRANSFERIVEL, NUMERO_C, COMPARTILHA_CREDITO"
            strSQL = strSQL + ", GRUPO_ACDG, ATIVO, CREDITO_MENSAL, LIMITE_COMPARTILHAMENTO, GRP_CODIGO, DDI, DDD, LOCAL, CELULAR"
            strSQL = strSQL + ", POSSUI_BLOQUEIO, A_COBRAR,DDG, EM_USO, CREDITO_EM_USO, BLOQUEIO_EMAIL, CUSTO_RAMAL, IDSUBGRUPO, CATEGORIA"
            strSQL = strSQL + ", BLOQUEAVEL, INTERNAL, OEM, ATIVACAO, DESATIVADO, CODIGO_MODELO, CODIGO_LINHA, CODIGO_LOCALIDADE"
            strSQL = strSQL + ", TN,CODIGO_SITE,TIPO_RAMAL, REDE, CT, STATION)"
            'NUMERO_A
            strSQL = strSQL + " values ('" + ramal.Numero_A + "',"
            'Tarifavel
            strSQL = strSQL + "'" + ramal.Tarifavel + "',"
            'Transferivel
            strSQL = strSQL + "'N',"
            'Numero_C
            strSQL = strSQL + "'0000',"
            'Comparilha_Credito
            strSQL = strSQL + "'N',"
            'Grupo_ACDG
            strSQL = strSQL + "'0000',"
            'ATIVO
            strSQL = strSQL + "'" + ramal.Ativo + "',"
            'CREDITO_MENSAL
            strSQL = strSQL + "'" + ramal.Credito_Mensal.ToString().Replace(",", ".") + "',"
            'SALDO_ATUAL
            'strSQL = strSQL + "'" + ramal.Saldo_Atual.ToString().Replace(",", ".") + "',"
            'LIMITE_COMPARTILHAMENTO
            strSQL = strSQL + "'000',"
            'GRP_CODIGO
            strSQL = strSQL + "'" + ramal.Grp_Codigo + "',"
            'DDI
            strSQL = strSQL + "'" + ramal.DDI + "',"
            'DDD
            strSQL = strSQL + "'" + ramal.DDD + "',"
            'LOCAL
            strSQL = strSQL + "'" + ramal.Local + "',"
            'CELULAR
            strSQL = strSQL + "'" + ramal.Celular + "',"
            'POSSUI_BLOQUEIO
            strSQL = strSQL + "'" + ramal.Possui_Bloqueio + "',"
            'A_COBRAR
            strSQL = strSQL + "'S',"
            'DDG
            strSQL = strSQL + "'S',"
            'EM_USO
            strSQL = strSQL + "'N',"
            'CREDITO_EM_USO
            strSQL = strSQL + "'" + ramal.Credito_em_Uso + "',"
            'BLOQUEIO_EMAIL
            strSQL = strSQL + "'N',"
            'CUSTO_RAMAL
            strSQL = strSQL + "'" + ramal.Custo_Ramal.ToString().Replace(",", ".") + "',"
            'IDSUBGRUPO  ***************************************************************CORRIGIR*********************
            strSQL = strSQL + "'1',"
            'CATEGORIA
            strSQL = strSQL + "'" + ramal.Categoria + "',"
            'BLOQUEAVEL
            strSQL = strSQL + "'" + ramal.Bloqueavel + "',"
            'INTERNAL
            strSQL = strSQL + "'" + ramal.Internal + "',"
            'OEM
            strSQL = strSQL + "'" + ramal.OEM + "',"
            'ATIVACAO
            strSQL = strSQL + "to_date('" + ramal.Ativacao + "','dd/mm/yyyy hh24:mi:ss'),"
            'DESATIVADO
            strSQL = strSQL + "to_date('" + ramal.Desativado + "','dd/mm/yyyy hh24:mi:ss'),"
            'CODIGO_MODELO
            strSQL = strSQL + "'" + ramal.Codigo_Modelo.ToString() + "',"
            'CODIGO_LINHA
            strSQL = strSQL + "'" + ramal.Codigo_Linha.ToString() + "',"
            'CODIGO_LOCALIDADE
            strSQL = strSQL + "'" + ramal.Codigo_Localidade.ToString() + "',"
            'TN
            strSQL = strSQL + "'" + ramal.TN + "',"
            'CODIGO_SITE
            strSQL = strSQL + "'" + ramal.Codigo_Site.ToString() + "',"
            'TIPO_RAMAL
            strSQL = strSQL + "'" + ramal.Tipo_Ramal.ToString() + "',"
            'REDE
            strSQL = strSQL + "'" + ramal.Rede + "',"
            'CT
            strSQL = strSQL + "'" + ramal.CT + "',"
            'STATION
            strSQL = strSQL + "'" + ramal.Station + "'"

            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            InsertRamalLog(ramal, "N", autorlog)
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function UpdateRamal(ByVal ramal As AppRamais, ByVal autorlog As String) As Boolean

        InsertRamalLog(ramal, "A", autorlog)

        Dim connection As New OleDbConnection(strConn)
        Try
            Dim strSQL As String = "update RAMAIS set "
            'Tarifavel
            strSQL = strSQL + "TARIFAVEL='" + ramal.Tarifavel + "',"
            'Transferivel
            strSQL = strSQL + "TRANSFERIVEL='N',"
            'Numero_C
            strSQL = strSQL + "NUMERO_C='0000',"
            'Comparilha_Credito
            strSQL = strSQL + "COMPARTILHA_CREDITO='N',"
            'Grupo_ACDG
            strSQL = strSQL + "GRUPO_ACDG='0000',"
            'ATIVO
            strSQL = strSQL + "ATIVO='" + ramal.Ativo + "',"
            'CREDITO_MENSAL
            strSQL = strSQL + "CREDITO_MENSAL='" + ramal.Credito_Mensal.ToString().Replace(",", ".") + "',"
            'SALDO_ATUAL
            'strSQL = strSQL + "SALDO_ATUAL='" + ramal.Saldo_Atual.ToString().Replace(",", ".") + "',"
            'LIMITE_COMPARTILHAMENTO
            strSQL = strSQL + "LIMITE_COMPARTILHAMENTO='000',"
            'GRP_CODIGO
            strSQL = strSQL + "GRP_CODIGO='" + ramal.Grp_Codigo + "',"
            'DDI
            strSQL = strSQL + "DDI='" + ramal.DDI + "',"
            'DDD
            strSQL = strSQL + "DDD='" + ramal.DDD + "',"
            'LOCAL
            strSQL = strSQL + "LOCAL='" + ramal.Local + "',"
            'CELULAR
            strSQL = strSQL + "CELULAR='" + ramal.Celular + "',"
            'POSSUI_BLOQUEIO
            strSQL = strSQL + "POSSUI_BLOQUEIO='" + ramal.Possui_Bloqueio + "',"
            'A_COBRAR
            strSQL = strSQL + "A_COBRAR='S',"
            'DDG
            strSQL = strSQL + "DDG='S',"
            'EM_USO
            strSQL = strSQL + "EM_USO='N',"
            'CREDITO_EM_USO
            strSQL = strSQL + "CREDITO_EM_USO='" + ramal.Credito_em_Uso + "',"
            'BLOQUEIO_EMAIL
            strSQL = strSQL + "BLOQUEIO_EMAIL='N',"
            'CUSTO_RAMAL
            strSQL = strSQL + "CUSTO_RAMAL='" + ramal.Custo_Ramal.ToString().Replace(",", ".") + "',"
            'IDSUBGRUPO  ***************************************************************CORRIGIR*********************
            strSQL = strSQL + "IDSUBGRUPO='1',"
            'CATEGORIA
            strSQL = strSQL + "CATEGORIA='" + ramal.Categoria + "',"
            'BLOQUEAVEL
            strSQL = strSQL + "BLOQUEAVEL='" + ramal.Bloqueavel + "',"
            'INTERNAL
            strSQL = strSQL + "INTERNAL='" + ramal.Internal + "',"
            'OEM
            strSQL = strSQL + "OEM='" + ramal.OEM + "',"
            'ATIVACAO
            strSQL = strSQL + "ATIVACAO= to_date('" + ramal.Ativacao + "','dd/mm/yyyy hh24:mi:ss'),"
            'DESATIVADO
            strSQL = strSQL + "DESATIVADO= to_date('" + ramal.Desativado + "','dd/mm/yyyy hh24:mi:ss'),"
            'CODIGO_MODELO
            strSQL = strSQL + "CODIGO_MODELO='" + ramal.Codigo_Modelo.ToString() + "',"
            'CODIGO_LINHA
            strSQL = strSQL + "CODIGO_LINHA='" + ramal.Codigo_Linha.ToString() + "',"
            'CODIGO_LOCALIDADE
            strSQL = strSQL + "CODIGO_LOCALIDADE='" + ramal.Codigo_Localidade.ToString() + "',"
            'TN
            strSQL = strSQL + "TN='" + ramal.TN + "',"
            'CODIGO_SITE
            strSQL = strSQL + "CODIGO_SITE='" + ramal.Codigo_Site.ToString() + "',"
            'TIPO_RAMAL
            strSQL = strSQL + "TIPO_RAMAL='" + ramal.Tipo_Ramal.ToString() + "',"
            'REDE
            strSQL = strSQL + "REDE='" + ramal.Rede + "',"
            'CT
            strSQL = strSQL + "CT='" + ramal.CT + "',"
            'STATION
            strSQL = strSQL + "STATION='" + ramal.Station + "'"

            strSQL = strSQL + " where RAMAIS.NUMERO_A = '" + ramal.Numero_A + "'"

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

        InsertRamalLog(ramal, "B", autorlog)
        Return True

    End Function

    Public Function ExcluiRamal(ByVal numero_A As String, ByVal autorlog As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim ramal As New List(Of AppRamais)

        GetRamaisById(numero_A, ramal)

        Try

            InsertRamalLog(ramal.Item(0), "D", autorlog)

            Dim strSQL As String = "delete RAMAIS "
            strSQL = strSQL + "where RAMAIS.NUMERO_A ='" + numero_A + "'"

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

    Public Function InsertRamalLog(ByVal ramal As AppRamais, ByVal tipo As Char, ByVal autorlog As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Try

            Dim strSQL As String

            strSQL = "insert into RAMAISLOG(ID, TIPO, DATA, AUTOR, NUMERO_A, GRP_CODIGO, CUSTO_RAMAL, "
            strSQL = strSQL + "TARIFAVEL, ATIVO, CREDITO_MENSAL, SALDO_ATUAL, HISTORICO, DATAHISTORICO, BLOQUEAVEL, POSSUI_BLOQUEIO, CATEGORIA)"
            'ID
            strSQL = strSQL + "( select (select nvl(max(ID),0)+1 FROM RAMAISLOG),"

            'TIPO
            strSQL = strSQL + " '" + tipo + "',"

            'DATA
            strSQL = strSQL + "to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss'),"
            'AUTOR
            strSQL = strSQL + "'" + autorlog + "',"
            'NUMERO_A
            strSQL = strSQL + "NUMERO_A,"
            'GRP_CODIGO
            strSQL = strSQL + "GRP_CODIGO,"
            'CUSTO_RAMAL
            strSQL = strSQL + "CUSTO_RAMAL,"
            'Tarifavel
            strSQL = strSQL + "TARIFAVEL,"
            'ATIVO
            strSQL = strSQL + "ATIVO,"
            'CREDITO_MENSAL
            strSQL = strSQL + "CREDITO_MENSAL,"
            'SALDO_ATUAL
            strSQL = strSQL + "SALDO_ATUAL,"
            'HISTORICO
            strSQL = strSQL + "'',"
            'DATAHISTORICO
            strSQL = strSQL + "'',"
            'BLOQUEAVEL
            strSQL = strSQL + "BLOQUEAVEL,"
            'POSSUI_BLOQUEIO
            strSQL = strSQL + "POSSUI_BLOQUEIO,"
            'CATEGORIA
            strSQL = strSQL + "CATEGORIA FROM RAMAIS where RAMAIS.NUMERO_A ='" + ramal.Numero_A + "')"


            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
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

    Public Function GetLinhaById(ByVal pcodigo As String) As List(Of AppLinhas)
        Dim connection As New OleDbConnection(strConn)
        Dim listLinha As New List(Of AppLinhas)

        Dim strSQL As String = ""
        strSQL = strSQL + " select CODIGO_LINHA, NUM_LINHA "
        strSQL = strSQL + " from LINHAS "
        strSQL = strSQL + " where CODIGO_LINHA ='" + pcodigo + "'"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppLinhas(reader.Item("CODIGO_LINHA").ToString, reader.Item("NUM_LINHA").ToString)
                listLinha.Add(_registro)
            End While
        End Using

        Return listLinha
    End Function


    Public Function UpdateParticulares(ByVal codigo As String, ByVal autorlog As String, ByVal particular As String, ByVal data As String) As Boolean
        Dim connection As New OleDbConnection(strConn)


        Try
            Dim sql As String = ""
            sql = "update cdrs set particular='" + particular + "',data_apontamento=sysdate,autor_apontamento='" + autorlog + "' where data_inicio=to_date('" + data + "','DD/MM/YYYY HH24:MI:SS')  and codigo='" + codigo + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
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
