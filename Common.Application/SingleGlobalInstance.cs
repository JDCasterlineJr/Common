using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Common.Application
{
    public class SingleGlobalInstance : IDisposable
    {
        public static SingleGlobalInstance AcquireWithin(TimeSpan timeout, string appGuid = null)
        {
            return new SingleGlobalInstance(timeout, appGuid);
        }
        private readonly Mutex _mutex;
        public bool HasHandle { get; private set; }

        private SingleGlobalInstance(TimeSpan timeout, string guid)
        {
            // get application GUID as defined in AssemblyInfo.cs
            string appGuid = guid ?? ((GuidAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;

            // unique id for global mutex - Global prefix means it is global to the machine
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);

            // Need a place to store a return value in Mutex() constructor call

            // edited by Jeremy Wiebe to add example of setting up security for multi-user usage
            // edited by 'Marc' to work also on localized systems (don't use just "Everyone") 
            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            try
            {
                bool createdNew;
                _mutex = new Mutex(false, mutexId, out createdNew, securitySettings);
                try
                {
                    HasHandle = _mutex.WaitOne(timeout, false);
                }
                catch (AbandonedMutexException)
                {
                    HasHandle = true;
                }
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            // Doesn't need to be "thread-safe" with disposing of a mutex because:
            // https://msdn.microsoft.com/en-us/library/system.threading.mutex
            // "The Mutex class enforces thread identity, so a mutex can be
            //   released only by the thread that acquired it. By contrast,
            //   the Semaphore class does not enforce thread identity."
            if (_mutex == null) return;
            if (HasHandle) _mutex.ReleaseMutex();
            HasHandle = false;
            _mutex.Close();
        }
    }
}
