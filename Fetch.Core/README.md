# Fetch.Core  

This project is to prove that a single entry point into native can mimic the Fetch api;  
https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API

I have a Fetch.Core.dll, which is my main entry point and after that it is basically a router.  It mimics what a web server does.
For the Javascript folks, they are codeing to the same Fetch api.
1. Use fetch(,) to call the cloud,
2. Use localFetch(,) to call native 
```
localFetch('local://v1/test/hello-there', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: {
            name: 'I am Vader'
        }
    }).then((result) => {
        document.getElementById("HelloThere").innerHTML = result.value;
        console.log(result);
    }).catch((e) => {
        console.log(e);
    });
```
where hello-there is implemented [Here](./src/LocalFetch/SimpleCommands/TestCommands.cs)




```
Visual Studio 2017
build electron-quick-start-core2\src\LocalFetch\LocalFetch.sln
```
[Nuclear Option Required](./Docs/Nuclear-Option.md)


```
# Go into the repository
cd electron-quick-start-core2\src
# Install dependencies
npm install
# Run the app
npm start
```
