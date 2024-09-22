import { generateTableHead } from "./GenerateTable.js"
import { getAllEntitiesAsync } from "./FetchGetUtils.js"

export async function createUpsertFormAsync(upsertFormId, controllerName) {
    var Form = document.createElement("form")
    Form.id = upsertFormId

    var Table = document.createElement("table")
    Table.id = upsertFormId + "-Table"
    Form.appendChild(Table)

    let data = await getAllEntitiesAsync(controllerName)

    generateTableHead(Table, data[0])
    generateForm(Table, data[0])

    let caption = document.createElement("caption");
    caption.textContent = controllerName + " upsert form";
    Table.appendChild(caption);

    return Form
}

// TODO: different functions for 
function generateForm(table, entity) {
    let row = document.createElement("tr")

    for (let key in entity) {
        let td = document.createElement("td");
        let tdId = table.id + "-" + key
        td.id = tdId;

        let input;

        // Check if the key is 'vehicleType' to create a dropdown
        if (key === "vehicleType") {
            input = createDropdown(tdId, ["Car", "Truck", "Motorcycle"]); // Call the new function
        } else {
            input = document.createElement("input");
            input.type = "text";
            input.value = entity[key];
            input.name = key;
        }

        td.appendChild(input);
        row.appendChild(td);
    }
    table.appendChild(row)
}

// New function to create a dropdown
function createDropdown(fieldId, options) {
    let select = document.createElement("select");
    select.name = fieldId.split('-').pop(); // Get the key from the field ID

    options.forEach(option => {
        let optionElement = document.createElement("option");
        optionElement.value = option;
        optionElement.textContent = option;
        select.appendChild(optionElement);
    });

    return select;
}