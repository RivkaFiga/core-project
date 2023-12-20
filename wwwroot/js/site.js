const uri = '/Task';
let tasks = [];
let token = sessionStorage.getItem("token");
getItems(token);

function getItems(token) {
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };

    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => displayItems(data))
        .catch(error => console.log('error', error));
}

function getItemById() {
    const id = document.getElementById('get-item').value;
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };
    fetch(`${uri}/${id}`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); } else { showItem(data); }
        })
        .catch(error => console.error('Unable to get items.', error));
}

function showItem(data) {
    const name = document.getElementById('name');
    const status = document.getElementById('status');
    name.innerText = "name: " + data.name;
    status.innerText = "is done: " + data.isDone;

}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const item = {
        id: 0,
        name: addNameTextbox.value.trim(),
        status: false,
        userId: 123
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            },
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            }
        }).then(() => { getItems(token) })
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-status').checked = item.status;
    document.getElementById('editForm').style.display = 'block';
    updateItem(item)
}

function updateItem(item1) {
    document.getElementById('save').onclick = () => {
        const item = {
            Id: item1.id,
            isDone: document.getElementById('edit-status').checked,
            Name: document.getElementById('edit-name').value.trim()
        };
        fetch(`${uri}/${item1.id}`, {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': "Bearer " + token
                },
                body: JSON.stringify(item)
            })
            .then(() => getItems(token))
            .catch(error => console.error('Unable to update item.', error));
        closeInput();
    }
    const close = document.getElementById('close')
    close.onclick= () => {
        closeInput();
    }
}


function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task kinds';
    document.getElementById('counter').innerText = `${itemCount} ${name} `;
}

function displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let statusCheckbox = document.createElement('input');
        statusCheckbox.type = 'checkbox';
        statusCheckbox.disabled = true;
        statusCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td0 = tr.insertCell(0);
        let textNodeId = document.createTextNode(item.id);
        td0.appendChild(textNodeId);

        let td1 = tr.insertCell(1);
        td1.appendChild(statusCheckbox);

        let td2 = tr.insertCell(2);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(3);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(4);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}

/// user ///
const url = '/User';

function getAllUsers() {
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");

    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow',
    };

    fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => displayUsers(data))
        .catch(error => { console.log('error', error), alert('Not Authorized!!') });

}

function displayUsers(data) {
    document.getElementById('manager').style.display = 'block';
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';
    data.forEach(user => {
        const button = document.createElement('button');

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${user.userId})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.userId})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(user.userId);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(user.userName);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(user.password);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

}

let flag = false;

function getMyUser() {
    if (flag)
        return;
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };
    fetch(`${url}/`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); } else { showUser(data); }
        }).then(flag = true)
        .catch(error => console.error('Unable to get items.', error));
}

function showUser(data) {
    const id = document.createElement('th');
    const name = document.createElement('th');
    const isAdmin = document.createElement('th');
    const password = document.createElement('th');
    id.innerHTML = data.id;
    name.innerHTML = data.name;
    isAdmin.innerHTML = data.isAdmin;
    password.innerHTML = data.password;
    const tbl = document.getElementById("tbl");
    tbl.append(id, name, isAdmin, password);
}

function addUser() {
    const addPassword = document.getElementById('add-password').value.trim();
    const addName = document.getElementById('add-user-name').value.trim();
    const user = {
        UserName: addName,
        Password: addPassword
    }
    fetch(`${url}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            },
            body: JSON.stringify(user)
        })
        .then((response) => {
            response.json()
        })
        .then(() => { getAllUsers() })
        .catch(() => alert("Not Authorized!!"));
}

function deleteUser(userId) {
    fetch(`${url}/${userId}?userId=${userId}`, {
        // http://localhost:5000/User/8?userId=8

            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
        })
        // .then(() => getAllUsers())
        .then(() => {
            getAllUsers()
            addName.value = ""
            addPassword.value = ""
        })
        .catch(error => console.log('Unable to delete user.', error));
}

function logOut() {
    location.href = "/index.html";
}