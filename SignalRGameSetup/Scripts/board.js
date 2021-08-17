// square functions

// get the position of a square based on a string
function getCoordinates(positionString) {
    let row;
    let col;

    // determine if first number is 10 or not to decide what to extract
    if (positionString.substring(1, 2) === "0") {
        row = parseInt(positionString.substring(0, 2));
        col = parseInt(positionString.substring(3, positionString.length));
    } else {
        row = parseInt(positionString.substring(0, 1));
        col = parseInt(positionString.substring(2, positionString.length));
    }

    let coordinates = { row, col };
    return coordinates;
}

function addSquares(boardId) {

    console.log('addSquares function reached');

    let board = document.getElementById(boardId);

    // test adding one row
    let squareLength = 30
    let insideSquareLength = 30;
    let xPos = 5; // starting spot
    let yPos = 5;

    // count rows
    for (let r = 0; r <= 10; r++) {

        // count columns
        for (let c = 0; c <= 10; c++) {
            console.log('adding');
            let square = document.createElement('div');

            // position the square
            square.style.position = 'absolute'; // ??
            square.style.display = 'flex';
            square.style.justifyContent = 'center';
            square.style.alignItems = 'center';
            square.style.left = xPos + 'px';
            square.style.top = yPos + 'px';

            // modify position
            xPos += squareLength + 5;

            // if it's the first space, make it blank)
            if (r === 0 & c === 0) {
                square.className = 'square empty';
            } else if (r === 0 || c === 0) {
                square.className = 'square info';
            } else {
                // if it's a regular square, also append a child square and a peg hole
                square.className = 'square';
                square.id = 'square' + r + '-' + c;
                // HACK these need to be set according to the game info passed in
                square.setAttribute('hasShip', false);
                square.setAttribute('hasPeg', false);
                square.setAttribute('hasPreview', false);
                square.setAttribute('position', r + '-' + c);
                square.addEventListener('click',
                    function () { clickSquare(square.getAttribute('position')); });

                // TODO consider adding a highlight to the inside square
                let pegHole = document.createElement('div');
                pegHole.className = 'peg-hole';
                pegHole.id = 'pegHole' + r + '-' + c;

                // Add a peg hole too
                square.appendChild(pegHole);

                // TEST - add preview square
                let shipPreview = document.createElement('div');
                shipPreview.className = 'ship-preview hide';
                shipPreview.id = 'preview' + r + '-' + c;
                square.appendChild(shipPreview);

            }

            // append it to the container
            board.appendChild(square);

        }

        // change position for next row
        yPos += squareLength + 5;

        // set the x position back to it's start
        xPos = 5;

    }

    // Square clicking functions
    function clickSquare(positionString) {
        console.log('square clicked');
        if (isSettingShips && shipToSet != null) {
            setShipPosition(positionString);
        }
        // do other stuff if not setting ships
    }

    // setting ship functions
    function setShipPosition(positionString) {
        console.log('setting position');
        let square = document.getElementById('square' + positionString);
        let shipPreview = document.getElementById('preview' + positionString);
        let positionCoordinates = getCoordinates(positionString);
        console.log('Square id: ' + square.id);
        console.log('has ship: ' + square.getAttribute('hasShip'));
        console.log('has peg: ' + square.getAttribute('hasPeg'));

        // first make sure it doesn't have a ship or peg
        if (square.getAttribute('hasShip') == 'false' &&
            square.getAttribute('hasPeg') == 'false') {

            console.log('no ship or peg on square');

            // check if it has a preview already - if it does and it's the first
            // square, then unset it
            if (square.getAttribute('hasPreview') == 'true' &&
                firstPosition.length != 0 && firstPosition[0] === positionCoordinates[0] &&
                firstPosition[1] === positionCoordinates[1]) {

                console.log('has preview already and is first square - unsetting');
                shipPreview.className = 'ship-preview hide';
                firstPosition.length = 0; // like setting them null
                previousPosition.length = 0; // like setting it to null
                square.setAttribute('hasPreview', false);

            } else if (firstPosition.length === 0) { // otherwise if a position is not set yet
                console.log('setting preview');
                firstPosition = positionCoordinates;
                previousPosition = positionCoordinates;
                shipPreview.className = 'ship-preview';
                square.setAttribute('hasPreview', true);

            } else { // otherwise if it's the last position that can be set - set the ship!

            }


        }

    }

    // TODO write method that shows a preview on adjacent squares when being dragged over them - or hides them - if first position is set


}

// variables
var VERTICAL = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"];
var HORIZONTAL = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
var BOARD_SIZE = 10;

// TEMPORARY
class TestShipClass {
    constructor(Name, Length) {
        this.Name = Name;
        this.Length = Length;
        this.Direction = null;
        this.Positions = null;
        this.IsSet = false;
        this.IsSunk = false;
    }
}

class TestPositionClass {
    constructor(XPos, YPos, Image) {
        this.XPos = XPos;
        this.YPos = YPos;
        this.Image = Image;
        this.IsHit = false;
    }
}

var testShip = new TestShipClass('Carrier', 5);

// ship setting variables
// HACK First two variables are set temporarily to create functions
var isSettingShips = true; // default = false
var shipToSet = testShip; // this should be null unless ships are being set
var firstPosition = []; // null indicates nothing is clicked
var previousPosition = []; // preview position that mouse dragged over
var lastPosition = []; // should stay null until last position is clicked
var setDirection = null;




// event listeners
document.body.addEventListener('load', function () { addSquares(playerBoard) }); 

document.body.onload = addSquares('playerBoard');