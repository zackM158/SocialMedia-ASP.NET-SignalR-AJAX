var stauses = document.querySelector("#statuses");

function checkForNewStatuses() {
    $.ajax({
        type: "GET",
        url: '/Home/GetNewStatuses',
        success: function (response) {
            if (response.trim() != '') {
                console.log(response);
                stauses.insertAdjacentHTML("afterbegin", response);
            }
        }
    });
}

var interval = setInterval(checkForNewStatuses, 10000);