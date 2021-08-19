﻿// HUB FUNCTIONS
'use strict';

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

function saveBoard() {}

// square functions

// TODO if a preview is over a ship, set it to red

// get the position of a square based on a string
function getCoordinates(positionString) {
    var row = undefined;
    var col = undefined;

    // determine if first number is 10 or not to decide what to extract
    if (positionString.substring(1, 2) === "0") {
        row = parseInt(positionString.substring(0, 2));
        col = parseInt(positionString.substring(3, positionString.length));
    } else {
        row = parseInt(positionString.substring(0, 1));
        col = parseInt(positionString.substring(2, positionString.length));
    }

    var coordinates = [row, col];
    return coordinates;
}

function addSquares(boardId) {

    var board = document.getElementById(boardId);

    // test adding one row
    var squareLength = 30;
    var insideSquareLength = 30;
    var xPos = 5; // starting spot
    var yPos = 5;

    // count rows
    for (var r = 0; r <= 10; r++) {
        var _loop = function (c) {
            var square = document.createElement('div');

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
                    var letter = VERTICAL_INFO[r - 1];
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
                square.addEventListener('click', function () {
                    clickSquare(square.getAttribute('position'));
                });
                square.addEventListener('mouseover', function () {
                    mouseOverSquare(square.getAttribute('position'));
                });

                // TODO consider adding a highlight to the inside square
                var pegHole = document.createElement('div');
                pegHole.className = 'peg-hole';
                pegHole.id = 'pegHole' + r + '-' + c;

                // Add a peg hole too
                square.appendChild(pegHole);

                // add ship preview square
                var shipPreview = document.createElement('div');
                shipPreview.className = 'ship-preview hide';
                shipPreview.id = 'preview' + r + '-' + c;
                square.appendChild(shipPreview);

                // add ship image square
                var shipImage = document.createElement('img');
                shipImage.id = 'ship' + r + '-' + c;
                shipImage.className = 'ship hide';
                square.appendChild(shipImage);
            }

            // append it to the container
            board.appendChild(square);
        };

        // count columns
        for (var c = 0; c <= 10; c++) {
            _loop(c);
        }

        // change position for next row
        yPos += squareLength + 5;

        // set the x position back to it's start
        xPos = 5;
    }
}

// TODO fix preview square placement so that if you don't move your mouse directly in a row, it doesn't leave squares and do weird thing - make it so if it's in the same row, it works

// Square clicking/mouseover functions
function clickSquare(positionString) {
    lastShipLog('First method: clickSquare');
    if (isSettingShips && currentShip != null) {
        setShip(positionString);
    }
    // do other stuff if not setting ships
}

//function setPreviewClass(coordinates) {
//    let positionString = coordinates[0] + '-' + coordinates[1];

//    let square = document.getElementById('square' + positionString);
//    let shipPreview = document.getElementById('preview' + positionString);

//    // see if it's the final square
//    if (square.getAttribute('hasShip') == 'true') {
//        shipPreview.className = 'ship-preview error';
//    } else if (isFinalPreviewSquare(coordinates)) {
//        shipPreview.className = 'ship-preview last';
//    } else if (isBeyondFinalPreviewSquare(coordinates)) { // if it's beyond the final square, show an error
//        shipPreview.className = 'ship-preview error';
//    } else { // otherwise set it to preview
//        shipPreview.className = 'ship-preview';
//    }
//}

