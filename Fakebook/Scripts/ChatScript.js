    var chatHistory = document.querySelector("#discussion");
    var messageButton = document.querySelector("#sendmessage");
    var messageBox = document.querySelector("#message");
    var senderId = document.querySelector("#senderId").value;
    var recieverId = document.querySelector("#recieverId").value;

    chatHistory.scrollTop = chatHistory.scrollHeight;

    // Reference the auto-generated proxy for the hub.  
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (senderId, text, sentAt) {
        // Add the message to the page. 
        chatHistory.innerHTML += encodeMessage(senderId, text, sentAt);
        chatHistory.scrollTop = chatHistory.scrollHeight;
    };

    

    // Set initial focus to message input box.  
    messageBox.focus();
    // Start the connection.

    $.connection.hub.start().done(
        function () {
            chat.server.addToGroup(senderId, recieverId);
        });

window.onbeforeunload = function (e) {
    $.ajax({
        type: "GET",
        url: '/Chat/ResetFriendId'
    });
};


//Send the message 

    var sendMessage = function () {
        // Call the Send method on the hub. 
        if (messageBox.value != '') {
            chat.server.send(senderId, recieverId, messageBox.value);
            // Clear text box and reset focus for next comment. 
            messageBox.value = '';
            messageBox.focus();
        }
};

//Check when message is being sent

    messageButton.addEventListener("click", sendMessage, false);
    document.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            sendMessage();
        }
    }, false);

//Convert message to html

function encodeMessage(senderId, text, sentAt) {
    var containerClass, timePosition, textAlign, contcolor, floatPos;
    if (senderId == this.senderId) {
        containerClass = "container";
        timePosition = "time-right text-light";
        textAlign = "text-right text-white messageText";
        contcolor = "bg-primary yourMessage";
        floatPos = "right";
    }
    else {
        containerClass = "container";
        timePosition = "time-left";
        textAlign = "text-left messageText";
        contcolor = "bg-light otherMessage";
        floatPos = "left";
    }


    let htmlText = `<div class="row">
                            <div class="${containerClass} ${contcolor} messageHolder" style="float:${floatPos}; width:auto;">
                                <p style="font-size: 1.5em" class="${textAlign}">${text}</p>
                                <span class="${timePosition}">${sentAt}</span>
                            </div>
                        </div>`;

    return htmlText;
};
