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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, nuint>)(lpVtbl[3]))((FObject*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, nuint>)(lpVtbl[4]))((FObject*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, byte>)(lpVtbl[5]))((FObject*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FObject*, byte>)(lpVtbl[6]))((FObject*)Unsafe.AsPointer(ref this)) != 0;
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

    public partial struct FInt3
    {
        [NativeTypeName("int32_t")]
        public int X;

        [NativeTypeName("int32_t")]
        public int Y;

        [NativeTypeName("int32_t")]
        public int Z;

        [NativeTypeName("int32_t")]
        private int _pad;
    }

    public partial struct FInt4
    {
        [NativeTypeName("int32_t")]
        public int X;

        [NativeTypeName("int32_t")]
        public int Y;

        [NativeTypeName("int32_t")]
        public int Z;

        [NativeTypeName("int32_t")]
        public int W;
    }

    public partial struct FUInt2
    {
        [NativeTypeName("uint32_t")]
        public uint X;

        [NativeTypeName("uint32_t")]
        public uint Y;
    }

    public partial struct FUInt3
    {
        [NativeTypeName("uint32_t")]
        public uint X;

        [NativeTypeName("uint32_t")]
        public uint Y;

        [NativeTypeName("uint32_t")]
        public uint Z;

        [NativeTypeName("uint32_t")]
        private uint _pad;
    }

    public partial struct FUInt4
    {
        [NativeTypeName("uint32_t")]
        public uint X;

        [NativeTypeName("uint32_t")]
        public uint Y;

        [NativeTypeName("uint32_t")]
        public uint Z;

        [NativeTypeName("uint32_t")]
        public uint W;
    }

    public partial struct FFloat2
    {
        public float X;

        public float Y;
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

    public partial struct FFloatRect
    {
        public float X;

        public float Y;

        public float W;

        public float H;
    }

    public enum FErrorType
    {
        None,
        Common,
        Sdl,
        HResult,
        Gpu,
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

    [NativeTypeName("struct FString8 : ccc::FObject")]
    public unsafe partial struct FString8
    {
        public void** lpVtbl;

        [NativeTypeName("const uint8_t *")]
        public byte* m_ptr;

        [NativeTypeName("const size_t")]
        public nuint m_len;

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Create@FString8@ccc@@SAPEAU12@UFrStr8@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FString8 *")]
        public static extern FString8* Create([NativeTypeName("ccc::FrStr8")] FrStr8 slice);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, nuint>)(lpVtbl[1]))((FString8*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, nuint>)(lpVtbl[2]))((FString8*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, nuint>)(lpVtbl[3]))((FString8*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, nuint>)(lpVtbl[4]))((FString8*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, byte>)(lpVtbl[5]))((FString8*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FString8*, byte>)(lpVtbl[6]))((FString8*)Unsafe.AsPointer(ref this)) != 0;
        }
    }

    public partial struct FString8
    {
    }

    [NativeTypeName("struct FString16 : ccc::FObject")]
    public unsafe partial struct FString16
    {
        public void** lpVtbl;

        [NativeTypeName("const uint16_t *")]
        public ushort* m_ptr;

        [NativeTypeName("const size_t")]
        public nuint m_len;

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Create@FString16@ccc@@SAPEAU12@UFrStr8@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FString16 *")]
        public static extern FString16* Create([NativeTypeName("ccc::FrStr8")] FrStr8 slice);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, nuint>)(lpVtbl[1]))((FString16*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, nuint>)(lpVtbl[2]))((FString16*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, nuint>)(lpVtbl[3]))((FString16*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, nuint>)(lpVtbl[4]))((FString16*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, byte>)(lpVtbl[5]))((FString16*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FString16*, byte>)(lpVtbl[6]))((FString16*)Unsafe.AsPointer(ref this)) != 0;
        }
    }

    public enum FLogLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        Trace,
    }

    public unsafe partial struct InitParams
    {
        [NativeTypeName("ccc::AppVars *")]
        public AppVars* p_vas;
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

        [NativeTypeName("ccc::fn_func__FrStr16__FString8p *")]
        public delegate* unmanaged[Cdecl]<FrStr16, FString8*> utf16_to_string8;

        [NativeTypeName("ccc::fn_action *")]
        public delegate* unmanaged[Cdecl]<void> start;

        [NativeTypeName("ccc::fn_action *")]
        public delegate* unmanaged[Cdecl]<void> exit;

        [NativeTypeName("ccc::fn_action__voidp_FWindowEventType_voidp *")]
        public delegate* unmanaged[Cdecl]<void*, FWindowEventType, void*, void> window_event_handle;

        [NativeTypeName("ccc::fn_func__FLogLevel_charp__void *")]
        public delegate* unmanaged[Cdecl]<FLogLevel, sbyte*, void> logger_cstr;

        [NativeTypeName("ccc::fn_func__FLogLevel_wcharp__void *")]
        public delegate* unmanaged[Cdecl]<FLogLevel, ushort*, void> logger_wstr;

        [NativeTypeName("ccc::fn_func__FLogLevel_FrStr8__void *")]
        public delegate* unmanaged[Cdecl]<FLogLevel, FrStr8, void> logger_str8;

        [NativeTypeName("ccc::fn_func__FLogLevel_FrStr16__void *")]
        public delegate* unmanaged[Cdecl]<FLogLevel, FrStr16, void> logger_str16;
    }

    public partial struct AppVars
    {
        [NativeTypeName("bool")]
        public byte debug;
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

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?Create@FWindow@ccc@@SAPEAU12@AEAUFError@2@AEBUWindowCreateOptions@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FWindow *")]
        public static extern FWindow* Create([NativeTypeName("ccc::FError &")] FError* err, [NativeTypeName("const WindowCreateOptions &")] WindowCreateOptions* options);

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
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, nuint>)(lpVtbl[3]))((FWindow*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, nuint>)(lpVtbl[4]))((FWindow*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, byte>)(lpVtbl[5]))((FWindow*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, byte>)(lpVtbl[6]))((FWindow*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint get_hwnd()
        {
            return ((delegate* unmanaged[Thiscall]<FWindow*, nuint>)(lpVtbl[7]))((FWindow*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_gc_handle(void* handle)
        {
            ((delegate* unmanaged[Thiscall]<FWindow*, void*, void>)(lpVtbl[8]))((FWindow*)Unsafe.AsPointer(ref this), handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void get_size([NativeTypeName("ccc::FError *")] FError* err, [NativeTypeName("ccc::FInt2 *")] FInt2* size)
        {
            ((delegate* unmanaged[Thiscall]<FWindow*, FError*, FInt2*, void>)(lpVtbl[9]))((FWindow*)Unsafe.AsPointer(ref this), err, size);
        }
    }

    public partial struct FGpuConsts
    {
        [NativeTypeName("const uint32_t")]
        public const uint FrameCount = 3;
    }

    public partial struct FGpuSurface
    {
    }

    [NativeTypeName("struct FGpuSurface : ccc::FGpuRt")]
    public unsafe partial struct FGpuSurface
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, nuint>)(lpVtbl[1]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, nuint>)(lpVtbl[2]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, nuint>)(lpVtbl[3]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, nuint>)(lpVtbl[4]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[5]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[6]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_res_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, void*>)(lpVtbl[7]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FInt2")]
        public FInt2 get_size()
        {
            FInt2 result;
            return *((delegate* unmanaged[Thiscall]<FGpuSurface*, FInt2*, FInt2*>)(lpVtbl[8]))((FGpuSurface*)Unsafe.AsPointer(ref this), &result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool has_rtv()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[9]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool has_dsv()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[10]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint get_cpu_rtv_handle([NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, FError*, nuint>)(lpVtbl[11]))((FGpuSurface*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint get_cpu_dsv_handle([NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, FError*, nuint>)(lpVtbl[12]))((FGpuSurface*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("int32_t")]
        public int frame_count()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, int>)(lpVtbl[13]))((FGpuSurface*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ready_frame([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FError*, void>)(lpVtbl[14]))((FGpuSurface*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void submit([NativeTypeName("const FGpuCmdList *")] FGpuCmdList* cmd_list, [NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FGpuCmdList*, FError*, void>)(lpVtbl[15]))((FGpuSurface*)Unsafe.AsPointer(ref this), cmd_list, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void present_frame([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FError*, void>)(lpVtbl[16]))((FGpuSurface*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void resize([NativeTypeName("ccc::FInt2")] FInt2 new_size)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FInt2, void>)(lpVtbl[17]))((FGpuSurface*)Unsafe.AsPointer(ref this), new_size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool get_v_sync()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[18]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_v_sync([NativeTypeName("bool")] byte v)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte, void>)(lpVtbl[19]))((FGpuSurface*)Unsafe.AsPointer(ref this), v);
        }
    }

    public partial struct FGpuSurface
    {
    }

    public partial struct FGpuDeviceCreateOptions
    {
    }

    public unsafe partial struct FGpuDeviceCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("ccc::fn_func__voidp_FLogLevel_charp__void *")]
        public delegate* unmanaged[Cdecl]<void*, FLogLevel, sbyte*, void> logger;

        public void* logger_object;

        [NativeTypeName("ccc::fn_func__voidp__void *")]
        public delegate* unmanaged[Cdecl]<void*, void> logger_drop_object;
    }

    public partial struct FGpuDevice
    {
    }

    [NativeTypeName("struct FGpuDevice : ccc::FObject")]
    public unsafe partial struct FGpuDevice
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, nuint>)(lpVtbl[1]))((FGpuDevice*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, nuint>)(lpVtbl[2]))((FGpuDevice*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, nuint>)(lpVtbl[3]))((FGpuDevice*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, nuint>)(lpVtbl[4]))((FGpuDevice*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, byte>)(lpVtbl[5]))((FGpuDevice*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, byte>)(lpVtbl[6]))((FGpuDevice*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuSurface *")]
        public FGpuSurface* CreateSurfaceFromHwnd([NativeTypeName("ccc::FGpuQueue *")] FGpuQueue* queue, [NativeTypeName("const FGpuSurfaceCreateOptions &")] FGpuSurfaceCreateOptions* options, [NativeTypeName("size_t")] nuint hwnd, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuQueue*, FGpuSurfaceCreateOptions*, nuint, FError*, FGpuSurface*>)(lpVtbl[7]))((FGpuDevice*)Unsafe.AsPointer(ref this), queue, options, hwnd, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuSurface *")]
        public FGpuSurface* CreateSurfaceFromWindow([NativeTypeName("ccc::FGpuQueue *")] FGpuQueue* queue, [NativeTypeName("const FGpuSurfaceCreateOptions &")] FGpuSurfaceCreateOptions* options, [NativeTypeName("ccc::FWindow *")] FWindow* window, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuQueue*, FGpuSurfaceCreateOptions*, FWindow*, FError*, FGpuSurface*>)(lpVtbl[8]))((FGpuDevice*)Unsafe.AsPointer(ref this), queue, options, window, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuQueue *")]
        public FGpuQueue* CreateQueue([NativeTypeName("const FGpuQueueCreateOptions &")] FGpuQueueCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuQueueCreateOptions*, FError*, FGpuQueue*>)(lpVtbl[9]))((FGpuDevice*)Unsafe.AsPointer(ref this), options, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuPipelineLayout *")]
        public FGpuPipelineLayout* CreateBindLessPipelineLayout([NativeTypeName("const FGpuBindLessPipelineLayoutCreateOptions &")] FGpuBindLessPipelineLayoutCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuBindLessPipelineLayoutCreateOptions*, FError*, FGpuPipelineLayout*>)(lpVtbl[10]))((FGpuDevice*)Unsafe.AsPointer(ref this), options, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuPipelineState *")]
        public FGpuPipelineState* CreatePipelineState([NativeTypeName("ccc::FGpuPipelineLayout *")] FGpuPipelineLayout* layout, [NativeTypeName("const FGpuPipelineStateCreateOptions &")] FGpuPipelineStateCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuPipelineLayout*, FGpuPipelineStateCreateOptions*, FError*, FGpuPipelineState*>)(lpVtbl[11]))((FGpuDevice*)Unsafe.AsPointer(ref this), layout, options, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuResource *")]
        public FGpuResource* CreateBuffer([NativeTypeName("const FGpuResourceBufferCreateOptions &")] FGpuResourceBufferCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuDevice*, FGpuResourceBufferCreateOptions*, FError*, FGpuResource*>)(lpVtbl[12]))((FGpuDevice*)Unsafe.AsPointer(ref this), options, err);
        }
    }

    [NativeTypeName("struct FGpu : ccc::FObject")]
    public unsafe partial struct FGpu
    {
        public void** lpVtbl;

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGpu@FGpu@ccc@@SAPEAU12@AEAUFError@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FGpu *")]
        public static extern FGpu* CreateGpu([NativeTypeName("ccc::FError &")] FError* err);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, nuint>)(lpVtbl[3]))((FGpu*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, nuint>)(lpVtbl[4]))((FGpu*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, byte>)(lpVtbl[5]))((FGpu*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, byte>)(lpVtbl[6]))((FGpu*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuDevice *")]
        public FGpuDevice* CreateDevice([NativeTypeName("const FGpuDeviceCreateOptions &")] FGpuDeviceCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpu*, FGpuDeviceCreateOptions*, FError*, FGpuDevice*>)(lpVtbl[7]))((FGpu*)Unsafe.AsPointer(ref this), options, err);
        }
    }

    public enum FGpuResState
    {
        Common,
        RenderTarget,
    }

    [NativeTypeName("struct FGpuRes : ccc::FObject")]
    public unsafe partial struct FGpuRes
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, nuint>)(lpVtbl[1]))((FGpuRes*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, nuint>)(lpVtbl[2]))((FGpuRes*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, nuint>)(lpVtbl[3]))((FGpuRes*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, nuint>)(lpVtbl[4]))((FGpuRes*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, byte>)(lpVtbl[5]))((FGpuRes*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, byte>)(lpVtbl[6]))((FGpuRes*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_res_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRes*, void*>)(lpVtbl[7]))((FGpuRes*)Unsafe.AsPointer(ref this));
        }
    }

    public enum FGpuResourceDimension
    {
        Unknown,
        Buffer,
        Texture1D,
        Texture2D,
        Texture3D,
    }

    public enum FGpuResourceLayout
    {
        Unknown,
        RowMajor,
        UndefinedSwizzle64KB,
        StandardSwizzle64KB,
    }

    public partial struct FGpuResourceFlags
    {
        public uint _bitfield;

        [NativeTypeName("uint32_t : 1")]
        public uint rtv
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return _bitfield & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~0x1u) | (value & 0x1u);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint dsv
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 1) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint uav
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 2) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint srv
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 3) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 3)) | ((value & 0x1u) << 3);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint cross_gpu
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 4) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 4)) | ((value & 0x1u) << 4);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint shared_access
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 5) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 5)) | ((value & 0x1u) << 5);
            }
        }

        [NativeTypeName("uint32_t : 1")]
        public uint ray_tracing_acceleration_structure
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (_bitfield >> 6) & 0x1u;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (_bitfield & ~(0x1u << 6)) | ((value & 0x1u) << 6);
            }
        }
    }

    public partial struct FGpuResourceBufferInfo
    {
        [NativeTypeName("ccc::FGpuResourceDimension")]
        public FGpuResourceDimension dimension;

        [NativeTypeName("int64_t")]
        public long align;

        [NativeTypeName("int64_t")]
        public long size;

        [NativeTypeName("ccc::FGpuResourceFlags")]
        public FGpuResourceFlags flags;
    }

    public partial struct FGpuResourceTextureInfo
    {
        [NativeTypeName("ccc::FGpuResourceDimension")]
        public FGpuResourceDimension dimension;

        [NativeTypeName("int64_t")]
        public long align;

        [NativeTypeName("int64_t")]
        public long width;

        [NativeTypeName("int32_t")]
        public int height;

        [NativeTypeName("int16_t")]
        public short depth_or_length;

        [NativeTypeName("int16_t")]
        public short mip_levels;

        [NativeTypeName("ccc::FGpuTextureFormat")]
        public FGpuTextureFormat format;

        [NativeTypeName("uint32_t")]
        public uint sample_count;

        [NativeTypeName("uint32_t")]
        public uint sample_quality;

        [NativeTypeName("ccc::FGpuResourceLayout")]
        public FGpuResourceLayout layout;

        [NativeTypeName("ccc::FGpuResourceFlags")]
        public FGpuResourceFlags flags;
    }

    [StructLayout(LayoutKind.Explicit)]
    public partial struct FGpuResourceInfo
    {
        [FieldOffset(0)]
        [NativeTypeName("ccc::FGpuResourceDimension")]
        public FGpuResourceDimension dimension;

        [FieldOffset(0)]
        [NativeTypeName("ccc::FGpuResourceBufferInfo")]
        public FGpuResourceBufferInfo buffer_info;

        [FieldOffset(0)]
        [NativeTypeName("ccc::FGpuResourceTextureInfo")]
        public FGpuResourceTextureInfo texture_info;
    }

    public enum FGpuResourceUsage
    {
        GpuOnly,
        CpuToGpu,
        GpuToCpu,
    }

    public partial struct FGpuResourceBufferCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("ccc::FGpuResourceUsage")]
        public FGpuResourceUsage usage;

        [NativeTypeName("int64_t")]
        public long size;

        [NativeTypeName("ccc::FGpuResourceFlags")]
        public FGpuResourceFlags flags;
    }

    public partial struct FGpuResourceTextureCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("ccc::FGpuResourceUsage")]
        public FGpuResourceUsage usage;

        [NativeTypeName("ccc::FGpuResourceDimension")]
        public FGpuResourceDimension dimension;

        [NativeTypeName("int64_t")]
        public long align;

        [NativeTypeName("int64_t")]
        public long width;

        [NativeTypeName("int32_t")]
        public int height;

        [NativeTypeName("int16_t")]
        public short depth_or_length;

        [NativeTypeName("int16_t")]
        public short mip_levels;

        [NativeTypeName("ccc::FGpuTextureFormat")]
        public FGpuTextureFormat format;

        [NativeTypeName("uint32_t")]
        public uint sample_count;

        [NativeTypeName("uint32_t")]
        public uint sample_quality;

        [NativeTypeName("ccc::FGpuResourceLayout")]
        public FGpuResourceLayout layout;

        [NativeTypeName("ccc::FGpuResourceFlags")]
        public FGpuResourceFlags flags;
    }

    [NativeTypeName("struct FGpuResource : ccc::FGpuRes")]
    public unsafe partial struct FGpuResource
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, nuint>)(lpVtbl[1]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, nuint>)(lpVtbl[2]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, nuint>)(lpVtbl[3]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, nuint>)(lpVtbl[4]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, byte>)(lpVtbl[5]))((FGpuResource*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, byte>)(lpVtbl[6]))((FGpuResource*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_res_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, void*>)(lpVtbl[7]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("const FGpuResourceInfo *")]
        public FGpuResourceInfo* get_info()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, FGpuResourceInfo*>)(lpVtbl[8]))((FGpuResource*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuView *")]
        public FGpuView* get_view([NativeTypeName("const FGpuViewCreateOptions &")] FGpuViewCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuResource*, FGpuViewCreateOptions*, FError*, FGpuView*>)(lpVtbl[9]))((FGpuResource*)Unsafe.AsPointer(ref this), options, err);
        }
    }

    public enum FGpuViewType
    {
        Unknown,
        Rtv,
        Dsv,
        Uav,
        Srv,
        Cbv,
    }

    public partial struct FGpuViewCreateOptions
    {
        [NativeTypeName("ccc::FGpuViewType")]
        public FGpuViewType type;
    }

    [NativeTypeName("struct FGpuView : ccc::FObject")]
    public unsafe partial struct FGpuView
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, nuint>)(lpVtbl[1]))((FGpuView*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, nuint>)(lpVtbl[2]))((FGpuView*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, nuint>)(lpVtbl[3]))((FGpuView*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, nuint>)(lpVtbl[4]))((FGpuView*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, byte>)(lpVtbl[5]))((FGpuView*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, byte>)(lpVtbl[6]))((FGpuView*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuViewType")]
        public FGpuViewType type()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuView*, FGpuViewType>)(lpVtbl[7]))((FGpuView*)Unsafe.AsPointer(ref this));
        }
    }

    [NativeTypeName("struct FGpuRt : ccc::FGpuRes")]
    public unsafe partial struct FGpuRt
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, nuint>)(lpVtbl[1]))((FGpuRt*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, nuint>)(lpVtbl[2]))((FGpuRt*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, nuint>)(lpVtbl[3]))((FGpuRt*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, nuint>)(lpVtbl[4]))((FGpuRt*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, byte>)(lpVtbl[5]))((FGpuRt*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, byte>)(lpVtbl[6]))((FGpuRt*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_res_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, void*>)(lpVtbl[7]))((FGpuRt*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FInt2")]
        public FInt2 get_size()
        {
            FInt2 result;
            return *((delegate* unmanaged[Thiscall]<FGpuRt*, FInt2*, FInt2*>)(lpVtbl[8]))((FGpuRt*)Unsafe.AsPointer(ref this), &result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool has_rtv()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, byte>)(lpVtbl[9]))((FGpuRt*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool has_dsv()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, byte>)(lpVtbl[10]))((FGpuRt*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint get_cpu_rtv_handle([NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, FError*, nuint>)(lpVtbl[11]))((FGpuRt*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint get_cpu_dsv_handle([NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuRt*, FError*, nuint>)(lpVtbl[12]))((FGpuRt*)Unsafe.AsPointer(ref this), err);
        }
    }

    public partial struct FGpuSurfaceCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("bool")]
        public byte v_sync;
    }

    public partial struct FGpuSurfaceCreateOptions
    {
    }

    public partial struct FGpuQueue
    {
    }

    [NativeTypeName("struct FGpuQueue : ccc::FObject")]
    public unsafe partial struct FGpuQueue
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, nuint>)(lpVtbl[1]))((FGpuQueue*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, nuint>)(lpVtbl[2]))((FGpuQueue*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, nuint>)(lpVtbl[3]))((FGpuQueue*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, nuint>)(lpVtbl[4]))((FGpuQueue*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, byte>)(lpVtbl[5]))((FGpuQueue*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, byte>)(lpVtbl[6]))((FGpuQueue*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuTask *")]
        public FGpuTask* CreateTask([NativeTypeName("const FGpuTaskCreateOptions &")] FGpuTaskCreateOptions* options, [NativeTypeName("ccc::FError &")] FError* err)
        {
            return ((delegate* unmanaged[Thiscall]<FGpuQueue*, FGpuTaskCreateOptions*, FError*, FGpuTask*>)(lpVtbl[7]))((FGpuQueue*)Unsafe.AsPointer(ref this), options, err);
        }
    }

    public partial struct FGpuQueueCreateOptions
    {
    }

    public partial struct FGpuQueueCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("ccc::FGpuQueueType")]
        public FGpuQueueType type;
    }

    public enum FGpuCmdType
    {
        BarrierTransition = 1,
        ClearRt,
        SetRt,
        ReadyRasterizer,
        DispatchMesh,
    }

    public partial struct FGpuCmdClearRtFlag
    {
        public byte _bitfield;

        [NativeTypeName("uint8_t : 1")]
        public byte color
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)(_bitfield & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~0x1u) | (value & 0x1u));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte depth
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 1) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte stencil
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 2) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2));
            }
        }
    }

    public unsafe partial struct FGpuCmdClearRt
    {
        [NativeTypeName("ccc::FGpuCmdType")]
        public FGpuCmdType type;

        [NativeTypeName("int32_t")]
        public int rect_len;

        [NativeTypeName("ccc::FGpuRt *")]
        public FGpuRt* rtv;

        [NativeTypeName("ccc::FGpuRt *")]
        public FGpuRt* dsv;

        [NativeTypeName("ccc::FFloat4")]
        public FFloat4 color;

        public float depth;

        [NativeTypeName("uint8_t")]
        public byte stencil;

        [NativeTypeName("ccc::FGpuCmdClearRtFlag")]
        public FGpuCmdClearRtFlag flag;
    }

    public unsafe partial struct FGpuCmdBarrierTransition
    {
        [NativeTypeName("ccc::FGpuCmdType")]
        public FGpuCmdType type;

        [NativeTypeName("uint32_t")]
        public uint sub_res;

        [NativeTypeName("ccc::FGpuRes *")]
        public FGpuRes* res;

        [NativeTypeName("ccc::FGpuResState")]
        public FGpuResState pre_state;

        [NativeTypeName("ccc::FGpuResState")]
        public FGpuResState cur_state;
    }

    public unsafe partial struct FGpuCmdSetRt
    {
        [NativeTypeName("ccc::FGpuCmdType")]
        public FGpuCmdType type;

        [NativeTypeName("ccc::FGpuRt *")]
        public FGpuRt* depth;

        [NativeTypeName("int32_t")]
        public int len;
    }

    public partial struct FGpuCmdRasterizerViewPort
    {
        [NativeTypeName("ccc::FFloat4")]
        public FFloat4 rect;

        [NativeTypeName("ccc::FFloat2")]
        public FFloat2 depth_range;
    }

    public partial struct FGpuCmdRasterizerInfo
    {
        [NativeTypeName("ccc::FInt4")]
        public FInt4 scissor_rect;

        [NativeTypeName("ccc::FGpuCmdRasterizerViewPort")]
        public FGpuCmdRasterizerViewPort view_port;
    }

    public partial struct FGpuCmdReadyRasterizer
    {
        [NativeTypeName("ccc::FGpuCmdType")]
        public FGpuCmdType type;

        [NativeTypeName("int32_t")]
        public int len;
    }

    public unsafe partial struct FGpuCmdDispatchMesh
    {
        [NativeTypeName("ccc::FGpuCmdType")]
        public FGpuCmdType type;

        [NativeTypeName("ccc::FUInt3")]
        public FUInt3 thread_groups;

        [NativeTypeName("ccc::FGpuPipelineState *")]
        public FGpuPipelineState* pipeline;
    }

    public unsafe partial struct FGpuCmdList
    {
        [NativeTypeName("uint8_t *")]
        public byte* datas;

        [NativeTypeName("int32_t *")]
        public int* indexes;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public partial struct FGpuTaskCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;
    }

    [NativeTypeName("struct FGpuTask : ccc::FObject")]
    public unsafe partial struct FGpuTask
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, nuint>)(lpVtbl[1]))((FGpuTask*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, nuint>)(lpVtbl[2]))((FGpuTask*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, nuint>)(lpVtbl[3]))((FGpuTask*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, nuint>)(lpVtbl[4]))((FGpuTask*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, byte>)(lpVtbl[5]))((FGpuTask*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuTask*, byte>)(lpVtbl[6]))((FGpuTask*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void start([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuTask*, FError*, void>)(lpVtbl[7]))((FGpuTask*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void submit([NativeTypeName("const FGpuCmdList *")] FGpuCmdList* cmd_list, [NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuTask*, FGpuCmdList*, FError*, void>)(lpVtbl[8]))((FGpuTask*)Unsafe.AsPointer(ref this), cmd_list, err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void end([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuTask*, FError*, void>)(lpVtbl[9]))((FGpuTask*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void wait_reset([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuTask*, FError*, void>)(lpVtbl[10]))((FGpuTask*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void wait_reset_async([NativeTypeName("ccc::FError &")] FError* err, void* obj, [NativeTypeName("ccc::fn_action__voidp")] delegate* unmanaged[Cdecl]<void*, void> cb)
        {
            ((delegate* unmanaged[Thiscall]<FGpuTask*, FError*, void*, delegate* unmanaged[Cdecl]<void*, void>, void>)(lpVtbl[11]))((FGpuTask*)Unsafe.AsPointer(ref this), err, obj, cb);
        }
    }

    public enum FGpuQueueType
    {
        Common,
        Compute,
        Copy,
    }

    public enum FGpuTextureFormat
    {
        Unknown = 0,
        R32G32B32A32_TypeLess = 1,
        R32G32B32A32_Float = 2,
        R32G32B32A32_UInt = 3,
        R32G32B32A32_SInt = 4,
        R32G32B32_TypeLess = 5,
        R32G32B32_Float = 6,
        R32G32B32_UInt = 7,
        R32G32B32_SInt = 8,
        R16G16B16A16_TypeLess = 9,
        R16G16B16A16_Float = 10,
        R16G16B16A16_UNorm = 11,
        R16G16B16A16_UInt = 12,
        R16G16B16A16_SNorm = 13,
        R16G16B16A16_SInt = 14,
        R32G32_TypeLess = 15,
        R32G32_Float = 16,
        R32G32_UInt = 17,
        R32G32_SInt = 18,
        R32G8X24_TypeLess = 19,
        D32_Float_S8X24_UInt = 20,
        R32_Float_X8X24_TypeLess = 21,
        X32_TypeLess_G8X24_Float = 22,
        R10G10B10A2_TypeLess = 23,
        R10G10B10A2_UNorm = 24,
        R10G10B10A2_UInt = 25,
        R11G11B10_Float = 26,
        R8G8B8A8_TypeLess = 27,
        R8G8B8A8_UNorm = 28,
        R8G8B8A8_UNorm_sRGB = 29,
        R8G8B8A8_UInt = 30,
        R8G8B8A8_SNorm = 31,
        R8G8B8A8_SInt = 32,
        R16G16_TypeLess = 33,
        R16G16_Float = 34,
        R16G16_UNorm = 35,
        R16G16_UInt = 36,
        R16G16_SNorm = 37,
        R16G16_SInt = 38,
        R32_TypeLess = 39,
        D32_Float = 40,
        R32_Float = 41,
        R32_UInt = 42,
        R32_SInt = 43,
        R24G8_TypeLess = 44,
        D24_UNorm_S8_UInt = 45,
        R24_UNorm_X8_TypeLess = 46,
        X24_TypeLess_G8_UInt = 47,
        R8G8_TypeLess = 48,
        R8G8_UNorm = 49,
        R8G8_UInt = 50,
        R8G8_SNorm = 51,
        R8G8_SInt = 52,
        R16_TypeLess = 53,
        R16_Float = 54,
        D16_UNorm = 55,
        R16_UNorm = 56,
        R16_UInt = 57,
        R16_SNorm = 58,
        R16_SInt = 59,
        R8_TypeLess = 60,
        R8_UNorm = 61,
        R8_UInt = 62,
        R8_SNorm = 63,
        R8_SInt = 64,
        A8_UNorm = 65,
        R1_UNorm = 66,
        R9G9B9E5_SharedExp = 67,
        R8G8_B8G8_UNorm = 68,
        G8R8_G8B8_UNorm = 69,
        BC1_TypeLess = 70,
        BC1_UNorm = 71,
        BC1_UNorm_sRGB = 72,
        BC2_TypeLess = 73,
        BC2_UNorm = 74,
        BC2_UNorm_sRGB = 75,
        BC3_TypeLess = 76,
        BC3_UNorm = 77,
        BC3_UNorm_sRGB = 78,
        BC4_TypeLess = 79,
        BC4_UNorm = 80,
        BC4_SNorm = 81,
        BC5_TypeLess = 82,
        BC5_UNorm = 83,
        BC5_SNorm = 84,
        B5G6R5_UNorm = 85,
        B5G5R5A1_UNorm = 86,
        B8G8R8A8_UNorm = 87,
        B8G8R8X8_UNorm = 88,
        R10G10B10_XR_Bias_A2_UNorm = 89,
        B8G8R8A8_TypeLess = 90,
        B8G8R8A8_UNorm_sRGB = 91,
        B8G8R8X8_TypeLess = 92,
        B8G8R8X8_UNorm_sRGB = 93,
        BC6H_TypeLess = 94,
        BC6H_UF16 = 95,
        BC6H_SF16 = 96,
        BC7_TypeLess = 97,
        BC7_UNorm = 98,
        BC7_UNorm_sRGB = 99,
    }

    public partial struct FGpuBindLessPipelineLayoutCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;
    }

    [NativeTypeName("struct FGpuPipelineLayout : ccc::FObject")]
    public unsafe partial struct FGpuPipelineLayout
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, nuint>)(lpVtbl[1]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, nuint>)(lpVtbl[2]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, nuint>)(lpVtbl[3]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, nuint>)(lpVtbl[4]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, byte>)(lpVtbl[5]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, byte>)(lpVtbl[6]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineLayout*, void*>)(lpVtbl[7]))((FGpuPipelineLayout*)Unsafe.AsPointer(ref this));
        }
    }

    public partial struct FGpuPipelineCreateFlag
    {
        public ushort _bitfield;

        [NativeTypeName("uint16_t : 1")]
        public ushort bind_less
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)(_bitfield & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~0x1u) | (value & 0x1u));
            }
        }

        [NativeTypeName("uint16_t : 1")]
        public ushort cs
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)((_bitfield >> 1) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }

        [NativeTypeName("uint16_t : 1")]
        public ushort ps
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)((_bitfield >> 2) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2));
            }
        }

        [NativeTypeName("uint16_t : 1")]
        public ushort vs
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)((_bitfield >> 3) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~(0x1u << 3)) | ((value & 0x1u) << 3));
            }
        }

        [NativeTypeName("uint16_t : 1")]
        public ushort ms
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)((_bitfield >> 4) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~(0x1u << 4)) | ((value & 0x1u) << 4));
            }
        }

        [NativeTypeName("uint16_t : 1")]
        public ushort ts
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (ushort)((_bitfield >> 5) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (ushort)((_bitfield & ~(0x1u << 5)) | ((value & 0x1u) << 5));
            }
        }
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelinePrimitiveTopologyType : byte
    {
        Undefined,
        Point,
        Line,
        Triangle,
        Patch,
    }

    public partial struct FGpuPipelineColorMask
    {
        public byte _bitfield;

        [NativeTypeName("uint8_t : 1")]
        public byte r
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)(_bitfield & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~0x1u) | (value & 0x1u));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte g
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 1) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 1)) | ((value & 0x1u) << 1));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte b
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 2) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 2)) | ((value & 0x1u) << 2));
            }
        }

        [NativeTypeName("uint8_t : 1")]
        public byte a
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return (byte)((_bitfield >> 3) & 0x1u);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _bitfield = (byte)((_bitfield & ~(0x1u << 3)) | ((value & 0x1u) << 3));
            }
        }
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineSwitch : byte
    {
        Off = 0,
        On = 1,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineFillMode : byte
    {
        WireFrame = 2,
        Solid = 3,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineCullMode : byte
    {
        Off = 1,
        Front = 2,
        Back = 3,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineBlendType : byte
    {
        Zero = 1,
        One = 2,
        SrcColor = 3,
        InvSrcColor = 4,
        SrcAlpha = 5,
        InvSrcAlpha = 6,
        DstAlpha = 7,
        InvDstAlpha = 8,
        DstColor = 9,
        InvDstColor = 10,
        SrcAlphaSat = 11,
        BlendFactor = 14,
        BlendInvBlendFactor = 15,
        Src1Color = 16,
        InvSrc1Color = 17,
        Src1Alpha = 18,
        InvSrc1Alpha = 19,
        AlphaFactor = 20,
        InvAlphaFactor = 21,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineBlendOp : byte
    {
        None = 0,
        Add = 1,
        Sub = 2,
        RevSub = 3,
        Min = 4,
        Max = 5,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineLogicOp : byte
    {
        None = 0,
        Clear,
        One,
        Copy,
        CopyInv,
        Noop,
        Inv,
        And,
        NAnd,
        Or,
        Nor,
        Xor,
        Equiv,
        AndRev,
        AndInv,
        OrRev,
        OrInv,
    }

    public partial struct FGpuPipelineRtBlendState
    {
        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch blend;

        [NativeTypeName("ccc::FGpuPipelineBlendType")]
        public FGpuPipelineBlendType src_blend;

        [NativeTypeName("ccc::FGpuPipelineBlendType")]
        public FGpuPipelineBlendType dst_blend;

        [NativeTypeName("ccc::FGpuPipelineBlendOp")]
        public FGpuPipelineBlendOp blend_op;

        [NativeTypeName("ccc::FGpuPipelineBlendType")]
        public FGpuPipelineBlendType src_alpha_blend;

        [NativeTypeName("ccc::FGpuPipelineBlendType")]
        public FGpuPipelineBlendType dst_alpha_blend;

        [NativeTypeName("ccc::FGpuPipelineBlendOp")]
        public FGpuPipelineBlendOp alpha_blend_op;

        [NativeTypeName("ccc::FGpuPipelineLogicOp")]
        public FGpuPipelineLogicOp logic_op;

        [NativeTypeName("ccc::FGpuPipelineColorMask")]
        public FGpuPipelineColorMask write_mask;
    }

    public partial struct FGpuPipelineBlendState
    {
        [NativeTypeName("FGpuPipelineRtBlendState[8]")]
        public _rts_e__FixedBuffer rts;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch alpha_to_coverage;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch independent_blend;

        [InlineArray(8)]
        public partial struct _rts_e__FixedBuffer
        {
            public FGpuPipelineRtBlendState e0;
        }
    }

    public partial struct FGpuPipelineRasterizerState
    {
        [NativeTypeName("ccc::FGpuPipelineFillMode")]
        public FGpuPipelineFillMode fill_mode;

        [NativeTypeName("ccc::FGpuPipelineCullMode")]
        public FGpuPipelineCullMode cull_mode;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch depth_clip;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch multisample;

        [NativeTypeName("uint32_t")]
        public uint forced_sample_count;

        [NativeTypeName("int32_t")]
        public int depth_bias;

        public float depth_bias_clamp;

        public float slope_scaled_depth_bias;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch aa_line;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch conservative;
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineDepthWriteMask : byte
    {
        Zero = 0,
        All = 1,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineCmpFunc : byte
    {
        Off = 0,
        Never = 1,
        Less = 2,
        Equal = 3,
        LessEqual = 4,
        Greater = 5,
        NotEqual = 6,
        GreaterEqual = 7,
        Always = 8,
    }

    [NativeTypeName("uint8_t")]
    public enum FGpuPipelineStencilFailOp : byte
    {
        Keep = 1,
        Zero = 2,
        Replace = 3,
        IncSat = 4,
        DecSat = 5,
        Invert = 6,
        Inc = 7,
        Dec = 8,
    }

    public partial struct FGpuPipelineStencilState
    {
        [NativeTypeName("ccc::FGpuPipelineStencilFailOp")]
        public FGpuPipelineStencilFailOp fail_op;

        [NativeTypeName("ccc::FGpuPipelineStencilFailOp")]
        public FGpuPipelineStencilFailOp depth_fail_op;

        [NativeTypeName("ccc::FGpuPipelineStencilFailOp")]
        public FGpuPipelineStencilFailOp pass_op;

        [NativeTypeName("ccc::FGpuPipelineCmpFunc")]
        public FGpuPipelineCmpFunc func;
    }

    public partial struct FGpuPipelineDepthStencilState
    {
        [NativeTypeName("ccc::FGpuPipelineCmpFunc")]
        public FGpuPipelineCmpFunc depth_func;

        [NativeTypeName("ccc::FGpuPipelineDepthWriteMask")]
        public FGpuPipelineDepthWriteMask depth_write_mask;

        [NativeTypeName("ccc::FGpuPipelineSwitch")]
        public FGpuPipelineSwitch stencil_enable;

        [NativeTypeName("uint8_t")]
        public byte stencil_read_mask;

        [NativeTypeName("uint8_t")]
        public byte stencil_write_mask;

        [NativeTypeName("ccc::FGpuPipelineStencilState")]
        public FGpuPipelineStencilState front_face;

        [NativeTypeName("ccc::FGpuPipelineStencilState")]
        public FGpuPipelineStencilState back_face;
    }

    public partial struct FGpuPipelineSampleState
    {
        [NativeTypeName("uint32_t")]
        public uint count;

        [NativeTypeName("int32_t")]
        public int quality;
    }

    public partial struct FGpuPipelineStateCreateOptions
    {
        [NativeTypeName("ccc::FrStr16")]
        public FrStr16 name;

        [NativeTypeName("ccc::FGpuPipelineCreateFlag")]
        public FGpuPipelineCreateFlag flag;

        [NativeTypeName("ccc::FGpuPipelineBlendState")]
        public FGpuPipelineBlendState blend_state;

        [NativeTypeName("ccc::FGpuPipelinePrimitiveTopologyType")]
        public FGpuPipelinePrimitiveTopologyType primitive_topology_type;

        [NativeTypeName("ccc::FGpuPipelineRasterizerState")]
        public FGpuPipelineRasterizerState rasterizer_state;

        [NativeTypeName("ccc::FGpuPipelineDepthStencilState")]
        public FGpuPipelineDepthStencilState depth_stencil_state;

        [NativeTypeName("uint32_t")]
        public uint sample_mask;

        [NativeTypeName("int32_t")]
        public int rt_count;

        [NativeTypeName("FrStr8[3]")]
        public _blob_e__FixedBuffer blob;

        [NativeTypeName("FGpuTextureFormat[8]")]
        public _rtv_formats_e__FixedBuffer rtv_formats;

        [NativeTypeName("ccc::FGpuTextureFormat")]
        public FGpuTextureFormat dsv_format;

        [NativeTypeName("ccc::FGpuPipelineSampleState")]
        public FGpuPipelineSampleState sample_state;

        [InlineArray(3)]
        public partial struct _blob_e__FixedBuffer
        {
            public FrStr8 e0;
        }

        [InlineArray(8)]
        public partial struct _rtv_formats_e__FixedBuffer
        {
            public FGpuTextureFormat e0;
        }
    }

    [NativeTypeName("struct FGpuPipelineState : ccc::FObject")]
    public unsafe partial struct FGpuPipelineState
    {
        public void** lpVtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRef()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, nuint>)(lpVtbl[1]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint Release()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, nuint>)(lpVtbl[2]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint AddRefWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, nuint>)(lpVtbl[3]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("size_t")]
        public nuint ReleaseWeak()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, nuint>)(lpVtbl[4]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDowngrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, byte>)(lpVtbl[5]))((FGpuPipelineState*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryUpgrade()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, byte>)(lpVtbl[6]))((FGpuPipelineState*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NativeTypeName("ccc::FGpuPipelineLayout *")]
        public FGpuPipelineLayout* get_layout_ref()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, FGpuPipelineLayout*>)(lpVtbl[7]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void* get_raw_ptr()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuPipelineState*, void*>)(lpVtbl[8]))((FGpuPipelineState*)Unsafe.AsPointer(ref this));
        }
    }

    public static unsafe partial class FFI
    {
        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?exit@ccc@@YAXH@Z", ExactSpelling = true)]
        public static extern void exit(int code);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?alloc@ccc@@YAPEAX_K@Z", ExactSpelling = true)]
        public static extern void* alloc([NativeTypeName("size_t")] nuint size);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?realloc@ccc@@YAPEAXPEAX_K@Z", ExactSpelling = true)]
        public static extern void* realloc(void* old, [NativeTypeName("size_t")] nuint size);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?free@ccc@@YAXPEAX@Z", ExactSpelling = true)]
        public static extern void free(void* ptr);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?alloc_str@ccc@@YA?AUFmStr8@1@_K@Z", ExactSpelling = true)]
        [return: NativeTypeName("ccc::FmStr8")]
        public static extern FmStr8 alloc_str([NativeTypeName("size_t")] nuint size);

        [DllImport("soario.exe", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?free_str@ccc@@YAXUFmStr8@1@@Z", ExactSpelling = true)]
        public static extern void free_str([NativeTypeName("ccc::FmStr8")] FmStr8 str);
    }
}