function mouseOverSquare(positionString) {
    /*    lastShipLog('First method: mouseOverSquare');*/
    var square = document.getElementById('square' + positionString);
    var shipPreview = document.getElementById('preview' + positionString);
    var positionCoordinates = getCoordinates(positionString);
    // first make sure ships are being set and there's already a first position in the preview
    if (isSettingShips && firstPosition.length != 0 && previewPositions.length != 0 && previousPosition.length != 0) {

        // if it's in the line of square previews and has a preview, unset everything after it
        if (isInPreviews(positionCoordinates) && square.getAttribute('hasPreview') == 'true') {
            //previewLog("isInPreviews(positionCoordinates) && square.getAttribute('hasPreview') == 'true'");
            // figure out position of that coordinate in the array
            var squareIndex = getPreviewIndex(positionCoordinates);
            // loop and unset anything after that index in the array
            previousPosition = positionCoordinates;

            // copy previewPositions and use that to splice from - then set previewPositions to that array
            var arrayCopy = new Array();
            for (var i = 0; i < previewPositions.length; i++) {
                arrayCopy.push(previewPositions[i]);
            }

            for (var i = squareIndex + 1; i < previewPositions.length; i++) {
                var preview = document.getElementById('preview' + previewPositions[i][0] + '-' + previewPositions[i][1]);
                preview.setAttribute('hasPreview', false);
                arrayCopy.splice(i, 1);
                /*                    previewPositions = previewPositions[i].splice(i, 1);*/
                preview.className = 'ship-preview hide';
                squareCount--;
            }

            previewPositions = arrayCopy;

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
                //previewLog("isNextTo(positionCoordinates, previousPosition)");

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

                    var previousPreview = document.getElementById('preview' + previousPosition[0] + '-' + previousPosition[1]);

                    // see if it's the final square
                    if (square.getAttribute('hasShip') == 'true' || previousPreview.className === "ship-preview error") {
                        shipPreview.className = 'ship-preview error';
                    } else if (isFinalPreviewSquare(positionCoordinates)) {
                        shipPreview.className = 'ship-preview last';
                    } else if (isBeyondFinalPreviewSquare(positionCoordinates)) {
                        // if it's beyond the final square, show an error
                        shipPreview.className = 'ship-preview error';
                    } else {
                        // otherwise set it to preview
                        shipPreview.className = 'ship-preview';
                    }
                }
            } else if (isInRow(positionCoordinates)) {
                previewLog('********************************************');
                previewLog("isInRow(positionCoordinates) WAS SUCCESSFUL");
                previewLog('********************************************');

                // get the row and index info of the position
                var rowPositionInfo = getRowIndexAndDirection(positionCoordinates);
                previewLog('positionInfo:');
                previewLog('direction: ' + rowPositionInfo.direction);
                previewLog('index: ' + rowPositionInfo.index);
                previewLog('rowPositions: ' + rowPositionInfo.rowPositions);

                // set the preview direction if it hasn't been set
                if (previewDirection === null) {
                    //previewLog("(previewDirection === null)");
                    previewDirection = rowPositionInfo.direction;
                }

                // get the positions that are gonna have to be set to a preview
                var positions = getPositionsToChange(firstPosition, rowPositionInfo);
                //previewLog('Preview Positions: ' + previewPositions);
                //previewLog('Positions: ' + positions);
                if (previewPositions.length > 1) {
                    /*                    previewPositions = previewPositions.splice(2, previewPositions.length - 1);*/
                    squareCount = previewPositions.length;
                }
                squareCount = previewPositions.length;
                //previewLog('Preview Positions after splice: ' + previewPositions);

                // first go through preview positions for errors
                var hasError = false;
                //for (let i = 0; i < previewPositions.length; i++) {
                //    let p = document.getElementById('preview' + previewPositions[i][0] + '-' + previewPositions[i][1]);
                //    if (p.className === "ship-preview error") {
                //        hasError = true;
                //        break;
                //    }
                //}

                // loop through positions and set them to preview
                for (var i = 0; i < positions.length; i++) {
                    var newSquare = document.getElementById('square' + positions[i][0] + '-' + positions[i][1]);
                    var newShipPreview = document.getElementById('preview' + positions[i][0] + '-' + positions[i][1]);
                    // if it is, then set the preview to visible and add to count - determine color
                    newSquare.setAttribute('hasPreview', true);
                    var coords = [positions[i][0], positions[i][1]];
                    if (!isInPreviews(coords)) {
                        previewPositions.push(positions[i]);
                        squareCount++;
                    }

                    // check for error
                    if (!hasError && newShipPreview.className === "ship-preview error") {
                        hasError = true;
                    }

                    // see if it's the final square
                    if (newSquare.getAttribute('hasShip') == 'true' || hasError) {
                        newShipPreview.className = 'ship-preview error';
                        hasError = true;
                    } else if (isFinalPreviewSquare(positions[i])) {
                        newShipPreview.className = 'ship-preview last';
                    } else if (isBeyondFinalPreviewSquare(positions[i])) {
                        // if it's beyond the final square, show an error
                        newShipPreview.className = 'ship-preview error';
                    } else {
                        // otherwise set it to preview
                        newShipPreview.className = 'ship-preview';
                    }
                }
                previewLog('Preview Positions after adding positions: ' + previewPositions);
                previewLog('____________________________________________');
            }
    }
    // check if it has
}

