function selectButtonClick() {
    window.chrome.webview.postMessage(1);
}

function setButtonClick() {
    const textField = document.getElementById("textField");
    window.chrome.webview.postMessage(2);
}
function stopButtonClick(){
    window.chrome.webview.postMessage(3)
}

function recieveFileNameFromForm(data){
    if(document.getElementById("textField")){
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
    const textField = document.getElementById("textField");
    textField.value = "";

    for (const file of files) {
        textField.value += file.name + ", ";
    }

    textField.value = textField.value.slice(0, -2);
    const selectedFile = event.target.files[0];
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
