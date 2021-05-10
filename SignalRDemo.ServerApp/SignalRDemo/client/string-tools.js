"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var btn = document.getElementById("btnGetFullName");
btn.addEventListener("click", function (event) {
    var firstName = document.getElementById("inputFirstName").value;
    var lastName = document.getElementById("inputLastName").value;
    connection.invoke("getFullName", firstName, lastName)
        .then(function (val) {
        alert(val);
    });
});
//create connection
var connection = new signalR.HubConnectionBuilder()
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
