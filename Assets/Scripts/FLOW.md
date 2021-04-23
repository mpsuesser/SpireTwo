## Connection

On initial connection, a user's experience will go like this:

1. Preload scene executes and finishes.
2. StartMenu scene appears. When user hits `Start`, the connection request goes out to the server.
3. When the server receives the request, it will register the client's user ID and send a message to other players in the server.
	3a. If the server has room for another player, it will send a message back to the client informing them to pick a hero and respond with their choice.
	3b. If the server is full of players, the server will send a message back to the client informing them that they are a spectator.
4. The client receives this message, and if they were told they were a spectator, the Main scene will load.
5. If the client was told to pick a hero, the HeroSelection scene will show up and the server will wait for a response with the hero selection before spawning them.