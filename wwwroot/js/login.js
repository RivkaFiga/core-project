const url = '/TaskManager';

sessionStorage.setItem("token","f");
function Login() {
    sessionStorage.clear();
    var headers = new Headers();
    const name = document.getElementById('name').value.trim();
    const password = document.getElementById('password').value.trim();

    headers.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        UserName: name,
        Password: password
    })
    var requestOptions = {
        method: "POST",
        headers: headers,
        body: raw,
        redirect: "follow",
    };

    fetch(`${url}`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            if (result.includes("401")) {
                name.value = "";
                password.value = "";
                alert("not exist!!")
            } else {
                token = result;
                sessionStorage.setItem("name", name);
                sessionStorage.setItem("password", password);
                sessionStorage.setItem("token", token)
                location.href = "task.html";

            }
        }).catch((error) => alert("error", error));

}