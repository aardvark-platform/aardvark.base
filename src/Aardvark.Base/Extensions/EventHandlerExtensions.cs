/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;

namespace Aardvark.Base;

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
