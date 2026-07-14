window.authHelper = {

    setToken: function (token) {
        localStorage.setItem("access_token", token);
    },

    getToken: function () {
        return localStorage.getItem("access_token");
    },

    removeToken: function () {
        localStorage.removeItem("access_token");
    }
};