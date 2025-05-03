import axios from "axios";

const BASE_URL = "https://localhost:9001/api/";

const $host = axios.create({
    baseURL: BASE_URL,
});

export { $host };