let socket;

const DashboardOpcode = {
    StatusUpdate: 0,
    OverlookUpdate: 1,
    AttacksUpdate: 2
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
                OnStatusUpdate(payload.data);
                break;
            case DashboardOpcode.OverlookUpdate:
                OnOverlookUpdate(payload.data);
                break;
            case DashboardOpcode.AttacksUpdate:
                OnAttacksUpdate(payload.data);
                break;
        }
    }
    //this probably means the server is down or we don't have a connection to the internet
    socket.onerror = function(error) {
        OnStatusUpdate({ status: "Unreachable" });

        OpenSocket();
    }
}


//sets serverStatus's text to the new status, as well as changing it's color depending on the status
function OnStatusUpdate(data) {
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
            OnOverlookUpdate({ accounts: 0, attacks: 0 });
            OnAttacksUpdate({});
            break;
        default:
            statusLabel.style.color = "rgb(170,192,195)";
    }
}


function OnOverlookUpdate(data) {
    document.getElementById('account-amount').innerHTML = 'Loaded accounts: <span style="color: rgb(230,252,255)">' + data.accounts + '</span>';
    document.getElementById('attack-amount').innerHTML = 'Ongoing attacks: <span style="color: rgb(230,252,255)">' + data.attacks + '</span>';
}


function OnAttacksUpdate(attackList) {
    let html = '';

    for (let i = 0; i < attackList.length; i++) {
        let row = '<tr id="row-' + i + '" style="letter-spacing: 0.7px; font-size: 17.5px">\n';
        row += '<td>' + attackList[i].type + '</td>\n';
        row += '<td>' + attackList[i].bots + '</td>\n';
        row += '</tr>';

        html += row;
    }

    document.getElementById('attack-list').innerHTML = html;
}