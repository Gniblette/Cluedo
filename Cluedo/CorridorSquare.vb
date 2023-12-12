Public Class CorridorSquare

    Inherits Square
    Public Sub New(symbol As Char)

        MyBase.New(symbol)
        _canLandOn = True
        _door = False
        _overlay = False

    End Sub

End Class
