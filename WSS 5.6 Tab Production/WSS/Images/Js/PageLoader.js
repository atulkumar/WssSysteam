
function InitializeRequest(sender, args) {
    if (document.getElementById('ProgressDiv') != null)
        $get('ProgressDiv').style.display = 'block';
    else
        createContorl();
}

function EndRequest(sender, args) {
   
    if (document.getElementById('ProgressDiv') != null) {
        $get('ProgressDiv').style.display = 'none';
    }
    else
        createContorl();
}

function createContorl() {

    var parentDiv = document.createElement("div");
    parentDiv.setAttribute("id", "ProgressDiv");

    parentDiv.setAttribute("class", "ModalProgressContainer");
    parentDiv.setAttribute("id", "ProgressDiv");


    var innerContent = document.createElement("div");
    innerContent.setAttribute("id", "ProgressinnerContent");
    innerContent.setAttribute("class", "ModalProgressContent");

    var img = document.createElement("img");
    img.setAttribute("src", "../../Images/ajax-loader.gif");

    var textDiv = document.createElement("div");
    textDiv.innerHTML = 'Loading....';



    innerContent.appendChild(img);
    innerContent.appendChild(textDiv);

    parentDiv.appendChild(innerContent);
    document.body.appendChild(parentDiv);
    document.getElementById("ProgressDiv").className = 'ModalProgressContainer';
    document.getElementById("ProgressinnerContent").className = 'ModalProgressContent';


}