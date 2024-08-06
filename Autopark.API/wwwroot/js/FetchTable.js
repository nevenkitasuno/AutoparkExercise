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
        temp += makeRowForVehicleWithManufacturerAndModelName(x);
        temp += "</tr>"
    });

    document.getElementById("VehiclesTable").innerHTML += temp;
}

function makeRowForVehicleWithManufacturerAndModelName(vehicleWithManufacturerAndModelName) {
    row = ""
    row += "<td>" + makeDeleteButton('deleteVehicle', vehicleWithManufacturerAndModelName.id) + "</td>";
    row += '<td><button class="editBtn">✏️</button></td>';
    row += "<td>" + vehicleWithManufacturerAndModelName.id + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.price + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.manufactureYear + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.mileage + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.licensePlate + "</td>";
    row += "<td>" + vehicleWithManufacturerAndModelName.manufacturerCompany + " " + vehicleWithManufacturerAndModelName.modelName + "</td>";
    return row
}

document.getElementById('SubmitVehicleBtn').addEventListener('click', async function(event) {
    event.preventDefault();

    const formData = {
        Price: document.getElementById('SubmitVehicleFormPrice').value,
        ManufactureYear: document.getElementById('SubmitVehicleFormManufactureYear').value,
        Mileage: document.getElementById('SubmitVehicleFormMileage').value,
        LicensePlate: document.getElementById('SubmitVehicleFormLicensePlate').value,
        BrandId: document.getElementById('SubmitVehicleFormBrand').value
    };

    try {
        const response = await fetch(apiPath + '/Vehicle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            console.log('Form submitted successfully');
        } else {
            console.error('Form submission failed');
        }
    } catch (error) {
        console.error('An error occurred during form submission:', error);
    }
    
    location.reload();
});

function makeDeleteButton(deleteFunctionName, id) {
    return `<button class="deleteBtn" onclick="${deleteFunctionName}('${id}')">❌</button>`;
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
    rowId = "vehicle-row-" + id
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

function vehicleTypeIntToStr(vehicleTypeInt) {
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