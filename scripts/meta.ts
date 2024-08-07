export type Meta = ShaderMeta; // todo other

export type MetaBase<T extends string> = {
  type: T;
  id: string;
  /** 默认为 on */
  bindless: "on" | "off";
};

export type ShaderMeta = MetaBase<"shader"> & {
  /** 可选的指定 hlsl 文件相对路径，默认使用 meta 同名 */
  src?: string;
  /** shader 步骤 */
  pass: Record<string, ShaderMetaPass>;
};

/** shader 步骤 */
export type ShaderMetaPass = {
  /** shader model */
  sm?: "6_6";
  /** 可选的指定 hlsl 文件相对路径，默认回退到主 meta 的 src */
  src?: string;
  /** 填充模式， 默认 solid */
  fill?: "solid" | "wireframe";
  /** 剔除模式，默认 back */
  cull?: "back" | "front" | "off";
  /** 是否启用保守光栅化，默认 off */
  conservative?: "on" | "off";
  /** 深度斜率 , 深度偏移 ; 范围 -1 到 1 */
  offset?: string;
  /** 深度裁剪，默认 on */
  zclip?: "on" | "off";
  /** 深度测试，默认 le */
  ztest?: ShaderComp | "off";
  /** 深度写入， 默认 on */
  zwrite?: "on" | "off";
  /** 模板 */
  stencil?: ShaderStencil;
} & { [S in ShaderMetaStage]: string } & ShaderMetaPassRT & {
    /** 单独对 rt 指定操作 */
    [K in `rt${
      | "0"
      | "1"
      | "2"
      | "3"
      | "4"
      | "5"
      | "6"
      | "7"}`]: ShaderMetaPassRT;
  };

/** 对 rt 指定的操作 */
export type ShaderMetaPassRT = {
  /** 颜色遮罩 rgba 任意组合 */
  colorMask?: string;
  /** 混合操作 */
  blend?:
    | `${ShaderBlend} ${ShaderBlend}`
    | `${ShaderBlend} ${ShaderBlend}, ${ShaderBlend} ${ShaderBlend}`
    | `${ShaderBlend} ${ShaderBlend},${ShaderBlend} ${ShaderBlend}`;
  blendOp?:
    | ShaderBlendOp
    | `${ShaderBlendOp}, ${ShaderBlendOp}`
    | `${ShaderBlendOp},${ShaderBlendOp}`;
  logicOp?: ShaderLogicOp;
};

export type ShaderBlend =
  | "off"
  | "zero"
  | "one"
  | "SrcColor"
  | "SrcAlpha"
  | "SrcAlphaSat"
  | "DstColor"
  | "DstAlpha"
  | "OneMinusSrcColor"
  | "OneMinusSrcAlpha"
  | "OneMinusDstColor"
  | "OneMinusDstAlpha";

export type ShaderBlendOp = "Add" | "Sub" | "RevSub" | "Min" | "Max";

export type ShaderLogicOp =
  | "Clear"
  | "One"
  | "Copy"
  | "CopyInv"
  | "Noop"
  | "Inv"
  | "And"
  | "Nand"
  | "Or"
  | "Nor"
  | "Xor"
  | "Equiv"
  | "AndRev"
  | "AndInv"
  | "OrRev"
  | "OrInv";

export type ShaderStencil = {
  /** 参考值，0 到 255，默认 0 */
  ref?: number;
  /** 读模板，0 到 255，默认 255 */
  readMask?: number;
  /** 读模板，0 到 255，默认 255 */
  writeMask?: number;
  /** 单独覆盖背面逻辑 */
  back?: ShaderStencilLogic;
  /** 单独覆盖正面逻辑 */
  front?: ShaderStencilLogic;
} & ShaderStencilLogic;

export type ShaderStencilLogic = {
  /** 比较方式 */
  comp?: ShaderComp;
  /** 通过时的操作 */
  pass?: ShaderStencilOp;
  /** 失败时的操作 */
  fail?: ShaderStencilOp;
  /** 深度失败时的操作 */
  zfail?: ShaderStencilOp;
};

export type ShaderComp =
  | "always"
  | "ne"
  | "ge"
  | "gt"
  | "le"
  | "eq"
  | "lt"
  | "never";

export type ShaderStencilOp =
  | "keep"
  | "zero"
  | "replace"
  | "incrSat"
  | "decrSat"
  | "invert"
  | "incr"
  | "decr";

export type ShaderMetaStage =
  | "ps"
  | "vs"
  | "gs"
  | "hs"
  | "ds"
  | "cs"
  | "lib"
  | "ms"
  | "as";
