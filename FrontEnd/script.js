function selectButtonClick() {
    console.log("~~~~~~~~~~~~~");
    window.chrome.webview.postMessage(1);
}
function setButtonClick() {
    console.log("~~~~~~~~~~~~~");
    window.chrome.webview.postMessage(2);
}
function recieveFileNameFromForm(data){
    JSON.stringify(data);
    console.log("Data received from WinForms:", data);;
    if(document.getElementById("textField")){
        document.getElementById("textField").value = data;
    }
}