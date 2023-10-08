# WebSocketApp

Using ASP.NET, MVC Project is required and a websocket is configured on local machine. 
Two modules are created, WebSocketHandler and a WebSocketAccessToken. Handler handles the connection processes and the access token is just a model for accessibility. Both services are added with dependency injection.

On frontend, using javascript, a text is sent and the response is received, which is basically the same response. On further applications, this could be done by json serialization, especially if multiple series of data is required (both user name and the user text needs to be sent, similar to how it works in chat applications).

Similarly, a console app within the same project is also able to reach the websocket and basically write the data to the console after going through the websocket.
