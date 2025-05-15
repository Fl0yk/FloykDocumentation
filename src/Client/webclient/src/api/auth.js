import { $host } from "./axios";

export const registerUser = async ({ username, email, password }) => {
  try {
    const { data } = await $host.post("Identity/registration", {
      username,
      email,
      password,
    });
    return data;
  } catch (err) {
    console.log(err);
    throw new Error(err.response?.data?.message || "Ошибка регистрации");
  }
};

export const confirmRegistration = async (userId, token) => {
  try {
    const { data } = await $host.get("Identity/registration/confirm", {
      params: { userId, token },
    });

    localStorage.setItem("accessToken", data.jwtToken);
    localStorage.setItem("refreshToken", data.refreshToken);
    console.log('Added tokens');

    return data;
  } catch (err) {
    throw new Error("Ошибка подтверждения регистрации");
  }
};

export const loginUser = async ({ username, password }) => {
  try {
    const { data } = await $host.post("Identity/login", {
      username,
      password,
    });

    localStorage.setItem("accessToken", data.jwtToken);
    localStorage.setItem("refreshToken", data.refreshToken);
    console.log('Added tokens');

    return data;
  } catch (err) {
    throw new Error("Ошибка авторизации");
  }
};

export const refreshToken = async () => {
  try {
    const jwt = localStorage.getItem("accessToken");
    const refresh = localStorage.getItem("refreshToken");

    const { data } = await $host.post("Identity/refresh", {
      jwt,
      refresh,
    });

    localStorage.setItem("accessToken", data.jwt);
    localStorage.setItem("refreshToken", data.refresh);
    return data;
  } catch (err) {
    throw new Error("Ошибка обновления токена");
  }
};