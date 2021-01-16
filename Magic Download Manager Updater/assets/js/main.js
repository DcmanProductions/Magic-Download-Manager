const progress = document.getElementById("progress-done");
const progress_bar = document.getElementById("actual-progress");
const uaup = require('uaup-js');

setInterval(() => {
    let value = Math.floor(progress_bar.value * 100);
    if (value >= 10)
        progress.style.opacity = 1;
    progress.style.width = `${value}%`;
    progress.innerHTML = `${value}%`;
}, 100)

uaup.Update({
    progressBar: progress_bar,
    gitRepo: "Magic Download Manager"
});