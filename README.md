# JsonDiff

JsonDiff is a WEB API that can show differences between two JSON encoded as base64 binary.

### Installation

* Clone/Download repository;
* Restore NuGet packages;
* Point connection string to your LocalDB (webconfig > defaultConnection);
* On Package Manager Console, type: **Update-Database -verbose**;
* Run application, Will start at Swagger page, you can test from here.

### Techs, Frameworks & Libraries

* WebAPI
* EntityFramework - CodeFirst
* Migrations
* MOQ
* NUnit
* LocalDB
* Swagger

### Unit test covarage

![N|Solid](http://i.imgur.com/RmZ7hpN.png)
* Removed repository from data report as all call are mocked using MOQ;
* Report extract using dotCover.


### Exposed endpoints:
  - < host >/v1/diff/{id}/left
  - < host >/v1/diff/{id}/right
  - < host >/v1/diff/{id}
  
* The property {id} is required and it will be the identifier to track the JSON.
* The JSON must be encoded in base64 binary string and wrapped using double quotes.

### Steps to perform a JSON diff:

* Json string encoded as base64 binary should be sent to endpoint sides (left/right) as body:
```sh
PUT /v1/diff/{id}/left
"eyJpZCI6IjUwIn0="
```
```sh
PUT /v1/diff/{id}/right
"eyJpZCI6IjEwIn0="
```
* After two side has ben sent to the endpoints with {id} tracked, you can call the endpoint <host>/v1/diff/{id} using the previous {id}
```sh
GET /v1/diff/{id}
```
* You will recieve results as:
```sh
{
    "id": "abc12345",
    "message": "Found 1 differences between jsons",
    "differences": [
        "value from id property changed from 50 to 10"
    ]
}
```

* As application already has Swagger installed, You can use **Swagger** as well:
![N|Solid](http://i.imgur.com/3gX2lzh.png)


License
----
MIT
