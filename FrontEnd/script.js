function selectButtonClick() {
    window.chrome.webview.postMessage(1);
}

function setButtonClick() {
    window.chrome.webview.postMessage(2);
}

function stopButtonClick() {
    showSnackbar("LiveWallpaper stopped","success");
    window.chrome.webview.postMessage(3);
}

function recieveFileNameFromForm(data) {
    if (document.getElementById("textField")) {
        document.getElementById("textField").value = data;
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

function successEvent(btnName, event) {
    const buttonName = btnName.toLowerCase();
    const eventName = event.toLowerCase();

    if (eventName === "success") {
        switch (buttonName) {
            case "setbutton":
                toggleButtonState("setbutton", false);
                toggleButtonState("stopbutton", true);
                break;

            case "stopbutton":
                toggleButtonState("stopbutton", false);
                toggleButtonState("setbutton", true);
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
            snackbar.style.backgroundColor = '#ac3232'; // Red color for error
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

