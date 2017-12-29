# Local Fetch
This project has been modified, using electron-edge-js, to load the fetch.core.dll.  This dll implements a native single entry point to mimic the browser fetch api.  The fetch.core.dll exposes REST type apis, and one of those is the graphql single entry api.

## Local Endpoint
```
local://v1/graphQL/post
```
## Test Query
## Query 
```
query q($id: String!, $treatment: String!, $culture: String!) {
  echo(input: {id: $id, treatment: $treatment, culture: $culture}) {
    id
    treatment
    culture
  }
}
```
## Variables 
```
{"id": "P7.Main.Resources.Main,P7.Main","treatment":"kva","culture":"fr-FR"}
```
## Produces 
```
{
  "data": {
    "echo": {
      "id": "P7.Main.Resources.Main,P7.Main",
      "treatment": "kva",
      "culture": "fr-FR"
    }
  }
}
```
## Program Query  
## Query 
```
query q($displayName: String!) {
  program(input:{displayName:$displayName}){
    isInstalled
    displayName
  }
}
```
## Variables 
```
{"displayName": "Norton Internet Security"}
```
## Produces 
```
{
  "data": {
    "program": {
      "isInstalled": true,
      "displayName": "Norton Internet Security"
    }
  }
}
```
