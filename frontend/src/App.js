import { useState } from "react";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import Lobby from "./components/Lobby";
import Chat from "./components/Chat";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";

const App = () => {
  const [connection, setConnection] = useState();
  const [messages, setMessages] = useState([]);
  const [token, setToken] = useState();

  const connectSignalR = async (email, room) => {
    try {
      const connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5187/chat", {
          accessTokenFactory: () => token,
        })
        .configureLogging(LogLevel.Information)
        .build();

      connection.on("ReceiveMessage", (email, message) => {
        setMessages((messages) => [...messages, { user: email, message }]);
      });

      connection.onclose((e) => {
        setConnection();
        setMessages([]);
      });

      await connection.start();
      await connection.invoke("JoinRoom", { user: email, room });
      setConnection(connection);
    } catch (e) {
      console.log(e);
    }
  };

  const joinRoom = async (email, password, room) => {
    try {
      const requestOptions = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email: email, password: password }),
      };
      fetch("http://localhost:5187/api/login", requestOptions)
        .then((response) => response.json())
        .then((data) => setToken(data.token));

      await connectSignalR(email, room);
    } catch (e) {
      console.log(e);
    }
  };

  const sendMessage = async (message) => {
    try {
      await connection.invoke("SendMessage", message);
    } catch (e) {
      console.log(e);
    }
  };

  const closeConnection = async () => {
    try {
      await connection.stop();
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <div className="app">
      <h2>FinancialChat</h2>
      {!connection ? (
        <Lobby joinRoom={joinRoom} />
      ) : (
        <Chat
          sendMessage={sendMessage}
          messages={messages}
          closeConnection={closeConnection}
        />
      )}
    </div>
  );
};

export default App;
