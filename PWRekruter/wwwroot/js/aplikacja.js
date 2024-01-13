function changeIcon(button) {
    if (!isDragEnabled) {
        var tr = button.closest('tr');
        var icon = button.querySelector('i');

        if (tr.classList.contains('collapsed')) {
            icon.classList.remove('bi-chevron-up');
            icon.classList.add('bi-chevron-down');
        } else {
            icon.classList.remove('bi-chevron-down');
            icon.classList.add('bi-chevron-up');
        }
    }
    
}

var isDragEnabled = false;
var prefOrder = {};

function sendReorderRequest() {
    let reorderRequest = {
        IdAplikacji: applicationId,
        Priorytety: prefOrder
    };
    let noChanges = true;
    for (const [key, value] of Object.entries(prefOrder)) {
        if (key != value) {
            noChanges = false; 
        }
    }
    if (noChanges) {
        enableAccordion();
        return;
    }

    fetch('/Kandydaci/ReorderPrefs', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: JSON.stringify(reorderRequest)
    })
        .then(() => {
            location.reload();
        });
    
}

function enableAccordion() {
    document.querySelectorAll('#prefTable tbody #visible').forEach(row => {
        row.setAttribute('data-target', row.getAttribute('data-target').slice(0, -1));
    });
}


function addDragEvents(row) {
    row.addEventListener('dragstart', handleDragStart);
    row.addEventListener('dragover', handleDragOver);
    row.addEventListener('drop', handleDrop);
    row.addEventListener('dragend', handleDragEnd);
}

function removeDragEvents(row) {
    row.removeEventListener('dragstart', handleDragStart);
    row.removeEventListener('dragover', handleDragOver);
    row.removeEventListener('drop', handleDrop);
    row.removeEventListener('dragend', handleDragEnd);
}

function handleDragStart(e) {
    this.style.opacity = 0.5;
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML);
}

function handleDragOver(e) {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
}

function handleDrop(e) {
    e.stopPropagation();
    if (dragSrcEl !== this) {
        dragSrcEl.innerHTML = this.innerHTML;
        this.innerHTML = e.dataTransfer.getData('text/html');
    }
    return false;
}

function handleDragEnd(e) {
    this.style.opacity = "";
}

var dragSrcEl = null;

function handleDragStart(e) {
    dragSrcEl = this;
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML);
}
function resetOrder() {
    document.querySelectorAll('#prefTable tbody #visible').forEach((row, index) => {
        var numberCell = row.querySelector('.pref');
        if (numberCell) {
            numberCell.textContent = index + 1;
        }
    });
}

function saveInitialOrder() {
    document.querySelectorAll('#prefTable tbody #visible').forEach((row, index) => {
        var numberCell = row.querySelector('.pref');
        if (numberCell) {
            prefOrder[numberCell.textContent] = -1;
        }
    });
}
function updateOrder() {
    document.querySelectorAll('#prefTable tbody #visible').forEach((row, index) => {
        var numberCell = row.querySelector('.pref');
        if (numberCell) {
            prefOrder[index + 1] = numberCell.textContent;
        }
    });
}

function toggleDraggableRows() {
    document.querySelectorAll('#prefTable tbody #visible').forEach(row => {
        var prefElement = row.querySelector('.pref');
        if (isDragEnabled) {
            // Enable draggable and add the icon
            row.setAttribute('draggable', true);
            if (prefElement) {
                prefElement.innerHTML = '<i class="bi bi-arrows-expand"></i> ' + prefElement.innerHTML;
            }
            addDragEvents(row);
        } else {
            // Disable draggable and remove the icon
            row.removeAttribute('draggable');
            if (prefElement) {
                prefElement.innerHTML = prefElement.innerHTML.split(' ')[3]
            }

            removeDragEvents(row);
        }
    });
}

