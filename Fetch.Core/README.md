# Fetch.Core  

This project is to prove that a single entry point into native can mimic the Fetch api;  
https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API

I have a Fetch.Core.dll, which is my main entry point and after that it is basically a router.  It mimics what a web server does.
For the Javascript folks, they are codeing to the same Fetch api.
1. Use fetch(,) to call the cloud,
2. Use localFetch(,) to call native 
```
localFetch('local://v1/graphQL/post', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
            'Accept':'application/json'
        },
        body: {
            operationName:'q',
            query: 'query q($offset: Int!,$count: Int!) {↵  installedApps(input:{offset:$offset,count:$count}){↵   currentOffset↵    nextOffset↵    count↵    installedApps{↵      displayName↵      unInstallPath↵    }↵  }↵}',
            variables:{
               offset:0,
               count:2
            }
        }
    }).then((result) => {
        document.getElementById("HelloThere").innerHTML = result.value;
        console.log(result);
    }).catch((e) => {
        console.log(e);
    });
```
where fetch is implemented [Here](./Fetch.Core/Local.cs)


[Nuclear Option Required](./Docs/Nuclear-Option.md)


