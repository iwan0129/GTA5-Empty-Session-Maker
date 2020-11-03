using GTA5_Empty_Session_Maker.Helpers;
using System;
using System.Threading;

namespace GTA5_Empty_Session_Maker
{
    internal static class Program
    {
        #region Private Fields

        private const string Game = "GTA5.exe";

        private const int SuspendTime = 10000;
        private const int VK_RETURN = 0x0D;

        #endregion Private Fields

        #region Private Methods

        private static void Main()
        {
            uint pID;

            IntPtr tHandle;

            while (true)
            {
                if ((pID = Game.GetProcessID()) != 0 && (tHandle = pID.GetThreadHandle()) != IntPtr.Zero)
                {
                    Console.WriteLine("Press ENTER to make empty session only if you are currently in a session and not alt-tabbed from the game");

                    while (NativeMethods.GetAsyncKeyState(VK_RETURN) != -32767)
                    {
                        Thread.Sleep(1);
                    }

                    if (NativeMethods.SuspendThread(tHandle) != -1)
                    {
                        Thread.Sleep(SuspendTime);

                        NativeMethods.ResumeThread(tHandle);
                    }

                    NativeMethods.CloseHandle(tHandle);

                    Console.Clear();
                }

                Thread.Sleep(1);
            }
        }

        #endregion Private Methods
    }
}