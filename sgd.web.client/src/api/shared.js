
const FetchManaged = (pUrl, pInit, pTypeResponse = "json", pSuccess = null, pError = null) => {
    fetch(pUrl, pInit)
        .then(r => r[pTypeResponse]())
        .then(data => {
            if (typeof pSuccess == "function") pSuccess(data);
        })
        .catch((data) => {
            if (typeof pError == "function") pError(data);
        });
}

const Shared = {
    FetchManaged
};

export default Shared;