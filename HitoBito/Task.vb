Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class Management
    Private Const PROCESS_TERMINATE As Integer = &H1

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function OpenProcess(ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function TerminateProcess(ByVal hProcess As IntPtr, ByVal uExitCode As UInteger) As Boolean
    End Function

    Public Shared Sub TerminateProcessByName(ByVal processName As String)
        Try
            Dim processes As Process() = Process.GetProcessesByName(processName)
            If processes.Length > 0 Then
                Dim processToTerminate As Process = processes(0)
                Dim processHandle As IntPtr = OpenProcess(PROCESS_TERMINATE, False, processToTerminate.Id)
                If processHandle <> IntPtr.Zero Then
                    Try
                        TerminateProcess(processHandle, 0)
                    Finally
                        CloseHandle(processHandle)
                    End Try
                Else
                End If
            Else
            End If
        Catch ex As Exception
        End Try
    End Sub

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CloseHandle(ByVal hObject As IntPtr) As Boolean
    End Function
End Class
