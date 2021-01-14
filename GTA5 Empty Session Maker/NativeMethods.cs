using System;
using System.Runtime.InteropServices;

namespace GTA5_Empty_Session_Maker
{
    [Flags]
    internal enum PROCESS : int
    {
        TERMINATE = 0x0001,
        CREATE_THREAD = 0x0002,
        VM_OPERATION = 0x0008,
        VM_READ = 0x0010,
        VM_WRITE = 0x0020,
        DUP_HANDLE = 0x0040,
        CREATE_PROCESS = 0x0080,
        SET_QUOTA = 0x0100,
        SET_INFORMATION = 0x0200,
        QUERY_INFORMATION = 0x0400,
        SUSPEND_RESUME = 0x0800,
        QUERY_LIMITED_INFORMATION = 0x1000,
        SYNCHRONIZE = 0x00100000,
        ALL_ACCESS = CREATE_PROCESS
            | CREATE_THREAD
            | DUP_HANDLE
            | QUERY_INFORMATION
            | QUERY_LIMITED_INFORMATION
            | SET_INFORMATION
            | SET_QUOTA
            | SUSPEND_RESUME
            | TERMINATE
            | VM_OPERATION
            | VM_READ
            | VM_WRITE
            | SYNCHRONIZE
    }

    internal enum PROCESSPRIORITY : int
    {
        NORMAL = 0x00000020,
        IDLE = 0x00000040,
        HIGH = 0x00000080,
        REALTIME = 0x00000100,
        BELOW_NORMAL = 0x00004000,
        ABOVE_NORMAL = 0x00008000,
        BACKGROUND_BEGIN = 0x00100000,
        BACKGROUND_END = 0x00200000
    }

    [Flags]
    internal enum TH32CS : uint
    {
        SNAPHEAPLIST = 0x00000001,
        SNAPPROCESS = 0x00000002,
        SNAPTHREAD = 0x00000004,
        SNAPMODULE = 0x00000008,
        SNAPMODULE32 = 0x00000010,
        SNAPALL = SNAPHEAPLIST | SNAPMODULE | SNAPMODULE32 | SNAPPROCESS | SNAPTHREAD,
        INHERIT = 0x80000000
    }

    [Flags]
    internal enum THREAD : int
    {
        TERMINATE = 0x0001,
        SUSPEND_RESUME = 0x0002,
        GET_CONTEXT = 0x0008,
        SET_CONTEXT = 0x0010,
        SET_INFORMATION = 0x0020,
        QUERY_INFORMATION = 0x0040,
        SET_THREAD_TOKEN = 0x0080,
        IMPERSONATE = 0x0100,
        DIRECT_IMPERSONATION = 0x0200,
        SET_LIMITED_INFORMATION = 0x0400,
        QUERY_LIMITED_INFORMATION = 0x0800,
        SYNCHRONIZE = (int)0x00100000L,
        ALL_ACCESS = SYNCHRONIZE
            | DIRECT_IMPERSONATION
            | GET_CONTEXT
            | IMPERSONATE
            | QUERY_INFORMATION
            | QUERY_LIMITED_INFORMATION
            | SET_CONTEXT
            | SET_INFORMATION
            | SET_LIMITED_INFORMATION
            | SET_THREAD_TOKEN
            | SUSPEND_RESUME
            | TERMINATE
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public uint dwFlags;

        public IntPtr th32DefaultHeapID;

        public int pcPriClassBase;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;

        public PROCESSENTRY32(uint dwSize) : this()
        {
            this.dwSize = dwSize;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(PROCESSENTRY32 pEntry, PROCESSENTRY32 _pEntry)
        {
            return pEntry.szExeFile == _pEntry.szExeFile
                && pEntry.th32ProcessID == _pEntry.th32ProcessID;
        }

        public static bool operator !=(PROCESSENTRY32 pEntry, PROCESSENTRY32 _pEntry)
        {
            return !(pEntry == _pEntry);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct THREADENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ThreadID;
        public uint th32OwnerProcessID;
        public uint tpBasePri;
        public uint tpDeltaPri;
        public uint dwFlags;

        public THREADENTRY32(uint dwSize) : this()
        {
            this.dwSize = dwSize;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(THREADENTRY32 tEntry, THREADENTRY32 _tEntry)
        {
            return tEntry.th32OwnerProcessID == _tEntry.th32OwnerProcessID
                && tEntry.th32ThreadID == _tEntry.th32ThreadID;
        }

        public static bool operator !=(THREADENTRY32 tEntry, THREADENTRY32 _tEntry)
        {
            return !(tEntry == _tEntry);
        }
    }

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr Handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(TH32CS dwFlags, uint th32ProcessID);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(PROCESS Access, bool bInheritHandle, uint processID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(THREAD Access, bool bInheritHandle, uint threadID);

        [DllImport("kernel32.dll")]
        public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lppe);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}