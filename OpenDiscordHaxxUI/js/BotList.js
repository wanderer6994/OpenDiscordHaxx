let socket;

window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/list");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        //populate list
    };
    socket.onerror = function() {
        //show error
    }
};