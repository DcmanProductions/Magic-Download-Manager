const { remote } = require('electron');

var win = remote.getCurrentWindow();
function CloseApplication() {
    win.close();
}
function MaximizeApplication() {
    if (!win.isMaximized())
        win.maximize();
    else
        win.unmaximize();
}
function MinimizeApplication() {
    win.minimize();
}

function OpenDevTools() {
    win.webContents.openDevTools()
}