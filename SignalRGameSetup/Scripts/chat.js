

function newMessage(newMessage) {

    console.log('sending message');

    // send to hub    chat.server.newMessage({
        name: clientName,
        message: newMessage,
        gameCode: gameSetup.GameCode
    });

}

function addChatMessage(name, message) {
    console.log('adding?');
    chatMessages.innerHTML = chatMessages.innerHTML +
        "<b>" + name + ": </b>" +
        message + "<br>";
    messageInput.value = '';
}

// these are notices such as a person entering or leaving the chat
function addNotice(message, color) {
    console.log('adding?');
    chatMessages.innerHTML = chatMessages.innerHTML +
        '<i style="color: ' + color + ';">' + message + " </i><br>";
}

// Add a person to the chat group
function addToChatGroup(participant) {
    console.log('adding to chat');
    chat.server.addToChatGroup({
        name: participant.Name,
        participantId: participant.ParticipantId,
        connectionId: participant.ConnectionId,
        gameCode: participant.GameCode
    });
}


// Variables 

var chat;

var chatArea = document.getElementById('chatArea');

var chatMessages = document.getElementById('chatMessages');
var addMessage = document.getElementById('addMessage');

var messageInput = document.getElementById('messageInput');
var addMessageBtn = document.getElementById('addMessageBtn');


// event handlers
addMessageBtn.addEventListener("click", function () { newMessage(messageInput.value); });