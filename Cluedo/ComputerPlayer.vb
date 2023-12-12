Public Class ComputerPlayer

    Inherits Player

    Protected _targetSquare As Square
    Protected _correctRoomFound As Boolean
    Protected _solution As New Guess

    Sub New(character As Suspect, pieceSymbol As Char, playerNumber As Integer)

        MyBase.New(character, pieceSymbol, playerNumber)

    End Sub

    'allows the player to move accrosss its desired path on the board
    Public Overrides Sub Move(diceRoll As Integer, board As Board, rooms As List(Of Room))

        Dim pathSquares As List(Of Square)
        Dim roomsToVisit As List(Of Room)
        Dim moves As Integer

        roomsToVisit = GetRoomsNotInHands(rooms)
        pathSquares = ChoosePath(board, roomsToVisit)

        If pathSquares Is Nothing Then

            Return

        End If

        'reverses the path so it starts at the current square
        pathSquares.Reverse()

        moves = 1

        Do

            _currentSquare.UnSetPlayer()
            pathSquares(moves).SetPlayer(Me)
            board.DisplayBoard()

            Threading.Thread.Sleep(400)

            'makes sure you are moving into room not out 
            If pathSquares(moves).GetDoor = True And (pathSquares(moves).GetSymbol = pathSquares(pathSquares.Count - 1).GetSymbol Or TypeOf pathSquares(moves - 1) Is CorridorSquare) Then

                _currentRoom = pathSquares(moves).GetRoom

                MovePieceIntoRoom(board)

                Exit Do

            Else

                _currentRoom = Nothing

            End If

            TurnInfomation.DisplayMoves(diceRoll - 1)

            diceRoll = diceRoll - IncreaseMoves(pathSquares(moves))

            moves = moves + 1

        Loop Until diceRoll = 0

    End Sub

    Protected Overridable Function ChoosePath(board As Board, roomsToVisit As List(Of Room)) As List(Of Square)

        Dim visited(board.GetWidth, board.GetHeight) As Boolean
        _targetSquare = GetTargetSquare(board, roomsToVisit)

        Return PathFind(_currentSquare, _targetSquare, board, visited)

    End Function

    'gets the target square for the pathfinder
    Protected Overridable Function GetTargetSquare(board As Board, roomsToVisit As List(Of Room)) As Square

        'testing
        Return board.GetBoardSquares(27, 14)

    End Function

    'gets a list of the rooms without known owners
    Protected Function GetRoomsNotInHands(rooms As List(Of Room)) As List(Of Room)

        Dim inHand As List(Of Card)
        Dim knownRooms As New List(Of Room)
        Dim unknownRooms As New List(Of Room)

        'goes through all the opponents
        For i = 0 To _opponents.Count - 1

            'gets the cards they are known to have
            inHand = _opponents(i).GetCardsInHand()

            For j = 0 To inHand.Count - 1

                If TypeOf inHand(j) Is Room Then

                    knownRooms.Add(inHand(j))

                End If

            Next

        Next

        'if the room is not known it is unknown
        For i = 0 To rooms.Count - 1

            If Not knownRooms.Contains(rooms(i)) Then

                unknownRooms.Add(rooms(i))

            End If

        Next

        Return unknownRooms

    End Function

    'gets a path of squares from to target sqaure back to the startsquare
    Protected Function PathFind(currentsquare As Square, targetsquare As Square, board As Board, ByRef visited(,) As Boolean) As List(Of Square)

        Dim oldX, oldY As Integer
        Dim squareToMoveTo As Boolean
        Dim newSquare As Square
        Dim squares As List(Of Square)
        Dim pathSquares As New List(Of Square)

        oldX = currentsquare.GetX
        oldY = currentsquare.GetY

        visited(oldX, oldY) = True

        'loops through each moveintoable adjacent square
        Do

            squareToMoveTo = False

            'if reached destination then start returning the used path
            If oldX = targetsquare.GetX And oldY = targetsquare.GetY Then

                pathSquares.Add(targetsquare)
                Return pathSquares

            Else

                'checks the squares adjacent and gets a list of if they could be in the path or not
                squares = CheckAdjacentSqures(currentsquare, visited, board)

                For i = 0 To squares.Count - 1

                    'if one of the adjacent squares could be in the path 
                    If squares(i) IsNot Nothing Then

                        squareToMoveTo = True

                    End If

                Next

                If squareToMoveTo = True Then

                    'chooses a square to check next
                    newSquare = ChooseSquareToMoveTo(currentsquare, targetsquare, squares)
                    'do recursion on chosen square
                    pathSquares = PathFind(newSquare, targetsquare, board, visited)

                    'if part of the found path
                    If pathSquares IsNot Nothing Then

                        'add to path and return
                        pathSquares.Add(currentsquare)
                        Return pathSquares

                    End If

                Else

                    'backtrack
                    Return Nothing

                End If

            End If

        Loop

    End Function

    'checks if that square could be used
    Private Function GetIfSquareAvailable(newSquare As Square, visited(,) As Boolean) As Boolean

        'if out of bounds
        If newSquare Is Nothing Then

            Return False

            'if cant be landed on
        ElseIf newSquare.GetCanlandOn = False Then

            Return False

            'if already visited
        ElseIf visited(newSquare.GetX(), newSquare.GetY()) = True Then

            Return False

        ElseIf newSquare.HasPlayer = True Then

            Return False

        End If

        If GetIfSquareMoveable(newSquare) = False Then

            Return False

        End If

        If visited(newSquare.GetX(), newSquare.GetY()) = True Then

            Return False

        End If

        Return True

    End Function

    'checks all the squares adjacent to the passed in square
    Private Function CheckAdjacentSqures(startsquare As Square, visited(,) As Boolean, board As Board) As List(Of Square)

        Dim squares As New List(Of Square)
        Dim squareUp, squareDown, squareLeft, squareRight As Square

        'get the adjacent squares
        squareUp = board.GetSquareUp(startsquare)
        squareDown = board.GetSquareDown(startsquare)
        squareLeft = board.GetSquareLeft(startsquare)
        squareRight = board.GetSquareRight(startsquare)

        'checks the square above
        If GetIfSquareAvailable(squareUp, visited) = True Then

            squares.Add(squareUp)

        Else

            squares.Add(Nothing)

        End If

        'checks the square below
        If GetIfSquareAvailable(squareDown, visited) = True Then

            squares.Add(squareDown)

        Else

            squares.Add(Nothing)

        End If

        'checks the sqaure left
        If GetIfSquareAvailable(squareLeft, visited) = True Then

            squares.Add(squareLeft)

        Else

            squares.Add(Nothing)

        End If

        'checks the square right
        If GetIfSquareAvailable(squareRight, visited) = True Then

            squares.Add(squareRight)

        Else

            squares.Add(Nothing)

        End If

        Return squares

    End Function

    'chooses a logical squareto move to next based on wherer it is trying to go
    Private Function ChooseSquareToMoveTo(startsqaure As Square, targetsquare As Square, squares As List(Of Square)) As Square

        Dim differenceInX, differenceInY, magnitudeX, magnitudeY As Integer

        differenceInX = startsqaure.GetX - targetsquare.GetX
        differenceInY = startsqaure.GetY - targetsquare.GetY

        magnitudeX = GetMagnitude(differenceInX)
        magnitudeY = GetMagnitude(differenceInY)

        'if need to priotise Y
        If magnitudeX < magnitudeY Then

            'if needs to move down 
            If differenceInY < 0 Then

                'try down
                If squares(1) IsNot Nothing Then

                    Return squares(1)

                    'cant move down
                Else

                    'needs to move right
                    If differenceInX < 0 Then

                        'try right first
                        If squares(3) IsNot Nothing Then

                            Return squares(3)

                            'try left
                        ElseIf squares(2) IsNot Nothing Then

                            Return squares(2)

                        End If

                        'try left first
                    Else

                        If squares(2) IsNot Nothing Then

                            Return squares(2)

                            'try right
                        ElseIf squares(3) IsNot Nothing Then

                            Return squares(3)

                        End If

                    End If

                End If

                'try up
                If squares(0) IsNot Nothing Then

                    Return squares(0)

                End If

                'needs to move up
            Else

                'try up
                If squares(0) IsNot Nothing Then

                    Return squares(0)

                    'cant move up
                Else

                    'needs to move right
                    If differenceInX < 0 Then

                        'try right first
                        If squares(3) IsNot Nothing Then

                            Return squares(3)

                            'try left
                        ElseIf squares(2) IsNot Nothing Then

                            Return squares(2)

                        End If

                        'try left first
                    Else

                        If squares(2) IsNot Nothing Then

                            Return squares(2)

                            'try right
                        ElseIf squares(3) IsNot Nothing Then

                            Return squares(3)

                        End If

                    End If

                End If

                'try down
                If squares(1) IsNot Nothing Then

                    Return squares(1)

                End If

            End If

            'prioitise x
        Else

            'needs to move right
            If differenceInX < 0 Then

                'try right
                If squares(3) IsNot Nothing Then

                    Return squares(3)

                    'cant move right
                Else

                    'needs to move down
                    If differenceInY < 0 Then

                        'try down first
                        If squares(1) IsNot Nothing Then

                            Return squares(1)

                            'try up
                        ElseIf squares(0) IsNot Nothing Then

                            Return squares(0)

                        End If

                        'try up first
                    Else

                        If squares(0) IsNot Nothing Then

                            Return squares(0)

                            'try down
                        ElseIf squares(1) IsNot Nothing Then

                            Return squares(1)

                        End If

                    End If

                    'try left
                    If squares(2) IsNot Nothing Then

                        Return squares(2)

                    End If

                End If

                'needs to move left
            Else

                'try left
                If squares(2) IsNot Nothing Then

                    Return squares(2)

                    'cant move right
                Else

                    'needs to move down
                    If differenceInY < 0 Then

                        'try down first
                        If squares(1) IsNot Nothing Then

                            Return squares(1)

                            'try up
                        ElseIf squares(0) IsNot Nothing Then

                            Return squares(0)

                        End If

                        'try up first
                    Else

                        If squares(0) IsNot Nothing Then

                            Return squares(0)

                            'try down
                        ElseIf squares(1) IsNot Nothing Then

                            Return squares(1)

                        End If

                    End If

                    'try right
                    If squares(3) IsNot Nothing Then

                        Return squares(3)

                    End If

                End If

            End If

        End If

        Return Nothing

    End Function

    'gets the magnitude of the distance
    Private Function GetMagnitude(number As Integer) As Integer

        If number < 0 Then

                number = number * -1

            End If

            Return number



    End Function

End Class
