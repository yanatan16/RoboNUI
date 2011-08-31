using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Messaging
{
    /**
     * <summary>
     * This interface provides a method for receiving messages from another component.
     * 
     * Generic: T is the class of message to be received
     * </summary>
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     */
    public interface IConsumer <T> 
    {

        /**
         * <summary>Update the object to this consumer</summary>
         * 
         * <param name="t">New update to consume</param>
         */
        void Update(T t);
    }
}
