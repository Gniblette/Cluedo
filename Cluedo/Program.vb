Imports System.Console

Module Cluedo
    Sub Main()

        CursorVisible = False
        Title = "Cluedo"

        Dim newGame As Game = New Game(4)
        newGame.SetupGame()

        WriteLine("Thank you for playing :)")
        ReadKey()

    End Sub

End Module
