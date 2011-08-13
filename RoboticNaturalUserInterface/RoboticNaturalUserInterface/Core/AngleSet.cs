using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * Struct of constants for translating pulse widths to angles and inverse
     * 
     * Passed in to getPulseWidthMap in AngleSet
     */
    struct PulseWidthConstants
    {
        /**
         * Multiplier M for linear function of angle to pulse width: PW = M * A + C
         */
        double Multiplier { private get; public set; }

        /**
         * Constant C for linear function of angle to pulse width: PW = M * A + C
         */
        ulong Constant { private get; public set; }

        /**
         * Constructor
         */
        public PulseWidthConstants(double M, ulong C)
        {
            Multiplier = M;
            Constant = C;
        }
        
        /**
         * Convert an Angle to a Pulse Width
         */
        public ulong AngleToPulseWidth(double a)
        {
            return (ulong)(a * Multiplier) + Constant;
        }

        /**
         * Convert a Pulse Width to an Angle
         */
        public double PulseWidthToAngle(ulong pw)
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
        }

        /**
         * Translate the Angle Map to a pulse width map given a struct of Pulse Width Constants
         * 
         * Parameters:
         *      Pulse Width Constant for pulse width translation
         *      
         * Returns: Pulse Width Dictionary Map
         */
        public Dictionary<RoboticAngle, ulong> getPulseWidthMap(PulseWidthConstants pwc)
        {
            Dictionary<RoboticAngle, ulong> PulseWidthMap = new Dictionary<RoboticAngle, ulong>();
            for (Dictionary<RoboticAngle, double>.Enumerator eAngle = AngleMap.GetEnumerator(); eAngle.MoveNext(); )
            {
                PulseWidthMap[eAngle.Current.Key] = pwc.AngleToPulseWidth(eAngle.Current.Value);
            }
            return PulseWidthMap;
        }

        /**
         * Set the Angle Map by setting a Pulse Width Map given a struct of Pulse Width Constants
         * 
         * Parameters:
         *      New Angle map in pulse widths
         *      Pulse Width Constant for pulse width translation
         */
        public void setPulseWidthMap(Dictionary<RoboticAngle, ulong> PulseWidthMap, PulseWidthConstants pwc)
        {
            AngleMap.Clear();
            for (Dictionary<RoboticAngle, ulong>.Enumerator ePW = PulseWidthMap.GetEnumerator(); ePW.MoveNext(); )
            {
                AngleMap[ePW.Current.Key] = pwc.PulseWidthToAngle(ePW.Current.Value);
            }
        }



    }
}
