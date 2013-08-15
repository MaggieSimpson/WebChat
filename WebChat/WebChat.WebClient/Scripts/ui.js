var ui = (function () {
    function buildOnlineUsersList(users) {
        var div = '<div id ="online-users-holder"><h1>Online users</h1>'
        var ul = '<ul id="online-users">';

        for (var i = 0; i < users.length; i++) {
            var li = '<li class = "online-user" data-username=' + users[i].username + '>';
            li += users[i].username;
            li += '</li>';
            ul += li;
        }

        ul += '</ul>';
        div += ul;
        div += '</div>';

        return div;
    }

    function buildMessages(users) {
        var div = '<div id="messages-wrapper">';
        var ul = '<ul id="user-messages">';
        for (var i = 0; i < users.length; i++) {
            var li = '<li class = "message">';

            li += '<div class="user"><h2>Sender: '+users[i].sender.username + '</h2></div>';
            li += '<div class="message-content"><h2>Message: ' + users[i].content + '</h2></div>';
            li += '</li>';
            ul += li;
        }

        ul += '</ul>';
        div += ul;

        div += '<form id="form-send-message">' +
            '<input type = "text" name="content" autofocus/>' +
        '</form>';

        div += '</div>';
        return div;
    }

    return {
        buildOnlineUsersList: buildOnlineUsersList,
        buildMessages: buildMessages
    }

}());