import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Navbar from "../../components/Navbar.jsx";

function Login() {
    const navigate = useNavigate();
    const goBackHandler = () => {
        navigate("/");
    };

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");


    const submitHandler = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/Auth/Login', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            });

            const data = await response.json();

            if (response.ok) {
                console.log("User login successful");
                localStorage.setItem('token', data.token);
                setUsername(data.userName);
                setLoggedIn(true);
            } else {
                console.log("Error during login.");
            }
        } catch (error) {
            console.error(error.message);
        }
    };

    return (
        <><Navbar/>
            {loggedIn ? (<div className="success-message">You are now logged in, {username}</div>) : (
                <div className="main-content">
                    <form onSubmit={submitHandler}>
                        <h1>login</h1>
                        <label htmlFor="email">Email:</label><br/>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        /><br/>
                        <label htmlFor="password">Password:</label><br/>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        /><br/>
                        <button type="submit">Submit</button>
                        <button type="button" className="back-button" onClick={goBackHandler}>Back</button>
                    </form>
                </div>
            )}


        </>
    );
}

export default Login;