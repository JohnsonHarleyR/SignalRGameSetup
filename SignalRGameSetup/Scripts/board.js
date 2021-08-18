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
            let square = document.createElement('div');

            // position the square
            square.style.position = 'absolute'; // ??
            square.style.display = 'flex';
            square.style.justifyContent = 'center';
            square.style.alignItems = 'center';
            square.style.left = xPos + 'px';
            square.style.top = yPos + 'px';

            // give the square an id
            square.id = 'square' + r + '-' + c;

            // modify position
            xPos += squareLength + 5;

            // if it's the first space, make it blank)
            if (r === 0 & c === 0) {
                square.className = 'square empty';
            } else if (r === 0 || c === 0) {
                square.className = 'square info';
                // display info depending on square
                if (r === 0 && c != 0) {
                    // the top row displays number, so put a number in the square
                    square.innerText = c;
                } else if (r != 0 && c === 0) {
                    // the first column displays letters - determine which letter
                    let letter = VERTICAL_INFO[r - 1];
                    square.innerText = letter;
                }

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

                // add ship preview square
                let shipPreview = document.createElement('div');
                shipPreview.className = 'ship-preview hide';
                shipPreview.id = 'preview' + r + '-' + c;
                square.appendChild(shipPreview);

                // add ship image square
                let shipImage = document.createElement('img');
                shipImage.id = 'ship' + r + '-' + c;
                shipImage.className = 'ship hide';
                square.appendChild(shipImage);

            }

            // append it to the container
            board.appendChild(square);

        }

        // change position for next row
        yPos += squareLength + 5;

        // set the x position back to it's start
        xPos = 5;

    }
    // TODO fix preview square placement so that if you don't move your mouse directly in a row, it doesn't leave squares and do weird thing - make it so if it's in the same row, it works

    // Square clicking/mouseover functions
    function clickSquare(positionString) {
        if (isSettingShips && shipToSet != null) {
            setShipPosition(positionString);
        }
        // do other stuff if not setting ships
    }

    function mouseOverSquare(positionString) {
        let square = document.getElementById('square' + positionString);
        let shipPreview = document.getElementById('preview' + positionString);
        let positionCoordinates = getCoordinates(positionString);
        // first make sure ships are being set and there's already a first position in the preview
        if (isSettingShips && firstPosition.length != 0 && previewPositions.length != 0 &&
            previousPosition.length != 0) {

            // if it's in the line of square previews and has a preview, unset everything after it
            if (isInPreviews(positionCoordinates) && square.getAttribute('hasPreview') == 'true') {
                consoleLog("isInPreviews(positionCoordinates) && square.getAttribute('hasPreview') == 'true'");
                // figure out position of that coordinate in the array
                let squareIndex = getPreviewIndex(positionCoordinates);
                // loop and unset anything after that index in the array
                previousPosition = positionCoordinates;
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
                consoleLog("isNextTo(positionCoordinates, previousPosition)");

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

            } else if (isInRow(positionCoordinates)) {
                consoleLog('********************************************');
                consoleLog("isInRow(positionCoordinates) WAS SUCCESSFUL");
                consoleLog('********************************************');
            }


        }
        // check if it has

    }

    // check if it should be the last position in a direction
    function isFinalPreviewSquare(coordinates) {
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
        return result;
    }

    function isBeyondFinalPreviewSquare(coordinates) {
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
        return result;
    }

    function getFinalPreviewSquare() {
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
        return position;
    }

    // check if a position is in a row of highlighted cells
    function isInPreviews(coordinates) {
        if (previewPositions.length === 0) {
            return false;
        }
        for (var i = previewPositions.length - 1; i >= 0; i--)
        {
            if (coordinates[0] === previewPositions[i][0] &&
                coordinates[1] === previewPositions[i][1]) {
                return true;
            }
        }
        return false;
    }

    // get the index of a position in the list of preview cells by a coordinate
    function getPreviewIndex(coordinates) {
        if (previewPositions.length === 0) {
            return -1; // -1 indicates error
        }
        for (var i = previewPositions.length - 1; i >= 0; i--) {
            if (coordinates[0] === previewPositions[i][0] &&
                coordinates[1] === previewPositions[i][1]) {
                return i;
            }
        }
        return -1;
    }

    // see if a square is next to another
    function isNextTo(coordinates, previous) {
        let north = [previous[0] - 1, previous[1]];
        let south = [previous[0] + 1, previous[1]]
        let west = [previous[0], previous[1] - 1];
        let east = [previous[0], previous[1] + 1];

        if ((coordinates[0] === north[0] && coordinates[1] === north[1]) ||
            (coordinates[0] === south[0] && coordinates[1] === south[1]) || 
            (coordinates[0] === west[0] && coordinates[1] === west[1]) || 
            (coordinates[0] === east[0] && coordinates[1] === east[1])) {
            return true;
        } else {
            return false;
        }

    }


    // determine if a square moused over is in the same row as the original square - depending if direction has been set or not
    function isInRow(coordinates) {
        consoleLog("--------------------------------------------");
        consoleLog("isInRow(coordinates)");
        consoleLog('Coordinates: ' + coordinates[0] + ', ' + coordinates[1])
        let result = false; // it will stay as false if nothing is found
        if (firstPosition.length != 0) {
            consoleLog("(firstPosition.length != 0)");
            // determine north south east and west row - it doesn't matter if it has out of range indexes
            let direction = null;
            let north = new Array();
            let west = new Array();
            let south = new Array();
            let east = new Array();

            // find values for arrays using addition
            for (let i = 1; i <= BOARD_SIZE; i++) {
                let northPosition = [firstPosition[0] + i, firstPosition[1]];
                let westPosition = [firstPosition[0], firstPosition[1] + i];
                let southPosition = [firstPosition[0] - i, firstPosition[1]]
                let eastPosition = [firstPosition[0], firstPosition[1] - i];
                north.push(northPosition);
                west.push(westPosition);
                south.push(southPosition);
                east.push(eastPosition);
            }

            // see if a preview direction has been set - if it has only check for that. If it hasn't then any of the rows can contain that position
            if (previewDirection != null) {
                consoleLog("Ln: 343 (previewDirection != null)")
                // figure out which row is set
                if (previewDirection === 'north') {
                    result = checkArrayForPosition(north, coordinates);
                    consoleLog('North?: ' + result);
                } else if (previewDirection === 'west') {
                    result = checkArrayForPosition(west, coordinates);
                    consoleLog('West?: ' + result);
                } else if (previewDirection === 'south') {
                    result = checkArrayForPosition(south, coordinates);
                    consoleLog('South?: ' + result);
                } else if (previewDirection === 'east') {
                    result = checkArrayForPosition(east, coordinates);
                    consoleLog('East?: ' + result);
                }

                // return result
                return result;
            } else {
                consoleLog("Ln: 363 No previewDirection")
                result = checkArrayForPosition(north, coordinates);
                consoleLog('North?: ' + result);
                if (result === true) {
                    consoleLog('END OF isInRow() METHOD - RESULT: ' + result);
                    return result;
                } else {
                    result = checkArrayForPosition(west, coordinates);
                    consoleLog('West?: ' + result);
                }
                if (result === true) {
                    consoleLog('END OF isInRow() METHOD - RESULT: ' + result);
                    return result;
                } else {
                    result = checkArrayForPosition(south, coordinates);
                    consoleLog('South?: ' + result);
                }
                if (result === true) {
                    consoleLog('END OF isInRow() METHOD - RESULT: ' + result);
                    return result;
                } else {
                    result = checkArrayForPosition(east, coordinates);
                    consoleLog('East?: ' + result);
                }
                if (result === true) {
                    consoleLog('END OF isInRow() METHOD - RESULT: ' + result);
                    return result;
                }

            }

        } else {
            consoleLog('Ln: 388 No firstPosition - result = ' + result);
        // you can't determine the position if a there is no firstPosition, because that means a preview row has not been started
        }
        consoleLog('END OF isInRow() METHOD - RESULT: ' + result);
        return result;

    }








    // see if a coordinate is listed in an array of coordinates
    function checkArrayForPosition(array, coordinates) {
        for (let i = 0; i < array.length; i++) {
            if (array[i][0] === coordinates[0] && array[i][1] === coordinates[1]) {
                return true;
            }
        }
        return false;
    }

    // determine which direction a line of squares is going
    function getDirection(coordinates, previous) {
        let north = [previous[0] + 1, previous[1]];
        let south = [previous[0] - 1, previous[1]];
        let west = [previous[0], previous[1] + 1];
        let east = [previous[0], previous[1] - 1];
        if (coordinates[0] === north[0] && coordinates[1] === north[1]) {
            return 'north';
        } else if (coordinates[0] === south[0] && coordinates[1] === south[1]) {
            return 'south';
        } else if (coordinates[0] === west[0] && coordinates[1] === west[1]) {
            return 'west';
        } else if (coordinates[0] === east[0] && coordinates[1] === east[1]) {
            return 'east';
        } else {
            return null;
        }
    }

    // determine which direction a line of squares is going - with an index reference
    function getDirectionUsingIndex(coordinates, previous, index) {
        let north = [(previous[0] + index), (previous[1])];
        let south = [previous[0] - index, previous[1]];
        let west = [previous[0], previous[1] + index];
        let east = [previous[0], previous[1] - index];
        if (coordinates[0] === north[0] && coordinates[1] === north[1]) {
            return 'north';
        } else if (coordinates[0] === south[0] && coordinates[1] === south[1]) {
            return 'south';
        } else if (coordinates[0] === west[0] && coordinates[1] === west[1]) {
            return 'west';
        } else if (coordinates[0] === east[0] && coordinates[1] === east[1]) {
            return 'east';
        } else {
            return null;
        }
    }

    // setting ship functions
    function setShipPosition(positionString) {
        let square = document.getElementById('square' + positionString);
        let shipPreview = document.getElementById('preview' + positionString);
        let positionCoordinates = getCoordinates(positionString);

        // first make sure it doesn't have a ship or peg
        if (square.getAttribute('hasShip') == 'false' &&
            square.getAttribute('hasPeg') == 'false') {

            // check if it has a preview already - if it does and it's the first
            // square, then unset it
            if (square.getAttribute('hasPreview') == 'true' &&
                firstPosition.length != 0 && firstPosition[0] === positionCoordinates[0] &&
                firstPosition[1] === positionCoordinates[1]) {

                shipPreview.className = 'ship-preview hide';
                firstPosition.length = 0; // like setting them null
                previousPosition.length = 0; // like setting it to null
                previewPositions.length = 0;
                square.setAttribute('hasPreview', false);
                squareCount = 0;

            } else if (firstPosition.length === 0) { // otherwise if a position is not set yet
                firstPosition = positionCoordinates;
                previousPosition = positionCoordinates;
                previewPositions.push(positionCoordinates);
                shipPreview.className = 'ship-preview';
                square.setAttribute('hasPreview', true);
                squareCount = 1;

            } else if (isFinalPreviewSquare(positionCoordinates)) { // otherwise if it's the last position that can be set - set the ship!

                // store info
                currentShip.Direction = previewDirection;
                currentShip.IsSet = true;

                // set all the ship positions
                for (let i = 0; i < previewPositions.length; i++) {
                    // TODO make sure this will translate correctly to hub
                    let imageSrc = '\\Images\\Ships\\' + currentShip.Name.toLowerCase() + '-' +
                        currentShip.Direction + '-' + (i + 1) + '.png';
                    let pos = new TestPositionClass(previewPositions[i][0], previewPositions[i][1], imageSrc);
                    currentShip.Positions
                        .push(pos);
                    // set image too
                    let ship = document.getElementById('ship' + previewPositions[i][0] + '-' +
                        previewPositions[i][1]);
                    ship.className = 'ship';
                    ship.src = imageSrc;
                    let preview = document.getElementById('preview' + previewPositions[i][0] + '-' +
                        previewPositions[i][1]);
                    preview.className = 'preview hide';
                    // set square properties
                    let square = document.getElementById('square' + previewPositions[i][0] + '-' +
                        previewPositions[i][1]);
                    square.setAttribute('hasShip', true);
                }

                // unset all the preview stuff
                squareCount = 0;
                firstPosition.length = 0;
                previousPosition.length = 0;
                finalPosition.length = 0;
                previewPositions.length = 0;
                previewDirection = null;


                // TODO send info to the game hub to update everyone


                // TODO cycle to next ship to place - check if it's done or not
                // HACK for now, just setting isSettingShips to false
                isSettingShips = false;
            }


        }

    }

    // TODO write method that shows a preview on adjacent squares when being dragged over them - or hides them - if first position is set


}


// TESTING METHODS

// this will log the text to the console only if the showTestLogs bool is true 
function consoleLog(text) {
    if (showTestLogs) {
        console.log(text);
    }
}


// variables

var showTestLogs = true; // Set true of false for whether to see the testing logs in the console

var VERTICAL_INFO = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"];
var BOARD_SIZE = 10;

// TEMPORARY
class TestShipClass {
    constructor(Name, Length) {
        this.Name = Name;
        this.Length = Length;
        this.Direction = null;
        this.Positions = new Array();
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