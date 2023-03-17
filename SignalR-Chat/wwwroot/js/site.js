$(document).ready(function () {
    const guserName = sessionStorage.getItem('SignalR_UserName');
    const guserId = sessionStorage.getItem('SignalR_UserId');

    if (guserName == undefined || guserId == undefined) {
        showAuthModal();
    } else {
        $('#userNameText').html('Hello ' + guserName);
        $('#userIdText').html(guserId);
    }

    $('#authContinueBtn').on('click', function () {
        var userName = $('#userName').val();
        getClientId(userName)
            .then(response => {
                var { userId, userName } = response;
                sessionStorage.setItem("SignalR_UserId", userId);
                sessionStorage.setItem("SignalR_UserName", userName);
                $('#userNameText').html('Hello ' + userName);
                $('#userIdText').html(userId);
                closeAuthModal();
            })
            .catch(error => {
                alert("An error occurred. Please try again.")
            });
    });

    function showAuthModal() {
        var authToggleBtn = $('#authModalToggleButton');
        if (authToggleBtn != undefined) {
            authToggleBtn.click();
        }
    }

    function closeAuthModal() {
        var authCloseBtn = $('#authModalCloseBtn');
        if (authCloseBtn != undefined) {
            authCloseBtn.click();
        }
    }

    function getClientId (userName) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: AuthURLs.GetClientId,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    userName: userName
                }),
                success: function (response) {
                    resolve(response);
                },
                error: function (error) {
                    reject();
                }
            });
        });
    }
});