let wrappedPromise = (promise) => {
    let myPromise = new Promise((resolve, reject) => {
        promise.then((data) => {
            var response = { value: data, statusCode: 200, statusMessage: success };
            resolve(response);
        }).catch((e) => {
            var response = { value: null, statusCode: 404, statusMessage: e.message };
            resolve(response);
        })

    });
    return myPromise;
}

module.exports = wrappedPromise