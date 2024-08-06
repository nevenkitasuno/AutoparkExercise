const apiPath = "http://127.0.0.1:5237/api";

function getAndDisplayData(controller, displayFunction) {
    fetch(apiPath + controller)
        .then(response => response.json())
        .then(data => displayFunction(data))
        .catch(error => console.error('Error:', error));
}

function displayVehiclesWithManufacturerAndModelName(vehiclesWithManufacturerAndModelNames) {
    var temp = "";

    vehiclesWithManufacturerAndModelNames.forEach((x) => {
        temp += '<tr id="vehicle-row-' + x.id + '">';
        temp += "<td>" + formDeleteButton('deleteVehicle', x.id) + "</td>";
        temp += '<td><button class="editbtn">✍️</button></td>';
        temp += "<td>" + x.id + "</td>";
        temp += "<td>" + x.price + "</td>";
        temp += "<td>" + x.manufactureYear + "</td>";
        temp += "<td>" + x.mileage + "</td>";
        temp += "<td>" + x.licensePlate + "</td>";
        temp += "<td>" + x.manufacturerCompany + " " + x.modelName + "</td>";
        temp += "</tr>"
    });

    document.getElementById("VehiclesTable").innerHTML += temp;
}

function formDeleteButton(deleteFunctionName, id)
{
    return `<button class="deletebtn" onclick="${deleteFunctionName}('${id}')">❌</button>`;
}

function deleteVehicle(id) {
    // fetch(apiPath + "/Vehicle/" + id, {
    //     method: 'DELETE',
    //     headers: {
    //         'Content-Type': 'application/json'
    //     }
    // })
    //    .then(response => response.json())
    //    .then(data => console.log(data))
    //    .catch(error => console.error('Error:', error));
    rowId = "vehicle-row-"+id
    console.log(rowId)
    const row = document.getElementById(rowId);
    row.remove(); 
}

function displayBrands(brands) {
    var temp = "";

    brands.forEach((x) => {
        temp += "<tr>";
        temp += "<td>" + x.id + "</td>";
        temp += "<td>" + x.manufacturerCompany + "</td>";
        temp += "<td>" + x.modelName + "</td>";
        temp += "<td>" + x.engineDisplacementLiters + "</td>";
        temp += "<td>" + vehicleTypeIntToStr(x.vehicleType) + "</td>";
        temp += "<td>" + x.fuelTankCapacityLiters + "</td>";
        temp += "<td>" + x.seatsCount + "</td>";
        temp += "<td>" + x.liftWeightCapacityKg + "</td>";
        temp += "</tr>"
    });

    document.getElementById("BrandsTable").innerHTML += temp;
}

function vehicleTypeIntToStr(vehicleTypeInt)
{
    switch (vehicleTypeInt) {
        case 0:
            return "Легковой автомобиль";
        case 1:
            return "Грузовой автомобиль";
        case 2:
            return "Автобус";
        default:
            return "Unknown";
    }
}