// check if it should be the last position in a direction
function isFinalPreviewSquare(coordinates) {
    var result = false;
    // figure out the final preview square - if fields are not blank
    if (isSettingShips && firstPosition.length != 0 && previewDirection != null && currentShip != null) {
        var position = getFinalPreviewSquare();
        if (position[0] === coordinates[0] && position[1] === coordinates[1]) {
            result = true;
        }
    }

    // return result
    return result;
}

function isBeyondFinalPreviewSquare(coordinates) {
    var result = false;
    // figure out the final preview square - if fields are not blank
    if (isSettingShips && firstPosition.length != 0 && previewDirection != null && currentShip != null) {
        var _finalPosition = getFinalPreviewSquare();
        // determine final based on ship length and direction
        if (previewDirection === "north") {
            if (coordinates[0] > _finalPosition[0] && coordinates[1] === _finalPosition[1]) {
                result = true;
            }
        } else if (previewDirection === "south") {
            if (coordinates[0] < _finalPosition[0] && coordinates[1] === _finalPosition[1]) {
                result = true;
            }
        } else if (previewDirection === "west") {
            if (coordinates[0] === _finalPosition[0] && coordinates[1] > _finalPosition[1]) {
                result = true;
            }
        } else if (previewDirection === "east") {
            if (coordinates[0] === _finalPosition[0] && coordinates[1] < _finalPosition[1]) {
                result = true;
            }
        }
    }
    return result;
}

