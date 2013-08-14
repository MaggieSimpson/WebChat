/// <reference path="jquery-2.0.3.min.js" />

var controllers = (function () {

    var updateTimer = null;

    var rootUrl = "http://localhost:49530/api/";

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },

        loadUI: function (selector) {
            if (this.persister.isUserLoggedIn()) {
                this.loadLoggedUserUI(selector);
            }
            else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);
        },

        loadLoginFormUI: function (selector) {
            $(selector).load('/PartialHtmls/LoginRegister.html');
        },

        loadLoggedUserUI: function (selector) {
            $(selector).load('/PartialHtmls/ProfilePicture.html');
            this.persister.user.online(function (data) {
                var users = ui.buildOnlineUsersList(data);
                $(selector).append(users);
            }, function (error) { alert(JSON.stringify(error))});
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            wrapper.on("click", "#btn-show-login", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#login-form").show();
                wrapper.find("#register-form").hide();

            });

            wrapper.on("click", "#btn-show-register", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#register-form").show();
                wrapper.find("#login-form").hide();
            });
            wrapper.on("click", "#btn-logout", function () {
                self.persister.user.logout(function () { self.loadLoginFormUI(selector) }, function () { alert("logout err") });
            });

            wrapper.on("click", "#btn-login", function () {
                var user = {
                    username: $(selector + " #tb-login-username").val(),
                    password: $(selector + " #tb-login-password").val()
                }

                self.persister.user.login(user, function () {
                    self.loadLoggedUserUI(selector);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });

                return false;
            });

            wrapper.on("click", "#btn-register", function () {
                var user = {
                    firstName: $(selector).find("#tb-register-firstName").val(),
                    lastName: $(selector).find("#tb-register-lastName").val(),
                    username: $(selector).find("#tb-register-username").val(),
                    password: $(selector + " #tb-register-password").val()
                }
                self.persister.user.register(user, function () {
                    self.loadLoginFormUI(selector);
                }, function (err) {
                });
                return false;
            });

            wrapper.on("click", "#btn-submit-image", function (e) {
                e.stopPropagation();
                self.persister.user.uploadImage(function () { alert("success picture upload") },
                    function () { alert("error picture upload") });
            });

            wrapper.on("click", ".online-user", function () {
                //username
                var username = $(this).data("username");
                self.persister.message.byUsername(username, function (data) {
                    $("#messages-wrapper").remove();
                    var messages = ui.buildMessages(data);
                    $(selector).append(messages);
                    $("#messages-wrapper").data("recieverUsername", username);
                }, function (err) {
                });
                return false;
            });

            wrapper.on("submit", "#form-send-message", function (event) {
                event.preventDefault();

                var input = $(this).find("[name=content]");
                input.attr('disabled', 'disabled')
                var recieverUsername = $("#messages-wrapper").data("recieverUsername");
                self.persister.message.send(recieverUsername, input.val(), function () {
                    input.val("");
                    input.removeAttr('disabled')
                }, function () { });
            });
        },

        updateUI: function (selector) {

        }
    });
    return {
        get: function () {
            return new Controller();
        }
    }
}());

$(function () {
    var controller = controllers.get();
    controller.loadUI("#content");
});