let socket;

const FarmerOpcode = {
    Start: 0,
    error: 1,
    Stopped: 2
}


window.onload = function() {
    socket = new WebSocket("ws://localhost:420/farmer");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case FarmerOpcode.Start:
                ShowToast(ToastType.Info, 'Farmer has started!')
                break;
            case FarmerOpcode.error:
                ShowToast(ToastType.Error, payload.error_message)
                break;
            case FarmerOpcode.Stopped:
                ShowToast(ToastType.Success, 'Farmer has finished')
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}

function SendJSON(data) {
    socket.send(JSON.stringify(data));
}