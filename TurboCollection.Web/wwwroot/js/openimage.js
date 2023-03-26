const output = document.querySelector("output");
const input = document.querySelector("#customFile");
let imagesArray;

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
});

input.addEventListener("change", () => {
    const file = input.files
    imagesArray = file[0]
    let image = `<div class="image">
                             <img style='height: 250px;' src="${URL.createObjectURL(imagesArray)}" alt="image">
                         </div>`
    output.innerHTML = image
})