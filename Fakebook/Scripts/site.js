//Statuses

var toggleLike = function (_statusId) {
    $.ajax({
        type: "GET",
        url: '/Social/ToggleLike',
        data: { statusId: _statusId },
        success: function (response) {
            changeLikeButtonText(statusId = _statusId, amount = response);
        }
    });
}

var changeLikeButtonText = function (statusId, amount) {
    var button = document.querySelector("#likeButton" + statusId);
    var amountOfLikesButton = document.querySelector("#amountOfLikes" + statusId);
    if (button.value == "Like") {
        button.value = "Liked";
        button.setAttribute("class", "LikedButton");
    }
    else {
        button.value = "Like";
        button.setAttribute("class", "DefaultButton");
    }
    amountOfLikesButton.text = amount + " Likes";
}

//Friends
var toggleFriend = function (friendId) {
    $.ajax({
        type: "GET",
        url: '/Friend/ToggleFriend',
        data: { id: friendId},
        success: function (response) {
            changeFriendButtonText(buttonId = friendId, message = response);
        }
    });
}

var rejectFriend = function (friendId) {
    $.ajax({
        type: "GET",
        url: '/Friend/RejectRequest',
        data: { id: friendId },
        success: function (response) {
            changeFriendButtonText(buttonId = friendId, message = response);
        }
    });
}

var changeFriendButtonText = function (buttonId, message) {
    var button = document.querySelector("#friendButton" + buttonId);
    var rejectButton = document.querySelector("#rejectButton" + buttonId);
    var friendAction = document.querySelector("#friendAction" + buttonId);

    if (rejectButton != null) {
        rejectButton.remove();
        friendAction.removeAttribute("style");
        friendAction.removeAttribute("class");
        button.className  = "UserButton";
    }

    if (message == "Cancel Request") {
        button.setAttribute("class", "RemoveButton");
    }
    else if (message == "Add Friend") {
        button.setAttribute("class", "AcceptButton");
    }

    button.value = message;
}


//Check For Friend Requests
var friendNavButton = document.querySelector("#friendNavButton");

function checkForNewFriendRequests() {
    $.ajax({
        type: "GET",
        url: '/Friend/CheckForFrendRequests',
        success: function (response) {
            if (response.trim() != '') {
                friendNavButton.text = "Friends (NEW)";
            }
        }
    });
}
checkForNewFriendRequests();
var friendRequestInterval = setInterval(checkForNewFriendRequests, 10000);

//Check For New Messages
var messageNavButton = document.querySelector("#messageNavButton");

function checkForNewMessages() {
    $.ajax({
        type: "GET",
        url: '/Chat/CheckForNewMessages',
        success: function (response) {
            if (response.trim() != '') {
                messageNavButton.text = "Messages (" + response + ")";
            }
        },
        error: function () {
            console.log("error");
        }
    });
};

checkForNewMessages();
var messageInterval = setInterval(checkForNewMessages, 10000);
