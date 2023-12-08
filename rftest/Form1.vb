Imports PSXRFD
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ob As New OpenFileDialog
        ob.Filter = "BIN File (*.bin)|*.bin"
        ob.Title = "Choose a BIN file"
        If ob.ShowDialog = DialogResult.OK Then
            RichTextBox1.AppendText($"{PSXRFD.PSXRFD.IDFinder(ob.FileName)}{Environment.NewLine}")
        End If
    End Sub
End Class
