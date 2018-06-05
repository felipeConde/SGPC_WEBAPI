Imports Microsoft.VisualBasic

Public Class AppSolicitacao

   Private _codigo As Integer
   Private _data_solicitacao As String
   Private _dias_aditivo As Integer
   Private _minutos_extra As Double
   Private _protocolo As String
   Private _atendente As String
   Private _descricao As String
   Private _prazo As String
   Private _observacao As String
   Private _data_fechamento As String

   Private _requerente_codigo As Integer
   Private _item_codigo As Integer
   Private _servicos_codigo As Integer
   Private _situacao_codigo As Integer

   Private _relacional_codigo As String

   Private _requerente As String
   Private _item_sgpc As String
   Private _servico As String
   Private _situacao As String
   Private _situacao_descricao As String

   Private _relacional As String

   Private _solicitacao As String
   Private _operadora As String
   Private _usuario As String
   Private _perfil As String
   Private _unidade As String
   Private _setor As String
   Private _fim_ciclo As String
   Private _venc_conta As String
   Private _grupo As String
   Private _ccusto As String
'relatorio painel
   Private _cidade As String
   Private _totalminutos As Integer
   Private _aditivo As Double
   Private _mesano As String
   Private _extra As Double
   Private _percmes As Double
   Private _maiordez As String

   Public Sub New()
   End Sub

   Public Sub New(ByVal pcodigo As Integer)
      _codigo = pcodigo
   End Sub

   Public Property Codigo() As Integer
      Get
         Return _codigo
      End Get
      Set(ByVal value As Integer)
         _codigo = value
      End Set
   End Property

   Public Property DataSolicitacao() As String
      Get
         Return _data_solicitacao
      End Get
      Set(ByVal value As String)
         _data_solicitacao = value
      End Set
   End Property

   Public Property DiasAditivo() As Integer
      Get
         Return _dias_aditivo
      End Get
      Set(ByVal value As Integer)
         If value = vbNull Then
            value = 0
         End If
         _dias_aditivo = value
      End Set
   End Property

   Public Property MinutosExtras() As Double
      Get
         Return _minutos_extra
      End Get
      Set(ByVal value As Double)
         If value = vbNull Then
            value = 0.0
         End If
         _minutos_extra = value
      End Set
   End Property

   Public Property Protocolo() As String
      Get
         Return _protocolo
      End Get
      Set(ByVal value As String)
         _protocolo = value
      End Set
   End Property

   Public Property Atendente() As String
      Get
         Return _atendente.ToUpper
      End Get
      Set(ByVal value As String)
         _atendente = value.ToUpper
      End Set
   End Property

   Public Property Descricao() As String
      Get
         Return _descricao.ToUpper
      End Get
      Set(ByVal value As String)
         _descricao = value.ToUpper
      End Set
   End Property

   Public Property Prazo() As String
      Get
         Return _prazo
      End Get
      Set(ByVal value As String)
         _prazo = value
      End Set
   End Property

   Public Property Observacao() As String
      Get
         Return _observacao.ToUpper
      End Get
      Set(ByVal value As String)
         _observacao = value.ToUpper
      End Set
   End Property

   Public Property DataFechamento() As String
      Get
         Return _data_fechamento
      End Get
      Set(ByVal value As String)
         _data_fechamento = value
      End Set
   End Property

   Public property RequerenteCodigo as integer
      Get
         Return _requerente_codigo
      End Get
      Set(ByVal value As Integer)
         _requerente_codigo = value
      End Set
   End Property

   Public Property ItemCodigo As Integer
      Get
         Return _item_codigo
      End Get
      Set(ByVal value As Integer)
         _item_codigo = value
      End Set
   End Property

   Public Property ServicoCodigo() As Integer
      Get
         Return _servicos_codigo
      End Get
      Set(ByVal value As Integer)
         _servicos_codigo = value
      End Set
   End Property

   Public Property SituacaoCodigo() As Integer
      Get
         Return _situacao_codigo
      End Get
      Set(ByVal value As Integer)
         _situacao_codigo = value
      End Set
   End Property


   Public Property RelacionalCodigo() As String
      Get
         Return _relacional_codigo
      End Get
      Set(ByVal value As String)
         _relacional_codigo = value
      End Set
   End Property


   Public Property Requerente() As String
      Get
         Return _requerente.ToUpper
      End Get
      Set(ByVal value As String)
         _requerente = value.ToUpper
      End Set
   End Property

   Public Property ItemSGPC() As String
      Get
         Return _item_sgpc.ToUpper
      End Get
      Set(ByVal value As String)
         _item_sgpc = value.ToUpper
      End Set
   End Property

   Public Property Servico() As String
      Get
         Return _servico.ToUpper
      End Get
      Set(ByVal value As String)
         _servico = value.ToUpper
      End Set
   End Property

   Public Property Situacao() As String
      Get
         Return _situacao.ToUpper
      End Get
      Set(ByVal value As String)
         _situacao = value.ToUpper
      End Set
   End Property

   Public Property SituacaoDescricao() As String
      Get
         Return _situacao_descricao.ToUpper
      End Get
      Set(ByVal value As String)
         _situacao_descricao = value.ToUpper
      End Set
   End Property

   Public Property Relacional() As String
      Get
         Return _relacional.ToUpper
      End Get
      Set(ByVal value As String)
         _relacional = value.ToUpper
      End Set
   End Property

   Public Property Solicitacao() As String
      Get
         Return _solicitacao.ToUpper
      End Get
      Set(ByVal value As String)
         _solicitacao = value.ToUpper
      End Set
   End Property

   Public Property Operadora() As String
      Get
         Return _operadora.ToUpper
      End Get
      Set(ByVal value As String)
         _operadora = value.ToUpper
      End Set
   End Property

   Public Property Usuario() As String
      Get
         Return _usuario.ToUpper
      End Get
      Set(ByVal value As String)
         _usuario = value.ToUpper
      End Set
   End Property

   Public Property Perfil() As String
      Get
         Return _perfil.ToUpper
      End Get
      Set(ByVal value As String)
         _perfil = value.ToUpper
      End Set
   End Property

   Public Property Unidade() As String
      Get
         Return _unidade.ToUpper
      End Get
      Set(ByVal value As String)
         _unidade = value.ToUpper
      End Set
   End Property

   Public Property Setor() As String
      Get
         Return _setor.ToUpper
      End Get
      Set(ByVal value As String)
         _setor = value.ToUpper
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

   Public Property Grupo() As String
      Get
         Return _grupo
      End Get
      Set(ByVal value As String)
         _grupo = value
      End Set
   End Property

   Public Property CCusto() As String
      Get
         Return _ccusto
      End Get
      Set(ByVal value As String)
         _ccusto = value
      End Set
   End Property

'relatorio painel
   Public Property Cidade() As String
      Get
         Return _cidade
      End Get
      Set(ByVal value As String)
         _cidade = value
      End Set
   End Property

   Public Property TotalMinutos() As Integer
      Get
         Return _totalminutos
      End Get
      Set(ByVal value As Integer)
         _totalminutos = value
      End Set
   End Property

   Public Property Aditivo() As Double
      Get
         Return _aditivo
      End Get
      Set(value As Double)
         _aditivo = value
      End Set
   End Property

   Public Property MesAno() As String
      Get
         Return _mesano
      End Get
      Set(ByVal value As String)
         _mesano = value
      End Set
   End Property

   Public Property Extra() As Double
      Get
         Return _extra
      End Get
      Set(value As Double)
         _extra = value
      End Set
   End Property

   Public Property PercMes() As Double
      Get
         Return _percmes
      End Get
      Set(value As Double)
         _percmes = value
      End Set
   End Property

   Public Property MaiorDez() As String
      Get
         Return _maiordez
      End Get
      Set(ByVal value As String)
         _maiordez = value
      End Set
   End Property

End Class
