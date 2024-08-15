import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Navbar from "../../components/Navbar.jsx";

function SolarData() {
    const navigate = useNavigate();
    const [city, setCity] = useState("");
    const [date, setDate] = useState("");
    const [sunriseDate, setSunriseDate] = useState("");
    const [sunsetDate, setSunsetDate] = useState("");
    const [isShowingResult, setIsShowingResult] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const submitHandler = async (e) => {
        e.preventDefault();

        try {
            // Token lekérése localStorage-ból
            const token = localStorage.getItem('token');
            if (!token) {
                console.error("No token found. User is not logged in.");
                setErrorMessage("You need to log in to get solar data");
                return;
            }

            const response = await fetch(`/api/SolarWatch/${city}/${date}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                }
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error('Error:', errorData);
                console.log('Request failed.');
                return;
            }

            const data = await response.json();
            console.log('Success:', data);
            console.log('Data retrieved successfully!');
            setSunriseDate(data.sunriseDate);
            setSunsetDate(data.sunsetDate);
            setIsShowingResult(true);

        } catch (error) {
            console.log("Error while fetching data:", error);
        }
    }

    return (
        <><Navbar/>
            {isShowingResult ? (
                <div>
                    <p>city: {city}</p>
                    <p>date: {date}</p>
                    <p>sunriseDate: {sunriseDate}</p>
                    <p>sunsetDate: {sunsetDate}</p>
                </div>
            ) : (<>
                    <form onSubmit={submitHandler}>
                        <label htmlFor="city">City:</label><br/>
                        <input
                            type="text"
                            id="city"
                            name="city"
                            value={city}
                            onChange={(e) => setCity(e.target.value)}
                        /><br/>
                        <label htmlFor="date">Date:</label><br/>
                        <input
                            type="date"
                            id="date"
                            name="date"
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                        /><br/>
                        <button type="submit">Submit</button>
                    </form>
                    <h2>{errorMessage}</h2></>
            )}
        </>
    );
}

export default SolarData;