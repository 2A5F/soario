using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soario.Native
{
    public unsafe partial struct FObject
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            ((delegate* unmanaged[Thiscall]<FObject*, void>)(lpVtbl[0]))((FObject*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, nuint>)(lpVtbl[1]))((FObject*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, nuint>)(lpVtbl[2]))((FObject*)Unsafe.AsPointer(ref this));
        }
    }

    public unsafe partial struct FmStr16
    {
        [NativeTypeName("uint16_t *")]
        public ushort* ptr;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public unsafe partial struct FmStr8
    {
        [NativeTypeName("uint8_t *")]
        public byte* ptr;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public unsafe partial struct FrStr16
    {
        [NativeTypeName("const uint16_t *")]
        public ushort* ptr;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public unsafe partial struct FrStr8
    {
        [NativeTypeName("const uint8_t *")]
        public byte* ptr;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public partial struct FInt2
    {
        [NativeTypeName("int32_t")]
        public int X;

        [NativeTypeName("int32_t")]
        public int Y;
    }

    public partial struct FFloat2
    {
        [NativeTypeName("int32_t")]
        public int X;

        [NativeTypeName("int32_t")]
        public int Y;
    }

    public partial struct FFloat3
    {
        public float X;

        public float Y;

        public float Z;

        private float _pad;
    }

    public partial struct FFloat4
    {
        public float X;

        public float Y;

        public float Z;

        public float W;
    }

    public enum FErrorType
    {
        None,
        Common,
        Sdl,
        HResult,
    }

    public enum FErrorMsgType
    {
        Utf8c,
        Utf16c,
        Utf8s,
        Utf16s,
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct FErrorMsg
    {
        [FieldOffset(0)]
        [NativeTypeName("const char *")]
        public sbyte* u8c;

        [FieldOffset(0)]
        [NativeTypeName("const wchar_t *")]
        public ushort* u16c;

        [FieldOffset(0)]
        [NativeTypeName("ccc::FrStr8")]
        public FrStr8 u8s;

        [FieldOffset(0)]
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 u16s;
    }

    public partial struct FError
    {
        [NativeTypeName("ccc::FErrorType")]
        public FErrorType type;

        [NativeTypeName("ccc::FErrorMsgType")]
        public FErrorMsgType msg_type;

        [NativeTypeName("ccc::FErrorMsg")]
        public FErrorMsg msg;

        [NativeTypeName("uint64_t")]
        public ulong data;
    }

    public unsafe partial struct InitParams
    {
        [NativeTypeName("ccc::TimeData *")]
        public TimeData* p_time_data;

        [NativeTypeName("ccc::FGpu *")]
        public FGpu* p_gpu;
    }

    public partial struct InitResult
    {
        [NativeTypeName("ccc::AppFnVtb")]
        public AppFnVtb fn_vtb;
    }

    public unsafe partial struct AppFnVtb
    {
        [NativeTypeName("ccc::fn_func__FrStr16__size_t *")]
        public delegate* unmanaged[Cdecl]<FrStr16, nuint> utf16_get_utf8_max_len;

        [NativeTypeName("ccc::fn_func__FrStr16_FmStr8__size_t *")]
        public delegate* unmanaged[Cdecl]<FrStr16, FmStr8, nuint> utf16_to_utf8;

        [NativeTypeName("ccc::fn_action *")]
        public delegate* unmanaged[Cdecl]<void> start;

        [NativeTypeName("ccc::fn_action__voidp_FWindowEventType_voidp *")]
        public delegate* unmanaged[Cdecl]<void*, FWindowEventType, void*, void> window_event_handle;
    }

    public partial struct TimeData
    {
        [NativeTypeName("int64_t")]
        public long start_time;

        [NativeTypeName("int64_t")]
        public long last_time;

        [NativeTypeName("int64_t")]
        public long now_time;

        [NativeTypeName("int64_t")]
        public long delta_time_raw;

        [NativeTypeName("int64_t")]
        public long total_time_raw;

        public double delta_time;

        public double total_time;
    }

    public partial struct WindowCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 title;

        [NativeTypeName("ccc::FInt2")]
        public FInt2 size;

        [NativeTypeName("ccc::FInt2")]
        public FInt2 min_size;

        [NativeTypeName("ccc::FBool")]
        public int has_min_size;
    }

    public enum FWindowEventType
    {
        None = 0,
        Close = 1,
        Resize = 2,
    }

    [NativeTypeName("struct FWindow : ccc::FObject")]
    public unsafe partial struct FWindow
    {
        public void** lpVtbl;

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?create@FWindow@ccc@@SAPEAU12@AEAUFError@2@AEBUWindowCreateOptions@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FWindow *")]
        public static extern FWindow* create([NativeTypeName("ccc::FError &")] FError* err, [NativeTypeName("const WindowCreateOptions &")] WindowCreateOptions* options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, nuint>)(lpVtbl[1]))((FWindow*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, nuint>)(lpVtbl[2]))((FWindow*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_gc_handle(void* handle)
        {
            ((delegate* unmanaged[Thiscall]<FWindow*, void*, void>)(lpVtbl[3]))((FWindow*)Unsafe.AsPointer(ref this), handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void get_size([NativeTypeName("ccc::FError *")] FError* err, [NativeTypeName("ccc::FInt2 *")] FInt2* size)
        {
            ((delegate* unmanaged[Thiscall]<FWindow*, FError*, FInt2*, void>)(lpVtbl[4]))((FWindow*)Unsafe.AsPointer(ref this), err, size);
        }
    }

    [NativeTypeName("struct FGpu : ccc::FObject")]
    public unsafe partial struct FGpu
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, nuint>)(lpVtbl[1]))((FGpu*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, nuint>)(lpVtbl[2]))((FGpu*)Unsafe.AsPointer(ref this));
        }
    }

    public static unsafe partial class FFI
    {
        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?exit@ccc@@YAXH@Z", ExactSpelling = true)]
        public static extern void exit(int code);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?alloc@ccc@@YAPEAX_K@Z", ExactSpelling = true)]
        public static extern void* alloc([NativeTypeName("size_t")] nuint size);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?free@ccc@@YAXPEAX@Z", ExactSpelling = true)]
        public static extern void free(void* ptr);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?alloc_str@ccc@@YA?AUFmStr8@1@_K@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FmStr8")]
        public static extern FmStr8 alloc_str([NativeTypeName("size_t")] nuint size);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?free_str@ccc@@YAXUFmStr8@1@@Z", ExactSpelling = true)]
        public static extern void free_str([NativeTypeName("ccc::FmStr8")] FmStr8 str);
    }
}
