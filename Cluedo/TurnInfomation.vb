Imports System.Console

Public Class TurnInfomation

    Private Shared _columnOffset As Integer
    Private Shared _rowOffset As Integer
    Private Shared _width As Integer

    'sets the width and location of the turn infomation column
    Public Shared Sub SetUp(columnOffset As Integer, rowOffset As Integer, width As Integer)

        _columnOffset = columnOffset
        _rowOffset = rowOffset
        _width = width

    End Sub

    'gets the width of the turn infomation column
    Public Shared Function GetWidth() As Integer

        Return _width

    End Function

    'diplsays the text in the desired position
    Private Shared Sub Display(text As String, row As Integer)

        text = text.PadRight(_width)
        SetCursorPosition(_columnOffset, _rowOffset + row)
        Write(text)

    End Sub

    'displays a message in the turn infoation coulmn and starts anew line if the next word will go outside of the space
    Public Shared Sub DisplayLong(text As String, row As Integer)
        Dim words() As String

        words = text.Split(" ")

        SetCursorPosition(_columnOffset, _rowOffset + row)

        For i = 0 To words.Count - 1

            If GetCursorPosition.Left + words(i).Length > _columnOffset + _width Then

                SetCursorPosition(_columnOffset, GetCursorPosition.Top + 1)

            End If

            If i = words.Count - 1 Then

                Write(words(i).PadRight(_width + _columnOffset - CursorLeft))

            Else

                Write(words(i) + " ")

            End If

        Next

    End Sub

    'displays the amount of moves remaining
    Public Shared Sub DisplayMoves(moves As Integer)

        Dim text As String
        text = String.Format("Moves: {0}", moves)

        Display(text, 6)

    End Sub

    'displays the current turn
    Public Shared Sub DisplayTurn(turnTaker As String)

        Dim text As String
        text = String.Format("Turn: {0}", turnTaker)

        Display(text, 0)

    End Sub

    'displays the amaount rolled
    Public Shared Sub DisplayRoll(roll As Integer)

        Dim text As String
        text = String.Format("Roll: {0}", roll)

        Display(text, 5)

    End Sub

    'displays if the player has chosen to stay or move from their current room
    Public Shared Sub DisplayStayOrMoveChoice(choice As String)

        Dim text As String
        text = String.Format("Stay Or Move: {0}", choice)

        Display(text, 7)

    End Sub

    'returns all the names of the cards in the players hand
    Public Shared Sub DisplayHand(hand As List(Of Card))

        Dim text As String

        text = "Your hand : "

        SetCursorPosition(_columnOffset, _rowOffset + 1)
        Write("Your hand : ")

        For i = 0 To hand.Count - 1

            If i = hand.Count - 1 Then

                text = text + hand(i).GetName

            Else

                text = text + hand(i).GetName + ", "

            End If

        Next

        DisplayLong(text, 1)

    End Sub

    'displays the players guess
    Public Shared Sub DisplayGuess(guess As Guess)

        Dim message As String

        message = "Suggestion: " + guess.GetGuessedSuspect.GetName + " in the " + guess.GetGuessedRoom.GetName + " with the " + guess.GetGuessedWeapon.GetName

        DisplayLong(message, 8)

    End Sub

    'displays the response to a card not being revealed
    Public Shared Sub CardNotRevealed(revealnumber As Integer, revealer As String)

        Dim text As String
        text = String.Format("{0} cannot disprove the suggestion", revealer)

        DisplayLong(text, 11 + revealnumber * 2)

    End Sub

    'displays the response to a card not being revealed
    Public Shared Sub CardRevealed(revealnumber As Integer, revealer As String, card As String, revealee As String)

        Dim text As String

        If revealee = "User" Then

            text = String.Format("{0} revealed the card {1} to you", revealer, card)

        Else

            text = String.Format("{0} has revealed a card to {1}", revealer, revealee)

        End If

        DisplayLong(text, 11 + revealnumber * 2)

    End Sub

    'displays the press enter to roll instruction
    Public Shared Sub DisplayPressEnterMessage()

        Dim text As String
        text = "Press Enter to Roll"

        Display(text, 5)

    End Sub

    'displays the final accusation
    Public Shared Sub DisplayFinalAccusation(player As Player, guess As Guess, numberOfPlayers As Integer)

        Dim message As String

        message = "A final accusation has been made by " + player.Getname + ": " + guess.GetGuessedSuspect.GetName + " in the " + guess.GetGuessedRoom.GetName + " with the " + guess.GetGuessedWeapon.GetName

        'displays below the card revealed responses
        DisplayLong(message, 8)

    End Sub


    'displays the final accusation
    Public Shared Sub DisplayCorrectSolution(solution As Guess)

        Dim message As String

        message = "The Correct Solution was: " + solution.GetGuessedSuspect.GetName + " in the " + solution.GetGuessedRoom.GetName + " with the " + solution.GetGuessedWeapon.GetName

        'displays below the card revealed responses
        DisplayLong(message, 12)

    End Sub

    'displays the result of the game
    Public Shared Sub DisplayResult(correct As Boolean, accuser As Player)

        Dim message As String = ""

        If correct = True And accuser.Getname = "User" Then

            message = "Congratulations you have won the game :)"

        ElseIf correct = True Then

            message = "You lose :( " + accuser.Getname + " has won the game"

        ElseIf correct = False Then

            message = "The final accusation was incorrect the game has ended in a draw"

        End If

        DisplayLong(message, 15)

    End Sub

End Class
