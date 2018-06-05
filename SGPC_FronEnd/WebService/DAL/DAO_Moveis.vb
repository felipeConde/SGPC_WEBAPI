Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Collections.Generic
Imports System

Public Class DAO_Moveis

    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

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

    Public Function GetMovelById(ByVal pcodigo As Integer) As List(Of AppMoveis)
        Dim connection As New OleDbConnection(strConn)
        Dim listMovel As New List(Of AppMoveis)

        Dim strSQL As String = "select CODIGO_APARELHO "
        strSQL = strSQL + ", nvl(VALOR, '') AS VALOR"
        strSQL = strSQL + ", nvl(ACESSORIOS, '') AS ACESSORIOS"
        strSQL = strSQL + ", nvl(NATUREZA, '') AS NATUREZA"
        strSQL = strSQL + ", nvl(IMEI, '') AS IMEI"
        strSQL = strSQL + ", nvl(HEXA, '') AS HEXA"
        strSQL = strSQL + ", nvl(GARANTIA, '') AS GARANTIA"
        strSQL = strSQL + ", nvl(COD_MODELO, '') AS COD_MODELO"
        strSQL = strSQL + ", nvl(NOTA_FISCAL, '') AS NOTA_FISCAL"
        strSQL = strSQL + ", nvl(PIN_APARELHO, '') AS PIN_APARELHO"
        strSQL = strSQL + ", nvl(ESTOQUE, '') AS ESTOQUE"
        strSQL = strSQL + ", nvl(BACKUP, '') AS BACKUP"
        strSQL = strSQL + ", nvl(SUCATA, '') AS SUCATA"
        strSQL = strSQL + ", nvl(PROPRIEDADE_ESTOQUE, '') AS PROPRIEDADE"
        strSQL = strSQL + ", nvl(ORDEM_SERVICO, '') AS ORDEM_SERVICO"
        strSQL = strSQL + ", nvl(CHAMADO_RETIRADA, '') AS CHAMADO"
        strSQL = strSQL + ", nvl(DATA_RETIRADA, '') AS DATA_RET"
        strSQL = strSQL + ", nvl(EMISSAO, '') AS EMISSAO"
        strSQL = strSQL + ", nvl(PERDIDO, '') AS PERDIDO"

        strSQL = strSQL + " FROM APARELHOS_MOVEIS "
        strSQL = strSQL + "where CODIGO_APARELHO = " + Convert.ToString(pcodigo) + ""


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppMoveis(reader.Item("CODIGO_APARELHO").ToString)
                listMovel.Add(_registro)
            End While
        End Using

        Return listMovel
    End Function

End Class
