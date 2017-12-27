const electron = require('electron')
const path = require('path')
const url = require('url')
const baseNetAppPath = path.join(__dirname, '\\PublishOutput');

process.env.EDGE_USE_CORECLR = 1;
process.env.EDGE_APP_ROOT = baseNetAppPath;

var edge = require('electron-edge-js');
let wrappedPromise = require('./wrapped-promise')

module.exports = class LocalFetch {
    // ..and an (optional) custom class constructor. If one is
    // not supplied, a default constructor is used instead:
    // constructor() { }
    constructor() {
        let self = this;

        self.fetchFunc = edge.func({
            assemblyFile: path.join(baseNetAppPath, 'Fetch.Core.dll'),
            typeName: 'Fetch.Core.Local',
            methodName: 'Fetch'
        });

        self.localFetchPromise = (url, init) => {
            let myPromise = new Promise((resolve, reject) => {
                try {
                    let localInit = { url: url };
                    Object.assign(localInit, init);
                    this.fetchFunc(localInit, function(error, result) {
                        if (error) {
                            var response = { value: null, statusCode: 404, statusMessage: error.message };
                            resolve(response);
                        } else {
                            //  console.log(result);
                            resolve(result);
                        }
                    });
                } catch (e) {
                    reject(e.message);
                }
            });
            return myPromise;
        }
        self.localFetch = (url, init) => {
            return self.localFetchPromise(url, init);
        }
    }
}