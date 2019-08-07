function ShowToast(success, message) {
    const alert = document.createElement('alert');
    alert.style = 'position: fixed; bottom: 0; margin-left: 14px;';
    if (success) {
        alert.classList = 'alert alert-success';
        alert.innerHTML = '<strong>Success!</strong> ' + message;
    }
    else {
        alert.classList = 'alert alert-danger';
        alert.innerHTML = "<strong>Failed</strong> " + message;
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