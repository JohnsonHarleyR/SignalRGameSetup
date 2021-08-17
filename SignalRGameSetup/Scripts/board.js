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

    let coordinates = [row, col];
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
                square.addEventListener('mouseover',
                    function () { mouseOverSquare(square.getAttribute('position')); });

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

    // Square clicking/mouseover functions
    function clickSquare(positionString) {
        console.log('square clicked');
        if (isSettingShips && shipToSet != null) {
            setShipPosition(positionString);
        }
        // do other stuff if not setting ships
    }

    function mouseOverSquare(positionString) {
        console.log('mouse over');
        let square = document.getElementById('square' + positionString);
        let shipPreview = document.getElementById('preview' + positionString);
        let positionCoordinates = getCoordinates(positionString);
        // first make sure ships are being set and there's already a first position in the preview
        if (isSettingShips && firstPosition.length != 0 && previewPositions.length != 0 &&
            previousPosition.length != 0) {

            // if it's in the line of square previews and has a preview, unset everything after it
            if (isInPreviews(positionCoordinates) && square.getAttribute('hasPreview') == 'true') {
                // figure out position of that coordinate in the array
                let squareIndex = getPreviewIndex(positionCoordinates);
                // loop and unset anything after that index in the array
                previousPosition = positionCoordinates;
                console.log('removing extra preview');
                for (let i = squareIndex + 1; i < previewPositions.length; i++) {
                    let preview = document.getElementById('preview' + previewPositions[i][0] +
                        '-' + previewPositions[i][1]);
                    preview.setAttribute('hasPreview', false);
                    previewPositions.splice(i, 1);
/*                    previewPositions = previewPositions[i].splice(i, 1);*/
                    preview.className = 'ship-preview hide';
                    squareCount--;
                }

                // if previous positions length is now only equal to one, set the direction to null
                if (previewPositions.length == 1) {
                    previewDirection = null;
                }

                //// now if the square in question is the same as the first square,
                //// unset the direction
                //if (positionCoordinates[0] === firstPosition[0] &&
                //    positionCoordinates[1] === firstPosition[1]) {
                //    previewDirection = null;
                //}

                // now check that this square is next to the previous square
            } else if (isNextTo(positionCoordinates, previousPosition)) {

                // check if it's the second position (check the count) - if it is, set the direction
                if (squareCount === 1) {
                    previewDirection = getDirection(positionCoordinates, previousPosition);
                }

                // check that it's going in the correct direction
                if (previewDirection === getDirection(positionCoordinates, previousPosition)) {
                    // if it is, then set the preview to visible and add to count - determine color
                    square.setAttribute('hasPreview', true);
                    previewPositions.push(positionCoordinates);
                    squareCount++;

                    // see if it's the final square
                    if (isFinalPreviewSquare(positionCoordinates)) {
                        shipPreview.className = 'ship-preview last';
                    } else if (isBeyondFinalPreviewSquare(positionCoordinates)) { // if it's beyond the final square, show an error
                        shipPreview.className = 'ship-preview error';
                    } else { // otherwise set it to preview
                        shipPreview.className = 'ship-preview';
                    }

                }

            }


        }
        // check if it has

    }

    // check if it should be the last position in a direction
    function isFinalPreviewSquare(coordinates) {
        console.log('Is it the last preview square?');
        let result = false;
        // figure out the final preview square - if fields are not blank
        if (isSettingShips && firstPosition.length != 0 &&
            previewDirection != null && currentShip != null) {
            let position = getFinalPreviewSquare();
            if (position[0] === coordinates[0] &&
                position[1] === coordinates[1]) {
                result = true;
            }
        }

        // return result
        console.log(result);
        return result;
    }

    function isBeyondFinalPreviewSquare(coordinates) {
        console.log('seeing if its beyound the final preview square');
        let result = false;
        // figure out the final preview square - if fields are not blank
        if (isSettingShips && firstPosition.length != 0 &&
            previewDirection != null && currentShip != null) {
            let finalPosition = getFinalPreviewSquare();
            // determine final based on ship length and direction
            if (previewDirection === "north") {
                if (coordinates[0] > finalPosition[0] &&
                    coordinates[1] === finalPosition[1]) {
                    result = true;
                }
            } else if (previewDirection === "south") {
                if (coordinates[0] < finalPosition[0] &&
                    coordinates[1] === finalPosition[1]) {
                    result = true;
                }
            } else if (previewDirection === "west") {
                if (coordinates[0] === finalPosition[0] &&
                    coordinates[1] > finalPosition[1]) {
                    result = true;
                }
            } else if (previewDirection === "east") {
                if (coordinates[0] === finalPosition[0] &&
                    coordinates[1] < finalPosition[1]) {
                    result = true;
                }
            }
        }
        console.log(result);
        return result;
    }

    function getFinalPreviewSquare() {
        console.log('settng final preview square');
        let position = new Array();
        let lengthToAccount = currentShip.Length - 1;
        // figure out the final preview square - if fields are not blank
        if (isSettingShips && firstPosition.length != 0 &&
            previewDirection != null && currentShip != null) {
            // determine final based on ship length and direction

            if (previewDirection === "north") {
                position = [firstPosition[0] + lengthToAccount, firstPosition[1]];
            } else if (previewDirection === "south") {
                position = [firstPosition[0] - lengthToAccount, firstPosition[1]];
            } else if (previewDirection === "west") {
                position = [firstPosition[0], firstPosition[1] + lengthToAccount];
            } else if (previewDirection === "east") {
                position = [firstPosition[0], firstPosition[1] - lengthToAccount];
            }
        }
        console.log(position);
        return position;
    }

    // check if a position is in a row of highlighted cells
    function isInPreviews(coordinates) {
        console.log('checking if it exists in previews');
        if (previewPositions.length === 0) {
            console.log('false - no positions set');
            return false;
        }
        for (var i = previewPositions.length - 1; i >= 0; i--)
        {
            if (coordinates[0] === previewPositions[i][0] &&
                coordinates[1] === previewPositions[i][1]) {
                console.log('true');
                return true;
            }
        }
        console.log('false');
        return false;
    }

    // get the index of a position in the list of preview cells by a coordinate
    function getPreviewIndex(coordinates) {
        console.log('getting preview index');
        if (previewPositions.length === 0) {
            console.log('error');
            return -1; // -1 indicates error
        }
        for (var i = previewPositions.length - 1; i >= 0; i--) {
            if (coordinates[0] === previewPositions[i][0] &&
                coordinates[1] === previewPositions[i][1]) {
                console.log('index: ' + i);
                return i;
            }
        }
        console.log('error');
        return -1;
    }

    // see if a square is next to another
    function isNextTo(coordinates, previous) {
        console.log('Is next to previous?');
        console.log('coordinates: ' + coordinates[0] + ', ' + coordinates[1]);
        console.log('previous: ' + previous);
        let north = [previous[0] - 1, previous[1]];
        let south = [previous[0] + 1, previous[1]]
        let west = [previous[0], previous[1] - 1];
        let east = [previous[0], previous[1] + 1];
        console.log('North: ' + north);
        console.log('South: ' + south);
        console.log('West: ' + west);
        console.log('East: ' + east);

        if ((coordinates[0] === north[0] && coordinates[1] === north[1]) ||
            (coordinates[0] === south[0] && coordinates[1] === south[1]) || 
            (coordinates[0] === west[0] && coordinates[1] === west[1]) || 
            (coordinates[0] === east[0] && coordinates[1] === east[1])) {
            console.log('true');
            return true;
        } else {
            console.log('false');
            return false;
        }

    }

    // determine which direction a line of squares is going
    function getDirection(coordinates, previous) {
        console.log('What direction?');
        let north = [(previous[0] + 1), (previous[1])];
        let south = [previous[0] - 1, previous[1]]
        let west = [previous[0], previous[1] + 1];
        let east = [previous[0], previous[1] - 1];
        if (coordinates[0] === north[0] && coordinates[1] === north[1]) {
            console.log('north');
            return 'north';
        } else if (coordinates[0] === south[0] && coordinates[1] === south[1]) {
            console.log('south');
            return 'south';
        } else if (coordinates[0] === west[0] && coordinates[1] === west[1]) {
            console.log('west');
            return 'west';
        } else if (coordinates[0] === east[0] && coordinates[1] === east[1]) {
            console.log('east');
            return 'east';
        } else {
            return null;
        }
    }

    // setting ship functions
    function setShipPosition(positionString) {
        console.log('setting position');
        let square = document.getElementById('square' + positionString);
        let shipPreview = document.getElementById('preview' + positionString);
        let positionCoordinates = getCoordinates(positionString);
        //console.log('Square id: ' + square.id);
        //console.log('has ship: ' + square.getAttribute('hasShip'));
        //console.log('has peg: ' + square.getAttribute('hasPeg'));

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
                previewPositions.length = 0;
                square.setAttribute('hasPreview', false);
                squareCount = 0;

            } else if (firstPosition.length === 0) { // otherwise if a position is not set yet
                console.log('setting preview');
                firstPosition = positionCoordinates;
                previousPosition = positionCoordinates;
                previewPositions.push(positionCoordinates);
                shipPreview.className = 'ship-preview';
                square.setAttribute('hasPreview', true);
                squareCount = 1;

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
var squareCount = 0; // how many squares have been dragged over
var firstPosition = new Array(); // null indicates nothing is clicked
var previousPosition = new Array(); // preview position that mouse dragged over
var finalPosition = new Array(); // should stay null until last position is clicked
var previewPositions = new Array(); // all positions in the current stream of previews lol
var previewDirection = null;
var currentShip = testShip; // HACK for now it is set to the test ship




// event listeners
document.body.addEventListener('load', function () { addSquares(playerBoard) }); 

document.body.onload = addSquares('playerBoard');