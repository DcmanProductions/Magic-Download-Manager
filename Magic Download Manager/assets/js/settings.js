let DownloadLocationDisplay = document.getElementById('DownloadLocationDisplay');
let DownloadLocationSelector = document.getElementById('DownloadLocationSelector');
let DownloadLocationExplorer = document.getElementById('DownloadLocationExplorer');


let defaultSettings = {
    Downloads: {
        DownloadLocation: DownloadLocation + "",
        PreAllocateStorage: PreAllocateStorage,
        MaxSplitCount: MaxSplitCount,
        MaxConcurrentDownloads: MaxConcurrentDownloads
    },
    Account: {
    },
    Application: {
    }
}

DownloadLocationDisplay.onclick = e => {
    require('electron').shell.openPath(DownloadLocation);
}

Array.from($('input'), e => {
    console.log(e.id);
    $(`#${e.id}`).on('change', () => {
        SaveSettings();
    })
})

let MaxSplitElement = $('#max-split-count')[0];
let PreAllocateElement = $('#pre-allocate-storage')[0];
let MaxConcurrentDownloadElement = $('#max-concurrent-downloads')[0];


DownloadLocationSelector.onclick = e => {
    DownloadLocationExplorer.click();
}
DownloadLocationExplorer.onchange = e => {
    let path = require('path').dirname(e.target.files[0].path + "");
    DownloadLocationDisplay.value = path;
    SaveSettings();
}

LoadSettings();
function SaveSettings() {
    Init();

    DownloadLocation = DownloadLocationDisplay.value;
    PreAllocateStorage = PreAllocateElement.checked;
    MaxSplitCount = Number.parseInt(MaxSplitElement.value);
    MaxConcurrentDownloads = Number.parseInt(MaxConcurrentDownloadElement.value);

    let settings = {
        Downloads: {
            DownloadLocation: DownloadLocation,
            PreAllocateStorage: PreAllocateStorage,
            MaxSplitCount: MaxSplitCount,
            MaxConcurrentDownloads: MaxConcurrentDownloads
        },
        Account: {
        },
        Application: {
        }
    }

    console.log(settings);
    fs.writeFileSync(SettingsFile, JSON.stringify(settings), {
        flag: "w+"
    });
    LoadSettings();
}

function LoadSettings() {
    let settings = JSON.parse(fs.readFileSync(SettingsFile));
    let split = Number.parseInt(settings.Downloads.MaxSplitCount);
    let connc = Number.parseInt(settings.Downloads.MaxConcurrentDownloads);
    let pre = settings.Downloads.PreAllocateStorage;
    MaxSplitElement.value = split < 2 ? 2 : split;
    MaxConcurrentDownloadElement.value = connc < 1 ? 1 : connc;
    PreAllocateElement.checked = pre;
    DownloadLocation = (settings.Downloads.DownloadLocation == "" || settings.Downloads.DownloadLocation == null) ? app.getPath('downloads') : settings.Downloads.DownloadLocation;
    DownloadLocationDisplay.value = DownloadLocation;
    return settings;
}