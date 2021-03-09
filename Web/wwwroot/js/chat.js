"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44337/chathub").build();

//Disable send button until connection is established
document.getElementById("checkButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    alert(message);
});

connection.start().then(function () {
    document.getElementById("checkButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document
    .getElementById("checkButton")
    .addEventListener("click", function (event) {
        connection
            .invoke("CheckUser")
            .catch(function (err) {
                return console.error(err.toString());
            });
        event.preventDefault();
    });