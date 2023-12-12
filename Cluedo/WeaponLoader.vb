Public Class WeaponLoader

    'loads the weapons from their file
    Public Function LoadWeapons(fileName As String) As List(Of Weapon)

        Dim lines As List(Of String())
        Dim reader As CSVLoader = New CSVLoader
        Dim weapon As Weapon
        Dim weapons As New List(Of Weapon)

        lines = reader.GetEntries(fileName)

        If lines.Count = 0 Then

            Return Nothing

        End If

        For i = 1 To lines.Count - 1

            weapon = New Weapon(lines(i)(0))
            weapons.Add(weapon)

        Next

        Return weapons

    End Function

End Class










