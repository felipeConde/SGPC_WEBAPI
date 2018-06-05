Imports Microsoft.VisualBasic

Public Class Uteis

    Public Shared Function FormataDuracao(ByVal diferenca As String) As String

        Dim Tempo As Double
        Dim Segundos As Double
        Dim Minutos As Double
        Dim Horas As Double

        Tempo = (((CDbl(diferenca) * (1440)) * 60))
        If Int(Tempo) <> Tempo Then
            Tempo = Int(Tempo) + 1
        End If
        Tempo = diferenca
        Minutos = Int(Tempo / 60)
        Segundos = CDbl(0)
        Segundos = CDbl(Tempo Mod 60)
        Horas = Int(Minutos / 60)
        Minutos = Minutos Mod 60

        Return FormataTempo(FormatNumber(Horas, 0)) & ":" & FormataTempo(FormatNumber(Minutos, 0)) & ":" & FormataTempo(FormatNumber(Segundos, 0))

    End Function

    Private Shared Function FormataTempo(ByVal pTempo As String) As String

        If pTempo.Length < 2 Then
            pTempo = "0" & pTempo
        End If
        'Return Convert.ToInt16(pTempo)
        Return Replace(pTempo, ".", "")
        'Return pTempo
    End Function

    Public Shared Function NormalizaDuracao(ByRef pDur As String) As Double
        'pDur = pDur / 1000
        pDur = pDur.Replace(",", ".")

        Return pDur
    End Function

    'Função que retorna o ultimo dia do mes
    Public Shared Function Func_Ultimo_Dia_Mes(ByVal paramDataX As Date) As Date
        Func_Ultimo_Dia_Mes = DateAdd("m", 1, DateSerial(Year(paramDataX), Month(paramDataX), 1))
        Func_Ultimo_Dia_Mes = DateAdd("d", -1, Func_Ultimo_Dia_Mes)
    End Function


End Class
