let socket;

const ListOpcode = {
    List: 0,
    Token: 1,
    BotModify: 2,
    BotInfo: 3
}


const ListAction = {
    Add: 0,
    Remove: 1,
    Update: 2
}


window.onload = function() {
    socket = new WebSocket("ws://localhost:420/list");

    socket.onopen = function() {
        SendJson({ op: ListOpcode.List });
    }

    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case ListOpcode.List:

                const container = document.getElementById('bot-list-container');
                const botList = document.getElementById('bot-list');

                switch (payload.action) {
                    case ListAction.Add:
                        let currentId = botList.rows.length;
                        payload.bots.forEach(bot => {
                            let row = botList.insertRow(botList.rows.length);
                            row.id = 'row-' + currentId++;
                            row.innerHTML = '<td>' + bot.at + '</td>\n'
                                            + '<td>' + bot.id + '</td>\n'
                                            + '<td>' + bot.hypesquad + '</td>\n'
                                            + '<td>' + bot.verification + '</td>\n';

                            $('#' + row.id).contextMenu({
                                menuSelector: "#bot-list-context-menu",
                                menuSelected: OnContextMenuUsed
                            });
                        });
                        break;
                    case ListAction.Remove:

                        payload.bots.forEach(bot => {
                            botList.childNodes.forEach(row => {

                                if (row.nodeName != "#text") {
                                    if (GetRowInformation(row).id == bot.id)
                                        row.remove();
                                }
                            });
                        });

                        break;
                    case ListAction.Update:

                        payload.bots.forEach(bot => {
                            
                            for (let i = 0; i < botList.childNodes.length; i++) {
                                const row = botList.childNodes[i];

                                if (row.nodeName != "#text") {
                                    
                                    if (GetRowInformation(row).id == bot.id) {
                                        row.childNodes[0].innerText = bot.at; 
                                        row.childNodes[2].innerText = bot.id;
                                        row.childNodes[4].innerText = bot.hypesquad;
                                        row.childNodes[6].innerText = bot.verification;

                                        break;
                                    }
                                }
                            }
                        });
                        
                        break;
                }


                container.style.display = botList.childNodes.length > 1 ? 'block' : 'none';

                break;
            case ListOpcode.Token:
                $('#bot-token-modal').modal({ show: true });

                document.getElementById('bot-token-title').innerText = 'Token for ' + payload.at;
                document.getElementById('bot-token').innerHTML = payload.token;
                break;
            case ListOpcode.BotModify:
                if (payload.success)
                    ShowToast(ToastType.Success, '<strong>Success!</strong> ' + payload.at + ' has been modified!');
                else
                    ShowToast(ToastType.Error, 'Failed to modify ' + payload.at);
                break;
            case ListOpcode.BotInfo:
                $('#profile-modal').modal({ show: true });

                document.getElementById('profile-at').innerHTML = payload.at.split('#')[0] + '<span style="font-size: 17px; color: rgb(170,192,195); margin-left: 3px">#' + payload.at.split('#')[1] + '</span>';

                let html = '';

                payload.badges.forEach(badge => {
                    html += '<img src="../Images/' + badge + '.png" style="width: 30px; height: 30px; margin-right: 6px">';
                });

                document.getElementById('profile-badges').innerHTML = html;


                const guildList = document.getElementById('guild-list');
                guildList.innerHTML = '';
                payload.guilds.forEach(guild => {
                    let row = guildList.insertRow(guildList.rows.length);
                    row.id = 'guild-row-' + guildList.rows.length;
                    row.innerHTML = '<td>' + guild.name + '</td>\n'
                                    + '<td>' + guild.id + '</td>\n';
                });

                const friendList = document.getElementById('friends-list');
                friendList.innerHTML = '';
                payload.friends.forEach(friend => {
                    let row = friendList.insertRow(friendList.rows.length);
                    row.id = 'friend-row-' + friendList.rows.length;
                    row.innerHTML = '<td>' + friend.at + '</td>\n'
                                    + '<td>' + friend.id + '</td>\n';
                });

                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function SendJson(jsonData) {
    socket.send(JSON.stringify(jsonData));
}


function OnContextMenuUsed(invokedOn, selectedMenu) {
    const info = GetRowInformation(document.getElementById(invokedOn[0].parentNode.id));

    switch (selectedMenu.text()) {
        case 'Modify user':
            OnModify(info);
            break;
        case 'Get profile':
            OnGetProfile(info);
            break;
        case 'Get token':
            SendJson({ op: ListOpcode.Token, id: info.id });
            break;
    }
}


function OnModify(info) {
    $('#modify-bot-modal').modal({ show: true });

    document.getElementById('modify-title').innerText = 'Modify ' + info.at;
    document.getElementById('modify-id').innerText = info.id;

    const hype = document.getElementById('modify-hype');
    for (i = 0; i < hype.options.length; i++) {
        if (hype.options[i].innerText == info.hypesquad)
        {
            hype.selectedIndex = i;
            break;
        }
    }
}


function OnGetProfile(info) {
    SendJson({ op: ListOpcode.BotInfo, id: info.id });
}


function GetRowInformation(row) {
    return { at: row.childNodes[0].innerText, 
             id: row.childNodes[2].innerText, 
             hypesquad: row.childNodes[4].innerText,
             verification: row.childNodes[6].innerText };
}