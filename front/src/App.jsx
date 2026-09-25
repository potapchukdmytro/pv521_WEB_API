import { Route, Routes } from "react-router";
import "./App.css";
import Navbar from "./components/navbar/Navbar";
import BookDetails from "./pages/books/bookDetails/BookDetails";
import BooksCatalog from "./pages/books/booksCatalog/BooksCatalog";
import Login from "./pages/auth/login/Login";
import Profile from "./pages/profile/Profile";

function App() {
    return (
        <div>
            <Navbar />
            <Routes>
                <Route path="/" element={<BooksCatalog />} />
                <Route path="/book/:id" element={<BookDetails />} />
                <Route path="/login" element={<Login />} />
                <Route path="/profile" element={<Profile />} />
            </Routes>
        </div>
    );
}

export default App;
