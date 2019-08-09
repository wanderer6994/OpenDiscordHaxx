let socket;

const CheckerOpcode = {
    Started: 0,
    BotChecked: 1,
    Done: 2
}


window.onload = function() {

    document.getElementById('checker-results').value = '';

    socket = new WebSocket("ws://localhost/bot/checker");
    socket.onmessage = function(args) {

        const payload = JSON.parse(args.data);
        
        switch (payload.op) {
            case CheckerOpcode.Started:
                UpdateProgress(payload.progress);

                ShowToast(ToastType.Info, 'Checker has started');
                break;
            case CheckerOpcode.BotChecked:
                UpdateProgress(payload.progress);
                const results = document.getElementById('checker-results');

                results.value = results.value + payload.bot.at + ' is ' + (payload.valid ? 'valid!' : 'invalid :/') + '\n';
                break;
            case CheckerOpcode.Done:
                ShowToast(ToastType.Success, 'Checker has finished');
                break;
        }
    }
    socket.onerror = function() { ServerUnreachable() };
}


function UpdateProgress(progress) {
    document.getElementById('progress').innerText = 'Valid: ' + progress.valid + ' | Invalid: ' + progress.invalid + ' | ' + (progress.valid + progress.invalid) + '/' + progress.total;
}