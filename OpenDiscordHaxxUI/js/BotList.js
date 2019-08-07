let socket;

const ListOpcode = {
    List: 0,
    Token: 1
}


window.onload = function() {
    socket = new WebSocket("ws://localhost/bot");

    socket.onopen = function() {
        socket.send(JSON.stringify({ op: 0 }));
    }

    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        switch (payload.op) {
            case ListOpcode.List:
                const table = document.getElementById('bot-list');

                let html = '';
            
                for (let i = 0; i < payload.list.length; i++) {
                    let row = '<tr id="row-' + i + '" style="border-style: hidden !important">\n';
                    row += "<td>" + payload.list[i].at + '</td>\n';
                    row += "<td>" + payload.list[i].id + '</td>\n';
                    row += "<td>" + payload.list[i].verification + '</td>\n';
                    row += "</tr>";
            
                    html += row;
                }
            
                table.innerHTML = html;
            
                for (let i = 0; i < payload.list.length; i++) {
                    $('#row-' + i).contextMenu({
                        menuSelector: "#bot-list-context-menu",
                        menuSelected: function (invokedOn, selectedMenu) {
                            const info = GetRowInformation(invokedOn[0]);

                            switch (selectedMenu.text()) {
                                case 'Modify':
                                    Modify(info);
                                    break;
                                case 'Get token':
                                    GetToken(info);
                                    break;
                            }
                        }
                    });
                }
                break;
            case ListOpcode.Token:
                $('#bot-token-modal').modal({ show: true });

                document.getElementById('bot-token-title').innerText = 'Token for ' + payload.id;
                document.getElementById('bot-token').innerHTML = payload.token;
                break;
        }
    };

    socket.onerror = function() {
        document.getElementById('unreachable').style.display = "block";
        document.getElementById('bot-list-container').style.display = "none";
    }
}


function Modify(info) {
    $('#modify-bot-modal').modal({ show: true });

    document.getElementById('modify-title').innerText = 'Modify ' + info.at;
}


function GetToken(info) {
    socket.send(JSON.stringify({ op: 1, id: info.id }));
}


function GetRowInformation(invoked) {
    const row = document.getElementById(invoked.parentNode.id);

    return { at: row.childNodes[1].innerText, 
             id: row.childNodes[3].innerText, 
             verification: row.childNodes[5].innerText }
}