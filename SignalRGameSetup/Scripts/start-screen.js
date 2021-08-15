// Hub

// Go to the game page
function goToGame() {
    connection.server.goToGame({
        gameCode: gameSetup.GameCode,
        participantId: clientId
    });
}


function updateEnterGameButton() {
    // only let them use or see this button if they started the game
    if (isStarter) {
        // only let it be available if there are enough players for the game
        if (gameSetup.Players.length < minimumPlayers) {

            startGameBtn.disabled = true;
            let howManyLeft = minimumPlayers - gameSetup.Players.length;
            if (howManyLeft === 1) {
                startGameBtn.innerHTML = 'Only ' + howManyLeft + ' player needed to start';
            } else {
                startGameBtn.innerHTML = 'only ' + howManyLeft + ' players needed to start';
            }
            
        } else {
            startGameBtn.disabled = false;
            startGameBtn.innerHTML = 'Start Game';
        }

    } else {
        startGameBtn.style.display = 'none';
    }

}

// Creating a new room
function newRoom(allowAudienceChoice) {
    console.log("New room.");
    clientName = enterName.value;
    console.log(clientName);

    // if someone is creating a room, they are automatically a player
    isStarter = true;

    // send to hub
    connection.server.newRoom({
        name: clientName,
        allowAudience: allowAudienceChoice
    });

}

// Fetch an existing wait room
function decideHowToEnter() {

    // make sure a game code has been entered - and is valid
    if (gameCodeInput.value != '') {
        checkValidGameCode(gameCodeInput.value);
    } else {
        decideHowToEnterProceed();
    }



}

// continuation of decideHowToEnter() that only gets called after important code is executed.
function decideHowToEnterProceed() {
    if (gameCodeInput.value === '') {
        alert('Please enter a room code.');
    } else if (!isValidGameCode) {
        alert('That code is not valid.');
    } else {
        console.log("Entering room.");
        clientName = enterName.value;
        console.log(clientName);

        // send to hub
        connection.server.existingRoom({
            name: clientName,
            gameCode: gameCodeInput.value
        });
    }
}

function joinAsPlayer() {
    console.log("joining as player");

    // send to hub
    connection.server.joinAsPlayer({
        name: clientName,
        setup: gameSetup
    });

}

function joinAsWatcher() {
    console.log("joining as watcher");

    // send to hub
    connection.server.joinAsWatcher({
        name: clientName,
        setup: gameSetup
    });

}


function askAboutAudience() {

    // first make sure a name has been entered
    if (enterName.value != '') {

        console.log("asking...");
        allowAudienceModal.style.display = "block";
    } else {
        alert('Please enter a name.');
    }

}

function askForGameCode() {

    // first make sure a name has been entered
    if (enterName.value != '') {

        console.log("asking for code...");
        gameCodeModal.style.display = "block";
        initialGameCodeModalDisplay.style.display = "block";
        joinOptionsDisplay.style.display = "none";

    } else {
        alert('Please enter a name.');
    }


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

// check the hub to make sure the game code exists
function checkValidGameCode(code) {
    console.log('checking if game code is valid');
    connection.server.isValidCode(code);
}

// return method for creating a room
function enterWaitRoom() {
    console.log("Entering wait room.");
    // hide the initial div and show the wait room
    intialOptions.style.display = "none";
    gameCodeModal.style.display = "none";
    waitRoom.style.display = "block";
    document.getElementById('chatArea').style.display = "block";
    roomCodeDisplay.innerHTML = gameSetup.GameCode;

    // update wait room
    updateWaitRoom();
}

// Update methods

// update all the participants in a wait room - and anything else necessary
function updateWaitRoom() {
    updateEnterGameButton();
    updateWaitRoomPlayers();
    updateWaitRoomWatchers();
}

// update wait room player list
function updateWaitRoomPlayers() {
    console.log('updating wait room players');
    playersAvailable.innerHTML = gameSetup.PlayersAvailableToJoin;
    waitRoomPlayers.innerHTML = "";
    // loop through list of players in the setup
    for (let i = 0; i < gameSetup.Players.length; i++) {
        waitRoomPlayers.innerHTML += gameSetup.Players[i].Name + "</br>";
    }
}

// update wait room watcher list
function updateWaitRoomWatchers() {
    console.log('updating wait room watchers');
    watchersAvailable.innerHTML = gameSetup.WatchersAvailableToJoin;
    waitRoomWatchers.innerHTML = "";
    // loop through list of players in the setup
    for (let i = 0; i < gameSetup.Watchers.length; i++) {
        waitRoomWatchers.innerHTML += gameSetup.Watchers[i].Name + "</br>";
    }
}


// variables
var connection;
/*var gameSetup;*/
/*var clientName;*/
var isValidGameCode = false;

var intialOptions = document.getElementById('initialOptions');
var enterName = document.getElementById('enterName'); // TODO Add validation for name
var newRoomBtn = document.getElementById('createRoomBtn');
var joinRoomBtn = document.getElementById('joinRoomBtn')

var allowAudienceModal = document.getElementById('allowAudienceModal');
var yesAudienceBtn = document.getElementById('yesAudienceBtn');
var noAudienceBtn = document.getElementById('noAudienceBtn');
var audienceModalClose = document.getElementById('audienceModalClose');

var gameCodeModal = document.getElementById('gameCodeModal');
var initialGameCodeModalDisplay = document.getElementById('initialGameCodeModalDisplay');
var gameCodeInput = document.getElementById('gameCodeInput');
var submitGameCodeBtn = document.getElementById('submitGameCodeBtn');
var codeModalClose = document.getElementById('codeModalClose');

var joinOptionsDisplay = document.getElementById('joinOptionsDisplay');
var joinAsPlayerBtn = document.getElementById('joinAsPlayerBtn');
var joinAsWatcherBtn = document.getElementById('joinAsWatcherBtn');
var noSpaceAvailable = document.getElementById('noSpaceAvailable');

var waitRoom = document.getElementById('waitRoom');
var roomCodeDisplay = document.getElementById('roomCode');
var waitRoomPlayers = document.getElementById('waitRoomPlayers');
var waitRoomWatchers = document.getElementById('waitRoomWatchers');
var playersAvailable = document.getElementById('playersAvailable');
var watchersAvailable = document.getElementById('watchersAvailable');

var startGameBtn = document.getElementById('startGameBtn');

// event handlers
newRoomBtn.addEventListener("click", askAboutAudience);
yesAudienceBtn.addEventListener("click", function () { newRoom(true); })
noAudienceBtn.addEventListener("click", function () { newRoom(false); })

joinRoomBtn.addEventListener("click", askForGameCode);
submitGameCodeBtn.addEventListener("click", function () { decideHowToEnter(); })

joinAsPlayerBtn.addEventListener("click", joinAsPlayer);
joinAsWatcherBtn.addEventListener("click", joinAsWatcher);

audienceModalClose.addEventListener("click", function () { allowAudienceModal.style.display = "none"; })
codeModalClose.addEventListener("click", function () { gameCodeModal.style.display = "none"; })

startGameBtn.addEventListener("click", goToGame)