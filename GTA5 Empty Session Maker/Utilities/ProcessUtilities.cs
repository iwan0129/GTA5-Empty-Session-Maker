﻿using System;

namespace GTA5_Empty_Session_Maker.Utilities
{
    internal static class ProcessUtilities
    {
        public static PROCESSENTRY32 GetProcess(this string procName)
        {
            PROCESSENTRY32 pEntry = new(PROCESSENTRY32.Size);

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

            return found
                ? pEntry
                : default;
        }

        public static THREADENTRY32 GetThread(this PROCESSENTRY32 pEntry)
        {
            THREADENTRY32 tEntry = new(THREADENTRY32.Size);

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

            return found
                ? tEntry
                : default;
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
            return NativeMethods.OpenProcess(PROCESS.ALL_ACCESS, false, pEntry.th32ProcessID);
        }

        public static IntPtr OpenHandle(this THREADENTRY32 tEntry)
        {
            return NativeMethods.OpenThread(THREAD.ALL_ACCESS, false, tEntry.th32ThreadID);
        }
    }
}