Imports Microsoft.VisualBasic

Public Class AppLote




    Private _num_lote As String
    Public Property Num_Lote As String
        Get
            Return _num_lote
        End Get
        Set(ByVal value As String)
            _num_lote = value
        End Set
    End Property


    Private _mesANo As String
    Public Property MesANo As String
        Get
            Return _mesANo
        End Get
        Set(ByVal value As String)
            _mesANo = value
        End Set
    End Property


    Private _codOperadora As String
    Public Property CodOperadora As String
        Get
            Return _codOperadora
        End Get
        Set(ByVal value As String)
            _codOperadora = value
        End Set
    End Property

    Private _nomeOperadora As String
    Public Property NomeOperadora As String
        Get
            Return _nomeOperadora
        End Get
        Set(ByVal value As String)
            _nomeOperadora = value
        End Set
    End Property

    Private _laudo As AppLoteLaudo
    Public Property Laudo As AppLoteLaudo
        Get
            Return _laudo
        End Get
        Set(ByVal value As AppLoteLaudo)
            _laudo = value
        End Set
    End Property

    Private _valor_cobrado As Double
    Public Property Valor_Cobrado As Double
        Get
            Return _valor_cobrado
        End Get
        Set(value As Double)
            _valor_cobrado = value
        End Set
    End Property

    Private _valor_apagar As Double
    Public Property Valor_Apagar As Double
        Get
            Return _valor_apagar
        End Get
        Set(value As Double)
            _valor_apagar = value
        End Set
    End Property

End Class
