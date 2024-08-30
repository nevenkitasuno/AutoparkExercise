export function mapTableColumn(tableId, columnName, func)
{
    var table = document.getElementById(tableId);
    var tbodyRowCount = table.tBodies[0].rows.length;
    for (var i = 0; i < tbodyRowCount; i++)
    {
        var cellId = table.Id + "-" + columnName + "-R" + i
        var cell = document.getElementById(cellId);
        cell.textContent = func(cell.textContent);
    }
}

export function deleteTableColumn(tableId, columnName)
{
    var table = document.getElementById(tableId);
    var tbodyRowCount = table.tBodies[0].rows.length;
    for (var i = 0; i < tbodyRowCount; i++)
    {
        var cellId = table.id + "-" + columnName + "-R" + i
        var cell = document.getElementById(cellId);
        cell.remove()
    }
    let thId = table.id + "-" + columnName + "-Head"
    let th = document.getElementById(thId)
    th.remove()
}

export async function mapTableColumnAsync(tableId, columnName, func)
{
    var table = document.getElementById(tableId);
    var tbodyRowCount = table.tBodies[0].rows.length;
    for (var i = 0; i < tbodyRowCount; i++)
    {
        var cellId = table.id + "-" + columnName + "-R" + i
        var cell = document.getElementById(cellId);
        cell.textContent = await func(cell.textContent);
    }
}