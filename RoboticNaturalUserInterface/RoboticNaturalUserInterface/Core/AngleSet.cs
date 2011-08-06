using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * Static struct of constants for translating pulse widths to angles and inverse
     * 
     * Consider this a global variable set by which servo controller pulse width user is currently on.
     */
    struct PulseWidthConstants
    {
        public static double Multiplier = 1500 / Math.PI;
        public static ulong Constant = 1500;

        /**
         * Convert an Angle to a Pulse Width
         */
        public static ulong AngleToPulseWidth(double a)
        {
            return (ulong)(a * Multiplier) + Constant;
        }

        /**
         * Convert a Pulse Width to an Angle
         */
        public static double PulseWidthToAngle(ulong pw)
        {
            return (pw - Constant) / Multiplier;
        }
    }
        

    /**
     * Angle Set
     * 
     * A single struct that contains the angles of the robot
     */
    struct AngleSet
    {
        /**
         * Mapping of RoboNUI Robotic Angles to pulse width values
         * Pulse Width values stored as ulong, range 0 - 2 * PulseWidthConstants.Constant
         */
        public Dictionary<RoboticAngle, ulong> PulseWidthMap {
            get
            {
                PulseWidthMap.Clear();
                for (Dictionary<RoboticAngle, double>.Enumerator eAngle = AngleMap.GetEnumerator(); eAngle.MoveNext(); )
                {
                    PulseWidthMap[eAngle.Current.Key] = PulseWidthConstants.AngleToPulseWidth(eAngle.Current.Value);
                }
                return PulseWidthMap;
            }

            set
            {
                AngleMap.Clear();
                for (Dictionary<RoboticAngle, ulong>.Enumerator ePW = PulseWidthMap.GetEnumerator(); ePW.MoveNext(); )
                {
                    AngleMap[ePW.Current.Key] = PulseWidthConstants.PulseWidthToAngle(ePW.Current.Value);
                }
            }
        }

        /**
         * Angle Map
         * 
         * Map of Robotic Angles to double angles.
         * Angles are in the range -Math.PI to Math.PI
         */
        public Dictionary<RoboticAngle, double> AngleMap { get; set; }

        /**
         * Construct the struct
         */
        public AngleSet()
        {
            AngleMap = new Dictionary<RoboticAngle, double>();
            PulseWidthMap = new Dictionary<RoboticAngle, ulong>();
        }

        /** 
         * Set the map with a list of keys and values
         * 
         * Parameters: key array and val array (must be same size)
         */
        public void setMap(RoboticAngle[] keyArray, ulong[] valArray)
        {
            for (int i = 0; i < keyArray.Length; i++)
            {
                PulseWidthMap[keyArray[i]] = valArray[i];
            }
        }

        private void translateAngleToPulseWidth()
        {
            PulseWidthMap.Clear();
            for (Dictionary<RoboticAngle, double>.Enumerator eAngle = AngleMap.GetEnumerator(); eAngle.MoveNext(); )
            {
                PulseWidthMap[eAngle.Current.Key] = (ulong) (eAngle.Current.Value * PulseWidthMax;
            }
        }

        private void translatePulseWidthToAngle()
        {
            
        }



    }
}
