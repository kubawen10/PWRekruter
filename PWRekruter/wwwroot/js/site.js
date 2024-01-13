// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showModal(id) {
    var modal = document.getElementById('myModal');
    modal.setAttribute('data-prefid', id);
    document.getElementById('myModal').style.display = 'block';
}

function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    location.reload();
}

function displayModalMessage(message) {
    clearModalContent();
    var modalMessageContent = document.getElementById('modalContent');
    modalMessageContent.innerText = message;
    document.getElementById('myModalMessage').style.display = 'block';
}

function clearModalContent() {
    var modalMessageContent = document.getElementById('modalContent');
    modalMessageContent.innerText = '';
}

function confirmAction(aplikacjaId) {

        var prefId = document.getElementById('myModal').getAttribute('data-prefid');

        prefId = parseInt(prefId);
    
        var selectedOption = document.querySelector('input[name="option"]:checked');

        if (selectedOption) {
            var optionValue = selectedOption.value;


            fetch('/Aplikacje/ChangeAppResult?id=' + prefId + '&option=' + optionValue, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then(response => response.text())
                .then(data => {
                    displayModalMessage(data);
                });
        } else {
            alert('Wybierz opcję przed zatwierdzeniem.');
        }
    
}
