using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace Main;

static class API
{
    [DllExport]
    public static int add2(int a, int b)
    {
        Console.WriteLine($"a={a} b={b}");
        return a + b;
    }
    static ThreadLocal<IntPtr> GreetingPtr = new ThreadLocal<IntPtr>();
    [DllExport]
    public static IntPtr greeting(IntPtr nameAddr)
    {
        if (GreetingPtr.Value != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(GreetingPtr.Value);
            GreetingPtr.Value = IntPtr.Zero;
        }
        var name = UTF8AddrToString(nameAddr);
        Console.WriteLine($"name={name}");
        var msg = $"こんにちは: {name}";
        GreetingPtr.Value = StringToUTF8Addr(msg);
        return GreetingPtr.Value;
    }
    public static IntPtr StringToUTF8Addr(string s)
    {
        int len = Encoding.UTF8.GetByteCount(s);
        byte[] buffer = new byte[len + 1];
        Encoding.UTF8.GetBytes(s, 0, s.Length, buffer, 0);
        IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
        return nativeUtf8;
    }
    public static string UTF8AddrToString(IntPtr s)
    {
        int len = 0;
        while (Marshal.ReadByte(s, len) != 0) ++len;
        byte[] buffer = new byte[len];
        Marshal.Copy(s, buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer);
    }
}
