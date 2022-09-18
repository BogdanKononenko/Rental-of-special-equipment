var tokenKey = "accessToken";

// отпавка запроса к контроллеру AccountController для получения токена
async function getTokenAsync() {

    if (document.getElementById("emailLogin").value == ""){
        alert("Введите email");
    }
    else if (document.getElementById("passwordLogin").value == ""){
        alert("Введите пароль" )
    }
    else {
        // получаем данные формы и фомируем объект для отправки
        const formData = new FormData();
        formData.append("grant_type", "password");
        formData.append("username", document.getElementById("emailLogin").value);
        formData.append("password", document.getElementById("passwordLogin").value);

        // отправляет запрос и получаем ответ
        const response = await fetch("/token", {
            method: "POST",
            headers: {"Accept": "application/json"},
            body: formData
        });
        // получаем данные 
        const data = await response.json();

        // если запрос прошел нормально
        if (response.ok === true) {

            // изменяем содержимое и видимость блоков на странице
            document.getElementById("userName").innerText = data.username;
            document.getElementById("userInfo").style.display = "block";
            document.getElementById("loginForm").style.display = "none";
            // сохраняем в хранилище sessionStorage токен доступа
            sessionStorage.setItem(tokenKey, data.access_token);
            console.log(data.access_token);
            console.log(tokenKey);
        }
        else {
            // если произошла ошибка, из errorText получаем текст ошибки
            console.log("Error: ", response.status, data.errorText);
        }
    }
};


async function handlingRequest(handleUrl, clickUrl)
{
    const token = sessionStorage.getItem(tokenKey);

    const response = await fetch(handleUrl, {
        method: "GET",
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + token  // передача токена в заголовке
        }
    });

    if (response.ok === true)
    {
        console.log("Status: ", response.status);
        open(clickUrl, "_self");
    }
    else
    {
        if (handleUrl == '/Home/CheckingForAdmin')
        {
            alert("Удалять может только админ. Если вы админ войдите в систему под своей учеткой");    
        }
        else
        {
            alert("Размещать объявления могут только авторизованные пользователи." +
                "Пожалуйста,войдите в систему");
        }
        
    }
};

window.addEventListener("DOMContentLoaded", function() {
    [].forEach.call( document.querySelectorAll('.tel'), function(input) {
        var keyCode;
        function mask(event) {
            event.keyCode && (keyCode = event.keyCode);
            var pos = this.selectionStart;
            if (pos < 3) event.preventDefault();
            var matrix = "+7 (___) ___ ____",
                i = 0,
                def = matrix.replace(/\D/g, ""),
                val = this.value.replace(/\D/g, ""),
                new_value = matrix.replace(/[_\d]/g, function(a) {
                    return i < val.length ? val.charAt(i++) || def.charAt(i) : a
                });
            i = new_value.indexOf("_");
            if (i != -1) {
                i < 5 && (i = 3);
                new_value = new_value.slice(0, i)
            }
            var reg = matrix.substr(0, this.value.length).replace(/_+/g,
                function(a) {
                    return "\\d{1," + a.length + "}"
                }).replace(/[+()]/g, "\\$&");
            reg = new RegExp("^" + reg + "$");
            if (!reg.test(this.value) || this.value.length < 5 || keyCode > 47 && keyCode < 58) this.value = new_value;
            if (event.type == "blur" && this.value.length < 5)  this.value = ""
        }

        input.addEventListener("input", mask, false);
        input.addEventListener("focus", mask, false);
        input.addEventListener("blur", mask, false);
        input.addEventListener("keydown", mask, false)

    });

});


    
