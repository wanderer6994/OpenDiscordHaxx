let socket;

const ReconOpcode = {
    StartRecon: 0,
    ReconCompleted: 1,
    ServerNotFound: 2
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/recon");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

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
                    roles: {},
                    bots_in_guild: 'No'
                });
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function UpdateRecon(data) {
    const name = document.getElementById('server-name');
    const description = document.getElementById('server-desc');
    const region = document.getElementById('server-region');
    const verification = document.getElementById('verification-level');
    const vanityInv = document.getElementById('vanity-invite');
    const roleList = document.getElementById('role-list');
    const botsInGuild = document.getElementById('bots-in-server');

    name.value = data.name;
    description.value = data.description;
    region.value = "Server region: " + data.region;
    verification.value = "Verification level " + data.verification;
    vanityInv.value = "Custom invite: " + data.vanity_invite;
    botsInGuild.vlaue = data.bots_in_guild + ' bots are in this server';

    let html = '';

    data.roles.forEach(role => {
        html += '<tr id="row-' + i + '">\n'
                + '<td>' + role.name + '</td>\n'
                + '<td>' + role.id + '</td>\n'
                + '</tr>';
    });

    roleList.innerHTML = html;
}