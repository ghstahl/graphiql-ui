# json-graphql-server endpoints 
 
You can use the cool [json-graph-server](https://github.com/marmelab/json-graphql-server) project to have a real-fake graphql server. 

I have a checked in fake located [here](./json-graphql-server)  

## Endpoint  
```
http://localhost:3000/
```
## Query 
```
query q($id: ID!) {
  Post(id:$id){
    title
    views
    user_id
    User {
      id
    }
    Comments {
      id
      post_id
      body
      date
      Post {
        id
        title
        views
      }     
    }
  }
}
```
## Variables 
```
{"id": 1}
```
## Produces 
```
{
  "data": {
    "Post": {
      "title": "Lorem Ipsum",
      "views": 254,
      "user_id": "123",
      "User": {
        "id": "123"
      },
      "Comments": [
        {
          "id": "987",
          "post_id": "1",
          "body": "Consectetur adipiscing elit",
          "date": "2017-07-03T00:00:00.000Z",
          "Post": {
            "id": "1",
            "title": "Lorem Ipsum",
            "views": 254
          }
        },
        {
          "id": "995",
          "post_id": "1",
          "body": "Nam molestie pellentesque dui",
          "date": "2017-08-17T00:00:00.000Z",
          "Post": {
            "id": "1",
            "title": "Lorem Ipsum",
            "views": 254
          }
        }
      ]
    }
  }
}
```
