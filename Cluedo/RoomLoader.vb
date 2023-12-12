Public Class RoomLoader

    'loads the rooms from their file
    Public Function LoadRooms(fileName As String) As List(Of Room)

        Dim lines As List(Of String())
        Dim reader As CSVLoader = New CSVLoader
        Dim room As Room
        Dim rooms As New List(Of Room)

        lines = reader.GetEntries(fileName)

        If lines.Count = 0 Then

            Return Nothing

        End If

        For i = 1 To lines.Count - 1

            room = New Room(lines(i)(0), lines(i)(1), lines(i)(2))
            rooms.Add(room)

        Next

        Return rooms

    End Function

End Class













