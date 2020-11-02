using System;
using System.Runtime.InteropServices;

namespace GTA5_Empty_Session_Maker.Helpers
{
    internal static class Extensions
    {
        #region Public Methods

        public static uint GetProcessID(this string procName)
        {
            PROCESSENTRY32 process = new PROCESSENTRY32((uint)Marshal.SizeOf(typeof(PROCESSENTRY32)));

            IntPtr hSnapshot = NativeMethods.CreateToolhelp32Snapshot(TH32CS.SNAPPROCESS, 0);

            uint pID = 0;

            if (hSnapshot != IntPtr.Zero)
            {
                do
                {
                    if (process.szExeFile == procName)
                    {
                        pID = process.th32ProcessID;

                        break;
                    }
                } while (NativeMethods.Process32Next(hSnapshot, ref process));

                NativeMethods.CloseHandle(hSnapshot);
            }

            return pID;
        }

        public static IntPtr GetThreadHandle(this uint pID)
        {
            THREADENTRY32 thread = new THREADENTRY32((uint)Marshal.SizeOf(typeof(THREADENTRY32)));

            IntPtr hSnapshot = NativeMethods.CreateToolhelp32Snapshot(TH32CS.SNAPTHREAD, 0);

            IntPtr tHandle = IntPtr.Zero;

            if (hSnapshot != IntPtr.Zero)
            {
                do
                {
                    if (thread.th32OwnerProcessID == pID)
                    {
                        tHandle = NativeMethods.OpenThread(THREAD.ALL_ACCESS, false, thread.th32ThreadID);

                        break;
                    }
                } while (NativeMethods.Thread32Next(hSnapshot, ref thread));

                NativeMethods.CloseHandle(hSnapshot);
            }

            return tHandle;
        }

        #endregion Public Methods
    }
}