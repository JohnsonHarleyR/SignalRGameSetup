

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
                square.className = 'empty-square';
            } else if (r === 0 || c === 0) {
                square.className = 'info-square';
            } else {
                // if it's a regular square, also append a child square and a peg hole
                square.className = 'square';

                // TODO consider adding a highlight to the inside square
                let pegHole = document.createElement('div');
                pegHole.className = 'peg-hole';

                //// Add a peg hole too

                square.appendChild(pegHole);

            }

            // append it to the container
            board.appendChild(square);

        }

        // change position for next row
        yPos += squareLength + 5;

        // set the x position back to it's start
        xPos = 5;

    }


}


//var playerBoard = document.getElementById('playerBoard');


document.body.addEventListener('load', function () { addSquares(playerBoard) }); 

document.body.onload = addSquares('playerBoard');