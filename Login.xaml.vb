Imports System.Data
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Xaml
Public Class Login
    Public Shared User As String = ""

    Public Shared Role As String = ""
    Public Shared Org_ID As Integer = 0
    Private WithEvents xTimer As New System.Windows.Forms.Timer
    Dim i As Integer = 0
    Public Sub New()
        InitializeComponent()
        Dim uriSource1 = New Uri("C:\Wosul\Wosul\login gallery\1.jpg", UriKind.Absolute)
        Dim bitmapImage1 As BitmapImage = New BitmapImage(uriSource1)
        Dim uriSource2 = New Uri("C:\Wosul\Wosul\login gallery\2.jpg", UriKind.Absolute)
        Dim bitmapImage2 As BitmapImage = New BitmapImage(uriSource2)
        Me.DataContext = Me
        Dim images As List(Of BitmapImage) = New List(Of BitmapImage)() From {
                bitmapImage1,
                bitmapImage2
            }
        CombinedImage = GetCombinedImage(images)
        xTimer = New System.Windows.Forms.Timer
        xTimer.Interval = 1000
    End Sub
    Private Shared Function GetCombinedImage(ByVal images As IEnumerable(Of BitmapImage)) As RenderTargetBitmap
        Dim totalWidthOfAllImages As Integer = images.Sum(Function(p) CInt(p.Width))
        Dim maxHeightOfAllImages As Integer = images.Max(Function(p) CInt(p.Height))
        Dim drawingVisual As DrawingVisual = New DrawingVisual()
        Dim drawingVisual1 As DrawingVisual = New DrawingVisual()

        Using drawingContext As DrawingContext = drawingVisual.RenderOpen()
            Dim left As Double = 0
            Dim height As Double = 0

            For Each image As BitmapImage In images
                drawingContext.DrawImage(image, New Rect(left, 0, image.Width, image.Height))

                ' drawingContext.DrawImage(image, New Rect(left, height, image.Width, image.Height))
                left += image.Width
                ' height += image.Height
            Next
        End Using

        Using drawingContext1 As DrawingContext = drawingVisual1.RenderOpen()
            Dim left As Double = 0
            Dim height As Double = 0

            For Each image As BitmapImage In images
                drawingContext1.DrawImage(image, New Rect(500, 300, image.Width, image.Height))

                ' drawingContext.DrawImage(image, New Rect(left, height, image.Width, image.Height))
                left += image.Width
                ' height += image.Height
            Next
        End Using

        ' Dim bmp As RenderTargetBitmap = New RenderTargetBitmap(totalWidthOfAllImages, maxHeightOfAllImages, 96, 96, PixelFormats.[Default])
        Dim bmp As RenderTargetBitmap = New RenderTargetBitmap(2000, 2000, 96, 96, PixelFormats.[Default])
        Dim bmp1 As RenderTargetBitmap = New RenderTargetBitmap(2000, 2000, 96, 96, PixelFormats.[Default])
        bmp.Render(drawingVisual)
        Return bmp
        bmp1.Render(drawingVisual1)
        Return bmp1
    End Function

    Public Property CombinedImage As ImageSource

    Private Sub ScrollViewerCanvas_ManipulationBoundaryFeedback(ByVal sender As Object, ByVal e As ManipulationBoundaryFeedbackEventArgs)
        e.Handled = True
    End Sub
    Public Sub New(TickValue As Integer)
        xTimer = New System.Windows.Forms.Timer
        xTimer.Interval = 1000
    End Sub
    Public Sub StartTimer()
        xTimer.Start()
    End Sub

    Public Sub StopTimer()
        xTimer.Stop()
    End Sub

    Private Sub Timer_Tick() Handles xTimer.Tick
        goNext()
        'goBack()
    End Sub
    Private Sub goNext()
        i += 1

        If i > 6 Then
            i = 1
        End If

        picHolder.Source = New BitmapImage(New Uri("login gallery/" & i & ".jpg", UriKind.Relative))
    End Sub
    Private Sub goBack()
        i -= 1

        If i < 1 Then
            i = 6
        End If

        picHolder.Source = New BitmapImage(New Uri("login gallery/" & i & ".jpg", UriKind.Relative))
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs)
        'DialogResult = True

        If txtUserName.Text.Trim = "" Then
            lblError.Content = "Enter Email"
            Exit Sub
        End If
        If InStr(txtUserName.Text.Trim, "@") = 0 Then
            MessageBox.Show("Enter a Valid Email", "Information")
            Exit Sub
        End If
        If txtPassword.Password.Trim = "" Then
            MessageBox.Show("Enterr Password", "Information")
            Exit Sub
        End If
        Dim da As New DataAccessLayer()

        Dim dt As DataTable = da.ValidateUser(txtUserName.Text.Trim(), txtPassword.Password.Trim())
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim User_Id As String = IIf(Convert.IsDBNull(dt.Rows(0)("email")), -1, dt.Rows(0)("email"))


            If User_Id <> "" Then
                Dim time As [String] = DateTime.Now.ToString("HHmmss")

                User = txtUserName.Text.Trim()

                'Role = IIf(Convert.IsDBNull(dt.Rows(0)("Role_Name")), "", dt.Rows(0)("Role_Name"))
                'MainWindow()
                Dim frmNewOrder As New Window2()
                frmNewOrder.Show()
            Else

                lblError.Content = "Invalid User Name/Password/Inactive User!"

            End If
        End If
        'Me.Hide()
        'lblError.Content = "Invalid User Name/Password/Inactive User!"
    End Sub

    Private Sub DisplayLoginScreen()
        'Dim frm As New Login()

        'frm.Owner = Me
        'frm.ShowDialog()
        'If frm.DialogResult.HasValue And frm.DialogResult.Value Then
        '    MessageBox.Show("User Logged In")
        'Else
        '    Me.Close()
        'End If
        txtUserName.Text = "Enter your Email"
        txtPassword.Password = "Enter your Password"
        xTimer.Enabled = True
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(ByVal e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
    End Sub

    Private Sub TxtUserName_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtUserName.TextChanged

    End Sub

    Private Sub txtUserName_GotFocus(sender As Object, e As RoutedEventArgs) Handles txtUserName.GotFocus
        txtUserName.Background = New SolidColorBrush(Colors.White)
    End Sub



    'Private Sub txtUserName_MouseEnter(sender As Object, e As MouseEventArgs) Handles txtUserName.MouseEnter
    '    If txtUserName.Text.Trim = "Enter your Email" Then
    '        txtUserName.Text = ""
    '    End If
    'End Sub

    Private Sub txtPassword_MouseEnter(sender As Object, e As MouseEventArgs) Handles txtPassword.MouseDoubleClick
        If txtPassword.Password.Trim = "Enter your Password" Then
            txtPassword.Password = ""
        End If
    End Sub

    Private Sub txtUserName_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles txtUserName.MouseDown
        If txtUserName.Text.Trim = "Enter your Email" Then
            txtUserName.Text = ""
        End If
    End Sub

    Private Sub txtPassword_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles txtPassword.MouseDown
        If txtPassword.Password.Trim = "Enter your Password" Then
            txtPassword.Password = ""
        End If
    End Sub

    Private Sub Login_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        txtUserName.Focus()
    End Sub
End Class
