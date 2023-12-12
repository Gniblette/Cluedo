Imports System.Console
Public Class SuspectLoader

    'loads the suspects from a file
    Public Function LoadSuspects(fileName As String) As List(Of Suspect)

        Dim lines As List(Of String())
        Dim reader As CSVLoader = New CSVLoader
        Dim suspect As Suspect
        Dim suspects As New List(Of Suspect)
        Dim colour As Integer

        lines = reader.GetEntries(fileName)

        If lines.Count = 0 Then

            Return Nothing

        End If

        For i = 1 To lines.Count - 1

            'makes sure that the number for the colours is a valid console colour
            If Int32.TryParse(lines(i)(2), colour) = True And colour >= 0 And colour <= 15 Then

                suspect = New Suspect(lines(i)(0), lines(i)(1), lines(i)(2))
                suspects.Add(suspect)

            Else

                WriteLine("error in file processing")
                Return Nothing

            End If

        Next

        Return suspects

    End Function

End Class
