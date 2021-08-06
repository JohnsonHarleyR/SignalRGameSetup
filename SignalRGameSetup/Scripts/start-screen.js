// Hub

// Creating a new room
function newRoom(allowAudienceChoice) {
    console.log("New room.");
    let clientName = enterName.value;
    console.log(clientName);

    // send to hub
    connection.server.newRoom({
        name: clientName,
        allowAudience: allowAudienceChoice
    });

    // callback method
    connection.client.enterNewRoom = function (setup) {
        gameSetup = setup;
        enterWaitRoom();
    };
}

function askAboutAudience() {
    console.log("asking...");
    allowAudienceModal.style.display = "block";
}

// return method for creating a room
function enterWaitRoom() {
    console.log("Entering wait room.");
    // hide the initial div and show the wait room
    intialOptions.style.display = "none";
    waitRoom.style.display = "block";
    roomCodeDisplay.innerHTML = gameSetup.GameCode;

}






// variables
var connection;
var gameSetup;

var intialOptions = document.getElementById('initialOptions');
var enterName = document.getElementById('enterName'); // TODO Add validation for name
var newRoomBtn = document.getElementById('createRoomBtn');
var joinRoomBtn = document.getElementById('joinRoomBtn')

var allowAudienceModal = document.getElementById('allowAudienceModal');
var yesAudienceBtn = document.getElementById('yesAudienceBtn');
var noAudienceBtn = document.getElementById('noAudienceBtn');

var waitRoom = document.getElementById('waitRoom');
var roomCodeDisplay = document.getElementById('roomCode');

// event handlers
newRoomBtn.addEventListener("click", askAboutAudience);
yesAudienceBtn.addEventListener("click", function () { newRoom(true); })
noAudienceBtn.addEventListener("click", function () { newRoom(false); })