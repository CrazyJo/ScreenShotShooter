using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using HelpersLib;
using ScreenCaptureLib.Properties;

namespace ScreenCaptureLib
{
    public sealed class RectangleRegionLightForm : Form
    {
        public static Rectangle LastSelectionRectangle0Based { get; private set; }

        public Rectangle ScreenRectangle { get; private set; }

        public Rectangle ScreenRectangle0Based
        {
            get
            {
                return new Rectangle(0, 0, ScreenRectangle.Width, ScreenRectangle.Height);
            }
        }

        public Rectangle SelectionRectangle { get; private set; }

        public Rectangle SelectionRectangle0Based
        {
            get
            {
                return new Rectangle(
                    SelectionRectangle.X - ScreenRectangle.X, 
                    SelectionRectangle.Y - ScreenRectangle.Y, 
                    SelectionRectangle.Width, 
                    SelectionRectangle.Height);
            }
        }

        private Timer timer;
        private Image _backgroundImage;
        private TextureBrush _backgroundBrush;
        private Pen _borderDotPen, _borderDotPen2;
        private Point _currentPosition, _positionOnClick;
        private bool _isMouseDown;
        private Stopwatch _penTimer;

        public RectangleRegionLightForm()
        {
            _backgroundImage = Screenshot.CaptureFullscreen();
            _backgroundBrush = new TextureBrush(_backgroundImage);  // объект осуществляющий заливку внутренней части формы
            _borderDotPen = new Pen(Color.Black, 1);    // _borderDotPen - рисуется сплошной линией 
            _borderDotPen2 = new Pen(Color.White, 1) {DashPattern = new float[] {7, 7}};    // _borderDotPen2 - рисуется штриховой линией 
            _penTimer = Stopwatch.StartNew();

            ScreenRectangle = CaptureHelpers.GetScreenBounds();

            InitializeComponent();

            Icon = Resources.AppIcon;

            using (MemoryStream cursorStream = new MemoryStream(Resources.Crosshair))
            {
                Cursor = new Cursor(cursorStream);
            }

            timer = new Timer { Interval = 10 }; // Частота обновления заливки формы и прямоугольников
            timer.Tick += timer_Tick;
            timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (timer != null) timer.Dispose();
            if (_backgroundImage != null) _backgroundImage.Dispose();
            if (_backgroundBrush != null) _backgroundBrush.Dispose();
            if (_borderDotPen != null) _borderDotPen.Dispose();
            if (_borderDotPen2 != null) _borderDotPen2.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            StartPosition = FormStartPosition.Manual;

            //TODO: change
            Bounds = ScreenRectangle;
            //Bounds = new Rectangle(481,229,600,400);

            FormBorderStyle = FormBorderStyle.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //Text = "ShareX - " + Resources.RectangleLight_InitializeComponent_Rectangle_capture_light;
            Text = @"ShareX - Rectangle Region Light";
            ShowInTaskbar = false;
            TopMost = true;

            Shown += RectangleLight_Shown;
            KeyUp += RectangleLight_KeyUp;
            MouseDown += RectangleLight_MouseDown;
            MouseUp += RectangleLight_MouseUp;

            ResumeLayout(false);
        }

        private void RectangleLight_Shown(object sender, EventArgs e)
        {
            this.ForceActivate();
        }

        public void RectangleLight_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void RectangleLight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _positionOnClick = CaptureHelpers.GetCursorPosition();
                _isMouseDown = true;
            }
        }

        private void RectangleLight_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isMouseDown)
                {
                    if (SelectionRectangle0Based.Width > 0 && SelectionRectangle0Based.Height > 0)
                    {
                        LastSelectionRectangle0Based = SelectionRectangle0Based;
                        DialogResult = DialogResult.OK;
                    }

                    Close();
                }
            }
            else
            {
                if (_isMouseDown)
                {
                    _isMouseDown = false;
                    Refresh();
                }
                else
                {
                    Close();
                }
            }
        }

        public Image GetAreaImage()
        {
            Rectangle rect = SelectionRectangle0Based;

            if (rect.Width > 0 && rect.Height > 0)
            {
                if (rect.X == 0 && rect.Y == 0 && rect.Width == _backgroundImage.Width && rect.Height == _backgroundImage.Height)
                {
                    return (Image)_backgroundImage.Clone();
                }

                return Helpers.CropImage(_backgroundImage, rect);
            }

            return null;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _currentPosition = CaptureHelpers.GetCursorPosition();
            SelectionRectangle = CaptureHelpers.CreateRectangle(_positionOnClick.X, _positionOnClick.Y, _currentPosition.X, _currentPosition.Y);

            Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.FillRectangle(_backgroundBrush, ScreenRectangle0Based);

            if (_isMouseDown)
            {
                _borderDotPen2.DashOffset = (float)_penTimer.Elapsed.TotalSeconds * -15.0f;
                g.DrawRectangleProper(_borderDotPen, SelectionRectangle0Based);
                g.DrawRectangleProper(_borderDotPen2, SelectionRectangle0Based);
            }
        }
    }
}