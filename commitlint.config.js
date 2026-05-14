export default {
  extends: ["@commitlint/config-conventional"],
  plugins: [
    {
      rules: {
        "no-duplicate-breaking": ({ header, body, footer }) => {
          const hasExclamation = /^[a-z]+(\(.+\))?!:/.test(header);
          const bodyHasBreaking = /BREAKING[ -]CHANGE/i.test(body || "");
          const footerHasBreaking = /BREAKING[ -]CHANGE/i.test(footer || "");
          const pass = !(
            hasExclamation &&
            (bodyHasBreaking || footerHasBreaking)
          );
          return [
            pass,
            "Use either `!:` in the header or `BREAKING CHANGE` in the body/footer, not both. Using both causes duplicate entries in the changelog.",
          ];
        },
      },
    },
  ],
  rules: {
    "body-leading-blank": [1, "always"],
    "body-max-line-length": [2, "always", 100],
    "footer-leading-blank": [1, "always"],
    "footer-max-line-length": [2, "always", 100],
    "header-max-length": [2, "always", 100],
    "no-duplicate-breaking": [2, "always"],
    "subject-case": [
      2,
      "never",
      ["sentence-case", "start-case", "pascal-case", "upper-case"],
    ],
    "subject-empty": [2, "never"],
    "subject-full-stop": [2, "never", "."],
    "type-case": [2, "always", "lower-case"],
    "type-empty": [2, "never"],
    "type-enum": [
      2,
      "always",
      [
        "build",
        "chore",
        "ci",
        "docs",
        "feat",
        "fix",
        "perf",
        "refactor",
        "revert",
        "style",
        "test",
      ],
    ],
  },
};
