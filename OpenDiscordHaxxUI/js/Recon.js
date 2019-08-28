let socket;

const ReconOpcode = {
    Id: 0,
    StartRecon: 1,
    ReconCompleted: 2,
    ServerNotFound: 3
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/recon");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        if (payload.op == ReconOpcode.Id)
            document.getElementById('recon-id').innerText = payload.id;
        if (payload.id != document.getElementById('recon-id').innerText)
            return;


        switch (payload.op) {
            case ReconOpcode.ReconCompleted:
                UpdateRecon(payload);
                break;
            case ReconOpcode.ServerNotFound:
                UpdateRecon({
                    name: 'Please enter a server ID',
                    description: 'No description',
                    region: 'Unknown',
                    verification: 'Unknown',
                    vanity_invite: 'None',
                    roles: [],
                    bots_in_guild: 'No'
                });

                ShowToast(ToastType.Error, 'Server was not found');
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function StartRecon() {
    socket.send(JSON.stringify({ op: ReconOpcode.StartRecon, guild_id: document.getElementById('guild-id').value }));

    document.getElementById('recon-container').disabled = true;
}


function UpdateRecon(data) {
    document.getElementById('recon-container').disabled = false;


    document.getElementById('server-name').innerText = data.name;
    document.getElementById('server-desc').innerText = data.description;
    document.getElementById('server-region').innerText = "Server region: " + data.region;
    document.getElementById('verification-level').innerText = "Verification level: " + data.verification;
    document.getElementById('vanity-invite').innerText = "Custom invite: " + data.vanity_invite;
    document.getElementById('bots-in-server').innerText = data.bots_in_guild + ' bots are in this server';

    const roleList = document.getElementById('role-list');

    let html = '';

    for (let i = 0; i < data.roles.length; i++) {
        html += '<tr id="row-' + i + '">\n'
                + '<td>' + data.roles[i].name + '</td>\n'
                + '<td>' + data.roles[i].id + '</td>\n'
                + '</tr>';
    }

    roleList.innerHTML = html;

    roleList.childNodes.forEach(row => {
        $('#' + row.id).contextMenu({
            menuSelector: "#roles-context-menu",
            menuSelected: OnContextMenuUsed
        });
    });
}


function OnContextMenuUsed(invokedOn, selectedMenu) {
    const info = GetRowInformation(document.getElementById(invokedOn[0].parentNode.id));

    switch (selectedMenu.text()) {
        case "Get messagable":
            $('#role-modal').modal({ show: true });

            document.getElementById('role-modal-title').innerText = info.name + ' as messagable';
            document.getElementById('role-messagable').innerText = '<@&' + info.id + '>';
            break;
    }
}


function GetRowInformation(row) {
    return { name: row.childNodes[1].innerText, 
             id: row.childNodes[3].innerText };
}