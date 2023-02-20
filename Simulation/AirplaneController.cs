using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorWPF
{
    [DataContract]
    /// <summary>
    /// Класс отвечающий за управление моделью самолета и вывод информации о ней.
    /// </summary>
    public class AirplaneController
    {
        [DataMember]
        ///<value> Модель самолета </value>
        private AirplaneModel _airplane;
        [DataMember]
        ///<value> Проверяющий на аварийные ситуации класс </value>
        private AirplaneState _airplale_state;

        [DataMember]
        ///<value> Коэф. для управления углом поворота </value>
        private double _angle_control = 0.4;

        [DataMember]
        ///<value> Коэф. для управления углом возвышения/понижения </value>
        private double _angle_climb_control = 1;

        public AirplaneController(double mass, double engine_power, double wingspan)
        {
            _airplane = new AirplaneModel(mass, engine_power, wingspan);
            _airplale_state = new AirplaneState();
        }

        /// <summary> Поведение обькта с течением времени </summary>
        public void Update()
        {
            _airplane.Tick();
        }

        /// <summary> Проверка моедли на аварийные ситуации </summary>
        public bool SimulationStatusChecked() => _airplale_state.AccidentChecked(_airplane);

        public void AddPower() => _airplane.Current_power += 20;
        public void ReducePower() => _airplane.Current_power -= 20;
        public void TurnAngle(bool plus) => _airplane.Angle += !plus ? _angle_control : -_angle_control;
        public void Сlimb(bool plus) => _airplane.AngleClimb += plus ? _angle_climb_control : -_angle_climb_control;
        public void ChassisStatusChange() => _airplane.Chassis = !_airplane.Chassis;

        public double GetSpeed() => _airplane.GetSpeed();
        public bool ChassisChecked() => _airplane.Chassis;
        public double GetClimbAngle() => _airplane.AngleClimb;
        public double GetAngle() => _airplane.Angle;
        public double GetHeight() => _airplane.Height;
        public double GetX() => _airplane.X;
        public double GetY() => _airplane.Y;
        public double GetAcaccelerationX() => _airplane.GetAcaccelerationX();
        public double GetAcaccelerationY() => _airplane.GetAcaccelerationY();
        public double GetAcaccelerationHeight() => _airplane.GetAcaccelerationHeight();
        public double GetPower() => _airplane.GetPowerBar();
    }
}
