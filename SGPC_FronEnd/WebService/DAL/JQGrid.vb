Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Runtime.Serialization.Formatters
Imports System.Collections.Generic



Public Class JQGrid

    Public Structure JQGridResult
        Public page As Integer
        Public total As Integer
        Public records As Integer
        Public rows As JQGridRow()
    End Structure


    Public Structure JQGridRow
        Public id As Integer
        Public cell As List(Of String)
    End Structure



    Private _strConn As String
    Public Property StrConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Public Function GetJson(ByVal dt As DataTable) As String
        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        serializer.MaxJsonLength = 2000000000
        Dim rows As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object)
        For Each dr As DataRow In dt.Rows
            row = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                row.Add(col.ColumnName, dr(col))
            Next
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function


    Public Function GetJsonV2(ByVal dt As DataTable, ByVal _page As Integer, ByVal _total As Integer, ByVal _record As Integer) As String
        Dim serializer As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
        serializer.MaxJsonLength = 2000000000
        Dim _rows As New List(Of JQGridRow)
        Dim row As List(Of String)
        Dim _jqgridresult As New JQGridResult

        For Each dr As DataRow In dt.Rows
            row = New List(Of String)
            Dim myRow As New JQGridRow()
            For Each col As DataColumn In dt.Columns
                If col.ColumnName = "ID" Then
                    myRow.id = dr(col)
                End If
                'row.Add(col.ColumnName, dr(col))
                row.Add(dr(col).ToString)
            Next
            ''rows.Add(row)
            myRow.cell = row
            _rows.Add(myRow)
        Next
        _jqgridresult.rows = _rows.ToArray
        _jqgridresult.page = _page
        _jqgridresult.total = _total
        _jqgridresult.records = _record
        'Return serializer.Serialize(rows)
        Return serializer.Serialize(_jqgridresult)
    End Function


    Public Function myDataTable(ByVal SQL As String) As DataTable
        Dim cn As OleDbConnection
        Dim dsTemp As DataSet
        Dim dsCmd As OleDbDataAdapter

        cn = New OleDbConnection(StrConn)
        cn.Open()

        dsCmd = New OleDbDataAdapter(SQL, cn)
        dsTemp = New DataSet()
        dsCmd.Fill(dsTemp, "myQuery")
        cn.Close()
        Return dsTemp.Tables(0)
    End Function

    Public Function CriaGrid(ByVal psql As String) As String
        Dim result As String = ""
        Dim _dt As DataTable = myDataTable(psql)
        result = GetJson(_dt)

        Return result

    End Function

    Public Function CriaGridV2(ByVal psql As String, ByVal page As Integer, ByVal total As Integer, ByVal record As Integer) As String
        Dim result As String = ""
        Dim _dt As DataTable = myDataTable(psql)
        result = GetJsonV2(_dt, page, total, record)

        Return result

    End Function

    Public Function JsonToArray(ByVal pJson As String) As List(Of JQGridFilterRules)


        'Dim textAreaJson As String = "[{""OrderId"":0,""Name"":""Summary"",""MaxLen"":""200""},{""OrderId"":1,""Name"":""Details"",""MaxLen"":""0""}]"
        Dim textAreaJson As String = pJson
        Dim js As New System.Web.Script.Serialization.JavaScriptSerializer
        Dim rawdata As New System.Web.Script.Serialization.JavaScriptSerializer
        'rawdata = js.DeserializeObject(textAreaJson)
        'rawdata = js.Deserialize(textAreaJson)
        Dim lstTextAreas As New System.Collections.Generic.List(Of JQGridFilterRules)
        lstTextAreas = js.Deserialize(Of List(Of JQGridFilterRules))(textAreaJson)

        Return lstTextAreas

    End Function


    Public Function CarregaData(ByVal numberOfRows As Integer, ByVal pageIndex As Integer, ByVal sortColumnName As String, ByVal sortOrderBy As String, ByVal strSQL As String, ByVal arrayFilters As List(Of JQGridFilterRules)) As String
        Dim strSQL2 As String = ""
        Dim startRow As Integer = (pageIndex * numberOfRows) + 1
        Dim endRow As Integer = startRow - 1

        'ordenação
        If sortColumnName <> "" Then
            strSQL = strSQL + " order by " & sortColumnName & " " & sortOrderBy
        End If

        strSQL2 = "SELECT  * from (select a.*,rownum as rnum from (" & strSQL

        strSQL2 = strSQL2 + "	)a WHERE 1=1 "

        'coloca os filtros na busca
        For Each _filtro As JQGridFilterRules In arrayFilters
            strSQL2 = strSQL2 + " and upper(a." & _filtro.field & ") like UPPER('%" & _filtro.data.Replace("_", "") & "%')"
        Next

        strSQL2 = strSQL2 + " ) where rnum BETWEEN  " & startRow - numberOfRows & " and " & endRow

        Return strSQL2
        'Response.Write("passou")
    End Function


    Public Function CarregaTotal(ByVal strSQL As String, ByVal strConexao As String) As Integer
        Dim strSQL2 As String
        Dim connection As New OleDbConnection(strConexao)
        Dim totalrecords As Integer = 0
        strSQL2 = ""
        strSQL2 = "SELECT  count(*)total from (" & strSQL

        strSQL2 = strSQL2 + "	)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL2
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                totalrecords = reader.Item("total")
            End While
        End Using

        Return totalrecords

    End Function


End Class
