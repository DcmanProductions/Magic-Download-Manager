class DownloadFile {
    url = "";
    path = "";
    fName = "";
    fExt = "";
    constructor(url = "", path = "") {
        this.url = url;
        this.path = path;
        this.fName = require('path').basename(this.path);
        this.fExt = require('path').extname(this.fName);
    }
    get Path() {
        return this.path;
    }
    get FileName() {
        return this.fName;
    }
    get FileExtension() {
        return this.fExt;
    }
    get URL() {
        return this.url;
    }
    Download() {
        global.CurrentDownloadingFiles.push(this)
        global.CurrentDownloadingFiles.forEach(l => {
            console.log(l);
        })
    }
}