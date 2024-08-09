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
        public void present_frame([NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FError*, void>)(lpVtbl[15]))((FGpuSurface*)Unsafe.AsPointer(ref this), err);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void resize([NativeTypeName("ccc::FInt2")] FInt2 new_size)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, FInt2, void>)(lpVtbl[16]))((FGpuSurface*)Unsafe.AsPointer(ref this), new_size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool get_v_sync()
        {
            return ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte>)(lpVtbl[17]))((FGpuSurface*)Unsafe.AsPointer(ref this)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set_v_sync([NativeTypeName("bool")] byte v)
        {
            ((delegate* unmanaged[Thiscall]<FGpuSurface*, byte, void>)(lpVtbl[18]))((FGpuSurface*)Unsafe.AsPointer(ref this), v);
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
        public void submit([NativeTypeName("const FGpuCmdList *")] FGpuCmdList* cmd_list, [NativeTypeName("ccc::FError &")] FError* err)
        {
            ((delegate* unmanaged[Thiscall]<FGpuQueue*, FGpuCmdList*, FError*, void>)(lpVtbl[7]))((FGpuQueue*)Unsafe.AsPointer(ref this), cmd_list, err);
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
        public FGpuRt* rt;

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

    public unsafe partial struct FGpuCmdList
    {
        [NativeTypeName("uint8_t *")]
        public byte* datas;

        [NativeTypeName("int32_t *")]
        public int* indexes;

        [NativeTypeName("size_t")]
        public nuint len;
    }

    public enum FGpuQueueType
    {
        Common,
        Compute,
        Copy,
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
