import signalR = require("@microsoft/signalr");

let btn = document.getElementById("btnGetFullName");
btn.addEventListener("click", event => {
    var firstName = (document.getElementById("inputFirstName") as HTMLInputElement).value;
    var lastName = (document.getElementById("inputLastName") as HTMLInputElement).value;

    connection.invoke("getFullName", firstName, lastName)
        .then(val => {
            alert(val);
        });
});

//create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/stringtools")
    .build();



function startSuccess() {
    console.log("Connected.");
}

function startFail() {
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);

//export { connection };