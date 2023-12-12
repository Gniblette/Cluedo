Public Class ComputerMediumPlayer

    Inherits ComputerEasyPlayer

    Public Sub New(character As Suspect, pieceSymbol As Char, playerNumber As Integer)

        MyBase.New(character, pieceSymbol, playerNumber)

    End Sub

    'only move when they know somebody has that room in their hand
    Public Overrides Function GetMoveFromRoomChoice() As Boolean

        For i = 0 To _opponents.Count - 1

            'if you know one of your opponents has that room then move
            If _opponents(i).GetCardsInHand.Contains(_currentRoom) Then

                TurnInfomation.DisplayStayOrMoveChoice("Move")
                Return True

            End If

        Next

        'keep checking room
        TurnInfomation.DisplayStayOrMoveChoice("Stay")

        Return False

    End Function

    'makes a guess based on which type of card they dont know about in the solution
    Public Overrides Function MakeGuess(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room)) As Guess

        Dim guess As New Guess
        Dim randomiser As New Random
        Dim choice As Integer
        Dim unknownWeapons As List(Of Weapon)
        Dim unknownSuspects As List(Of Suspect)

        'if not found correct solution
        If CheckSolution(suspects, weapons, rooms) = False Then

            unknownWeapons = GetUnkownWeapons(weapons)
            unknownSuspects = GetUnkownSuspects(suspects)

            'if correct room found
            If _solution.GetGuessedRoom IsNot Nothing Then

                'if correct weapon found
                If _solution.GetGuessedWeapon IsNot Nothing Then

                    'a random weapon from hand a known room and an unkown suspect
                    TryToFindSuspect(guess, weapons, unknownSuspects)

                    'if correct suspect found
                ElseIf _solution.GetGuessedSuspect IsNot Nothing Then

                    'a random suspect from hand a known room and an unkown weapon
                    TryToFindWeapon(guess, suspects, unknownWeapons)

                    'if found neither try to find a random thing
                Else

                    choice = randomiser.Next(0, 2)

                    'find weapon
                    If choice = 0 Then

                        TryToFindWeapon(guess, suspects, unknownWeapons)

                        'find suspect
                    Else

                        TryToFindSuspect(guess, weapons, unknownSuspects)

                    End If

                End If

                    'if room not found
                    Else

                'a random suspect and weapon from hand and unkown room
                TryToFindRoom(guess, suspects, weapons)

            End If

            'just make random decision
        Else

            MakeRandomGuess(guess, suspects, weapons)

        End If

        guess.AddRoom(_currentRoom)

        Return guess

    End Function

    'makes a guess of a suspect from their hand and an unkown weapon
    Protected Sub TryToFindWeapon(ByRef guess As Guess, suspects As List(Of Suspect), unknownWeapons As List(Of Weapon))

        Dim randomWeapon As Integer

        guess.AddSuspect(GetRandomSuspectFromHand(suspects))
        randomWeapon = MakeRandomDecision(0, unknownWeapons.Count - 1)
        guess.AddWeapon(unknownWeapons(randomWeapon))

    End Sub

    'makes a guess of a weapon from their hand and an unkown suspect
    Protected Sub TryToFindSuspect(ByRef guess As Guess, weapons As List(Of Weapon), unknownsuspects As List(Of Suspect))

        Dim randomSuspect As Integer

        guess.AddWeapon(GetRandomWeaponFromHand(weapons))
        randomSuspect = MakeRandomDecision(0, unknownsuspects.Count - 1)
        guess.AddSuspect(unknownsuspects(randomSuspect))

    End Sub

    'makes a guess of a weapon and a suspect from their hand
    Protected Sub TryToFindRoom(ByRef guess As Guess, suspects As List(Of Suspect), weapons As List(Of Weapon))

        guess.AddSuspect(GetRandomSuspectFromHand(suspects))
        guess.AddWeapon(GetRandomWeaponFromHand(weapons))

    End Sub

    'gets a random weapon from their hand if they have any and if not just a random weapon
    Protected Function GetRandomWeaponFromHand(weapons As List(Of Weapon)) As Weapon

        Dim randomWeapon As Integer

        'if they have suspects in their hand
        If _weaponsInHand.Count > 0 Then

            randomWeapon = MakeRandomDecision(0, _weaponsInHand.Count - 1)
            Return _weaponsInHand(randomWeapon)

            'if they dont get random
        Else

            randomWeapon = MakeRandomDecision(0, weapons.Count - 1)
            Return weapons(randomWeapon)

        End If

    End Function

    'gets a random suspect from their hand if they have any and if not just a random weapon
    Protected Function GetRandomSuspectFromHand(suspects As List(Of Suspect)) As Suspect

        Dim randomSuspect As Integer

        'if they have suspects in their hand
        If _suspectsInHand.Count > 0 Then

            randomSuspect = MakeRandomDecision(0, _suspectsInHand.Count - 1)
            Return _suspectsInHand(randomSuspect)

            'if they dont get random
        Else

            randomSuspect = MakeRandomDecision(0, suspects.Count - 1)
            Return suspects(randomSuspect)

        End If

    End Function

    'gets a list of all the suspects who are not known to be in a hand
    Protected Function GetUnkownSuspects(suspects As List(Of Suspect)) As List(Of Suspect)

        Dim opponentsHand As List(Of Card)
        Dim knownSuspects As New List(Of Suspect)
        Dim unknownSuspects As New List(Of Suspect)

        For i = 0 To _opponents.Count - 1

            opponentsHand = _opponents(i).GetCardsInHand

            For j = 0 To opponentsHand.Count - 1

                If TypeOf opponentsHand(j) Is Suspect Then

                    knownSuspects.Add(opponentsHand(j))

                End If


            Next

        Next

        'if they arent known then they are unknown
        For i = 0 To suspects.Count - 1

            If Not knownSuspects.Contains(suspects(i)) Then

                unknownSuspects.Add(suspects(i))

            End If

        Next

        Return unknownSuspects

    End Function

    'gets a list of all the weapons that are not known to be in a hand
    Protected Function GetUnkownWeapons(weapons As List(Of Weapon)) As List(Of Weapon)

        Dim opponentsHand As List(Of Card)
        Dim knownWeapons As New List(Of Weapon)
        Dim unknownWeapons As New List(Of Weapon)

        For i = 0 To _opponents.Count - 1

            opponentsHand = _opponents(i).GetCardsInHand

            For j = 0 To opponentsHand.Count - 1

                If TypeOf opponentsHand(j) Is Weapon Then

                    knownWeapons.Add(opponentsHand(j))

                End If


            Next

        Next

        'if they arent known then they are unknown
        For i = 0 To weapons.Count - 1

            If Not knownWeapons.Contains(weapons(i)) Then

                unknownWeapons.Add(weapons(i))

            End If

        Next

        Return unknownWeapons

    End Function

    'chooses a path to the random door of the closest room that is desired to visit 
    Protected Overrides Function ChoosePath(board As Board, roomsToVisit As List(Of Room)) As List(Of Square)

        Dim targetDoor As Integer
        Dim doors As List(Of Square)
        Dim path As List(Of Square) = Nothing
        Dim tempPath As List(Of Square)

        'for each room to check
        For i = 0 To roomsToVisit.Count - 1

            Dim visited(board.GetWidth, board.GetHeight) As Boolean

            'get all of that rooms doors
            doors = board.GetDoorPositions(roomsToVisit(i))
            'choose a random one to pathfind to
            targetDoor = MakeRandomDecision(0, doors.Count - 1)

            tempPath = PathFind(_currentSquare, doors(targetDoor), board, visited)

            If tempPath IsNot Nothing Then

                'if its the first path make it the new best path
                If path Is Nothing Then

                    path = tempPath

                    'if new path is the shortest make it the new bestpath
                ElseIf tempPath.Count < path.Count Then

                    path = tempPath

                End If

            End If

        Next

        Return path

    End Function

End Class
