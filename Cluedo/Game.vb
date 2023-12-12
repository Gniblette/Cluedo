Imports System.Console
Public Class Game

    Private _players As New List(Of Player)
    Private _difficulty As Integer
    Private _difficulties As New List(Of String)
    Private _userCharacter As Suspect
    Private _cards As New List(Of Card)
    Private _solution As New Guess
    Private _suspects As New List(Of Suspect)
    Private _weapons As New List(Of Weapon)
    Private _rooms As New List(Of Room)
    Private _numberOfPlayers As Integer
    Private _gameBoard As New Board
    Private _finalAccusation As Guess
    Private _finalAccuser As Player

    Const _turnInfomationWidth As Integer = 35
    Const _turninfomationRow As Integer = 2

    Public Sub New(numberOfPlayers As Integer)

        _numberOfPlayers = numberOfPlayers

    End Sub

    'navigates the players main menu choice to the apppropriate output
    Public Sub SetupGame()

        'loads all the files
        GetSuspects()
        GetWeapons()
        GetRooms()
        GetBoard()

        Dim menuChoice As String

        'only plays game if all files have loaded a value
        If _suspects IsNot Nothing And _weapons IsNot Nothing And _rooms IsNot Nothing And _gameBoard IsNot Nothing Then

            'only plays the game if there are enough cards of each type
            If _suspects.Count >= 4 And _numberOfPlayers <= 8 And _numberOfPlayers <= _suspects.Count - 1 And _numberOfPlayers >= 2 And _rooms.Count >= 4 And _weapons.Count >= 4 Then

                _difficulty = 1
                _userCharacter = _suspects(0)
                _difficulties.AddRange({"Easy", "Medium", "Hard"})

                Do
                    menuChoice = GetMainMenuChoice()

                    'plays the game
                    If menuChoice = 0 Then

                        Play()

                        'allows the user to select their difficulty level
                    ElseIf menuChoice = 1 Then

                        _difficulty = GetDifficultyMenuChoice()

                        'allows user to change their character
                    ElseIf menuChoice = 2 Then

                        _userCharacter = GetCharacterMenuChoice()

                        'takes the user to the help screen
                    ElseIf menuChoice = 3 Then

                        Help()

                    End If

                Loop Until menuChoice = 4

            Else

                WriteLine("There is an error in the setup of the game")
                WriteLine("Please check your files")

            End If

        End If

    End Sub

    'loads the suspects into the game
    Private Sub GetSuspects()

        Dim charactersFromFile As New SuspectLoader
        _suspects = charactersFromFile.LoadSuspects("Suspect Infomation.txt")

    End Sub

    'loads the weapons into the game
    Private Sub GetWeapons()

        Dim weaponsfromFile As New WeaponLoader
        _weapons = weaponsfromFile.LoadWeapons("Weapon infomation.txt")

    End Sub

    'loads the rooms into the game
    Private Sub GetRooms()

        Dim roomsfromFile As New RoomLoader
        _rooms = roomsfromFile.LoadRooms("Room infomation.txt")

    End Sub

    'loads the board into the game
    Private Sub GetBoard()

        _gameBoard.LoadInBoard(_rooms, _suspects)

    End Sub

    'gets the users selection from the main menu
    Public Function GetMainMenuChoice() As String

        Clear()

        Dim mainOptions As New List(Of String)
        mainOptions.AddRange({"Play", "Difficulty: " & _difficulties(_difficulty), "Character: " & _userCharacter.GetName, "Help", "Exit"})
        Dim main As Menu = New Menu(mainOptions, "Welcome to Cluedo please select one of the following options")

        Return main.ProcessInputs()

    End Function

    'gets the users selection from the difficulty menu
    Public Function GetDifficultyMenuChoice() As String

        Clear()

        Dim difficulty As Menu = New Menu(_difficulties, "Please select your difficulty level")

        Return difficulty.ProcessInputs()

    End Function

    'gets the users selection from the character menu if the file has been loaded correctly
    Public Function GetCharacterMenuChoice() As Suspect

        Clear()

        Dim characterMenu As Menu = New Menu(GetListOfSuspectNames(), "Please select your character")
        Dim userCharacterindex As Integer

        userCharacterindex = characterMenu.ProcessInputs()

        Return _suspects(userCharacterindex)

    End Function

    'outputs the rules of the game
    Private Sub Help()

        Dim entre As ConsoleKeyInfo

        Clear()

        WriteLine("HELP")
        WriteLine("The cluesheet rows represent the cards in the game and the columns represent the players")
        WriteLine("The columns in the cluesheet represent the players with each numbered piece on the board ")
        WriteLine("The first column in the cluesheet represents yourself")
        WriteLine("'-' indicates that you have no infomation about this card for that player")
        WriteLine("'O' indicates that you know that the card is in that players hand")
        WriteLine("'X' indicates that you know that the card is NOT in that players hand")
        WriteLine("'Y' indicates that you know the card is in the correct solution")
        WriteLine("The aim of the game is to find all three aspects of the correct solution")
        WriteLine("Good Luck :)")

        'only enter exits because that what the objectives say for some reason
        Do

            entre = ReadKey()

        Loop Until entre.Key = ConsoleKey.Enter

    End Sub

    'gets the name of all the suspects 
    Private Function GetListOfSuspectNames() As List(Of String)

        Dim suspectNames As New List(Of String)

        For i = 0 To _suspects.Count - 1

            suspectNames.Add(_suspects(i).GetName)

        Next

        Return suspectNames

    End Function

    'sets up and processes the game
    Private Sub Play()

        Clear()

        SetUpCards()

        CreateCorrectSolution()

        SetUpPlayers()

        SetUpOpponents()

        DealOutCards()

        FillInCardsNotInOwnHand()

        SetUpClueSheets()

        SetUpTurnInfomation()

        PlayGame()

        DisplayCorrectSolution()

        CheckFinalAccusationCorrect()

        ClearGame()

        ReadKey()

    End Sub

    'adds all the cards of each type to the game
    Private Sub SetUpCards()

        _cards.Clear()

        _cards.AddRange(_suspects)

        _cards.AddRange(_weapons)

        _cards.AddRange(_rooms)

    End Sub

    'removes three random cards one of each type and adds the to the solution
    Private Sub CreateCorrectSolution()

        Dim randomiser As New Random
        Dim removedCardPosition As Integer

        'selects a suspect and adds it to solution
        removedCardPosition = randomiser.Next(0, _suspects.Count - 1)
        _solution.AddSuspect(_cards(removedCardPosition))
        'removes the suspect from the deck
        _cards.RemoveAt(removedCardPosition)

        'selects a weapon and adds it to solution
        '-1 because the list of cards is now one shorter
        removedCardPosition = randomiser.Next(_suspects.Count - 1, _suspects.Count - 1 + _weapons.Count - 1)
        _solution.AddWeapon(_cards(removedCardPosition))
        'removes the weapon from the deck
        _cards.RemoveAt(removedCardPosition)

        'removes a room and adds it to solution
        'double -1s
        removedCardPosition = randomiser.Next(_suspects.Count - 1 + _weapons.Count - 1, _cards.Count - 1)
        _solution.AddRoom(_cards(removedCardPosition))
        'removes the room from the deck
        _cards.RemoveAt(removedCardPosition)

    End Sub

    'adds all the players to the list putting the user first
    Private Sub SetUpPlayers()

        'puts the user first in the list of players and assigns it the piece 1
        Dim user As Player = New Player(_userCharacter, "❶", 1)
        _players.Add(user)

        Dim pieces As New List(Of Char)
        pieces.AddRange({"❷", "❸", "❹", "❺", "❻", "❼", "❽", "❾"})

        Dim newPlayer As Player
        Dim availableSuspects As List(Of Suspect)

        'gets the list of rival players
        availableSuspects = GetRandomSuspects(_userCharacter)

        'assigns the users player to its suspect
        For i = 0 To _suspects.Count - 1

            If _suspects(i).Equals(_userCharacter) Then

                _suspects(i).SetPlayer(user)

            End If

        Next

        'goes through each selected suspect and adds it to the list with the correct difficulty, and the next piece
        For i = 0 To availableSuspects.Count - 1

            'creates a computer player of the correct difficulty
            If _difficulty = 0 Then

                newPlayer = New ComputerEasyPlayer(availableSuspects(i), pieces(i), i + 2)

            ElseIf _difficulty = 1 Then

                newPlayer = New ComputerMediumPlayer(availableSuspects(i), pieces(i), i + 2)

            Else

                newPlayer = New ComputerHardPlayer(availableSuspects(i), pieces(i), i + 2)

            End If

            _players.Add(newPlayer)

            'assigns  the player to the correct suspect
            For j = 0 To _suspects.Count - 1

                If _suspects(j) Is availableSuspects(i) Then

                    _suspects(j).SetPlayer(newPlayer)

                End If

            Next

        Next

        'puts the players on the board
        _gameBoard.MakePieces(_players)

    End Sub

    'selects the correct random suspects to be the the users rival players
    Private Function GetRandomSuspects(userSuspect As Suspect) As List(Of Suspect)

        Dim randomiser As New Random
        Dim availableSuspects As New List(Of Suspect)
        Dim randomSuspect As Integer

        Do

            randomSuspect = randomiser.Next(0, _suspects.Count - 1)

            'if the suspect has not already been selected and is not the users suspect then add it to the list
            If Not userSuspect Is _suspects(randomSuspect) And Not availableSuspects.Contains(_suspects(randomSuspect)) Then

                availableSuspects.Add(_suspects(randomSuspect))

            End If

        Loop Until availableSuspects.Count = _numberOfPlayers - 1

        Return availableSuspects

    End Function

    'creates a list of opponents for each player (they are also an opponent)
    Private Sub SetUpOpponents()

        For i = 0 To _players.Count - 1

            _players(i).MakeOpponents(_players)

        Next

    End Sub

    'deals out radom cards to each player until there are no more cards 
    Private Sub DealOutCards()

        Randomize()
        Dim randomiser As New Random
        Dim cardToDeal As Integer

        Do

            For i = 0 To _players.Count - 1

                If _cards.Count = 0 Then

                    Exit For

                End If

                'takes a card from a random position in the deck
                'gives it to the player, removes it from the deck

                cardToDeal = randomiser.Next(0, _cards.Count - 1)
                _players(i).DealCard(_cards(cardToDeal))
                _cards.RemoveAt(cardToDeal)

            Next

        Loop Until _cards.Count = 0

    End Sub

    'fills in all the cards not in their hand for each player
    Private Sub FillInCardsNotInOwnHand()

        For i = 0 To _players.Count - 1

            _players(i).CardsNotInHand(_suspects, _weapons, _rooms)

        Next

    End Sub

    'creates a cluesheet for each player
    Private Sub SetUpClueSheets()

        For i = 0 To _players.Count - 1

            _players(i).SetUpClueSheet(_suspects, _weapons, _rooms)

        Next

    End Sub

    'sets up the turn infomation column position
    Private Sub SetUpTurnInfomation()

        TurnInfomation.SetUp(_gameBoard.GetWidth + 2, _turninfomationRow, _turnInfomationWidth)

    End Sub

    'allows players to take turns until a final accuasation has been made
    Private Sub PlayGame()

        Dim currentPlayer As Integer

        Do

            DisplayState(_players(currentPlayer))

            ReadKey()

            HaveTurn(_players(currentPlayer))

            RecieveFinalAccusations()

            'loops through the players
            If currentPlayer = _players.Count - 1 Then

                currentPlayer = 0

            Else

                currentPlayer = currentPlayer + 1

            End If

        Loop Until _finalAccusation IsNot Nothing

    End Sub

    'plays a turn of the game for a player
    Private Sub HaveTurn(currentPlayer As Player)

        'if that player is in a room
        If currentPlayer.GetInRoom() = True Then

            'ask if they want to move
            If currentPlayer.GetMoveFromRoomChoice() = True Then

                DisplayState(currentPlayer)

                'if they do move
                currentPlayer.Move(RollDice(), _gameBoard, _rooms)

                'if they reach a room guess
                If currentPlayer.GetInRoom() = True Then

                    MakeGuess(currentPlayer)

                End If

            Else

                'if they dont move guess

                MakeGuess(currentPlayer)

            End If

            'if they arent in room
        Else

            'then move
            currentPlayer.Move(RollDice(), _gameBoard, _rooms)

            'if they reach a room guess
            If currentPlayer.GetInRoom() = True Then

                MakeGuess(currentPlayer)

            End If

        End If

    End Sub

    'rolls two dice and adds up their scores
    Private Function RollDice() As Integer

        Dim randomiser As New Random
        Dim dice1, dice2, diceRoll As Integer
        Dim input As ConsoleKeyInfo
        Const diceMin = 1
        Const diceMax = 7

        Do

            TurnInfomation.DisplayPressEnterMessage()
            input = ReadKey()

            If input.Key = ConsoleKey.Enter Then

                'rolls two dice
                dice1 = randomiser.Next(diceMin, diceMax)
                dice2 = randomiser.Next(diceMin, diceMax)

                diceRoll = dice1 + dice2

                TurnInfomation.DisplayRoll(diceRoll)
                'WriteLine("You rolled a " + diceRoll.ToString)

                Return diceRoll

            End If

        Loop

    End Function

    'allows the current player to make a guess and then recieves a response
    Private Sub MakeGuess(currentPlayer As Player)

        Dim guess As Guess

        'gets the guess
        guess = currentPlayer.MakeGuess(_suspects, _weapons, _rooms)

        Dim nextPlayer, counter As Integer
        Dim cardRevealed As Boolean = False
        Dim revealedCard As Card

        nextPlayer = currentPlayer.GetPlayerNumber

        'displays everything
        DisplayState(currentPlayer)
        DisplayGuess(guess)
        ReadKey()

        Do

            'goes back to the start of the list
            If nextPlayer = _players.Count Then

                nextPlayer = 1

                'goes the the next position of the list
            Else

                nextPlayer = nextPlayer + 1

            End If

            'gets the response of the person in the circle
            revealedCard = _players(nextPlayer - 1).RevealCard(guess)

            ' DisplayState(currentPlayer)

            'if they have revealed a card
            If revealedCard IsNot Nothing Then

                cardRevealed = True
                FillInCardRevealed(currentPlayer, nextPlayer, guess, revealedCard)

                'if they havent revealed a card
            Else

                FillInCardNotRevealed(nextPlayer, guess)

            End If

            DisplayRevealedResponse(cardRevealed, nextPlayer, currentPlayer, revealedCard, counter)

            'how many people you have asked
            counter = counter + 1

        Loop Until cardRevealed = True Or counter = _players.Count - 1

        ReadKey()

    End Sub

    'displays a message of if what and to who card has been revealed
    Private Sub DisplayRevealedResponse(cardRevealed As Boolean, askedPlayer As Integer, currentPlayer As Player, revealedCard As Card, counter As Integer)

        Dim revealer As String

        If askedPlayer = 1 Then

            revealer = "User"

        Else

            revealer = String.Format("Player {0}", askedPlayer)

        End If

        If cardRevealed = True Then

            TurnInfomation.CardRevealed(counter, revealer, revealedCard.GetName, currentPlayer.Getname)
            'WriteLine("{0} revealed the card {1} to {2}", askedPlayer, revealedCard.GetName, currentPlayer.Getname)

        Else

            TurnInfomation.CardNotRevealed(counter, revealer)
            'WriteLine("{0} cannot disprove the suggestion", revealer)

        End If

    End Sub

    'fills in the information gained for all players if a card is revealed
    Private Sub FillInCardRevealed(currentPlayer As Player, cardRevealer As Integer, guess As Guess, revealedCard As Card)

        cardRevealer = cardRevealer - 1

        For i = 0 To _players.Count - 1

            If _players(i) Is currentPlayer Then

                'fills in cluesheet for  current player
                For j = 0 To guess.GetGuess.Count - 1

                    If guess.GetGuess(j) Is revealedCard Then

                        _players(i).AddInHandCard(revealedCard, cardRevealer)

                    End If

                Next

                'fills in cluesheet for all other players
            ElseIf Not _players(i) Is _players(cardRevealer) Then

                For j = 0 To guess.GetGuess.Count - 1

                    _players(i).AddSuspectedInHandCard(guess.GetGuess(j), cardRevealer)

                Next

            End If

        Next

    End Sub

    'fills in the infomation gained for all players if a card is not revealed
    Private Sub FillInCardNotRevealed(cardRevealer As Integer, guess As Guess)

        cardRevealer = cardRevealer - 1

        For i = 0 To _players.Count - 1

            If Not _players(i) Is _players(cardRevealer) Then

                For j = 0 To guess.GetGuess.Count - 1

                    _players(i).AddNotInHandCard(guess.GetGuess(j), cardRevealer)

                Next

            End If

        Next

    End Sub

    'displays the players guess
    Private Sub DisplayGuess(guess As Guess)

        TurnInfomation.DisplayGuess(guess)

    End Sub

    'displays whos turn it currently is
    Private Sub DisplayTurn(currentPlayer As Player)

        TurnInfomation.DisplayTurn(currentPlayer.Getname)

    End Sub

    'displays the cards in the users hand
    Private Sub DisplayUserHand(user As Player)

        user.DisplayHand()

    End Sub

    'displays the full screeen including board cluesheet and messages
    Private Sub DisplayState(currentPlayer As Player)

        Clear()

        _gameBoard.DisplayBoard()

        DisplayTurn(currentPlayer)

        DisplayUserHand(_players(0))

        _players(0).GetClueSheet().DisplayClueSheet(_gameBoard.GetWidth + TurnInfomation.GetWidth + 4)

    End Sub

    'outputs the correct solution onto the screen 
    Private Sub DisplayCorrectSolution()

        TurnInfomation.DisplayCorrectSolution(_solution)

    End Sub

    'asks all players if they want to make a final accusation and gets confirmation
    Private Sub RecieveFinalAccusations()

        _finalAccusation = Nothing
        _finalAccuser = Nothing

        For i = 0 To _players.Count - 1

            'if they have chosen to make a final accusation
            If _players(i).GetMakeFinalAccusationChoice() = True Then

                _finalAccusation = _players(i).GetFinalAccusation(_suspects, _weapons, _rooms)

                'if they have confirmed
                If _finalAccusation IsNot Nothing Then

                    _finalAccuser = _players(i)

                    'displays the final accusation
                    DisplayState(_players(i))

                    TurnInfomation.DisplayFinalAccusation(_finalAccuser, _finalAccusation, _players.Count)

                    ReadKey()

                    Exit For

                Else

                    'allows then to accuse again
                    i = i - 1

                End If

            End If

        Next

    End Sub

    'checks if the final accusation made is the same as the solution
    Private Sub CheckFinalAccusationCorrect()

        Dim allCorrect As Boolean = False

        If _solution.GetGuessedSuspect Is _finalAccusation.GetGuessedSuspect And
                _solution.GetGuessedWeapon Is _finalAccusation.GetGuessedWeapon And
            _solution.GetGuessedRoom Is _finalAccusation.GetGuessedRoom Then

            allCorrect = True

        End If

        TurnInfomation.DisplayResult(allCorrect, _finalAccuser)

    End Sub

    'resets the variables for the next game
    Private Sub ClearGame()

        _gameBoard.ClearBoard()
        _players.Clear()
        _solution = New Guess

    End Sub

End Class
