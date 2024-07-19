import * as path from "https://deno.land/std@0.224.0/path/mod.ts";
import yaml from "npm:yaml";
import { compress } from "https://deno.land/x/zip@v1.2.5/mod.ts";
import { existsSync } from "https://deno.land/std@0.224.0/fs/exists.ts";
import { ShaderMeta, ShaderMetaItem } from "./meta.ts";

console.log(
  "================================================== start build shader =================================================="
);

const [dxc, build_output, file_name, file_path, debug, root_path] = Deno.args;

const shader_path = path
  .relative(root_path, path.join(file_path, "..", file_name))
  .replaceAll("\\", "/");

const is_debug = debug.toLowerCase() == "d";

const meta_fiel_full_path = path.resolve(Deno.cwd(), file_path);

const meta: ShaderMeta = yaml.parse(Deno.readTextFileSync(meta_fiel_full_path));

const tmp_output_path = path.resolve(build_output, "./shader/tmp", meta.id);
Deno.mkdirSync(tmp_output_path, { recursive: true });
Deno.removeSync(tmp_output_path, { recursive: true });
Deno.mkdirSync(tmp_output_path, { recursive: true });

const meta_output_path = path.resolve(tmp_output_path, `meta.json`);
const output_meta: {
  id: string;
  path: string;
  items: Record<string, Omit<ShaderMetaItem, "src" | "main" | "name">>;
} = {
  id: meta.id,
  path: shader_path,
  items: {},
};

const files: string[] = [meta_output_path];

for (const item of meta.items) {
  const src = item.src || meta.src || `./${file_name}.hlsl`;
  const src_path = path.resolve(meta_fiel_full_path, "..", src);
  const name = item.name;
  const type = item.type;
  const sm = item.sm || "6_6";

  const obj_output_path = path.resolve(tmp_output_path, `${name}.o`);
  const re_output_path = path.resolve(tmp_output_path, `${name}.re`);
  // deno-lint-ignore no-explicit-any
  const obj: any = { ...item };
  delete obj.src;
  delete obj.name;
  delete obj.main;
  output_meta.items[name] = obj;

  files.push(obj_output_path, re_output_path);

  const cmd = new Deno.Command(dxc, {
    cwd: Deno.cwd(),
    args: [
      "-T",
      `${type}_${sm}`,
      "-E",
      item.main,
      is_debug ? "-Od" : "-O3",
      "-Zi",
      "-Qembed_debug",
      "-Fo",
      obj_output_path,
      "-Fre",
      re_output_path,
      src_path,
    ],
    stdout: "inherit",
    stderr: "inherit",
  });
  cmd.outputSync();
}

Deno.writeTextFileSync(meta_output_path, JSON.stringify(output_meta));

const output_dir_path = path.resolve(build_output, "bin", "assets", "shaders");
Deno.mkdirSync(output_dir_path, { recursive: true });
const output_path_zip = path.resolve(output_dir_path, `${meta.id}.zip`);
const output_path = path.resolve(output_dir_path, `${meta.id}.shader`);

await compress(
  files.filter((path) => existsSync(path)),
  output_path_zip
);

Deno.renameSync(output_path_zip, output_path);

console.log(
  "================================================== end build shader =================================================="
);
