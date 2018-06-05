Imports Microsoft.VisualBasic

Public Class AppUsuarios

   Private _codigo As Integer
   Private _nome_usuario As String
   Private _cargo_usuario As String
   Private _login_usuario As String
   Private _rml_numero_a As String
   Private _senha_usuario As String
   Private _email_usuario As String
   Private _email_supervisor As String
   Private _recebe_email As String
   Private _senha_web As String
   Private _recebe_relatorio As String
   Private _acesso_web As String
   Private _endereco As String
   Private _bairro As String
   Private _numero As String
   Private _complemento As String
   Private _matricula As String
   Private _cpf As String
   Private _id As String
   Private _cep As String
   Private _telefone As String
   Private _codigo_cidade As Integer
   Private _municipio As String
   Private _grp_codigo As String
   Private _expiracao_senha_web As String
   Private _bloqueio_web As String
   Private _dias_senha_expira As Integer
   Private _id_usuario_parent As Integer
   Private _codigo_uc As String
   Private _uf As String
    Private _cidade As String
    Private _recebe_celular As String
    Private _codigo_localidade As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer, ByVal pnome_usuario As String)
      _codigo = pcodigo
      _nome_usuario = pnome_usuario
   End Sub

    Public Sub New(ByVal pcodigo As Integer, ByVal pnome_usuario As String, ByVal pcargo_usuario As String, ByVal plogin_usuario As String, ByVal prml_numero_a As String, ByVal psenha_usuario As String, ByVal pemail_usuario As String, ByVal pemail_supervisor As String, ByVal precebe_email As String, ByVal psenha_web As String, ByVal precebe_relatorio As String, ByVal pacesso_web As String, ByVal pendereco As String, ByVal pbairro As String, ByVal pnumero As String, ByVal pcomplemento As String, ByVal pmatricula As String, ByVal pcpf As String, ByVal pid As String, ByVal pcep As String, ByVal ptelefone As String, ByVal pcodigo_cidade As Integer, ByVal pmunicipio As String, ByVal pgrp_codigo As String, ByVal pexpiracao_senha_web As String, ByVal pbloqueio_web As String, ByVal pdias_senha_expira As Integer, ByVal pid_usuario_parent As Integer, ByVal pcodigo_uc As String, ByVal puf As String, ByVal pcidade As String, ByVal precebe_celular As String, ByVal pcodigo_localidade As String)
        _codigo = pcodigo
        _nome_usuario = pnome_usuario
        _cargo_usuario = pcargo_usuario
        _login_usuario = plogin_usuario
        _rml_numero_a = prml_numero_a
        _senha_usuario = psenha_usuario
        _email_usuario = pemail_usuario
        _email_supervisor = pemail_supervisor
        _recebe_email = precebe_email
        _senha_web = psenha_web
        _recebe_relatorio = precebe_relatorio
        _acesso_web = pacesso_web
        _endereco = pendereco
        _bairro = pbairro
        _numero = pnumero
        _complemento = pcomplemento
        _matricula = pmatricula
        _cpf = pcpf
        _id = pid
        _cep = pcep
        _telefone = ptelefone
        _codigo_cidade = pcodigo_cidade
        _municipio = pmunicipio
        _grp_codigo = pgrp_codigo
        _expiracao_senha_web = pexpiracao_senha_web
        _bloqueio_web = pbloqueio_web
        _dias_senha_expira = pdias_senha_expira
        _id_usuario_parent = pid_usuario_parent
        _codigo_uc = pcodigo_uc
        _uf = puf
        _cidade = pcidade
        _recebe_celular = precebe_celular
        _codigo_localidade = pcodigo_localidade
    End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
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

   Public Property Cargo_Usuario() As String
      Get
         Return _cargo_usuario
      End Get
      Set(ByVal value As String)
         _cargo_usuario = value
      End Set
   End Property

   Public Property Login_Usuario() As String
      Get
         Return _login_usuario
      End Get
      Set(ByVal value As String)
         _login_usuario = value
      End Set
   End Property

   Public Property Rml_Numero_A() As String
      Get
         Return _rml_numero_a
      End Get
      Set(ByVal value As String)
         _rml_numero_a = value
      End Set
   End Property

   Public Property Senha_Usuario() As String
      Get
         Return _senha_usuario
      End Get
      Set(ByVal value As String)
         _senha_usuario = value
      End Set
   End Property

   Public Property Email_Usuario() As String
      Get
         Return _email_usuario
      End Get
      Set(ByVal value As String)
         _email_usuario = value
      End Set
   End Property

   Public Property Email_Supervisor() As String
      Get
         Return _email_supervisor
      End Get
      Set(ByVal value As String)
         _email_supervisor = value
      End Set
   End Property

   Public Property Recebe_Email() As String
      Get
         Return _recebe_email
      End Get
      Set(ByVal value As String)
         _recebe_email = value
      End Set
   End Property

   Public Property Senha_Web() As String
      Get
         Return _senha_web
      End Get
      Set(ByVal value As String)
         _senha_web = value
      End Set
   End Property

   Public Property Recebe_Relatorio() As String
      Get
         Return _recebe_relatorio
      End Get
      Set(ByVal value As String)
         _recebe_relatorio = value
      End Set
   End Property

   Public Property Acesso_Web() As String
      Get
         Return _acesso_web
      End Get
      Set(ByVal value As String)
         _acesso_web = value
      End Set
   End Property

   Public Property Endereco() As String
      Get
         Return _endereco
      End Get
      Set(ByVal value As String)
         _endereco = value
      End Set
   End Property

   Public Property Bairro() As String
      Get
         Return _bairro
      End Get
      Set(ByVal value As String)
         _bairro = value
      End Set
   End Property

   Public Property Numero() As String
      Get
         Return _numero
      End Get
      Set(ByVal value As String)
         _numero = value
      End Set
   End Property

   Public Property Complemento() As String
      Get
         Return _complemento
      End Get
      Set(ByVal value As String)
         _complemento = value
      End Set
   End Property

   Public Property Matricula() As String
      Get
         Return _matricula
      End Get
      Set(ByVal value As String)
         _matricula = value
      End Set
   End Property

   Public Property CPF() As String
      Get
         Return _cpf
      End Get
      Set(ByVal value As String)
         _cpf = value
      End Set
   End Property

   Public Property ID_usuario() As String
      Get
         Return _id
      End Get
      Set(ByVal value As String)
         _id = value
      End Set
   End Property

   Public Property CEP() As String
      Get
         Return _cep
      End Get
      Set(ByVal value As String)
         _cep = value
      End Set
   End Property

   Public Property Telefone() As String
      Get
         Return _telefone
      End Get
      Set(ByVal value As String)
         _telefone = value
      End Set
   End Property

   Public Property Codigo_Cidade() As Integer
      Get
         Return _codigo_cidade
      End Get
      Set(ByVal value As Integer)
         _codigo_cidade = value
      End Set
   End Property

   Public Property Municipio() As String
      Get
         Return _municipio
      End Get
      Set(ByVal value As String)
         _municipio = value
      End Set
   End Property

   Public Property GRP_Codigo() As String
      Get
         Return _grp_codigo
      End Get
      Set(ByVal value As String)
         _grp_codigo = value
      End Set
   End Property

   Public Property Expiracao_Senha_Web() As String
      Get
         Return _expiracao_senha_web
      End Get
      Set(ByVal value As String)
         _expiracao_senha_web = value
      End Set
   End Property

   Public Property Bloqueio_Web() As String
      Get
         Return _bloqueio_web
      End Get
      Set(ByVal value As String)
         _bloqueio_web = value
      End Set
   End Property

   Public Property Dias_Senha_Expira() As Integer
      Get
         Return _dias_senha_expira
      End Get
      Set(ByVal value As Integer)
         _dias_senha_expira = value
      End Set
   End Property

   Public Property ID_Usuario_Parent() As Integer
      Get
         Return _id_usuario_parent
      End Get
      Set(ByVal value As Integer)
         _id_usuario_parent = value
      End Set
   End Property

   Public Property Codigo_UC() As String
      Get
         Return _codigo_uc
      End Get
      Set(ByVal value As String)
         _codigo_uc = value
      End Set
   End Property

   Public Property Requerente() As String
      Get
         Return _nome_usuario
      End Get
      Set(ByVal value As String)
      End Set
   End Property

   Public Property uf() As String
      Get
         Return _uf
      End Get
      Set(ByVal value As String)
         _uf = value
      End Set
   End Property

   Public Property Cidade() As String
      Get
         Return _cidade
      End Get
        Set(ByVal value As String)
            _cidade = value
        End Set
    End Property

    Public Property RecebeCelular() As String
        Get
            Return _recebe_celular
        End Get
        Set(ByVal value As String)
            _recebe_celular = value
        End Set
    End Property

    Public Property CodigoLocalidade() As String
        Get
            Return _codigo_localidade
        End Get
        Set(ByVal value As String)
            _codigo_localidade = value
        End Set
    End Property

    Private _matricula_sup As String

    Public Property Matricula_sup() As String
        Get
            Return _matricula_sup
        End Get
        Set(ByVal value As String)
            _matricula_sup = value
        End Set
    End Property

    Private _VICE As String

    Public Property VICE() As String
        Get
            Return _VICE
        End Get
        Set(ByVal value As String)
            _VICE = value
        End Set
    End Property

    Private _DIR As String

    Public Property DIR() As String
        Get
            Return _DIR
        End Get
        Set(ByVal value As String)
            _DIR = value
        End Set
    End Property

    Private _SUPTE As String

    Public Property SUPTE() As String
        Get
            Return _SUPTE
        End Get
        Set(ByVal value As String)
            _SUPTE = value
        End Set
    End Property

    Private _GER As String

    Public Property GER() As String
        Get
            Return _GER
        End Get
        Set(ByVal value As String)
            _GER = value
        End Set
    End Property

    Private _SEC As String

    Public Property SEC() As String
        Get
            Return _SEC
        End Get
        Set(ByVal value As String)
            _SEC = value
        End Set
    End Property

    Private _NUC As String

    Public Property NUC() As String
        Get
            Return _NUC
        End Get
        Set(ByVal value As String)
            _NUC = value
        End Set
    End Property

    Private _DATA_DEMISSAO As String

    Public Property DATA_DEMISSAO() As String
        Get
            Return _DATA_DEMISSAO
        End Get
        Set(ByVal value As String)
            _DATA_DEMISSAO = value
        End Set
    End Property

    Private _DATA_ADMISSAO As String

    Public Property DATA_ADMISSAO() As String
        Get
            Return _DATA_ADMISSAO
        End Get
        Set(ByVal value As String)
            _DATA_ADMISSAO = value
        End Set
    End Property

    Private _status As String

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property
End Class
