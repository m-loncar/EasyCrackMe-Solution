using System.Diagnostics;

namespace EasyCrackMe
{
    class Program
    {
        static void Main()
        {
            string procName = "EasyCrackMe";
            Process? targetProc = Process.GetProcessesByName(procName).FirstOrDefault();
            if (targetProc == null)
            {
                throw new Exception($"Process {procName} is not running.");
            }

            IntPtr handle = NativeMethods.ProcessApi.OpenProcess(NativeMethods.ProcessApi.ProcessAccessFlags.All, false,
                targetProc.Id);
            if (targetProc == null)
            {
                throw new Exception($"Could not open a handle to {procName}");
            }

            IntPtr mainModuleBaseAddress = targetProc.MainModule.BaseAddress;
            if (mainModuleBaseAddress == IntPtr.Zero)
            {
                throw new Exception("Unable to get the main module base address");
            }

            byte[] newInstructionBytes = new byte[]
            {
                0xEB,     // jmp
                0x7E      // Relative offset (0x13C1 (location of the print for the correct entry) - 0x1341 (location we're jumping from)).
            };
            
            IntPtr addressToPatch = mainModuleBaseAddress + 0x1341; // Address of the je (jump if equal) instruction after the password validation

            NativeMethods.MemoryApi.WriteProcessMemory(handle, addressToPatch, newInstructionBytes, newInstructionBytes.Length, out _);

            Console.WriteLine("Patch applied. Enter any password into the EasyCrackMe application.\nSuccess message will not be shown. The application will immediately exit if the password is correct.");
            Console.ReadKey();
        }
    }
}