Public Class SquareMaker

    'makes the correct type of square for the board from its symbol loaded in from the file
    Public Function MakeSquare(symbol As Char, roomDisplayKeys As List(Of Char), roomDoorKeys As List(Of Char), startPositions As List(Of Char)) As Square

        Dim newSquare As Square

        'walls
        If symbol = "▓" Or symbol = "│" Or symbol = "─" Or symbol = "┌" Or symbol = "┐" Or symbol = "└" Or symbol = "┘" Then

            newSquare = New WallSquare(symbol)

            'corridors
        ElseIf symbol = "░" Then

            newSquare = New CorridorSquare(symbol)

            'place to displayed in room
        ElseIf roomDisplayKeys.Contains(symbol) Then

            newSquare = New DisplayPieceSquare(symbol, " ")

            'doors
        ElseIf roomDoorKeys.Contains(symbol) Then

            newSquare = New DoorSquare(symbol, " ")

            'start squares
        ElseIf startPositions.Contains(symbol) Then

            newSquare = New StartingSquare(symbol, "░")

            'inside of room squares
        ElseIf symbol = " " Then

            newSquare = New InRoomSquare(symbol)

        Else

            'general squares
            newSquare = New Square(symbol)

        End If

        Return newSquare

    End Function

End Class
