Imports System.Console
Public Class BoardLoader

    'ensures that the file attempting to be loaded exists
    Private Function CheckIfFileExists(fileName As String) As Boolean

        If Not IO.File.Exists(fileName) Then

            WriteLine("File {0} does not exist", fileName)
            Return False

        End If

        Return True

    End Function

    'returns all the characters in the baord file added to a 2d array of characters
    Public Function LoadBoard(fileName As String, ByRef width As Integer, ByRef height As Integer) As Char(,)

        If CheckIfFileExists(fileName) = True Then

            Dim fileToLoad As New IO.StreamReader(fileName)
            Dim dimensions As New List(Of Integer)
            Dim lines As String()

            Try

                lines = IO.File.ReadAllLines(fileName)
                height = lines.Count - 1
                width = lines(0).Length - 1

                Dim boardArray(width, height) As Char

                For i = 0 To height

                    For j = 0 To width

                        boardArray(j, i) = lines(i)(j)

                    Next

                Next

                Return boardArray

            Catch theError As Exception

                'outputs the respective error message
                WriteLine("Error reading file {0} : {1}", fileName, theError.Message)

            End Try

        End If

        ReadKey()
        Return Nothing

    End Function

End Class
