let socket;


window.onload = function() {
    socket = new WebSocket("ws://localhost:420/raid");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case 'info':
                HandleInfo(payload); //all raidbot implementations are expected to have one of these
                break;
            case 'raid_success':
                if (payload.succeeded)
                    payload.message = '<strong>Success!</strong> ' + payload.message;
        
                ShowToast(payload.succeeded ? ToastType.Success : ToastType.Error, payload.message);
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}

function StartBot(data) {
    socket.send(JSON.stringify(data));
}