Imports System.Console

Public Class CSVLoader

    'gets a list of each row as an array from the requsted file 
    Public Function GetEntries(fileName As String) As List(Of String())

        If CheckIfFileExists(fileName) = True Then

            Return GetFileLines(fileName)

        End If

        Return Nothing

    End Function

    'ensures that the file attempting to be loaded exists
    Private Function CheckIfFileExists(fileName As String) As Boolean

        If Not IO.File.Exists(fileName) Then

            WriteLine("File {0} does not exist", fileName)
            Return False

        End If

        Return True

    End Function

    'takes a cvs file and splits it up into a list of arrays 
    Private Function GetFileLines(fileName As String) As List(Of String())

        Dim line() As String
        Dim lines As New List(Of String())

        Try

            Dim fileToLoad As New IO.StreamReader(fileName)

            'reads through the entire file
            While fileToLoad.EndOfStream = False

                'adds each line of the file into an array split by commas
                line = fileToLoad.ReadLine.Split(",")
                'adds the array of the line to the list
                lines.Add(line)

            End While

            fileToLoad.Close()

        Catch theError As Exception

            'outputs the respective error message
            WriteLine("Error reading file {0} : {1}", fileName, theError.Message)
            Return Nothing

        End Try

        Return lines

    End Function

End Class










