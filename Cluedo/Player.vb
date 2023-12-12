Imports System.Console
Public Class Player

    Protected _hand As New List(Of Card)
    Protected _roomsInHand As New List(Of Room)
    Protected _suspectsInHand As New List(Of Suspect)
    Protected _weaponsInHand As New List(Of Weapon)
    Protected _character As Suspect
    Protected _colour As ConsoleColor
    Protected _opponents As New List(Of Opponent)
    Protected _pieceSymbol As Char
    Protected _myself As Opponent
    Protected _clueSheet As ClueSheet
    Protected _currentRoom As Room
    Protected _playerNumber As Integer
    Protected _currentSquare As Square

    Public Sub New(character As Suspect, pieceSymbol As Char, playerNumber As Integer)

        _pieceSymbol = pieceSymbol
        _character = character
        _colour = character.GetColour
        _playerNumber = playerNumber

    End Sub

    'returns the players character
    Public Function GetCharacter() As Suspect

        Return _character

    End Function

    'returns the players base symbol
    Public Function GetSymbol() As Char

        Return _pieceSymbol

    End Function

    'returns the current square that the players piece is in
    Public Function GetSquare() As Square

        Return _currentSquare

    End Function

    'sets the players current square to their new square
    Public Sub SetSquare(square As Square)

        _currentSquare = square

    End Sub

    'returns the name of the player
    Public Function Getname() As String

        If _playerNumber = 1 Then

            Return "User"

        Else

            Return String.Format("Player {0}", _playerNumber)

        End If

    End Function

    'returns the players cluesheet
    Public Function GetClueSheet() As ClueSheet

        Return _clueSheet

    End Function

    'returns if the player is currently on a room or not
    Public Function GetInRoom() As Boolean

        Return Not _currentRoom Is Nothing

    End Function

    'gets the number of the player
    Public Function GetPlayerNumber() As Integer

        Return _playerNumber

    End Function

    'makes each player (including itself) into an opponent and adds the to a list
    Public Sub MakeOpponents(players As List(Of Player))

        Dim newOpponent As Opponent

        For i = 0 To players.Count - 1

            newOpponent = New Opponent(players(i))
            _opponents.Add(newOpponent)

            If players(i) Is Me Then

                _myself = newOpponent

            End If

        Next

    End Sub

    'adds a card to an ooponents suspected in hand list
    Public Sub AddSuspectedInHandCard(card As Card, opponent As Integer)

        _opponents(opponent).AddSuspectedInHandCard(card)

    End Sub

    'adds a card to an ooponents not in hand list
    Public Sub AddNotInHandCard(card As Card, opponent As Integer)

        _opponents(opponent).AddNotInHandCard(card)

    End Sub

    'adds a card to an opponents in hand list
    Public Sub AddInHandCard(card As Card, opponent As Integer)

        _opponents(opponent).AddInHandCard(card)

        FillInCompleteRow(card, _opponents(opponent))

    End Sub

    'adds a card to the players hand
    Public Sub DealCard(dealtCard As Card)

        _hand.Add(dealtCard)

        If TypeOf dealtCard Is Room Then

            _roomsInHand.Add(dealtCard)

        ElseIf TypeOf dealtCard Is Suspect Then

            _suspectsInHand.Add(dealtCard)

        Else

            _weaponsInHand.Add(dealtCard)

        End If

        'fills in card for yourself the opponenent
        For i = 0 To _opponents.Count - 1

            If _opponents(i) Is _myself Then

                AddInHandCard(dealtCard, i)

            End If

        Next

    End Sub

    'adds all the cards that the player does not have in their hand to a list to be crossed off from their cluesheet
    Public Sub CardsNotInHand(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room))

        'aads suspects
        For i = 0 To suspects.Count - 1

            If Not _myself.GetCardsInHand.Contains(suspects(i)) Then

                _myself.AddNotInHandCard(suspects(i))

            End If

        Next

        'adds weapons
        For i = 0 To weapons.Count - 1

            If Not _myself.GetCardsInHand.Contains(weapons(i)) Then

                _myself.AddNotInHandCard(weapons(i))

            End If

        Next

        'adds rooms
        For i = 0 To rooms.Count - 1

            If Not _myself.GetCardsInHand.Contains(rooms(i)) Then

                _myself.AddNotInHandCard(rooms(i))

            End If

        Next

    End Sub

    'fills in a complete row of Xs apart from the owner when a card is discovered in a hand
    Protected Sub FillInCompleteRow(card As Card, OpponentThatHasCard As Opponent)

        For i = 0 To _opponents.Count - 1

            If Not _opponents(i) Is OpponentThatHasCard Then

                _opponents(i).AddNotInHandCard(card)

            End If

        Next

    End Sub

    'creates a cluesheet for the player
    Public Sub SetUpClueSheet(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room))

        _clueSheet = New ClueSheet(suspects, weapons, rooms, _opponents)

    End Sub

    'returns all the names of the cards in the players hand
    Public Sub DisplayHand()

        TurnInfomation.DisplayHand(_hand)

    End Sub

    'allows the user to make a suggestion by selecting a suspect and a weapon as well as the room they are currently in
    Public Overridable Function MakeGuess(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room)) As Guess

        Dim menuChoice As Integer
        Dim guess As New Guess

        'gets the suspect
        Dim suspectOptions As New List(Of String)
        suspectOptions.AddRange(GetListOfSuspectNames(suspects))
        Dim suspectGuessMenu As Menu = New Menu(suspectOptions, "Which suspect would you like to suggest?")

        menuChoice = suspectGuessMenu.ProcessInputs()
        guess.AddSuspect(suspects(menuChoice))

        'gets the weapon
        Dim weaponOptions As New List(Of String)
        weaponOptions.AddRange(GetListOfWeaponNames(weapons))
        Dim weaponGuessMenu As Menu = New Menu(weaponOptions, "Which weapon would you like to suggest?")

        menuChoice = weaponGuessMenu.ProcessInputs()
        guess.AddWeapon(weapons(menuChoice))

        'gets the room
        guess.AddRoom(_currentRoom)

        Return guess

    End Function

    'gets if the user wants to stay in the current room  or move
    Public Overridable Function GetMoveFromRoomChoice() As Boolean

        Clear()

        Dim options As New List(Of String)
        options.AddRange({"Stay in room", "Move"})
        Dim moveFromRoomMenu As Menu = New Menu(options, "Would you like to remain in your current room or move?")
        Dim menuChoice As Integer

        menuChoice = moveFromRoomMenu.ProcessInputs()

        'stay
        If menuChoice = 0 Then

            TurnInfomation.DisplayStayOrMoveChoice("Stay")
            Return False

            'move
        Else

            TurnInfomation.DisplayStayOrMoveChoice("Move")
            Return True

        End If

    End Function

    'asks if a player would like to make a final accusation or not 
    Public Overridable Function GetMakeFinalAccusationChoice() As Boolean

        Clear()

        Dim options As New List(Of String)
        options.AddRange({"No", "Yes"})
        Dim moveFromRoomMenu As Menu = New Menu(options, "Would you like to make a final accusation?")
        Dim menuChoice As Integer

        menuChoice = moveFromRoomMenu.ProcessInputs()

        'no
        If menuChoice = 1 Then

            Return True

            'yes
        Else

            Return False

        End If

    End Function

    'gets the players final accusation
    Public Overridable Function GetFinalAccusation(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room)) As Guess

        Dim menuChoice As Integer
        Dim guess As New Guess

        'gets the suspect
        Dim suspectOptions As New List(Of String)
        suspectOptions.AddRange(GetListOfSuspectNames(suspects))
        Dim suspectGuessMenu As Menu = New Menu(suspectOptions, "Which suspect would you like to accuse?")

        menuChoice = suspectGuessMenu.ProcessInputs()
        guess.AddSuspect(suspects(menuChoice))

        'gets the weapon
        Dim weaponOptions As New List(Of String)
        weaponOptions.AddRange(GetListOfWeaponNames(weapons))
        Dim weaponGuessMenu As Menu = New Menu(weaponOptions, "Which weapon would you like to accuse?")

        menuChoice = weaponGuessMenu.ProcessInputs()
        guess.AddWeapon(weapons(menuChoice))

        'gets the room
        Dim roomOptions As New List(Of String)
        roomOptions.AddRange(GetListOfRoomNames(rooms))
        Dim roomGuessMenu As Menu = New Menu(roomOptions, "Which room would you like to accuse?")

        menuChoice = roomGuessMenu.ProcessInputs()
        guess.AddRoom(rooms(menuChoice))

        If ConfirmFinalAccusation(guess) = True Then

            Return guess

        Else

            Return Nothing

        End If

    End Function

    'takes the user to a menu wherer they can confirm their final accusation
    Private Function ConfirmFinalAccusation(guess As Guess) As Boolean

        Clear()

        Dim message As String
        message = "Would you like to accuse " + guess.GetGuessedSuspect.GetName + " in the " + guess.GetGuessedRoom.GetName + " with the " + guess.GetGuessedWeapon.GetName

        Dim options As New List(Of String)
        options.AddRange({"Yes", "No"})
        Dim moveFromRoomMenu As Menu = New Menu(options, message)
        Dim menuChoice As Integer

        menuChoice = moveFromRoomMenu.ProcessInputs()

        'yes
        If menuChoice = 0 Then

            Return True

            'no
        Else

            Return False

        End If

    End Function

    'allows the player to move around the board
    Public Overridable Sub Move(diceRoll As Integer, board As Board, rooms As List(Of Room))

        Dim input As ConsoleKeyInfo
        Dim startSquare = GetSquare()
        Dim moveSquare As Square = Nothing
        Dim previousSquare As Square
        Dim moves As Integer
        Dim onDoor, exitingRoom As Boolean
        Dim oldRoom As Room

        oldRoom = _currentRoom

        'allows the user to move as much as theyve rolled
        Do While moves <= diceRoll

            TurnInfomation.DisplayMoves(diceRoll - moves)

            input = ReadKey()

            'entering 
            If onDoor = True And exitingRoom = False Then

                'landed on a door
                If input.Key = ConsoleKey.Enter Then

                    'move into room
                    _currentRoom = moveSquare.GetRoom()

                    MovePieceIntoRoom(board)
                    Exit Do

                ElseIf input.Key = ConsoleKey.Backspace Then

                    'reset player position and go agaion
                    previousSquare = GetSquare()
                    ResetMove(previousSquare, startSquare, oldRoom)
                    board.DisplayBoard()

                    Move(diceRoll, board, rooms)
                    Exit Do

                End If

            ElseIf moves = diceRoll Then

                'last move
                If input.Key = ConsoleKey.Enter Then

                    Exit Do

                ElseIf input.Key = ConsoleKey.Backspace Then

                    'reset player position and go agaion
                    previousSquare = GetSquare()
                    ResetMove(previousSquare, startSquare, oldRoom)
                    board.DisplayBoard()

                    Move(diceRoll, board, rooms)
                    Exit Do

                End If

            Else

                moveSquare = Nothing
                previousSquare = GetSquare()

                'gets the square in the entered direction
                If input.Key = ConsoleKey.RightArrow Then

                    moveSquare = board.GetSquareRight(GetSquare())

                ElseIf input.Key = ConsoleKey.LeftArrow Then

                    moveSquare = board.GetSquareLeft(GetSquare())

                ElseIf input.Key = ConsoleKey.UpArrow Then

                    moveSquare = board.GetSquareUp(GetSquare())

                ElseIf input.Key = ConsoleKey.DownArrow Then

                    moveSquare = board.GetSquareDown(GetSquare())

                ElseIf input.Key = ConsoleKey.Backspace Then

                    'reset player position and go agaion
                    ResetMove(previousSquare, startSquare, oldRoom)
                    board.DisplayBoard()

                    Move(diceRoll, board, rooms)
                    Exit Do

                End If

                'only allows the player to move within the bounds of the board
                If moveSquare Is Nothing Then

                    Continue Do

                End If

                'only allows the player to move onto landable sqaues
                If moveSquare.GetCanlandOn = False Then

                    Continue Do

                End If

                'only allows the player to move into squares without other players in them
                If moveSquare.HasPlayer = True Then

                    Continue Do

                End If

                'moves the player on the board
                previousSquare.UnSetPlayer()
                moveSquare.SetPlayer(Me)
                board.DisplayBoard()

                'if on door and not in room then entering
                If moveSquare.GetDoor = True And _currentRoom Is Nothing Then

                    onDoor = True
                    exitingRoom = False

                ElseIf moveSquare.GetDoor = True Then

                    exitingRoom = True
                    _currentRoom = Nothing

                End If

                'only increases move if not moving in room
                moves = moves + IncreaseMoves(moveSquare)

            End If

        Loop

    End Sub

    'increases the moves if they arent moving
    Protected Function IncreaseMoves(square As Square) As Integer

        If Not TypeOf square Is InRoomSquare And
           Not TypeOf square Is DisplayPieceSquare And
           Not TypeOf square Is DoorSquare Then

            Return 1

        Else

            Return 0

        End If

    End Function

    Protected Sub MovePieceIntoRoom(board As Board)

        Dim counter As Integer
        Dim roomDisplaySquare As Square

        _currentSquare.UnSetPlayer()
        roomDisplaySquare = board.GetRoomDisplayPosition(_currentRoom)

        Do

            If counter Mod 4 = 0 And Not counter = 0 Then

                roomDisplaySquare = board.GetSquareDown(roomDisplaySquare)
                roomDisplaySquare = board.GetSquareLeft(roomDisplaySquare)
                roomDisplaySquare = board.GetSquareLeft(roomDisplaySquare)
                roomDisplaySquare = board.GetSquareLeft(roomDisplaySquare)

            Else

                roomDisplaySquare = board.GetSquareRight(roomDisplaySquare)

            End If


            If GetIfSquareMoveable(roomDisplaySquare) = True Then

                Exit Do


            End If

            counter = counter + 1

        Loop

        roomDisplaySquare.SetPlayer(Me)
        board.DisplayBoard()


    End Sub

    'resets the players position if they want to pick how they move again
    Private Sub ResetMove(previousSquare As Square, startsquare As Square, oldRoom As Room)

        previousSquare.UnSetPlayer()
        SetSquare(startsquare)
        startsquare.SetPlayer(Me)
        _currentRoom = oldRoom

    End Sub

    'checks if a piece could move onto that square
    Protected Function GetIfSquareMoveable(square As Square) As Boolean

        'if out of bounds
        If square Is Nothing Then

            Return False

            'if cant be landed on
        ElseIf square.GetCanlandOn = False Then

            Return False

        ElseIf square.HasPlayer = True Then

            Return False

        End If

        Return True

    End Function

    'returns the card the player has revealed from the guess
    Public Function RevealCard(guess As Guess) As Card

        Dim cardsToReveal As New List(Of Card)

        'gets all the cards from the guess that the user has in their hand
        For i = 0 To _hand.Count - 1

            If _hand(i) Is guess.GetGuessedSuspect Then

                cardsToReveal.Add(_hand(i))

            End If

            If _hand(i) Is guess.GetGuessedWeapon Then

                cardsToReveal.Add(_hand(i))

            End If

            If _hand(i) Is guess.GetGuessedRoom Then

                cardsToReveal.Add(_hand(i))

            End If

        Next

        'if they dont have any matching cards reveal nothing
        If cardsToReveal.Count = 0 Then

            Return Nothing

            'if they only have one reveal it
        ElseIf cardsToReveal.Count = 1 Then

            Return cardsToReveal(0)

            'if they have multiple let them choose one to reveal
        Else

            Return GetCardRevealChoice(cardsToReveal)

        End If

    End Function

    'gets the card that the user chooses to reveal (if they have multiple options)
    Protected Overridable Function GetCardRevealChoice(cards As List(Of Card)) As Card

        Dim cardOptions As New List(Of String)
        Dim menuchoice As Integer

        'adds all the cards they could choose to reveal to a list
        For i = 0 To cards.Count - 1

            cardOptions.Add(cards(i).GetName())

        Next

        Dim cardRevealMenu As Menu = New Menu(cardOptions, "Which card would you like to reveal?")
        menuchoice = cardRevealMenu.ProcessInputs

        Return cards(menuchoice)

    End Function

    'gets the name of all the suspects 
    Private Function GetListOfSuspectNames(suspects As List(Of Suspect)) As List(Of String)

        Dim suspectNames As New List(Of String)

        For i = 0 To suspects.Count - 1

            suspectNames.Add(suspects(i).GetName)

        Next

        Return suspectNames

    End Function

    'gets the name of all the weapons
    Private Function GetListOfWeaponNames(weapons As List(Of Weapon)) As List(Of String)

        Dim weaponNames As New List(Of String)

        For i = 0 To weapons.Count - 1

            weaponNames.Add(weapons(i).GetName)

        Next

        Return weaponNames

    End Function

    'gets the name of all the rooms
    Private Function GetListOfRoomNames(rooms As List(Of Room)) As List(Of String)

        Dim roomNames As New List(Of String)

        For i = 0 To rooms.Count - 1

            roomNames.Add(rooms(i).GetName)

        Next

        Return roomNames

    End Function


End Class
