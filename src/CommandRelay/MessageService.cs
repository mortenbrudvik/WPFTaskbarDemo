using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommandRelay;

public class MessageService
{
    public void Send(string message)
    {
        Process proc = Process.GetCurrentProcess();
        Process[] processes = Process.GetProcessesByName("WPFTaskbarDemoUI");

        if (processes.Length > 0)
        {
            foreach (Process p in processes)
            {
                if (p.Id != proc.Id)
                {
                    COPYDATASTRUCT copyData = new COPYDATASTRUCT();
                    copyData.dwData = new IntPtr(2);    // Just a number to identify the data type
                    copyData.cbData = message.Length + 1;  // One extra byte for the \0 character
                    copyData.lpData = Marshal.StringToHGlobalAnsi(message);

                    // Allocate memory for the data and copy
                    IntPtr ptrCopyData = IntPtr.Zero;
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);
                    SendMessage(p.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                    
                }
            }
        }
    }
    const uint WM_COPYDATA = 0x004A;
    
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
}

[StructLayout(LayoutKind.Sequential)]
public struct COPYDATASTRUCT
{
    // User defined data to be passed to the receiving application.
    public IntPtr dwData;

    // The size, in bytes, of the data pointed to by the lpData member.
    public int cbData;

    // The data to be passed to the receiving application. This member can be IntPtr.Zero.
    public IntPtr lpData;
}