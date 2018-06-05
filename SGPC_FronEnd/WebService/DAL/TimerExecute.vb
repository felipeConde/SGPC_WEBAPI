Imports Microsoft.VisualBasic
Imports System.Timers
Imports System.Collections.Generic
Imports System

Public Class TimerExecute

    Shared _timer As Timer
    Shared _list As List(Of String) = New List(Of String)
    Shared _webService As New WSGestao

    ''' <summary>
    ''' Start the timer.
    ''' </summary>
    Shared Sub Start()
        _timer = New Timer(15000)
        AddHandler _timer.Elapsed, New ElapsedEventHandler(AddressOf Handler)
        _timer.Enabled = True
        _timer.Start()
    End Sub


    ''' <summary>
    ''' Timer event handler.
    ''' </summary>
    Shared Sub Handler(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        '_list.Add(DateTime.Now.ToString())
        'If _webService.Executando = False Then
        _webService.Executa()
        'End If

    End Sub

    Shared Sub EndProcess()
        Try
            _timer.Stop()
            _timer.Dispose()
            _timer = Nothing
        Catch ex As Exception

        End Try

    End Sub


End Class
