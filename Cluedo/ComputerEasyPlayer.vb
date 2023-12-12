Public Class ComputerEasyPlayer

    Inherits ComputerPlayer

    Sub New(character As Suspect, pieceSymbol As Char, playerNumber As Integer)

        MyBase.New(character, pieceSymbol, playerNumber)

    End Sub

    'makes a ranom descision if it should remain in a room or move
    Public Overrides Function GetMoveFromRoomChoice() As Boolean

        Dim choice As Integer
        choice = MakeRandomDecision(0, 2)

        'randomly stay or move
        If choice = 1 Then

            TurnInfomation.DisplayStayOrMoveChoice("Move")
            Return True

        Else

            TurnInfomation.DisplayStayOrMoveChoice("Stay")
            Return False

        End If

    End Function

    'choses a random target room and a random door to pathfind to
    Protected Overrides Function GetTargetSquare(board As Board, roomsToVisit As List(Of Room)) As Square

        Dim targetRoom As Integer
        Dim targetDoor As Integer
        Dim doors As List(Of Square)

        'gets a random room
        targetRoom = MakeRandomDecision(0, roomsToVisit.Count - 1)

        doors = board.GetDoorPositions(roomsToVisit(targetRoom))

        'gets a random door on that room
        targetDoor = MakeRandomDecision(0, doors.Count - 1)

        Return doors(targetDoor)

    End Function

    'chooses a random card to reveal if there are multiple options
    Protected Overrides Function GetCardRevealChoice(cards As List(Of Card)) As Card

        Dim choice As Integer
        choice = MakeRandomDecision(0, cards.Count) '-1)

        Return cards(choice)

    End Function

    'makes a guess of a random suspect a random weapon and the current room
    Public Overrides Function MakeGuess(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room)) As Guess

        CheckSolution(suspects, weapons, rooms)

        Dim guess As New Guess

        MakeRandomGuess(guess, suspects, weapons)
        guess.AddRoom(_currentRoom)

        Return guess

    End Function

    Protected Sub MakeRandomGuess(ByRef guess As Guess, suspects As List(Of Suspect), weapons As List(Of Weapon))

        Dim randomSuspect, randomWeapon As Integer

        randomSuspect = MakeRandomDecision(0, suspects.Count - 1)
        guess.AddSuspect(suspects(randomSuspect))
        randomWeapon = MakeRandomDecision(0, weapons.Count - 1)
        guess.AddWeapon(weapons(randomWeapon))

    End Sub

    'the computer player chooses to make a final accusation if they know all of its elements
    Public Overrides Function GetMakeFinalAccusationChoice() As Boolean

        If _solution.GetGuessedSuspect IsNot Nothing And _solution.GetGuessedWeapon IsNot Nothing And _solution.GetGuessedRoom IsNot Nothing Then

            Return True

        Else

            Return False

        End If

    End Function

    'returns the solution
    Public Overrides Function GetFinalAccusation(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room)) As Guess

        Return _solution

    End Function

    'generates a random number between  a given range
    Protected Function MakeRandomDecision(min As Integer, max As Integer)

        Dim randomiser As New Random

        Return randomiser.Next(min, max)

    End Function

    'gets if a room is not in all of the opponents hands and returns it 
    Protected Function GetIfRoomFound(rooms As List(Of Room)) As Room

        Dim counter As Integer

        For i = 0 To rooms.Count - 1

            For j = 0 To _opponents.Count - 1

                If _opponents(j).GetCardsNotInHand.Contains(rooms(i)) Then

                    counter = counter + 1

                End If

            Next

            'if not in all hands then it must be in the solution
            If counter = _opponents.Count Then

                Return rooms(i)

            Else

                counter = 0

            End If

        Next

        Return Nothing

    End Function

    'gets if a weapon is not in all of the opponents hands and returns it 
    Protected Function GetIfWeaponFound(weapons As List(Of Weapon)) As Weapon

        Dim counter As Integer

        For i = 0 To weapons.Count - 1

            For j = 0 To _opponents.Count - 1

                If _opponents(j).GetCardsNotInHand.Contains(weapons(i)) Then

                    counter = counter + 1

                End If

            Next

            'if not in all hands then it must be in the solution
            If counter = _opponents.Count Then

                Return weapons(i)

            Else

                counter = 0

            End If

        Next

        Return Nothing

    End Function

    'gets if a suspect is not in all of the opponents hands and returns it 
    Protected Function GetIfSuspectFound(suspects As List(Of Suspect)) As Suspect

        Dim counter As Integer

        For i = 0 To suspects.Count - 1

            For j = 0 To _opponents.Count - 1

                If _opponents(j).GetCardsNotInHand.Contains(suspects(i)) Then

                    counter = counter + 1

                End If

            Next

            'if not in all hands then it must be in the solution
            If counter = _opponents.Count Then

                Return suspects(i)

            Else

                counter = 0

            End If

        Next

        Return Nothing

    End Function

    'checks if any of the components of the solution has been found
    Protected Function CheckSolution(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room))

        Dim counter As Integer
        Const numberOfCardTypes As Integer = 3

        'if not already found suspect
        If _solution.GetGuessedSuspect Is Nothing Then

            'see if you have found it
            _solution.AddSuspect(GetIfSuspectFound(suspects))

        Else

            counter = counter + 1

        End If

        'if not already found weapon
        If _solution.GetGuessedWeapon Is Nothing Then

            'see if you have found it
            _solution.AddWeapon(GetIfWeaponFound(weapons))

        Else

            counter = counter + 1

        End If

        'if not already found room
        If _solution.GetGuessedRoom Is Nothing Then

            'see if you have found it
            _solution.AddRoom(GetIfRoomFound(rooms))

        Else

            counter = counter + 1

        End If

        If counter = numberOfCardTypes Then

            Return True

        Else

            Return False

        End If

    End Function

End Class
