export type Meta = ShaderMeta; // todo other

export type MetaBase<T extends string> = {
  type: T;
  id: string;
};

export type ShaderMeta = MetaBase<"shader"> & {
  // 可选的指定 hlsl 文件相对路径，默认使用 meta 同名
  src?: string;
  // shader 子元素
  items: ShaderMetaItem[];
};

// shader 子元素
export type ShaderMetaItem = {
  // pass 名称
  name: string
  // shader model
  sm?: "6_6";
  // 阶段
  type: "ps" | "vs" | "gs" | "hs" | "ds" | "cs" | "lib" | "ms" | "as";
  // 入口点
  main: string;
  // 可选的指定 hlsl 文件相对路径，默认回退到主 meta 的 src
  src?: string;
};
