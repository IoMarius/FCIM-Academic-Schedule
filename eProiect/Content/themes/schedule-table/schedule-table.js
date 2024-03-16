window.onload = function () {
    transformTable();
};

function transformTable() {
    var originalTable = document.getElementById("desktopTable");
    var transformedTable = document.getElementById("mobileTable");

    for (var i = 1; i < originalTable.rows.length; i++) {
        var row = originalTable.rows[i];
        for (var j = 0; j < row.cells.length; j++) {
            var cell = row.cells[j];
            var newRow = transformedTable.insertRow();
            var newCell = newRow.insertCell();

            // Clone attributes from original cell to the new cell
            for (var k = 0; k < cell.attributes.length; k++) {
                var attr = cell.attributes[k];
                newCell.setAttribute(attr.name, attr.value);
            }

            // Clone nested elements
            var cellContents = cell.cloneNode(true);
            // Remove the cloned <td> element and append its children
            while (cellContents.firstChild) {
                newCell.appendChild(cellContents.removeChild(cellContents.firstChild));
            }
        }
    }
}