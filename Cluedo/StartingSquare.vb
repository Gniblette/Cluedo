Public Class StartingSquare

    Inherits Square
    Public Sub New(symbol As Char, overlaychar As Char)

        MyBase.New(symbol)
        _canLandOn = True
        _door = False
        _overlay = True
        _overlayChar = overlaychar

    End Sub

End Class
