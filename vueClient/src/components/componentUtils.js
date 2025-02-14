import { NButton } from "naive-ui";
import { h } from "vue";

export const buildActionListButton = (name, onClick) =>
  h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick,
    },
    name
  );
