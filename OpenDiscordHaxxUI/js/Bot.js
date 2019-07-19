function SendData(jsonData) {
    const http = new XMLHttpRequest();
    http.onreadystatechange = function() {
        if (http.readyState == 4)
            Callback(http);
    }
    http.open("POST", "http://localhost", true);
    http.send(jsonData);
}


function Callback(http) {
    const successAlert = `
    <div class="alert alert-success" style="position: fixed; bottom: 0; margin-left: 14px;">
    <strong>Success!</strong> bot should be starting shortly
    </div>`;
    const failedAlert = `
    <div class="alert alert-danger" style="position: fixed; bottom: 0; margin-left: 14px;">
    <strong>Failed</strong> an error occured
    </div>`;
    
    const alert = document.createElement('alert');
    if (http.status == 200)
        alert.innerHTML = successAlert;
    else
        alert.innerHTML = failedAlert;
    
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