function getFinalPreviewSquare() {
    var position = new Array();
    var lengthToAccount = currentShip.Length - 1;
    // figure out the final preview square - if fields are not blank
    if (isSettingShips && firstPosition.length != 0 && previewDirection != null && currentShip != null) {
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
    for (var i = previewPositions.length - 1; i >= 0; i--) {
        if (coordinates[0] === previewPositions[i][0] && coordinates[1] === previewPositions[i][1]) {
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
        if (coordinates[0] === previewPositions[i][0] && coordinates[1] === previewPositions[i][1]) {
            return i;
        }
    }
    return -1;
}

// see if a square is next to another
function isNextTo(coordinates, previous) {
    var north = [previous[0] - 1, previous[1]];
    var south = [previous[0] + 1, previous[1]];
    var west = [previous[0], previous[1] - 1];
    var east = [previous[0], previous[1] + 1];

    if (coordinates[0] === north[0] && coordinates[1] === north[1] || coordinates[0] === south[0] && coordinates[1] === south[1] || coordinates[0] === west[0] && coordinates[1] === west[1] || coordinates[0] === east[0] && coordinates[1] === east[1]) {
        return true;
    } else {
        return false;
    }
}

// get all the positions to add preview squares to based on an index
function getPositionsToChange(coordinates, info) {
    //previewLog("getPositionsToChange(coordinates, info)");
    var positions = new Array();
    var count = info.index + 1; // should be 1 more than the index
    if (info.direction === 'north') {
        for (var i = 1; i <= count; i++) {
            var position = [coordinates[0] + i, coordinates[1]];
            if (position[0] > 10 || position[0] < 1) {
                break;
            } else {
                positions.push(position);
            }
        }
    } else if (info.direction === 'west') {
        for (var i = 1; i <= count; i++) {
            var position = [coordinates[0], coordinates[1] + i];
            if (position[1] > 10 || position[1] < 1) {
                break;
            } else {
                positions.push(position);
            }
        }
    } else if (info.direction === 'south') {
        for (var i = 1; i <= count; i++) {
            var position = [coordinates[0] - i, coordinates[1]];
            if (position[0] > 10 || position[0] < 1) {
                break;
            } else {
                positions.push(position);
            }
        }
    } else if (info.direction === 'east') {
        for (var i = 1; i <= count; i++) {
            var position = [coordinates[0], coordinates[1] - i];
            if (position[1] > 10 || position[1] < 1) {
                break;
            } else {
                positions.push(position);
            }
        }
    }
    //previewLog('Positions: ' + positions);
    return positions;
}

// determine if a square moused over is in the same row as the original square - depending if direction has been set or not
function isInRow(coordinates) {
    //previewLog("--------------------------------------------");
    //previewLog("isInRow(coordinates)");
    //previewLog('Coordinates: ' + coordinates[0] + ', ' + coordinates[1])
    var result = false; // it will stay as false if nothing is found
    if (firstPosition.length != 0) {
        //previewLog("(firstPosition.length != 0)");
        // determine north south east and west row - it doesn't matter if it has out of range indexes
        var north = new Array();
        var west = new Array();
        var south = new Array();
        var east = new Array();

        // find values for arrays using addition
        for (var i = 1; i <= BOARD_SIZE; i++) {
            var northPosition = [firstPosition[0] + i, firstPosition[1]];
            var westPosition = [firstPosition[0], firstPosition[1] + i];
            var southPosition = [firstPosition[0] - i, firstPosition[1]];
            var eastPosition = [firstPosition[0], firstPosition[1] - i];
            north.push(northPosition);
            west.push(westPosition);
            south.push(southPosition);
            east.push(eastPosition);
        }

        // see if a preview direction has been set - if it has only check for that. If it hasn't then any of the rows can contain that position
        if (previewDirection != null) {
            //previewLog("Ln: 343 (previewDirection != null)")
            // figure out which row is set
            if (previewDirection === 'north') {
                result = checkArrayForPosition(north, coordinates);
                //previewLog('North?: ' + result);
            } else if (previewDirection === 'west') {
                    result = checkArrayForPosition(west, coordinates);
                    //previewLog('West?: ' + result);
                } else if (previewDirection === 'south') {
                        result = checkArrayForPosition(south, coordinates);
                        //previewLog('South?: ' + result);
                    } else if (previewDirection === 'east') {
                            result = checkArrayForPosition(east, coordinates);
                            //previewLog('East?: ' + result);
                        }

            // return result
            return result;
        } else {
            //previewLog("Ln: 363 No previewDirection")
            result = checkArrayForPosition(north, coordinates);
            previewLog('North?: ' + result);
            if (result === true) {
                previewLog('END OF isInRow() METHOD - RESULT: ' + result);
                return result;
            } else {
                result = checkArrayForPosition(west, coordinates);
                previewLog('West?: ' + result);
            }
            if (result === true) {
                previewLog('END OF isInRow() METHOD - RESULT: ' + result);
                return result;
            } else {
                result = checkArrayForPosition(south, coordinates);
                previewLog('South?: ' + result);
            }
            if (result === true) {
                previewLog('END OF isInRow() METHOD - RESULT: ' + result);
                return result;
            } else {
                result = checkArrayForPosition(east, coordinates);
                previewLog('East?: ' + result);
            }
            if (result === true) {
                previewLog('END OF isInRow() METHOD - RESULT: ' + result);
                return result;
            }
        }
    } else {
        previewLog('Ln: 388 No firstPosition - result = ' + result);
        // you can't determine the position if a there is no firstPosition, because that means a preview row has not been started
    }
    previewLog('END OF isInRow() METHOD - RESULT: ' + result);
    return result;
}

// grab all coordinates that are relevent to a position and index of position in that row - will only work if isInRow is true for the coordinates
function getRowIndexAndDirection(coordinates) {
    //previewLog("--------------------------------------------");
    //previewLog("getRowIndexAndDirection(coordinates)");
    //previewLog('Coordinates: ' + coordinates[0] + ', ' + coordinates[1])
    var result = {
        rowPositions: new Array(),
        direction: null,
        index: -1 // -1 indicates error
    };
    if (firstPosition.length != 0) {
        /*            previewLog("(firstPosition.length != 0)");*/
        // determine north south east and west row - it doesn't matter if it has out of range indexes
        var north = new Array();
        var west = new Array();
        var south = new Array();
        var east = new Array();

        // find values for arrays using addition
        for (var i = 1; i <= BOARD_SIZE; i++) {
            var northPosition = [firstPosition[0] + i, firstPosition[1]];
            var westPosition = [firstPosition[0], firstPosition[1] + i];
            var southPosition = [firstPosition[0] - i, firstPosition[1]];
            var eastPosition = [firstPosition[0], firstPosition[1] - i];
            north.push(northPosition);
            west.push(westPosition);
            south.push(southPosition);
            east.push(eastPosition);
        }

        // see if a preview direction has not been set - it needs to set it first.
        if (result.direction === null) {
            //previewLog("Ln: 455 No result.direction")
            var _isInRow = checkArrayForPosition(north, coordinates);
            //previewLog('North?: ' + isInRow);
            if (_isInRow === false) {
                _isInRow = checkArrayForPosition(west, coordinates);
                //previewLog('West?: ' + isInRow);
                if (_isInRow === false) {
                    _isInRow = checkArrayForPosition(south, coordinates);
                    //previewLog('South?: ' + isInRow);
                    if (_isInRow === false) {
                        _isInRow = checkArrayForPosition(east, coordinates);
                        //previewLog('East?: ' + isInRow);
                        if (_isInRow === false) {
                            //previewLog('NO DIRECTION COULD BE FOUND - RETURNING NULL');
                            //previewLog('-----------------------------------------');
                            return null;
                        } else {
                            result.direction = 'east';
                        }
                    } else {
                        result.direction = 'south';
                    }
                } else {
                    result.direction = 'west';
                }
            } else {
                result.direction = 'north';
            }
        }

        previewLog('result.direction: ' + result.direction);

        // now double check it isn't null
        if (result.direction != null) {
            previewLog("Ln: 490 (result.direction != null)");
            // figure out which row is set
            if (result.direction === 'north') {
                result.rowPositions = north;
                result.index = getArrayIndexOfPosition(north, coordinates);
            } else if (result.direction === 'west') {
                result.rowPositions = west;
                result.index = getArrayIndexOfPosition(west, coordinates);
            } else if (result.direction === 'south') {
                result.rowPositions = south;
                result.index = getArrayIndexOfPosition(south, coordinates);
            } else if (result.direction === 'east') {
                result.rowPositions = north;
                result.index = getArrayIndexOfPosition(east, coordinates);
            }

            // return result
            previewLog('END OF getRowAndIndex(coordinates) METHOD');
            previewLog('-----------------------------------------');
            return result;
        }
    } else {
        previewLog('Ln: 513 No firstPosition - result = ' + result);
        // you can't determine the position if a there is no firstPosition, because that means a preview row has not been started
    }

    if (result.rowPositions.length === 0 || result.index === -1) {
        // if it's -1, nothing can be returned
        result = null;
    }

    previewLog('END OF getRowAndIndex(coordinates) METHOD - RESULT: ' + result);
    previewLog('-----------------------------------------');
    return result;
}

// see if a coordinate is listed in an array of coordinates
function checkArrayForPosition(array, coordinates) {
    for (var i = 0; i < array.length; i++) {
        if (array[i][0] === coordinates[0] && array[i][1] === coordinates[1]) {
            return true;
        }
    }
    return false;
}

// get the index of coordinates in an array of coordinates
function getArrayIndexOfPosition(array, coordinates) {
    for (var i = 0; i < array.length; i++) {
        if (array[i][0] === coordinates[0] && array[i][1] === coordinates[1]) {
            return i;
        }
    }
    return -1; // -1 indicates error
}

// determine which direction a line of squares is going
function getDirection(coordinates, previous) {
    var north = [previous[0] + 1, previous[1]];
    var south = [previous[0] - 1, previous[1]];
    var west = [previous[0], previous[1] + 1];
    var east = [previous[0], previous[1] - 1];
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
    previewLog('getDirectionUsingIndex(coordinates, previous, index)');
    var north = [previous[0] + index, previous[1]];
    var south = [previous[0] - index, previous[1]];
    var west = [previous[0], previous[1] + index];
    var east = [previous[0], previous[1] - index];
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
function getDifferenceUsingIndex(coordinates, previous, index) {
    previewLog('getDirectionUsingIndex(coordinates, previous, index)');
    var north = [previous[0] + index, previous[1]];
    var south = [previous[0] - index, previous[1]];
    var west = [previous[0], previous[1] + index];
    var east = [previous[0], previous[1] - index];
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
function setShip(positionString) {
    var square = document.getElementById('square' + positionString);
    var shipPreview = document.getElementById('preview' + positionString);
    var positionCoordinates = getCoordinates(positionString);

    // first make sure it doesn't have a ship or peg
    if (square.getAttribute('hasShip') == 'false' && square.getAttribute('hasPeg') == 'false') {

        // check if it has a preview already - if it does and it's the first
        // square, then unset it
        if (square.getAttribute('hasPreview') == 'true' && firstPosition.length != 0 && firstPosition[0] === positionCoordinates[0] && firstPosition[1] === positionCoordinates[1]) {

            shipPreview.className = 'ship-preview hide';
            firstPosition.length = 0; // like setting them null
            previousPosition.length = 0; // like setting it to null
            previewPositions.length = 0;
            square.setAttribute('hasPreview', false);
            squareCount = 0;
        } else if (firstPosition.length === 0) {
            // otherwise if a position is not set yet
            firstPosition = positionCoordinates;
            previousPosition = positionCoordinates;
            previewPositions.push(positionCoordinates);
            shipPreview.className = 'ship-preview';
            square.setAttribute('hasPreview', true);
            squareCount = 1;
        } else if (isFinalPreviewSquare(positionCoordinates)) {
            // otherwise if it's the last position that can be set - set the ship!
            //console.log('is final square');
            // make sure the previews have no red before allowing this to happen
            var isRed = checkForRed();
            //console.log('has red? ' + isRed);
            if (!isRed) {
                if (currentShip.Length != previewPositions.length) {
                    eliminateExtraPreviewPositions();
                }
                // HACK set this to fix a bug with setting the final square of a ship
                greenPreview = positionCoordinates;

                placeShipOnBoard();
                // TODO send info to the game hub to update everyone

                // go to the next ship
                //console.log('Going to next');
                goToNextShip();
            }
        }
    }
}

function placeShipPosition(positionId, previewIndex) {
    previewLog('placeShipPosition(positionId)');
    // the isRed boolean should already be checked by another function at this point
    // TODO make sure this will translate correctly to hub
    var imageSrc = '\\Images\\Ships\\' + currentShip.Name.toLowerCase() + '-' + currentShip.Direction + '-' + (previewIndex + 1) + '.png';
    currentShip.Positions[positionId].YPos = previewPositions[previewIndex][0];
    currentShip.Positions[positionId].XPos = previewPositions[previewIndex][1];
    currentShip.Positions[positionId].Image = imageSrc;

    // set position on actual board so that it has a ship
    gameInformation.Board.PlayerBoard.Positions[positionId].HasShip = true;
    gameInformation.Board.PlayerBoard.Positions[positionId].Image = imageSrc;

    // set image too
    var ship = document.getElementById('ship' + positionId);
    ship.className = 'ship';
    ship.src = imageSrc;
    var preview = document.getElementById('preview' + positionId);
    preview.className = 'preview hide';
    // set square properties
    var square = document.getElementById('square' + positionId);
    square.setAttribute('hasShip', true);
    square.setAttribute('hasPreview', false);
}

function placeShipOnBoard() {
    // the isRed boolean should already be checked by another function at this point
    previewLog('placeShipOnBoard()');
    // store info
    currentShip.Direction = previewDirection;
    currentShip.IsSet = true;

    // set all the ship positions - on both the UI and in the board info
    for (var i = 0; i < currentShip.Length; i++) {

        var positionId = previewPositions[i][0] + '-' + previewPositions[i][1];

        placeShipPosition(positionId, i);
    }
}

function checkForRed() {
    //console.log('checking for red');
    for (var i = 0; i < previewPositions.length; i++) {
        var preview = document.getElementById('preview' + previewPositions[i][0] + '-' + previewPositions[i][1]);
        if (preview.className.includes('error')) {
            //console.log('yes red');
            return true;
        }
    }
    //console.log('no red');
    return false;
}

// SETTING SHIPS
function startSettingShips() {
    // only at the beginning of the game - should just be called once
    //console.log('settingShips');
    isSettingShips = true;
    shipArray = new Array();
    shipArray.push(gameInformation.Board.PlayerBoard.Ships.Battleship);
    shipArray.push(gameInformation.Board.PlayerBoard.Ships.Carrier);
    shipArray.push(gameInformation.Board.PlayerBoard.Ships.Cruiser);
    shipArray.push(gameInformation.Board.PlayerBoard.Ships.Destroyer);
    shipArray.push(gameInformation.Board.PlayerBoard.Ships.Submarine);
    shipIndex = 0;
    currentShip = shipArray[shipIndex];
}

// eliminate any extra preview positions getting in the way of placing a ship
// HACK using this method to fix problem with previewPositions - figure out what causes them in the first place
function eliminateExtraPreviewPositions() {
    lastShipLog('Ln 819: previewPositions before: ' + previewPositions);

    // first create an array without repeats
    var newArray = new Array();

    // loop through previewPositions and only add positions that aren't already in the array
    for (var i = 0; i < previewPositions.length; i++) {
        if (!containPosition(previewPositions[i], newArray)) {
            lastShipLog('adding position ' + previewPositions[i] + ' to new array');
            newArray.push(previewPositions[i]);
        } else {
            lastShipLog('NOT adding position ' + previewPositions[i] + ' to new array');
        }
    }

    // set the previewPositions to the new array
    lastShipLog('Setting new array');
    previewPositions = newArray;

    // now just make sure the length is correct
    if (previewPositions.length != currentShip.Length) {
        lastShipLog('splicing off unnecessary positions');
        previewPositions = previewPositions.splice(0, currentShip.Length);
    }

    lastShipLog('previewPositions after: ' + previewPositions);
}

// returns a bool for if an array has a position or not
function containPosition(position, array) {
    lastShipLog('Does the array ' + array + ' contain position ' + position + '?');
    for (var i = 0; i < array.length; i++) {
        if (array[i][0] === position[0] && array[i][1] === position[1]) {
            lastShipLog('true');
            return true;
        }
    }
    lastShipLog('false');
    return false;
}

function goToNextShip() {
    console.log('****************Going to the next ship*******************');

    // HACK there is a timing issue with the last square in the ship placement - this should ensure it will be resolved
    // loop backwards through preview positions - if any of them are not set right, try setting it again

    for (var i = currentShip.Length - 1; i >= 0; i--) {
        var positionId = previewPositions[i][0] + '-' + previewPositions[i][1];

        var square = document.getElementById('square' + positionId);
        var preview = document.getElementById('preview' + positionId);

        if (currentShip.Length != previewPositions.length) {
            eliminateExtraPreviewPositions();
        }

        if (square.getAttribute('hasShip') == 'false' || square.getAttribute('hasPreview') == 'true' || preview.className != 'preview hide') {

            //console.log('found unresolved position at ' + positionId);

            placeShipPosition(positionId, i);
        } else {
            // counting backwards - if a position is ok, we should break because all the others should be fine too
            break;
        }
    }

    //// also double check the last position in the preview chain
    //let lastPositionId = previousPosition[previewPositions.length - 1][0] + '-' +
    //    previousPosition[previewPositions.length - 1][1];
    //let lastPreview = document.getElementById('preview' + lastPositionId);
    //let lastSquare = document.getElementById('square' + lastPositionId);

    //if (lastPreview.className === "ship-preview last" ||
    //    lastSquare.getAttribute('hasShip') == 'false') {
    //    placeShipPosition(positionId, previewPositions.length - 1);
    //    lastPreview.className = "ship-preview hide";
    //}

    // save the board through the hub and database!
    saveBoard();

    // unset all the preview stuff
    squareCount = 0;
    firstPosition.length = 0;
    previousPosition.length = 0;
    finalPosition.length = 0;
    previewPositions.length = 0;
    previewDirection = null;
    allowSet = true;
    greenPreview = null;

    // increment the index
    shipIndex++;

    // make sure it wasn't the last ship - if it was, stop setting ships
    if (shipIndex >= shipArray.length) {
        //console.log('index out of range');
        stopSettingShips();
    } else {
        currentShip = shipArray[shipIndex];
        //console.log('setting next ship');
        //console.log(currentShip);
    }
}

function stopSettingShips() {
    //console.log('stop setting ships');
    isSettingShips = false;
    currentShip = null;
    shipIndex = 0;
    shipArray = null;
}

// TESTING METHODS

// this will log the text to the console only if the showTestLogs bool is true
function previewLog(text) {
    if (showPreviewLogs) {
        // determine
        console.log(text);
    }
}

function lastShipLog(text) {
    if (showLastShipSquareLogs) {
        // determine
        console.log(text);
    }
}

// variables

var showPreviewLogs = true; // Set true of false for whether to see the preview console logs in the console
var showLastShipSquareLogs = false;

var VERTICAL_INFO = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"];
var BOARD_SIZE = 10;

// TEMPORARY

var TestShipClass = function TestShipClass(Name, Length) {
    _classCallCheck(this, TestShipClass);

    this.Name = Name;
    this.Length = Length;
    this.Direction = null;
    this.Positions = new Array();
    this.IsSet = false;
    this.IsSunk = false;
};

var TestPositionClass = function TestPositionClass(XPos, YPos, Image) {
    _classCallCheck(this, TestPositionClass);

    this.XPos = XPos;
    this.YPos = YPos;
    this.Image = Image;
    this.IsHit = false;
};

var testShip = new TestShipClass('Carrier', 5);

// ship setting variables
var isSettingShips = true; // default = false
var currentShip = null; // this should be null unless ships are being set
var shipIndex = 0; // for looping through ships
var squareCount = 0; // how many squares have been dragged over
var firstPosition = new Array(); // null indicates nothing is clicked
var previousPosition = new Array(); // preview position that mouse dragged over
var finalPosition = new Array(); // should stay null until last position is clicked
var previewPositions = new Array(); // all positions in the current stream of previews lol
var previewDirection = null;
var shipArray = null; // for holding all the ships that are dictionary values
var allowSet = true; // gets turned to false if a preview line runs over something
var greenPreview = null; // this is to fix a bug with the last preview square

// event listeners
document.body.addEventListener('load', function () {
    addSquares(playerBoard);
});

document.body.onload = addSquares('playerBoard');
