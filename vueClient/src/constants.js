const BACKED_URL = "http://localhost:5027/api/";

export const AUTH_URL = BACKED_URL + "auth/login";
export const USER_INFO_URL = BACKED_URL + "auth/userInfo";

export const PAGIANTED_ORDER_LIST_URL = BACKED_URL + "orders/paginated";

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
