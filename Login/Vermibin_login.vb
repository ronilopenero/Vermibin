'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'Developed by: VermiTeam                                '
'Description:                                           '
'   This form creates a login system. Contained in this '
' part of the program is the login manager which help   '
' authenticate whether the current user is a valid user '
' and is responsible for calling the monitoring module  '
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''


Imports System.Data.SqlClient 'Imports the SQL Client so that SQL Commands can be used

Public Class Vermibin_login
    Dim ds As New DataSet 'Creates a dataset that handles data returned by SQL

    'Start of Form1_Load - Loads upon starting the program
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim adap As New SqlDataAdapter 'SQL Adapter that works with the dataset
        'Connect the program with the database
        Dim connection As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\PENERO\Documents\Visual Studio 2013\Projects\Vermibin_VB\Login\vermibin.mdf';Integrated Security=True;Connect Timeout=30")
        'Send command: SELECT * FROM LOGIN using the database stated in connection
        Dim command As SqlCommand = New SqlCommand("SELECT * FROM LOGIN;", connection)
        connection.Open() 'Opens the Connection
        adap.SelectCommand = command 'Selects the SQL command to use
        adap.Fill(ds, "0") 'fills the 1st table of the dataset ds with those returned from the SQL Command
        connection.Close() 'Closes the connection from the database
    End Sub
    'End of Form1_Load

    'Start of btn_login_Click - An event that triggers upon clicking the button named "btn_login"
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Dim count As Integer = 0
        'Checks whether the user exists or not and tells whether the user entered the password corresponding to that user
        While (count < ds.Tables("0").Rows.Count) 'While count is less than the number of rows in ds table 0, execute the succeeding codes
            'if the username and the password entered in the textbox = to the username and corresponding password in the database, then
            'execute the succeeding code
            If (tb_username.Text.Equals(ds.Tables("0").Rows(count).Item(0)) And tb_password.Text.Equals(ds.Tables("0").Rows(count).Item(1))) Then
                'output Login Successful
                MessageBox.Show("Login Successful")
                'Create an instance of monitoring_form with parameters entered in the tb_username.Text
                Dim monitoring_form As New Monitoring(tb_username.Text)
                monitoring_form.Show() 'Show the form
                'Clear the text. This is done so that when this form is shown again, the previous entry will not be present
                tb_password.Text = ""
                tb_username.Text = ""
                'Hide this form
                Me.Hide()
                'if the condition was not met, execute the succeeding code
            Else
                'output Invalid Employee_ID and/or Password!
                MessageBox.Show("Invalid Employee_ID and/or Password!")
            End If

            count += 1
        End While
    End Sub


End Class
