namespace Soario.Native;

public unsafe class NativeWaitHandle : WaitHandle
{
    public NativeWaitHandle(void* handle, bool ownsHandle = false)
    {
        SafeWaitHandle = new((IntPtr)handle, ownsHandle);
    }
}
