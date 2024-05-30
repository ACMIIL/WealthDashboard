$(document).ready(function () {
    //var data = Html.Raw(Json.Serialize(userData));

   
    var data = userData;
    var result = JSON.parse(data);
    var divElement1 = document.getElementById("username");
    var divElement = document.getElementById("username1");

    divElement1.innerHTML = result.FirstName + " " + result.LastName;
    divElement.innerHTML1 = result.FirstName + " " + result.LastName;
});