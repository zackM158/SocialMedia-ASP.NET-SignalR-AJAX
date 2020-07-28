var mutualButton = document.querySelector("#mutualButton");
var allFriends = document.querySelector("#allFriends");
var mutualFriends = document.querySelector("#mutualFriends");
var totalMutual = document.querySelector("#totalMutual");


var toggleMutualFriends = function () {
    if (allFriends.style.display === "none") {
        allFriends.style.display = "block";
        mutualFriends.style.display = "none";
        mutualButton.value = "Mutual Friends: " + totalMutual.innerHTML; 
    } else {
        allFriends.style.display = "none";
        mutualFriends.style.display = "block";
        mutualButton.value = "All Friends";
    }
}

if (mutualButton != null) {
    mutualButton.addEventListener("click", toggleMutualFriends, false);
}