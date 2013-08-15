/// <reference path="jquery-2.0.3.min.js" />
var httpRequester = (function () {
    function getJSON(url, success, error) {
        $.ajax({
            url: url,
            type: "GET",
            timeout: 7000,
            contentType: "application/json",
            success: success,
            error: error
        });
    }

    function postJSON(url, data, success, error) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            timeout: 7000,
            data: JSON.stringify(data),
            success: success,
            error: error
        });
    }

    function postFileData(url, data, success, error) {
        $.ajax({
            url: url,
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: success,
            error: error
        });
    }

    return {
        getJSON: getJSON,
        postJSON: postJSON,
        postFileData: postFileData
    };
}());