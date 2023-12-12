Imports System.Console
Public Class Square

    Protected _symbol As Char
    Protected _canLandOn As Boolean
    Protected _overlay As Boolean
    Protected _door As Boolean
    Protected _overlayChar As Char
    Protected _character As Suspect
    Protected _player As Player
    Protected _X As Integer
    Protected _Y As Integer

    Public Sub New(symbol As Char)

        _symbol = symbol

    End Sub

    'returns the symbol of the card
    Public Function GetSymbol() As Char

        Return _symbol

    End Function

    'returns if the player can move their piece onto that square
    Public Function GetCanlandOn() As Boolean

        Return _canLandOn

    End Function

    'returns if the square is a door
    Public Function GetDoor() As Boolean

        Return _door

    End Function

    'sets the room
    Public Overridable Sub SetRoom(rooms As List(Of Room))

    End Sub

    'returns the room
    Public Overridable Function GetRoom() As Room

        Return Nothing

    End Function

    'sets the square coordinates
    Public Sub SetPosition(x As Integer, y As Integer)

        _X = x
        _Y = y

    End Sub

    'returns the x position of the square
    Public Function GetX() As Integer

        Return _X

    End Function

    'returns the y position of the square
    Public Function GetY() As Integer

        Return _Y

    End Function

    'returns the symbol that should be displayed in the baord for that square
    Public Function GetDisplayCharacter() As Char

        If HasPlayer() = True Then

            ForegroundColor = _character.GetColour
            Return _character.GetPieceSymbol

        ElseIf _overlay = True Then

            Return _overlayChar

        Else

            Return _symbol

        End If

    End Function

    'sets the player on the square
    Public Sub SetPlayer(player As Player)

        _player = player
        _character = player.GetCharacter
        player.SetSquare(Me)

    End Sub

    'removes the player piece from the square
    Public Sub UnSetPlayer()

        _character = Nothing
        _player.SetSquare(Nothing)
        _player = Nothing

    End Sub

    'returns if the square has a player piece in it 
    Public Function HasPlayer() As Boolean

        Return Not _player Is Nothing

    End Function

    'displays the square
    Public Sub DisplaySquare()

        SetCursorPosition(_X, _Y)
        Write(GetDisplayCharacter())
        ForegroundColor = ConsoleColor.White

    End Sub

End Class
