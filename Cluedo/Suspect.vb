Public Class Suspect

    Inherits Card

    Private _startposition As Char
    Private _colour As ConsoleColor
    Private _pieceSymbol As Char
    Private _player As Player

    Public Sub New(name As String, startPosition As Char, colour As ConsoleColor)

        MyBase.New(name)

        _startposition = startPosition
        _colour = colour
        _pieceSymbol = "⬤"

    End Sub

    'assigns a player to the suspect (so the player picece number thing works)
    Public Sub SetPlayer(player As Player)

        _player = player

    End Sub

    'returns the players start position
    Public Function GetStartPosition() As Char

        Return _startposition

    End Function

    'returns the players pieces colour
    Public Function GetColour() As ConsoleColor

        Return _colour

    End Function

    'returns the players pieces symbol
    Public Function GetPieceSymbol() As Char

        If _player Is Nothing Then

            Return _pieceSymbol

        Else

            Return _player.GetSymbol

        End If

    End Function

End Class
