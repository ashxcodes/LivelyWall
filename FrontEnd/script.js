function selectButtonClick() {
    console.log("~~~~~~~~~~~~~");
    window.chrome.webview.postMessage(1);
}

function setButtonClick() {
    console.log("~~~~~~~~~~~~~");
    const textField = document.getElementById("textField");
    window.chrome.webview.postMessage(2);
}

function recieveFileNameFromForm(data){
    console.log("Data received from WinForms:", data);;
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

    // You can process the dropped files here
    // For example, display the file names in the text field
    const textField = document.getElementById("textField");
    textField.value = "";

    for (const file of files) {
        textField.value += file.name + ", ";
    }

    // Remove the trailing comma and space
    textField.value = textField.value.slice(0, -2);
    const selectedFile = event.target.files[0];
    if (selectedFile) {
        const filePath = selectedFile.name;
        console.log(filePath);
    }
}

// function handleFileSelection(event) {
//     // Handle the selected file(s) from the file dialog
//     const selectedFiles = event.target.files;

//     const textField = document.getElementById("textField");
//     textField.value = "";

//     for (const file of selectedFiles) {
//         textField.value += file.name + ", ";
//     }

//     // Remove the trailing comma and space
//     textField.value = textField.value.slice(0, -2);
// }
function handleFileSelection(event) {
    const selectedFile = event.target.files[0];
    if (selectedFile) {
        const filePath = selectedFile.name;
        console.log(filePath);
    }
}


// Prevent default drag-and-drop behavior
document.addEventListener('dragover', function (e) {
    e.preventDefault();
});

document.addEventListener('drop', function (e) {
    e.preventDefault();
});
