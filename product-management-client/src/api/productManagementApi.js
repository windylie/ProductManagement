import axios from 'axios';

const { REACT_APP_API_HOST } = process.env;

const axiosInstance = axios.create({
    baseURL: REACT_APP_API_HOST + '/api'
});

axiosInstance.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error.response.status === 401) {
        window.location = '/user/login';
    }
    return Promise.reject(error);
});

axiosInstance.interceptors.request.use((request) => {
    if (request.url.indexOf("user") !== -1) {
        return request;
    }

    const token = localStorage.getItem('token');
    if (!token) {
        window.location = '/user/login';
        return request;
    }

    request.headers[request.method]['Authorization'] = 'Bearer ' + token;
    return request;
  }, (error) => {
    return Promise.reject(error);
  });

export default axiosInstance;
