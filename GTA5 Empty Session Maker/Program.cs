using GTA5_Empty_Session_Maker.Helpers;
using System;
using System.Threading;

namespace GTA5_Empty_Session_Maker
{
    internal static class Program
    {
        private const string Game = "GTA5.exe";

        private const int SuspendTime = 10000;
        private const int VK_RETURN = 0x0D;

        private static bool Notify { get; set; }

        private static void Main()
        {
            PROCESSENTRY32 gtaEntry = default;

            THREADENTRY32 gtaThread = default;

            while (true)
            {
                if (gtaEntry == default || !gtaEntry.IsAlive())
                {
                    gtaEntry = Game.GetProcess();

                    if ((gtaThread = gtaEntry.GetThread()) != default)
                    {
                        Notify = true;
                    }
                    else
                    {
                        Console.WriteLine("Unable to Get GTA5 Thread\n");

                        Console.ReadLine();

                        Environment.Exit(-1);
                    }
                }
                else
                {
                    if (Notify)
                    {
                        Console.WriteLine("Press ENTER to make empty session only and make sure you are not ALT-TAB-ed\n");
                        
                        Notify = false;
                    }

                    IntPtr handle = gtaEntry.OpenHandle();

                    while (NativeMethods.GetAsyncKeyState(VK_RETURN) != -32767 && NativeMethods.GetForegroundWindow() != handle)
                    {
                        Thread.Sleep(1);
                    }

                    NativeMethods.CloseHandle(handle);

                    handle = gtaThread.OpenHandle();

                    Console.WriteLine($"Attempting To Suspend Game Thread for {SuspendTime} miliseconds\n");

                    if (NativeMethods.SuspendThread(handle) != -1)
                    {
                        Thread.Sleep(SuspendTime);

                        Console.WriteLine("Resuming Game Thread\n");

                        NativeMethods.ResumeThread(handle);

                        Console.WriteLine("Game Thread Resumed\n");
                    }
                    else
                    {
                        Console.WriteLine("Unable To Suspend Game Thread\n");
                    }

                    NativeMethods.CloseHandle(handle);
                }

                Thread.Sleep(1);
            }
        }
    }
}