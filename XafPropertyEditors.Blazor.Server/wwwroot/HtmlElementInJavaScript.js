function createTextboxWithBlackBackground(divId) {
    // Find the div by ID

    //alert(divId);
    var div = document.getElementById(divId);
    if (!div) {
        console.error("Div not found:", divId);
        return;
    }
    // Create a new textbox (input element of type 'text')
    var textbox = document.createElement('input');
    textbox.type = 'text';

    // Set the textbox's background color to black and text color to white for visibility
    textbox.style.backgroundColor = 'black';
    textbox.style.color = 'white';

    // Append the textbox to the div
    div.appendChild(textbox);
}
function createCanvas()
{
    var id = document.getElementById("drawflow");
    const editor = new Drawflow(id);
    editor.start();
    alert("Editor started");
}