/// <reference path="jquery-2.0.3.min.js" />

var controllers = (function () {
    var rootUrl = "http://localhost:49530/api/";

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
            this.currentReciever = null;
        },

        loadUI: function (selector) {
            if (this.persister.isUserLoggedIn()) {
                this.loadLoggedUserUI(selector);
            } else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);
        },

        loadLoginFormUI: function (selector) {
            $(selector).load('/PartialHtmls/LoginRegister.html');
        },

        loadLoggedUserUI: function (selector) {
            var self = this;
            $(selector).load('/PartialHtmls/ProfilePicture.html', function () {
                self.persister.user.online(function (data) {
                    var users = ui.buildOnlineUsersList(data);
                    $(selector).append(users);
                }, function (error) { alert(JSON.stringify(error)); });
                var channel = self.persister.username() + "-channel";

                //$("#btn-logout").text("Logout ");
                var username = self.persister.username();
                var profileUrl = self.persister.user.profilePicture(function (data) {
                    $("#profile-picture").attr("src", data);

                }, function () {
                });
                var profileInfo = ui.getProfileInfo(username);
                $(selector).append(profileInfo);

                PUBNUB.subscribe({
                    channel: channel,
                    callback: function (message) {
                        var message = JSON.parse(message);
                        if (message.Sender.Username == self.currentReciever) {
                            var li = ui.appendRecievedMessage(message.Content, self.persister.username());
                            $("#user-messages").prepend(li);
                        } else {
                            $(".online-user[data-username=" + message.Sender.Username + "]").addClass('unread');
                        }
                    }
                });
            });
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
                self.persister.user.logout(function () { self.loadLoginFormUI(selector); }, function () { alert("logout err"); });
            });

            wrapper.on("click", "#btn-login", function () {
                var user = {
                    username: $(selector + " #tb-login-username").val(),
                    password: $(selector + " #tb-login-password").val()
                };
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
                };
                self.persister.user.register(user, function () {
                    self.loadLoginFormUI(selector);
                }, function (err) {
                });
                return false;
            });

            wrapper.on("submit", "#uploadImageForm", (function (e) {
                var formData = new FormData(this);

                self.persister.user.uploadProfilePicture(formData, function () {
                    self.persister.user.profilePicture(function (data) {
                        $("#profile-picture").attr("src", data);
                    }, function () {
                        alert("Error getting profile picture");
                    });
                }, function () {
                    alert("Error uploading picture");
                });

                e.preventDefault();
            }));

            wrapper.on("click", ".online-user", function () {
                $(this).removeClass('unread');
                var username = $(this).data("username");
                self.persister.message.byUsername(username, function (data) {
                    $("#messages-wrapper").remove();
                    var messages = ui.buildMessages(data);
                    $(selector).append(messages);
                    self.currentReciever = username;
                }, function (err) {
                });
                return false;
            });

            wrapper.on("submit", "#form-send-message", function (event) {
                event.preventDefault();

                var input = $(this).find("[name=content]");
                input.attr('disabled', 'disabled');
                var recieverUsername = self.currentReciever;
                self.persister.message.send(recieverUsername, input.val(), function () {

                    input.removeAttr('disabled');

                    var li = ui.appendRecievedMessage(input.val(), self.persister.username());

                    $("#user-messages").prepend(li);
                    input.val("");
                }, function () {
                });
            });
        },

        updateUI: function (selector) {

        }
    });
    return {
        get: function () {
            return new Controller();
        }
    };
}());

var controller;
$(function () {
    controller = controllers.get();
    controller.loadUI("#content");
});