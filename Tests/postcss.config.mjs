import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = dirname(fileURLToPath(import.meta.url));

// Resolve _content/Hviktor/* imports to the Hviktor project's wwwroot at build time,
// since these paths are only available as ASP.NET static web assets at runtime.
const hviktorWwwroot = resolve(__dirname, "../Hviktor/wwwroot");

export default {
  plugins: {
    "postcss-import": {
      resolve(id, basedir) {
        const contentPrefix = "_content/Hviktor/";
        const contentPrefixWithSlash = "../_content/Hviktor/";
        if (id.startsWith(contentPrefixWithSlash)) {
          return join(hviktorWwwroot, id.slice(contentPrefixWithSlash.length));
        }
        if (id.startsWith(contentPrefix)) {
          return join(hviktorWwwroot, id.slice(contentPrefix.length));
        }
        return id;
      },
    },
    "postcss-mixins": {
      mixinsFiles: "./wwwroot/styles/mixins/**/*.css",
    },
    "postcss-nested": {},
    autoprefixer: {},
    cssnano: {},
  },
};
