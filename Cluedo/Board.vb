Imports System.Console
Public Class Board

    Private _boardSquares(,) As Square
    Private _width As Integer
    Private _height As Integer

    'returns the width of the board
    Public Function GetWidth() As Integer

        Return _width

    End Function

    'returns the height of the board
    Public Function GetHeight() As Integer

        Return _height

    End Function

    'returns the full board array
    Public Function GetBoardSquares() As Square(,)

        Return _boardSquares

    End Function

    'recives the loaded board and makes a square for each player and adds it to the array
    Public Sub LoadInBoard(rooms As List(Of Room), suspects As List(Of Suspect))

        Dim boardCharArray(,) As Char
        Dim boardFromFile As BoardLoader = New BoardLoader()
        Dim currentSquare As SquareMaker = New SquareMaker()

        boardCharArray = boardFromFile.LoadBoard("Board.txt", _width, _height)
        _boardSquares = New Square(_width, _height) {}

        For y = 0 To _height

            For x = 0 To _width

                _boardSquares(x, y) = currentSquare.MakeSquare(boardCharArray(x, y), GetRoomDisplayKeys(rooms), GetRoomDoorKeys(rooms), GetStartPositions(suspects))
                _boardSquares(x, y).SetPosition(x, y)
                _boardSquares(x, y).SetRoom(rooms)

            Next

        Next

    End Sub

    'goes through the board and if the squre is a start square then it 
    Public Sub MakePieces(players As List(Of Player))

        Dim found As Boolean

        'goes through each player
        For k = 0 To players.Count - 1

            found = False

            For y = 0 To _height

                For x = 0 To _width

                    'if the square is a starting position 
                    If TypeOf _boardSquares(x, y) Is StartingSquare Then

                        'if the starting position belongs to that player
                        If _boardSquares(x, y).GetSymbol = players(k).GetCharacter.GetStartPosition Then

                            'make piece
                            _boardSquares(x, y).SetPlayer(players(k))
                            found = True

                            'stop looking
                            Exit For

                        End If


                    End If

                Next

                'stop looking
                If found = True Then

                    Exit For

                End If

            Next

        Next

    End Sub

    'returns a list of the keys of where the suspects in the rooms will be displayed from
    Private Function GetRoomDisplayKeys(rooms As List(Of Room)) As List(Of Char)

        Dim roomDisplayKeys As New List(Of Char)
        For i = 0 To rooms.Count - 1

            roomDisplayKeys.Add(rooms(i).GetDisplayKey)

        Next

        Return roomDisplayKeys

    End Function

    'returns a list of keys that represent the doors
    Private Function GetRoomDoorKeys(rooms As List(Of Room)) As List(Of Char)

        Dim roomDoorKeys As New List(Of Char)
        For i = 0 To rooms.Count - 1

            roomDoorKeys.Add(rooms(i).GetDoorKey)

        Next

        Return roomDoorKeys

    End Function

    'returns a list of the player start positions
    Private Function GetStartPositions(suspects As List(Of Suspect)) As List(Of Char)

        Dim startPositions As New List(Of Char)

        For i = 0 To suspects.Count - 1

            startPositions.Add(suspects(i).GetStartPosition)

        Next

        Return startPositions

    End Function

    'goes the the board and writes out aeach squares display character (overlays, player pieces)
    Public Sub DisplayBoard()

        OutputEncoding = Text.Encoding.UTF8

        For y = 0 To _height

            For x = 0 To _width

                _boardSquares(x, y).DisplaySquare()

            Next

        Next

    End Sub

    'gets the square to the left of the passed in square if it is within the bounds of the board
    Public Function GetSquareLeft(square As Square) As Square

        Dim x = square.GetX
        Dim y = square.GetY

        If x = 0 Then

            Return Nothing

        End If

        x = x - 1
        Return _boardSquares(x, y)

    End Function

    'gets the square to the right of the passed in square if it is within the bounds of the board
    Public Function GetSquareRight(square As Square) As Square

        Dim x = square.GetX
        Dim y = square.GetY

        If y = _height Then

            Return Nothing

        End If

        x = x + 1
        Return _boardSquares(x, y)

    End Function

    'gets the square up from the passed in square if it is within the bounds of the board
    Public Function GetSquareUp(square As Square) As Square

        Dim x = square.GetX
        Dim y = square.GetY

        If y = 0 Then

            Return Nothing

        End If

        y = y - 1
        Return _boardSquares(x, y)

    End Function

    'gets the square down from the passed in square if it is within the bounds of the board
    Public Function GetSquareDown(square As Square) As Square

        Dim x = square.GetX()
        Dim y = square.GetY()

        If y = _width Then

            Return Nothing

        End If

        y = y + 1
        Return _boardSquares(x, y)

    End Function

    'gets the all positions of a rooms doors
    Public Function GetDoorPositions(room As Room) As List(Of Square)

        Dim doors As New List(Of Square)

        For y = 0 To _height

            For x = 0 To _width

                'if the square is a door
                If TypeOf _boardSquares(x, y) Is DoorSquare Then

                    'if the door belongs to the room
                    If _boardSquares(x, y).GetSymbol = room.GetDoorKey Then

                        'add door
                        doors.Add(_boardSquares(x, y))

                    End If

                End If

            Next

        Next

        Return doors

    End Function

    'gets the all positions of a rooms display key
    Public Function GetRoomDisplayPosition(room As Room) As Square

        Dim displayKey As Square = Nothing

        For y = 0 To _height

            For x = 0 To _width

                'if the square is a displaysquare
                If TypeOf _boardSquares(x, y) Is DisplayPieceSquare Then

                    'if the displaykey belongs to the room
                    If _boardSquares(x, y).GetSymbol = room.GetDisplayKey Then

                        'add door
                        displayKey = _boardSquares(x, y)

                    End If

                End If

            Next

        Next

        Return displayKey

    End Function

    'removes the player pieces from the board 
    Public Sub ClearBoard()

        For y = 0 To _height

            For x = 0 To _width

                If _boardSquares(x, y).HasPlayer = True Then

                    _boardSquares(x, y).UnSetPlayer()

                End If

            Next

        Next

    End Sub

End Class
