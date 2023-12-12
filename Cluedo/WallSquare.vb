Public Class WallSquare

    Inherits Square

    Public Sub New(symbol As Char)

        MyBase.New(symbol)
        _canLandOn = False
        _door = False
        _overlay = False

    End Sub

End Class
