import axios from 'axios';

//const { REACT_APP_API_HOST } = process.env;

export default axios.create({
    //baseURL: REACT_APP_API_HOST + '/api'
    baseURL: 'https://localhost:44326/api'
});
