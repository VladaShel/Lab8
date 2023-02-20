using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlightSimulatorWPF
{
    /// <summary>
    /// Отображение вида
    /// </summary>
    public class ViewEngine: ViewAirplaneState
    {
        private Canvas _view_canvas;

        private const string _sky_sprite_file = "Blue_sky.png";
        private const string _plane_sprite_file = "plane.png";
        private const string _mountain_sprite_file = "Mountain.png";
        private const string _mountain_2_sprite_file = "Mountain2.png";
        private const string _mountain_3_sprite_file = "Mountain3.png";
        private const string _design_element_sprite_file = "DesertDesignElement1.png";

        private Image SkyImage;
        private Image PlaneImage;
        private Image Mountain1Image;
        private Image Mountain2Image;
        private Image Mountain3Image;
        private Image DesignElemet1;

        public ViewEngine(Canvas view_canvas, Canvas panel_canvas):base(panel_canvas)
        {
            _view_canvas = view_canvas;
            Init();
        }

        /// <summary> Инициализация </summary>
        private void Init()
        {
            SkyImage = new Image()
            {
                Width = 3200,
                Height = 2000,
                Name = "Sky",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _sky_sprite_file))
            };

            PlaneImage = new Image()
            {
                Width = 2000,
                Height = 900,
                Name = "Plane",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _plane_sprite_file))
            };

            Mountain1Image = new Image()
            {
                Width = 250,
                Height = 250,
                Name = "Mountain",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _mountain_sprite_file))
            };

            Mountain2Image = new Image()
            {
                Width = 250,
                Height = 250,
                Name = "Mountain2",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _mountain_2_sprite_file))
            };

            Mountain3Image = new Image()
            {
                Width = 250,
                Height = 250,
                Name = "Mountain3",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _mountain_3_sprite_file))
            };

            DesignElemet1 = new Image()
            {
                Width = 60,
                Height = 200,
                Name = "DesignElemet1",
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + _design_element_sprite_file))
            };
        }

        /// <summary> Отображение неба и земли </summary>
        public void DrawView(double Heigth, double Angle, double X, double Y)
        {
            _view_canvas.Children.Clear();

            Canvas.SetLeft(SkyImage, -300);
            Canvas.SetTop(SkyImage, -600);

            Canvas.SetLeft(PlaneImage, -100);
            Canvas.SetTop(PlaneImage, 600 + Heigth / 20);

            _view_canvas.Children.Add(SkyImage);
            DrawPlaneBackgroundDesign(Y, Heigth);
            _view_canvas.Children.Add(PlaneImage);
            DrawPlaneDesign(X, Heigth);

            RotateTransform rotateTransform = new RotateTransform(Angle, 772, 400 + Heigth / 30);
            _view_canvas.RenderTransform = rotateTransform;
        }

        /// <summary> Отображение обьектов заднего плана </summary>
        public void DrawPlaneBackgroundDesign(double airplane_y, double heigth)
        {
            DrawImage(_view_canvas, 600 + airplane_y, 400 + heigth / 20, Mountain1Image);
            DrawImage(_view_canvas, 200 + airplane_y, 400 + heigth / 20, Mountain1Image);
            DrawImage(_view_canvas, 800 + airplane_y, 400 + heigth / 20, Mountain1Image);
            DrawImage(_view_canvas, 1100 + airplane_y, 400 + heigth / 20, Mountain1Image);
            DrawImage(_view_canvas, 1500 + airplane_y, 400 + heigth / 20, Mountain1Image);

            DrawImage(_view_canvas, 0 + airplane_y, 430 + heigth / 20, Mountain2Image);
            DrawImage(_view_canvas, 2000 + airplane_y, 430 + heigth / 20, Mountain2Image);

            DrawImage(_view_canvas, 100 + airplane_y, 400 + heigth / 20, Mountain3Image);
            DrawImage(_view_canvas, 900 + airplane_y, 400 + heigth / 20, Mountain3Image);
            DrawImage(_view_canvas, 400 + airplane_y, 400 + heigth / 20, Mountain3Image);
            DrawImage(_view_canvas, 1600 + airplane_y, 400 + heigth / 20, Mountain3Image);
        }

        /// <summary> Отображение обьектов переднего плана </summary>
        public void DrawPlaneDesign(double airplane_x, double heigth) {

            DrawImage(_view_canvas, 600 - airplane_x * 2, 450 + airplane_x * 0.5, PerspectiveElement(airplane_x, DesignElemet1));
            DrawImage(_view_canvas, 850 - airplane_x * 2, 450 + airplane_x * 0.5, PerspectiveElement(airplane_x, DesignElemet1));
            DrawImage(_view_canvas, 100 - airplane_x * 2, 450 + airplane_x * 0.5, PerspectiveElement(airplane_x, DesignElemet1));
            DrawImage(_view_canvas, 400 - airplane_x * 2, 450 + airplane_x * 0.5, PerspectiveElement(airplane_x, DesignElemet1));
        }

        /// <summary> Обработка изображения для создания перспективы </summary>
        private Image PerspectiveElement(double airplane_x, Image image) {

            const double perspective_coeff = 1.05;

            Image PerspectiveImage = new Image() {
                Width = image.Width + airplane_x * perspective_coeff,
                Height = image.Height + airplane_x * perspective_coeff,
                Name = "NewPersEl" + image.Name,
                Source = image.Source,
            };

            return PerspectiveImage;
        }

        /// <summary> Отрисовка изображения </summary>
        private void DrawImage(Canvas canvas, double x, double y, Image image)
        {

            double Width = image.Width;

            Image newImage = new Image()
            {
                Width = image.Width,
                Height = image.Height,
                Name = "new" + image.Name,
                Source = image.Source,
            };

            Canvas.SetLeft(newImage, x);
            Canvas.SetTop(newImage, y);

            canvas.Children.Add(newImage);
        }
    }
}
