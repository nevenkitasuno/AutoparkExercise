import { getAllEntitiesAsync } from "../utils/FetchUtils.js";

export async function createTableFromGetRequestAsync(tableId, controllerName) {
    var Table = document.createElement("table")
    Table.Id = tableId

    let data = await getAllEntitiesAsync(controllerName)

    generateTable(Table, data)
    generateTableHead(Table, data[0])
    return Table
}

function generateTableHead(table, dataSample) {
    let thead = table.createTHead();
    let row = thead.insertRow();
    let keys = Object.keys(dataSample);
    for (let key of keys) {
        let th = document.createElement("th");
        let text = document.createTextNode(key);
        th.appendChild(text);
        row.appendChild(th);
    }
}

function generateTable(table, data) {
    for (let element of data) {
        let row = table.insertRow();
        for (let key in element) {
            let cell = row.insertCell();
            let text = document.createTextNode(element[key]);
            cell.appendChild(text);
        }
    }
}