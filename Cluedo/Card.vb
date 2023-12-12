Public Class Card

    Private _cardName As String

    Public Sub New(cardName As String)

        _cardName = cardName

    End Sub

    'returns the name of the card
    Public Function GetName() As String

        Return _cardName

    End Function

End Class
