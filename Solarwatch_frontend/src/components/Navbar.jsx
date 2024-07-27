import React from "react";
import { useNavigate } from "react-router-dom";
import SolarData from "../pages/SolarData/SolarData.jsx";
import './Navbar.css';

function Navbar() {
    const navigate = useNavigate();

    const registrationClickHandler = () => {
        navigate("/registration");
    };

    const loginClickHandler = () => {
        navigate("/login");
    }

    const getSolarDataHandler = () =>{
        navigate("/getSolarData");
    }


    const logoutClickHandler = () =>{
        navigate("/logout")
    }


    return (
        <>
            <nav className="navbar">
                <a>Admin page</a>
                <a onClick={getSolarDataHandler}>Sunrise and sunset info</a>
                <a onClick={registrationClickHandler}>Registration</a>
                <a onClick={loginClickHandler}>Login</a>
                <a onClick={logoutClickHandler}>Logout</a>
            </nav>
        </>
    );
}

export default Navbar;