let socket;

const ListOpcode = {
    List: 0,
    Token: 1,
    BotModification: 2
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot");

    socket.onopen = function() {
        SendJson({ op: 0 });
    }

    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case ListOpcode.List:
                OnList(payload.list);
                break;
            case ListOpcode.Token:
                $('#bot-token-modal').modal({ show: true });

                let at = '';

                document.getElementById('bot-list').childNodes.forEach(row => {
                    const info = GetRowInformation(row);

                    if (info.id == payload.id)
                        at = info.at;
                });


                document.getElementById('bot-token-title').innerText = 'Token for ' + at;
                document.getElementById('bot-token').innerHTML = payload.token;
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function SendJson(jsonData) {
    socket.send(JSON.stringify(jsonData));
}


function OnList(botList) {
    const table = document.getElementById('bot-list');

    let html = '';

    for (let i = 0; i < botList.length; i++) {
        let row = '<tr id="row-' + i + '" style="">\n';
        row += '<td>' + botList[i].at + '</td>\n';
        row += '<td>' + botList[i].id + '</td>\n';
        row += '<td>' + botList[i].hypesquad + '</td>\n';
        row += '<td>' + botList[i].verification + '</td>\n';
        row += '</tr>';

        html += row;
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