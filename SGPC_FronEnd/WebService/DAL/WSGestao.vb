Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Configuration
Imports System
Imports System.Collections.Generic

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WSGestao
    Inherits System.Web.Services.WebService
    Private dao As New GestaoDAL
    Private daoFixo As New GestaoDALFixo
    Private tarefas As List(Of Tarefa)
    Private _executando As Boolean = False

    Public ReadOnly Property Executando As Boolean
        Get
            Return _executando
        End Get

    End Property

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function Executa() As String

        Dim conexao As String = ConfigurationManager.ConnectionStrings("ConnectionString").ToString
        Dim _auditChamadasZeradas As String = 0
        Try
            _auditChamadasZeradas = ConfigurationManager.ConnectionStrings("auditChamadasZeradas").ToString
        Catch ex As Exception

        End Try

        Dim log As IO.StreamWriter


        'conexao = "Provider=OraOLEDB.Oracle;Password=sgpcnovo;User ID=sgpcnovo;Data Source=server;"
        dao.StrConn = conexao
        daoFixo.StrConn = conexao
        'verifica se existe alguma tarefa agendada na tabela gestao_agendamentos_tarefas
        'tarefas.Clear()
        tarefas = dao.getTarefas()

        If tarefas.Count > 0 And _executando = False Then
            Try
                _executando = True
                'verifica se tem faturas especificas para fazer auditoria
                'For Each _tarefa As Tarefa In tarefas
                '    _tarefa.Faturas = dao.getTarefasFaturas(_tarefa.Codigo)
                'Next

                'vamos fazer as atualizações
                For Each _tarefa As Tarefa In tarefas
                    _tarefa.Faturas = dao.getTarefasFaturas(_tarefa.Codigo)
                    'ATUALIZA O STATUS DA TAREFA PARA PENDENTE
                    dao.AtualizaStatusAgendamento(_tarefa, 1, True)
                    '_executando = True

                    If _tarefa.Faturas.Count > 0 Then
                        'log.WriteLine("Tem faturas para executar. Total de faturas:" & _tarefa.Faturas.Count)
                        'faz somente destas faturas
                        If _tarefa.Codtarefa = 1 Then
                            'percorre as faturas
                            For Each _fatura As Fatura In _tarefa.Faturas
                                ExecutaAuditoria(conexao, _fatura, _auditChamadasZeradas, _tarefa)
                            Next
                            'deletar faturas
                        ElseIf _tarefa.Codtarefa = 2 Then
                            For Each _fatura As Fatura In _tarefa.Faturas
                                ApagarFatura(_fatura, _tarefa)
                            Next
                        ElseIf _tarefa.Codtarefa = 3 Then
                            For Each _fatura As Fatura In _tarefa.Faturas
                                AtualizarDebito(_fatura, _tarefa)
                            Next
                        End If
                    Else
                        'faz de todas as faturas
                    End If
                    'atualiza a view materializada
                    dao.AtualizaRelatorios()
                    'ATUALIZA O STATUS DA TAREFA PARA CONCLUIDO
                    dao.AtualizaStatusAgendamento(_tarefa, 2, False)
                    ' _executando = False
                Next
                _executando = False
            Catch ex As Exception

                'Dim log As IO.StreamWriter
                Dim caminhoLog As String = AppDomain.CurrentDomain.BaseDirectory + "log.txt"
                If Not IO.File.Exists(caminhoLog) Then
                    log = IO.File.CreateText(caminhoLog)
                    log.WriteLine(Date.Now + " Log de monitoramento de congelamentos:")
                Else
                    log = New IO.StreamWriter(caminhoLog, True, System.Text.Encoding.UTF8)
                End If
                log.WriteLine(Date.Now + "Erro:'" + ex.Message)
                _executando = False
            End Try
        End If

        'log.Close()
        'log.Dispose()
        Return "Hello World"
    End Function


    Public Function ApagarFatura(ByVal _fatura As Fatura, ByVal _tarefa As Tarefa) As String
        Dim MSG As String = ""
        dao.InsereLOG(_fatura, _tarefa, "INICIANDO APAGAR FATURA", "")

        If ConfigurationManager.AppSettings("FaturasLOG") = "1" Then
            dao.InsereLOG(_fatura, _tarefa, "VAI COLOCAR NO LOG DE FATURAS", "")
            dao.FaturaLOG(_fatura, _tarefa)
        End If
        MSG = dao.ApagarFatura(_fatura.ID)
        If MSG.ToUpper = "OK" Then
            dao.InsereLOG(_fatura, _tarefa, "FIM APAGAR FATURA", "")
        Else
            dao.InsereLOG(_fatura, _tarefa, "ERRO APAGAR FATURA:[" & MSG & "]", "S")
        End If

        Return "OK"

    End Function

    Public Function AtualizarDebito(ByVal _fatura As Fatura, ByVal _tarefa As Tarefa) As String
        Dim MSG As String = ""
        dao.InsereLOG(_fatura, _tarefa, "INICIANDO ATUALIZAR C.CUSTO E USUÁRIOS", "")
        MSG = dao.AtualizaDebito(_fatura.ID)
        If MSG.ToUpper = "OK" Then
            dao.InsereLOG(_fatura, _tarefa, "FIM ATUALIZAR C.CUSTO E USUÁRIOS", "")
        Else
            dao.InsereLOG(_fatura, _tarefa, "ERRO ATUALIZAR C.CUSTO E USUÁRIOS:[" & MSG & "]", "S")
        End If

        Return "OK"

    End Function



    <WebMethod()> _
    Public Function ExecutaAuditoria(ByVal pConexao As String, ByVal _fatura As Fatura, ByVal auditChamadasZeradas As String, ByVal _tarefa As Tarefa) As String
        Dim MSG As String = ""
        Dim TipoTarifacao0800 As Integer
        Dim AplicaImpostoMovel As String = ConfigurationManager.AppSettings("AplicaImpostoMovel")
        Dim AplicaImpostoFixo As String = ConfigurationManager.AppSettings("AplicaImpostoFixo")
        Dim AplicaImposto0800 As String = ConfigurationManager.AppSettings("AplicaImposto0800")


        Dim log As IO.StreamWriter
        'Dim caminhoLog As String = Application.StartupPath & ConfigurationManager.AppSettings("nomeArquivo").ToString
        Dim caminhoLog As String = AppDomain.CurrentDomain.BaseDirectory + "log.txt"
        If Not IO.File.Exists(caminhoLog) Then
            log = IO.File.CreateText(caminhoLog)
            log.WriteLine(Date.Now + " Log de monitoramento de congelamentos:")
        Else
            log = New IO.StreamWriter(caminhoLog, True, System.Text.Encoding.UTF8)
        End If

        log.WriteLine(Date.Now + "Tem fatura para auditar")

        Try
            TipoTarifacao0800 = ConfigurationManager.AppSettings("TipoTarifacao0800")
        Catch ex As Exception
            TipoTarifacao0800 = 1
        End Try
        'define a conexao com o BD

        dao.InsereLOG(_fatura, _tarefa, "** COMEÇO DA AUDITORIA **", "")

        If _fatura.CodigoTipo = "1" Then

            Try
                'fatura de móvel
                dao.InsereLOG(_fatura, _tarefa, "PREPARANDO AUDITORIA DE CHAMADAS", "")
                MSG = dao.ZeraAuditoria(_fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM PREPARANDO DE CHAMADAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO PREPARANDO DE CHAMADAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'REPARANDO AUDITORIA DE CHAMADAS'" + ex.Message)

            End Try

            Try
                'Classificação das chamadas
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO CLASSIFICAÇÃO-TARIFAS", "")
                MSG = dao.UpdateClassificacao(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM CLASSIFICAÇÃO-TARIFAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO CLASSIFICAÇÃO-TARIFAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO CLASSIFICAÇÃO-TARIFAS'" + ex.Message)
            End Try

            Try
                'classificação dos SERVIÇOS
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO CLASSIFICAÇÃO-SERVIÇOS", "")
                MSG = dao.UpdateClassificacaoServicos(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM CLASSIFICAÇÃO-SERVIÇOS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO CLASSIFICAÇÃO-SERVIÇOS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO CLASSIFICAÇÃO-SERVIÇOS'" + ex.Message)
            End Try

            Try

                'audita os serviços
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA DOS SERVIÇOS", "")
                MSG = dao.UpdateAudit(_fatura.CodigoOperadora, _fatura.CodigoConta, 1)
                MSG = dao.UpdateAudit(_fatura.CodigoOperadora, _fatura.CodigoConta, 2)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM AUDITORIA DOS SERVIÇOS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO AUDITORIA DOS SERVIÇOS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA DOS SERVIÇOS'" + ex.Message)

            End Try

            Try
                'verifica VC2 e VC3
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO VERIFICAÇÃO DE VC2 INDEVIDOS", "")
                MSG = dao.GestaoVerificaVC2(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM VERIFICAÇÃO DE VC2 INDEVIDOS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO VERIFICAÇÃO DE VC2 INDEVIDOS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO VERIFICAÇÃO DE VC2 INDEVIDOS'" + ex.Message)

            End Try

            Try
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO VERIFICAÇÃO DE VC3 INDEVIDOS", "")
                MSG = dao.GestaoVerificaVC3(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM VERIFICAÇÃO DE VC3 INDEVIDOS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO VERIFICAÇÃO DE VC3 INDEVIDOS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO VERIFICAÇÃO DE VC3 INDEVIDOS'" + ex.Message)

            End Try

            Try

                'audita as chamadas

                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA DE CHAMADAS", "")
                MSG = dao.TarifaGestao(_fatura.CodigoOperadora, _fatura.CodigoConta, 1)
                MSG = dao.TarifaGestao(_fatura.CodigoOperadora, _fatura.CodigoConta, 2)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM AUDITORIA DE CHAMADAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO AUDITORIA DE CHAMADAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA DE CHAMADAS'" + ex.Message)

            End Try

            Try
                'audita DDI
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA DE CHAMADAS DDI", "")
                MSG = dao.TarifaGestaoDDI(_fatura.CodigoOperadora, _fatura.CodigoConta, 1)
                MSG = dao.TarifaGestaoDDI(_fatura.CodigoOperadora, _fatura.CodigoConta, 2)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA DE CHAMADAS DDI", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA DE CHAMADAS DDI:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA DE CHAMADAS DDI'" + ex.Message)

            End Try

            Try

                'Aplicamos o imposto sobre a tarifa cadastrada sem imposto
                If AplicaImpostoMovel = "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INCIANDO - APLICAR IMPOSTO NA FATURA", "")
                    dao.AplicaImpostoValor(_fatura.CodigoConta)
                    dao.InsereLOG(_fatura, _tarefa, "FIM - APLICAR IMPOSTO NA FATURA", "")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INCIANDO - APLICAR IMPOSTO NA FATURA'" + ex.Message)

            End Try

            Try
                'intragrupo
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA DE INTRAGRUPO", "")
                MSG = dao.ProcessaIntragrupo(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA DE INTRAGRUPO", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA DE INTRAGRUPO:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA DE INTRAGRUPO'" + ex.Message)

            End Try

            Try
                'franquias
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA DE FRANQUIA", "")
                MSG = dao.ProcessaAuditFranquias(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA DE FRANQUIA", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA DE FRANQUIA:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA DE FRANQUIA'" + ex.Message)

            End Try

            Try
                'auditar chamadas zeradas
                If auditChamadasZeradas <> "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INICIANDO ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS", "")
                    MSG = dao.UpdateChamadasZeradas(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                    If MSG.ToUpper = "OK" Then
                        dao.InsereLOG(_fatura, _tarefa, "FIM  ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS", "")
                    Else
                        dao.InsereLOG(_fatura, _tarefa, "ERRO  ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS:[" & MSG & "]", "S")
                    End If
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS'" + ex.Message)

            End Try
        End If

        If _fatura.CodigoTipo = "2" Then

            Try

                'Fixo Externo

                'classificação das chamadas
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO CLASSIFICAÇÃO-CHAMADAS", "")
                MSG = daoFixo.ClassificaLigacaoFixo(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  CLASSIFICAÇÃO-CHAMADAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  CLASSIFICAÇÃO-CHAMADAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO CLASSIFICAÇÃO-CHAMADAS'" + ex.Message)

            End Try

            Try
                'Tarifas as chamadas
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO AUDITORIA-CHAMADAS", "")
                MSG = daoFixo.TarifaLigacaoFixoV3(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA-CHAMADAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA-CHAMADAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO AUDITORIA-CHAMADAS'" + ex.Message)

            End Try

            Try

                'zera serviços não contratados
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO VERIFICAÇÃO DE SERVIÇOS NÃO CONTRATADOS", "")
                MSG = daoFixo.AtualizaValorAuditServicos(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  VERIFICAÇÃO DE SERVIÇOS NÃO CONTRATADOS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  VERIFICAÇÃO DE SERVIÇOS NÃO CONTRATADOS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO VERIFICAÇÃO DE SERVIÇOS NÃO CONTRATADOS'" + ex.Message)

            End Try

            Try

                'Aplicamos o imposto sobre a tarifa cadastrada sem imposto
                If AplicaImpostoFixo = "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INCIANDO - APLICAR IMPOSTO NA FATURA", "")
                    dao.AplicaImpostoValor(_fatura.CodigoConta)
                    dao.InsereLOG(_fatura, _tarefa, "FIM - APLICAR IMPOSTO NA FATURA", "")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INCIANDO - APLICAR IMPOSTO NA FATURA'" + ex.Message)

            End Try

            Try
                'verifica as franquias
                dao.InsereLOG(_fatura, _tarefa, "INICIANDO VERIFICAÇÃO DE FRANQUIAS", "")
                MSG = daoFixo.ProcessaAuditFranquiasFixo(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  VERIFICAÇÃO DE FRANQUIAS", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  VERIFICAÇÃO DE FRANQUIAS:[" & MSG & "]", "S")
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO VERIFICAÇÃO DE FRANQUIAS'" + ex.Message)

            End Try

            Try
                'audita chamadas zeradas
                If auditChamadasZeradas <> "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INICIANDO ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS", "")
                    MSG = daoFixo.UpdateChamadasZeradas(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                    If MSG.ToUpper = "OK" Then
                        dao.InsereLOG(_fatura, _tarefa, "FIM  ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS", "")
                    Else
                        dao.InsereLOG(_fatura, _tarefa, "ERRO  ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS:[" & MSG & "]", "S")
                    End If
                End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIANDO ZERANDO VALOR AUDITADO DE CHAMADAS ZERADAS'" + ex.Message)

            End Try

        End If
        If _fatura.CodigoTipo = 4 Then
            Try


                'auditoria de 0800
                'pega as chamadas da conta
                dao.InsereLOG(_fatura, _tarefa, "INICIO CLASSIFICAÇÃO 0800", "")
                MSG = daoFixo.Classifica_0800(_fatura.CodigoConta, _fatura.CodigoOperadora, TipoTarifacao0800)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  CLASSIFICAÇÃO 0800", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  CLASSIFICAÇÃO 0800:[" & MSG & "]", "S")
                End If


            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIO CLASSIFICAÇÃO 0800'" + ex.Message)

            End Try

            Try

                'tarifando as chamadas
                dao.InsereLOG(_fatura, _tarefa, "INICIO AUDITORIA 0800", "")
                MSG = daoFixo.TarifaLigacaoFixoV30800(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA 0800", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA 0800:[" & MSG & "]", "S")
                End If
                'audita os serviços
                daoFixo.AtualizaValorAuditServicos(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                ' If ConfigurationManager.AppSettings("AuditarChamadasZeradas").ToString <> "1" Then
                ' daoFixo.UpdateChamadasZeradas(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                ' End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIO AUDITORIA 0800'" + ex.Message)

            End Try

            Try

                'Aplicamos o imposto sobre a tarifa cadastrada sem imposto
                If AplicaImposto0800 = "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INCIANDO - APLICAR IMPOSTO NA FATURA", "")
                    dao.AplicaImpostoValor(_fatura.CodigoConta)
                    dao.InsereLOG(_fatura, _tarefa, "FIM - APLICAR IMPOSTO NA FATURA", "")
                End If
            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INCIANDO - APLICAR IMPOSTO NA FATURA'" + ex.Message)

            End Try


        End If


        'AUDITORIA DE NÚMERO ÚNICO
        If _fatura.CodigoTipo = "6" Then

            Try


                'auditoria de 0800
                'pega as chamadas da conta
                dao.InsereLOG(_fatura, _tarefa, "INICIO CLASSIFICAÇÃO NÚMERO ÚNICO", "")
                MSG = daoFixo.Classifica_3003(_fatura.CodigoConta, _fatura.CodigoOperadora, TipoTarifacao0800)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  CLASSIFICAÇÃO NÚMERO ÚNICO", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  CLASSIFICAÇÃO NÚMERO ÚNICO:[" & MSG & "]", "S")
                End If


            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIO CLASSIFICAÇÃO NÚMERO ÚNICO'" + ex.Message)

            End Try

            Try

                'tarifando as chamadas
                dao.InsereLOG(_fatura, _tarefa, "INICIO AUDITORIA NÚMERO ÚNICO", "")
                MSG = daoFixo.TarifaLigacaoFixo3003(_fatura.CodigoOperadora, _fatura.CodigoConta)
                If MSG.ToUpper = "OK" Then
                    dao.InsereLOG(_fatura, _tarefa, "FIM  AUDITORIA NÚMERO ÚNICO", "")
                Else
                    dao.InsereLOG(_fatura, _tarefa, "ERRO  AUDITORIA NÚMERO ÚNICO:[" & MSG & "]", "S")
                End If
                'audita os serviços
                daoFixo.AtualizaValorAuditServicos(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                ' If ConfigurationManager.AppSettings("AuditarChamadasZeradas").ToString <> "1" Then
                ' daoFixo.UpdateChamadasZeradas(_fatura.CodigoOperadora, _fatura.CodigoConta, "")
                ' End If

            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INICIO AUDITORIA NÚMERO ÚNICO'" + ex.Message)

            End Try

            Try

                'Aplicamos o imposto sobre a tarifa cadastrada sem imposto
                If AplicaImposto0800 = "1" Then
                    dao.InsereLOG(_fatura, _tarefa, "INCIANDO - APLICAR IMPOSTO NA FATURA", "")
                    dao.AplicaImpostoValor(_fatura.CodigoConta)
                    dao.InsereLOG(_fatura, _tarefa, "FIM - APLICAR IMPOSTO NA FATURA", "")
                End If
            Catch ex As Exception

                log.WriteLine(Date.Now + "Erro em 'INCIANDO - APLICAR IMPOSTO NA FATURA'" + ex.Message)

            End Try

        End If

        dao.InsereLOG(_fatura, _tarefa, "** TERMINO DA AUDITORIA **", "")

        log.Close()
        Return "OK"

        'Return "Hello World"
    End Function

End Class
