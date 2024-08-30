import { getAllEntitiesAsync } from "./FetchGetUtils.js";

export async function createTableFromGetRequestAsync(tableId, controllerName) {
    var Table = document.createElement("table")
    Table.id = tableId

    let data = await getAllEntitiesAsync(controllerName)

    generateTable(Table, data)
    generateTableHead(Table, data[0])

    let caption = document.createElement("caption");
    caption.textContent = "Table of " + controllerName + " entities";
    Table.appendChild(caption);

    return Table
}

export function generateTableHead(table, dataSample) {
    let thead = table.createTHead();
    let theadId = table.id + "-thead"
    thead.id = theadId;
    
    let row = thead.insertRow();
    let keys = Object.keys(dataSample);
    for (let key of keys) {
        let th = document.createElement("th");
        let thId = table.id + "-" + key + "-Head"
        th.id = thId;
        let text = document.createTextNode(key);
        th.appendChild(text);
        row.appendChild(th);
    }
}

export function generateTable(table, data) {
    let rowNum = 0
    for (let element of data) {
        let row = table.insertRow();
        for (let key in element) {
            let cell = row.insertCell();
            let text = document.createTextNode(element[key]);
            let cellId = table.id + "-" + key + "-R" + rowNum
            cell.id = cellId;
            cell.appendChild(text);
        }
        rowNum++
    }
}