﻿var ui = (function () {
    function buildOnlineUsersList(users) {
        var div = '<h1>Online users</h1>'
        var ul = '<ul id="online-users">';
        for (var i = users.length - 1; i >= 0; i--) {
            var li = $('<li class = "online-user" data-username=' + users[i].username + '/>');

            li.text(users[i].username);
            li.prepend('<img src="' + users[i].profilePicture + '" width="30" height = "50" />');
            if (users[i].messagesState)
            {
                li.addClass("unread");
            }
            ul +=li[0].outerHTML;
        }

        ul += '</ul>';
        div += ul;
        //div += '</div>';

        return div;
    }

    function buildMessages(users) {
        var div = '';

        div += '<form id="form-send-message">' +
            '<input type = "text" name="content" autofocus/>' +
        '</form>';
        //div += '<form id="sendFileForm" enctype="multipart/form-data">' +
        //            '<input name="file" type="file" />' +
        //            '<input type="submit" id="UploadFile" value="Upload" />' +
        //        '</form>';

        var ul = '<ul id="user-messages">';
        for (var i = users.length - 1; i >= 0; i--) {
            var li;
            if (users[i].content) {
                li = appendRecievedMessage(users[i].content, users[i].sender.username);
            } else {
                li = appendRecievedFile(users[i].filePath, users[i].sender.username);
            }
            ul += li;
        }

        ul += '</ul>';
        div += ul
        div += '<form id="sendFileForm" enctype="multipart/form-data">' +
                    '<input name="file" type="file" id="chooseFile" /><br/>' +
                    '<input type="submit" id="UploadFile" value="Upload" />' +
                '</form>';
        //div += '</div>';
        return div;
    }

    function appendRecievedMessage(messageContent, senderUsername) {
        var li = '<li class = "message">';
        li += '<div><h2>' + senderUsername + ': ';

        // Adding colors to the messages
        if (localStorage.getItem("username") == senderUsername) {
            li += '<span class="blue-message">';
        }
        else {
            li += '<span class="red-message">';
        }

        li += messageContent + '</span>' + '</h2></div>';
        li += '</li>';
        return li;
        //var li = '<li class = "message">';
        //li += '<div><h2>Sender: ' + senderUsername + '</h2></div>';
        //li += '<div class="message-content"><h2>Message: ' + messageContent + '</h2></div>';
        //li += '</li>';
        //return li;
    }

    function appendRecievedFile(messageContent, senderUsername) {
        var li = '<li class = "message">';
        li += '<h2>' + senderUsername ;
        li += ': <a href ="' + messageContent + '">CLICK ME</a></h2>';
        li += '</li>';
        return li;
    }

    function getProfileInfo(username) {
        var div = '';
        div += '<h2>' + username + '</h2>';
        div += '<img id= "profile-picture" src = "" width= "70" height = "50"/>';
        return div;
    }

    return {
        buildOnlineUsersList: buildOnlineUsersList,
        buildMessages: buildMessages,
        appendRecievedMessage: appendRecievedMessage,
        getProfileInfo: getProfileInfo,
        appendRecievedFile: appendRecievedFile
    };

}());