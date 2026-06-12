import { defineConfig } from "vite";
import { resolve, basename, extname } from "node:path";
import { readdirSync } from "node:fs";

function toKebab(name) {
  return name
    .replaceAll(/([a-z0-9])([A-Z])/g, "$1-$2") // fooBar -> foo-Bar
    .replaceAll(/([A-Z])([A-Z][a-z])/g, "$1-$2") // XMLParser -> XML-Parser
    .toLowerCase();
}

// Helper: build entries from a directory (non-recursive). For recursive, use fast-glob.
function buildDirEntries(
  dirAbsPath,
  exts = [".js", ".ts", ".tsx"],
  outputExt = ".js",
) {
  return Object.fromEntries(
    readdirSync(dirAbsPath)
      .filter((f) => exts.includes(extname(f))) // include TS
      .map((f) => {
        const raw = basename(f, extname(f)); // strip actual extension
        const name = toKebab(raw) + outputExt; // convert to kebab-case
        return [name, resolve(dirAbsPath, f)];
      }),
  );
}

const outputExt = ".css";
const styleExts = [outputExt, ".scss"];
const modalCss = resolve(__dirname, "wwwroot/styles/modals");
const modalCssEntries = buildDirEntries(modalCss, styleExts, outputExt);

export default defineConfig(async ({ mode }) => {
  const plugins = [];

  if (mode === "development") {
    const { visualizer } = await import("rollup-plugin-visualizer");
    plugins.push(
      visualizer({ open: true, filename: "vite-chunk-visualizer.html" }),
    );
  }

  return {
    plugins,
    base: `/_content/Hviktor/dist/`, // makes dynamic chunk imports absolute
    experimental: {
      renderBuiltUrl(filename, { hostType }) {
        // keep dynamic chunk imports absolute and CSS asset URLs relative
        if (hostType === "css") {
          return { relative: true };
        }
        return undefined; // undefined => default: prepend `base`
      },
    },
    build: {
      target: "esnext",
      cssTarget: ["chrome120", "firefox120", "safari17", "edge120"],
      outDir: resolve(__dirname, "wwwroot/dist"),
      emptyOutDir: true,
      sourcemap: mode === "development",
      manifest: true,
      cssCodeSplit: true,
      rollupOptions: {
        external: [
          "prettier",
          "@babel/core",
          "@babel/standalone",
          "@babel/parser",
          "typescript",
          /^@babel\//, // regex to catch all @babel/* packages
          /^prettier\//,
        ],
        input: {
          // Dynamic styles (assets)
          ...modalCssEntries,

          // Scripts
          entry: resolve(__dirname, "entry.ts"),
        },
        output: {
          entryFileNames: "[name].js",
          chunkFileNames(chunkInfo) {
            // Group lazy-loaded internal icon chunks into an icons/ subdirectory
            // Only 3 internal icons (files, arrows-circlepath, x-mark) - full icon library is in Hviktor.Icons
            if (chunkInfo.name?.startsWith("icon-")) {
              return `icons/${chunkInfo.name}.js`;
            }
            return "[name].js";
          },
          assetFileNames: "assets/[name][extname]",
        },
      },
    },
    // Must be run with vite server separately to msbuild/blazor with proxying
    // server: {
    //     hmr: true // Hot Module Replacement
    // }
  };
});
