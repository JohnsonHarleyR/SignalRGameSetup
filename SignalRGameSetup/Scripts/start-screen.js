// Hub

// Creating a new room
function newRoom(allowAudienceChoice) {
    console.log("New room.");
    clientName = enterName.value;
    console.log(clientName);

    // send to hub
    connection.server.newRoom({
        name: clientName,
        allowAudience: allowAudienceChoice
    });

    //// callback method
    //connection.client.enterNewRoom = function (setup) {
    //    gameSetup = setup;
    //    enterWaitRoom();
    //};
}

// Fetch an existing wait room
function existingRoom() {
    console.log("Entering room.");
    clientName = enterName.value;
    console.log(clientName);

    // send to hub
    connection.server.existingRoom({
        name: clientName,
        gameCode: gameCodeInput.value
    });

//    // callback method
//    connection.client.enterExistingRoom = function (setup) {
//        gameSetup = setup;
//        getJoinRoomOptions();
///*        enterWaitRoom();*/
//    };
}

function joinAsPlayer() {
    console.log("joining as player");

    // send to hub
    connection.server.joinAsPlayer({
        name: clientName,
        setup: gameSetup
    });

    //// callback method
    //connection.client.joinRoom = function (setup) {
    //    gameSetup = setup;
    //    enterWaitRoom();
    //};

}


function askAboutAudience() {
    console.log("asking...");
    allowAudienceModal.style.display = "block";
}

function askForGameCode() {
    console.log("asking for code...");
    gameCodeModal.style.display = "block";
    initialGameCodeModalDisplay.style.display = "block";
    joinOptionsDisplay.style.display = "none";
}

function getJoinRoomOptions() {
    console.log("getting join options...");
    initialGameCodeModalDisplay.style.display = "none";
    joinOptionsDisplay.style.display = "block";
    noSpaceAvailable.style.display = "block";

    // determine which buttons to show
    if (gameSetup.PlayersAvailableToJoin > 0) {
        noSpaceAvailable.style.display = "none";
        joinAsPlayerBtn.style.display = "block";
    } else {
        joinAsPlayerBtn.style.display = "none";
    }

    if (gameSetup.WatchersAvailableToJoin > 0) {
        noSpaceAvailable.style.display = "none";
        joinAsWatcherBtn.style.display = "block";
    } else {
        joinAsWatcherBtn.style.display = "none";
    }
}

// return method for creating a room
function enterWaitRoom() {
    console.log("Entering wait room.");
    // hide the initial div and show the wait room
    intialOptions.style.display = "none";
    gameCodeModal.style.display = "none";
    waitRoom.style.display = "block";
    roomCodeDisplay.innerHTML = gameSetup.GameCode;
    // update wait room
    updateWaitRoom();
}

// Update methods

// update all the participants in a wait room - and anything else necessary
function updateWaitRoom() {
    updateWaitRoomPlayers();
    updateWaitRoomWatchers();
}

// update wait room player list
function updateWaitRoomPlayers() {
    console.log('updating wait room players');
    waitRoomPlayers.innerHTML = "<h3>Players</h3>";
    // loop through list of players in the setup
    for (let i = 0; i < gameSetup.Players.length; i++) {
        waitRoomPlayers.innerHTML += gameSetup.Players[i].Name + "</br>";
    }
}

// update wait room watcher list
function updateWaitRoomWatchers() {
    console.log('updating wait room watchers');
    waitRoomWatchers.innerHTML = "<h3>Watchers</h3>";
    // loop through list of players in the setup
    for (let i = 0; i < gameSetup.Watchers.length; i++) {
        waitRoomWatchers.innerHTML += gameSetup.Watchers[i].Name + "</br>";
    }
}






// variables
var connection;
var gameSetup;
var clientName;

var intialOptions = document.getElementById('initialOptions');
var enterName = document.getElementById('enterName'); // TODO Add validation for name
var newRoomBtn = document.getElementById('createRoomBtn');
var joinRoomBtn = document.getElementById('joinRoomBtn')

var allowAudienceModal = document.getElementById('allowAudienceModal');
var yesAudienceBtn = document.getElementById('yesAudienceBtn');
var noAudienceBtn = document.getElementById('noAudienceBtn');

var gameCodeModal = document.getElementById('gameCodeModal');
var initialGameCodeModalDisplay = document.getElementById('initialGameCodeModalDisplay');
var gameCodeInput = document.getElementById('gameCodeInput');
var submitGameCodeBtn = document.getElementById('submitGameCodeBtn');

var joinOptionsDisplay = document.getElementById('joinOptionsDisplay');
var joinAsPlayerBtn = document.getElementById('joinAsPlayerBtn');
var joinAsWatcherBtn = document.getElementById('joinAsWatcherBtn');
var noSpaceAvailable = document.getElementById('noSpaceAvailable');

var waitRoom = document.getElementById('waitRoom');
var roomCodeDisplay = document.getElementById('roomCode');
var waitRoomPlayers = document.getElementById('waitRoomPlayers');
var waitRoomWatchers = document.getElementById('waitRoomWatchers');

// event handlers
newRoomBtn.addEventListener("click", askAboutAudience);
yesAudienceBtn.addEventListener("click", function () { newRoom(true); })
noAudienceBtn.addEventListener("click", function () { newRoom(false); })

joinRoomBtn.addEventListener("click", askForGameCode);
submitGameCodeBtn.addEventListener("click", function () { existingRoom(); })

joinAsPlayerBtn.addEventListener("click", joinAsPlayer);
joinAsWatvherBtn.addEventListener("click", joinAsWatcher);