﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Led
{
    internal class Led7SegmentBase : Control
    {

        public Led7SegmentBase()
        {
            this.SuspendLayout();
            this.Name = "Led7SegmentBase";
            this.Size = new System.Drawing.Size(32, 64);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SevenSegment_Paint);
            this.Resize += new System.EventHandler(this.SevenSegment_Resize);
            this.ResumeLayout(false);

            this.TabStop = false;
            this.Padding = new Padding(0, 4, 4, 4);
            this.DoubleBuffered = true;

            segPoints = new Point[7][];
            for (int i = 0; i < 7; i++) segPoints[i] = new Point[6];

            RecalculatePoints();
        }

        /// <summary>
        /// Recalculate the points that represent the polygons of the
        /// seven segments, whether we're just initializing or
        /// changing the segment width.
        /// </summary>
        private void RecalculatePoints()
        {
            int halfHeight = gridHeight / 2, halfWidth = elementWidth / 2;

            int p = 0;
            segPoints[p][0].X = elementWidth + 1; segPoints[p][0].Y = 0;
            segPoints[p][1].X = gridWidth - elementWidth - 1; segPoints[p][1].Y = 0;
            segPoints[p][2].X = gridWidth - halfWidth - 1; segPoints[p][2].Y = halfWidth;
            segPoints[p][3].X = gridWidth - elementWidth - 1; segPoints[p][3].Y = elementWidth;
            segPoints[p][4].X = elementWidth + 1; segPoints[p][4].Y = elementWidth;
            segPoints[p][5].X = halfWidth + 1; segPoints[p][5].Y = halfWidth;

            p++;
            segPoints[p][0].X = 0; segPoints[p][0].Y = elementWidth + 1;
            segPoints[p][1].X = halfWidth; segPoints[p][1].Y = halfWidth + 1;
            segPoints[p][2].X = elementWidth; segPoints[p][2].Y = elementWidth + 1;
            segPoints[p][3].X = elementWidth; segPoints[p][3].Y = halfHeight - halfWidth - 1;
            segPoints[p][4].X = 4; segPoints[p][4].Y = halfHeight - 1;
            segPoints[p][5].X = 0; segPoints[p][5].Y = halfHeight - 1;

            p++;
            segPoints[p][0].X = gridWidth - elementWidth; segPoints[p][0].Y = elementWidth + 1;
            segPoints[p][1].X = gridWidth - halfWidth; segPoints[p][1].Y = halfWidth + 1;
            segPoints[p][2].X = gridWidth; segPoints[p][2].Y = elementWidth + 1;
            segPoints[p][3].X = gridWidth; segPoints[p][3].Y = halfHeight - 1;
            segPoints[p][4].X = gridWidth - 4; segPoints[p][4].Y = halfHeight - 1;
            segPoints[p][5].X = gridWidth - elementWidth; segPoints[p][5].Y = halfHeight - halfWidth - 1;

            p++;
            segPoints[p][0].X = elementWidth + 1; segPoints[p][0].Y = halfHeight - halfWidth;
            segPoints[p][1].X = gridWidth - elementWidth - 1; segPoints[p][1].Y = halfHeight - halfWidth;
            segPoints[p][2].X = gridWidth - 5; segPoints[p][2].Y = halfHeight;
            segPoints[p][3].X = gridWidth - elementWidth - 1; segPoints[p][3].Y = halfHeight + halfWidth;
            segPoints[p][4].X = elementWidth + 1; segPoints[p][4].Y = halfHeight + halfWidth;
            segPoints[p][5].X = 5; segPoints[p][5].Y = halfHeight;

            p++;
            segPoints[p][0].X = 0; segPoints[p][0].Y = halfHeight + 1;
            segPoints[p][1].X = 4; segPoints[p][1].Y = halfHeight + 1;
            segPoints[p][2].X = elementWidth; segPoints[p][2].Y = halfHeight + halfWidth + 1;
            segPoints[p][3].X = elementWidth; segPoints[p][3].Y = gridHeight - elementWidth - 1;
            segPoints[p][4].X = halfWidth; segPoints[p][4].Y = gridHeight - halfWidth - 1;
            segPoints[p][5].X = 0; segPoints[p][5].Y = gridHeight - elementWidth - 1;

            p++;
            segPoints[p][0].X = gridWidth - elementWidth; segPoints[p][0].Y = halfHeight + halfWidth + 1;
            segPoints[p][1].X = gridWidth - 4; segPoints[p][1].Y = halfHeight + 1;
            segPoints[p][2].X = gridWidth; segPoints[p][2].Y = halfHeight + 1;
            segPoints[p][3].X = gridWidth; segPoints[p][3].Y = gridHeight - elementWidth - 1;
            segPoints[p][4].X = gridWidth - halfWidth; segPoints[p][4].Y = gridHeight - halfWidth - 1;
            segPoints[p][5].X = gridWidth - elementWidth; segPoints[p][5].Y = gridHeight - elementWidth - 1;

            p++;
            segPoints[p][0].X = elementWidth + 1; segPoints[p][0].Y = gridHeight - elementWidth;
            segPoints[p][1].X = gridWidth - elementWidth - 1; segPoints[p][1].Y = gridHeight - elementWidth;
            segPoints[p][2].X = gridWidth - halfWidth - 1; segPoints[p][2].Y = gridHeight - halfWidth;
            segPoints[p][3].X = gridWidth - elementWidth - 1; segPoints[p][3].Y = gridHeight;
            segPoints[p][4].X = elementWidth + 1; segPoints[p][4].Y = gridHeight;
            segPoints[p][5].X = halfWidth + 1; segPoints[p][5].Y = gridHeight - halfWidth;
        }

        private Point[][] segPoints;

        private int gridHeight = 80;
        private int gridWidth = 48;
        private int elementWidth = 10;
        private float italicFactor = 0.0F;
        private Color colorBackground = Color.Black;
        private Color colorDark = Color.DimGray;
        private Color colorLight = Color.Red;


        /// <summary>
        /// Background color of the 7-segment display.
        /// </summary>
        public Color ColorBackground { get { return colorBackground; } set { colorBackground = value; Invalidate(); } }
        /// <summary>
        /// Color of inactive LED segments.
        /// </summary>
        public Color ColorDark { get { return colorDark; } set { colorDark = value; Invalidate(); } }
        /// <summary>
        /// Color of active LED segments.
        /// </summary>
        public Color ColorLight { get { return colorLight; } set { colorLight = value; Invalidate(); } }

        /// <summary>
        /// Width of LED segments.
        /// </summary>
        public int ElementWidth { get { return elementWidth; } set { elementWidth = value; RecalculatePoints(); Invalidate(); } }
        /// <summary>
        /// Shear coefficient for italicizing the displays. Try a value like -0.1.
        /// </summary>
        public float ItalicFactor { get { return italicFactor; } set { italicFactor = value; Invalidate(); } }

        private void SevenSegment_Resize(object sender, EventArgs e) { this.Invalidate(); }
        protected override void OnPaddingChanged(EventArgs e) { base.OnPaddingChanged(e); this.Invalidate(); }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            e.Graphics.Clear(colorBackground);
        }

        /// <summary>
        /// These are the various bit patterns that represent the characters
        /// that can be displayed in the seven segments. Bits 0 through 6
        /// correspond to each of the LEDs, from top to bottom!
        /// </summary>
        public enum ValuePattern
        {
            None = 0x0, Zero = 0x77, One = 0x24, Two = 0x5D, Three = 0x6D,
            Four = 0x2E, Five = 0x6B, Six = 0x7B, Seven = 0x25,
            Eight = 0x7F, Nine = 0x6F, A = 0x3F, B = 0x7A, C = 0x53,
            D = 0x7C, E = 0x5B, F = 0x1B, G = 0x73, H = 0x3E,
            J = 0x74, L = 0x52, N = 0x38, O = 0x78, P = 0x1F, Q = 0x2F, R = 0x18,
            T = 0x5A, U = 0x76, Y = 0x6E,
            Dash = 0x8, Equals = 0x48
        }

        private string theValue = null;

        /// <summary>
        /// Character to be displayed on the seven segments. Supported characters
        /// are digits and most letters.
        /// </summary>
        public string Value
        {
            get { return theValue; }
            set
            {
                customPattern = 0;
                if (value != null)
                {
                    //is it an integer?
                    bool success = false;
                    //try
                    //{
                    if (value == "0" | value == "1" | value == "2" | value == "3" | value == "4" | value == "5" |
                        value == "6" | value == "7" | value == "8" | value == "9")
                    {
                        int tempValue = Convert.ToInt32(value);
                        if (tempValue > 9) tempValue = 9; if (tempValue < 0) tempValue = 0;
                        switch (tempValue)
                        {
                            case 0: customPattern = (int)ValuePattern.Zero; break;
                            case 1: customPattern = (int)ValuePattern.One; break;
                            case 2: customPattern = (int)ValuePattern.Two; break;
                            case 3: customPattern = (int)ValuePattern.Three; break;
                            case 4: customPattern = (int)ValuePattern.Four; break;
                            case 5: customPattern = (int)ValuePattern.Five; break;
                            case 6: customPattern = (int)ValuePattern.Six; break;
                            case 7: customPattern = (int)ValuePattern.Seven; break;
                            case 8: customPattern = (int)ValuePattern.Eight; break;
                            case 9: customPattern = (int)ValuePattern.Nine; break;
                        }
                        success = true;
                    }
                    //}
                    //catch { }
                    if (!success)
                    {
                        //try
                        //{
                        //is it a letter?
                        string tempString = Convert.ToString(value);
                        switch (tempString.ToLower()[0])
                        {
                            case 'a': customPattern = (int)ValuePattern.A; break;
                            case 'b': customPattern = (int)ValuePattern.B; break;
                            case 'c': customPattern = (int)ValuePattern.C; break;
                            case 'd': customPattern = (int)ValuePattern.D; break;
                            case 'e': customPattern = (int)ValuePattern.E; break;
                            case 'f': customPattern = (int)ValuePattern.F; break;
                            case 'g': customPattern = (int)ValuePattern.G; break;
                            case 'h': customPattern = (int)ValuePattern.H; break;
                            case 'j': customPattern = (int)ValuePattern.J; break;
                            case 'l': customPattern = (int)ValuePattern.L; break;
                            case 'n': customPattern = (int)ValuePattern.N; break;
                            case 'o': customPattern = (int)ValuePattern.O; break;
                            case 'p': customPattern = (int)ValuePattern.P; break;
                            case 'q': customPattern = (int)ValuePattern.Q; break;
                            case 'r': customPattern = (int)ValuePattern.R; break;
                            case 't': customPattern = (int)ValuePattern.T; break;
                            case 'u': customPattern = (int)ValuePattern.U; break;
                            case 'y': customPattern = (int)ValuePattern.Y; break;
                            case '-': customPattern = (int)ValuePattern.Dash; break;
                            case '=': customPattern = (int)ValuePattern.Equals; break;
                        }
                        //}
                        //catch { }
                    }
                }
                theValue = value; Invalidate();
            }
        }

        private int customPattern = 0;
        /// <summary>
        /// Set a custom bit pattern to be displayed on the seven segments. This is an
        /// integer value where bits 0 through 6 correspond to each respective LED
        /// segment.
        /// </summary>
        public int CustomPattern { get { return customPattern; } set { customPattern = value; Invalidate(); } }

        private bool showDot = true, dotOn = false;
        /// <summary>
        /// Specifies if the decimal point LED is displayed.
        /// </summary>
        public bool DecimalShow { get { return showDot; } set { showDot = value; Invalidate(); } }
        /// <summary>
        /// Specifies if the decimal point LED is active.
        /// </summary>
        public bool DecimalOn { get { return dotOn; } set { dotOn = value; Invalidate(); } }


        private void SevenSegment_Paint(object sender, PaintEventArgs e)
        {
            int useValue = customPattern;

            Brush brushLight = new SolidBrush(colorLight);
            Brush brushDark = new SolidBrush(colorDark);

            // Define transformation for our container...
            RectangleF srcRect = new RectangleF(0.0F, 0.0F, gridWidth, gridHeight);
            RectangleF destRect = new RectangleF(Padding.Left, Padding.Top, this.Width - Padding.Left - Padding.Right, this.Height - Padding.Top - Padding.Bottom);

            // Begin graphics container that remaps coordinates for our convenience
            GraphicsContainer containerState = e.Graphics.BeginContainer(destRect, srcRect, GraphicsUnit.Pixel);

            Matrix trans = new Matrix();
            trans.Shear(italicFactor, 0.0F);
            e.Graphics.Transform = trans;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Default;

            // Draw elements based on whether the corresponding bit is high
            e.Graphics.FillPolygon((useValue & 0x1) == 0x1 ? brushLight : brushDark, segPoints[0]);
            e.Graphics.FillPolygon((useValue & 0x2) == 0x2 ? brushLight : brushDark, segPoints[1]);
            e.Graphics.FillPolygon((useValue & 0x4) == 0x4 ? brushLight : brushDark, segPoints[2]);
            e.Graphics.FillPolygon((useValue & 0x8) == 0x8 ? brushLight : brushDark, segPoints[3]);
            e.Graphics.FillPolygon((useValue & 0x10) == 0x10 ? brushLight : brushDark, segPoints[4]);
            e.Graphics.FillPolygon((useValue & 0x20) == 0x20 ? brushLight : brushDark, segPoints[5]);
            e.Graphics.FillPolygon((useValue & 0x40) == 0x40 ? brushLight : brushDark, segPoints[6]);

            if (showDot)
                e.Graphics.FillEllipse(dotOn ? brushLight : brushDark, gridWidth - 1, gridHeight - elementWidth + 1, elementWidth, elementWidth);

            e.Graphics.EndContainer(containerState);
        }
    }
}
