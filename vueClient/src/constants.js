const BACKED_URL = "http://localhost:5027/api/";

export const AUTH_URL = BACKED_URL + "auth/login";
export const LOGOUT_URL = BACKED_URL + "auth/logout";
export const USER_INFO_URL = BACKED_URL + "auth/userInfo";

export const PAGIANTED_ORDER_LIST_URL = BACKED_URL + "orders/paginated";
export const GET_ORDER_ID_URL = (id) => BACKED_URL + `orders/${id}`;
export const GET_ACCEPT_ORDER_URL = (id) => BACKED_URL + `orders/${id}/accept`;
export const GET_CLOSE_ORDER_URL = (id) => BACKED_URL + `orders/${id}/close`;
export const ORDER_URL = BACKED_URL + `orders`;

export const ITEMS_URL = BACKED_URL + `items`;
export const PAGIANTED_ITEMS_LIST_URL = BACKED_URL + "items/paginated";
export const GET_ITEM_ID_URL = (id) => BACKED_URL + `items/${id}`;

export const GET_SUGGESTS_URL = (query) =>
  BACKED_URL + `suggests/category?query=${query}`;

export const USERS_URL = BACKED_URL + `users`;
export const PAGINATED_USERS_LIST_URL = BACKED_URL + "users/paginated";
export const GET_USER_ID_URL = (id) => BACKED_URL + `users/${id}`;

export const MY_CART_URL = BACKED_URL + "carts/my";

export const ROLES = {
  Manager: 0,
  Customer: 1,
};

export const FILTER_ROLES = {
  All: 0,
  Manager: 1,
  Customer: 2,
};

export const FilterOrderStatus = {
  All: 0,
  New: 1,
  InProgress: 2,
  Completed: 3,
};

export const ORDER_STATUS = {
  New: 0,
  InProgress: 1,
  Completed: 2,
};

export const CART_ACTION_TYPE = {
  AddItems: 0,
  RemoveItems: 1,
};
