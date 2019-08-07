let socket;


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/raid");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        ShowToast(payload.succeeded, payload.message);
    }
}

function StartBot(data) {
    socket.send(JSON.stringify(data));
}