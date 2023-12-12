Imports System.Console
Public Class Guess

    ' Private _guess As New List(Of Card)
    Private _guess(2) As Card
    Private _guessedSuspect As Suspect
    Private _guessedWeapon As Weapon
    Private _guessedRoom As Room

    Public Sub New() 'suspect As Suspect, weapon As Weapon, room As Room)

        _guessedSuspect = Nothing
        _guessedWeapon = Nothing
        _guessedRoom = Nothing

        '_guess.AddRange({suspect, weapon, room})

    End Sub

    'returns the suggested suspect for that guess
    Public Function GetGuessedSuspect() As Suspect

        Return _guessedSuspect

    End Function

    'returns the suggested weapon for that guess
    Public Function GetGuessedWeapon() As Weapon

        Return _guessedWeapon

    End Function

    'returns the suggested room for that guess
    Public Function GetGuessedRoom() As Room

        Return _guessedRoom

    End Function

    'returns the guess
    Public Function GetGuess() As Card()

        Return _guess

    End Function

    'adds a suspect to the guess
    Public Sub AddSuspect(suspect As Suspect)

        _guessedSuspect = suspect
        _guess(0) = _guessedSuspect

    End Sub

    'adds a weapon to the guess
    Public Sub AddWeapon(weapon As Weapon)

        _guessedWeapon = weapon
        _guess(1) = _guessedWeapon

    End Sub

    'adds a room to the guess
    Public Sub AddRoom(room As Room)

        _guessedRoom = room
        _guess(2) = _guessedRoom

    End Sub

End Class
