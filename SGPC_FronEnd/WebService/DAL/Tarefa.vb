Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class Tarefa

    Private _codigo As Integer
    Public Property Codigo As Integer
        Get
            Return _codigo
        End Get
        Set(ByVal value As Integer)
            _codigo = value
        End Set
    End Property

    Private _data As Date
    Public Property Data As Date
        Get
            Return _data
        End Get
        Set(ByVal value As Date)
            _data = value
        End Set
    End Property

    Private _descricao As String
    Public Property Descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

    Private _autor As String
    Public Property Autor As String
        Get
            Return _autor
        End Get
        Set(ByVal value As String)
            _autor = value
        End Set
    End Property

    Private _status As Integer
    Public Property Status As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property


    Private _cod_tarefa As Integer
    Public Property Codtarefa As String
        Get
            Return _cod_tarefa
        End Get
        Set(ByVal value As String)
            _cod_tarefa = value
        End Set
    End Property

    Private _obs As String
    Public Property OBS As String
        Get
            Return _obs
        End Get
        Set(ByVal value As String)
            _obs = value
        End Set
    End Property

    Private _inicioTarefa As Date
    Public Property InicioTarefa As Date
        Get
            Return _inicioTarefa
        End Get
        Set(ByVal value As Date)
            _inicioTarefa = value
        End Set
    End Property

    Private _fimTarefa As Date
    Public Property FimTarefa As Date
        Get
            Return _fimTarefa
        End Get
        Set(ByVal value As Date)
            _fimTarefa = value
        End Set
    End Property

    Private _faturas As List(Of Fatura)
    Public Property Faturas As List(Of Fatura)
        Get
            Return _faturas
        End Get
        Set(ByVal value As List(Of Fatura))
            _faturas = value
        End Set
    End Property

End Class
