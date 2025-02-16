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

export const BaseItemColumn = [
  {
    title: "Code",
    key: "code",
  },
  {
    title: "Name",
    key: "name",
  },
  {
    title: "Price",
    key: "price",
  },
  {
    title: "Category",
    key: "category",
  },
];

export const getSelectedItems = (selectedItems, items, filter) => {
  const ids = new Set(Object.values(selectedItems));
  const filtredItems = items.filter((i) => filter(i, ids));
  return filtredItems;
};
