using System;

namespace Aardvark.Base
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Safely invokes the event using null as sender and EventArgs.Empty
        /// </summary>
        public static void TryInvoke(this EventHandler eh)
        {
            eh?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Safely invokes the event using the supplied sender with EventArgs.Empty
        /// </summary>
        public static void TryInvoke(this EventHandler eh, object sender)
        {
            eh?.Invoke(sender, EventArgs.Empty);
        }
    }
}
