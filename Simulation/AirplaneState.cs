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
    /// Класс отвечающий за проверку модели самолета на аварийные ситуации
    /// </summary>
    public class AirplaneState
    {
        [DataMember]
        ///<value> Макс. возможная скорость с раскрытым шасси </value>
        private const double speed_with_chassis_limit = 250;
        [DataMember]
        /// <value> Коэф. для определения угла, аварийного для посадки  </value>
        private const int angle_coeff = 3;

        public AirplaneState()
        {

        }

        /// <summary> Проверка на аварию с шасси </summary>
        private bool ChassisСrashChecked(AirplaneModel airplane_status)
        {

            if (airplane_status.Chassis && airplane_status.GetSpeed() > speed_with_chassis_limit)
                return true;

            if (!airplane_status.Chassis && airplane_status.Height <= 1)
                return true;


            return false;
        }

        /// <summary> Авария - удар о землю </summary>
        private bool Hit_the_ground(AirplaneModel airplane_status)
        {

            double critical_angle_1 = 90 - airplane_status.GetCoefficientRotationAngles() * angle_coeff;
            double critical_angle_2 = 270 + airplane_status.GetCoefficientRotationAngles() * angle_coeff;

            if (airplane_status.Height <= 1 && airplane_status.Angle > critical_angle_1 && airplane_status.Angle < critical_angle_2)
                return true;
            else
                return false;
        }

        /// <summary> Проверка модели самолета на аварийные ситуации </summary>
        public bool AccidentChecked(AirplaneModel airplane_status) => ChassisСrashChecked(airplane_status) || Hit_the_ground(airplane_status);

    }
}
