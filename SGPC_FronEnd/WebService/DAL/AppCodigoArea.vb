﻿Imports Microsoft.VisualBasic

Public Class AppCodigoArea
    
    Private _codigo As String
    Private _operadora As String
    Private _tipo_ligacao As String
    Private _descricao As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal poperadora As String)
        _codigo = pcodigo
        _operadora = poperadora
    End Sub

    Public Sub New(ByVal pcodigo As String, ByVal poperadora As String, ByVal ptipo_ligacao As String, ByVal pdescricao As String)
        _codigo = pcodigo
        _operadora = poperadora
        _tipo_ligacao = ptipo_ligacao
        _descricao = pdescricao
    End Sub

    Public Property codigo As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property operadora As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

    Public Property tipo_ligacao As String
        Get
            Return _tipo_ligacao
        End Get
        Set(ByVal value As String)
            _tipo_ligacao = value
        End Set
    End Property

    Public Property descricao As String
        Get
            Return _descricao
        End Get
        Set(ByVal value As String)
            _descricao = value
        End Set
    End Property

End Class
