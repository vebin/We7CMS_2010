using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace We7.CMS.Web.Admin
{
    public class CaptchaImage
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private string familyName;

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }
        private Bitmap image;

        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        public CaptchaImage(string s, int width, int height)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        public CaptchaImage(string s, int width, int height, string family)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(family);
            this.GenerateImage();
        }

        ~CaptchaImage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
                this.image.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);
            HatchBrush brush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(brush, rect);

            SizeF size;
            float fontsize = rect.Height + 1;
            Font font;
            do 
            {
                fontsize--;
                font = new Font(this.familyName, fontsize, FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while(rect.Width < size.Width);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points = 
            {
                new PointF(this.random.Next(rect.Width), this.random.Next(rect.Height)),
                new PointF(this.Width - this.random.Next(rect.Width), this.random.Next(rect.Height)),
                new PointF(this.random.Next(rect.Width), this.Height-this.random.Next(rect.Height)),
                new PointF(this.Width-this.random.Next(rect.Width), this.Height - this.random.Next(rect.Height))
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix,WarpMode.Perspective, 0F);
            
            brush = new HatchBrush(HatchStyle.SmallConfetti, Color.DarkGray, Color.DarkGray);
            g.FillPath(brush, path);

            int m = Math.Max(rect.Width, rect.Height);
            for (int i=0; i< (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = m/50;
                int h = m/50;
                g.FillEllipse(brush, x, y, w, h);
            }
            font.Dispose();
            brush.Dispose();
            g.Dispose();
            
            this.image = bitmap;
        }

        private Random random = new Random();

        private void SetDimensions(int width, int height)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this.width = width;
            this.height = height;
        }

        private void SetFamilyName(string familyName)
        {
            try
            {
                Font font = new Font(familyName, 12F);
                this.FamilyName = familyName;
                font.Dispose();
            }
            catch (Exception ex)
            {
                this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }
    }
}