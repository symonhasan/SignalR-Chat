var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$('#chatBox').attr('disabled', true);

connection.start().then(function (res) {
})
    .catch(error => {

    });

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement('li');
    document.getElementById('messageBox').appendChild(li);
    li.textContent = `${user}: ${message}`;
});

connection.on("ConnectionEstablished", function (message) {
    var textElm = $('#connectionMsg');
    textElm.text(message);
    $('#chatBox').removeAttr('disabled');
});

$('#chatBox').on('keypress', function (e) {
    if (e.which == 13) {
        var user = $('#userNameText').text().replace("Hello ", "");
        var message = $('#chatBox').val();
        connection.invoke("SendMessage", user, message).catch(function (err) {
            console.log(err);
        })
        $('#chatBox').val('');
    }
});

function subscribeUser(userId, userName) {
    connection.invoke("Subscribe", userId, userName).then(res => {
        searchForConnection(userId);
    }).catch(err => {
        console.log(err);
    })
}

function searchForConnection(userId) {
    connection.invoke("SearchForConnection", userId).then(res => {
        console.log(res);
    })
        .catch(err => {
            console.log(err);
        })
}