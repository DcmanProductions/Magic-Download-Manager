var dlQueue = new Array();
var autoClear = false;

function AddDownload(dlFile) {
    dlQueue.push(dlFile);
    document.getElementById('download-list').innerHTML += dlFile.HTML;
}

function ClearDownloads() {
    document.querySelectorAll('.download-item-completed').forEach(item => {
        item.parentElement.removeChild(item);
    })
}

function ToggleAutoClear() {
    let cl = document.getElementById('clear-download-btn');
    cl.classList.toggle('download-list-controls-toggled');
    autoClear = cl.classList.contains('download-list-controls-toggled');
    cl.innerHTML = autoClear ? "AUTO" : "CLEAR";
}

function UpdateDownloadQueue() {
    let dList = document.getElementById('download-list');
    dList.innerHTML = "";
    dlQueue.forEach(n => {
        dList.innerHTML += n.HTML;
        console.log(n.HTML)
    })
}

setInterval(() => {
    let cl = document.getElementById('clear-download-btn');
    cl.innerHTML = autoClear ? "AUTO" : "CLEAR";
    let downloadItems = document.querySelectorAll('.download-item');
    downloadItems.forEach(item => {
        let progress_bar = item.querySelector('.download-actual-progress');
        let progress = item.querySelector('.download-progress-done');
        let value = Math.floor(progress_bar.value * 100);
        if (value >= 3)
            progress.style.opacity = 1;
        if (value >= 99) {
            progress.innerHTML = `Completed!`;
            progress.style.width = `100%`;
            progress_bar.value += 1;
            item.classList.add('download-item-completed');
            item.classList.remove('download-item');
            if (autoClear)
                setTimeout(() => {
                    item.parentElement.removeChild(item);
                }, 1500)
        } else {
            progress_bar.value += .1;
            progress.style.width = `${value}%`;
            progress.innerHTML = `${value}%`;
        }
    })

}, 700)