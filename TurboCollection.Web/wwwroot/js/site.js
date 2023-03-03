// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let checkNumber = 1;
let itemId = 0;
let turboDiv;

const contextMenu = document.querySelector(".wrapper1");
const notdefined = document.querySelector("#notdefined");
const present = document.querySelector("#present");
const absent = document.querySelector("#absent");

document.addEventListener("click", (e) => {
    if (contextMenu.style.visibility == "visible") {
        contextMenu.style.visibility = "hidden";

        notdefined.style.visibility = "hidden";
        absent.style.visibility = "hidden";
        present.style.visibility = "hidden";
    }
});

function myFunction(element) {
    setTimeout(function () {
        checkNumber = element.getAttribute("status");
        itemId = element.getAttribute("itemid");
        //turboDiv = element.getAttribute("" );
        turboDiv = document.querySelector("#turbo" + itemId);
        
        console.log(checkNumber);

        var rect = element.getBoundingClientRect();
        let x = rect.right;
        let y = element.offsetTop;
        winWidth = window.innerWidth,
            winHeight = window.innerHeight,
            cmWidth = contextMenu.offsetWidth;
        cmHeight = contextMenu.offsetHeight;

        x = x > winWidth - cmWidth ? winWidth - cmWidth : x;

        x = Math.round(x);
        y = Math.round(y);
        contextMenu.style.left = `${x}px`;
        contextMenu.style.top = `${y}px`;

        contextMenu.style.visibility = "visible";

        $.ajax({
            type: 'POST',
            url: '/collection/getstatus',
            //url: 'api/collection/getstatus',
            data: { "privateTurboItemId": itemId},
            dataType: "text"
        })
            .done(function (result) {
                //console.log(result);
                if (result == '1') {
                    notdefined.style.visibility = "visible";
                    absent.style.visibility = "hidden";
                    present.style.visibility = "hidden";
                }
                if (result == '2') {
                    notdefined.style.visibility = "hidden";
                    absent.style.visibility = "visible";
                    present.style.visibility = "hidden";
                }
                if (result == '3') {
                    notdefined.style.visibility = "hidden";
                    absent.style.visibility = "hidden";
                    present.style.visibility = "visible";
                }
            });
    }, 30);
}

function clickingStatus(statusId) {
    setTimeout(function () {
    $.ajax({
        type: 'POST',
        //contentType: "application/json",
        //url: 'api/collection/getstatus',
        url: '/collection/setstatus',
        data: { "privateTurboItemId": itemId, "statusId": statusId },

        //data: {privateTurboItemId: "5", statusId: "1" },
        //data: { sortOrder: "aaa", searchString: "bbb" },
        //data: { sortOrder: 5, searchString: 7 },
        dataType: "text"
    })
        .done(function (result) {
            if (statusId == 1) turboDiv.style.background = "#eee";
            if (statusId == 2) turboDiv.style.background = "#FFC0CB";
            if (statusId == 3) turboDiv.style.background = "#00FF7F";
        })
    }, 30);
}
