import axios from "axios";
import { UserCredential } from './contexts/AuthContext';
import { AxiosConstants, LocalStorageConstants } from "./common/constants";
import { useNavigate } from "react-router-dom";
import { routeNames } from "./routesConstants";

const axiosInstance = axios.create({
    baseURL: AxiosConstants.AXIOS_BASEURL,
    timeout: AxiosConstants.AXIOS_TIMEOUT,
    headers: AxiosConstants.AXIOS_HEADER
});



axiosInstance.interceptors.request.use(
    (config) => {
        const storedCredential = localStorage.getItem(LocalStorageConstants.USER_CREDENTIAL);
        const userCredential: UserCredential | null = storedCredential ? JSON.parse(storedCredential) : null;
        if (userCredential) {
            config.headers['Authorization'] = `Bearer: ${userCredential.accessToken}`
        }

        if (process.env.NODE_ENV === 'development') {
            const method = config.method?.toUpperCase() ?? 'GET';
            const urlWithParams = method.concat(` ${config.url}`, (config.params ? `?${new URLSearchParams(config.params).toString()}` : ''));
            console.log('Request URL:', urlWithParams);
        }

        return config
    },
    (error) => {
        Promise.reject(new Error(error))
    }
)

axiosInstance.interceptors.response.use(
    response => {
        return response
    },
    error => {
        const navigate = useNavigate()
        // Any status codes that fall outside the range of 2xx cause this function to trigger
        if (error.response && error.response.status === 401) {
            // Handle unauthorized errors (e.g., redirect to login)
            console.error('Unauthorized, redirecting to login...');
            return navigate(routeNames.login);
        }
        // You can add other error handling logic here

        return Promise.reject(new Error(error));
    }
)

export default axiosInstance;