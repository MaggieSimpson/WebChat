/// <reference path="http-requester.js" />
/// <reference path="class.js" />
/// <reference path="http://crypto-js.googlecode.com/svn/tags/3.1.2/build/rollups/sha1.js" />

var persisters = (function () {
    var username = localStorage.getItem("username");
    var sessionKey = localStorage.getItem("sessionKey");

    function saveUserData(userData) {
        localStorage.setItem("username", userData.username);
        localStorage.setItem("sessionKey", userData.sessionKey);
        username = userData.username;
        sessionKey = userData.sessionKey;
    }

    function clearUserData() {
        localStorage.removeItem("username");
        localStorage.removeItem("sessionKey");
        username = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.message = new MessagesPersister(this.rootUrl);
        },

        isUserLoggedIn: function () {
            var isLoggedIn = username != null && sessionKey != null;
            return isLoggedIn;
        },

        username: function () {
            return username;
        }
    });

    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "user/";
        },

        login: function (user, success, error) {
            var url = this.rootUrl + "login";
            var userData = {
                username: user.username,
                password: CryptoJS.SHA1(user.username + user.password).toString()
            };

            httpRequester.postJSON(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },

        register: function (user, success, error) {
            var url = this.rootUrl + "register";
            var userData = {
                username: user.username,
                password: CryptoJS.SHA1(user.username + user.password).toString(),
                firstName: user.firstName,
                lastName: user.lastName
            };

            httpRequester.postJSON(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },

        logout: function (success, error) {
            var url = this.rootUrl + "logout/" + sessionKey;
            httpRequester.getJSON(url, function (data) {
                clearUserData();
                success(data);
            }, error)
        }
    });


    var MessagesPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "message/";
        },

        all: function (success, error) {
            var url = this.rootUrl + "?sessionKey=" + sessionKey;
            httpRequester.getJSON(url, success, error);
        },

        send: function (recieverUsername, content, success, error) {
            var url = this.rootUrl + "?sessionKey=" + sessionKey + "&reciever=" + recieverUsername;
            var messageData = {
                reciever: recieverUsername,
                state: false
            };

            httpRequester.postJSON(url, messageData, success, error);
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());