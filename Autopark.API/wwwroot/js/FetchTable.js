const url = "http://127.0.0.1:5237/api/Vehicle";

function getVehicles() {
    fetch(url)
        .then(response => response.json())
        .then(data => _displayVehicles(data))
        .catch(error => console.error('Error:', error));
}

function _displayVehicles(vehicles) {
    console.log(vehicles);
    var temp = "";

    vehicles.forEach((x) => {
        temp += "<tr>";
        temp += "<td>" + x.id + "</td>";
        temp += "<td>" + x.price + "</td>";
        temp += "<td>" + x.manufactureYear + "</td>";
        temp += "<td>" + x.mileage + "</td>";
        temp += "<td>" + x.licensePlate + "</td>";
        temp += "</tr>"
    });

    document.getElementById("vehicles").innerHTML += temp;
}