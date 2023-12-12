Public Class ComputerHardPlayer

    Inherits ComputerMediumPlayer

    Public Sub New(character As Suspect, PieceSymbol As Char, playerNumber As Integer)

        MyBase.New(character, PieceSymbol, playerNumber)

    End Sub

    '    'chooses a path to the random door of the closest room that is desired to visit 
    '    Protected Overrides Function ChoosePath(board As Board, roomsToVisit As List(Of Room)) As List(Of Square)

    '        Dim doors As List(Of Square)
    '        Dim visited(board.GetWidth, board.GetHeight) As Boolean
    '        Dim path As List(Of Square) = Nothing
    '        Dim tempPath As List(Of Square)

    '        For i = 0 To roomsToVisit.Count - 1

    '            doors = board.GetDoorPositions(roomsToVisit(i))

    '            For j = 0 To doors.Count - 1

    '                tempPath = PathFind(_currentSquare, doors(i), board, visited)

    '                If tempPath IsNot Nothing Then

    '                    If i = 0 Then

    '                        path = tempPath

    '                    ElseIf tempPath.Count < path.Count Then

    '                        path = tempPath

    '                    End If

    '                End If

    '            Next

    '        Next

    '        Return path

    '    End Function


End Class
