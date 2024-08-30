import { getAllEntitiesAsync } from "../utils/FetchUtils.js";

function removeFieldsWithArrayType(obj)
{
    for (let key in obj) {
        if (Array.isArray(obj[key])) {
            delete obj[key];
        }
    }
    return obj;
}

export async function createTableFromGetRequestAsync(tableId, controllerName) {
    var Table = document.createElement("table")
    Table.id = tableId

    let data = await getAllEntitiesAsync(controllerName)

    for (let i in data) {
        data[i] = removeFieldsWithArrayType(data[i])
    }

    generateTable(Table, data)
    generateTableHead(Table, data[0])

    let caption = document.createElement("caption");
    caption.textContent = "Table of " + controllerName + " entities";
    Table.appendChild(caption);

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
    // console.log(data)
    let rowNum = 0
    for (let element of data) {
        let row = table.insertRow();
        for (let key in element) {
            let cell = row.insertCell();
            let text = document.createTextNode(element[key]);
            let cellId = table.Id + "-" + key + "-R" + rowNum
            cell.id = cellId;
            cell.appendChild(text);
        }
        rowNum++
    }
}