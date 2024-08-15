import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import './Registration.css';
import Navbar from "../../components/Navbar.jsx";

function Registration() {
    const navigate = useNavigate();
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [registrationSuccessful, setRegistrationSuccessful] = useState(false);
    const [registeredUser, setRegisteredUser] = useState({
        email: '',
        username: ''
    });

    const goBackHandler = () => {
        navigate("/");
    }

    const submitHandler = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/Auth/Register', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email,
                    username: username,
                    password: password
                })
            });
            const data = await response.json();
            if (response.ok) {
                console.log("User registration successful");

                setRegistrationSuccessful(true);
                setRegisteredUser(prev => ({
                    ...prev,
                    email: data.email,
                    username: data.userName
                }));

            } else {
                console.log("Error during registration.");
            }
        } catch (error) {
            console.error(error.message);
        }
    }

    return (<><Navbar/>
        <div className="main-content">
            {registrationSuccessful ? (
                <div className="success-message">
                    Registration successful.
                    <p>Username: {registeredUser.username}</p>
                    <p>Email: {registeredUser.email}</p>
                </div>
            ) : (
                <form onSubmit={submitHandler}>
                    <h1>Registration</h1>
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <label htmlFor="email">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <button type="submit">Submit</button>
                    <button type="button" className="back-button" onClick={goBackHandler}>Back</button>
                </form>
            )}
        </div></>
    );
}

export default Registration;