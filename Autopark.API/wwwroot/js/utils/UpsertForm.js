import { generateTableHead } from "./GenerateTable.js"
import { getAllEntitiesAsync } from "./FetchGetUtils.js"

export async function createUpsertFormAsync(upsertFormId, controllerName) {
    var form = document.createElement("form")
    form.id = upsertFormId

    var Table = document.createElement("table")
    form.appendChild(Table)

    let data = await getAllEntitiesAsync(controllerName)

    generateTableHead(Table, data[0])
    generateForm(Table, data[0])

    let caption = document.createElement("caption");
    caption.textContent = controllerName + " edit form";
    Table.appendChild(caption);

    return Table
}

function generateForm(table, entity) {
    let row = document.createElement("tr")

    for (let key in entity) {
        let td = document.createElement("td");
        let tdId = table.id + "-" + key
        td.id = tdId;

        let input = document.createElement("input");
        input.type = "text";
        input.name = key;
        input.value = entity[key];

        td.appendChild(input);
        row.appendChild(td);
    }
    table.appendChild(row)
}