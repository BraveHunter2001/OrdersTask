const BACKED_URL = "http://localhost:5027/api/";

export const AUTH_URL = BACKED_URL + "auth/login";
export const USER_INFO_URL = BACKED_URL + "auth/userInfo";

export const PAGIANTED_ORDER_LIST_URL = BACKED_URL + "orders/paginated";
export const GET_ACCEPT_ORDER_URL = (id) => BACKED_URL + `orders/${id}/accept`;
export const GET_CLOSE_ORDER_URL = (id) => BACKED_URL + `orders/${id}/close`;

export const ROLES = {
  Manager: 0,
  Customer: 1,
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
