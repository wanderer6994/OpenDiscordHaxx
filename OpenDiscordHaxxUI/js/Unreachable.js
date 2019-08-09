function ServerUnreachable() {
    HideElements();
    
    const unreachable = document.createElement('p');
    unreachable.classList = 'dark-title';
    unreachable.style.fontSize = '50px';
    unreachable.style.textAlign = 'center';
    unreachable.innerHTML = 'The server is unreachable,<br>try again later.';
    
    document.body.appendChild(unreachable);
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