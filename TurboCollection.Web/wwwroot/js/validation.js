const form = document.getElementById("editExtraForm");

form.addEventListener("submit", (event) => {

    var fileName = $("#customFile").val().split("\\").pop();
    
    //alert(fileName);

    var number = document.getElementById("editExtraNumber");
    var errorNumber = document.getElementById('errorNumber');
    var errorPicture = document.getElementById('errorPicture');
    var select = document.getElementById("selectCollection");
    var selectValue = select.value;
    var numberValue = number.value;

    
    if (fileName == '') {
        errorPicture.textContent = "Please choose picture";
        event.preventDefault();
    }

    if (selectValue == 1 && (numberValue < 1 || numberValue > 50)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 2 && (numberValue < 51 || numberValue > 120)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 3 && (numberValue < 121 || numberValue > 190)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 4 && (numberValue < 191 || numberValue > 260)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 5 && (numberValue < 261 || numberValue > 330)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 6 && (numberValue < 331 || numberValue > 400)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 7 && (numberValue < 401 || numberValue > 470)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 8 && (numberValue < 471 || numberValue > 540)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 9 && (numberValue < 1 || numberValue > 70)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 10 && (numberValue < 71 || numberValue > 140)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 11 && (numberValue < 141 || numberValue > 210)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 12 && (numberValue < 1 || numberValue > 70)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
    if (selectValue == 13 && (numberValue < 71 || numberValue > 140)) {
        errorNumber.textContent = 'Number is out of collection range';
        event.preventDefault();
    }
});