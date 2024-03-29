function my_function(message) {
    console.log("from utilities: " + message);
}

function dotnetStaticInvocation() {
    DotNet.invokeMethodAsync("BlazorMovies.Client", "GetCurrentCount")
        .then(result => {
            console.log("count from javascript " + result);
        });
}

function dotnetInstanceInvocation(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}


function initializeInactivityTimer(dotnetHelper) {
    var timer;
    document.onmousemove = resetTimer;
    document.onkeydown = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 1000*60*5);
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }

}


function setInLocalStorage(key, value) {
    localStorage[key] = value;
}

function getFromLocalStorage(key) {
    return localStorage[key];
}