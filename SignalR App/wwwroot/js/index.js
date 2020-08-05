var chatterName = 'Visitor';
var dialogEl = document.getElementById('chatDialog');
//Initialize SignalR Client
var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();
connection.on('ReceiveMessage', renderMessage);
connection.onclose(function () {
    onDisconnected();
    console.log('Reconnecting in 5 seconds...');
    setTimeout(startConnection, 5000);
});

function startConnection() {
    connection.start()
        .then(onConnected)
        .catch(function (err) {
            console.error(err);
        });
}
function showChatDialog() {
    dialogEl.style.display = 'block';
}
function onDisconnected() {
    dialogEl.classList.add('disconnected');
}

function onConnected() {
    dialogEl.classList.remove()('disconnected');
    var messageTextBoxEl = document.getElementById('messageTextbox');
    messageTextBoxEl.focus();
}



function sendMessage(text) {
    if (text && text.length) {
        connection.invoke('SendMessage', chatterName, text);
    }
}
function ready() {
    setTimeout(showChatDialog, 750);
    var charFormElement = document.getElementById('chatForm');
    charFormElement.addEventListener('submit', function (e) {
        e.preventDefault();
        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);

    });
    var welcomePanelEl = document.getElementById('chatWelcomePanel');
    welcomePanelEl.addEventListener('submit', function(e) {
        e.preventDefault();
        var name = e.target[0].value;
        if (name && name.length) {
            welcomePanelEl.style.display = 'none';
            chatterName = name;
            startConnection();
        }
    });
}

function renderMessage(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'message';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}


document.addEventListener('DOMContentLoaded', ready);
