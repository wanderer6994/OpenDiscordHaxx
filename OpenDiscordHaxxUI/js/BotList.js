let socket;

const ListOpcode = {
    List: 0,
    Token: 1,
    BotModify: 2
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot");

    socket.onopen = function() {
        SendJson({ op: ListOpcode.List });
    }

    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case ListOpcode.List:
                OnList(payload.bots);
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
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function SendJson(jsonData) {
    socket.send(JSON.stringify(jsonData));
}


function OnList(botList) {

    if (botList.length == 0) {
        FatalError('No tokens are loaded');

        return;
    }

    const table = document.getElementById('bot-list');

    let html = '';

    for (let i = 0; i < botList.length; i++) {
        const bot = botList[i];

        html += '<tr id="row-' + i + '">\n'
                + '<td>' + bot.at + '</td>\n'
                + '<td>' + bot.id + '</td>\n'
                + '<td>' + bot.hypesquad + '</td>\n'
                + '<td>' + bot.verification + '</td>\n'
                + '</tr>';
    }

    table.innerHTML = html;

    table.childNodes.forEach(row => {
        $('#' + row.id).contextMenu({
            menuSelector: "#bot-list-context-menu",
            menuSelected: OnContextMenuUsed
        });
    });
}


function OnContextMenuUsed(invokedOn, selectedMenu) {
    const info = GetRowInformation(document.getElementById(invokedOn[0].parentNode.id));

    switch (selectedMenu.text()) {
        case 'Modify':
            OnModify(info);
            break;
        case 'Get token':
            OnGetToken(info);
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


function OnGetToken(info) {
    socket.send(JSON.stringify({ op: 1, id: info.id }));
}


function GetRowInformation(row) {
    return { at: row.childNodes[1].innerText, 
             id: row.childNodes[3].innerText, 
             hypesquad: row.childNodes[5].innerText,
             verification: row.childNodes[7].innerText };
}