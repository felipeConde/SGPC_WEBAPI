Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class AppLinhas

    Private _codigo_linha As Integer
    Private _num_linha As String

    Private _codigo_aparelho As Integer

    Private _codigo_operadora As Integer
    Private _operadora As String

    Private _codigo_usuario As Integer
    Private _nome_usuario As String

    Private _unidade As String
    Private _setor As String
    Private _perfil As String
    Private _fim_ciclo As String
    Private _venc_conta As String
    Private _minutos As Integer

    Public Sub New()
    End Sub

    Public Sub New(ByVal pcodigo_linha As Integer)
        _codigo_linha = pcodigo_linha
    End Sub

    Public Sub New(ByVal pcodigo_linha As Integer, ByVal pnum_linha As String)
        _codigo_linha = pcodigo_linha
        _num_linha = pnum_linha
    End Sub

    Public Sub New(ByVal pcodigo_linha As Integer, ByVal pnum_linha As String, ByVal pcodigo_aparelho As Integer, ByVal pcodigo_operadora As Integer, ByVal poperadora As String, ByVal pcodigo_usuario As Integer, ByVal pnome_usuario As String)
        _codigo_linha = pcodigo_linha
        _num_linha = pnum_linha
        _codigo_aparelho = pcodigo_aparelho
        _codigo_operadora = pcodigo_operadora
        _operadora = poperadora
        _codigo_usuario = pcodigo_usuario
        _nome_usuario = pnome_usuario
    End Sub

    Public Sub New(ByVal pcodigo_linha As Integer, ByVal pnum_linha As String, ByVal pcodigo_aparelho As Integer, ByVal pcodigo_operadora As Integer, ByVal poperadora As String, ByVal pcodigo_usuario As Integer, ByVal pnome_usuario As String, ByVal punidade As String, ByVal psetor As String, ByVal pperfil As String, ByVal pfim_ciclo As String, ByVal pvenc_conta As String, ByVal pminutos As Integer, ByVal pprotocolo_cancel As String)
        _codigo_linha = pcodigo_linha
        _num_linha = pnum_linha
        _codigo_aparelho = pcodigo_aparelho
        _codigo_operadora = pcodigo_operadora
        _operadora = poperadora
        _codigo_usuario = pcodigo_usuario
        _nome_usuario = pnome_usuario
        _unidade = punidade
        _setor = psetor
        _perfil = pperfil
        _fim_ciclo = pfim_ciclo
        _venc_conta = pvenc_conta
        _minutos = pminutos
        _protocolo_cancel = pprotocolo_cancel
    End Sub

    Public Property Codigo_Linha() As Integer
        Get
            Return _codigo_linha
        End Get
        Set(ByVal value As Integer)
            _codigo_linha = value
        End Set
    End Property

    Public Property Num_Linha() As String
        Get
            Return _num_linha
        End Get
        Set(ByVal value As String)
            _num_linha = value
        End Set
    End Property

    Public Property Codigo_Aparelho() As Integer
        Get
            Return _codigo_aparelho
        End Get
        Set(ByVal value As Integer)
            _codigo_aparelho = value
        End Set
    End Property

    Public Property Codigo_Operadora() As Integer
        Get
            Return _codigo_operadora
        End Get
        Set(ByVal value As Integer)
            _codigo_operadora = value
        End Set
    End Property

    Public Property Operadora() As String
        Get
            Return _operadora
        End Get
        Set(ByVal value As String)
            _operadora = value
        End Set
    End Property

    Public Property Codigo_Usuario() As Integer
        Get
            Return _codigo_usuario
        End Get
        Set(ByVal value As Integer)
            _codigo_usuario = value
        End Set
    End Property

    Public Property Nome_Usuario() As String
        Get
            Return _nome_usuario
        End Get
        Set(ByVal value As String)
            _nome_usuario = value
        End Set
    End Property


    Public Property Unidade() As String
        Get
            Return _unidade
        End Get
        Set(ByVal value As String)
            _unidade = value
        End Set
    End Property

    Public Property Setor() As String
        Get
            Return _setor
        End Get
        Set(ByVal value As String)
            _setor = value
        End Set
    End Property

    Public Property Perfil() As String
        Get
            Return _perfil
        End Get
        Set(ByVal value As String)
            _perfil = value
        End Set
    End Property

    Public Property FimCiclo() As String
        Get
            Return _fim_ciclo
        End Get
        Set(ByVal value As String)
            _fim_ciclo = value
        End Set
    End Property


    Public Property VencConta() As String
        Get
            Return _venc_conta
        End Get
        Set(ByVal value As String)
            _venc_conta = value
        End Set
    End Property

    Public Property Minutos() As Integer
        Get
            Return _minutos
        End Get
        Set(ByVal value As Integer)
            _minutos = value
        End Set
    End Property


    Private _status As Integer
    Public Property Status As Integer
        Get
            Return _status
        End Get
        Set(ByVal value As Integer)
            _status = value
        End Set
    End Property

    Private _ativacao As String
    Public Property Ativacao As String
        Get
            Return _ativacao
        End Get
        Set(ByVal value As String)
            _ativacao = value
        End Set
    End Property

    Private _desativada As String
    Public Property Desativada As String
        Get
            Return _desativada
        End Get
        Set(ByVal value As String)
            _desativada = value
        End Set
    End Property

    Private _digital As String
    Public Property Digital As String
        Get
            Return _digital
        End Get
        Set(ByVal value As String)
            _digital = value
        End Set
    End Property

    Private _contrato As String
    Public Property Contrato As String
        Get
            Return _contrato
        End Get
        Set(ByVal value As String)
            _contrato = value
        End Set
    End Property

    Private _oem As String
    Public Property Oem As String
        Get
            Return _oem
        End Get
        Set(ByVal value As String)
            _oem = value
        End Set
    End Property

    Private _vencContrato As String
    Public Property VencContrato As String
        Get
            Return _vencContrato
        End Get
        Set(ByVal value As String)
            _vencContrato = value
        End Set
    End Property

    Private _conta As String
    Public Property Conta As String
        Get
            Return _conta
        End Get
        Set(ByVal value As String)
            _conta = value
        End Set
    End Property

    Private _internet As String
    Public Property Internet As String
        Get
            Return _internet
        End Get
        Set(ByVal value As String)
            _internet = value
        End Set
    End Property

    Private _transferencia As String
    Public Property Transferencia As String
        Get
            Return _transferencia
        End Get
        Set(ByVal value As String)
            _transferencia = value
        End Set
    End Property

    Private _fax As String
    Public Property Fax As String
        Get
            Return _fax
        End Get
        Set(ByVal value As String)
            _fax = value
        End Set
    End Property

    Private _codigoPlano As Integer
    Public Property CodigoPlano As Integer
        Get
            Return _codigoPlano
        End Get
        Set(ByVal value As Integer)
            _codigoPlano = value
        End Set
    End Property

    Private _codigoFornecedor As Integer
    Public Property CodigoFornecedor As Integer
        Get
            Return _codigoFornecedor
        End Get
        Set(ByVal value As Integer)
            _codigoFornecedor = value
        End Set
    End Property

    Private _codigoLocalidade As Integer
    Public Property CodigoLocalidade As Integer
        Get
            Return _codigoLocalidade
        End Get
        Set(ByVal value As Integer)
            _codigoLocalidade = value
        End Set
    End Property

    Private _range1 As String
    Public Property Range1 As String
        Get
            Return _range1
        End Get
        Set(ByVal value As String)
            _range1 = value
        End Set
    End Property

    Private _range2 As String
    Public Property Range2 As String
        Get
            Return _range2
        End Get
        Set(ByVal value As String)
            _range2 = value
        End Set
    End Property

    Private _endereco As String
    Public Property Endereco As String
        Get
            Return _endereco
        End Get
        Set(ByVal value As String)
            _endereco = value
        End Set
    End Property

    Private _protocolo As String
    Public Property Protocolo As String
        Get
            Return _protocolo
        End Get
        Set(ByVal value As String)
            _protocolo = value
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

    Private _chavePabx As String
    Public Property ChavePabx As String
        Get
            Return _chavePabx
        End Get
        Set(ByVal value As String)
            _chavePabx = value
        End Set
    End Property

    Private _codigoTipo As String
    Public Property CodigoTipo As String
        Get
            Return _codigoTipo
        End Get
        Set(ByVal value As String)
            _codigoTipo = value
        End Set
    End Property

    Private _pontaB As String
    Public Property PontaB As String
        Get
            Return _pontaB
        End Get
        Set(ByVal value As String)
            _pontaB = value
        End Set
    End Property

    Private _codigoUC As Integer
    Public Property CodigoUC As Integer
        Get
            Return _codigoUC
        End Get
        Set(ByVal value As Integer)
            _codigoUC = value
        End Set
    End Property

    Private _circuito As String
    Public Property Circuito As String
        Get
            Return _circuito
        End Get
        Set(ByVal value As String)
            _circuito = value
        End Set
    End Property

    Private _Grupos As New List(Of AppGrupo)
    Public Property Grupos As List(Of AppGrupo)
        Get
            Return _Grupos
        End Get
        Set(ByVal value As List(Of AppGrupo))
            _Grupos = value
        End Set
    End Property

    Private _contratoEmpresa As String
    Public Property ContratoEmpresa As String
        Get
            Return _contratoEmpresa
        End Get
        Set(ByVal value As String)
            _contratoEmpresa = value
        End Set
    End Property

    Private _local As String
    Public Property Local As String
        Get
            Return _local
        End Get
        Set(ByVal value As String)
            _local = value
        End Set
    End Property

    Private _codigoCliente As String
    Public Property CodigoCliente As String
        Get
            Return _codigoCliente
        End Get
        Set(ByVal value As String)
            _codigoCliente = value
        End Set
    End Property

    Private _protocolo_cancel As String
    Public Property Protocolo_Cancel() As String
        Get
            Return _protocolo_cancel
        End Get
        Set(ByVal value As String)
            _protocolo_cancel = value
        End Set
    End Property

    Private _conta_cont As String

    Public Property Conta_cont() As String
        Get
            Return _conta_cont
        End Get
        Set(ByVal value As String)
            _conta_cont = value
        End Set
    End Property
End Class
