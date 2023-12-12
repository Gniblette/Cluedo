Imports System.Console
Public Class Menu

    Private _options As List(Of String)
    Private _choice As Integer
    Private _message As String

    Public Sub New(options As List(Of String), message As String)

        _options = options
        _choice = 0
        _message = message

    End Sub

    'displays the menu options
    Private Sub DisplayOptions()

        Clear()
        WriteLine(_message)

        'outputs each option onto the screen
        For i = 0 To _options.Count - 1

            If i <> _choice Then

                WriteLine(_options(i))

            Else

                Highlight()

                WriteLine(_options(i))

                StopHighlight()

            End If

        Next

    End Sub

    'gets the users choice from the displayed options
    Public Function ProcessInputs() As Integer

        Do

            Dim input As ConsoleKeyInfo
            DisplayOptions()

            input = ReadKey()

            If input.Key = ConsoleKey.Enter Then

                Return _choice

            ElseIf input.Key = ConsoleKey.UpArrow Then

                MoveUp()

            ElseIf input.Key = ConsoleKey.DownArrow Then

                MoveDown()

            End If

        Loop

    End Function

    'moves up a position in the menu 
    Private Sub MoveUp()

        If _choice = 0 Then

            _choice = _options.Count - 1

        Else

            _choice = _choice - 1

        End If

    End Sub

    'moves down a position in the menu
    Private Sub MoveDown()

        If _choice = _options.Count - 1 Then

            _choice = 0

        Else

            _choice = _choice + 1

        End If

    End Sub

    'highlights the option the user is on in white
    Private Sub Highlight()

        BackgroundColor = ConsoleColor.White
        ForegroundColor = ConsoleColor.Black

    End Sub

    'stops highlighting the options the user isnt on
    Private Sub StopHighlight()

        BackgroundColor = ConsoleColor.Black
        ForegroundColor = ConsoleColor.White

    End Sub

End Class
