namespace Soario.Rendering;

/// <summary>
/// 着色器阶段
/// </summary>
public enum ShaderStage
{
    /// <summary>
    /// 像素着色器
    /// </summary>
    Ps = 1,
    /// <summary>
    /// 顶点着色器
    /// </summary>
    Vs,
    /// <summary>
    /// 计算着色器
    /// </summary>
    Cs,
    /// <summary>
    /// 面网着色器
    /// </summary>
    Ms,
    /// <summary>
    /// 装配着色器
    /// </summary>
    As,
}

/// <summary>
/// 语义化的 bool 值
/// </summary>
public enum ShaderSwitch
{
    /// <summary>
    /// 关闭
    /// </summary>
    Off = 0,
    /// <summary>
    /// 启用
    /// </summary>
    On = 1,
}

/// <summary>
/// 填充模式
/// </summary>
public enum ShaderFill
{
    // 绘制连接顶点的线条， 不绘制相邻顶点
    WireFrame = 2,
    // 填充顶点形成的三角形， 不绘制相邻顶点
    Solid = 3,
}

/// <summary>
/// 剔除模式
/// </summary>
public enum ShaderCull
{
    /// <summary>
    /// 始终绘制所有三角形
    /// </summary>
    Off = 1,
    /// <summary>
    /// 不要绘制正面的三角形
    /// </summary>
    Front = 2,
    /// <summary>
    /// 不要绘制朝背的三角形
    /// </summary>
    Back = 3,
}

/// <summary>
/// 比较方式
/// </summary>
public enum ShaderComp
{
    Off = 0,
    Never = 1,
    Lt = 2,
    Eq = 3,
    LE = 4,
    Gt = 5,
    Ne = 6,
    Ge = 7,
    Always = 8,
}

/// <summary>
/// 模板操作
/// </summary>
public enum ShaderStencilOp
{
    Keep = 1,
    Zero = 2,
    Replace = 3,
    IncrSat = 4,
    DecrSat = 5,
    Invert = 6,
    Incr = 7,
    Decr = 8,
}

/// <summary>
/// 模板操作逻辑
/// </summary>
public record ShaderStencilLogic
{
    /// <summary>
    /// 比较方式
    /// </summary>
    public ShaderComp? Comp { get; set; }
    /// <summary>
    /// 通过时的操作
    /// </summary>
    public ShaderStencilOp? Pass { get; set; }
    /// <summary>
    /// 失败时的操作
    /// </summary>
    public ShaderStencilOp? Fail { get; set; }
    /// <summary>
    /// 深度失败时的操作
    /// </summary>
    public ShaderStencilOp? ZFail { get; set; }
}

/// <summary>
/// 模板
/// </summary>
public record ShaderStencil : ShaderStencilLogic
{
    /// <summary>
    /// 参考值
    /// </summary>
    public byte? Ref { get; set; }
    /// <summary>
    /// 读取遮罩
    /// </summary>
    public byte? ReadMask { get; set; }
    /// <summary>
    /// 写入遮罩
    /// </summary>
    public byte? WriteMask { get; set; }
    /// <summary>
    /// 单独覆盖正面逻辑
    /// </summary>
    public ShaderStencilLogic? Front { get; set; }
    /// <summary>
    /// 单独覆盖背面逻辑
    /// </summary>
    public ShaderStencilLogic? Back { get; set; }
}

public enum ShaderRtLogicOp
{
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

public record ShaderPassRtDesc
{
    /// <summary>
    /// 颜色遮罩 R G B A 任意组合 // todo 搞个专用结构处理 json 序列化
    /// </summary>
    public string? ColorMask { get; set; }
    /// <summary>
    /// 混合模式 // todo 搞个专用格式处理 json 序列化
    /// </summary>
    public string? Blend { get; set; }
    /// <summary>
    /// 混合操作 // todo 搞个专用格式处理 json 序列化
    /// </summary>
    public string? BlendOp { get; set; }
    /// <summary>
    /// 逻辑操作
    /// </summary>
    public ShaderRtLogicOp? LogicOp { get; set; }
}

/// <summary>
/// 此类型仅用于反序列化
/// </summary>
public record ShaderPassStateMeta : ShaderPassRtDesc
{
    /// <inheritdoc cref="ShaderFill"/>
    public ShaderFill? ShaderFill { get; set; }
    /// <inheritdoc cref="ShaderCull"/>
    public ShaderCull? Cull { get; set; }
    /// <summary>
    /// 是否启用保守光栅化
    /// </summary>
    public ShaderSwitch? Conservative { get; set; }
    /// <summary>
    /// 深度斜率 , 深度偏移 ; 范围 -1 到 1 // todo 搞个专用结构处理 json 序列化
    /// </summary>
    public string? Offset { get; set; }
    /// <summary>
    /// 深度裁剪
    /// </summary>
    public ShaderSwitch? ZClip { get; set; }
    /// <summary>
    /// 深度测试比较方式
    /// </summary>
    public ShaderComp? ZTest { get; set; }
    /// <summary>
    /// 深度写入
    /// </summary>
    public ShaderSwitch? ZWrite { get; set; }
    /// <summary>
    /// 模板
    /// </summary>
    public ShaderStencil? Stencil { get; set; }

    public ShaderPassRtDesc? Rt0 { get; set; }
    public ShaderPassRtDesc? Rt1 { get; set; }
    public ShaderPassRtDesc? Rt2 { get; set; }
    public ShaderPassRtDesc? Rt3 { get; set; }
    public ShaderPassRtDesc? Rt4 { get; set; }
    public ShaderPassRtDesc? Rt5 { get; set; }
    public ShaderPassRtDesc? Rt6 { get; set; }
    public ShaderPassRtDesc? Rt7 { get; set; }
    public ShaderPassRtDesc? Rt8 { get; set; }
}

/// <summary>
/// 着色器步骤元信息，此类型仅用于反序列化
/// </summary>
public record ShaderPassMeta : ShaderPassStateMeta
{
    /// <summary>
    /// 着色器阶段
    /// </summary>
    public required List<string> Stages { get; set; }
}

/// <summary>
/// 着色器元信息，此类型仅用于反序列化
/// </summary>
public record ShaderMeta : ShaderPassStateMeta
{
    /// <summary>
    /// 资产 ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 资产路径
    /// </summary>
    public required string Path { get; set; }
    /// <summary>
    /// 着色器步骤
    /// </summary>
    public required Dictionary<string, ShaderPassMeta> Pass { get; set; }
}
