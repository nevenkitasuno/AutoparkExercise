fetch("http://127.0.0.1:5237/api/Vehicle", {
    "method": "GET",
    "headers": {
        "Access-Control-Allow-Origin": "http://127.0.0.1:5237/api/Vehicle",
    }
}).then(
    response => {
        response.json().then(
            data => {

                console.log(data);
                var temp = "";

                data.records.forEach((x) => {
                    temp += "<tr>";
                    temp += "<td>" + x.Id + "</td>";
                    temp += "<td>" + x.Price + "</td>";
                    temp += "<td>" + x.Year + "</td>";
                    temp += "<td>" + x.Mileage + "</td>";
                    temp += "<td>" + x.LicensePlate + "</td>";
                    temp += "</tr>"
                });

                document.getElementById("data").innerHTML += temp;
            }
        )
    })