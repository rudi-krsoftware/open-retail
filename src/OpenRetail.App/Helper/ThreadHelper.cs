/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OpenRetail.App.Helper
{
    /// <summary>
    /// Cancelable Alternative to Thread.Sleep Method
    /// referensi: http://urosv.blogspot.co.id/2010/10/cancelable-alternative-to-threadsleep.html
    /// </summary>
    public class ThreadHelper
    {
        private const int NotCanceled = 0;
        private const int Canceled = 1;

        private int _canceled = NotCanceled;
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(false); 

        /// <summary>
        /// Sleeps for the specified amount of time.
        /// It can wake up earlier if Cancel() is called during the sleep.
        /// Also, it returns immediately if Cancel() has already been called.
        /// </summary>
        /// <param name="millisecondsTimeout">Sleep interval in milliseconds.</param>
        /// <returns>True if sleep wasn't canceled, false otherwise.</returns>
        public bool Sleep(int millisecondsTimeout)
        {
            return !_manualResetEvent.WaitOne(millisecondsTimeout);
        }

        /// <summary>
        /// Cancels the current sleep operation (if there is one in progress),
        /// and causes all future sleep operations to return immediately when called.
        /// </summary>
        public void Cancel()
        {
            // Only one thread calling Cancel() can actually set the event.
            if (Interlocked.Exchange(ref _canceled, Canceled) == NotCanceled)
            {
                _manualResetEvent.Set();
            }
        }

        /// <summary>
        /// Returns true if light sleeper has been canceled.
        /// </summary>
        public bool HasBeenCanceled
        {
            get
            {
                return (_canceled == Canceled);
            }
        }        
    }
}
