using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Messaging
{
    /**
     * <summary>
     * This base class provides an interface for a component to provide a message to the system.
     * It will send objects of type T to registered <see cref="IConsumer{T}"/>.
     * </summary>
     * 
     * <remarks> Author: Jon Eisen (jon.m.eisen@gmail.com) </remarks>
     * <seealso cref="T:IConsumer"/>
     */
    public class Provider <T>
    {
        /**
         * <summary>
         * List of <see cref="T:IConsumer"/> registered to this provider
         * </summary>
         */
        private List<IConsumer<T>> Consumers;

        /**
         * <summary>
         * Boolean value representing if <see cref="Provider"/> is active. If false, no messages will be sent
         * </summary>
         */
        private bool Active;

        /**
         * <summary>
         * Constructor for the Provider
         * </summary>
         */
        protected Provider()
        {
            Consumers = new List<IConsumer<T> >();
            Active = false;
        }

        /**
         * <summary>Add a new consumer to the list</summary>
         * <param name="newConsumer">A consumer to add to the list</param>
         */
        public void AddConsumer(IConsumer<T> newConsumer)
        {
            Consumers.Add(newConsumer);
        }

        /**
         * <summary>Remove a consumer from the list</summary>
         * <param name="oldConsumer">A consumer to remove from the list</param>
         */
        public void RemoveConsumer(IConsumer<T> oldConsumer)
        {
            Consumers.Remove(oldConsumer);
        }

        /**
         * <summary>Clear the list of consumers</summary>
         */
        public void ClearAllConsumers()
        {
            Consumers.Clear();
        }

        /**
         * <summary>
         * Activate this <see cref="Provider"/>. Allow sending of messages.
         * </summary>
         */
        public virtual void Activate()
        {
            Active = true;
        }

        /**
         * <summary>
         * Deactivate this <see cref="Provider"/>. Disallow sending of messages.
         * </summary>
         */
        public virtual void Deactivate()
        {
            Active = false;
        }

        /**
         * <summary>
         * Send an object of type T to all consumers
         * </summary>
         * <param name="t">An object of type T to send to consumers</param>
         */
        protected void Send(T t)
        {
            if (Active)
            {
                foreach (IConsumer<T> con in Consumers)
                {
                    con.Update(t);
                }
            }
        }
    }
}
