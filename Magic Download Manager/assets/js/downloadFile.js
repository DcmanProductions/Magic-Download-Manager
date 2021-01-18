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

    get HTML() {
        return `
        <div class="download-item">
            <div class="download-title">
                test
            </div>
            <div class="download-information">
                250Mb / 5Gb (2.45Mbs) - 25%
            </div>
            <div class="download-progress">
                <progress class="download-actual-progress" style="display:none" value=".0"></progress>
                <div class="download-progress-bar">
                    <div class="download-progress-done">
                    </div>
                </div>
            </div>
            <div class="download-open-folder">

            </div>
            <div class="download-delete">

            </div>
    </div>
    `;
    }

}