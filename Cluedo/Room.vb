Public Class Room

    Inherits Card

    Private _doorKey As String
    Private _displayKey As Char
    Public Sub New(name As String, doorKey As String, displayKey As String)

        MyBase.New(name)

        _doorKey = doorKey
        _displayKey = displayKey

    End Sub

    'returns the rooms door key 
    Public Function GetDoorKey() As String

        Return _doorKey

    End Function

    'returns the rooms display key which indicates where the playerpieces will be displayed from inside the room
    Public Function GetDisplayKey() As Char

        Return _displayKey

    End Function


End Class
