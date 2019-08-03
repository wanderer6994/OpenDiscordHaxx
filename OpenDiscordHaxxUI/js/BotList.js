let socket;

window.onload = function() {
    socket = new WebSocket("ws://localhost/bot/list");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        const table = document.getElementById('bot-list');

        let html = '';

        payload.forEach(bot => {
            let row = '<tr style="border-style: hidden !important">\n';
            row += "<td>" + bot.at + '</td>\n';
            row += "<td>" + bot.id + '</td>\n';
            row += "<td>" + bot.verification + '</td>\n';
            row += "</tr>";

            html += row;
        });

        table.innerHTML = html;
    };
    socket.onerror = function() {
        document.getElementById('unreachable').style.display = "block";
        document.getElementById('bot-list-form').style.display = "none";
    }
};