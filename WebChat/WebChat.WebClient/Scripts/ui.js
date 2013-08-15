var ui = (function () {
    function buildOnlineUsersList(users) {
        var div = '<div id ="online-users-holder"><h1>Online users</h1>'
        var ul = '<ul id="online-users">';

        for (var i = users.length - 1; i >= 0; i--) {
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

        div += '<form id="form-send-message">' +
            '<input type = "text" name="content" autofocus/>' +
        '</form>';

        var ul = '<ul id="user-messages">';
        for (var i = users.length - 1; i >= 0; i--) {
            var li = AppendRecievedMessage(users[i].content, users[i].sender.username);
            ul += li;
        }

        ul += '</ul>';
        div += ul;
        div += '</div>';
        return div;
    }

    function AppendRecievedMessage(messageContent, senderUsername) {
        var li = '<li class = "message">';
        li += '<div><h2>Sender: ' + senderUsername + '</h2></div>';
        li += '<div class="message-content"><h2>Message: ' + messageContent + '</h2></div>';
        li += '</li>';
        return li;
    }

    return {
        buildOnlineUsersList: buildOnlineUsersList,
        buildMessages: buildMessages,
        AppendRecievedMessage: AppendRecievedMessage
    }

}());