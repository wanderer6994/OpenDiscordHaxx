let socket;


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/recon");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


