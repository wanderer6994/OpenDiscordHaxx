let socket;

const ReconOpcode = {
    Id: 0,
    StartRecon: 1,
    ReconCompleted: 2,
    ServerNotFound: 3
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/recon");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        if (payload.op == ReconOpcode.Id)
            document.getElementById('recon-id').innerText = payload.id;
        if (payload.id != document.getElementById('recon-id').innerText) {
            
            console.log('ono');
            
            return;
        }


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


function StartRecon() {
    socket.send(JSON.stringify({ op: ReconOpcode.StartRecon, guild_id: document.getElementById('guild-id').value }));
}


function UpdateRecon(data) {

    console.log(data);

    const name = document.getElementById('server-name');
    const description = document.getElementById('server-desc');
    const region = document.getElementById('server-region');
    const verification = document.getElementById('verification-level');
    const vanityInv = document.getElementById('vanity-invite');
    const roleList = document.getElementById('role-list');
    const botsInGuild = document.getElementById('bots-in-server');

    name.innerText = data.name;
    description.innerText = data.description;
    region.innerText = "Server region: " + data.region;
    verification.innerText = "Verification level: " + data.verification;
    vanityInv.innerText = "Custom invite: " + data.vanity_invite;
    botsInGuild.innerText = data.bots_in_guild + ' bots are in this server';

    let html = '';

    data.roles.forEach(role => {
        html += '<tr id="row-' + i + '">\n'
                + '<td>' + role.name + '</td>\n'
                + '<td>' + role.id + '</td>\n'
                + '</tr>';
    });

    roleList.innerHTML = html;
}