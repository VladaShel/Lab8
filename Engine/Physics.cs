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
    /// Класс отвечающий физические свойство объекта
    /// </summary>
    public class Physics : Transform
    {
        [DataMember]
        ///<value> Значение гравитации </value>
        private const double _gravity = 2;
        [DataMember]
        ///<value> Значение коэф. силы трения </value>
        private const double _friction_force = 0.03;

        [DataMember]
        ///<value> Максимальная высота для сильного ускорения по высоте </value>
        private const double _max_height_for_acceleration = 2800;

        [DataMember]
        ///<value> Значение коэф. сопротивления воздуха </value>
        private const double _windage_coef = 0.2;
        [DataMember]
        ///<value> Максимальная скорость физ. модели </value>
        private const double _speed_limit = 300;

        [DataMember]
        ///<value> Ускорение ось X </value>
        private double _acceleration_x;
        [DataMember]
        ///<value> Ускорение ось Y </value>
        private double _acceleration_y;
        [DataMember]
        ///<value> Ускорение по высоте </value>
        private double _height_acceleration;

        [DataMember]
        ///<value> Ускорение скорость по оси X </value>
        private double _speed_x;
        [DataMember]
        ///<value> Ускорение скорость по оси Y </value>
        private double _speed_y;
        [DataMember]
        ///<value> Ускорение скорость по высоте </value>
        private double _speed_height;

        [DataMember]
        /// <value> Статус: объект на земле ? </value>
        private bool _on_ground;

        [DataMember]
        ///<value> Масса модели </value>
        private double _mass;

        /// <summary> Уменьшение модуля значения </summary>
        private double ReduceAbs(double x, double delta)
        {
            double result = x > 0 ? x - delta : x + delta;
            return Math.Sign(result) == Math.Sign(x) ? result : 0;
        }

        /// <summary> Расчет взаимодействия со внешними силами: сила трения и сопр. воздуха </summary>
        private double ExternalForces(double speed, double acceleration)
        {
            if (speed != 0)
            {
                acceleration = (_on_ground) ? (ReduceAbs(acceleration, _friction_force)):(acceleration);
                acceleration = ReduceAbs(acceleration, Math.Abs(speed) / _mass);
            }
            return acceleration;
        }

        /// <summary> Расчет взаимодействия со внешними силами: сила трения и сопр. воздуха </summary>
        private double ExternalForces(double speed, double acceleration, double value)
        {
            if (speed != 0)
            {
                //acceleration = (_on_ground) ? (ReduceAbs(acceleration, _friction_force)):(acceleration);
                acceleration = ReduceAbs(acceleration, Math.Abs(speed) / value);
            }
            return acceleration;
        }

        public Physics(double mass_value) : base()
        {
            _acceleration_x = 0;
            _acceleration_y = 0;
            _height_acceleration = 0;

            _speed_x = 0;
            _speed_y = 0;

            _on_ground = true;
            _mass = mass_value;
        }

        /// <summary> Поведение обькта с течением времени </summary>
        public virtual void Tick()
        {
            _acceleration_x = ExternalForces(_speed_x, _acceleration_x);
            _acceleration_y = ExternalForces(_speed_y, _acceleration_y, 100f);
            if (_height_acceleration != -1)
                _height_acceleration = ExternalForces(_speed_height, _height_acceleration);

            if (_acceleration_x == 0)
                _speed_x = ReduceAbs(_speed_x, Math.Abs(_speed_x) / _mass);

            if (_acceleration_y == 0)
                _speed_y = ReduceAbs(_speed_y, Math.Abs(_speed_y) * Math.Pow(_windage_coef, 2));

            if(Height > _max_height_for_acceleration)
                _speed_height = ReduceAbs(_speed_height, Math.Abs(_speed_height) * _windage_coef);
            else if (_height_acceleration == 0)
                _speed_height = ReduceAbs(_speed_height, Math.Abs(_speed_height) * Math.Pow(_windage_coef, 2));

            _speed_x += _acceleration_x;
            _speed_y += _acceleration_y;

            _speed_x = _speed_x > _speed_limit ? _speed_limit : _speed_x;
            _speed_y = _speed_y > _speed_limit ? _speed_limit : _speed_y;

            if (Height > 1)
                _on_ground = false;
            else
                _on_ground = true;

            _speed_height += _height_acceleration;

            if (_height_acceleration == -1)
                _speed_height = -_gravity;

            ShiftPosition(_speed_x, _speed_y);

            Height += _speed_height;

            Console.WriteLine($"Ускорение X: {_acceleration_x}");
            Console.WriteLine($"Ускорение Y: {_acceleration_y}");
            Console.WriteLine($"Ускорение высота: {_height_acceleration}");

            Console.WriteLine($"Скорость X: {_speed_x}");
            Console.WriteLine($"Скорость Y: {_speed_y}");
            Console.WriteLine($"Скорость высота: {_speed_height}");
        }

        public virtual void AddForceX(double force_value) => _acceleration_x += force_value / _mass;

        public virtual void AddForceY(double force_value) => _acceleration_y += force_value / _mass;

        public virtual void AddForce(double force_value_x, double force_value_y)
        {
            AddForceX(force_value_x);
            AddForceY(force_value_y);
        }

        public virtual void Height_acceleration(double value) => _height_acceleration = value != -1 ? value / _mass : -1;

        public double GetSpeed() => Math.Sqrt(Math.Pow(_speed_x, 2) + Math.Pow(_speed_y, 2));

        public double GetAcacceleration() => Math.Sqrt(Math.Pow(_acceleration_x, 2) + Math.Pow(_acceleration_y, 2));

        public double GetAcaccelerationX() => _speed_x;
        public double GetAcaccelerationY() => _speed_y;
        public double GetAcaccelerationHeight() => _speed_height;

    }
    
}
