import {React, useState} from "react";
import { useNavigate } from "react-router-dom";
import "../../components/Navbar.css";
import Navbar from "../../components/Navbar.jsx";

function Logout() {

    const navigate = useNavigate();

    const [message, setMessage] = useState("");

    const logoutClickHandler = () =>{
        localStorage.removeItem('token');
        setMessage("You are now logged out");

    }

    const goBackHandler = () => {
        navigate("/");
    }

    return (
        <>{message != "" ? (<div>{message}
            <button onClick={goBackHandler}>Go back to homepage</button></div>) : (<> <Navbar/>
            <div>Are you sure you want to log out?</div>
            <button onClick={logoutClickHandler}>Yes</button>
            <button onClick={goBackHandler}>No</button></>)}

        </>
    );
}

export default Logout;