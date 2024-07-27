import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage.jsx';
import Registration from "./pages/Registration/Registration.jsx";
import Login from "./pages/Login/Login.jsx";
import SolarData from "./pages/SolarData/SolarData.jsx";
import Logout from "./pages/Logout/Logout.jsx";

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/registration" element={<Registration />} />
                <Route path="/login" element={<Login />} />
                <Route path="/getSolarData" element={<SolarData />} />
                <Route path="/logout" element={<Logout />} />


            </Routes>
        </BrowserRouter>
    </React.StrictMode>
);