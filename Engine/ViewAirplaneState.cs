using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlightSimulatorWPF
{
    /// <summary>
    /// Отображение панели в кабине
    /// </summary>
    public class ViewAirplaneState
    {
        private Canvas _cabin_panel_canvas;

        public ViewAirplaneState(Canvas panel_canvas) {
            _cabin_panel_canvas = panel_canvas;
        }

        /// <summary> Отображение панели в кабине </summary>
        public void DrawPanel(double airplane_speed, double airplane_climb_angle, double heigth, double airplane_acacceleration_y, double airplane_acacceleration_heigth, double power, bool chassis_status)
        {
            DrawSpeed(_cabin_panel_canvas, airplane_speed);
            DrawHeigth(_cabin_panel_canvas, heigth);
            DrawEngineCurrentPower(_cabin_panel_canvas, power);

            DrawClimbAngle(_cabin_panel_canvas, airplane_climb_angle);
            DrawMark(airplane_acacceleration_y, airplane_acacceleration_heigth);
            DrawChassisStatus(_cabin_panel_canvas, chassis_status);
        }

        /// <summary> Отображение скорости </summary>
        private void DrawSpeed(Canvas panel_canvas, double airplane_speed)
        {
            panel_canvas.Children.Clear();

            var rectangle = new Rectangle();

            rectangle.Fill = Brushes.Black;
            rectangle.Width = 120;
            rectangle.Height = 25;

            Canvas.SetLeft(rectangle, 90);
            Canvas.SetTop(rectangle, 100);

            panel_canvas.Children.Add(rectangle);

            TextBlock speed_text = new TextBlock();
            speed_text.FontSize = 15;
            speed_text.Text = "Скорость: " + airplane_speed.ToString("00.00");
            speed_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            Canvas.SetLeft(speed_text, 90);
            Canvas.SetTop(speed_text, 100);
            panel_canvas.Children.Add(speed_text);
        }

        /// <summary> Отображение высоты </summary>
        private void DrawHeigth(Canvas panel_canvas, double heigth) {
            var rectangle = new Rectangle();

            rectangle.Fill = Brushes.Black;
            rectangle.Width = 120;
            rectangle.Height = 25;

            Canvas.SetLeft(rectangle, 90);
            Canvas.SetTop(rectangle, 140);

            panel_canvas.Children.Add(rectangle);

            TextBlock speed_text = new TextBlock();
            speed_text.FontSize = 15;
            speed_text.Text = "Высота: " + heigth.ToString("00.00");
            speed_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            Canvas.SetLeft(speed_text, 90);
            Canvas.SetTop(speed_text, 140);
            panel_canvas.Children.Add(speed_text);
        }

        /// <summary> Отображение текущей мощности </summary>
        private void DrawEngineCurrentPower(Canvas panel_canvas, double power) {
            var rectangle = new Rectangle();

            rectangle.Fill = Brushes.Black;
            rectangle.Width = 120;
            rectangle.Height = 25;

            Canvas.SetLeft(rectangle, 90);
            Canvas.SetTop(rectangle, 180);

            panel_canvas.Children.Add(rectangle);

            TextBlock speed_text = new TextBlock();
            speed_text.FontSize = 15;

            speed_text.Text = "Мощность: " + power.ToString("00.00");
            speed_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            Canvas.SetLeft(speed_text, 90);
            Canvas.SetTop(speed_text, 180);
            panel_canvas.Children.Add(speed_text);
        }

        /// <summary> Отображение угла для возвышения/понижения </summary>
        private void DrawClimbAngle(Canvas panel_canvas, double angle) {
            var rectangle = new Rectangle();

            rectangle.Fill = Brushes.Black;
            rectangle.Width = 125;
            rectangle.Height = 25;

            Canvas.SetLeft(rectangle, 230);
            Canvas.SetTop(rectangle, 140);

            panel_canvas.Children.Add(rectangle);

            TextBlock speed_text = new TextBlock();
            speed_text.FontSize = 15;

            speed_text.Text = "Угол корпуса: " + angle.ToString();
            speed_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            Canvas.SetLeft(speed_text, 230);
            Canvas.SetTop(speed_text, 140);
            panel_canvas.Children.Add(speed_text);
        }

        /// <summary> Отображение статуса шасси </summary>
        private void DrawChassisStatus(Canvas panel_canvas, bool chassis_status) {
            var rectangle = new Rectangle();

            rectangle.Fill = Brushes.Black;
            rectangle.Width = 125;
            rectangle.Height = 25;

            Canvas.SetLeft(rectangle, 230);
            Canvas.SetTop(rectangle, 100);

            panel_canvas.Children.Add(rectangle);

            TextBlock speed_text = new TextBlock();
            speed_text.FontSize = 15;

            speed_text.Text = "Шасси: " + ((chassis_status) ? ("Не убраны") : ("Убраны"));
            speed_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            Canvas.SetLeft(speed_text, 230);
            Canvas.SetTop(speed_text, 100);
            panel_canvas.Children.Add(speed_text);
        }

        /// <summary> Отображение крестика направления </summary>
        private void DrawMark(double acacceleration_y, double acacceleration_heigth) {

            double X = 225 + acacceleration_y * (-3);
            double Y = -acacceleration_heigth;

            const double size = 25;

            var MarkLine1 = new Line();
            MarkLine1.X1 = X;
            MarkLine1.X2 = X;

            MarkLine1.Y1 = Y - size;
            MarkLine1.Y2 = Y + size;

            MarkLine1.Stroke = Brushes.White;
            MarkLine1.StrokeThickness = 8;

            MarkLine1.HorizontalAlignment = HorizontalAlignment.Left;
            MarkLine1.VerticalAlignment = VerticalAlignment.Top;

            _cabin_panel_canvas.Children.Add(MarkLine1);

            var MarkLine2 = new Line();
            MarkLine2.X1 = X - size;
            MarkLine2.X2 = X + size;

            MarkLine2.Y1 = Y;
            MarkLine2.Y2 = Y;

            MarkLine2.Stroke = Brushes.White;
            MarkLine2.StrokeThickness = 8;

            MarkLine2.HorizontalAlignment = HorizontalAlignment.Left;
            MarkLine2.VerticalAlignment = VerticalAlignment.Top;

            _cabin_panel_canvas.Children.Add(MarkLine2);
        }
    }
}
