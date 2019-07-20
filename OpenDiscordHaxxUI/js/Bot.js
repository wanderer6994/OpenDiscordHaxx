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
    const alert = document.createElement('alert');
    alert.style = 'position: fixed; bottom: 0; margin-left: 14px;';
    if (http.status == 200) {
        alert.classList = 'alert alert-success';
        alert.innerHTML = '<strong>Success!</strong> bot should be starting shortly';
    }
    else {
        alert.classList = 'alert alert-danger';
        alert.innerHTML = "<strong>Failed</strong> " + http.responseText;
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