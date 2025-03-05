
let loginData = [];

document.getElementById('login-form').addEventListener('submit', function(event) {
  event.preventDefault();

  let username = document.getElementById('username').value;
  let password = document.getElementById('password').value;

  loginData.push({ username: username, password: password });

  console.log(loginData);

  document.getElementById('username').value = '';
  document.getElementById('password').value = '';
});


/*boş bir web sitesinin sağ üstüne nasıl anlık olarak saat gösterecek bir kısım eklerim*/
function updateClock() {
    let now = new Date();
    let hours = now.getHours().toString().padStart(2, '0');
    let minutes = now.getMinutes().toString().padStart(2, '0');
    let seconds = now.getSeconds().toString().padStart(2, '0');
    document.getElementById('clock').textContent = `${hours}:${minutes}:${seconds}`;
}

setInterval(updateClock, 1000);

updateClock();

/*bir web sitesine klavyeden bir tuşa basıldığında bir formun görünürlüğünün değişmesini nasıl yapabilirim*/

let formsVisible = true; 

document.addEventListener('keydown', function(event) {
    /*kullanıcı siteye input veriyorsa h tuşunun işlevi gerçekleşmemeli*/ 
    if (document.activeElement.tagName === "INPUT") {
        return;
    }

    if (event.key === 'H' || event.key === 'h') {
        let forms = document.querySelectorAll('.login-container');
        forms.forEach(form => {
            form.style.display = formsVisible ? 'none' : 'block';
        });
        formsVisible = !formsVisible; 
    }
});