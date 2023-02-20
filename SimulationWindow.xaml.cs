using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlightSimulatorWPF
{
    /// <summary>
    /// Окно симуляции
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private AirplaneController _airplaneController;
        private DispatcherTimer _timer;

        private ViewEngine _view;

        /// <summary> Инициализация </summary>
        private void Initialization() {
            AccidentLabel.Visibility = Visibility.Hidden;

            _view = new ViewEngine(ViewCanvas, CabinPanel);

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Update);
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();

            _view.DrawView(_airplaneController.GetHeight(), _airplaneController.GetAngle(), _airplaneController.GetX(), _airplaneController.GetY());
        }

        public SimulationWindow()
        {
            InitializeComponent();

            _airplaneController = new AirplaneController(250, 800, 8);
            Initialization();
        }

        public SimulationWindow(AirplaneController airplaneController) {
            InitializeComponent();
            _airplaneController = airplaneController;
            Initialization();
        }

        /// <summary> Поведение со временем </summary>
        public void Update(object sender, EventArgs e) {
            _airplaneController.Update();
            var controll = _airplaneController;
            _view.DrawPanel(controll.GetSpeed(), -controll.GetClimbAngle(), controll.GetHeight(), controll.GetAcaccelerationY(), controll.GetAcaccelerationHeight(), controll.GetPower(), controll.ChassisChecked());
            _view.DrawView(controll.GetHeight(), controll.GetAngle(), controll.GetX(), controll.GetY());

            if (_airplaneController.SimulationStatusChecked())
            {
                _timer.Stop();
                AccidentLabel.Visibility = Visibility.Visible;
                if (File.Exists("SimulationSave.json"))
                    File.Delete("SimulationSave.json");
            }
        }

        /// <summary> Управление </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: _airplaneController.AddPower(); break;
                case Key.Back: _airplaneController.ReducePower(); break;
                case Key.Right: _airplaneController.TurnAngle(true); break;
                case Key.Left: _airplaneController.TurnAngle(false); break;
                case Key.Up: _airplaneController.Сlimb(true); break;
                case Key.Down: _airplaneController.Сlimb(false); break;
                case Key.Tab: _airplaneController.ChassisStatusChange(); break;
                case Key.CapsLock: ShowControllWindow(); break;
            }
        }

        /// <summary> Действия при закрытии окна - сериализация </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(AirplaneController));

                using (FileStream fileStream = new FileStream("SimulationSave.json", FileMode.Create))
                {
                    jsonF.WriteObject(fileStream, _airplaneController);
                }
            }
            catch
            {

            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void ShowControllWindow() {
            ControlInformationWindow window = new ControlInformationWindow();
            window.ShowDialog();
        }
    }
}
