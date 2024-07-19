using Soario.Native;

namespace Soario.Rendering;

public sealed unsafe class ShaderPass : IDisposable, IEquatable<ShaderPass>
{
    #region Fields

    internal FShaderPass* m_inner;
    public string Name { get; }

    #endregion

    #region Ctor

    internal ShaderPass(string name, FShaderPass* inner)
    {
        Name = name;
        m_inner = inner;
    }

    #endregion

    #region Dispose

    private void ReleaseUnmanagedResources()
    {
        if (m_inner == null) return;
        m_inner->Release();
        m_inner = null;
    }
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~ShaderPass() => ReleaseUnmanagedResources();

    #endregion

    #region Equals

    public bool Equals(ShaderPass? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return m_inner == other.m_inner;
    }
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ShaderPass other && Equals(other);
    }
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return unchecked((int)(long)m_inner);
    }
    public static bool operator ==(ShaderPass? left, ShaderPass? right)
    {
        return Equals(left, right);
    }
    public static bool operator !=(ShaderPass? left, ShaderPass? right)
    {
        return !Equals(left, right);
    }

    #endregion

    #region Load

    public static ShaderPass Load(Shader.PassData pass_data) => Load(Gpu.Instance, pass_data);

    public static ShaderPass Load(Gpu gpu, Shader.PassData pass_data)
    {
        if (gpu is null) throw new NullReferenceException($"{gpu} is null");

        FError err;
        FShaderPassData fd = default;
        FShaderPass* p;
        if (pass_data.Cs is { } cs)
        {
            fixed (byte* blob_cs = cs.Blob)
            {
                fixed (byte* re_cs = cs.ReflectionBlob)
                {
                    var data_cs = new FShaderStageData
                    {
                        blob = new(blob_cs, cs.Blob.Length),
                        reflection = new(re_cs, cs.ReflectionBlob.Length),
                    };
                    fd.Cs = &data_cs;

                    p = FShaderPass.load(&err, gpu.m_inner, &fd);
                }
            }
        }
        else if (pass_data.Ps is { } ps)
        {
            fixed (byte* blob_ps = ps.Blob)
            {
                fixed (byte* re_ps = ps.ReflectionBlob)
                {
                    var data_ps = new FShaderStageData
                    {
                        blob = new(blob_ps, ps.Blob.Length),
                        reflection = new(re_ps, ps.ReflectionBlob.Length),
                    };
                    fd.Ps = &data_ps;

                    if (pass_data.Vs is { } vs)
                    {
                        fixed (byte* blob_vs = vs.Blob)
                        {
                            fixed (byte* re_vs = vs.ReflectionBlob)
                            {
                                var data_vs = new FShaderStageData
                                {
                                    blob = new(blob_vs, vs.Blob.Length),
                                    reflection = new(re_vs, vs.ReflectionBlob.Length),
                                };
                                fd.Vs = &data_vs;
                                p = FShaderPass.load(&err, gpu.m_inner, &fd);
                            }
                        }
                    }
                    else if (pass_data.Ms is { } ms)
                    {
                        fixed (byte* blob_ms = ms.Blob)
                        {
                            fixed (byte* re_ms = ms.ReflectionBlob)
                            {
                                var data_ms = new FShaderStageData
                                {
                                    blob = new(blob_ms, ms.Blob.Length),
                                    reflection = new(re_ms, ms.ReflectionBlob.Length),
                                };
                                fd.Ms = &data_ms;

                                if (pass_data.As is { } ts)
                                {
                                    fixed (byte* blob_as = ts.Blob)
                                    {
                                        fixed (byte* re_as = ts.ReflectionBlob)
                                        {
                                            var data_as = new FShaderStageData
                                            {
                                                blob = new(blob_as, ts.Blob.Length),
                                                reflection = new(re_as, ts.ReflectionBlob.Length),
                                            };
                                            fd.As = &data_as;
                                            p = FShaderPass.load(&err, gpu.m_inner, &fd);
                                        }
                                    }
                                }
                                else
                                {
                                    p = FShaderPass.load(&err, gpu.m_inner, &fd);
                                }
                            }
                        }
                    }
                    else throw new Exception("Invalid pipeline");
                }
            }
        }
        else throw new Exception("Invalid pipeline");
        if (p == null) err.Throw();
        return new(pass_data.Name, p);
    }

    #endregion
}
