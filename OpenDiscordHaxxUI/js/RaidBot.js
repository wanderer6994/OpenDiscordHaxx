let socket;


window.onload = function() {
    socket = new WebSocket("ws://localhost:420/raid");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case 'info':
                console.log(payload.bots)

                if (payload.bots == 0) {
                    FatalError('No bots have been loaded');
                }
                else {
                    if (typeof HandleInfo !== 'undefined')
                        HandleInfo(payload);
                }
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

function SendJSON(data) {
    socket.send(JSON.stringify(data));
}