Imports Microsoft.VisualBasic

Public Class AppRamais

   Private _numero_a As String
   Private _tarifavel As String
   Private _transferivel As String
   Private _numero_c As String
   Private _compartilha_credito As String
   Private _grupo_acdg As String
   Private _ativo As String
    Private _credito_mensal As String
    Private _saldo_atual As String
    Private _limite_compartilhamento As String
    Private _grp_codigo As String
    Private _ddi As String
    Private _ddd As String
    Private _local As String
    Private _celular As String
    Private _possui_bloqueio As String
    Private _a_cobrar As String
    Private _ddg As String
    Private _em_uso As String
    Private _credito_em_uso As String
    Private _bloqueio_email As String
    Private _custo_ramal As String
    Private _idsubgrupo As String
    Private _categoria As String
    Private _bloqueavel As String
    Private _internal As String
    Private _oem As String
    Private _ativacao As String
    Private _desativado As String
    Private _codigo_modelo As String
    Private _codigo_linha As String
    Private _codigo_localidade As String
    Private _tn As String
    Private _codigo_site As String
    Private _rede As String
    Private _ct As String
    Private _station As String
    Private _tipo_ramal As String

    Private _grupo As String
    Private _usuario As String
    Private _unidade As String
    Private _setor As String

    Public Sub New(ByVal pnumero_a As String)
        _numero_a = pnumero_a
    End Sub

    Public Sub New(ByVal pnumero_a As String, ByVal ptarifavel As String, ByVal ptransferivel As String, ByVal pnumero_c As String, ByVal pcompartilha_credito As String, ByVal pgrupo_acdg As String, ByVal pativo As String, ByVal pcredito_mensal As String, ByVal psaldo_atual As String, ByVal plimite_compartilhamento As String, ByVal pgrp_codigo As String, ByVal pddi As String, ByVal pddd As String, ByVal plocal As String, ByVal pcelular As String, ByVal ppossui_bloqueio As String, ByVal pa_cobrar As String, ByVal pddg As String, ByVal pem_uso As String, ByVal pcredito_em_uso As String, ByVal pbloqueio_email As String, ByVal pcusto_ramal As String, ByVal pidsubgrupo As String, ByVal pcategoria As String, ByVal pbloqueavel As String, ByVal pinternal As String, ByVal poem As String, ByVal pativacao As String, ByVal pdesativado As String, ByVal pcodigo_modelo As String, ByVal pcodigo_linha As String, ByVal pcodigo_localidade As String, ByVal ptn As String, ByVal pcodigo_site As String, ByVal prede As String, ByVal pct As String, ByVal pstation As String, ByVal ptipo_ramal As String, ByVal pgrupo As String, ByVal pusuario As String, ByVal punidade As String, ByVal psetor As String)
        _numero_a = pnumero_a
        _tarifavel = ptarifavel
        _transferivel = ptransferivel
        _numero_c = pnumero_c
        _compartilha_credito = pcompartilha_credito
        _grupo_acdg = pgrupo_acdg
        _ativo = pativo
        _credito_mensal = pcredito_mensal
        _saldo_atual = psaldo_atual
        _limite_compartilhamento = plimite_compartilhamento
        _grp_codigo = pgrp_codigo
        _ddi = pddi
        _ddd = pddd
        _local = plocal
        _celular = pcelular
        _possui_bloqueio = ppossui_bloqueio
        _a_cobrar = pa_cobrar
        _ddg = pddg
        _em_uso = pem_uso
        _credito_em_uso = pcredito_em_uso
        _bloqueio_email = pbloqueio_email
        _custo_ramal = pcusto_ramal
        _idsubgrupo = pidsubgrupo
        _categoria = pcategoria
        _bloqueavel = pbloqueavel
        _internal = pinternal
        _oem = poem
        _ativacao = pativacao
        _desativado = pdesativado
        _codigo_modelo = pcodigo_modelo
        _codigo_linha = pcodigo_linha
        _codigo_localidade = pcodigo_localidade
        _tn = ptn
        _codigo_site = pcodigo_site
        _rede = prede
        _ct = pct
        _station = pstation
        _tipo_ramal = ptipo_ramal
        _grupo = pgrupo
        _usuario = pusuario
        _unidade = punidade
        _setor = psetor

    End Sub

    Public Property Numero_A() As String
        Get
            Return _numero_a
        End Get
        Set(ByVal value As String)
            _numero_a = value
        End Set
    End Property

    Public Property Tarifavel() As String
        Get
            Return _tarifavel
        End Get
        Set(ByVal value As String)
            _tarifavel = value
        End Set
    End Property

    Public Property Transferivel() As String
        Get
            Return _transferivel
        End Get
        Set(ByVal value As String)
            _transferivel = value
        End Set
    End Property

    Public Property Numero_C() As String
        Get
            Return _numero_c
        End Get
        Set(ByVal value As String)
            _numero_c = value
        End Set
    End Property

    Public Property Compartilha_Credito() As String
        Get
            Return _compartilha_credito
        End Get
        Set(ByVal value As String)
            _compartilha_credito = value
        End Set
    End Property

    Public Property Grupo_Acdg() As String
        Get
            Return _grupo_acdg
        End Get
        Set(ByVal value As String)
            _grupo_acdg = value
        End Set
    End Property

    Public Property Ativo() As String
        Get
            Return _ativo
        End Get
        Set(ByVal value As String)
            _ativo = value
        End Set
    End Property

    Public Property Credito_Mensal() As String
        Get
            Return _credito_mensal
        End Get
        Set(ByVal value As String)
            _credito_mensal = value
        End Set
    End Property

    Public Property Saldo_Atual() As String
        Get
            Return _saldo_atual
        End Get
        Set(ByVal value As String)
            _saldo_atual = value
        End Set
    End Property

    Public Property Limite_Compartilhamento() As String
        Get
            Return _limite_compartilhamento
        End Get
        Set(ByVal value As String)
            _limite_compartilhamento = value
        End Set
    End Property

    Public Property Grp_Codigo() As String
        Get
            Return _grp_codigo
        End Get
        Set(ByVal value As String)
            _grp_codigo = value
        End Set
    End Property

    Public Property DDI() As String
        Get
            Return _ddi
        End Get
        Set(ByVal value As String)
            _ddi = value
        End Set
    End Property

    Public Property DDD() As String
        Get
            Return _ddd
        End Get
        Set(ByVal value As String)
            _ddd = value
        End Set
    End Property

    Public Property Local() As String
        Get
            Return _local
        End Get
        Set(ByVal value As String)
            _local = value
        End Set
    End Property

    Public Property Celular() As String
        Get
            Return _celular
        End Get
        Set(ByVal value As String)
            _celular = value
        End Set
    End Property

    Public Property Possui_Bloqueio() As String
        Get
            Return _possui_bloqueio
        End Get
        Set(ByVal value As String)
            _possui_bloqueio = value
        End Set
    End Property

    Public Property A_Cobrar() As String
        Get
            Return _a_cobrar
        End Get
        Set(ByVal value As String)
            _a_cobrar = value
        End Set
    End Property

    Public Property DDG() As String
        Get
            Return _ddg
        End Get
        Set(ByVal value As String)
            _ddg = value
        End Set
    End Property

    Public Property Em_Uso() As String
        Get
            Return _em_uso
        End Get
        Set(ByVal value As String)
            _em_uso = value
        End Set
    End Property

    Public Property Credito_em_Uso() As String
        Get
            Return _credito_em_uso
        End Get
        Set(ByVal value As String)
            _credito_em_uso = value
        End Set
    End Property

    Public Property Bloqueio_Email() As String
        Get
            Return _bloqueio_email
        End Get
        Set(ByVal value As String)
            _bloqueio_email = value
        End Set
    End Property

    Public Property Custo_Ramal() As String
        Get
            Return _custo_ramal
        End Get
        Set(ByVal value As String)
            _custo_ramal = value
        End Set
    End Property

    Public Property Idsubgrupo() As String
        Get
            Return _idsubgrupo
        End Get
        Set(ByVal value As String)
            _idsubgrupo = value
        End Set
    End Property

    Public Property Categoria() As String
        Get
            Return _categoria
        End Get
        Set(ByVal value As String)
            _categoria = value
        End Set
    End Property

    Public Property Bloqueavel() As String
        Get
            Return _bloqueavel
        End Get
        Set(ByVal value As String)
            _bloqueavel = value
        End Set
    End Property

    Public Property Internal() As String
        Get
            Return _internal
        End Get
        Set(ByVal value As String)
            _internal = value
        End Set
    End Property

    Public Property OEM() As String
        Get
            Return _oem
        End Get
        Set(ByVal value As String)
            _oem = value
        End Set
    End Property

    Public Property Ativacao() As String
        Get
            Return _ativacao
        End Get
        Set(ByVal value As String)
            _ativacao = value
        End Set
    End Property

    Public Property Desativado() As String
        Get
            Return _desativado
        End Get
        Set(ByVal value As String)
            _desativado = value
        End Set
    End Property

    Public Property Codigo_Modelo() As String
        Get
            Return _codigo_modelo
        End Get
        Set(ByVal value As String)
            _codigo_modelo = value
        End Set
    End Property

    Public Property Codigo_Linha() As String
        Get
            Return _codigo_linha
        End Get
        Set(ByVal value As String)
            _codigo_linha = value
        End Set
    End Property

    Public Property Codigo_Localidade() As String
        Get
            Return _codigo_localidade
        End Get
        Set(ByVal value As String)
            _codigo_localidade = value
        End Set
    End Property

    Public Property TN() As String
        Get
            Return _tn
        End Get
        Set(ByVal value As String)
            _tn = value
        End Set
    End Property

    Public Property Codigo_Site() As String
        Get
            Return _codigo_site
        End Get
        Set(ByVal value As String)
            _codigo_site = value
        End Set
    End Property

    Public Property Rede() As String
        Get
            Return _rede
        End Get
        Set(ByVal value As String)
            _rede = value
        End Set
    End Property

    Public Property CT() As String
        Get
            Return _ct
        End Get
        Set(ByVal value As String)
            _ct = value
        End Set
    End Property

    Public Property Station() As String
        Get
            Return _station
        End Get
        Set(ByVal value As String)
            _station = value
        End Set
    End Property

    Public Property Tipo_Ramal() As String
        Get
            Return _tipo_ramal
        End Get
        Set(ByVal value As String)
            _tipo_ramal = value
        End Set
    End Property


   Public Property Grupo() As String
      Get
         Return _grupo
      End Get
      Set(ByVal value As String)
         _grupo = value
      End Set
   End Property

   Public Property Usuario() As String
      Get
         Return _usuario
      End Get
      Set(ByVal value As String)
         _usuario = value
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

End Class
