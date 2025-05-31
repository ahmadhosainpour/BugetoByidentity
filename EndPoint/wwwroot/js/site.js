

function showCenteredModal() {
    document.getElementById('centeredModal').style.display = 'flex';
}

function closeCenteredModal() {
    document.getElementById('centeredModal').style.display = 'none';


}



function checkedbox() {
var checkbox = document.getElementById('selectedcheckbox');
        if (checkbox.checked) {
            console.log('1');
        }
        else { console.log(false); }
    } 





// Optional: Close the modal if clicking outside the modal box
document.getElementById('centeredModal').addEventListener('click', function (event) {
    if (event.target === this) {
        closeCenteredModal();
    }
});

function showCenteredModal2() {
    document.getElementById('centeredModal2').style.display = 'flex';
}

function closeCenteredModal2() {
    document.getElementById('centeredModal2').style.display = 'none';
}

// Optional: Close the modal if clicking outside the modal box
document.getElementById('centeredModal2').addEventListener('click', function (event) {
    if (event.target === this) {
        closeCenteredModal2();
    }
});

