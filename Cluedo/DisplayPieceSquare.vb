Public Class DisplayPieceSquare

    Inherits Square

    Public Sub New(symbol As Char, overlayChar As Char)

        MyBase.New(symbol)
        _canLandOn = True
        _door = False
        _overlay = True
        _overlayChar = overlayChar

    End Sub

End Class
