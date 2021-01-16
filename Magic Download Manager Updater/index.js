let { app, BrowserWindow } = require('electron');
let path = require('path');
function createWindow() {
    const win = new BrowserWindow({
        height: 540,
        width: 960,
        webPreferences: {
            nodeIntegration: true,
            enableRemoteModule: true
        }, 
        frame:false,
        minimizable: false,
        resizable: false,
        icon: require('path').join(__dirname, 'assets', 'img', 'Icon.ico'),
        
    });
    win.loadFile(path.join(__dirname, 'assets', 'html', 'index.html'));
    // win.loadFile('assets/html/index.html');
    win.setTitle('Magic Download Manager');
    win.menuBarVisible = false;
    // win.webContents.openDevTools();
}

app.whenReady().then(createWindow);

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