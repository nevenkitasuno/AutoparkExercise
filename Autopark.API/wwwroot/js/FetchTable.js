const apiPath = "http://127.0.0.1:5237/api";

function getAndDisplayData(controller, displayFunction) {
    fetch(apiPath + controller)
        .then(response => response.json())
        .then(data => displayFunction(data))
        .catch(error => console.error('Error:', error));
}

function displayVehicles(vehicles) {
    var temp = "";

    vehicles.forEach((x) => {
        temp += "<tr>";
        temp += "<td>" + x.id + "</td>";
        temp += "<td>" + x.price + "</td>";
        temp += "<td>" + x.manufactureYear + "</td>";
        temp += "<td>" + x.mileage + "</td>";
        temp += "<td>" + x.licensePlate + "</td>";
        temp += "<td>" + x.brandId + "</td>";
        temp += "</tr>"
    });

    document.getElementById("VehiclesTable").innerHTML += temp;
}

function displayBrands(brands) {
    var temp = "";

    console.log(brands);

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