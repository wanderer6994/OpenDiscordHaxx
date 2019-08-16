function FatalError(errorMsg) {

    HideElements();    

    const error = document.createElement('p');
    error.classList = 'dark-title';
    error.style.fontSize = '50px';
    error.style.textAlign = 'center';
    error.innerHTML = errorMsg;
    
    document.body.appendChild(error);
}


function ServerUnreachable() {
    FatalError('The server is unreachable,<br>try again later.');
}


function HideElements() {
    document.body.childNodes.forEach(element => {
        if (element.id != 'odh-nav') {
            //some elements dont have a style property
            if (typeof element.style !== 'undefined')
                element.style.display = 'none';
        }
    });
}