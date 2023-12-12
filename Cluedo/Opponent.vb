Public Class Opponent

    Private _opponentPlayer As Player
    Private _cardsSuspectedInHand As New List(Of Card)
    Private _cardsInHand As New List(Of Card)
    Private _cardsNotInHand As New List(Of Card)

    Private Const _inHandSymbol As Char = "O"
    Private Const _notInHandSymbol As Char = "X"
    Private Const _suspectedSymbol As Char = "?"
    Private Const _unknownSymbol As Char = "-"
    Private Const _solvedSymbol As Char = "Y"

    Public Sub New(opponenentPlayer As Player)

        _opponentPlayer = opponenentPlayer

    End Sub

    'gets the opponent player
    Public Function GetPlayer() As Player

        Return _opponentPlayer

    End Function

    'gets the cards that are known not to be in the opponents hand
    Public Function GetCardsNotInHand() As List(Of Card)

        Return _cardsNotInHand

    End Function

    'gets the cards known to be in the opponents hand
    Public Function GetCardsInHand() As List(Of Card)

        Return _cardsInHand

    End Function

    'gets the cards that are known not to be in the opponents hand
    Public Function GetCardsSuspectedInHand() As List(Of Card)

        Return _cardsSuspectedInHand

    End Function

    'gets the symbol for solved
    Public Function GetSolvedSymbol() As Char

        Return _solvedSymbol

    End Function

    'gets the symbol for not in hand
    Public Function GetNotInHandSymbol() As Char

        Return _notInHandSymbol

    End Function

    'adds a card to the opponents hand
    Public Sub AddInHandCard(card As Card)

        _cardsInHand.Add(card)

    End Sub

    'adds a card to not in the opponents hand
    Public Sub AddNotInHandCard(card As Card)

        _cardsNotInHand.Add(card)

    End Sub

    'adds a card to suspected in the opponents hand
    Public Sub AddSuspectedInHandCard(card As Card)

        _cardsSuspectedInHand.Add(card)

    End Sub

    'returns the symbol that should be displayed in the row of that card for this opponent
    Public Function GetSymbol(card As Card) As Char

        If _cardsInHand.Contains(card) Then

            Return _inHandSymbol

        ElseIf _cardsNotInHand.Contains(card) Then

            Return _notInHandSymbol

        ElseIf _cardsSuspectedInHand.Contains(card) Then

            Return _suspectedSymbol

        End If

        Return _unknownSymbol

    End Function

End Class
