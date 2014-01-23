Imports System.Data.SqlClient 'Imports the SQL Client so that SQL Commands can be used
Imports System.IO

Public Class Monitoring

    'Start of Global Variables
    Dim ds As New DataSet 'Instantiates a Dataset
    Dim user As String 'holds the value of the current user
    Dim temp, moisture As String 'Catches the temperature and moisture
    Dim htmlfile As String
    'End of Global Variables

    Sub New(p1 As String) 'Creates a constructor
        InitializeComponent() 'Initializes all the components
        user = p1 'Assigns the value received from the previous form to p1 and p1 to user
        Timer1.Interval = 1000 'Sets an interval of 1 sec for Timer1 which is a timer object
        'This will be used for the timer tick event which is executed per timer interval
        Timer1.Enabled = True ' this enables the timer
    End Sub
    'End of constructor

    'Start of Monitoring_Load which is executed everytime the form starts
    Private Sub Monitoring_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim adap As New SqlDataAdapter 'SQL Adapter that works with the dataset
        'Connect the program with the database
        Dim connection As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\PENERO\Documents\Visual Studio 2013\Projects\Vermibin_VB\Login\vermibin.mdf';Integrated Security=True;Connect Timeout=30")
        connection.Open() ' Opens the connection
        'Makes a prepared statement which is a query with variable values
        Dim prpdStatement As String = "SELECT * FROM USER_INFO WHERE EMPLOYEE_ID = '" + user + "'"
        'Send command: SELECT * FROM LOGIN using the database stated in connection
        Dim command As SqlCommand = New SqlCommand(prpdStatement, connection)
        adap.SelectCommand = command 'executes the SQL command which is the prepared statement made
        adap.Fill(ds, "0") 'fills the dataset ds table 0
        lbl_name.Text = ds.Tables("0").Rows(0).Item(1) 'Replaces the value of the label lbl_name into the user's name
        If (ds.Tables("0").Rows(0).Item(2).Equals("A")) Then
            lbl_privelege.Text = "Administrator" 'Replaces the value of the label lbl_privelege into Administrator if it's
            'value in the database is A
        Else
            lbl_privelege.Text = "Standard User" 'Replaces the value of the label lbl_privelege into Standard User if it's
            'value in the database is not A
        End If

        command = New SqlCommand("SELECT * FROM EVENT", connection) 'makes a new SQL Command that selects all value of event
        adap.SelectCommand = command 'execute the command
        adap.Fill(ds, "1") 'fill the dataset ds table 1

        With dg_Data 'denotes that all properties belong to dg_Data
            .DataSource = ds.Tables("1") 'sets the data source to table 1 of ds
            .AutoGenerateColumns = True 'auto generate columns to fill the entire data grid view
        End With

        command = New SqlCommand("SELECT * FROM EVENT", connection) 'makes a new SQL Command that selects all value of event
        adap.SelectCommand = command 'execute the command
        adap.Fill(ds, "2") 'fill the dataset ds table 2

        'Start of Chart Settings
        With c_temp
            .Series.Clear() 'Clears the entire series of data
            .Series.Add("Temperature") 'Adds the series named temperature
            .Series.Add("Moisture") 'Adds  the series named moisture
            .DataSource = ds.Tables("2") 'sets the source of the data as table 2 of ds
            .Series(0).XValueMember = "LOG_ID" 'sets the x-axis of series 0 as the Log ID
            .Series(0).YValueMembers = "TEMPERATURE" 'sets the y-axis of series 0 as the temperature
            .Series(1).XValueMember = "LOG_ID" 'sets the x-axis of series 1 as the Log ID
            .Series(1).YValueMembers = "MOISTURE" 'sets the y-axis of series 1 as the moisture
        End With
        'End of Chart Settings
    End Sub
    'End of Monitoring_Load

    'Start of LogoutToolStripMenuItem_Click - Event if Logout was pressed in the tool strip menu under file
    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Me.Close() 'Closes this form
        Vermibin_login.Show() 'shows the login menu
    End Sub
    'End of LogoutToolStripMenuItem_Click

    'Start of TabPage2_Click
    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click
        Me.Refresh() 'Refreshes the chart everytime the chart tab is clicked
    End Sub
    'End of TabPage2_Click

    'Start of TabPage1_Click
    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click
        Me.Refresh() 'Refreshes the data grid everytime the incoming data tab is clicked
    End Sub
    'End of TabPage1_Click


    Private Sub AsPDFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsPDFToolStripMenuItem.Click
    End Sub

    Private Sub AddUsersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddUsersToolStripMenuItem.Click
        If (lbl_privelege.Text.Equals("Administrator")) Then
            MessageBox.Show("")
        Else
            MessageBox.Show("Standard users not allowed to add new users.")
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

    End Sub
End Class


