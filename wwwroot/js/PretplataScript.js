var pretplataButton = document.getElementById('pretplataButton');
var pretplataInput = document.getElementById('pretplataInput');
function pretplata() {
    if (pretplataInput.getAttribute('value') == 1)
    {
        pretplataButton.classList.remove('btn-secondary');
        pretplataButton.classList.add('btn-success');
        pretplataInput.setAttribute('value', 0);
        pretplataButton.innerHTML = 'Pretplati Se';
    }
    else
    {
        pretplataButton.classList.remove('btn-success');
        pretplataButton.classList.add('btn-secondary');
        pretplataInput.setAttribute('value', 1);
        pretplataButton.innerHTML = 'Odjavi Se';
    }
}