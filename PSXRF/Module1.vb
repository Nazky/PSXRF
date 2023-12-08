﻿Imports System.IO
Imports System.Text.RegularExpressions

Module Module1
    Sub Main()
        If My.Application.CommandLineArgs.Count < 1 Then
            Console.WriteLine("Usage: PSXRF <FilePath>")
            Return
        End If

        Dim filePath As String = My.Application.CommandLineArgs(0)

        Dim matchingString As String = IDFinder(filePath)

        ' Check if the console is empty
        If Console.CursorLeft = 0 AndAlso Console.CursorTop = 0 Then
            Console.WriteLine("No matching string found in the binary file.")
        End If
    End Sub

    Function IDFinder(filePath As String) As String
        Dim result As String = Nothing

        Try
            Dim pattern As String = "(SCUS|SLES|SLUS|SCPS|SCKA|SCED|SCEL|SCAS|SCAA|SCAJ|SLPS)-\d{5}"
            Dim regex As New Regex(pattern)

            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Dim buffer(4096) As Byte
                Dim bytesRead As Integer
                Dim totalBytesRead As Long = 0
                Dim fileLength As Long = fs.Length
                Dim remainingText As String = String.Empty

                Do
                    bytesRead = fs.Read(buffer, 0, buffer.Length)
                    totalBytesRead += bytesRead

                    Dim text As String = remainingText & System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead)
                    remainingText = String.Empty

                    Dim matches As MatchCollection = regex.Matches(text)
                    If matches.Count > 0 Then
                        Dim lastIndex As Integer = matches(matches.Count - 1).Index + matches(matches.Count - 1).Length
                        remainingText = text.Substring(lastIndex)

                        For Each match As Match In matches
                            Dim matchValue As String = match.Value
                            If result Is Nothing Then
                                result = matchValue
                                Console.WriteLine($"Found: {result}")
                            End If
                        Next
                    End If

                    If result IsNot Nothing Then
                        Exit Do
                    End If

                Loop While bytesRead > 0
            End Using

        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Return result
    End Function
End Module