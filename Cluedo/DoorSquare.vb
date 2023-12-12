Public Class DoorSquare

    Inherits Square

    Protected _room As Room

    Public Sub New(symbol As Char, overlaychar As Char)

        MyBase.New(symbol)
        _canLandOn = True
        _door = True
        _overlay = True
        _overlayChar = overlaychar

    End Sub

    'sets the room of the door square
    Public Overrides Sub SetRoom(rooms As List(Of Room))

        For i = 0 To rooms.Count

            If rooms(i).GetDoorKey = GetSymbol() Then

                _room = rooms(i)
                Return

            End If

        Next

    End Sub

    'returns the room of the door square
    Public Overrides Function GetRoom() As Room

        Return _room

    End Function

End Class
