using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Struct of constants for translating pulse widths to angles and angles to pulse widths.s
     * </summary>
     * 
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     */
    struct PulseWidthConstants
    {
        /**
         * <summary>Multiplier M for linear function of angle to pulse width: PW = M * A + C</summary>
         */
        double Multiplier { private get; public set; }

        /**
         * <summary>Constant C for linear function of angle to pulse width: PW = M * A + C</summary>
         */
        ulong Constant { private get; public set; }

        /**
         * <summary>
         * Constructor with arguments
         * </summary>
         * <param name="C">Constant value</param>
         * <param name="M">Multiplier value</param>
         */
        public PulseWidthConstants(double M, ulong C)
        {
            Multiplier = M;
            Constant = C;
        }
        
        /**
         * <summary>
         * Convert an Angle to a Pulse Width
         * </summary>
         * <param name="a">Angle to convert</param>
         * <returns>Pulse width</returns>
         */
        public ulong AngleToPulseWidth(double a)
        {
            return (ulong)(a * Multiplier) + Constant;
        }

        /**
         * <summary>
         * Convert a Pulse Width to an Angle
         * </summary>
         * <param name="pw">Pulse width to convert</param>
         * <returns>Angle value</returns>
         */
        public double PulseWidthToAngle(ulong pw)
        {
            return (pw - Constant) / Multiplier;
        }
    }
        

    /**
     * <summary>
     * A struct that contains the angles of the robot
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * <seealso cref="RoboticAngle"/>
     */
    struct AngleSet
    {
        
        /**
         * <summary>
         * Map of Robotic Angles to double angles.
         * </summary>
         * <remarks>
         * Angles are in the range -Math.PI to Math.PI
         * </remarks>
         */
        public Dictionary<RoboticAngle, double> AngleMap { get; set; }

        /**
         * <summary>
         * Default Constructor
         * </summary>
         */
        public AngleSet()
        {
            AngleMap = new Dictionary<RoboticAngle, double>();
        }

        /**
         * <summary>
         * Translate the Angle Map to a pulse width map given a struct of Pulse Width Constants
         * </summary>
         * <param name="pwc">Pulse Width Constant for pulse width translation</param>
         * <returns>Pulse Width Dictionary Map</returns>
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
         * <summary>
         * Set the Angle Map by setting a Pulse Width Map given a struct of Pulse Width Constants
         * </summary>
         * <param name="PulseWidthMap">Map of roboticangles to pulse widths</param>
         * <param name="pwc">Pulse Width Constant for pulse width translation</param>
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
