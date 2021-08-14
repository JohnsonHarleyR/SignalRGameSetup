// function that checks if the enter button has been hit while inside the chat message input
// if it has, then it will send a message
function checkIfEnterPressed(event) {
    console.log('pressed');
    if (event.keyCode == 13) {
        console.log('enter pressed');
        newMessage(messageInput.value);
    }
}

function newMessage(newMessage) {

    console.log('sending message');

    // send to hub    chat.server.newMessage({
        name: clientName,
        message: newMessage,
        gameCode: gameSetup.GameCode
    });

}

function addChatMessage(name, message) {
    if (message != '') {
        console.log('adding chat message');
        chatMessages.innerHTML = chatMessages.innerHTML +
            "<b>" + name + ": </b>" +
            message + "<br>";
        messageInput.value = '';

        // also save the chat
        saveChatHtml();
    }

}

// these are notices such as a person entering or leaving the chat
function addNotice(message, color) {
    console.log('adding notice');
    chatMessages.innerHTML = chatMessages.innerHTML +
        '<i style="color: ' + color + ';">' + message + " </i><br>";

    // also save the chat
    saveChatHtml();
}

// Add a person to the chat group
function addToChatGroup() {
    console.log('adding to chat');
    chat.server.addToChatGroup({
        name: clientName,
        participantId: clientId,
        connectionId: null,
        gameCode: gameSetup.GameCode
    });
}

// load the chat
function loadGameChat() {
    console.log('loading game chat')
    chat.server.loadGameChat({
        gameCode: gameSetup.GameCode
    })
}

function showChatHtml(chatHtml, doSave) {
    chatMessages.innerHTML = chatHtml;
    updateScroll();
    if (doSave) {
        console.log('saving chat...');
        saveChatHtml();
    }
}

// save the chat
function saveChatHtml() {
    console.log('saving chat');
    chat.server.saveGameChat({
        gameCode: gameSetup.GameCode,
        chatHtml: chatMessages.innerHTML,
        doSaveAfterShow: false
    })
}


// layout javascript

// used so the scrollbar stays at the bottom
function updateScroll() {
    chatMessages.scrollTop = chatMessages.scrollHeight;
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