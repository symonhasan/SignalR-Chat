var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function (res) {
})
    .catch(error => {

    });

connection.on("ReceiveMessage", function (message, user) {
    addMessage(message, user);
});

connection.on("ConnectionEstablished", function (message) {
    $('#loading').hide();
    $('#chat').show();
    var textElm = $('#connectionMsg');
    textElm.text(message);
    $('#chatBox').removeAttr('disabled');
});

$('#chatBox').on('keypress', function (e) {
    if (e.which == 13) {
        sendMessage();
    }
});

$('#sendBtn').on('click', function () {
    sendMessage();
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

function addMessage(message, src) {
    const div = document.createElement('div');
    div.classList.add('message-block');
    div.classList.add('d-flex');

    const userId = sessionStorage.getItem('SignalR_UserId');
    const textDiv = document.createElement('div');

    if (src == userId) {
        div.classList.add('justify-content-end');
        textDiv.classList.add('self-text');
    } else {
        div.classList.add('justify-content-start');
        textDiv.classList.add('recieved-text');
    }

    textDiv.innerHTML = message;
    div.appendChild(textDiv);
    document.getElementById('messages').appendChild(div);
}

function sendMessage() {
    const userId = $('#userIdText').text();
    const message = $('#chatBox').val();

    if (message.trim() != '') {
        connection.invoke("SendMessage", userId, message)
            .catch(err => {
                console.log(err);
            });
        $('#chatBox').val('');
    }
}