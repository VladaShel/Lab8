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
    /// Класс отвечающий за позицию обьекта на условной координатной сетке и перемещение по ней
    /// </summary>
    public class Transform
    {
        [DataMember]
        ///<value> Координата по оси X </value>
        private double _x;
        [DataMember]
        ///<value> Координата по оси Y </value>
        private double _y;
        [DataMember]
        ///<value> Ограничения координатной сетки </value>
        private const double _max_field_size = double.PositiveInfinity;

        [DataMember]
        ///<value> Координата по оси высоты </value>
        private double _height;
        [DataMember]
        ///<value> Ограничения по оси высоты </value>
        private const double _max_height = 4500;


        public Transform()
        {
            _x = 0;
            _y = 0;
            _height = 0;
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (value >= 0 && value <= _max_height)
                    _height = value;
                else if (value < 0)
                    _height = 0;
                else
                    _height = _max_height;
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (value > _max_field_size)
                    _x = _max_field_size;
                else
                    _x = value;
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (value > _max_field_size)
                    _y = _max_field_size;
                else
                    _y = value;
            }
        }

        public void ShiftPosition(double delta_x, double delta_y)
        {
            X += delta_x;
            Y += delta_y;
        }

        public void SetPosition(double new_x, double new_y)
        {
            X = new_x;
            Y = new_y;
        }

    }
}
