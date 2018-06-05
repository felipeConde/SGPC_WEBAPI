Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data.OleDb

<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetStringList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        ' Create array of movies  
        Dim list As New List(Of String)

        Dim connection As New OleDbConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ToString)

        Dim strSQL As String = ""
        strSQL = strSQL + " select tipo_serv from AUTOCOMPLETE_SERVICES "
        strSQL = strSQL + " where Upper(tipo_serv) like Upper('" + prefixText + "%')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("tipo_serv").ToString)
                list.Add(_registro)
            End While
        End Using

        ' Return matching movies
        Return list
    End Function

End Class