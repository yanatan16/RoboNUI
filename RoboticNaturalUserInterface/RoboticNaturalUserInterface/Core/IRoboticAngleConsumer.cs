using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Interface for receiving updated angles from the JAT.
     * </summary>
     * <remarks>Author: Jon Eisen (jonathan.eisen@ngc.com)</remarks>
     */
    interface IRoboticAngleConsumer
    {

        /**
         * <summary>Update the angles to this consumer<summary>
         * 
         * <param name="angles">Angle set to update this consumer with</param>
         */
        public void UpdateAngles(AngleSet angles);
    }
}
