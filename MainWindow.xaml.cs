using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;


namespace FlightSimulatorWPF
{
    /// <summary>
    /// Главное меню
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadButton.Visibility = (File.Exists("SimulationSave.json")) ? (Visibility.Visible):(Visibility.Hidden);

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SimulationWindow simulation = new SimulationWindow();
            simulation.ShowDialog();
            this.Hide();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            AirplaneController load_simulation;
            try
            {
                DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(AirplaneController));

                using (FileStream fileStream = new FileStream("SimulationSave.json", FileMode.Open))
                {
                    load_simulation = (AirplaneController)jsonF.ReadObject(fileStream);
                }

                SimulationWindow simulation = new SimulationWindow(load_simulation);
                simulation.ShowDialog();
                this.Hide();
            }
            catch { 

            }
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e)
        {
            ControlInformationWindow window = new ControlInformationWindow();
            window.ShowDialog();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
