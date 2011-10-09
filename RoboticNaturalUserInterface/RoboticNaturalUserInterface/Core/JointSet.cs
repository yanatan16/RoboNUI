using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{

    /**
     * <summary>
     * A struct that contains the joint positions of the controller
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * <seealso cref="ControllerJoints"/>
     * <seealso cref="Position3d"/>
     */
    public class JointSet
    {
        /**
         * <summary>
         * Construct the JointSet.
         * </summary>
         */
        public JointSet()
        {
            JointMap = new Dictionary<ControllerJoints, Position3d>();
        }
        
        /**
         * <summary>
         * Map of Controller Joints to Positions.
         * </summary>
         */
        public Dictionary<ControllerJoints, Position3d> JointMap { get; set; }

        public string ToString()
        {
            string str = "JointSet{ ";
            int i = 0;
            foreach (KeyValuePair<ControllerJoints, Position3d> pair in JointMap)
            {
                str += "(" + pair.Key.ToString() + "," + pair.Value.ToString() + ")";
                if (++i < JointMap.Count)
                    str += ", ";
            }
            str += " }";
            return str;
        }



    }
}
