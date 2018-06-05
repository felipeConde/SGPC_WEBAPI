Imports Microsoft.VisualBasic

Public Class AppCentral

    Public Sub New()

    End Sub

    Public Sub New(ByVal pAreaCode As String, ByVal pCentral As String, ByVal pTipo As Integer, ByVal pDescricao As String, ByVal pOperadora As Integer)

        _area_code = pAreaCode
        _central = pCentral
        _tipo = pTipo
        _descricao = pDescricao
        _operadora = pOperadora

    End Sub


    Private _area_code As String
    Public Property Area_Code As String
        Get
            Return _area_code
        End Get
        Set(ByVal value As String)
            _area_code = value
        End Set
    End Property

    Private _central As String
    Public Property Central As String
        Get
            Return _central
        End Get
        Set(ByVal value As String)
            _central = value
        End Set
    End Property

    Private _tipo As Integer
    Public Property Tipo As Integer
        Get
            Return _tipo
        End Get
        Set(ByVal value As Integer)
            _tipo = value
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

    Private _operadora As Integer
    Public Property Operadora As Integer
        Get
            Return _operadora
        End Get
        Set(ByVal value As Integer)
            _operadora = value
        End Set
    End Property

End Class
