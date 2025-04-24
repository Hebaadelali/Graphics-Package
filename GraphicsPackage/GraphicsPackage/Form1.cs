namespace GraphicsPackage
{
    public partial class Form1 : Form
    {
        int cx;
        int cy;
        public Form1()
        {
            InitializeComponent();
            cx = pictureBox1.Size.Width / 2;
            cy = pictureBox1.Size.Height / 2;
        }

        private void button5_Click(object sender, EventArgs e) //Clear 
        {
            pictureBox1.Image = null;
        }

        private bool TryGetInputs(List<(TextBox, string)> boxes, out List<float> values, out string errorMessages)
        {
            values = new List<float>();
            errorMessages = "";
            bool isValid = true;

            foreach (var (box, name) in boxes)
            {
                string text = box.Text.Trim();

                if (string.IsNullOrEmpty(text))
                {
                    errorMessages += $"{name} is empty.\n";
                    isValid = false;
                }
                else if (!float.TryParse(text, out float val))
                {
                    errorMessages += $"{name} is not a valid number.\n";
                    isValid = false;
                }
                else
                {
                    values.Add(val);
                }
            }

            return isValid;
        }


        public void LineDDA(double x0, double y0, double xEnd, double yEnd)
        {
            double dx = xEnd - x0, dy = yEnd - y0, steps, k;
            double xIncrement, yIncrement, x = x0, y = y0;

            if (Math.Abs(dx) > Math.Abs(dy))
                steps = Math.Abs(dx);
            else
                steps = Math.Abs(dy);

            xIncrement = (float)(dx) / (float)(steps);
            yIncrement = (float)(dy) / (float)(steps);
            Brush abrush = Brushes.Black;
            Graphics g = pictureBox1.CreateGraphics();

            g.FillRectangle(abrush, (int)Math.Round((float)x) + cx, cy - (int)Math.Round((float)y), 3, 3);

            for (k = 0; k < steps; k++)
            {
                x += xIncrement;
                y += yIncrement;
                g.FillRectangle(abrush, (int)Math.Round((float)x) + cx, cy - (int)Math.Round((float)y), 3, 3);
            }
        }

        public void lineBres(int x0, int y0, int xEnd, int yEnd)
        {
            int dx = Math.Abs(xEnd - x0), dy = Math.Abs(yEnd - y0);
            int x, y, p = 2 * dy - dx;
            int twoDy = 2 * dy, twoDyMinusDx = 2 * (dy - dx);

            if (x0 > xEnd)
            {
                x = xEnd; y = yEnd; xEnd = x0;
            }
            else
            {
                x = x0; y = y0;
            }
            Brush abrush = Brushes.Black;
            Graphics g = pictureBox1.CreateGraphics();
            g.FillRectangle(abrush, x + cx, cy - y, 3, 3);

            while (x < xEnd)
            {
                x++;
                if (p < 0)
                    p += twoDy;
                else
                {
                    y++;
                    p += twoDyMinusDx;
                }
                g.FillRectangle(abrush, x + cx, cy - y, 3, 3);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (TryGetInputs(new List<(TextBox, string)>
            {
                (textBox1, "X0"),
                (textBox2, "Y0"),
                (textBox3, "XEnd"),
                (textBox4, "YEnd")
            }, out var values, out var error))
            {
                LineDDA(values[0], values[1], values[2], values[3]);
            }
            else
            {
                MessageBox.Show("Please fix the following errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (TryGetInputs(new List<(TextBox, string)>
            {
                (textBox8, "X0"),
                (textBox7, "Y0"),
                (textBox6, "XEnd"),
                (textBox5, "YEnd")
            }, out var values, out var error))
            {
                lineBres((int)values[0], (int)values[1], (int)values[2], (int)values[3]);
            }
            else
            {
                MessageBox.Show("Please fix the following errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    Pen p = new Pen(Color.Black);
        //    g.DrawLine(p, pictureBox1.Size.Width / 2, 0, pictureBox1.Size.Width / 2, pictureBox1.Height);
        //    g.DrawLine(p, 0, pictureBox1.Size.Height / 2, pictureBox1.Size.Width, pictureBox1.Size.Height / 2);

        //}


        public void circlePlotPoints(float xCenter, float yCenter, float x, float y)
        {
            Brush abrush = Brushes.Black;
            Graphics g = pictureBox1.CreateGraphics();
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + x) + cx, cy - (float)Math.Round((float)yCenter + y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - x) + cx, cy - (float)Math.Round((float)yCenter + y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + x) + cx, cy - (float)Math.Round((float)yCenter - y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - x) + cx, cy - (float)Math.Round((float)yCenter - y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + y) + cx, cy - (float)Math.Round((float)yCenter + x), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - y) + cx, cy - (float)Math.Round((float)yCenter + x), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + y) + cx, cy - (float)Math.Round((float)yCenter - x), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - y) + cx, cy - (float)Math.Round((float)yCenter - x), 3, 3);
        }


        public void circleMidpoint(float xc, float yc, float r)
        {
            float x = 0, y = r;
            float d = 3 - 2 * r;

            /* Plot first set of points */
            circlePlotPoints(xc, yc, x, y);

            while (y >= x)
            {
                x++;
                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                    d = d + 4 * x + 6;
                circlePlotPoints(xc, yc, x, y);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TryGetInputs(new List<(TextBox, string)>
            {
                (textBox10, "X Center"),
                (textBox9, "Y Center"),
                (textBox11, "Radius")
            }, out var values, out var error))
            {
                circleMidpoint(values[0], values[1], values[2]);
            }
            else
            {
                MessageBox.Show("Please fix the following errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void ellipsePlotPoints(int xCenter, int yCenter, int x, int y)
        {
            Brush abrush = Brushes.Black;
            Graphics g = pictureBox1.CreateGraphics();
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + x) + cx, cy - (float)Math.Round((float)yCenter + y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - x) + cx, cy - (float)Math.Round((float)yCenter + y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter + x) + cx, cy - (float)Math.Round((float)yCenter - y), 3, 3);
            g.FillRectangle(abrush, (float)Math.Round((float)xCenter - x) + cx, cy - (float)Math.Round((float)yCenter - y), 3, 3);
        }

        void ellipseMidpoint(int xCenter, int yCenter, int Rx, int Ry)
        {
            int Rx2 = Rx * Rx;
            int Ry2 = Ry * Ry;
            int twoRx2 = 2 * Rx2;
            int twoRy2 = 2 * Ry2;
            int p;
            int x = 0;
            int y = Ry;
            int px = 0;
            int py = twoRx2 * y;
            //void ellipsePlotPoints(int, int, int, int);
            /* Plot the initial point in each quadrant. */
            ellipsePlotPoints(xCenter, yCenter, x, y);
            /* Region 1 */
            p = (int)Math.Round(Ry2 - (Rx2 * Ry) + (0.25 * Rx2));
            while (px < py)
            {
                x++;
                px += twoRy2;
                if (p < 0)
                    p += Ry2 + px;
                else
                {
                    y--;
                    py -= twoRx2;
                    p += Ry2 + px - py;
                }
                ellipsePlotPoints(xCenter, yCenter, x, y);
            }
            /* Region 2 */
            p = (int)Math.Round(Ry2 * (x + 0.5) * (x + 0.5) + Rx2 * (y - 1) * (y - 1) - Rx2 * Ry2);
            while (y > 0)
            {
                y--;
                py -= twoRx2;
                if (p > 0)
                    p += Rx2 - py;
                else
                {
                    x++;
                    px += twoRy2;
                    p += Rx2 - py + px;
                }
                ellipsePlotPoints(xCenter, yCenter, x, y);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (TryGetInputs(new List<(TextBox, string)>
            {
                (textBox12, "X Center"),
                (textBox13, "Y Center"),
                (textBox14, "Rx"),
                (textBox15, "Ry")
            }, out var values, out var error))
            {
                ellipseMidpoint((int)values[0], (int)values[1], (int)values[2], (int)values[3]);
            }
            else
            {
                MessageBox.Show("Please fix the following errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static bool IsValidTriangle(int x1, int y1, int x2,
                           int y2, int x3, int y3)
        {
            int a = x1 * (y2 - y3)
                  + x2 * (y3 - y1)
                  + x3 * (y1 - y2);

            if (a == 0)
                return false;
            else
                return true;
        }


        private bool TryGetShapePoints(out List<(int x, int y)> points, out string errorMessages)
        {
            points = new List<(int x, int y)>();
            errorMessages = "";

            var inputBoxes = new List<(TextBox X, TextBox Y, string Name)>
            {
                (textBox19, textBox18, "Point 1"),
                (textBox25, textBox24, "Point 2"),
                (textBox17, textBox16, "Point 3"),
                (textBox23, textBox22, "Point 4")
            };

            foreach (var (xBox, yBox, name) in inputBoxes)
            {
                string xText = xBox.Text.Trim();
                string yText = yBox.Text.Trim();

                if (string.IsNullOrEmpty(xText) && string.IsNullOrEmpty(yText))
                    continue;

                if (!int.TryParse(xText, out int x))
                {
                    errorMessages += $"{name} X is not valid.\n";
                    continue;
                }
                if (!int.TryParse(yText, out int y))
                {
                    errorMessages += $"{name} Y is not valid.\n";
                    continue;
                }

                points.Add((x, y));
            }

            if (points.Count <= 1 || points.Count > 4)
            {
                errorMessages += "Please enter between 2 and 4 valid points.\n";
                return false;
            }

            return string.IsNullOrEmpty(errorMessages);
        }



        private void button14_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (points.Count == 2)
            {
                LineDDA(points[0].x, points[0].y, points[1].x, points[1].y);
            }
            else if (points.Count == 3 && IsValidTriangle(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y))
            {
                LineDDA(points[0].x, points[0].y, points[1].x, points[1].y);
                LineDDA(points[1].x, points[1].y, points[2].x, points[2].y);
                LineDDA(points[2].x, points[2].y, points[0].x, points[0].y);
            }
            else if (points.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var p1 = points[i];
                    var p2 = points[(i + 1) % 4];
                    LineDDA(p1.x, p1.y, p2.x, p2.y);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetInputs(new List<(TextBox, string)>
            {
                (textBox21, "Tx"),
                (textBox20, "Ty")
            }, out var translationValues, out var transError))
            {
                MessageBox.Show("Translation Error:\n" + transError, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int tx = (int)translationValues[0];
            int ty = (int)translationValues[1];

            var translated = points.Select(p => (p.x + tx, p.y + ty)).ToList();

            for (int i = 0; i < translated.Count; i++)
            {
                var p1 = translated[i];
                var p2 = translated[(i + 1) % translated.Count];
                LineDDA(p1.Item1, p1.Item2, p2.Item1, p2.Item2);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetInputs(new List<(TextBox, string)>
            {
                (textBox26, "Angle")
            }, out var values, out var errorRotation))
            {
                MessageBox.Show("Angle Error:\n" + errorRotation, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double angle = values[0];
            float sin = (float)Math.Sin((angle * Math.PI) / 180);
            float cos = (float)Math.Cos((angle * Math.PI) / 180);

            var rotated = points.Select(p =>
            {
                double x = (p.x * cos) - (p.y * sin);
                double y = (p.x * sin) + (p.y * cos);
                return (x, y);
            }).ToList();

            for (int i = 0; i < rotated.Count; i++)
            {
                var p1 = rotated[i];
                var p2 = rotated[(i + 1) % rotated.Count];
                LineDDA(p1.x, p1.y, p2.x, p2.y);
            }
        }



        //Shear X
        private void button9_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetInputs(new List<(TextBox, string)>
            {
                (textBox28, "ShX")
            }, out var values, out var errorShear))
            {
                MessageBox.Show("Shear X Error:\n" + errorShear, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int shx = (int)values[0];

            var sheared = points.Select(p => (p.x + shx * p.y, p.y)).ToList();

            for (int i = 0; i < sheared.Count; i++)
            {
                var p1 = sheared[i];
                var p2 = sheared[(i + 1) % sheared.Count];
                LineDDA(p1.Item1, p1.Item2, p2.Item1, p2.Item2);
            }
        }




        //Shear Y
        private void button10_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetInputs(new List<(TextBox, string)>
            {
                (textBox27, "ShY")
            }, out var values, out var errorShear))
            {
                MessageBox.Show("Shear Y Error:\n" + errorShear, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int shy = (int)values[0];

            var sheared = points.Select(p => (p.x, p.y + shy * p.x)).ToList();

            for (int i = 0; i < sheared.Count; i++)
            {
                var p1 = sheared[i];
                var p2 = sheared[(i + 1) % sheared.Count];
                LineDDA(p1.Item1, p1.Item2, p2.Item1, p2.Item2);
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string extraErrors = "";
            if (!int.TryParse(textBox21.Text.Trim(), out int SX))
            {
                extraErrors += "Scaling X is not valid.\n";
            }
            if (!int.TryParse(textBox20.Text.Trim(), out int SY))
            {
                extraErrors += "Scaling Y is not valid.\n";
            }

            if (!string.IsNullOrEmpty(extraErrors))
            {
                MessageBox.Show("Errors:\n" + extraErrors, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (points.Count == 2)
            {
                LineDDA(points[0].x * SX, points[0].y * SY, points[1].x * SX, points[1].y * SY);
            }
            else if (points.Count == 3 && IsValidTriangle(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y))
            {
                LineDDA(points[0].x * SX, points[0].y * SY, points[1].x * SX, points[1].y * SY);
                LineDDA(points[1].x * SX, points[1].y * SY, points[2].x * SX, points[2].y * SY);
                LineDDA(points[2].x * SX, points[2].y * SY, points[0].x * SX, points[0].y * SY);
            }
            else if (points.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var p1 = points[i];
                    var p2 = points[(i + 1) % 4];
                    LineDDA(p1.x * SX, p1.y * SY, p2.x * SX, p2.y * SY);
                }
            }
        }


        private void button12_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (points.Count == 2)
            {
                LineDDA(points[0].x, -points[0].y, points[1].x, -points[1].y);
            }
            else if (points.Count == 3 && IsValidTriangle(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y))
            {
                LineDDA(points[0].x, -points[0].y, points[1].x, -points[1].y);
                LineDDA(points[1].x, -points[1].y, points[2].x, -points[2].y);
                LineDDA(points[2].x, -points[2].y, points[0].x, -points[0].y);
            }
            else if (points.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var p1 = points[i];
                    var p2 = points[(i + 1) % 4];
                    LineDDA(p1.x, -p1.y, p2.x, -p2.y);
                }
            }
        }


        private void button11_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (points.Count == 2)
            {
                LineDDA(-points[0].x, points[0].y, -points[1].x, points[1].y);
            }
            else if (points.Count == 3 && IsValidTriangle(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y))
            {
                LineDDA(-points[0].x, points[0].y, -points[1].x, points[1].y);
                LineDDA(-points[1].x, points[1].y, -points[2].x, points[2].y);
                LineDDA(-points[2].x, points[2].y, -points[0].x, points[0].y);
            }
            else if (points.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var p1 = points[i];
                    var p2 = points[(i + 1) % 4];
                    LineDDA(-p1.x, p1.y, -p2.x, p2.y);
                }
            }
        }


        private void button13_Click(object sender, EventArgs e)
        {
            if (!TryGetShapePoints(out var points, out var error))
            {
                MessageBox.Show("Errors:\n" + error, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (points.Count == 2)
            {
                LineDDA(-points[0].x, -points[0].y, -points[1].x, -points[1].y);
            }
            else if (points.Count == 3 && IsValidTriangle(points[0].x, points[0].y, points[1].x, points[1].y, points[2].x, points[2].y))
            {
                LineDDA(-points[0].x, -points[0].y, -points[1].x, -points[1].y);
                LineDDA(-points[1].x, -points[1].y, -points[2].x, -points[2].y);
                LineDDA(-points[2].x, -points[2].y, -points[0].x, -points[0].y);
            }
            else if (points.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var p1 = points[i];
                    var p2 = points[(i + 1) % 4];
                    LineDDA(-p1.x, -p1.y, -p2.x, -p2.y);
                }
            }
        }


    }
}