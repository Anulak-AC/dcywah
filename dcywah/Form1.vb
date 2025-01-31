﻿
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports System.Threading.Tasks
Imports OpenQA.Selenium.Support.UI
Imports System.Net
Imports Newtonsoft
Public Class Form1
    ' Declare BackgroundWorker
    Private WithEvents backgroundWorker1 As New System.ComponentModel.BackgroundWorker

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        CloseAllChromeDrivers()
        'Dim startInfo As New ProcessStartInfo("cmd.exe")
        'startInfo.Arguments = "/c taskkill /f /im chromedriver.exe"
        'Process.Start(startInfo)
    End Sub
    Private Sub CloseAllChromeDrivers()
        Try
            ' Find all running ChromeDriver processes
            Dim chromeDriverProcesses() As Process = Process.GetProcessesByName("chromedriver")
            ' Iterate through each ChromeDriver process and kill it
            For Each chromeDriverProcess As Process In chromeDriverProcesses
                chromeDriverProcess.Kill()
            Next

            ' Optionally, you can also close all Chrome browser instances if needed
            ' Dim chromeProcesses() As Process = Process.GetProcessesByName("chrome")
            ' For Each chromeProcess As Process In chromeProcesses
            '     chromeProcess.Kill()
            ' Next

            MessageBox.Show("All ChromeDriver instances have been closed.")
        Catch ex As Exception
            MessageBox.Show("Error occurred while closing ChromeDriver instances: " & ex.Message)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Check current time
        Dim currentTime As DateTime = DateTime.Now

        ' Check if current time is greater than 08:59:40
        If currentTime.TimeOfDay > TimeSpan.Parse("08:59:40") Then
            ' Update button text
            Timer1.Stop()
            Button1_Click(sender, e)

        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize BackgroundWorker
        backgroundWorker1.WorkerReportsProgress = True
        backgroundWorker1.WorkerSupportsCancellation = True
        Timer1.Interval = 1000
        Timer1.Start()

    End Sub
    ' BackgroundWorker DoWork event handler
    Private Sub backgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles backgroundWorker1.DoWork
        ' Move your code here that you want to run in the background
        ' This includes the code inside your Button1_Click event handler
        ' For example:
        '' Create a list to hold tasks for opening pages concurrently
        Dim tasks As New List(Of Task)()
        ' Create tasks to open each page and perform actions concurrently
        For loopX As Integer = 0 To 15
            Dim randomLocation As String = GenerateLocationInOrder(loopX) ' Generate a random location
            Dim task As Task = Task.Factory.StartNew(Sub()
                                                         OpenAndPerformAction(txtURL.Text, randomLocation)
                                                     End Sub)
            tasks.Add(task)
        Next
        ' Wait for all tasks to complete
        Task.WaitAll(tasks.ToArray())
    End Sub

    ' BackgroundWorker RunWorkerCompleted event handler
    Private Sub backgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles backgroundWorker1.RunWorkerCompleted
        ' Handle completion, perform any cleanup or UI updates
        ' For example:
        MessageBox.Show("Background tasks completed.")
    End Sub

    Dim urlxx As String = "file:///C:/Users/anulak/Downloads/WYC/%E0%B8%81%E0%B8%A3%E0%B8%A1%E0%B8%81%E0%B8%B4%E0%B8%88%E0%B8%81%E0%B8%B2%E0%B8%A3%E0%B9%80%E0%B8%94%E0%B9%87%E0%B8%81%E0%B9%81%E0%B8%A5%E0%B8%B0%E0%B9%80%E0%B8%A2%E0%B8%B2%E0%B8%A7%E0%B8%8A%E0%B8%99.html"
    ''http://dcywah.dcy.go.th/opp/app/register.php
    ' Set Chrome options to maximize the window
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Check if BackgroundWorker is not already busy
        If Not backgroundWorker1.IsBusy Then
            ' Start BackgroundWorker
            backgroundWorker1.RunWorkerAsync()
        End If
        '' Define the URLs to load
        '' Add more URLs as needed

        '' Create a list to hold tasks for opening pages concurrently
        'Dim tasks As New List(Of Task)()
        '' Create tasks to open each page and perform actions concurrently
        'For loopX As Integer = 0 To 5
        '    Dim randomLocation As String = GenerateLocationInOrder(loopX) ' Generate a random location
        '    Dim task As Task = Task.Factory.StartNew(Sub()
        '                                                 OpenAndPerformAction(txtURL.Text, randomLocation)
        '                                             End Sub)
        '    tasks.Add(task)
        'Next
        '' Wait for all tasks to complete
        'Task.WaitAll(tasks.ToArray())




    End Sub
    Private ReadOnly xCoordinates As Integer() = {0, 350, 700, 1000, 1200, 0, 350, 700, 1000, 1200, 0, 350, 700, 1000, 1200}
    Function GenerateLocationInOrder(ByVal index As Integer) As String
        ' Check if the index is within the bounds of the xCoordinates array
        If index >= 0 AndAlso index < xCoordinates.Length Then
            ' Get the X-coordinate from the array
            Dim xCoordinate As Integer = xCoordinates(index)

            ' Define the Y-coordinate (e.g., tail position)
            Dim yCoordinate As Integer = 0 ' Adjust as needed
            If index >= 5 And index <= 10 Then
                yCoordinate = 275
            ElseIf index >= 11 And index <= 15 Then
                yCoordinate = 500
            End If
            ' Return the formatted location string (X,Y)
            Return $"{xCoordinate},{yCoordinate}"
        Else
            ' Return an empty string or throw an exception, depending on your requirements
            Return ""
        End If
    End Function

    Private Function IsTimeGreaterThan859() As Boolean
        Dim result As Boolean = False

        ' Invoke the UI update operation on the main UI thread
        If Me.InvokeRequired Then
            Me.Invoke(Sub() result = CheckTimeGreaterThan859())
        Else
            result = CheckTimeGreaterThan859()
        End If

        Return result
    End Function

    Private Function CheckTimeGreaterThan859() As Boolean
        ' Get the current time
        Dim currentTime As DateTime = DateTime.Now
        ' Create a DateTime object representing 08:59:00
        Dim time859 As New DateTime(2024, 5, 30, 8, 59, 0)
        ' Compare the current time with 08:59:00
        If currentTime > time859 Then
            Return True
        Else
            Return False
        End If
    End Function




    ' Function to open a page and perform actions
    Sub OpenAndPerformAction(ByVal url As String, ByVal position As String)
        Dim rand As New Random()
        Threading.Thread.Sleep(rand.Next(500, 5000))
        Dim filePath As String = txtPath.Text ' "C:\Users\anulak\Downloads\green-hills-panorama-landscape-isolated-on-white-o (1).jpg" ' Update with your file 

        If filePath = "" Then
            filePath = Application.StartupPath & "\S__25649163.jpg"
        End If

        ' Set Chrome options to maximize the window
        Dim options As New ChromeOptions()
        options.AddArgument("window-size=350,350")
        ' Calculate X and Y coordinates based on the loopID

        ' Generate random X and Y coordinates

        Dim X As Integer = rand.Next(0, 1000) ' Adjust the range as needed
        Dim Y As Integer = rand.Next(0, 1000) ' Adjust the range as needed


        options.AddArgument($"window-position={position}")
        ' Initialize ChromeDriver with options
        Dim driver As New ChromeDriver(options)
        ' Navigate to the specified URL
        driver.Navigate().GoToUrl(url)
        ' Get the HTTP status of the webpage
        Dim CH As Integer = 5

        For ICHECK As Integer = 0 To CH
            If IsTimeGreaterThan859() Then
                ' Check if a button with ID "submitBtn" is present
                Dim isElementPresent As Boolean = CheckForElement(driver, By.Name("fileUpload"), TimeSpan.FromSeconds(10))
                CH = CH + 5
                If isElementPresent Then
                    Exit For
                Else
                    driver.Navigate().GoToUrl(url)
                    Console.WriteLine("Element not found within the specified timeout period.")
                End If
                Threading.Thread.Sleep(5000)
            Else
                CH = CH + 5
                Threading.Thread.Sleep(2500)
            End If
        Next
        Try
            Dim element As IWebElement
            element = driver.FindElement(By.Name("fileUpload"))
            element.SendKeys(filePath)

            element = driver.FindElement(By.Name("CARD_ID"))
            element.SendKeys("1369900361691")

            element = driver.FindElement(By.Name("PREFIX_TH"))
            Dim selectObject As New SelectElement(element)
            selectObject.SelectByText("นางสาว")

            element = driver.FindElement(By.Name("NAME_TH"))
            element.SendKeys("อาริษา")

            element = driver.FindElement(By.Name("LASTNAME_TH"))
            element.SendKeys("วัชรคุปต์")


            element = driver.FindElement(By.Name("PREFIX_EN"))
            selectObject = New SelectElement(element)
            selectObject.SelectByText("MISS")

            element = driver.FindElement(By.Name("NAME_EN"))
            element.SendKeys("ARISA")

            element = driver.FindElement(By.Name("LASTNAME_EN"))
            element.SendKeys("WATCHARAKUP")

            element = driver.FindElement(By.Name("EMAIL"))
            element.SendKeys("Arisa.watcharakup1996@gmail.com")


            element = driver.FindElement(By.Name("ADDRESS"))
            element.SendKeys("125/199 หมู่7 ตึกโบว์ เปรมฤทัย ซ.4 ต.บางโฉลง อ. บางพลี สมุทรปราการ 10540")


            element = driver.FindElement(By.Name("passw2"))
            Dim OTPX As String = element.GetAttribute("value")

            element = driver.FindElement(By.Name("capt"))
            element.SendKeys(OTPX)

            element = driver.FindElement(By.Name("ckAccept"))
            element.Click()

            element = driver.FindElement(By.Name("btLogin"))
            element.Click()
            ' Get the current time
            Dim currentTime As DateTime = DateTime.Now
            ' Create a DateTime object representing 08:59:00
            Dim time859 As New DateTime(2024, 5, 30, 9, 30, 0)
            ' Compare the current time with 08:59:00
            If currentTime > time859 Then
                driver.Quit()
            End If

        Catch ex As Exception

            'OpenAndPerformAction(txtURL.Text, position)
        Finally
            Threading.Thread.Sleep(20500)
            ' Clean up resources
            If driver IsNot Nothing Then
                driver.Quit()
            End If
        End Try


        'driver.Quit()
    End Sub
    Function CheckForElement(ByVal driver As IWebDriver, ByVal locator As By, ByVal timeout As TimeSpan) As Boolean
        Dim wait As New WebDriverWait(driver, timeout)
        Try
            ' Wait until the condition is true (the element is located)
            wait.Until(Function(d) d.FindElements(locator).Count > 0)
            Return True ' Return true if the element is found within the timeout period
        Catch ex As WebDriverTimeoutException
            ' If timeout occurs, output a message indicating a timeout
            Return False ' Return false if the element is not found within the timeout period
        End Try
    End Function
    Sub InteractWithElements(driver As IWebDriver)
        ' Interact with a text field
        Dim textField As IWebElement = driver.FindElement(By.Id("text_field_id"))
        textField.SendKeys("Your text here")
        ' Interact with a select element (dropdown)
        Dim selectElement As IWebElement = driver.FindElement(By.Id("select_element_id"))
        Dim selectObject As New OpenQA.Selenium.Support.UI.SelectElement(selectElement)
        selectObject.SelectByText("Option Text") ' or SelectByValue or SelectByIndex

        ' Interact with a button
        Dim button As IWebElement = driver.FindElement(By.Id("button_id"))
        button.Click()
        ' Additional interactions as needed
    End Sub

    Function CheckElementExists(ByVal driver As IWebDriver, ByVal locator As By) As Boolean
        Try
            ' Attempt to find the element
            Dim element As IWebElement = driver.FindElement(locator)
            ' If no exception is thrown, the element exists
            Return True
        Catch ex As NoSuchElementException
            ' If NoSuchElementException is thrown, the element does not exist
            Return False
        End Try
    End Function



    Function MeasurePageLoadStatus(ByVal driver As IWebDriver, ByVal url As String) As Boolean
        Dim stopwatch As New Stopwatch()
        Try
            ' Start stopwatch to measure time
            stopwatch.Start()
            ' Navigate to the webpage
            driver.Navigate().GoToUrl(url)
            ' Wait until the page is fully loaded
            Dim wait As New WebDriverWait(driver, TimeSpan.FromSeconds(30)) ' Set timeout as needed
            wait.Until(Function(d) CType(d, IJavaScriptExecutor).ExecuteScript("return document.readyState") = "complete")
            ' Stop the stopwatch
            stopwatch.Stop()

            ' Return True indicating successful page load
            Return True
        Catch ex As WebDriverTimeoutException
            ' If timeout occurs, output a message indicating a timeout
            Console.WriteLine("Timeout occurred while loading the page.")
            ' Return False indicating timeout
            Return False
        End Try
    End Function



    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        ' Show the OpenFileDialog to select an image file
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.jpeg; *.png; *.gif"
        openFileDialog.Multiselect = False ' Allow only single file selection
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            ' Get the selected image file path
            txtPath.Text = openFileDialog.FileName

        End If
    End Sub

End Class
