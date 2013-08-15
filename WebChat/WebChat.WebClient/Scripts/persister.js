/// <reference path="http-requester.js" />
/// <reference path="class.js" />
/// <reference path="http://crypto-js.googlecode.com/svn/tags/3.1.2/build/rollups/sha1.js" />

var persisters = (function () {
    var username = localStorage.getItem("username");
    var sessionkey = localStorage.getItem("sessionkey");
    var profilePictureUrl = localStorage.getItem("profilePicture");
    function saveUserData(userData) {
        localStorage.setItem("username", userData.username);
        localStorage.setItem("sessionkey", userData.sessionkey);
        localStorage.setItem("profilePicture", userData.profilePicture);
        username = userData.username;
        sessionkey = userData.sessionkey;
        profilePictureUrl = userData.profilePicture;
    }

    function clearUserData() {
        localStorage.removeItem("username");
        localStorage.removeItem("sessionkey");
        localStorage.removeItem("profilePicture");

        username = "";
        sessionkey = "";
        profilePictureUrl = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.message = new MessagesPersister(this.rootUrl);
        },

        isUserLoggedIn: function () {
            var isLoggedIn = username != null && sessionkey != null;
            return isLoggedIn;
        },

        username: function () {
            return username;
        },

        profilePictureUrl: function () {
            return profilePictureUrl;
        },

        sessionKey: function () {
            return sessionkey;
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

        profilePicture: function (success, error) {
            var url = this.rootUrl + "profilePicture?sessionKey=" + sessionkey;
            httpRequester.getJSON(url, success, error);
        },
        
        uploadProfilePicture: function (data, success, error) {
            var url = this.rootUrl + "/uploadImage?sessionKey=" + sessionkey;

            httpRequester.postFileData(url, data, success, error);
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
            var url = this.rootUrl + "logout/";
            httpRequester.postJSON(url, {
                sessionkey: sessionkey
            }, function (data) {
                clearUserData();
                success(data);
            }, error)
        },

        uploadImage: function (success, error) {
            var url = this.rootUrl + "uploadImage/";
            httpRequester.postJSON(url, {
                sessionkey: sessionkey
            }, function (data) {
            }, error)
        },

        online: function (success, error) {
            var url = this.rootUrl + "online/?sessionKey=" + sessionkey;

            httpRequester.getJSON(url, success, error);
        }
    });


    var MessagesPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "message/";
        },

        all: function (success, error) {
            var url = this.rootUrl + "?sessionkey=" + sessionkey;
            httpRequester.getJSON(url, success, error);
        },

        byUsername: function (username, success, error) {
            var url = this.rootUrl + "byUsername?sessionkey=" + sessionkey + "&username=" + username;
            httpRequester.getJSON(url, success, error);
        },

        send: function (recieverUsername, content, success, error) {
            var url = this.rootUrl + "send?sessionkey=" + sessionkey + "&reciever=" + recieverUsername;
            var messageData = {
                reciever: recieverUsername,
                content: content,
                sessionKey: sessionkey
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