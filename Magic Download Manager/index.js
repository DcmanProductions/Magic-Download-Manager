const { app, BrowserWindow, Tray, Menu } = require('electron');
const icon = require('path').join(__dirname, 'assets', 'img', 'Icon.ico')
var tray = null;
var win = null;
function createWindow() {
    win = new BrowserWindow({
        width: 1280,
        height: 720,
        minWidth: 1280,
        minHeight: 600,
        webPreferences: {
            nodeIntegration: true,
            enableRemoteModule: true
        },
        icon: icon,
        frame: false,
    });

    win.setTitle('Magic Download Manager');
    win.loadFile('assets/html/index.html');
}

function createTray() {
    tray = new Tray(icon);
    tray.addListener('double-click', () => {
        win.show();
    })
    tray.setContextMenu(Menu.buildFromTemplate(
        [
            {
                label: "Magic Download Manager",
                enabled:false
            },
            {
                label: "Open MagicDM",
                click: () => { win.show() }
            },
            {
                label: "Quit MagicDM",
                click: () => { app.quit() }
            }
        ]
    ))
}

app.whenReady().then(createWindow).then(createTray);

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
        createWindow();
    }
});