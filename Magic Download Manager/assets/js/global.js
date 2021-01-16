let app = (require('electron').app || require('electron').remote.app);
const path = require('path');
const fs = require('fs');

const RootDirectory = path.join(app.getPath('appData'), "Chase Labs", "Magic Download Manager");
const ApplicationDirectory = path.join(RootDirectory, "bin");
const SettingsDirectory = path.join(RootDirectory, "configuration");
const LogDirectory = path.join(RootDirectory, "logs");
const LogFile = path.join(LogDirectory, "latest.log");
const SettingsFile = path.join(SettingsDirectory, "settings.json");


var CurrentDownloadingFiles = Array.prototype;
var DownloadLocation = app.getPath('downloads');
var PreAllocateStorage = true;
var MaxSplitCount = 16;
var MaxConcurrentDownloads = 4;

function ParseBoolean(value = "false") {
    return value.toLowerCase() == "true"
}
function Init() {
    if (!fs.existsSync(ApplicationDirectory)) {
        fs.mkdirSync(ApplicationDirectory, { recursive: true })
    }
    if (!fs.existsSync(LogDirectory)) {
        fs.mkdirSync(LogDirectory, { recursive: true })
    }
    if (!fs.existsSync(SettingsDirectory)) {
        fs.mkdirSync(SettingsDirectory, { recursive: true })
    }
}