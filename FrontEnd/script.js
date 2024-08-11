const SELECT_ACTION = 111;
const SET_ACTION = 222;
const STOP_ACTION = 333;
function selectButtonClick() {
    window.chrome.webview.postMessage(SELECT_ACTION);
}

async function setButtonClick() {
    await sendPlaybackSpeed();
    window.chrome.webview.postMessage(SET_ACTION);
}

function stopButtonClick() {
    showSnackbar("LiveWallpaper Stopped","success");
    window.chrome.webview.postMessage(STOP_ACTION);
}

function recieveDataFromForm(encodedString) {
    let data = '';
    if (encodedString) {
        data = atob(encodedString);
    }
    if (document.getElementById("textField")) {
        document.getElementById("textField").value = data;
    }
}

async function sendPlaybackSpeed(){
    if (document.getElementById("speedField")) {
        const playback = Number(document.getElementById("speedField").value);
        await window.chrome.webview.postMessage(playback);
    }
}

function allowDrop(event) {
    event.preventDefault();
    document.getElementById("dropArea").style.border = "2px dashed #39f";
}

function handleDrop(event) {
    event.preventDefault();
    const files = event.dataTransfer.files;
    if (files.length > 1) {
        showSnackbar("Drop one file.","error");
        return;
    }
    if (!isVideoFile(files[0])) {
        showSnackbar("Drop only video files.","error");
        return;
    }
    populateTextField(files[0]);
}


function isVideoFile(file) {
    return file.type.startsWith("video/");
}

function populateTextField(file) {
    if (file.name == "") {
        showSnackbar("Invalid file name","error");
    }
    const textField = document.getElementById("textField");
    textField.value = "";
    textField.value = file.name;
}

function setInitialState(WallPaperDetails) {
    let details = JSON.parse(WallPaperDetails)
    document.getElementById("textField").value = atob(details.FilePath);
    document.getElementById("speedField").value = details.PlaybackSpeed;
    handleEvent("setbutton", "success");
}

function handleEvent(btnName, event) {
    const buttonName = btnName.toLowerCase();
    const eventName = event.toLowerCase();

    if (eventName === "success") {
        switch (buttonName) {
            case "setbutton":
                toggleButtonState("setbutton", false);
                toggleButtonState("stopbutton", true);
                document.getElementById("speedField").disabled = true;
                break;

            case "stopbutton":
                toggleButtonState("stopbutton", false);
                toggleButtonState("setbutton", true);   
                document.getElementById("textField").value = '';
                document.getElementById("speedField").disabled = false;
                break;

            default:
                break;
        }
    }
}

function toggleButtonState(btnName, isEnabled) {
    const myButton = document.getElementById(btnName);
    if (myButton) {
        myButton.disabled = !isEnabled;
    }
}

function showSnackbar(message, type) {
    var snackbar = document.getElementById("snackbar");
    snackbar.innerHTML = message;
    switch (type) {
        case "error":
            snackbar.style.backgroundColor = '#ac3232';
            break;

        case "success":
            snackbar.style.backgroundColor = '#b2e2b4';
            break;

        default:
            break;
    }
    snackbar.className = "show";

    setTimeout(() => { 
        snackbar.className = snackbar.className.replace("show", ""); 
        snackbar.style.backgroundColor = '';
    }, 2200);
}