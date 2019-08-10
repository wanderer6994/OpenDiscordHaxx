let socket;

const DashboardOpcode = {
    StatusUpdate: 0,
    OverlookUpdate: 1
}


window.onload = function() {
    OpenSocket();
}


function OpenSocket() {
    socket = new WebSocket("ws://localhost/dashboard");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.opcode) {
            case DashboardOpcode.StatusUpdate: //this does not account for the server dying
                StatusUpdate(payload.data);
                break;
            case DashboardOpcode.OverlookUpdate:
                OverlookUpdate(payload.data);
                break;
        }
    }
    //this probably means the server is down or we don't have a connection to the internet
    socket.onerror = function(error) {
        StatusUpdate({ status: "Unreachable" });

        OpenSocket();
    }
}


//sets serverStatus's text to the new status, as well as changing it's color depending on the status
function StatusUpdate(data) {
    const statusLabel = document.getElementById('serverStatus');

    statusLabel.innerText = data.status.toUpperCase();

    switch (statusLabel.innerText) {
        case "READY":
            statusLabel.style.color = "rgb(50,205,50)";
            break;
        case "LOADING BOTS":
            statusLabel.style.color = "rgb(255,255,0)";
            break;
        case "UNREACHABLE":
            statusLabel.style.color = "rgb(130,0,0)";
            OverlookUpdate({ accounts: 0, attacks: 0 });
            break;
        default:
            statusLabel.style.color = "rgb(170,192,195)";
    }
}


function OverlookUpdate(data) {
    document.getElementById('accountAmount').innerHTML = 'Loaded accounts: <span style="color: rgb(230,252,255)">' + data.accounts + '</span>';
    document.getElementById('attackAmount').innerHTML = 'Ongoing attacks: <span style="color: rgb(230,252,255)">' + data.attacks + '</span>';
}