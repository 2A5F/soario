import * as path from "https://deno.land/std@0.224.0/path/mod.ts";
import yaml from "npm:yaml";
import { compress } from "https://deno.land/x/zip@v1.2.5/mod.ts";
import { existsSync } from "https://deno.land/std@0.224.0/fs/exists.ts";

const [dxc, build_output, file_name, file_path, debug] = Deno.args;

const is_debug = debug.toLowerCase() == "d";

const meta_fiel_full_path = path.resolve(Deno.cwd(), file_path);

type Meta = {
  src: string;
  items: MetaItem[];
};

type MetaItem = {
  sm?: "6_6";
  type: "ms" | "ps";
  main: string;
  src?: string;
};

const meta: Meta = yaml.parse(Deno.readTextFileSync(meta_fiel_full_path));

const tmp_output_path = path.resolve(build_output, "./shader/tmp", file_name);
Deno.mkdirSync(tmp_output_path, { recursive: true });

const meta_output_path = path.resolve(tmp_output_path, `${file_name}.json`);
Deno.writeTextFileSync(
  meta_output_path,
  JSON.stringify({ items: meta.items.map((a) => a.main) })
);

const files: string[] = [meta_output_path];

for (const item of meta.items) {
  const src = item.src || meta.src;
  const src_path = path.resolve(meta_fiel_full_path, "..", src);
  const main = item.main;
  const type = item.type;
  const sm = item.sm || "6_6";

  const obj_output_path = path.resolve(
    tmp_output_path,
    `${file_name}.${main}.o`
  );
  const re_output_path = path.resolve(
    tmp_output_path,
    `${file_name}.${main}.re`
  );
  const meta_output_path = path.resolve(
    tmp_output_path,
    `${file_name}.${main}.json`
  );
  Deno.writeTextFileSync(meta_output_path, JSON.stringify({ type, main, sm }));

  files.push(obj_output_path, re_output_path, meta_output_path);

  const cmd = new Deno.Command(dxc, {
    cwd: Deno.cwd(),
    args: [
      "-T",
      `${type}_${sm}`,
      "-E",
      main,
      is_debug ? "-Od" : "-O3",
      "-Zi",
      "-Fo",
      obj_output_path,
      "-Fre",
      re_output_path,
      src_path,
    ],
  });
  cmd.outputSync();
}

const output_dir_path = path.resolve(build_output, "bin", "shader");
Deno.mkdirSync(output_dir_path, { recursive: true });
const output_path_zip = path.resolve(output_dir_path, `${file_name}.zip`);
const output_path = path.resolve(output_dir_path, `${file_name}.shader`);
console.log(output_path);

await compress(
  files.filter((path) => existsSync(path)),
  output_path_zip
);

Deno.renameSync(output_path_zip, output_path);
