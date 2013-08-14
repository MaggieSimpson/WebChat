var ui = (function () {
    function buildOnlineUsersList(users) {
        var ul = '<ul id="online-users">';
        for (var i = 0; i < users.length; i++) {
            var li = '<li class = "online-user" data-username=' + users[i].username + '>';
            li += users[i].username;
            li += '</li>';
            ul += li;
        }
        ul += '</ul>';
        return ul;
    }

    function buildMessages(users) {
        var div = '<div id="messages-wrapper">';
        var ul = '<ul id="user-messages">';
        for (var i = 0; i < users.length; i++) {
            var li = '<li class = "message">';
            li += users[i].sender.username;
            li += '<div class="message-content">' + users[i].content + '</div>';
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