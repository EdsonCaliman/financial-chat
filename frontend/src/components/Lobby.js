import { useState } from "react";
import { Form, Button } from "react-bootstrap";

const Lobby = ({ joinRoom, registerAndJoinRoom }) => {
  const [email, setEmail] = useState();
  const [room, setRoom] = useState();
  const [password, setPassword] = useState();
  const [confirmPassword, setConfirmPassword] = useState();

  return (
    <>
      <Form
        className="lobby"
        onSubmit={(e) => {
          e.preventDefault();
          joinRoom(email, password, room);
        }}
      >
        <Form.Group>
          <Form.Label>Login to chat</Form.Label>
          <Form.Control
            placeholder="email"
            type="email"
            required
            onChange={(e) => setEmail(e.target.value)}
          />
          <Form.Control
            placeholder="password"
            type="password"
            required
            onChange={(e) => setPassword(e.target.value)}
          />
          <Form.Control
            placeholder="room"
            required
            onChange={(e) => setRoom(e.target.value)}
          />
        </Form.Group>
        <Button
          variant="success"
          type="submit"
          disabled={!email || !password || !room}
        >
          Join
        </Button>
      </Form>
      <Form
        className="lobby"
        onSubmit={(e) => {
          e.preventDefault();
          registerAndJoinRoom(email, password, confirmPassword, room);
        }}
      >
        <Form.Group>
          <Form.Label>Register to chat</Form.Label>
          <Form.Control
            placeholder="email"
            type="email"
            requireds
            onChange={(e) => setEmail(e.target.value)}
          />
          <Form.Control
            placeholder="password"
            type="password"
            required
            onChange={(e) => setPassword(e.target.value)}
          />
          <Form.Control
            placeholder="confirmPassword"
            type="password"
            required
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
          <Form.Control
            placeholder="room"
            required
            onChange={(e) => setRoom(e.target.value)}
          />
        </Form.Group>
        <Button
          variant="success"
          type="submit"
          disabled={!email || !password || !confirmPassword || !room}
        >
          Join
        </Button>
      </Form>
    </>
  );
};

export default Lobby;
