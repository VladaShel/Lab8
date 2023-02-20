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
    /// Класс отвечающий за поведение модели самолета
    /// </summary>
    public class AirplaneModel : Physics
    {
        /// 
        ///                    0
        ///                  * * *   90 - _coeff
        ///  270 + _coeff  *        *
        ///              *           *
        ///         270  *           * 90 
        ///              *           *
        ///  270 - _coeff  *       * 90 + _coeff
        ///                  * * *
        ///                   180
        /// 
        /// 

        [DataMember]
        /// <value> Коэф. для определения поворотных углов  </value>
        private const double _coefficient_rotation_angles = 20;

        [DataMember]
        ///<value> Мощность двигателя </value>
        private double _engine_power;
        [DataMember]
        ///<value> Используемая мощность </value>
        private double _current_power;

        [DataMember]
        ///<value> Статус шасси (раскрыты или нет) </value>
        public bool Chassis { get; set; }

        [DataMember]
        /// <value> Размах крыла </value>
        private double _wingspan;

        [DataMember]
        ///<value> Угол для поворота </value>
        private double _angle;

        [DataMember]
        ///<value> Угол для изменения высоты </value>
        private double _angle_climb;
        [DataMember]
        ///<value> Ограничения угла для изменения высоты </value>
        private const double _max_angle_climb_abs = 30;

        /// <summary> Расчет подъемной силы </summary>
        private double Lifting_force() => GetSpeed() > _wingspan * 10 ? GetSpeed() * (_wingspan * _angle_climb / 15) : -1;

        /// <summary> Поведение самолета при различных углах повора </summary>
        private void FlightDirection(double common_vector_direction)
        {
            var vector = common_vector_direction;
            switch (_angle)
            {
                case < 90 - 4 * _coefficient_rotation_angles:
                    AddForceX(vector); Height_acceleration(Lifting_force()); break;

                case < 90 - 3 * _coefficient_rotation_angles:
                    AddForce(vector * 0.90f, vector * 0.10f); Height_acceleration(Lifting_force() * 0.90f); break;

                case < 90 - _coefficient_rotation_angles:
                    AddForce(vector * 0.55f, vector * 0.30f); Height_acceleration(Lifting_force() * 0.70f); break;

                case < 90 + _coefficient_rotation_angles:
                    AddForce(vector * 0.40f, vector * 0.60f); break;

                case < 270 - _coefficient_rotation_angles:
                    AddForceX(vector); break;

                case < 270 + _coefficient_rotation_angles:
                    AddForce(vector * 0.40f, -vector * 0.60f); Height_acceleration(-Lifting_force()); break;

                case < 270 + 3 * _coefficient_rotation_angles:
                    AddForce(vector * 0.55f, -vector * 0.30f); Height_acceleration(Lifting_force() * 0.70f); break;

                case < 270 + 4 * _coefficient_rotation_angles:
                    AddForce(vector * 0.90f, -vector * 0.10f); Height_acceleration(Lifting_force() * 0.90f); break;

                case < 360:
                    AddForceX(vector); Height_acceleration(Lifting_force()); break;
            }
        }

        public AirplaneModel(double mass, double engine_power, double wingspan) : base(mass)
        {
            _engine_power = engine_power;
            _current_power = 0;
            _angle = 0;
            _angle_climb = 0;
            Chassis = true;
            _wingspan = wingspan;
        }

        /// <summary> Поведение обькта с течением времени </summary>
        public override void Tick()
        {
            FlightDirection(_current_power);
            base.Tick();

        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                if (value >= 0 && value < 360)
                    _angle = value;
                else if (value < 0)
                    _angle = 359;
                else
                    _angle = 0;
            }
        }

        public double AngleClimb
        {
            get { return _angle_climb; }
            set
            {
                if (value >= -_max_angle_climb_abs && value <= _max_angle_climb_abs)
                    _angle_climb = value;
                else if (value > _max_angle_climb_abs)
                    _angle_climb = _max_angle_climb_abs;
                else
                    _angle_climb = -_max_angle_climb_abs;
            }
        }

        public double Current_power
        {
            get { return _current_power; }
            set
            {
                if (value > _engine_power)
                    _current_power = _engine_power;
                else if (value < 0)
                    _current_power = 0;
                else
                    _current_power = value;
            }
        }

        public double GetCoefficientRotationAngles() => _coefficient_rotation_angles;

        private double Climb_Vector
        {
            get
            {
                var climb_vector = Math.Abs(_angle_climb / _max_angle_climb_abs);
                return climb_vector == 0 ? 1 : climb_vector;
            }
        }

        public override void AddForceX(double force_value)
        {
            force_value *= Climb_Vector;
            base.AddForceX(force_value);
        }

        public override void AddForceY(double force_value)
        {
            force_value *= Climb_Vector;
            base.AddForceY(force_value);
        }

        public override void AddForce(double force_value_x, double force_value_y)
        {
            force_value_x *= Climb_Vector;
            force_value_y *= Climb_Vector;
            base.AddForce(force_value_x, force_value_y);
        }

        public double GetPowerBar() => (_current_power / _engine_power) * 100;
    }
}
