﻿

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
    console.log('showing chat...');
    console.log('Html: ' + chatHtml);
    console.log('Do save: ' + doSave);
    chatMessages.innerHTML = "";
    chatMessages.innerHTML = chatHtml;
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

// Variables 

var chat;

var chatArea = document.getElementById('chatArea');

var chatMessages = document.getElementById('chatMessages');
var addMessage = document.getElementById('addMessage');

var messageInput = document.getElementById('messageInput');
var addMessageBtn = document.getElementById('addMessageBtn');


// event handlers
addMessageBtn.addEventListener("click", function () { newMessage(messageInput.value); });