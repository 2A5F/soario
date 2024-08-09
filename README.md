# soario

- 计划是搞一个工厂游戏
- 设计方向：
  - 说实话么想好，可能有几个方向
    - factorio 类似的 2d
    - 无限平面 3d 方块体素
    - 宇宙星球为单位，高度图星球
- 开发方向：
  - c艹 作为入口，和 c# 深度结合
  - 用 deno ts 做资产构建
  - 尽可能用 mesh shader、无绑定等现代特性

## 构建
- 需要 windows 11 环境
- 使用了 gitmodule，需要 clone submodule
- 需要 pwsh 7 环境
- 需要 .net8 环境
- 需要环境安装 [deno](https://deno.com/)
  ```
  irm https://deno.land/install.ps1 | iex
  ```
  - 当前 deno 版本
    ```
    deno 1.45.2
    ```
- 需要环境安装 [ClangSharpPInvokeGenerator](https://github.com/dotnet/ClangSharp)
  ```
  dotnet tool install --global ClangSharpPInvokeGenerator --version <版本>
  ```

## 备注 
- 命名空间 `ccc`只是方便敲，没有特殊意义
