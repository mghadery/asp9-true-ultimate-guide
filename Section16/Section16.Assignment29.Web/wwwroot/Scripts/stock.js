var token = document.getElementById("token").value;
var stockSymbol = document.getElementById("stockSymbol").value;
var oldPrice = -1;
var url = 'wss://ws.finnhub.io?token=' + token;

console.log(token);
console.log(stockSymbol);
console.log(url);

var socket = new WebSocket(url);


socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }));
})

socket.addEventListener('message', function (event) {
    //console.log(event.data)
    //return;
    if (event.data.type == 'error') {
        console.log(event.data.msg);
        return;
    }

    var data = JSON.parse(event.data);
    if (!data) {
        console.log("null data");
        return;
    }

    var price = data.data[0].p;
    console.log(price);

    if (price != oldPrice) {
        document.getElementById("price").innerHTML = price.toFixed(2);
        oldPrice = price;
    }
}
)

window.onunload = function () {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': stockSymbol }))
}