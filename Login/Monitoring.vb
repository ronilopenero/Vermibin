Imports System.Data.SqlClient
Imports System.IO

Public Class Monitoring
    Dim ds As New DataSet
    Dim user As String
    Dim temp, moisture As String
    Dim htmlfile As String
    
    Sub New(p1 As String)
        InitializeComponent()
        user = p1
            Timer1.Interval = 1000
            Timer1.Enabled = True

    End Sub



    Private Sub Monitoring_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim adap As New SqlDataAdapter
        Dim connection As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\PENERO\Documents\Visual Studio 2013\Projects\Vermibin_VB\Login\vermibin.mdf';Integrated Security=True;Connect Timeout=30")
        connection.Open()
        Dim prpdStatement As String = "SELECT * FROM USER_INFO WHERE EMPLOYEE_ID = '" + user + "'"
        Dim command As SqlCommand = New SqlCommand(prpdStatement, connection)
        adap.SelectCommand = command
        adap.Fill(ds, "0")
        lbl_name.Text = ds.Tables("0").Rows(0).Item(1)
        If (ds.Tables("0").Rows(0).Item(2).Equals("A")) Then
            lbl_privelege.Text = "Administrator"
        Else
            lbl_privelege.Text = "Standard User"
        End If

        command = New SqlCommand("SELECT * FROM EVENT", connection)
        adap.SelectCommand = command
        adap.Fill(ds, "1")

        With dg_Data
            .DataSource = ds.Tables("1")
            .AutoGenerateColumns = True

        End With

        command = New SqlCommand("SELECT * FROM EVENT", connection)
        adap.SelectCommand = command
        adap.Fill(ds, "2")
        '<--Chart Settings-->
        With c_temp
            .Series.Clear()
            .Series.Add("Temperature")
            .Series.Add("Moisture")
            .DataSource = ds.Tables("2")
            .Series(0).XValueMember = "LOG_ID"
            .Series(0).YValueMembers = "TEMPERATURE"
            .Series(1).XValueMember = "LOG_ID"
            .Series(1).YValueMembers = "MOISTURE"
        End With
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Me.Close()
        Vermibin_login.Show()
    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click
        Me.Refresh()
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click
        Me.Refresh()
    End Sub

    Private Sub AsPDFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsPDFToolStripMenuItem.Click
        Dim _output_menu As New Output
        _output_menu.Show()
    End Sub

    Private Sub AddUsersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddUsersToolStripMenuItem.Click
        If (lbl_privelege.Text.Equals("Administrator")) Then
            MessageBox.Show("")
        Else
            MessageBox.Show("Standard users not allowed to add new users.")
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim read As StreamReader = New StreamReader("D:\cpe development tools\xampp\htdocs\VermiWebsite\vermi.html")
        Dim line As String
        Dim temp As String = String.Empty
        Dim moist As String = String.Empty

readAgaintemp:
        line = read.ReadLine()
        If (line.Contains("TemperatureDisplay")) Then
            For i = 0 To line.Length
                If (line.ElementAt(i) = ">") Then
                    For j = i + 1 To line.Length
                        If (line.ElementAt(j) = "<") Then
                            GoTo exitlooptemp
                        Else
                            temp += line.ElementAt(j)
                        End If
                    Next
                End If
            Next

        Else
            GoTo readAgaintemp
        End If
exitlooptemp:
        MessageBox.Show(temp)
        dg_Data.Refresh()
    End Sub
End Class


