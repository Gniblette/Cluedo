Imports System.Console
Public Class ClueSheet

    Private _rows As List(Of String)
    Private _suspects As List(Of Suspect)
    Private _weapons As List(Of Weapon)
    Private _rooms As List(Of Room)
    Private _opponents As List(Of Opponent)

    Const _rowHeadingLength As Integer = 15
    Const _suspectTitle As String = "Who?"
    Const _weaponTitle As String = "What?"
    Const _roomTitle As String = "Where?"

    Public Sub New(suspects As List(Of Suspect), weapons As List(Of Weapon), rooms As List(Of Room), opponents As List(Of Opponent))

        _suspects = suspects
        _weapons = weapons
        _rooms = rooms
        _opponents = opponents

    End Sub

    'makes all the row headers the same length
    Public Function GetDisplayContents(contents As String, charlength As Integer)

        Dim displayContents As String = contents

        For i = 0 To charlength - contents.Length

            displayContents = displayContents + " "

        Next

        Return displayContents

    End Function

    'updates the rows of the cluesheet with the new symbols
    Private Sub UpdateRows()


        _rows = New List(Of String)

        'creates the top now of the table inclueding all the column headings
        _rows.Add(GetDisplayContents("Players:", _rowHeadingLength) + MakeColumns())

        _rows.Add(GetDisplayContents(_suspectTitle, _rowHeadingLength))

        'creates all the suspect rows with symbols
        For i = 0 To _suspects.Count - 1

            _rows.Add(GetDisplayContents(_suspects(i).GetName, _rowHeadingLength) + GetOpponentSymbols(_suspects(i)))

        Next

        _rows.Add(GetDisplayContents(_weaponTitle, _rowHeadingLength))

        'creates all the weapon rows with symbols
        For i = 0 To _weapons.Count - 1

            _rows.Add(GetDisplayContents(_weapons(i).GetName, _rowHeadingLength) + GetOpponentSymbols(_weapons(i)))

        Next

        _rows.Add(GetDisplayContents(_roomTitle, _rowHeadingLength))

        'creates all the room rows with symbols
        For i = 0 To _rooms.Count - 1

            _rows.Add(GetDisplayContents(_rooms(i).GetName, _rowHeadingLength) + GetOpponentSymbols(_rooms(i)))

        Next

    End Sub

    'adds the player numbers to the top row of the cluesheet
    Public Function MakeColumns() As String

        Dim columns As String = ""

        For i = 0 To _opponents.Count - 1

            columns = columns + (i + 1).ToString + " "

        Next

        Return columns

    End Function

    'gets the approperiate symbol for each opponent for a given card and adds the to a string
    Public Function GetOpponentSymbols(card As Card) As String

        Dim solved As String = ""
        Dim inHand As String = ""
        Dim columns As String = ""

        For i = 0 To _opponents.Count - 1

            columns = columns + _opponents(i).GetSymbol(card) + " "
            solved = solved + _opponents(i).GetSolvedSymbol + " "
            inHand = inHand + _opponents(i).GetNotInHandSymbol + " "

        Next

        If columns = inHand Then

            Return solved

        End If

        Return columns

    End Function

    'goes through each row of the updated cluesheet and writes it
    Public Sub DisplayClueSheet(offset As Integer)

        UpdateRows()

        Dim titles As New List(Of String)

        titles.Add(_suspectTitle.PadRight(_rowHeadingLength + 1))
        titles.Add(_weaponTitle.PadRight(_rowHeadingLength + 1))
        titles.Add(_roomTitle.PadRight(_rowHeadingLength + 1))

        SetCursorPosition(offset, 0)
        Write("------ Clue Sheet ------")

        For i = 0 To _rows.Count - 1

            SetCursorPosition(offset, i + 2)

            If titles.Contains(_rows(i)) Then

                Highlight()

                'highlights until the end of the columns
                Write(_rows(i).PadRight(_rowHeadingLength + 1 + 2 * _opponents.Count))

                StopHighlight()

            Else

                Write(_rows(i))

            End If

        Next

    End Sub

    'highlights the option the user is on in white
    Private Sub Highlight()

        BackgroundColor = ConsoleColor.White
        ForegroundColor = ConsoleColor.Black

    End Sub

    'stops highlighting the options the user isnt on
    Private Sub StopHighlight()

        BackgroundColor = ConsoleColor.Black
        ForegroundColor = ConsoleColor.White

    End Sub

End Class
