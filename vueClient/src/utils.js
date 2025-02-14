export const showErrorMessages = (mesager, errors) => {
  let mes = "";
  for (const message of errors) mes += message;
  mesager.error(mes);
};
