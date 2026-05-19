export default {
  plugins: {
    "postcss-import": {},
    "postcss-mixins": {
      mixinsFiles: "./wwwroot/styles/mixins/**/*.css",
    },
    "postcss-nested": {},
    autoprefixer: {},
    cssnano: {},
  },
  map: process.env.NODE_ENV !== "production",
};
