let socket;

window.onload = function() {
    socket = new WebSocket("ws://localhost/bot");
    socket.onmessage = function(args) {
        
        const payload = JSON.parse(args.data);

        ShowResult(payload);
    };
};

function StartBot(data) {
    socket.send(JSON.stringify(data));
}

function ShowResult(data) {
    const alert = document.createElement('alert');
    alert.style = 'position: fixed; bottom: 0; margin-left: 14px;';
    if (data.succeeded) {
        alert.classList = 'alert alert-success';
        alert.innerHTML = '<strong>Success!</strong> ' + data.message;
    }
    else {
        alert.classList = 'alert alert-danger';
        alert.innerHTML = "<strong>Failed</strong> " + data.message;
    }
    
    document.body.appendChild(alert);

    setTimeout(RemoveAlert, 4000, alert);
}


function RemoveAlert(alertElement) {
    alertElement.style.opacity = 1;

    setInterval(function() 
    {
        if (alertElement.style.opacity == 0) {
            clearInterval();
            alertElement.remove();
            return;
        }

        alertElement.style.opacity -= 0.01;
    }, 10);
}