window.onload = function () {
    transformTable();
    setTimeout(scrollToTop, 100);
};

function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'instant' });
}

function transformTable() {
    var originalTable = document.getElementById("desktopTable");
    var transformedTable = document.getElementById("mobileTable");
    var numColumns = originalTable.rows[0].cells.length;


    for (var i = 1; i < numColumns; i++) {
        for (var j = 0; j < originalTable.rows.length; j++) {
            var cell = originalTable.rows[j].cells[i];

            if (cell.className == "empty") {
                continue;
            } 

            var newRow = transformedTable.insertRow();
            var newCell = newRow.insertCell();

            if (j == 0) {
                newRow.className = "mobile-table-header";
                if (transformedTable.rows.length - 2 >= 0) {
                    var rowBefore= transformedTable.rows[transformedTable.rows.length - 2];
                    if (rowBefore.cells[0].className != "mobile-weekday") {
                        rowBefore.cells[0].classList.add("mobile-last-day");
                    } else {
                        transformedTable.deleteRow(transformedTable.rows.length - 2);
                    }
                }
            }        

            // Clone attributes from original cell to the new cell
            var classList = cell.classList
            for (var k = 0; k < cell.classList.length; k++) {
                newCell.classList.add(classList[k])
            }

            // Clone nested elements
            var cellContents = cell.cloneNode(true);

            // Remove the cloned <td> element and append its children
            while (cellContents.firstChild) {
                newCell.appendChild(cellContents.removeChild(cellContents.firstChild));
            }    
        }
    }

    if (transformedTable.rows[transformedTable.rows.length - 1].cells[0].className == "mobile-weekday") {
        transformedTable.deleteRow(transformedTable.rows.length - 1);
    } else {
        transformedTable.rows[transformedTable.rows.length - 1].cells[0].classList.add("mobile-last-day");
    }
}