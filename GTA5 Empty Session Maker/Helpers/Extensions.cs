using System;
using System.Runtime.InteropServices;

namespace GTA5_Empty_Session_Maker.Helpers
{
    internal static class Extensions
    {
        public static PROCESSENTRY32 GetProcess(this string procName)
        {
            PROCESSENTRY32 pEntry = new PROCESSENTRY32((uint)Marshal.SizeOf(typeof(PROCESSENTRY32)));

            bool found = false;

            IntPtr hSnapshot = NativeMethods.CreateToolhelp32Snapshot(TH32CS.SNAPPROCESS, 0);

            if (hSnapshot != IntPtr.Zero)
            {
                do
                {
                    if (pEntry.szExeFile == procName)
                    {
                        found = true;

                        break;
                    }
                } while (NativeMethods.Process32Next(hSnapshot, ref pEntry));

                NativeMethods.CloseHandle(hSnapshot);
            }

            if (found)
            {
                return pEntry;
            }

            return default;
        }

        public static THREADENTRY32 GetThread(this PROCESSENTRY32 pEntry)
        {
            THREADENTRY32 tEntry = new THREADENTRY32((uint)Marshal.SizeOf(typeof(THREADENTRY32)));

            bool found = false;

            IntPtr hSnapshot = NativeMethods.CreateToolhelp32Snapshot(TH32CS.SNAPTHREAD, 0);

            if (hSnapshot != IntPtr.Zero)
            {
                do
                {
                    if (tEntry.th32OwnerProcessID == pEntry.th32ProcessID)
                    {
                        found = true;

                        break;
                    }
                } while (NativeMethods.Thread32Next(hSnapshot, ref tEntry));

                NativeMethods.CloseHandle(hSnapshot);
            }

            if (found)
            {
                return tEntry;
            }

            return default;
        }

        public static bool IsAlive(this PROCESSENTRY32 pEntry)
        {
            IntPtr pHandle = NativeMethods.OpenProcess(PROCESS.ALL_ACCESS, false, pEntry.th32ProcessID);

            bool isAlive = pHandle != IntPtr.Zero;

            if (isAlive)
            {
                NativeMethods.CloseHandle(pHandle);
            }

            return isAlive;
        }

        public static IntPtr OpenHandle(this PROCESSENTRY32 pEntry)
        {
            IntPtr handle = NativeMethods.OpenProcess(PROCESS.ALL_ACCESS, false, pEntry.th32ProcessID);

            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Unable To Open Game Process\n");

                Console.ReadLine();

                Environment.Exit(-1);
            }

            return handle;
        }

        public static IntPtr OpenHandle(this THREADENTRY32 tEntry)
        {
            IntPtr handle = NativeMethods.OpenThread(THREAD.ALL_ACCESS, false, tEntry.th32ThreadID);

            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Unable To Open Game Thread\n");

                Console.ReadLine();

                Environment.Exit(-1);
            }

            return handle;
        }
    }
}