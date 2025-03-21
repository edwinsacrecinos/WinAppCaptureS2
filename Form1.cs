using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CapturaPantallaApp
{
    public partial class Form1 : Form
    {
        private string saveDirectory = string.Empty;
        private Button captureButton = new Button();
        private Button closeButton = new Button();
        private System.Windows.Forms.Timer captureTimer = new System.Windows.Forms.Timer();
        private bool isCapturing = false;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Form1()
        {
            InitializeComponent();
            InitializeScreenshotFolder();
            InitializeUI();
        }

        private void InitializeScreenshotFolder()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            saveDirectory = Path.Combine(appDirectory, timestamp);
            Directory.CreateDirectory(saveDirectory);
        }

        private void InitializeUI()
        {
            this.Text = "Capturador";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(250, 120); // FORMULARIO MÁS GRANDE

            var screen = Screen.PrimaryScreen ?? Screen.AllScreens[0]; 
            this.Location = new Point(10, screen.Bounds.Height - this.Height - 10);
            
            this.TopMost = true;

            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;

            captureButton.Text = "📸 Iniciar Captura";
            captureButton.Font = new Font("Arial", 12, FontStyle.Bold);
            captureButton.Size = new Size(150, 60); // MANTENEMOS EL MISMO TAMAÑO
            captureButton.BackColor = Color.LightBlue;
            captureButton.ForeColor = Color.Black;
            captureButton.FlatStyle = FlatStyle.Flat;
            captureButton.Location = new Point(10, 30);
            captureButton.FlatAppearance.BorderSize = 0;
            captureButton.Click += ToggleCapture;

            closeButton.Text = "❌";
            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.Size = new Size(60, 60); // MANTENEMOS EL MISMO TAMAÑO
            closeButton.BackColor = Color.Red;
            closeButton.ForeColor = Color.White;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Location = new Point(180, 30);
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (sender, e) => Application.Exit();

            this.Controls.Add(captureButton);
            this.Controls.Add(closeButton);

            captureTimer.Interval = 2000;
            captureTimer.Tick += CaptureScreen;
        }

        private void ToggleCapture(object? sender, EventArgs e)
        {
            if (!isCapturing)
            {
                isCapturing = true;
                captureTimer.Start();
                captureButton.Text = "🛑 Detener";
                captureButton.BackColor = Color.OrangeRed;
            }
            else
            {
                isCapturing = false;
                captureTimer.Stop();
                captureButton.Text = "📸 Iniciar Captura";
                captureButton.BackColor = Color.LightBlue;
            }
        }

        private void CaptureScreen(object? sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(100);  

                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                using (Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(screenshot))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }

                    string fileName = Path.Combine(saveDirectory, $"Captura_{DateTime.Now:HH-mm-ss}.jpg");
                    using (EncoderParameters encoderParams = new EncoderParameters(1))
                    {
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        screenshot.Save(fileName, jpgEncoder, encoderParams);
                    }

                    ShowTemporaryMessage("Captura de pantalla GUARDADA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al capturar pantalla: {ex.Message}");
            }
        }

        private void ShowTemporaryMessage(string message)
        {
            Form messageForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(250, 60),
                BackColor = Color.Black,
                TopMost = true
            };

            Label messageLabel = new Label
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            messageForm.Controls.Add(messageLabel);
            messageForm.Show();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                messageForm.Close();
                timer.Stop();
            };
            timer.Start();
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            throw new InvalidOperationException("No se encontró un codificador de imagen válido.");
        }

        private void Form_MouseDown(object? sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form_MouseMove(object? sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form_MouseUp(object? sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
