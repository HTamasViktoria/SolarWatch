import React, { useState } from "react";
import Navbar from "../../components/Navbar.jsx";
import './HomePage.css';

function HomePage() {
    const [text, setText] = useState("");

    const blablaHandler = async () => {
        try {
            const response = await fetch('/api/SolarWatch/bla');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setText(data);
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

    return (
        <>
            <Navbar/>

            <h1>{text}</h1>
        </>
    );
}

export default HomePage;